using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class panel_map : Panel_Base
{
    /// <summary>
    /// 掉落列表
    /// </summary>
    private Transform pos_life,pos_map,pos_btn_map;
    private btn_item btn_item_prefab;
    /// <summary>
    /// 地图名称
    /// </summary>
    private Text map_name;
    /// <summary>
    /// 等级要求
    /// </summary>
    private Text need_lv;
    /// <summary>
    /// 怪物列表
    /// </summary>
    private Text monster_list;
    /// <summary>
    /// 门票要求
    /// </summary>
    private Text need_Required;
    /// <summary>
    /// 地图列表
    /// </summary>
    private Dictionary<map_pos_item, user_map_vo> maplists = new Dictionary<map_pos_item, user_map_vo>();

    /// <summary>
    /// 战斗地图
    /// </summary>
    private panel_fight fight_panel;
    /// <summary>
    /// 当前选择地图
    /// </summary>
    private user_map_vo crt_map;
    /// <summary>
    /// 进入地图按钮
    /// </summary>
    private Button enter_map_button;
    /// <summary>
    /// 展示列表
    /// </summary>
    private Button btnOpen_list;
    private Transform base_show_info;
    private material_item material_item_parfabs;
    private Image show_map_list;
    protected override void Awake()
    {
        base.Awake();
    }


    public override void Initialize()
    {
        base.Initialize();
        pos_map = Find<Transform>("bg_main/Scroll View/Viewport/Content/bg_map");
        map_name = Find<Text>("bg_main/base_info/map_name");
        need_lv = Find<Text>("bg_main/base_info/need_lv");
        monster_list = Find<Text>("bg_main/base_info/monster_list");
        need_Required = Find<Text>("bg_main/base_info/need_Required");
        pos_life = Find<Transform>("bg_main/base_info/ProfitList/Scroll View/Viewport/Content");
        enter_map_button = Find<Button>("bg_main/base_info/enter_map_button");
        enter_map_button.onClick.AddListener(Open_Map);
        fight_panel = UI_Manager.I.GetPanel<panel_fight>();
        base_show_info = Find<Transform>("bg_main/base_info");
        material_item_parfabs = Battle_Tool.Find_Prefabs<material_item>("material_item");  //Resources.Load<material_item>("Prefabs/panel_bag/material_item");
        btn_item_prefab = Battle_Tool.Find_Prefabs<btn_item>("btn_item"); //Resources.Load<btn_item>("Prefabs/base_tool/btn_item");
        btnOpen_list=Find<Button>("bg_main/btn_list");
        btnOpen_list.onClick.AddListener(Open_List);
        pos_btn_map= Find<Transform>("bg_main/Reward_map/Viewport/maps");
        show_map_list = Find<Image>("bg_main/Reward_map");
    }
    /// <summary>
    /// 展示地图列表
    /// </summary>
    private void Open_List()
    {
        show_map_list.gameObject.SetActive(true);
        ClearObject(pos_btn_map);
        foreach (var map in SumSave.read_lose_map)
        {
            if (map.need_lv <= SumSave.crt_MaxHero_okd.Lv && map.map_type <= 3)
            {
                btn_item btn = Instantiate(btn_item_prefab, pos_btn_map);
                string dec = map.map_name + "\n(Lv." + map.need_lv + "级)";
                switch (map.map_type)
                {
                    case 2:
                        dec = Show_Color.Yellow(dec);
                        break;
                    case 3:
                        dec = Show_Color.Red(dec);
                        break;
                }
                btn.Show(map.map_index, dec);
                btn.GetComponent<Button>().onClick.AddListener(delegate { Select_Map(btn); });
            }

        }

    }
    private void Select_Map(btn_item item)
    {
        base_show_info.gameObject.SetActive(true);
        user_map_vo map = ArrayHelper.Find(SumSave.read_lose_map, m => m.map_index == item.index);
        Show_Info(map);
    }
    /// <summary>
    /// 
    /// 显示信息
    /// </summary>
    /// <param name="map"></param>
    private void Show_Info(user_map_vo map)
    {
        crt_map = map;
        map_name.text = map.map_name;
        need_lv.text = "等级要求： " + map.need_lv.ToString();
        monster_list.text = "怪物列表： " + map.monster_list.ToString();
        need_Required.text = "门票要求： " + map.need_Required.ToString();
        for (int i = pos_life.childCount - 1; i >= 0; i--)//清空区域内按钮
        {
            Destroy(pos_life.GetChild(i).gameObject);

        }
        foreach (string str in map.ProfitList.Split('&'))
        {
            string[] str1 = str.Split(' ');

            material_item item= Instantiate(material_item_parfabs, pos_life);
            item.Init(((str1[0]), 0));
            item.GetComponent<Button>().onClick.AddListener(delegate { Alert.Show(str1[0], str1[0]); });
#if UNITY_EDITOR
            //task_equip(str1[0]);
#elif UNITY_ANDROID
#elif UNITY_IPHONE
#endif
        }
    }


    /// <summary>
    /// 测试掉落
    /// </summary>
    private void task_equip(string map_name)
    {
        //测试使用
        Bag_Base_VO  bag = ArrayHelper.Find(SumSave.db_stditems, e => e.Name == map_name);
        switch ((EquipConfigTypeList)Enum.Parse(typeof(EquipConfigTypeList), bag.StdMode))
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
            case EquipConfigTypeList.护符:
            case EquipConfigTypeList.灵宝:
            case EquipConfigTypeList.勋章:
            case EquipConfigTypeList.饰品:
            case EquipConfigTypeList.玉佩:
            case EquipConfigTypeList.披风:
                bag = tool_Categoryt.crate_equip(bag.Name, 7);
                //SumSave.crt_bag.Add(bag);
                break;
            default:
                break;
        }
    }
   /// <summary>
   /// 初始化地图列表
   /// </summary>
   /// <param name="item"></param>
    private void Instance_Pos(map_pos_item item)
    {
        if (!maplists.ContainsKey(item))
        {
            foreach (var map in SumSave.read_lose_map)
            {
                if (map.map_type <= 3)
                {
                    if (map.map_index == item.index)
                    {
                        if (map.need_lv <= SumSave.crt_MaxHero_okd.Lv)
                        {
                            maplists.Add(item, map);
                            btn_item btn = Instantiate(btn_item_prefab, item.transform);
                            string dec = map.map_name + "\n(Lv." + map.need_lv + "级)";
                            switch (map.map_type)
                            {
                                case 2:
                                    dec = Show_Color.Yellow(dec);
                                    break;
                                case 3:
                                    dec = Show_Color.Red(dec);
                                    break;
                            }
                            btn.Show(item.index, dec);
                            btn.GetComponent<Button>().onClick.AddListener(delegate { Select_Map(item); });
                        }
                    }

                }
            }
        }
    }
    /// <summary>
    /// 选择地图
    /// </summary>
    /// <param name="item"></param>
    private void Select_Map(map_pos_item item)
    {
        //crt_map = item;
        base_show_info.gameObject.SetActive(true);
        user_map_vo map = maplists[item];
        Show_Info(map);
    }
    

    public override void Hide()
    {
        if (base_show_info.gameObject.activeInHierarchy)
        {
            base_show_info.gameObject.SetActive(false);
        }
        else
            base.Hide();
    }

    public override void Show()
    {
        base.Show();
        Base_Show();
        base_show_info.gameObject.SetActive(false);
    }

    private void Base_Show()
    {
        for (int i = 0; i < pos_map.childCount; i++)
        { 
            Instance_Pos(pos_map.GetChild(i).GetComponent<map_pos_item>());
        }
        if (show_map_list.gameObject.activeInHierarchy)
        {
            Open_List();
        }
    }

    /// <summary>
    /// 打开地图
    /// </summary>
    private void Open_Map()
    {
        if(crt_map==null)  return;
        if (crt_map.need_Required != "")
        {
            NeedConsumablesold(crt_map.need_Required, 1);
            if (RefreshConsumables_old())
            {
                Set_Map();
            }else Alert_Dec.Show("门票不足");
        }
        else
        {
            Set_Map();
        }
       
    }

    private void Set_Map()
    {
        Base_Task();

        fight_panel.Show();
        fight_panel.Open_Map(crt_map);
        Hide(); 
    }
    /// <summary>
    /// 进入地图任务
    /// </summary>
    private void Base_Task()
    {
        if (crt_map.map_index == 1)//新手任务
        {

            //Battle_Tool.NewbieTask(1001);
            tool_Categoryt.Base_Task(1001);

        }

        if (crt_map.map_name == "葬旧墟")
        {
            tool_Categoryt.Base_Task(1030);
        }

        if (crt_map.map_name == "黄泉岔")
        {
            tool_Categoryt.Base_Task(1044);
        }

        if (crt_map.map_name == "血牙狼窟")
        {
            tool_Categoryt.Base_Task(1052);
        }
        if (crt_map.map_name == "千悲窟")
        {
            tool_Categoryt.Base_Task(1059);
        }
        if (crt_map.map_name == "天敕封岳")
        {
            tool_Categoryt.Base_Task(1064);
        }
        if (crt_map.map_name == "肉棺穴")
        {
            tool_Categoryt.Base_Task(1078);
        }
        if (crt_map.map_name == "万足渊")
        {
            tool_Categoryt.Base_Task(1080);
        }
    }
}
