using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 装备显示图标
/// </summary>

public class equip_item : Base_Mono
{
    private Image item_icon, show_suit;
    /// <summary>
    /// 判断位置 0背包 1装备 2仓库
    /// </summary>
    private int state_pos=0;

    private Transform crt_bag, crt_btn;

    private bag_item bag_item_Prefabs;

    private btn_item btn_item_Prefabs;
    private string[] btn_list = new string[] { "穿戴", "锁定", "出售" };
    private string[] take_btn_list = new string[] { "卸下" };
    private string[] warehouse_btn_list = new string[] { "放入" };
    private Text show_name, show_base_need, show_info, info_suit;

    private void Awake()
    {
        crt_bag = Find<Transform>("show_icon");
        crt_btn = Find<Transform>("btn_list");
        btn_item_Prefabs = Battle_Tool.Find_Prefabs<btn_item>("btn_item"); //Resources.Load<btn_item>("Prefabs/base_tool/btn_item");
        bag_item_Prefabs = Battle_Tool.Find_Prefabs<bag_item>("bag_item"); //Resources.Load<bag_item>("Prefabs/panel_bag/bag_item");
        show_name = Find<Text>("show_name/info");
        show_base_need = Find<Text>("show_base_need");
        show_info = Find<Text>("show_bg/show_info");
        info_suit = Find<Text>("show_suit/show_info");
        show_suit = Find<Image>("show_suit");
    }


    private Bag_Base_VO data;

    /// <summary>
    /// Data
    /// </summary>
    public Bag_Base_VO Data
    {
        set
        {
            data = value;
            if (data == null) return;
            base_init();
        }
        get
        {
            return data;
        }
    }

    private void base_init()
    { 
    
    }

    private void base_init1()
    {
        //生成物品显示
        if (bag_item_Prefabs == null || crt_bag == null)
        {
            Awake();
        }
        show_suit.gameObject.SetActive(false);
        ClearObject(crt_bag);
        Instantiate(bag_item_Prefabs, crt_bag).Data = data;
        show_name.text = data.Name;
        string[] info = data.user_value.Split(' ');
        int strengthenlv = int.Parse(info[1]);
        int quilty = int.Parse(info[2]);
        //show_base_need.text = "品质:" + (enum_equip_quality_list)int.Parse(info[2]) + "\n" +
        //    "类型:" + data.StdMode + "\n" +
        //    "需求:" + data.equip_lv + "级";
        string dec = Show_Color.Yellow("[基础属性]");

        if (data.damgemin > 0 || data.damagemax > 0)
        {
            dec += "\n" + Show_Color.Black("物理攻击:" + data.damgemin + "-" + data.damagemax);
            if (strengthenlv > 0)
            {
                dec += Show_Color.Grey("(+" + (data.need_lv * strengthenlv) + ")");
            }
        }
        if (data.magicmin > 0 || data.magicmax > 0)
        {
            dec += "\n" + Show_Color.Black("魔法攻击:" + data.magicmin + "-" + data.magicmax);
            if (strengthenlv > 0)
            {
                dec += Show_Color.Grey("(+" + (data.need_lv * strengthenlv) + ")");
            }
        }
        if (data.defmin > 0 || data.defmax > 0)
        {
            dec += "\n" + Show_Color.Black("物理防御:" + data.defmin + "-" + data.defmax);
            if (strengthenlv > 0)
            {
                dec += Show_Color.Grey("(+" + (data.need_lv * strengthenlv / 2) + ")");
            }
        }
        if (data.macdefmin > 0 || data.macdefmax > 0)
        {
            dec += "\n" + Show_Color.Black("魔法防御:" + data.macdefmin + "-" + data.macdefmax);
            if (strengthenlv > 0)
            {
                dec += Show_Color.Grey("(+" + (data.need_lv * strengthenlv / 2) + ")");
            }
        }
        if (data.hp > 0)
        {
            dec += "\n" + Show_Color.Black("生命值:  " + data.hp);
        }
        if (data.mp > 0)
        {
            dec += "\n" + Show_Color.Black("魔法值:  " + data.mp);
        }
        if (info.Length > 4)
        {
            //类型
            string[] arr = info[3].Split(',');
            //值
            string[] arr_value = info[4].Split(',');
            dec += "\n" + Show_Color.Yellow("[附加属性]");
            int index = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                ++index;
                if (i == 0)
                {
                    switch (int.Parse(arr[i]))
                    {
                        case 1: dec += "\n" + Show_Color.Red("物理攻击: " + arr_value[i] + " - 0"); break;
                        case 2: dec += "\n" + Show_Color.Red("物理攻击: 0 -" + arr_value[i]); break;
                        case 3: dec += "\n" + Show_Color.Red("魔法攻击: " + arr_value[i] + " - 0"); break;
                        case 4: dec += "\n" + Show_Color.Red("魔法攻击: 0 -" + arr_value[i]); break;
                        case 5: dec += "\n" + Show_Color.Red("物理防御: " + arr_value[i]); break;
                        case 6: dec += "\n" + Show_Color.Red("魔法防御: " + arr_value[i]); break;
                        case 7: dec += "\n" + Show_Color.Red("生命值:   " + arr_value[i]); break;
                        case 8: dec += "\n" + Show_Color.Red("魔法值:   " + arr_value[i]); break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (index >= 7)
                    {
                        if (index == 7)
                        {
                            dec += "\n" + Show_Color.Yellow("[五行加成]");
                        }
                        dec += "\n" + Show_Color.Purple((enum_skill_attribute_list)(int.Parse(arr[i])) + ":" + arr_value[i]);

                    }
                    else
                    if (index >= 4)
                    {
                        if (index == 4)
                        {
                            dec += "\n" + Show_Color.Yellow("[加成属性]");
                        }
                        dec += "\n" + Show_Color.Red((enum_skill_attribute_list)(int.Parse(arr[i])) + ":" + arr_value[i] + "%");
                    } else
                        dec += "\n" + Show_Color.Green((enum_skill_attribute_list)(int.Parse(arr[i])) + ":" + arr_value[i] + "");


                }

            }
        }

        dec += Show_strengthenlv();
        if (Data.suit != 0)
        {
            show_suit.gameObject.SetActive(true);
            info_suit.text = Show_Suit(quilty);
        }
        show_info.text = dec;

    }


