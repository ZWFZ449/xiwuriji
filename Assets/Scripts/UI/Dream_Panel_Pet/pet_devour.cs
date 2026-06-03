using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class pet_devour : Base_Mono
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
        int max = (SumSave.crtHero.zs_lv - 1) * 10;
        switch (item)
        {
            case enum_equip_entry_list.物理防御:
                if (value >= crt_pet.pet_ac+ max) str += "Max"; break;
            case enum_equip_entry_list.魔法防御:
                if (value >= crt_pet.pet_mac + max) str += "Max"; break;
            case enum_equip_entry_list.物理攻击:
                if (value >= crt_pet.pet_dc + max) str += "Max"; break;
            case enum_equip_entry_list.魔法攻击:
                if (value >= crt_pet.pet_mc + max) str += "Max"; break;
            case enum_equip_entry_list.道术攻击:
                if (value >= crt_pet.pet_sc + max) str += "Max"; break;
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
        Alert.Show("吞噬", "请确认是否吞噬选中的灵宠?", devour_Pet, bagItem);
    }
    /// <summary>
    /// 吞噬宠物
    /// </summary>
    /// <param name="arg0"></param>
    private void devour_Pet(object arg0)
    {
        db_pet_vo pet = (arg0 as dream_BagItem).Pet_Data;
        if (pet == null) return;
        SendNotification(NotiList.Read_Mysql_Base_Time);
        int max = (SumSave.crtHero.zs_lv - 1) * 10;
        if (SumSave.openMysql)
        { 
            Alert_Dec.Show("网络连接失败");
            return;
        }
        SumSave.crt_pet.GetPets.Remove(pet);
        int num = 0;
        if (Random.Range(0, 100) < 50)
        {
            num++;
            if (Random.Range(0, 100) < 30)
            {
                num++;
            }
        }
        if (num > 0)
        {
            (int, int, int, int, int) AddAttr = crt_pet.GetAddAttr;
            for (int i = 0; i < num; i++)
            {
                int random= Random.Range(0, 5);
                switch (random)
                { 
                    case 0: AddAttr.Item1++; AddAttr.Item1 = (int)MathF.Min(crt_pet.pet_ac + max, AddAttr.Item1);Alert_Dec.Show("物理防御+1"); break;
                    case 1: AddAttr.Item2++; AddAttr.Item2 = (int)MathF.Min(crt_pet.pet_mac + max, AddAttr.Item2); Alert_Dec.Show("魔法防御+1"); break;
                    case 2: AddAttr.Item3++; AddAttr.Item3 = (int)MathF.Min(crt_pet.pet_dc + max, AddAttr.Item3); Alert_Dec.Show("物理攻击+1"); break;
                    case 3: AddAttr.Item4++; AddAttr.Item4 = (int)MathF.Min(crt_pet.pet_mc + max, AddAttr.Item4); Alert_Dec.Show("魔法攻击+1"); break;
                    case 4: AddAttr.Item5++; AddAttr.Item5 = (int)MathF.Min(crt_pet.pet_sc + max, AddAttr.Item5);  Alert_Dec.Show("道术攻击+1"); break;
                }
            }
            crt_pet.SetAddAttr(AddAttr.Item1, AddAttr.Item2, AddAttr.Item3, AddAttr.Item4, AddAttr.Item5);
            transform.parent.parent.SendMessage("update_pet");
        }
        else Alert_Dec.Show("似乎什么都没有发生");
        Base_Show();
        Show_BagItem();
        SumSave.crt_pet.MysqlData();
    }
}
