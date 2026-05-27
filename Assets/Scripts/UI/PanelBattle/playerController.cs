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
        if (oneselfHealthState.Get_MP >= skill.Get_Mp)
        {
            int mp = skill.Get_Mp;
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

            oneselfHealthState.Set_Mp = mp;
            StartCoroutine(On_Attack(skill));
            skill_index++;
            return true;
        }
        else Alert_Dec.Show("MP不足,释放" + skill.show_name + "失败");
        return false;
    }
    /// <summary>
    /// 判断技能效果
    /// </summary>
    /// <param name="target"></param>
    /// <param name="skill"></param>
    private IEnumerator On_Attack(db_skill_vo skill)
    {
        int number = 1;
#if UNITY_EDITOR
        number = 10;
#elif UNITY_ANDROID
#elif UNITY_IPHONE
#endif
        foreach (var item in skill.GetBuff.Keys)
        {
            switch (item)
            {
                case enum_talent_offect_list.生命:
                    break;
                case enum_talent_offect_list.攻击:
                    break;
                case enum_talent_offect_list.魔法:
                    break;
                case enum_talent_offect_list.道术:
                    break;
                case enum_talent_offect_list.防御:
                    break;
                case enum_talent_offect_list.攻击速度:
                    break;
                case enum_talent_offect_list.物理攻击:
                    break;
                case enum_talent_offect_list.魔法攻击:
                    break;
                case enum_talent_offect_list.道术攻击:
                    break;
                case enum_talent_offect_list.防御值:
                    break;
                case enum_talent_offect_list.躲避:
                    break;
                case enum_talent_offect_list.命中:
                    break;
                case enum_talent_offect_list.技能:
                    break;
                case enum_talent_offect_list.附加攻击:
                    break;
                case enum_talent_offect_list.附加魔法:
                    break;
                case enum_talent_offect_list.附加道术:
                    break;
                case enum_talent_offect_list.附加双防:
                    break;
                case enum_talent_offect_list.附加伤害:
                    break;
                case enum_talent_offect_list.附加回血:
                    break;
                case enum_talent_offect_list.附加攻击范围:
                    break;
                case enum_talent_offect_list.无视防御:
                    break;
                case enum_talent_offect_list.召唤兽:
                    break;
                case enum_talent_offect_list.召唤兽攻击:
                    break;
                case enum_talent_offect_list.召唤兽生命:
                    break;
                case enum_talent_offect_list.召唤兽防御:
                    break;
                case enum_talent_offect_list.召唤兽速度:
                    break;
                case enum_talent_offect_list.召唤兽死亡爆炸:
                    break;
                case enum_talent_offect_list.特殊效果:
                    break;
                case enum_talent_offect_list.临时伤害:
                    break;
                case enum_talent_offect_list.临时防御:
                    break;
                case enum_talent_offect_list.临时速度:
                    break;
                case enum_talent_offect_list.单体改群体:
                    break;
                case enum_talent_offect_list.技能攻击个数:
                    break;
                case enum_talent_offect_list.技能概率不消耗蓝:
                    break;
                case enum_talent_offect_list.技能全体伤害:
                    break;
                case enum_talent_offect_list.群体技能攻击范围:
                    break;
                case enum_talent_offect_list.每秒回复全体血量百分比:
                    break;
                case enum_talent_offect_list.攻击击退敌人概率:
                    break;
                case enum_talent_offect_list.弹道:
                    number += skill.GetBuff[item];
                    break;
                default:
                    break;
            }
        }
        if (number == 1)
            On_Skill(skill);
        else
        {
            while (number > 0)
            {
                number--;
                On_Skill(skill);
                int mp = skill.spells[skill.SetLv()];
                if (oneselfHealthState.Get_MP >= mp)
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
                    oneselfHealthState.Set_Mp = mp;
                }
                else number = 0;//蓝量不足
                yield return new WaitForSeconds(0.1F);
            }
        }

    }
    /// <summary>
    /// 多次释放
    /// </summary>
    /// <param name="base_name"></param>
    /// <returns></returns>

    private void On_Skill(db_skill_vo skill)
    {
        if (Terget == null) return;
        if (!Terget.gameObject.activeInHierarchy || Terget.isDead) return; 
        if (!dic.ContainsKey(skill))
        {
            GameObject skill_prefabs = Resources.Load<GameObject>("UI/skill_prefabs/skill_" + skill.id);// skill.id); 
            //技能放大效果 未完成
            //float rand = Random.Range(1, 2);
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