    private string Show_strengthenlv()
    {
        string dec = "";
        if (state_pos != 1) return dec;
        string[] infos = data.user_value.Split(' ');
        if (int.Parse(infos[1]) == 0&& int.Parse(infos[2]) != 7) return dec;
        switch ((EquipConfigTypeList)Enum.Parse(typeof(EquipConfigTypeList), data.StdMode))
        {
            case EquipConfigTypeList.护符:
            case EquipConfigTypeList.灵宝:
            case EquipConfigTypeList.勋章:
            case EquipConfigTypeList.饰品:
            case EquipConfigTypeList.玉佩:
            case EquipConfigTypeList.披风:
                return dec;
            default:
                break;
        }
        //装备加成 item1数量 item2最低判断表准（强化18 ） item3 品质7 item4 最低等级 item 绝世数量
        //  List<(int, int, int,int)> crt_euqips = new List<(int, int, int, int)>() { (1,18,0,100),(2,7,0,100)};
        (int, int, int, int, int) crt_euqip = (0, 18, 7, 100, 0);
        //添加装备效果
        List<Bag_Base_VO> crt_euqips = SumSave.crt_equips.Get(Dream_User_Equip_Type.装备);
        for (int i = 0; i < crt_euqips.Count; i++)
        {
            Bag_Base_VO datas = crt_euqips[i];
            string[] info = datas.user_value.Split(' ');
            int strengthenlv = int.Parse(info[1]);
            int quilty = int.Parse(info[2]);
            switch ((EquipConfigTypeList)Enum.Parse(typeof(EquipConfigTypeList), datas.StdMode))
            {
                case EquipConfigTypeList.武器:
                case EquipConfigTypeList.衣服:
                case EquipConfigTypeList.头盔:
                case EquipConfigTypeList.项链:
                case EquipConfigTypeList.护臂:
                case EquipConfigTypeList.戒指:
                case EquipConfigTypeList.手镯:
                case EquipConfigTypeList.扳指:
                case EquipConfigTypeList.腰带:
                case EquipConfigTypeList.靴子:
                    //case EquipConfigTypeList.护符:
                    //case EquipConfigTypeList.灵宝:
                    //case EquipConfigTypeList.勋章:
                    // case EquipConfigTypeList.饰品:
                    //case EquipConfigTypeList.玉佩:
                    //case EquipConfigTypeList.披风:
                    crt_euqip = (crt_euqip.Item1 + 1, (int)MathF.Min(crt_euqip.Item2, strengthenlv),
                        (int)MathF.Min(crt_euqip.Item3, quilty), (int)MathF.Min(crt_euqip.Item4, datas.equip_lv),
                        quilty >= 7 ? crt_euqip.Item5 + 1 : crt_euqip.Item5);

                    break;
                default:
                    break;
            }
        }
        //装备强化加成 
        if (crt_euqip.Item1 >= 10)
        {
            if (crt_euqip.Item2 >= 1)
            {
                dec += Show_Color.Yellow("\n[强化加成](" + crt_euqip.Item1 + "/10)\n") + Show_Color.Red((enum_skill_attribute_list.伤害加成).ToString() + ": +" + crt_euqip.Item2 * 5 + "%");
            }
            else
            {
                dec += Show_Color.Grey("\n[强化加成](" + crt_euqip.Item1 + "/10)\n") + Show_Color.Grey((enum_skill_attribute_list.伤害加成).ToString() + ": +" + crt_euqip.Item2 * 5 + "%");
            }

            if (crt_euqip.Item3 >= 7)
            {
                dec += Show_Color.Yellow("\n[绝世加成](" + crt_euqip.Item1 + "/10)" + Show_Color.Red("\n五行加成 " + crt_euqip.Item4));
            }
            else 
            {
                dec += Show_Color.Grey("\n[绝世加成](" + crt_euqip.Item5 + "/10)" + Show_Color.Grey("\n五行加成 " + crt_euqip.Item4));

            }
        }
        else
        {
            if (int.Parse(infos[1]) > 0)
            {
                dec += Show_Color.Grey("\n[强化加成](" + crt_euqip.Item1 + "/10)\n") + Show_Color.Grey((enum_skill_attribute_list.伤害加成).ToString() + ": +" + crt_euqip.Item2 * 5 + "%");
            }
            if (int.Parse(infos[2]) == 7)
            {
                dec += Show_Color.Grey("\n[绝世加成](" + crt_euqip.Item5 + "/10)" + Show_Color.Grey("\n五行加成 " + crt_euqip.Item4));
            }
        }

        return dec;
    }
    private string Show_Suit(int quilty)
    {
        string dec = "";
        int number = 0;
        bool exist = true;
        for (int i = 0; i < SumSave.db_Equip_Suits.Count; i++)
        {
            if (data.Name == SumSave.db_Equip_Suits[i].equip_name)
            {
                exist = false;
                int lv = int.Parse(data.user_value.Split(' ')[1]);
                dec = "\n" + Show_Color.Yellow("[" + data.StdMode + "]");
                if (quilty < 4) dec += Show_Color.Red("\n未激活");
                dec += "\n" + Show_Color.Black(data.StdMode + "特性: ");
                for (int j = 0; j < SumSave.db_Equip_Suits[i].equip_suit.Length; j++)
                { 
                    string[] value= SumSave.db_Equip_Suits[i].equip_suit[j].Split(' ');
                    string[] value2 = SumSave.db_Equip_Suits[i].equip_uplv[j].Split(' ');
                    dec += "\n" + Show_Color.Black((enum_equip_show_list)(int.Parse(value[0]))
                        + " " + (int.Parse(value[1]) + (lv / int.Parse(value2[0])) * (int.Parse(value2[1])) + "%"));
                }
            }
        }
        //if (data.StdMode == EquipConfigTypeList.灵宝.ToString())
        //{
        //    dec="\n" + Show_Color.Yellow("[灵宝]");
        //    dec += "\n" + Show_Color.Black("灵宝特性: " + data.suit_dec);
        //}
        //else
        if(exist)
        {
            db_suit_vo suit = SumSave.db_suits.Find(x => x.suit_type == Data.suit);
            //foreach (Bag_Base_VO item in SumSave.crt_euqip)
            //{
            //    if (item.suit == data.suit)
            //    {
            //        foreach (var value in SumSave.db_suits)
            //        {
            //            if (item.suit == value.suit_type)
            //            {
            //                number++;
            //            }
            //        }
            //    }
            //}
            dec += "\n" + Show_Color.Yellow("[套装] " + suit.suit_name) + "(" + number + "/" + suit.suit_number + ")";
            for (int i = 0; i < suit.suit_list.Count; i++)
            {
                if (number >= suit.suit_list[i].Item1)
                {
                    dec += "\n" + (enum_skill_attribute_list)suit.suit_list[i].Item2 + " : " + Math.Abs(suit.suit_list[i].Item3) + tool_Categoryt.Obtain_unit(suit.suit_list[i].Item2);
                }
                else
                    dec += "\n" + Show_Color.Grey((enum_skill_attribute_list)suit.suit_list[i].Item2 + " : " + Math.Abs(suit.suit_list[i].Item3) + tool_Categoryt.Obtain_unit(suit.suit_list[i].Item2));
            }
        }
        
        return dec;
    }

