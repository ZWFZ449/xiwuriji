using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class panel_smallWorld : Panel_Base
{
    /// <summary>
    /// 宠物探索位置
    /// </summary>
    private Pet_explore _explore;

    /// <summary>
    /// 按钮位置
    /// </summary>
    private Transform pos_btn;
    /// <summary>
    /// 按钮预制体
    /// </summary>
    private btn_item btn_item;
    /// <summary>
    /// 背景
    /// </summary>
    private Image small_World_bg;
    /// <summary>
    /// 显示值
    /// </summary>
    private Text base_info;
    private string[] btn_list = new string[] { "升级", "农庄", "灵宠", "探险" }; 
    /// <summary>
    /// 宠物信息窗口
    /// </summary>
    private hatching_progress hatching_progress;
    /// <summary>
    /// 关闭宠物信息窗口
    /// </summary>
    private Button close_hatching;
    /// <summary>
    /// 上阵探索的宠物
    /// </summary>
    private List<db_pet_vo_old> pet_expedition_list=new List<db_pet_vo_old>();
    /// <summary>
    /// 探索宠物位置
    /// </summary>
    private Transform pos_btn_explore;
    /// <summary>
    /// 宠物预制体
    /// </summary>
    private pet_item pet_item_Prefabs;
    /// <summary>
    /// 探索奖励显示位置
    /// </summary>
    private Transform pos_items_reward;
    /// <summary>
    /// 奖励预制体
    /// </summary>
    private material_item material_item_Prefabs;
    /// <summary>
    /// 庄园列表
    /// </summary>
    private show_Plant show_plant;
    /// <summary>
    /// 当前宠物奖励列表
    /// </summary>
     List<(string, int)> pet_item_reward= new List<(string, int)>();
    public override void Initialize()
    {
        base.Initialize();
        small_World_bg = Find<Image>("small_World");
        _explore = Find<Pet_explore>("small_World/Pet_explore");
        pos_btn=Find<Transform>("bg_main/btn_list");
        base_info = Find<Text>("bg_main/Scroll View/Viewport/base_info/info");
        btn_item = Battle_Tool.Find_Prefabs<btn_item>("btn_item");// Resources.Load<btn_item>("Prefabs/base_tool/btn_item");
        
        hatching_progress = Find<hatching_progress>("small_World/hatching_progress");
        close_hatching = Find<Button>("small_World/hatching_progress/but");
        close_hatching.onClick.AddListener(() => { CloseHatching(); });

        pos_btn_explore = Find<Transform>("small_World/Pet_explore/explore/pet_pos_btn");
        pet_item_Prefabs = Battle_Tool.Find_Prefabs<pet_item>("pet_item"); //Resources.Load<pet_item>("Prefabs/panel_smallWorld/pets/pet_item");
        pos_items_reward= Find<Transform>("small_World/Pet_explore/Income/Viewport/Items");
        material_item_Prefabs= Battle_Tool.Find_Prefabs<material_item>("material_item"); //Resources.Load<material_item>("Prefabs/panel_bag/material_item");
        show_plant=Find<show_Plant>("small_World/show_plant");
        for (int i = 0; i < btn_list.Length; i++)
        {
            btn_item btn_items = Instantiate(btn_item, pos_btn);//实例化背包装备
            btn_items.Show(i, btn_list[i]);
            btn_items.GetComponent<Button>().onClick.AddListener(delegate { Select_Btn(btn_items); });
        }

        if (SumSave.crt_world != null)
        {
            //刷新下状态
            Update_State();
        }
    }
    /// <summary>
    /// 刷新状态
    /// </summary>
    private void Update_State()
    {
        //List<string> list = SumSave.crt_world.Get();
        //int time = Battle_Tool.SettlementTransport(list[0]); ;
        //SumSave.crt_world.Set(Obtain_Init(1, time, int.Parse(list[1])));

        //Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_world, SumSave.crt_world.Set_Uptade_String(), SumSave.crt_world.Get_Update_Character());
    }

    //点击宠物显示奖励信息
    private void UpSetReward(pet_item pet_item)
    {
        RefreshReward(pet_item);
    }

    /// <summary>
    /// 刷新奖励信息
    /// </summary>
    private void RefreshReward(pet_item pet_item)
    {
        pet_item_reward = pet_item.SetItemList();
        ClearObject(pos_items_reward);
        for (int i = 0; i < pet_item_reward.Count; i++)
        {
            if (pet_item_reward.Count > 0)
            {
                material_item item = Instantiate(material_item_Prefabs, pos_items_reward);
                item.Init(pet_item_reward[i]);
            }

        }
    }

    /// <summary>
    /// 关闭宠物信息窗口
    /// </summary>
    private void CloseHatching()
    {
        small_World_bg.gameObject.SetActive(false);
        hatching_progress.gameObject.SetActive(false);
        close_hatching.gameObject.SetActive(false);
    }

    public override void Hide()
    {
        if (small_World_bg.gameObject.activeInHierarchy)//从最上层关闭
        {
            if (show_plant.gameObject.activeInHierarchy) show_plant.gameObject.SetActive(false);           
            if (_explore.gameObject.activeInHierarchy) _explore.Hide();
            small_World_bg.gameObject.SetActive(false);
        }
        else
            base.Hide();
    }

    /// <summary>
    /// 打开界面
    /// </summary>
    /// <param name="btn_item"></param>
    private void Select_Btn(btn_item btn_item)
    {
        small_World_bg.gameObject.SetActive(true);
        switch (btn_list[btn_item.index])
        { 
            case "升级":
                small_World_bg.gameObject.SetActive(false);
                uplv();
                break;
            case "农庄":
                show_plant.gameObject.SetActive(true);
                show_plant.Show();
                break;
            case "灵宠":
                hatching_progress.gameObject.SetActive(true);
                close_hatching.gameObject.SetActive(true);
                hatching_progress.Show();
                break;
            case "探险":
                _explore.Show();
                //InitExplore();
                break;
        }
    }

    /// <summary>
    /// 初始化探索
    /// </summary>
    private void InitExplore()
    {
        ///探索宠物but
        ClearObject(pos_btn_explore);
        ClearObject(pos_items_reward);
        if (pet_expedition_list.Count > 0)
        {
            for (int i = 0; i < pet_expedition_list.Count; i++)
            {
                pet_item pet_item = Instantiate(pet_item_Prefabs, pos_btn_explore);
                pet_item.Init(pet_expedition_list[i]);
                pet_item.StartPetItem();
                pet_item.GetComponent<Button>().onClick.AddListener(delegate { UpSetMaterial(pet_item); });
            }
        } 
    } 
    private void UpSetMaterial(pet_item pet_item)
    {

    }   
    /// <summary>
    /// 升级
    /// </summary>
    private void uplv()
    {
        if (SumSave.crt_world.World_Lv >= SumSave.db_lvs_old.world_lv_list_dic.Count)
        { 
            Alert_Dec.Show("已达最高等级");
            return;
        }
        List<(string,int)> dec = SumSave.db_lvs_old.world_lv_list_dic[SumSave.crt_world.World_Lv];
        for (int i = 0; i < dec.Count; i++)
        {
            NeedConsumablesold(dec[i].Item1, dec[i].Item2);
        }
        if (RefreshConsumables_old())
        {
            SumSave.crt_world.World_Lv++;
            Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_world, SumSave.crt_world.Set_Uptade_String(), SumSave.crt_world.Get_Update_Character());
        }
        else Alert_Dec.Show("材料不足");
    }
    public override void Show()
    {
        base.Show();

        if(SumSave.ios_account_number=="admin001")
        {
            Open_smallWorld();
            //SumSave.crt_needlist.user_value_list.Add(new List<string>());
            //SumSave.crt_needlist.user_value_list[0].Clear();
            //SumSave.crt_needlist.user_value_list[0].Add("100");
            //SumSave.crt_needlist.user_value_list[0].Add("100");
        }

        if (SumSave.crt_world == null)
        {
            Alert_Dec.Show("小世界未激活");
            Hide();
            return;
        }
        UpWorldTask();
        Base_Show();
    }



    /// <summary>
    /// 开启小世界
    /// </summary>
    private void Open_smallWorld()
    {
        SumSave.crt_world = new user_world_vo();
        SumSave.crt_world.World_Lv = 1;
        Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.mo_user_world, SumSave.crt_world.Set_Instace_String());
        AddLand();
        Alert_Dec.Show("小世界开启成功");
    }
    /// <summary>
    /// 添加一块土地
    /// </summary>
    private static void AddLand()
    {
        if (SumSave.crt_plant.Set().Count > 0)
        {
            return;
        }
        List<(string, DateTime)> Set = SumSave.crt_plant.Set();
        Set.Add(("0", SumSave.nowtime));
        SumSave.crt_plant.Set_data(Set);
    }



    /// <summary>
    /// 开启小世界任务
    /// </summary>
    private static void UpWorldTask()
    {
        tool_Categoryt.Base_Task(1086);
    }
    public  void Base_Show()
    {
        List<string> list = SumSave.crt_world.Get();
        int time = Battle_Tool.SettlementTransport(list[0]);
        string dec = "界灵：Lv." + SumSave.crt_world.World_Lv + "\n";
        dec += "灵气 ：" + Obtain_Init(1) + "(Max" + Obtain_Init(2) + ")\n";
        dec += "每分钟可获得 ：" + SumSave.db_lvs_old.world_offect_list[SumSave.crt_world.World_Lv]+  "灵气\n";
        dec += "历练获得 :" + (SumSave.crt_world.World_Lv * 10) + "%\n";
        dec += "最大种植数量 :" + (SumSave.crt_world.World_Lv / 5 +3) + "个\n";
        dec += "最大宠物数量 :" + (SumSave.crt_world.World_Lv / 10 + 1) + "个\n";
        dec += "宠物探险数量 :" + (SumSave.crt_world.World_Lv / 30 + 1) + "个\n";
        dec += "宠物探险时长 :" + (SumSave.crt_world.World_Lv * 2 + 5) + "小时\n";
        dec += "宠物属性继承 :" + (SumSave.crt_world.World_Lv / 10 + 5) + "%\n";
        dec += "宠物孵化最高品质 :" + (SumSave.crt_world.World_Lv / 10 + 1) + "\n";
        dec += "宠物属性继承 :" + (SumSave.crt_world.World_Lv / 10 + 5) + "%\n";
        if (SumSave.crt_world.World_Lv >= SumSave.db_lvs_old.world_lv_list_dic.Count)
        {
            dec += "已达最高等级";
        }
        else
        {
            List<(string, int)> item = SumSave.db_lvs_old.world_lv_list_dic[SumSave.crt_world.World_Lv];
            for (int i = 0; i < item.Count; i++)
            {
                dec += Show_Color.Yellow((i == 0 ? "升级需求 1." : "\n" + (i + 1) + ".") + item[i].Item1 + " * " + item[i].Item2);
            }
        }
        base_info.text = dec;
    }
    /// <summary>
    /// 获取灵气值
    /// </summary>
    /// <param name="time"></param>
    private int Obtain_Init(int type)
    {
        int value = 0;
        switch (type)
        {
            case 1:
                List<long> list = SumSave.crt_user_unit.Set();
                value = (int)list[(int)currency_unit.灵气];
                break;
            case 2:
                value = SumSave.db_lvs_old.word_lv_max_value[SumSave.crt_world.World_Lv];
                value += Tool_State.Value_playerprobabilit(enum_skill_attribute_list.灵气上限);
                break;
            default:
                break;
        }

        return value;
    }
    
    protected override void Awake()
    {
        base.Awake();
    }


    

}
