using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 展示装备的界面
/// </summary>
public class panel_equip : Panel_Base
{

    private equip_item equip_item_prafabs;
    private Transform crt_pos_equip;
    private panel_bag panel_bag;
    private panel_hero panel_hero;
    private bag_item crt_bag;
    private Bag_Base_VO crt_equip;
    private GridLayoutGroup gridLayoutgroup;
    private Show_Material show_material_prafabs;
    protected override void Awake()
    {
        base.Awake();
    }

    public override void Initialize()
    {
        base.Initialize();
        equip_item_prafabs = Battle_Tool.Find_Prefabs<equip_item>("equip_item"); //Resources.Load<equip_item>("Prefabs/panel_equip/equip_item");
        show_material_prafabs= Resources.Load<Show_Material>("Prefabs/panel_equip/Show_Material");
        crt_pos_equip = Find<Transform>("bg_main");
        panel_bag=UI_Manager.I.GetPanel<panel_bag>();
        gridLayoutgroup = Find<GridLayoutGroup>("bg_main");
        panel_hero= UI_Manager.I.GetPanel<panel_hero>();
    }

    public override void Show()
    {
        base.Show();
    }


    #region 显示装备
    /// <summary>
    /// 传递参数
    /// </summary>
    /// <param name="bag"></param>
    public void Init(bag_item bag)
    {
        gridLayoutgroup.cellSize = new Vector2(178, 360);
        for (int i = crt_pos_equip.childCount - 1; i >= 0; i--)
        {
            Destroy(crt_pos_equip.GetChild(i).gameObject);
        }
        crt_bag = bag;
        equip_item item = Instantiate(equip_item_prafabs, crt_pos_equip);
        item.Show_Info_Btn();
        item.Data = bag.Data;
        //foreach (var equip in SumSave.crt_euqip)
        //{
        //    if (equip.StdMode == crt_bag.Data.StdMode)
        //    {
        //        equip_item equip_item = Instantiate(equip_item_prafabs, crt_pos_equip);
        //        equip_item.Show_take_Btn();
        //        equip_item.Data = equip;
        //        crt_equip = equip;
        //    }
        //}
    }

    /// <summary>
    /// 显示装备
    /// </summary>
    /// <param name="bag"></param>
    public void Select_Equip(bag_item bag)
    {
        gridLayoutgroup.cellSize = new Vector2(178, 360);
        ClearObject(crt_pos_equip);
        crt_equip = bag.Data;
        equip_item item = Instantiate(equip_item_prafabs, crt_pos_equip);
        item.Show_take_Btn();
        item.Data = bag.Data;
    }