    /// <summary>
    /// 判断是否有开关 背包
    /// </summary>
    public void Show_Info_Btn()
    {
        state_pos = 0;
        for (int i = 0; i < btn_list.Length; i++)
        {
            btn_item item = Instantiate(btn_item_Prefabs, crt_btn);
            item.Show(i, btn_list[i]);
            item.GetComponent<Button>().onClick.AddListener(() => { OnClick_Btn(item); });
        }
    }
    /// <summary>
    /// 仓库开关
    /// </summary>
    /// <param name="ishouse"></param>
    public void Show_House_Btn(bool ishouse)
    {
        state_pos = 2;
        ClearObject(crt_btn);
        string house = ishouse ? "存入" : "取出";
        btn_item item = Instantiate(btn_item_Prefabs, crt_btn);
        item.Show(0, house);
        item.GetComponent<Button>().onClick.AddListener(() => {
            if (ishouse) OnHousedeposit_Btn(item);
            else
            {
                OnTakeOut_Btn(item);
            }  
        });
    }
    /// <summary>
    ///取出
    /// </summary>
    /// <param name="item"></param>
    private void OnTakeOut_Btn(btn_item item)
    {
        transform.parent.SendMessage("OnTakeOut_Btn",Data);
    }
    /// <summary>
    /// 存入
    /// </summary>
    /// <param name="item"></param>
    private void OnHousedeposit_Btn(btn_item item)
    {
        transform.parent.SendMessage("OnHousedeposit_Btn", Data);

    }

