using CodeStage.AntiCheat.ObscuredTypes;
using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using static UnityColorPresets;

public class offect_refined : Base_Mono
{

    private enum refined_btn_list
    { 
        幸运强化,
        转生强化,
        进阶强化,
    }
    private Transform pos_equip, pos_btn;
    /// <summary>
    /// 装备类型
    /// </summary>
    private equipuiItem equipuiItemprefab;
    /// <summary>
    /// 英雄图片
    /// </summary>
    private Image hero_icon;

    private btn_item btn_itemprefab;

    private show_refined_item show_refined_item_prefab;
    /// <summary>
    /// 装备类型数据存储
    /// </summary>
    private Dictionary<equip_refined_list, show_refined_item> keyValuePairs = new Dictionary<equip_refined_list, show_refined_item>();

    private TMP_Text redined_lucky_info, redined_type_info;
    private void Awake()
    {
        pos_btn = Find<Transform>("Scroll View/Viewport/Content");
        pos_equip = Find<Transform>("hero_equips/equips");
        equipuiItemprefab = Tool_UI.Find_Prefabs<equipuiItem>("equipuiItem");
        hero_icon = Find<Image>("hero_equips/hero_icon/icon");
        btn_itemprefab = Tool_UI.Find_Prefabs<btn_item>("btn_item");
        redined_lucky_info = Find<TMP_Text>("other_info/redined_lucky_info");
        redined_type_info = Find<TMP_Text>("other_info/redined_type_info");
        show_refined_item_prefab = Tool_UI.Find_Prefabs<show_refined_item>("show_refined_item");
        Init();
    }
    private void Init()
    {
        ClearObject(pos_btn);
        for (int i = 0; i < Enum.GetNames(typeof(equip_refined_list)).Length; i++)
        {
            equipuiItem equipuiItem = Instantiate(equipuiItemprefab,pos_equip);
            equipuiItem.Insance_Crate((equip_refined_list)(i));
            show_refined_item show_refined_item = Instantiate(show_refined_item_prefab, equipuiItem.transform);
            keyValuePairs.Add((equip_refined_list)(i), show_refined_item);
        }
        for (int i = 0; i < Enum.GetNames(typeof(refined_btn_list)).Length; i++)
        {
            btn_item btn_Item = Instantiate(btn_itemprefab, pos_btn);
            btn_Item.Show(i,(refined_btn_list)(i));
            btn_Item.GetComponent<Button>().onClick.AddListener(delegate { OnClick(btn_Item); });
        }
    }
    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="btn_Item"></param>
    private void OnClick(btn_item btn_Item)
    {
        switch ((refined_btn_list)btn_Item.index)
        {
            case refined_btn_list.幸运强化:
                Alert.Show(refined_btn_list.幸运强化.ToString(), "将消耗背包内全部的帝级幸运项链,\n请确认\n每件消耗元宝 * 100", confirm_lucky,100);
                break;
            case refined_btn_list.转生强化:
                if (SumSave.crtHero.zs_lvs > 1)
                {
                    Alert.Show(refined_btn_list.转生强化.ToString(), "将消耗背包内全部的皇级幸运项链,\n请确认\n每件消耗元宝 * 300", confirm_lucky, 300);
                }
                else
                { 
                   Alert_Dec.Show("未达到开启条件");
                }
                break;
            case refined_btn_list.进阶强化:
                Alert_Dec.Show("未达到开启条件");
                break;
        }
    } 
    
    private void confirm_lucky(object arg0)
    {
        int price = (int)arg0;
        int baselv = price == 100 ? 6 : 7;
        List<Bag_Base_VO> baglist = SumSave.crt_bags.Get_Bag_List();
        int number = 0;//数量
        int exp = 0;//给经验
        for (int i = 0; i < baglist.Count; i++)
        {
            if (baglist[i].StdMode == Stditem_StdMode_List.项链.ToString())
            {
                string[] info_str = baglist[i].user_value.Split(' ');
                if (info_str[1] == "2")//幸运项链
                {
                    int lv = int.Parse(info_str[2]);
                    if (lv == baselv)
                    {
                        number++;
                        if (baglist[i].need_lv < 30) exp += 1;
                        else if (baglist[i].need_lv < 50 && baglist[i].need_lv >= 30) exp += 2;
                        else if (baglist[i].need_lv < 60 && baglist[i].need_lv >= 50) exp += 3;
                        else if (baglist[i].need_lv < 70 && baglist[i].need_lv >= 60) exp += 4;
                        else if (baglist[i].need_lv >= 70) exp += 5;
                        baglist.RemoveAt(i);
                        i--;
                    }
                }
                
              
            }
          
           
        }
        number = 10; exp = 10;
        if (number > 0)
        {
            Clear_Condition();
            Need_Condition(currency_unit.元宝, number * price);
            if (Return_Condition())
            {
                if (SumSave.crt_refined.refined_numbers.Count > baselv - 6)
                {
                    SumSave.crt_refined.refined_numbers[baselv - 6] += exp;
                }
                else
                {
                    SumSave.crt_refined.refined_numbers.Add(exp);
                }
                SumSave.crt_refined.MysqlData();
                show_info();
                SumSave.crt_bags.Set_Bag_List(baglist);
                Alert.Show("洗炼强化", "消耗 " + currency_unit.元宝 + " * " + (number * price) + "\n获得 强化点" + exp);
            }
            else Alert_Dec.Show("元宝不足");

        }else Alert_Dec.Show("背包内没有可用物品");
    }

