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
        string dec = Tool_Battle.show_Talent(item);
        Alert.Show(item.GetTalentValue.pet_talent_name, dec);
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
        SendNotification(NotiList.Read_Mysql_Base_Time);
        if (SumSave.openMysql)
        {
            Alert_Dec.Show("网络连接失败");
            return;
        }
        SumSave.crt_pet.GetPets.Remove(select_pet);
        int number = select_pet.GetCrtTalent.Count + crt_pet.GetCrtTalent.Count;
        int crt_number = Random.Range(number / 2, (number) / 2 + 2);
#if UNITY_EDITOR
        crt_number = 9;
#elif UNITY_ANDROID
#elif UNITY_IPHONE
#endif
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
