using Common;
using Components;
using GifImporter;
using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerController : BaseBattleAttack 
{
    private int skill_index = 0;
    GameObject ArrowPrefabs;
    /// <summary>
    /// 存储技能预制体
    /// </summary>
    private Dictionary<db_skill_vo, GameObject> dic = new Dictionary<db_skill_vo, GameObject>();
    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnAuto()
    {
        base.OnAuto();
        if (Terget == null || !Terget.gameObject.activeSelf || Terget.isDead) Find_Terget();
        if (Terget == null) return;
        if (battle_skills != null && battle_skills.Count > 0)
        {
            if (skill_index >= battle_skills.Count) skill_index = 0;
            for (int i = skill_index; i < battle_skills.Count; i++)
            {
                if (Select_Skill(battle_skills[i], i)) return;//往后看技能释放
            }
            for (int i = 0; i < skill_index; i++)
            { 
                if (Select_Skill(battle_skills[i], i)) return;//往前看技能释放
            }
        }
        //平a
        BaseAttack();
    }
    /// <summary>
    /// 选择技能
    /// </summary>
    /// <param name="item"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    private bool Select_Skill(db_skill_vo skill,int index)
    {
        int mp = IsRelease(skill);
        if (mp == -1)
        {
            Alert_Dec.Show("MP不足,释放" + skill.show_name + "失败");
            return false;
        }
        StartCoroutine(On_Attack(skill, mp));
        skill_index++;
        return true;
        //if (oneselfHealthState.Get_MP >= skill.Get_Mp)
        //{
        //    foreach (var item in data.data.buffList)
        //    {
        //        switch (item.Item1)
        //        {
        //            case enum_battle_pet_talent_list.任意门:
        //                break;
        //            case enum_battle_pet_talent_list.嗜血追击:
        //                break;
        //            case enum_battle_pet_talent_list.慧根:
        //                mp = (int)(mp * (100 - item.Item3) / 100);
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    oneselfHealthState.Set_Mp = mp;
        //    StartCoroutine(On_Attack(skill));
        //    skill_index++;
        //    return true;
        //}
        //else Alert_Dec.Show("MP不足,释放" + skill.show_name + "失败");
        //return false;
    }

    private int IsRelease(db_skill_vo skill)
    {
        int mp = skill.Get_Mp;
        if (oneselfHealthState.Get_MP >= skill.Get_Mp)
        {
            foreach (var item in data.data.buffList)
            {
                switch (item.Item1)
                {
                    case enum_battle_pet_talent_list.任意门:
                        break;
                    case enum_battle_pet_talent_list.嗜血追击:
                        break;
                    case enum_battle_pet_talent_list.慧根:
                        mp = (int)(mp * (100 - item.Item3) / 100);
                        break;
                    default:
                        break;
                }
            }
            return mp;
        }
        else
        {
            if (SumSave.crt_setting.user_data_settings.Count >= 12 && SumSave.crt_setting.user_data_settings[11] == 1)//开启魔瓶
            {
                List<(int, int, long)> list = SumSave.crt_user_artifact.Get;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Item1 == 2)
                    {
                        int maxmp = oneselfHealthState.fill_Mp;
                        if (list[i].Item3 >= oneselfHealthState.fill_Mp)
                        {
                            oneselfHealthState.Use_Medicine(0, maxmp);
                            list[i] = (list[i].Item1, list[i].Item2, list[i].Item3 - maxmp);
                            SumSave.crt_user_artifact.MysqlData();
                            Alert_Dec.Show("魔瓶使用成功");
                            return mp;
                        }
                    }
                }

            }
            return -1;
        } 
    }
    /// <summary>
    /// 判断技能效果
    /// </summary>
    /// <param name="target"></param>
    /// <param name="skill"></param>
    private IEnumerator On_Attack(db_skill_vo skill,int mp)
    {
        int number = 1;
#if UNITY_EDITOR
        number = 30;
#elif UNITY_ANDROID
#elif UNITY_IPHONE
#endif
        foreach (var item in skill.GetBuff.Keys)
        {
            switch (item)
            {
                case enum_talent_offect_list.弹道:
                    number += skill.GetBuff[item];
                    break;
                default:
                    break;
            }
        }
        if (number == 1)
            On_Skill(skill, mp);
        else
        {
            while (number > 0)
            {
                number--;
                On_Skill(skill,mp);
                yield return new WaitForSeconds(0.1F);
            }
        }

    }
    /// <summary>
    /// 多次释放
    /// </summary>
    /// <param name="base_name"></param>
    /// <returns></returns>

    private void On_Skill(db_skill_vo skill,int mp)
    {
        if (oneselfHealthState.Get_MP < mp)
        {
            return;//蓝量不足
        } 
        oneselfHealthState.Set_Mp = mp;
        if (Terget == null) { Find_Terget(); return; }
        
        if (!Terget.gameObject.activeInHierarchy || Terget.isDead) { Find_Terget(); return; }
        if (!dic.ContainsKey(skill))
        {
            GameObject skill_prefabs = Resources.Load<GameObject>("UI/skill_prefabs/skill_" + skill.id);// skill.id); 
            //技能放大效果 未完成
            //float rand = 5;
            //skill_prefabs.transform.localScale = new Vector3(rand, rand, rand);
            dic.Add(skill, skill_prefabs);
        }
        ArrowPrefabs = dic[skill];
        GameObject go = ObjectPoolManager.instance.GetObjectFormPool(skill.show_name, ArrowPrefabs, new Vector3(transform.position.x, transform.position.y), Quaternion.identity, transform);
        if (go.GetComponent<Skill_Hit>() == null)
        {
            go.AddComponent<Skill_Hit>();
        }
        go.GetComponent<Skill_Hit>().SetTargetPosition(this, skill, Terget);

        if (skill.MoveType == 0)//剑气类技能
        {
            skill_damage(skill);  
        }
    }

}
