using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class pet_Demon : Base_Mono
{
    private Transform m_Talent_brom, m_BagItem_brom;

    private pet_talent_item p_talent_item_prefab;

    private dream_BagItem p_bagItem_prefab;

    private Button close;
    db_pet_vo crt_pet,select_pet;
    private Image offect;
    private Button offect_close;
    private Transform moffect_Talent_brom, moffect_auxiliaryTalent_brom;
    private Button select_talent_confirm;
    private void Awake()
    {
        m_Talent_brom = Find<Transform>("talent_list/Viewport/Content");
        p_talent_item_prefab = Tool_UI.Find_Prefabs<pet_talent_item>("pet_talent_item");
        m_BagItem_brom = Find<Transform>("Scroll View/Viewport/Content");
        p_bagItem_prefab = Tool_UI.Find_Prefabs<dream_BagItem>("dream_BagItem");
        close = Find<Button>("close_button");
        close.onClick.AddListener(() => { gameObject.SetActive(false); transform.parent.gameObject.SetActive(false); });
        offect= Find<Image>("offect");
        offect_close = Find<Button>("offect/close_button");
        offect_close.onClick.AddListener(() => { offect.gameObject.SetActive(false); });
        moffect_Talent_brom = Find<Transform>("offect/talent_list/Viewport/Content");
        moffect_auxiliaryTalent_brom= Find<Transform>("offect/talent_auxiliary_list/Viewport/Content");
        select_talent_confirm= Find<Button>("offect/select_talent_confirm");
        select_talent_confirm.onClick.AddListener( ()=> { Select_Talent_Confirm(); });
    }

    

    private void OnEnable()
    {
        //Base_Show();
    }
    private void Base_Show()
    {
        Show_Talent(crt_pet, m_Talent_brom);
        offect.gameObject.SetActive(false);
        //ClearObject(m_Talent_brom);
        //List<db_pet_talent_vo> CrtTalent = crt_pet.GetCrtTalent;
        //for (int i = 0; i < CrtTalent.Count; i++)
        //{
        //    pet_talent_item item = Instantiate(p_talent_item_prefab, m_Talent_brom);
        //    item.Init(CrtTalent[i]);
        //    item.GetComponent<Button>().onClick.AddListener(() => { show_Talent(item); });
        //}
    }
    /// <summary>
    /// 显示天赋
    /// </summary>
    /// <param name="talent"></param>
    /// <param name="brom"></param>
    private void Show_Talent(db_pet_vo talent,Transform brom)
    {
        ClearObject(brom);
        List<db_pet_talent_vo> CrtTalent = talent.GetCrtTalent;
        for (int i = 0; i < CrtTalent.Count; i++)
        {
            pet_talent_item item = Instantiate(p_talent_item_prefab, brom);
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
        Show_BagItem();
    }

    private void Show_BagItem()
    {
        ClearObject(m_BagItem_brom);
        List<db_pet_vo> pet_list = SumSave.crt_pet.GetPets;
        for (int i = 0; i < pet_list.Count; i++)
        {
            if (pet_list[i].crt_name == pet_list[i].pet_name)
            {
                dream_BagItem bagItem = Instantiate(p_bagItem_prefab, m_BagItem_brom);
                bagItem.Pet_Data = pet_list[i];
                bagItem.GetComponent<Button>().onClick.AddListener(() => SelectPetItem(bagItem));
            }
        }
    }
    /// <summary>
    /// 选择宠物
    /// </summary>
    /// <param name="bagItem"></param>
    private void SelectPetItem(dream_BagItem bagItem)
    {
        select_pet = bagItem.Pet_Data;
        offect.gameObject.SetActive(true);
        Show_Talent(crt_pet, moffect_Talent_brom);
        Show_Talent(select_pet, moffect_auxiliaryTalent_brom);
        //string dec = "请确认是否选中的灵宠?";
        //int number= pet.GetCrtTalent.Count+crt_pet.GetCrtTalent.Count;
        //dec += "\n炼妖可重置天赋数量 " + Show_Color.Red(number / 2) + "---" + Show_Color.Red((number) / 2 + 2);
        //Alert.Show("炼妖", dec, devour_Pet, bagItem);
    }
    private void Select_Talent_Confirm()
    {
        if (crt_pet.GetCrtTalent.Count >= 20) { Alert_Dec.Show("天赋位已满"); return; }
        if (select_pet == null) return;
        SumSave.crt_pet.GetPets.Remove(select_pet);
        int number = select_pet.GetCrtTalent.Count + crt_pet.GetCrtTalent.Count;
        int crt_number = Random.Range(number / 2, (number) / 2 + 2);
        if (crt_number >= crt_pet.GetCrtTalent.Count)
        {
            crt_number -= crt_pet.GetCrtTalent.Count;
            for (int i = 0; i < crt_number; i++)
            { 
                db_pet_talent_vo talent = Obtain_Talent(crt_pet.GetCrtTalent);
                if (talent != null)
                {
                    Alert_Dec.Show("获得天赋 " + Show_Color.Red(talent.pet_talent_name));
                    crt_pet.GetCrtTalent.Add(talent);
                }
            }
            if (crt_number == 0) Alert_Dec.Show("似乎什么都没有发生");
        }
        else
        {
            for (int i = crt_number; i < crt_pet.GetCrtTalent.Count; i++)
            {
                Alert_Dec.Show("失去天赋 " + Show_Color.Red(crt_pet.GetCrtTalent[i].pet_talent_name));
                crt_pet.GetCrtTalent.RemoveAt(i);
                i--;
            }
        }
        transform.parent.parent.SendMessage("update_pet");
        Base_Show();
        Show_BagItem();
        SendNotification(NotiList.Refresh_Max_Hero_Attribute);
        SumSave.crt_pet.MysqlData();
    }
    /// <summary>
    /// 吞噬宠物
    /// </summary>
    /// <param name="arg0"></param>
    private void devour_Pet(object arg0)
    {
        if (crt_pet.GetCrtTalent.Count >= 20) { Alert_Dec.Show("天赋位已满"); return; }
        db_pet_vo pet = (arg0 as dream_BagItem).Pet_Data;
        if (pet == null) return;
        SumSave.crt_pet.GetPets.Remove(pet);
        int number = pet.GetCrtTalent.Count + crt_pet.GetCrtTalent.Count;
        int crt_number = Random.Range(number / 2, (number) / 2 + 2);
        if (crt_number >= crt_pet.GetCrtTalent.Count)
        {
            crt_number -= crt_pet.GetCrtTalent.Count;
            for (int i = 0; i < crt_number; i++)
            {
                db_pet_talent_vo talent = Obtain_Talent(crt_pet.GetCrtTalent);
                if (talent != null)
                {
                    Alert_Dec.Show("获得天赋 " + Show_Color.Red(talent.pet_talent_name));
                    crt_pet.GetCrtTalent.Add(talent);
                } 
            }
            if (crt_number == 0) Alert_Dec.Show("似乎什么都没有发生"); 
        }
        else
        {
            for (int i = crt_number; i < crt_pet.GetCrtTalent.Count; i++)
            {
                Alert_Dec.Show("失去天赋 " + Show_Color.Red(crt_pet.GetCrtTalent[i].pet_talent_name));
                crt_pet.GetCrtTalent.RemoveAt(i);
                i--;
            }
        }
        transform.parent.parent.SendMessage("update_pet");
        Base_Show();
        Show_BagItem();
        SendNotification(NotiList.Refresh_Max_Hero_Attribute);
        SumSave.crt_pet.MysqlData();
    }

    private db_pet_talent_vo Obtain_Talent(List<db_pet_talent_vo> CrtTalent)
    {
        List<db_pet_talent_vo> list = ArrayHelper.FindAll(SumSave.db_pet_talents, e => e.pet_talent_level <= 2);

        db_pet_talent_vo talent = list[Random.Range(0, list.Count)];
        int number = 0;
        while (CrtTalent.Contains(talent) && number < 1000)
        {
            talent = list[Random.Range(0, list.Count)];
            number++;
        }
        if (number >= 1000) talent = null;
        return talent;
    }
}
