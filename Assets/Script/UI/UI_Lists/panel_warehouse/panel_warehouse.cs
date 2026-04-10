using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class panel_warehouse : Panel_Base
{
    /// <summary>
    /// 装备预制体
    /// </summary>
    private bag_item bag_item_Prefabs;

    /// <summary>
    /// 装备栏显示位置,仓库栏显示位置
    /// </summary>
    private Transform Equipment, Warehouse;
    /// <summary>
    /// 显示仓库数量，显示装备数量
    /// </summary>
    private Text Warehouse_quantity, Equipment_quantity;

    /// <summary>
    /// 装备属性显示
    /// </summary>
    private equip_item equip_item_Prefabs;
    /// <summary>
    /// 选中装备
    /// </summary>
    private equip_item crt_euqip;
    /// <summary>
    /// 扩容
    /// </summary>
    private Button Equipment_title, Warehouse_title;


    public override void Initialize()
    {
        base.Initialize();
        bag_item_Prefabs = Battle_Tool.Find_Prefabs<bag_item>("bag_item");
        equip_item_Prefabs= Battle_Tool.Find_Prefabs<equip_item>("equip_item");
        Equipment = Find<Transform>("Equipment/Viewport/Content");
        Warehouse = Find<Transform>("Warehouse/Viewport/Content");
        Warehouse_quantity= Find<Text>("Warehouse/Warehouse_quantity");
        Equipment_quantity = Find<Text>("Equipment/Equipment_quantity");
        crt_euqip = Instantiate(equip_item_Prefabs, this.gameObject.transform);
        crt_euqip.gameObject.SetActive(false);
        Warehouse_title = Find<Button>("Warehouse/Warehouse_title"); 
        Equipment_title = Find<Button>("Equipment/Equipment_title");
        Equipment_title.onClick.AddListener(delegate { OnEquipment(); });
        Warehouse_title.onClick.AddListener(delegate { OnWarehouse(); });
    }
    /// <summary>
    /// 扩容仓库
    /// </summary>
    private void OnWarehouse()
    {
        if (SumSave.crt_resources.pages[1] >= SumSave.base_setting[5])
        {
            Alert_Dec.Show("仓库扩容已满");
            return;
        }
        int value = (SumSave.crt_resources.pages[1] - 59) * 500;
        if (value > 0)
        { 
            value=(int)MathF.Min(value, 20000);
            Alert.Show("扩容仓库", "扩容仓库\n消耗灵气 * " + value, confirmWarehouse, value);
        }
    }
    /// <summary>
    /// 扩容仓库
    /// </summary>
    /// <param name="arg0"></param>
    private void confirmWarehouse(object arg0)
    {
        int value = (int)arg0;
        NeedConsumablesold(currency_unit.灵气, value);
        if (RefreshConsumables_old())
        {
            SumSave.crt_resources.SetPage(1);
            Alert_Dec.Show("扩容成功");
            Refresh();
        }
    }
    /// <summary>
    /// 扩容装备
    /// </summary>
    private void OnEquipment()
    {
        if (SumSave.crt_resources.pages[0] >= SumSave.base_setting[4])
        {
            Alert_Dec.Show("背包扩容已满");
            return;
        }
        int value = (SumSave.crt_resources.pages[0] - 119) * 10;
        if (value > 0)
        {
            value = (int)MathF.Min(value, 200);
            Alert.Show("扩容背包", "扩容背包和材料包\n消耗魔丸 * " + value, confirmEquipment, value);
        }
    }
    /// <summary>
    /// 刷新扩容
    /// </summary>
    /// <param name="arg0"></param>
    private void confirmEquipment(object arg0)
    {
        int value = (int)arg0;
        NeedConsumablesold(currency_unit.Boss积分, value);
        if (RefreshConsumables_old())
        {
            SumSave.crt_resources.SetPage(0);
            Alert_Dec.Show("扩容成功");
            Refresh();
        }
    } 

    /// <summary>
    /// 显示装备
    /// </summary>
    public void ShowEquipment()
    {
        ClearObject(Equipment);
        //ArrayHelper.OrderDescding(SumSave.crt_bag, e => int.Parse(e.user_value.Split(' ')[2]));
        //for (int i = 0; i < SumSave.crt_bag.Count; i++)
        //{
        //    bag_item item = Instantiate(bag_item_Prefabs, Equipment);
        //    item.Data = SumSave.crt_bag[i];
        //    item.GetComponent<Button>().onClick.AddListener(delegate { Show_Bag_item(item,true); });
        //}
        //page_info.text = SumSave.crt_bag.Count + "/" + SumSave.crt_resources.pages[0];
    }

    /// <summary>
    /// 取出
    /// </summary>
    /// <param name="item"></param>
    protected void OnTakeOut_Btn(Bag_Base_VO item)
    {
        //if (SumSave.crt_bag.Count < SumSave.crt_resources.pages[0])
        //{
        //    List<Bag_Base_VO> euqip = new List<Bag_Base_VO>();
        //    SumSave.crt_house.Remove(item);
        //    SumSave.crt_bag.Add(item);
        //    Game_Omphalos.i.Wirte_ResourcesList(Emun_Resources_List.bag_value, SumSave.crt_bag);
        //    Game_Omphalos.i.Wirte_ResourcesList(Emun_Resources_List.house_value, SumSave.crt_house);
        //    Refresh();
        //}
        //else
        //{
        //    Alert_Dec.Show("背包已满，无法放入");
        //}
    }

    /// <summary>
    /// 存入
    /// </summary>
    /// <param name="item"></param>
    protected void OnHousedeposit_Btn(Bag_Base_VO item)
    {
    //    if (SumSave.crt_house.Count < SumSave.crt_resources.pages[1])
    //    {
    //        List<Bag_Base_VO> euqip = new List<Bag_Base_VO>();
    //        SumSave.crt_bag.Remove(item);
    //        SumSave.crt_house.Add(item);
    //        Game_Omphalos.i.Wirte_ResourcesList(Emun_Resources_List.bag_value, SumSave.crt_bag);
    //        Game_Omphalos.i.Wirte_ResourcesList(Emun_Resources_List.house_value, SumSave.crt_house);
    //        Refresh();
    //    }
    //    else
    //    {
    //        Alert_Dec.Show("仓库已满，无法放入");
    //    }
    }


    /// <summary>
    /// 点击从装备放入到仓库
    /// </summary>
    /// <param name="item"></param>
    private void Show_Bag_item(bag_item _item,bool ishouse)
    {
        crt_euqip.gameObject.SetActive(true);
        crt_euqip.Show_House_Btn(ishouse);
        crt_euqip.Data = _item.Data;
    }



    /// <summary>
    /// 显示仓库
    /// </summary>
    public void ShowWarehouse()
    {
        ClearObject(Warehouse);
        //ArrayHelper.OrderDescding(SumSave.crt_house, e => int.Parse(e.user_value.Split(' ')[2]));
        //for (int i = 0; i < SumSave.crt_house.Count; i++)
        //{
        //    bag_item item = Instantiate(bag_item_Prefabs, Warehouse);
        //    item.Data = SumSave.crt_house[i];
        //    item.GetComponent<Button>().onClick.AddListener(delegate { Show_Bag_item(item,false); });
        //}
    }
 
    /// <summary>
    /// 刷新显示
    /// </summary>
    public void Refresh()
    {
        crt_euqip.gameObject.SetActive(false);
        ShowEquipment();
        ShowWarehouse();
        ShowQuantity();
    }

    /// <summary>
    /// 显示数量
    /// </summary>
    private void ShowQuantity()
    {
        //Warehouse_quantity.text = SumSave.crt_house.Count.ToString()+"/"+SumSave.crt_resources.pages[1];
        //Equipment_quantity.text = SumSave.crt_bag.Count.ToString()+"/"+SumSave.crt_resources.pages[0];
    }


    public override void Hide()
    {
        base.Hide();
    }



    public override void Show()
    {
        base.Show();
        Refresh();
    }



}
