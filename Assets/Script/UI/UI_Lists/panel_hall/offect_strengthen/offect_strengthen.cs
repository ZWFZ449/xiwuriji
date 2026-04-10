using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class offect_strengthen : Base_Mono
{
    private List<string> btn_list = new List<string>() { "穿戴", "背包","合成","洗练" };
    private Transform pos_btn,pos_bag,pos_icon;
    /// <summary>
    /// 功能按钮
    /// </summary>
    private btn_item btn_item_Prefabs;
    /// <summary>
    /// 背包预制体
    /// </summary>
    private bag_item bag_item_Prefabs;
    /// <summary>
    /// 强化费用
    /// </summary>
    private List<long> needs = new List<long> { 100, 1000, 10000, 100000, 100000, 1000000, 1000000, 10000000, 100000000, 1000000000, 2000000000, 3000000000, 3000000000, 3000000000 };
    /// <summary>
    /// 当前选择
    /// </summary>
    private bag_item crt_bag;
    /// <summary>
    /// 合成列表
    /// </summary>
    private List<Bag_Base_VO> synthesis_list = new List<Bag_Base_VO>();
    /// <summary>
    /// 强化
    /// </summary>
    private Button  btn_strengthen;
    /// <summary>
    /// 显示信息
    /// </summary>
    private Text info;
    /// <summary>
    /// 当前选择
    /// </summary>
    private int index;
    private void Awake()
    {
        pos_btn = Find<Transform>("btn_list");
        pos_bag = Find<Transform>("Scroll View/Viewport/Content");
        pos_icon = Find<Transform>("icon");
        btn_item_Prefabs = Battle_Tool.Find_Prefabs<btn_item>("btn_item"); //Resources.Load<btn_item>("Prefabs/base_tool/btn_item");
        bag_item_Prefabs = Battle_Tool.Find_Prefabs<bag_item>("bag_item"); //Resources.Load<bag_item>("Prefabs/panel_bag/bag_item");
        btn_strengthen = Find<Button>("btn_strengthen");
        info = Find<Text>("info");
        btn_strengthen.onClick.AddListener(Strengthen);
        for (int i = 0; i < btn_list.Count; i++)
        {
            btn_item item = Instantiate(btn_item_Prefabs, pos_btn);
            item.Show(i, btn_list[i]);
            item.GetComponent<Button>().onClick.AddListener(() => { Select_Btn(item.index); });
        }
    }
    /// <summary>
    /// 强化
    /// </summary>
    private void Strengthen()
    {
        if (index == 3)
        {
            Strengthen_succinct();//洗练
            return;
        }
        if (index == 2)
        {
            Strengthen_synthesis();//合成
            return;
        }
        string[] infos = crt_bag.Data.user_value.Split(' ');
        int lv = int.Parse(infos[1]);
        if (lv >= crt_bag.Data.need_lv / 10 + 3)
        {
            Alert_Dec.Show("当前装备强化等级已满");
            return;
        }

        long need = CostReduction(lv);

        for (int i = 0; i < SumSave.db_strengthen_need_list.Count; i++)
        {
            if (crt_bag.Data.Name == SumSave.db_strengthen_need_list[i].need_value)
            {
                if (lv >= SumSave.db_strengthen_need_list[i].need_lv)
                {
                    Alert_Dec.Show("当前装备强化等级已满");
                    return;
                }
                need = needs[7 + lv];

                for (int j = 0; j < SumSave.db_strengthen_need_list[i].need_value_list.Length; j++)
                {
                    string[] need_value = SumSave.db_strengthen_need_list[i].need_value_list[j].Split('*');
                    if (need_value.Length == 2)
                    {
                        NeedConsumablesold(need_value[0], int.Parse(need_value[1]) * (lv + 1));
                    }
                }
            }
        }
        NeedConsumablesold(currency_unit.金币, need);
        if (RefreshConsumables_old())
        {
            infos[1] = (lv + 1).ToString();
            crt_bag.Data.user_value = Battle_Tool.Equip_User_Value(infos);
            Select_Strengthen(crt_bag);
            //if (index == 0)
            //{
            //    Game_Omphalos.i.Wirte_ResourcesList(Emun_Resources_List.equip_value, SumSave.crt_euqip);
            //}
            //else Game_Omphalos.i.Wirte_ResourcesList(Emun_Resources_List.bag_value, SumSave.crt_bag);
            SendNotification(NotiList.Refresh_Max_Hero_Attribute);
            EquipmentEnhancementTask();
        }
        else Alert_Dec.Show("材料不足");
    }

    private void Strengthen_succinct()
    {
        if (crt_bag == null) return;
        SendNotification(NotiList.Read_Mysql_Base_Time);
        if (!SumSave.is_ToMysqlOpen)
        {
            Alert_Dec.Show("网络连接失败");
            return;  
        }
        NeedConsumablesold(currency_unit.灵气, 2000);
        NeedConsumablesold(currency_unit.金币, 50000000);
        if (RefreshConsumables_old())
        {
            string[] infos = crt_bag.Data.user_value.Split(' ');
            crt_bag.Data.user_value = tool_Categoryt.Obtain_Equip(crt_bag.Data, int.Parse(infos[1]), int.Parse(infos[2]));
            //Game_Omphalos.i.Wirte_ResourcesList(Emun_Resources_List.equip_value, SumSave.crt_euqip);
            Game_Omphalos.i.archive();
            Alert_Dec.Show("洗练成功");
            Base_Show();
        }
        else Alert_Dec.Show("条件不足");
    }

    /// <summary>
    /// 合成功能
    /// </summary>
    private void Strengthen_synthesis()
    {
        if (synthesis_list.Count < 3)
        {
            Alert_Dec.Show("当前装备数量不足");
            return;
        }
        NeedConsumablesold(currency_unit.灵气, synthesis_list[0].equip_lv * 20 + 500);
        if (RefreshConsumables_old())
        {
            //foreach (Bag_Base_VO item in synthesis_list)
            //{
            //    SumSave.crt_bag.Remove(item);
            //}
            //SumSave.crt_bag.Add(tool_Categoryt.crate_equip(synthesis_list[0].Name, 7));
            //Game_Omphalos.i.Wirte_ResourcesList(Emun_Resources_List.bag_value, SumSave.crt_bag);
            Alert_Dec.Show("合成成功");
            Base_Show();
        }
        else Alert_Dec.Show(currency_unit.灵气 + "不足 " + (synthesis_list[0].equip_lv * 20 + 500));
    }

    /// <summary>
    /// 减少强化费用
    /// </summary>
    /// <param name="lv"></param>
    /// <returns></returns>
    private long CostReduction(int lv)
    {
        long need = needs[lv];
        for (int i = 0; i < SumSave.db_vip_list.Count; i++)
        {
            if (SumSave.db_vip_list[i].vip_lv == SumSave.crt_accumulatedrewards.SetRecharge().Item1)
            {
                need -= need * SumSave.db_vip_list[i].strengthenCosts / 100;
            }
        }
        return need;
    }

    /// <summary>
    /// 装备强化任务
    /// </summary>
    private void EquipmentEnhancementTask()
    {
        tool_Categoryt.Base_Task(1027);
        tool_Categoryt.Base_Task(1037);
        tool_Categoryt.Base_Task(1041);
    }

        /// <summary>
        /// 选择对象
        /// </summary>
        /// <param name="index"></param>
    private void Select_Btn(object index)
    {
        this.index = (int)index;
        Base_Show();
    }

    public override void Show()
    {
        base.Show();
        Base_Show();
    }
    /// <summary>
    /// 初始化
    /// </summary>
    private void Base_Show()
    {
        ClearObject(pos_bag);
        pos_icon.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
        crt_bag = null;
        //if (index == 0)
        //{
        //    if (Show_Bag(SumSave.crt_euqip))
        //    {
        //        Alert_Dec.Show("当前无可强化装备");
        //    }
        //}
        //if (index == 1)
        //{
        //    if (Show_Bag(SumSave.crt_bag))
        //    {
        //        Alert_Dec.Show("当前无可强化装备");
        //    }
        //}
        //if (index == 2)
        //{
        //    synthesis_list = new List<Bag_Base_VO>();
        //    pos_icon.GetComponent<RectTransform>().sizeDelta = new Vector2(360, 100);
        //    info.text = "请放入合成装备(仅可使用神话级)";
        //    ClearObject(pos_icon);
        //    if (Show_synthesis(SumSave.crt_bag))
        //    {
        //        Alert_Dec.Show("当前无可合成装备");
        //    }
        //}
        //if (index == 3)
        //{
        //    synthesis_list = new List<Bag_Base_VO>();
        //    info.text = "请放入洗练装备(仅可使用绝世级灵宝)";
        //    ClearObject(pos_icon);
        //    SendNotification(NotiList.Refresh_Max_Hero_Attribute);
        //    if (Show_succinct(SumSave.crt_euqip))
        //    {
        //        Alert_Dec.Show("当前无可洗练装备");
        //    }
        //}
    }
    /// <summary>
    /// 洗练装备
    /// </summary>
    /// <param name="crt_euqip"></param>
    /// <returns></returns>
    private bool Show_succinct(List<Bag_Base_VO> list)
    {
        bool exist = true;
        for (int i = 0; i < list.Count; i++)
        {
            switch ((EquipConfigTypeList)Enum.Parse(typeof(EquipConfigTypeList), list[i].StdMode))
            {
                //case EquipConfigTypeList.武器:
                //case EquipConfigTypeList.衣服:
                //case EquipConfigTypeList.头盔:
                //case EquipConfigTypeList.项链:
                //case EquipConfigTypeList.护臂:
                //case EquipConfigTypeList.戒指:
                //case EquipConfigTypeList.手镯:
                //case EquipConfigTypeList.扳指:
                //case EquipConfigTypeList.腰带:
                //case EquipConfigTypeList.靴子:
                //case EquipConfigTypeList.护符:
                case EquipConfigTypeList.灵宝:
                //case EquipConfigTypeList.勋章:
                //case EquipConfigTypeList.饰品:
                //case EquipConfigTypeList.玉佩:
                //case EquipConfigTypeList.披风:
                    exist = false;
                    bag_item item = Instantiate(bag_item_Prefabs, pos_bag);
                    item.Data = (list[i]);
                    item.GetComponent<Button>().onClick.AddListener(() => { Select_succinct(item); });
                    if (crt_bag == null) Select_succinct(item);
                    break;
                default:
                    break;
            }
        }
        return exist;
    }
    /// <summary>
    /// 选择洗练对象
    /// </summary>
    /// <param name="item"></param>
    private void Select_succinct(bag_item data)
    {
        if (data != null)
        {
            crt_bag = data;
        }
        ClearObject(pos_icon);
        Instantiate(bag_item_Prefabs, pos_icon).Data = data.Data;
        info.text = "洗练" + data.Data.Name + "需要" + currency_unit.金币 + Battle_Tool.FormatNumberToChineseUnit(50000000);
        info.text += "\n灵气 * 2000";
    }

    private bool Show_Bag(List<Bag_Base_VO> list)
    {
        bool exist = true;
        for (int i = 0; i < list.Count; i++)
        {
            switch ((EquipConfigTypeList)Enum.Parse(typeof(EquipConfigTypeList), list[i].StdMode))
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
                case EquipConfigTypeList.灵宝:
                case EquipConfigTypeList.勋章:
                //case EquipConfigTypeList.饰品:
                //case EquipConfigTypeList.玉佩:
                case EquipConfigTypeList.披风:
                    exist = false;
                    bag_item item = Instantiate(bag_item_Prefabs, pos_bag);
                    item.Data = (list[i]);
                    item.GetComponent<Button>().onClick.AddListener(() => { Select_Strengthen(item); });
                    if (crt_bag == null) Select_Strengthen(item);
                    break;
                default:
                    break;
            }
        }

        return exist;
        
    }
    /// <summary>
    /// 合成列表
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    private bool Show_synthesis(List<Bag_Base_VO> list)
    {
        bool exist = true;

        for (int i = 0; i < list.Count; i++)
        {
            switch ((EquipConfigTypeList)Enum.Parse(typeof(EquipConfigTypeList), list[i].StdMode))
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
                //case EquipConfigTypeList.饰品:
                //case EquipConfigTypeList.玉佩:
                case EquipConfigTypeList.披风:
                    int state = 0;
                    if (list[i].user_value.Split(' ')[2] == "6")
                    {
                        if (synthesis_list.Count > 0)
                        {
                            for (int j = 0; j < synthesis_list.Count; j++) 
                            {
                                if (synthesis_list[j].Name == list[i].Name && synthesis_list[j]!= list[i])
                                {
                                    state++;
                                }
                            }
                        }
                        if (state == synthesis_list.Count)
                        {
                            exist = false;
                            bag_item item = Instantiate(bag_item_Prefabs, pos_bag);
                            item.Data = (list[i]);
                            item.GetComponent<Button>().onClick.AddListener(() => { Select_synthesis(item); });
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        return exist;

    }
    /// <summary>
    /// 选择合成物品
    /// </summary>
    /// <param name="item"></param>
    private void Select_synthesis(bag_item item)
    {
        for (int i = 0; i < synthesis_list.Count; i++)
        {
            if(synthesis_list[i] == item.Data)
            {
                synthesis_list.RemoveAt(i);
                Show_synthesis_list();
                Alert_Dec.Show("已取消合成材料");
                return;
            }
        }
        if (synthesis_list.Count < 3)
        {
            synthesis_list.Add(item.Data);
            Show_synthesis_list();
            ClearObject(pos_bag);
            //Show_synthesis(SumSave.crt_bag);
        }
        else Alert_Dec.Show("合成装备已满");
    }
    /// <summary>
    /// 显示合成材料
    /// </summary>
    private void Show_synthesis_list()
    {
        ClearObject(pos_icon);
        for (int i = 0; i < synthesis_list.Count; i++)
        {
            info.text = "合成" + synthesis_list[i].Name + "需要" + currency_unit.灵气 + (synthesis_list[i].equip_lv * 20 + 500);
            bag_item item = Instantiate(bag_item_Prefabs, pos_icon);
            item.Data = (synthesis_list[i]);
            item.GetComponent<Button>().onClick.AddListener(() => { Select_synthesis(item); });
        }

    }

    /// <summary>
    /// 选择强化
    /// </summary>
    /// <param name="data"></param>
    private void Select_Strengthen(bag_item data)
    {
        if(data!=null)
        {
            crt_bag = data;
        }
       
        ClearObject(pos_icon);
        Instantiate(bag_item_Prefabs, pos_icon).Data = data.Data;
        string[] infos = crt_bag.Data.user_value.Split(' ');
        int lv = int.Parse(infos[1]);
        long need = CostReduction(lv);
        info.text = "强化" + data.Data.Name + "需要\n";
        if (crt_bag.Data.StdMode == EquipConfigTypeList.灵宝.ToString())
        {
            need = needs[7 + lv];
            for (int i = 0; i < SumSave.db_strengthen_need_list.Count; i++)
            {
                if (crt_bag.Data.Name == SumSave.db_strengthen_need_list[i].need_value)
                {
                    for (int j = 0; j < SumSave.db_strengthen_need_list[i].need_value_list.Length; j++)
                    {
                        string[] need_value = SumSave.db_strengthen_need_list[i].need_value_list[j].Split('*');
                        if (need_value.Length == 2)
                        {
                            info.text += need_value[0] + " * " + (int.Parse(need_value[1]) * (lv + 1)) + (j % 3 == 0 ? "\n" : "   ");
                        }
                    }
                }
            }
        }
        info.text += currency_unit.金币 + Battle_Tool.FormatNumberToChineseUnit(need);


    }
}
