using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class pet_inheritance : Base_Mono
{
    private Transform m_info_brom, m_BagItem_brom;

    private info_item p_info_item_prefab;

    private dream_BagItem p_bagItem_prefab;


    private List<enum_equip_entry_list> m_list = new List<enum_equip_entry_list>();

    private Dictionary<enum_equip_entry_list, info_item> info_Dic = new Dictionary<enum_equip_entry_list, info_item>();

    private Button close;
    db_pet_vo crt_pet;
    private void Awake()
    {
        m_BagItem_brom = Find<Transform>("Scroll View/Viewport/Content");
        p_bagItem_prefab = Tool_UI.Find_Prefabs<dream_BagItem>("dream_BagItem");
        m_info_brom = Find<Transform>("show_list/Viewport/Content");
        p_info_item_prefab = Tool_UI.Find_Prefabs<info_item>("info_item");
        close = Find<Button>("close_button");
        close.onClick.AddListener(() => { gameObject.SetActive(false); transform.parent.gameObject.SetActive(false); });
        Init();
    }

    private void Init()
    {
        m_list.Add(enum_equip_entry_list.物理防御);
        m_list.Add(enum_equip_entry_list.魔法防御);
        m_list.Add(enum_equip_entry_list.物理攻击);
        m_list.Add(enum_equip_entry_list.魔法攻击);
        m_list.Add(enum_equip_entry_list.道术攻击);
        for (int i = 0; i < m_list.Count; i++)
        {
            info_item item = Instantiate(p_info_item_prefab, m_info_brom);
            info_Dic.Add(m_list[i], item);
        }
    }

    private void OnEnable()
    {
        //Base_Show();
    }
    private void Base_Show()
    {
        (int, int, int, int, int) CrtAttr = crt_pet.GetCrtAttr;
        (int, int, int, int, int) AddAttr = crt_pet.GetAddAttr;
        List<db_pet_talent_vo> CrtTalent = crt_pet.GetCrtTalent;
        Bag_Base_VO bag = ArrayHelper.Find(SumSave.db_stditems, e => e.Name == crt_pet.pet_name);
        foreach (enum_equip_entry_list item in info_Dic.Keys)
        {
            switch (item)
            {
                case enum_equip_entry_list.物理防御:
                    info_Dic[item].SetPetInfo(item, 1 + " - " + (bag.ac2 + CrtAttr.Item1) + Show_Color.Red(" +( " + State_Value(item,AddAttr.Item1) +")")); break;
                case enum_equip_entry_list.魔法防御:
                    info_Dic[item].SetPetInfo(item, 1 + " - " + (bag.mac2 + CrtAttr.Item2) + Show_Color.Red(" +( " + State_Value(item, AddAttr.Item2) + ")")); break;
                case enum_equip_entry_list.物理攻击:
                    info_Dic[item].SetPetInfo(item, 1 + " - " + (bag.dc2 + CrtAttr.Item3) + Show_Color.Red(" +( " + State_Value(item, AddAttr.Item3) + ")")); break;
                case enum_equip_entry_list.魔法攻击:
                    info_Dic[item].SetPetInfo(item, 1 + " - " + (bag.mc2 + CrtAttr.Item4) + Show_Color.Red(" +( " + State_Value(item, AddAttr.Item4) + ")")); break;
                case enum_equip_entry_list.道术攻击:
                    info_Dic[item].SetPetInfo(item, 1 + " - " + (bag.sc2 + CrtAttr.Item5) + Show_Color.Red(" +( " + State_Value(item, AddAttr.Item5) + ")")); break;
            }
        }

    }

    private string State_Value(enum_equip_entry_list item,int value)
    {
        string str = value + "";
        switch (item)
        {
            case enum_equip_entry_list.物理防御:
                if (value >= crt_pet.pet_ac) str += "Max"; break;
            case enum_equip_entry_list.魔法防御:
                if (value >= crt_pet.pet_mac) str += "Max"; break;
            case enum_equip_entry_list.物理攻击:
                if (value >= crt_pet.pet_dc) str += "Max"; break;
            case enum_equip_entry_list.魔法攻击:
                if (value >= crt_pet.pet_mc) str += "Max"; break;
            case enum_equip_entry_list.道术攻击:
                if (value >= crt_pet.pet_sc) str += "Max"; break;
        }
        return str;
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
        Alert.Show("继承", "请确认是否将当前灵宠属性完全继承给选中的宠物?\n继承需消耗5000元宝", devour_Pet, bagItem);
    }
    /// <summary>
    /// 吞噬宠物
    /// </summary>
    /// <param name="arg0"></param>
    private void devour_Pet(object arg0)
    {
        db_pet_vo pet = (arg0 as dream_BagItem).Pet_Data;
        if (pet == null) return;
        Clear_Condition();
        Need_Condition(currency_unit.元宝, 5000);
        if (Return_Condition())
        {
            pet.SetAddAttr(crt_pet.GetAddAttr);
            List<db_pet_talent_vo> CrtTalent = pet.GetCrtTalent;
            List<db_pet_talent_vo> CrtTalents = crt_pet.GetCrtTalent;
            int crt_index = 1;//要继承灵宠的等级
            if (pet.pet_id >= 8) crt_index = 2;
            if (pet.pet_id >= 10) crt_index = 3;
            int index = 1;//当前灵宠的等级
            if (crt_pet.pet_id >= 8) index = 2;
            if (crt_pet.pet_id >= 10) index = 3;
            for (int i = crt_index; i < CrtTalent.Count; i++)
            {
                CrtTalent.RemoveAt(i);
                i--;
            }
            crt_pet.SetAddAttr((0, 0, 0, 0, 0));
            for (int i = index; i < CrtTalents.Count; i++)
            {
                CrtTalent.Add(CrtTalents[i]);
                CrtTalents.RemoveAt(i);
                i--;
            }
            transform.parent.parent.SendMessage("update_pet");
            SumSave.crt_pet.MysqlData();
            Game_Omphalos.i.archive();
            Base_Show();
            Show_BagItem();
            Alert_Dec.Show("继承成功");
        }
        else { Alert_Dec.Show(currency_unit.元宝 + "不足"); }

      
    }
}
