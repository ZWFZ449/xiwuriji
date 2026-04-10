using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class panel_bag : Panel_Base
{
    /// <summary>
    /// 装备类型
    /// </summary>
    private Dictionary<EquipTypeList, show_equip_item> dic_equips = new Dictionary<EquipTypeList, show_equip_item>();
    /// <summary>
    /// 功能按钮
    /// </summary>
    private btn_item btn_item_Prefabs;

    private bag_item bag_item_Prefabs;

    private material_item material_item_Prefabs;
    /// <summary>
    /// 按钮位置
    /// </summary>
    private Transform crt_btn, crt_bag, pos_function;

    private panel_equip panel_equip;
    /// <summary>
    /// 角色皮肤预制体
    /// </summary>
    private GameObject skin_prefabs;
    /// <summary>
    /// 角色within位置
    /// </summary>
    private Transform panel_role_health;

    private bag_btn_list crt_select_btn = bag_btn_list.装备;
    /// <summary>
    /// 功能列表
    /// </summary>
    private string[] function_list =new string[] { "一键分解","一键出售"};
    /// <summary>
    /// 显示页面
    /// </summary>
    private Text page_info;
    protected override void Awake()
    {
        base.Awake();
    }

    public override void Initialize()
    {
        base.Initialize();
        crt_btn = Find<Transform>("bg_main/show_bag/btns");
        crt_bag = Find<Transform>("bg_main/show_bag/Scroll View/Viewport/Content");
        //btn_item_Prefabs = Resources.Load<btn_item>("Prefabs/base_tool/btn_item");
        btn_item_Prefabs = Battle_Tool.Find_Prefabs<btn_item>("btn_item");
        bag_item_Prefabs = Battle_Tool.Find_Prefabs<bag_item>("bag_item");
        material_item_Prefabs = Battle_Tool.Find_Prefabs<material_item>("material_item");
        //material_item_Prefabs= Resources.Load<material_item>("Prefabs/panel_bag/material_item"); 
        panel_equip = UI_Manager.I.GetPanel<panel_equip>();

       
        skin_prefabs = Resources.Load<GameObject>("Prefabs/Skins/within_" + SumSave.old_crt_hero.hero_pos);
        panel_role_health = Find<Transform>("bg_main/bag_equips/panel_role_health");
        Instantiate(skin_prefabs, panel_role_health);
        show_tianming_Platform= Find<Transform>("bg_main/bag_equips/tianming_Platform");


        for (int i = 0; i < Enum.GetNames(typeof(bag_btn_list)).Length; i++)
        {
            btn_item btn_item = Instantiate(btn_item_Prefabs, crt_btn);//实例化背包装备
            btn_item.Show(i, (bag_btn_list)i);
            btn_item.GetComponent<Button>().onClick.AddListener(delegate { Select_Btn(btn_item); });
        }

        pos_function=Find<Transform>("bg_main/show_bag/function_list");
        page_info= Find<Text>("bg_main/show_bag/function_list/page_info");
        for (int i = 0; i < function_list.Length; i++)
        { 
            btn_item btn_item = Instantiate(btn_item_Prefabs, pos_function);//实例化背包装备
            btn_item.Show(i, function_list[i]);
            btn_item.GetComponent<Button>().onClick.AddListener(delegate { Select_functionBtn(btn_item); });
        }

        
    }
    /// <summary>
    /// 一键回收分解
    /// </summary>
    /// <param name="btn_item"></param>
    private void Select_functionBtn(btn_item btn_item)
    {

        switch (function_list[btn_item.index])
        { 
            case "一键分解":
                Alert.Show("一键分解", "是否分解所有可分解装备？", Break_Down);
              
                break;
            case "一键出售":
                Alert.Show("一键出售", "是否出售所有可出售装备？", Sell_All);
                break;
        }
    }
    private void Break_Down(object arg0)
    {
        break_down();
    }
        /// <summary>
        /// 分解
        /// </summary>
    private void break_down()
    {
        List<Bag_Base_VO> sell_list = new List<Bag_Base_VO>();

        foreach (Bag_Base_VO item in sell_list)//SumSave.crt_bag
        {
            if (item.user_value != null)
            {
                string[] info_str = item.user_value.Split(' ');
                if (info_str.Length >= 6)
                {
                    if (int.Parse( info_str[2])>=7 && info_str[5] == "0")
                    {
                        sell_list.Add(item);
                    }
                }
            }
        }
        if (sell_list.Count > 0)
        {
            foreach (Bag_Base_VO item in sell_list)
            {
                //SumSave.crt_bag.Remove(item);
            }
            int random = Random.Range(1, 100);
            int number = sell_list.Count * 2;
            int maxnumber = number + Random.Range(1, 100);
            Battle_Tool.Obtain_Resources(Obtain_Int.Add(1, "绝世碎片", new int[] { number + random, random }), maxnumber);
            //Battle_Tool.Obtain_Resources("绝世碎片", sell_list.Count * 2);
            //Game_Omphalos.i.Wirte_ResourcesList(Emun_Resources_List.bag_value, SumSave.crt_bag);
            Show_Bag();
            Alert.Show("分解成功", "获得" + Show_Color.Red("绝世碎片") + " * " + (sell_list.Count * 2));

        }
        else Alert_Dec.Show("暂无可分解装备");
    }

    /// <summary>
    /// 一件出售
    /// </summary>
    /// <param name="arg0"></param>
    private void Sell_All(object arg0)
    {
        sell_all();
    }

    /// <summary>
    /// 全部出售
    /// </summary>
    private void sell_all()
    {
        int moeny= 0;
        List<(string,int)> list = SumSave.crt_needlist.SetMap();
        List<Bag_Base_VO> sell_list = new List<Bag_Base_VO>();
        int number = 0, value=0;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Item1 == MapStateList.背包回收魔丸.ToString())
            {
                number= list[i].Item2;
                value= list[i].Item2;
            }
        }
        //foreach (Bag_Base_VO item in SumSave.crt_bag)
        //{
        //    if (item.user_value != null)
        //    {
        //        string[] info_str = item.user_value.Split(' ');
        //        if (info_str.Length >= 6)
        //        {
        //            if (info_str[5] == "0")
        //            {
        //                if (info_str[2] == "7")
        //                {
        //                    if (number + (item.need_lv / 2 + 1) < 50)
        //                    {
        //                        number += (item.need_lv / 2 + 1);
        //                        sell_list.Add(item);
        //                        moeny += item.price;
        //                    }
        //                    else
        //                    {
        //                        if (number < 50)
        //                        {
        //                            number = 50;
        //                            sell_list.Add(item);
        //                            moeny += item.price;

        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    sell_list.Add(item);
        //                    moeny += item.price;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            sell_list.Add(item);
        //            moeny += item.price;
        //        }
        //    }
        //}
        //if (sell_list.Count > 0)
        //{
        //    foreach (Bag_Base_VO item in sell_list)
        //    { 
        //        SumSave.crt_bag.Remove(item);
        //    }
        //}
        if (value < number)
        {
            SumSave.crt_user_unit.verify_data(currency_unit.Boss积分, number - value);
            string dec = "出售奖励\n" + "本次出售获得魔丸 " + (number - value) + "\n今日剩余获取魔丸" + (50 - number);
            Alert.Show("出售奖励", dec);
            SumSave.crt_needlist.SetMap((MapStateList.背包回收魔丸.ToString(), number));
        }
        else
        {
            if (number < 50)
            {
                Alert_Dec.Show("出售提示，今日还可获取魔丸 " + (50 - number));
            }
        }
        Alert_Dec.Show("出售成功，获得灵珠 " + moeny);
        SellingSellingEquipmentTask();
        //Game_Omphalos.i.Wirte_ResourcesList(Emun_Resources_List.bag_value, SumSave.crt_bag);
        SumSave.crt_user_unit.verify_data(currency_unit.金币, moeny);
        Show_Bag();
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
    /// 选中功能
    /// </summary>
    /// <param name="btn_item"></param>
    private void Select_Btn(btn_item btn_item)
    {
        crt_select_btn = (bag_btn_list)btn_item.index;
        Show_Bag();
        Alert_Dec.Show("打开 " + crt_select_btn + " 列表");
    }

    /// <summary>
    /// 初始化位置
    /// </summary>
    /// <param name="item"></param>
    protected void Instance_Pos(show_equip_item item)
    {
        if (!dic_equips.ContainsKey(item.type)) dic_equips.Add(item.type, item);
    }

    public override void Show()
    {
        base.Show();
        Show_Bag();
        //Base_Show();
        base_Equip();
        ObtainEquipmentTasks();
        if (tianming_Platform == null || !tianming_Platform.SequenceEqual(SumSave.old_crt_hero.tianming_Platform))
        {
            Show_Info_life();
        }
    }

    /// <summary>
    /// 获得装备任务
    /// </summary>
    private static void ObtainEquipmentTasks()
    {
        
            if (SumSave.GreenhandGuide_TotalTasks[SumSave.crt_greenhand.crt_task].tasktype == GreenhandGuideTaskType.收集任务)
            {
                GreenhandGuide_TotalTaskVO task = SumSave.GreenhandGuide_TotalTasks[SumSave.crt_greenhand.crt_task];//读取任务
                List<(string, int)> bag = SumSave.crt_bag_resources.Set();
                for (int i=0;i< bag.Count;i++)
                {
                    if (task.TaskDesc.Contains(bag[i].Item1))//判断任务名字是否包涵该装备
                    {
                        tool_Categoryt.Base_Task(1033);
                        tool_Categoryt.Base_Task(1048);
                        tool_Categoryt.Base_Task(1055);
                        tool_Categoryt.Base_Task(1056);
                        tool_Categoryt.Base_Task(1058);
                        tool_Categoryt.Base_Task(1065);
                    }
                }
                
            }
    }


    /// <summary>
    /// 显示装备
    /// </summary>
    private void base_Equip()
    {
        foreach (EquipTypeList item in dic_equips.Keys)
        {
            dic_equips[item].Init();
            //foreach (Bag_Base_VO equip in SumSave.crt_euqip)
            //{
            //    if (equip.StdMode == item.ToString())
            //    {
                    
            //        dic_equips[item].Data = equip;
            //    }
            //}
        }
    }




    #region 显示天命光环
    /// <summary>
    /// 天命台
    /// </summary>
    private int[] tianming_Platform;
    /// <summary>
    /// 天命台位置
    /// </summary>
    private Transform show_tianming_Platform;
    /// <summary>
    /// 天命台父物体大小,当前天命大小
    /// </summary>
    private Vector2 pos_tianming_size, tianming_size;
    /// <summary>
    /// 缩放比例
    /// </summary>
    private float scaling = 1;
    /// <summary>
    /// 每个天命的数量
    /// </summary>
    private Dictionary<int, int> tianming_num;

    /// <summary>
    /// 显示五行光环
    /// </summary>
    private void Show_Info_life()
    {

        tianming_Platform = (int[])SumSave.old_crt_hero.tianming_Platform.Clone();

        for (int i = show_tianming_Platform.childCount - 1; i >= 0; i--)//清空区域内按钮
        {
            Destroy(show_tianming_Platform.GetChild(i).gameObject);
        }
        pos_tianming_size = show_tianming_Platform.GetComponent<RectTransform>().rect.size;

        tianming_num = new Dictionary<int, int>();



        for (int i = 0; i < SumSave.old_crt_hero.tianming_Platform.Length; i++)
        {
            if (tianming_num.ContainsKey(SumSave.old_crt_hero.tianming_Platform[i]))
            {
                tianming_num[SumSave.old_crt_hero.tianming_Platform[i]]++;
            }
            else
            {
                tianming_num.Add(SumSave.old_crt_hero.tianming_Platform[i], 1);
            }
        }


        for (int i = 0; i < SumSave.old_crt_hero.tianming_Platform.Length; i++)
        {
            GameObject game = Resources.Load<GameObject>("Prefabs/halo/halo_" + (SumSave.old_crt_hero.tianming_Platform[i] + 1));
            GameObject tianming = Instantiate(game, show_tianming_Platform);

            tianming.transform.Rotate(new Vector3(0, 0, 15 * i));


            tianming_size = new Vector2(pos_tianming_size.x * scaling, pos_tianming_size.y * scaling);
            tianming.GetComponent<RectTransform>().sizeDelta = tianming_size;

            Color currentColor = tianming.GetComponentInChildren<Image>().color;
            currentColor.a = tianming_num[SumSave.old_crt_hero.tianming_Platform[i]] * 0.2f;
            tianming.GetComponentInChildren<Image>().color = currentColor;
        }
    }

    #endregion


    /// <summary>
    /// 显示背包装备
    /// </summary>
    private void Show_Bag()
    {
        ClearObject(crt_bag);
        switch (crt_select_btn)
        {
            case bag_btn_list.装备:
                //ArrayHelper.OrderDescding(SumSave.crt_bag,e=>int.Parse( e.user_value.Split(' ')[2]));
                //for (int i = 0; i < SumSave.crt_bag.Count; i++)
                //{
                //    bag_item item = Instantiate(bag_item_Prefabs, crt_bag);
                //    item.Data = SumSave.crt_bag[i];
                //    item.GetComponent<Button>().onClick.AddListener(delegate { Select_Bag(item); });
                //}
                //page_info.text= SumSave.crt_bag.Count+"/"+ SumSave.crt_resources.pages[0];
                break;
            case bag_btn_list.材料:
                //List<(string, int)> lists = SumSave.crt_bag_resources.Set();
                //for (int i = 0; i < lists.Count; i++)
                //{
                //    (string,int) data = lists[i];
                //    if (data.Item2 > 0)
                //    {
                //        material_item item = Instantiate(material_item_Prefabs, crt_bag);
                //        item.Init(data);
                //        item.GetComponent<Button>().onClick.AddListener(delegate { Select_Material(item); });
                //    }
                //}
                //page_info.text = lists.Count + "/" + SumSave.crt_resources.pages[0];

                break;   
               case bag_btn_list.丹囊:
                List<(string, List<string>)> list = SumSave.crt_seeds.Getformulalist();
                for (int i = 0; i < list.Count; i++)
                {
                    material_item item = Instantiate(material_item_Prefabs, crt_bag);
                    item.Init(list[i]);
                    item.GetComponent<Button>().onClick.AddListener(delegate { Select_Getformula_Material(item); });
                }
                page_info.text = list.Count + "/" + SumSave.crt_resources.pages[1];
                break;
            case bag_btn_list.消耗品:
                List<(string, List<string>)> Seedlist = SumSave.crt_seeds.GetSeedList();
                for (int i = 0; i < Seedlist.Count; i++)
                {
                    material_item item = Instantiate(material_item_Prefabs, crt_bag);
                    item.Init(Seedlist[i]);
                    item.GetComponent<Button>().onClick.AddListener(delegate { Select_SeedMaterial(item); });
                }
                page_info.text = Seedlist.Count + "/" + SumSave.crt_resources.pages[1];
                break;
            default:
                break;
        }
        
    }
    /// <summary>
    /// 吃丹药
    /// </summary>
    /// <param name="item"></param>
    private void Select_SeedMaterial(material_item item)
    {
        panel_equip.Show();
        panel_equip.Use_Seed(item);
    }
    /// <summary>
    /// 选择合成材料
    /// </summary>
    /// <param name="item"></param>
    private void Select_Getformula_Material(material_item item)
    {
        panel_equip.Show();
        panel_equip.Select_Seed(item);
    }

    /// <summary>
    /// 选择材料
    /// </summary>
    /// <param name="item"></param>
    private void Select_Material(material_item item)
    {
        panel_equip.Show();
        panel_equip.Select_Material(item);
    }

    /// <summary>
    /// 选择物品
    /// </summary>
    /// <param name="item"></param>
    private void Select_Bag(bag_item item)
    {
        panel_equip.Show();
        panel_equip.Init(item);
       
    }
    /// <summary>
    /// 显示自身装备
    /// </summary>
    /// <param name="item"></param>
    protected void Select_Equip(bag_item item)
    {
        panel_equip.Show();
        panel_equip.Select_Equip(item);

    }
}