    /// <summary>
    /// 脱下
    /// </summary>
    /// <param name="index"></param>
    protected void OnTake_Btn(int index)
    {
        if (index == 0)
        {
            //SumSave.crt_bag.Add(crt_equip);
            //SumSave.crt_euqip.Remove(crt_equip);
            //Game_Omphalos.i.Wirte_ResourcesList(Emun_Resources_List.bag_value, SumSave.crt_bag);
            //Game_Omphalos.i.Wirte_ResourcesList(Emun_Resources_List.equip_value, SumSave.crt_euqip);
            //SendNotification(NotiList.Refresh_Max_Hero_Attribute);
        }
        Refresh();
    }
    /// <summary>
    /// 点击事件 0 穿戴 1出售
    /// </summary>
    /// <param name="data"></param>
    protected void OnClick_Btn(int index)
    {
        //穿戴
        if (index == 0)
        {
            //List<Bag_Base_VO> euqip = new List<Bag_Base_VO>();
            //foreach (var item in SumSave.crt_euqip)
            //{
            //    if (item.StdMode == crt_bag.Data.StdMode)
            //    {
            //        euqip.Add(item);
            //    }
            //}
            //for (int i = 0; i < euqip.Count; i++)
            //{
            //    SumSave.crt_bag.Add(euqip[i]);
            //    SumSave.crt_euqip.Remove(euqip[i]);
            //}

            //WearingEquipmentTask();
            //SumSave.crt_bag.Remove(crt_bag.Data);
            //SumSave.crt_euqip.Add(crt_bag.Data);
            //Game_Omphalos.i.Wirte_ResourcesList(Emun_Resources_List.bag_value, SumSave.crt_bag);
            //Game_Omphalos.i.Wirte_ResourcesList(Emun_Resources_List.equip_value, SumSave.crt_euqip);
            //SendNotification(NotiList.Refresh_Max_Hero_Attribute);
        }
        else
        if (index == 1)//锁定
        {
            string[] info_str = crt_bag.Data.user_value.Split(' ');
            if (info_str.Length >= 6)
            {
                info_str[5] = info_str[5] == "1" ? "0" : "1";
                crt_bag.Data.user_value = Battle_Tool.Equip_User_Value(info_str);
                //Game_Omphalos.i.Wirte_ResourcesList(Emun_Resources_List.bag_value, SumSave.crt_bag);
            }
            else Alert_Dec.Show("装备品质过低,无法锁定");

        }
        else
         if (index == 2)
        {
            sell();
        }
        Refresh();
    }
    /// <summary>
    /// 出售单个物品
    /// </summary>
    private void sell()
    {
        int moeny = crt_bag.Data.price;
        bool exist = true;
        string[] info_str = crt_bag.Data.user_value.Split(' ');
        if (info_str.Length >= 6)
        {
            //回收绝世
            if (info_str[2] == "7")
            {
                exist = false;
                List<(string, int)> list = SumSave.crt_needlist.SetMap();
                int number = 0, value = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Item1 == MapStateList.背包回收魔丸.ToString())
                    {
                        number = list[i].Item2;
                        value = list[i].Item2;
                    }
                }
                if (number + (crt_bag.Data.need_lv / 2 + 1) < 50)
                {
                    number += (crt_bag.Data.need_lv / 2 + 1);
                    SumSave.crt_user_unit.verify_data(currency_unit.Boss积分, (crt_bag.Data.need_lv / 2 + 1));
                    string dec = "出售奖励\n" + "本次出售获得魔丸 " + ((crt_bag.Data.need_lv / 2 + 1)) + "\n今日剩余获取魔丸" + (50 - number);
                    Alert.Show("出售奖励", dec);
                    SumSave.crt_needlist.SetMap((MapStateList.背包回收魔丸.ToString(), number));
                    exist = true;
                }
                else
                {
                    if (number < 50)
                    {
                        SumSave.crt_user_unit.verify_data(currency_unit.Boss积分, (50 - number));
                        string dec = "出售奖励\n" + "本次出售获得魔丸 " + ((50 - number)) + "\n今日剩余获取魔丸" + (50 - number);
                        Alert.Show("出售奖励", dec);
                        number = 50;
                        SumSave.crt_needlist.SetMap((MapStateList.背包回收魔丸.ToString(), number));
                        exist = true;

                    }
                    else Alert_Dec.Show("本日绝世装备出售已满");
                }
                //else Alert_Dec.Show("本日绝世装备出售已满");
                //else
                //{
                //    if (number < 50)
                //    {
                //        number = 50;
                //    }
                //} 
                //else Alert_Dec.Show("本日绝世装备出售已满");
            }
        }
        if (exist)
        {
            SumSave.crt_user_unit.verify_data(currency_unit.金币, moeny);
            Alert_Dec.Show("出售成功 获得灵珠" + moeny);
            //SumSave.crt_bag.Remove(crt_bag.Data);
            //Game_Omphalos.i.Wirte_ResourcesList(Emun_Resources_List.bag_value, SumSave.crt_bag);
            SellingSellingEquipmentTask();
        }
        
    }
    /// <summary>
    /// 回收装备任务
    /// </summary>
    private void SellingSellingEquipmentTask()
    {
        tool_Categoryt.Base_Task(1014);
        tool_Categoryt.Base_Task(1021);
    }

    /// <summary>
    /// 穿戴装备任务
    /// </summary>
    private void WearingEquipmentTask()
    {
        tool_Categoryt.Base_Task(1009);
        tool_Categoryt.Base_Task(1012);
        if (crt_bag.Data.StdMode == "武器")
        {
            tool_Categoryt.Base_Task(1002);
        }
        tool_Categoryt.Base_Task(1013);
        tool_Categoryt.Base_Task(1042);
        if (crt_bag.Data.StdMode == "玉佩")
        {
            tool_Categoryt.Base_Task(1067);
        }
    }


    #endregion
    /// <summary>
    /// 选择材料
    /// </summary>
    /// <param name="item"></param>
    public void Select_Material(material_item item)
    {
        ClearObject(crt_pos_equip);
        gridLayoutgroup.cellSize = new Vector2(600, 400);
        //Show_Material
        Show_Material show_material = Instantiate(show_material_prafabs, crt_pos_equip);
        show_material.Init(item.GetItemData());
    }
    /// <summary>
    /// 选择丹药 或技能书
    /// </summary>
    /// <param name="item"></param>
    public void Select_Seed(material_item item)
    {
        ClearObject(crt_pos_equip);
        gridLayoutgroup.cellSize = new Vector2(600, 400);
        Show_Material show_material = Instantiate(show_material_prafabs, crt_pos_equip);
        show_material.Init(item.GetSeedData());
    }
    /// <summary>
    /// 使用丹药
    /// </summary>
    /// <param name="item"></param>
    public void Use_Seed(material_item item)
    {
        ClearObject(crt_pos_equip);
        gridLayoutgroup.cellSize = new Vector2(600, 400);
        Show_Material show_material = Instantiate(show_material_prafabs, crt_pos_equip);
        show_material.Init_Seed(item.GetSeedData());
    }
    /// <summary>
    /// 刷新
    /// </summary>
    protected void Refresh()
    {
        Hide();
        if (panel_bag.gameObject.activeInHierarchy) panel_bag.Show();
        if (panel_hero.gameObject.activeInHierarchy) panel_hero.Show();
    }
}
