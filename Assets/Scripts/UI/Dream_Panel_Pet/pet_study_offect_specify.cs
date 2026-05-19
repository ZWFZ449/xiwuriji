using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class pet_study_offect_specify : Base_Mono
{
    private Transform m_Talent_brom;

    db_pet_vo crt_pet;

    private Button offect_close;

    private Button select_talent_confirm;

    private pet_talent_item p_talent_item_prefab;

    private TMP_Dropdown dropdown;

    db_pet_talent_vo talent;
    private void Awake()
    {
        m_Talent_brom = Find<Transform>("offect/talent_list/Viewport/Content");
        offect_close = Find<Button>("offect/close_button");
        offect_close.onClick.AddListener(() => { hide(); });
        select_talent_confirm = Find<Button>("offect/select_talent_confirm");
        p_talent_item_prefab = Tool_UI.Find_Prefabs<pet_talent_item>("pet_talent_item");
        select_talent_confirm.onClick.AddListener(() => { Select_Talent_Confirm(); });
        dropdown = Find<TMP_Dropdown>("offect/Dropdown");
        dropdown.onValueChanged.AddListener(Dropdown_Change);
    }
    /// <summary>
    /// 选中天赋
    /// </summary>
    /// <param name="index"></param>
    private void Dropdown_Change(int index)
    {
        string cleanText = Regex.Replace(dropdown.options[index].text, "<[^>]*>", "");
        talent = ArrayHelper.Find(SumSave.db_pet_talents, e => e.pet_talent_name == cleanText);
    }

    private void hide()
    {
        gameObject.SetActive(false);
    }

    public void Init(db_pet_vo pet)
    {
        crt_pet= pet;
        talent = null;
        BaseInfo();
        InitDropdown();
    }
    private void InitDropdown()
    {
        dropdown.ClearOptions();
        List<string> list = new List<string>();
        db_vip crt_vip = Tool_Battle.Obtain_Vip();
        List<string> vip_pet_talent_level = new List<string>();
        if (crt_vip != null)
        {
            for (int i = 0; i < SumSave.db_pets.Count; i++)
            {
                if (crt_vip.vip_lv >= SumSave.db_pets[i].pet_id)
                { 
                    vip_pet_talent_level.Add(SumSave.db_pets[i].pet_talent);
                }
            }
        }
        List<db_pet_talent_vo> CrtTalent = crt_pet.GetCrtTalent;
        for (int i = 0; i < SumSave.db_pet_talents.Count; i++)
        {
            bool exist = true;
            for (int j= 0; j < CrtTalent.Count; j++)  
            {
                if (CrtTalent[j].pet_talent_name == SumSave.db_pet_talents[i].pet_talent_name)
                {
                    exist = false;
                    break;
                }
            }
            if (exist)
            {
                if (SumSave.db_pet_talents[i].pet_talent_level == 3)//金色技能
                {
                    for (int j = 0; j < vip_pet_talent_level.Count; j++)
                    {
                        if (vip_pet_talent_level[j] == SumSave.db_pet_talents[i].pet_talent_name)
                        {
                            list.Add(Show_Color.Yellow(SumSave.db_pet_talents[i].pet_talent_name));
                            break;
                        }
                    }
                }
                else
                {
                    list.Add(SumSave.db_pet_talents[i].pet_talent_level == 2 ? Show_Color.Purple(SumSave.db_pet_talents[i].pet_talent_name) : Show_Color.White(SumSave.db_pet_talents[i].pet_talent_name));
                }
            }
        }
        dropdown.AddOptions(list);
    }

    private void BaseInfo()
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
    /// <summary>
    /// 显示天赋效果
    /// </summary>
    /// <param name="item"></param>
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


    private void Select_Talent_Confirm()
    {
        if (talent == null) 
        { 
            Alert_Dec.Show("请选择一个天赋");
            return;
        }
        SendNotification(NotiList.Read_Mysql_Base_Time);
        if (SumSave.openMysql)
        {
            Alert_Dec.Show("网络连接失败");
            return;
        }
        Clear_Condition();
        Need_Condition(currency_unit.元宝, 9000);
        if (Return_Condition())
        {
            List<db_pet_talent_vo> CrtTalent = crt_pet.GetCrtTalent;
            int index = 1;
            if (crt_pet.pet_id >= 8) index = 2;
            if (crt_pet.pet_id >= 10) index = 3;
            if (talent.pet_talent_level == 3)
            {
                CrtTalent[Random.Range(0, index)] = talent; 
            }
            else 
            CrtTalent[Random.Range(index, CrtTalent.Count)] = talent;
            Alert_Dec.Show("获得天赋" + talent.pet_talent_name);
            SumSave.crt_pet.MysqlData();
            Game_Omphalos.i.archive();
            Init(crt_pet);
            transform.parent.SendMessage("update_specify");
            SendNotification(NotiList.Refresh_Max_Hero_Attribute);
        }
        else Alert_Dec.Show("条件不足");
    }
}