    /// <summary>
    /// 脱下
    /// </summary>
    public void Show_take_Btn()
    {
        state_pos = 1;
        for (int i = 0; i < take_btn_list.Length; i++)
        {
            btn_item item = Instantiate(btn_item_Prefabs, crt_btn);
            item.Show(i, take_btn_list[i]);
            item.GetComponent<Button>().onClick.AddListener(() => { OnTake_Btn(item); });
        }
    }
    /// <summary>
    /// 仓库转移
    /// </summary>
    public void Show_warehouse()
    {
        for (int i = 0; i < warehouse_btn_list.Length; i++)
        {
            btn_item item = Instantiate(btn_item_Prefabs, crt_btn);
            item.Show(i, warehouse_btn_list[i]);
            item.GetComponent<Button>().onClick.AddListener(() => { warehouseTransfer(); });
        }
    }
    /// <summary>
    /// 仓库转移
    /// </summary>
    private void warehouseTransfer()
    {
        //transform.parent();
    }


    /// <summary>
    /// 脱下
    /// </summary>
    /// <param name="item"></param>
    private void OnTake_Btn(btn_item item)
    {
        transform.parent.parent.SendMessage("OnTake_Btn", item.index);
    }

    /// <summary>
    /// 点击按钮
    /// </summary>
    /// <param name="item"></param>
    private void OnClick_Btn(btn_item item)
    {
        transform.parent.parent.SendMessage("OnClick_Btn", item.index);
    }
}
