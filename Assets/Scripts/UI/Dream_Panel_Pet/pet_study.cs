using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class pet_study : Base_Mono
{
    private Transform m_btn_brom, m_Talent_brom;

    private pet_talent_item p_talent_item_prefab;

    private btn_item p_btn_item_prefab;

    private Button close;
    db_pet_vo crt_pet;
    private enum btn_type
    {
        兽诀,
        高级兽诀,
    }
    private void Awake()
    {
        m_Talent_brom = Find<Transform>("talent_list/Viewport/Content");
        p_talent_item_prefab = Tool_UI.Find_Prefabs<pet_talent_item>("pet_talent_item");
        m_btn_brom= Find<Transform>("btn_list");
        p_btn_item_prefab = Tool_UI.Find_Prefabs<btn_item>("btn_item");
        close = Find<Button>("close_button");
        close.onClick.AddListener(() => { gameObject.SetActive(false);transform.parent.gameObject.SetActive(false); });
        Init();
    }

    private void OnEnable()
    {
        //Base_Show();
    }
    private void Base_Show()
    {
        ClearObject(m_Talent_brom);
        List<db_pet_talent_vo> CrtTalent = crt_pet.GetCrtTalent;
        for (int i = 0; i < CrtTalent.Count; i++)
        {
            pet_talent_item item = Instantiate(p_talent_item_prefab, m_Talent_brom);
            item.Init(CrtTalent[i]);
            item.GetComponent<Button>().onClick.AddListener(() => { show_Talent(item); });
        }
    }
    private void show_Talent(pet_talent_item item)
    {
        db_pet_talent_vo talent = item.GetTalentValue;
        string dec = talent.pet_talent_name + "\n";
        /// <summary>
        /// 触发类型 
        /// 1作用自身
        /// 1.1物理伤害百分比
        /// 1.2魔法伤害百分比
        /// 1.3召唤兽伤害百分比
        /// 1.4物理防御
        /// 1.5魔法防御
        /// 1.6回复hp
        /// 1.7回复mp
        /// 1.8减少物理伤害%
        /// 1.9减少魔法伤害%
        /// 1.11增加躲避    
        /// 2战斗触发
        /// 2.1连击  
        /// 2.2忽视物理防御
        /// 2.3忽视魔法防御
        /// 2.4忽视召唤兽防御
        /// 2.5反镇
        /// 2.6防爆
        /// 2.7招架
        /// 2.8反击
        /// 2.10技能释放消耗减少

        /// 3特殊
        /// 1 增加生命上限
        /// 2 增加基础属性
        /// 3 概率随机传送一个敌人
        /// 4 击杀后追击另一个目标
        /// 5 攻击无视防御
        /// 6 攻击概率10倍
        /// 7 攻击概率斩杀
        /// 8 连击效果提升
        switch (talent.pet_talent_type)
        {

            case 3:
                switch ((talent.pet_talent_offect))
                {
                    case 1: dec += "生命上限 + " + Show_Color.Red(talent.pet_talent_offectvalue) + " %"; break;
                    case 2:
                        dec += "基础属性\n" + enum_equip_entry_list.物理攻击 + " +" + Show_Color.Red(talent.pet_talent_offectvalue * SumSave.crtHero.lv)
                        + "\n" + enum_equip_entry_list.魔法攻击 + " +" + Show_Color.Red(talent.pet_talent_offectvalue * SumSave.crtHero.lv)
                        + "\n" + enum_equip_entry_list.道术攻击 + " +" + Show_Color.Red(talent.pet_talent_offectvalue * SumSave.crtHero.lv)
                        ; break;
                    case 3: dec += "攻击目标时 " + Show_Color.Red(talent.pet_talent_offecttype + "%") + " 概率 触发 " + "随机传送一个敌人"; break;
                    case 4: dec += "击杀后追击另一个目标\n每次触发消耗最大Hp的" + Show_Color.Red("10%"); break;
                    case 5: dec += "攻击目标时 " + Show_Color.Red(talent.pet_talent_offecttype + "%") + " 概率 触发 " + Show_Color.Red("无视防御") + " 效果"; ; break;
                    case 6: dec += "攻击目标时 " + Show_Color.Red(talent.pet_talent_offecttype + "%") + " 概率 触发 " + Show_Color.Red(" 伤害 * " + talent.pet_talent_offectvalue) + " 效果"; break;
                    case 7: dec += "攻击目标时 当目标血量低于" + Show_Color.Red(talent.pet_talent_offecttype + "%") + " 时 触发 " + Show_Color.Red("斩杀") + " 效果"; break;
                    case 8: dec += "连击效果提升 " + Show_Color.Red(talent.pet_talent_offecttype + "%"); break;
                }
                break;
            case 1:
                switch ((talent.pet_talent_offect))
                {
                    case 1: dec += "物理伤害 + " + Show_Color.Red(talent.pet_talent_offectvalue) + " %"; break;
                    case 2: dec += "魔法伤害 + " + Show_Color.Red(talent.pet_talent_offectvalue) + " %"; break;
                    case 3: dec += "召唤兽伤害 + " + Show_Color.Red(talent.pet_talent_offectvalue) + " %"; break;
                    case 4: dec += "物理防御 + " + Show_Color.Red(talent.pet_talent_offectvalue * SumSave.crtHero.lv); break;
                    case 5: dec += "魔法防御 + " + Show_Color.Red(talent.pet_talent_offectvalue * SumSave.crtHero.lv); break;
                    case 6: dec += "每s回复 + " + Show_Color.Red(talent.pet_talent_offectvalue * SumSave.crtHero.lv) + " Hp"; break;
                    case 7: dec += "每s回复 + " + Show_Color.Red(talent.pet_talent_offectvalue * SumSave.crtHero.lv) + " Mp"; break;
                    case 8: dec += "受到物理伤害减少  " + Show_Color.Red(talent.pet_talent_offectvalue) + " %"; break;
                    case 9: dec += "受到魔法伤害减少  " + Show_Color.Red(talent.pet_talent_offectvalue) + " %"; break;
                    case 11: dec += "躲避 + " + Show_Color.Red(talent.pet_talent_offectvalue) + " "; break;

                    default:
                        break;
                }
                break;
            case 2:
                switch ((talent.pet_talent_offect))
                {
                    case 1:
                        dec += "攻击目标时 " + Show_Color.Red((Hero_Type)(talent.pet_talent_job)) + " 职业 "
                            //+ (talent.pet_talent_job == 3 ? "(召唤兽)" : "")
                            + Show_Color.Red(talent.pet_talent_offecttype + "%") + " 概率 触发 " + Show_Color.Red(talent.pet_talent_offectvalue + "%") + " 伤害"; break;
                    case 2:
                    case 3:
                    case 4:
                        dec += "攻击目标时 " + Show_Color.Red((Hero_Type)(talent.pet_talent_offect - 1)) + " 职业 "
                            //+ (talent.pet_talent_offect == 4 ? "(召唤兽)" : "")
                            + Show_Color.Red(talent.pet_talent_offecttype + "%") + " 概率 忽视 " + Show_Color.Red(talent.pet_talent_offectvalue * SumSave.crtHero.lv) + " 防御"; break;
                    case 5:
                        dec += "受到伤害时 " + Show_Color.Red(talent.pet_talent_offecttype + "%") + "概率 反震 " + Show_Color.Red(talent.pet_talent_offectvalue + "%") + " 伤害"; break;
                    case 6: dec += "受到攻击时 " + Show_Color.Red(talent.pet_talent_offecttype + "%") + "概率 降低 " + Show_Color.Red(talent.pet_talent_offectvalue + "%") + " 暴击概率"; break;
                    case 7: dec += "受到伤害时 " + Show_Color.Red(talent.pet_talent_offecttype + "%") + "概率 降低 " + Show_Color.Red(talent.pet_talent_offectvalue + "%") + " 伤害"; break;
                    case 8: dec += "受到攻击时 " + Show_Color.Red(talent.pet_talent_offecttype + "%") + "概率 反弹 " + Show_Color.Red(talent.pet_talent_offectvalue + "%") + " 伤害"; break;
                    case 10: dec += "技能释放消耗减少  " + Show_Color.Red(talent.pet_talent_offectvalue) + " %"; break;
                }
                break;
            default:
                break;
        }
        Alert.Show(talent.pet_talent_name, dec);
    }

    public void Init(db_pet_vo pet)
    {
        crt_pet = pet;
        Base_Show();
    }

    private void Init()
    {
        for (int i = 0; i < Enum.GetNames(typeof(btn_type)).Length; i++)
        {
            btn_item btn_item = Instantiate(p_btn_item_prefab, m_btn_brom);
            btn_item.Show(i, (btn_type)i);
            btn_item.GetComponent<Button>().onClick.AddListener(() => { OnClick_btn(btn_item); });
        }
    }
    /// <summary>
    /// 点击按钮
    /// </summary>
    /// <param name="btn_item"></param>
    private void OnClick_btn(btn_item btn_item)
    {
        SendNotification(NotiList.Read_Mysql_Base_Time);
        if (SumSave.openMysql)
        {
            Alert_Dec.Show("网络连接失败");
            return;
        }
        Need_Condition((btn_type)btn_item.index, 1);
        if (!Return_Condition()) { Alert_Dec.Show("物品不足");return; }
        
        int pet_talent_level = 1;
        switch ((btn_type)btn_item.index)
        {
            case btn_type.兽诀:
                pet_talent_level = 1;
                break;
            case btn_type.高级兽诀:
                pet_talent_level = 2;
                break;
            default:
                break;
        }
        
        List<db_pet_talent_vo> list_vo = ArrayHelper.FindAll(SumSave.db_pet_talents, e => e.pet_talent_level == pet_talent_level);
        List<db_pet_talent_vo> CrtTalent = crt_pet.GetCrtTalent;
        if (list_vo.Count > 0)
        {
            db_pet_talent_vo talent = Obtain_Talent(CrtTalent, list_vo);
            if (talent == null) { Alert_Dec.Show("似乎什么都没有发生"); return; } 
            if (m_Talent_brom.childCount < 5)//数量低于5个
            {
                if (Random.Range(0, 100) >= m_Talent_brom.childCount * 20)
                {
                    //新增
                    CrtTalent.Add(talent);
                }
                else
                {
                    //替换
                    CrtTalent[Random.Range(crt_pet.pet_id >= 8 ? 2 : 1, CrtTalent.Count)] = talent;
                }
            }
            else
                CrtTalent[Random.Range(crt_pet.pet_id >= 8 ? 2 : 1, CrtTalent.Count)] = talent;
            Alert_Dec.Show("获得天赋" + talent.pet_talent_name);
            SumSave.crt_pet.MysqlData();
            Game_Omphalos.i.archive();
            transform.parent.parent.SendMessage("update_pet");
            Base_Show();
        }
    }

    private db_pet_talent_vo Obtain_Talent(List<db_pet_talent_vo> CrtTalent, List<db_pet_talent_vo> list)
    {
        db_pet_talent_vo talent = list[Random.Range(0, list.Count)];
        int number = 0;
        while (CrtTalent.Contains(talent)&&number<1000)
        { 
            talent = list[Random.Range(0, list.Count)];
            number++;
        }
        if (number >= 1000) talent = null;
        return talent;
    }
}