    private void OnEnable()
    {
        hero_icon.sprite = UI.UI_Manager.I.GetEquipSprite("UI/player/", SumSave.crtMaxBattle.hero_type);

        if (SumSave.crtHero.lv < 30)
        {
            Alert_Dec.Show("等级不足30级，无法进行强化");
            Hide();
            return;
        }
        show_info();
    }

    private void show_info()
    {
        string dec = "",decs="";
        int num = 0, surplus = -1;
        for (int k = 0; k < Enum.GetNames(typeof(refined_btn_list)).Length; k++)
        {
            num = 0; surplus = -1;
            switch ((refined_btn_list)k)
            {
                case refined_btn_list.幸运强化:
                    if (SumSave.crt_refined.refined_numbers.Count > 0)
                    {
                        num = SumSave.crt_refined.refined_numbers[0] / Enum.GetNames(typeof(redined_lucky_type)).Length;
                        surplus = SumSave.crt_refined.refined_numbers[0] > 0 ? SumSave.crt_refined.refined_numbers[0] % Enum.GetNames(typeof(redined_lucky_type)).Length : -1;
                        dec += Show_Color.Set_String(refined_btn_list.幸运强化 + " Lv." + SumSave.crt_refined.refined_numbers[0] + "", GameColors.Heal) + "\n";
                    }
                    else
                        dec += Show_Color.Set_String(refined_btn_list.幸运强化+" Lv." + 0 + "", GameColors.Heal) + "\n";

                    for (int j = 0; j < Enum.GetNames(typeof(redined_lucky_type)).Length; j++)
                    {
                        int number = Tool_Battle.Refined_MaxNumbers(num + (surplus == j ? 1 : 0));
                        Show_Lv((j), number);
                        switch ((redined_lucky_type)j)
                        {
                            case redined_lucky_type.生命值:
                                switch ((Hero_Type)SumSave.crtHero.job)
                                {
                                    case Hero_Type.平民:
                                        number = number * 10;
                                        break;
                                    case Hero_Type.战士:
                                        number = (int)(number * 18f);
                                        break;
                                    case Hero_Type.法师:
                                        number = (int)(number * 5f);
                                        break;
                                    case Hero_Type.道士:
                                        number = (int)(number * 12f);
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case redined_lucky_type.魔法值:
                                switch ((Hero_Type)SumSave.crtHero.job)
                                {
                                    case Hero_Type.平民:
                                        number = number * 10;
                                        break;
                                    case Hero_Type.战士:
                                        number = (int)(number * 5f);
                                        break;
                                    case Hero_Type.法师:
                                        number = (int)(number * 18f);
                                        break;
                                    case Hero_Type.道士:
                                        number = (int)(number * 12f);
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case redined_lucky_type.物理防御:
                                break;
                            case redined_lucky_type.魔法防御:
                                break;
                            case redined_lucky_type.攻击:
                                break;
                        }
                        dec += Show_Color.Set_String((redined_lucky_type)(j) + " + " + number + "", GameColors.Heal) + "\n";
                    }
                    break;
                case refined_btn_list.转生强化:
                    if (SumSave.crt_refined.refined_numbers.Count > 1)
                    {
                        num = SumSave.crt_refined.refined_numbers[1] / Enum.GetNames(typeof(redined_type)).Length;
                        surplus = SumSave.crt_refined.refined_numbers[1] > 0 ? SumSave.crt_refined.refined_numbers[1] % Enum.GetNames(typeof(redined_type)).Length : -1;
                        decs += Show_Color.Set_String(refined_btn_list.转生强化 + " Lv." + SumSave.crt_refined.refined_numbers[1] + "", GameColors.PhysicalDamage) + "\n";
                    }
                    else
                        decs += Show_Color.Set_String(refined_btn_list.转生强化 + " Lv." + 0 + "", GameColors.PhysicalDamage) + "\n";
                    for (int j = 0; j < Enum.GetNames(typeof(redined_type)).Length; j++)
                    {
                        int number = Tool_Battle.Refined_MaxNumbers(num + (surplus == j ? 1 : 0));
                        Show_Lv((j + Enum.GetNames(typeof(redined_lucky_type)).Length), number);
                        decs += Show_Color.Set_String((redined_type)(j) + " + " + number + "%", GameColors.PhysicalDamage) + "\n";
                    }
                    break;
                case refined_btn_list.进阶强化:
                    break;
                default:
                    break;
            }
        }
        redined_lucky_info.text = dec;
        redined_type_info.text = decs;
    }
    /// <summary>
    /// 显示强化等级
    /// </summary>
    /// <param name="i"></param>
    /// <param name="num"></param>
    private void Show_Lv(int i, int num)
    {
        foreach (var item in keyValuePairs)
        {
            if (item.Key == (equip_refined_list)i)
            {
                item.Value.Show("+" + num);
            }
        }
    }

    private void OnDisable()
    {
        //UI_Manager.Instance.GetPanel<PanelMian>().Show();
    }
    private void Hide()
    {
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }
}
