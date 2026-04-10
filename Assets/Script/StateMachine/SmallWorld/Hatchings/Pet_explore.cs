using Common;
using Components;
using MVC;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Pet_explore : Base_Mono
{

    /// <summary>
    /// 探索按钮组
    /// </summary>
    private List<btn_item> button_map;

    private Transform pos_map;
    private Image explore_map;
    /// <summary>
    /// 玩家选择的探索
    /// </summary>
    private string explore;

    /// <summary>
    /// 探索次数
    /// </summary>
    private int IsExploring = 5;
    /// <summary>
    /// 宠物探索一次消耗的体力
    /// </summary>
    private int exhausting = 20;

    /// <summary>
    /// 按钮预制体
    /// </summary>
    private btn_item btn_item_Prefabs;
    /// <summary>
    /// 物品信息预制体
    /// </summary>
    private material_item info_item_Prefabs;
    /// <summary>
    /// 锚点
    /// </summary>
    private Transform pos_item_Prefabs;
    /// <summary>
    /// 探索宠物父物体
    /// </summary>
    private Transform pos_btn;
    /// <summary>
    /// 探索奖励父物体
    /// </summary>
    private Transform pos_Items;
    /// <summary>
    /// 功能按键父物体
    /// </summary>
    private Transform function_pos_btn;
    /// <summary>
    /// 获取探索位置
    /// </summary>
    private Dictionary<int,Transform> btn_item_Dic = new Dictionary<int, Transform>();
    /// <summary>
    /// 探索宠物预制体
    /// </summary>
    private explore_item explore_item_Prefabs;
    /// <summary>
    /// 当前探索
    /// </summary>
    private explore_item crt_explore=new explore_item();
    /// <summary>
    /// 功能按键列表
    /// </summary>
    private string[] function_btn_list = new string[] { "收获", "探索" };
    /// <summary>
    /// 存储路径
    /// </summary>
    private string setting_path = "宠物探索体力";
    /// <summary>
    /// 探索按钮列表
    /// </summary>
    private string[] pos_btn_list = new string[] { "1", "2", "3" };

    private material_item material_item_Prefabs;

    /// <summary>
    /// 探索体力条
    /// </summary>
    private Text physicalStrength;
    /// <summary>
    /// 探索体力滚动条
    /// </summary>
    private Slider physicalStrengthSlider;
    private Text info;
    protected void Awake()
    {
        pos_btn = Find<Transform>("explore/pet_pos_btn");
        pos_Items = Find<Transform>("Income/Viewport/Items");
        pos_item_Prefabs = Battle_Tool.Find_Prefabs<Transform>("pos_item"); //Resources.Load<Transform>("Prefabs/base_tool/pos_item");
        function_pos_btn = Find<Transform>("explore/function_pos_btn");
        btn_item_Prefabs = Battle_Tool.Find_Prefabs<btn_item>("btn_item"); //Resources.Load<btn_item>("Prefabs/base_tool/btn_item");
        info_item_Prefabs = Battle_Tool.Find_Prefabs<material_item>("material_item"); //Resources.Load<material_item>("Prefabs/panel_bag/material_item");
        explore_item_Prefabs = Battle_Tool.Find_Prefabs<explore_item>("explore_item"); //Resources.Load<explore_item>("Prefabs/panel_smallWorld/pets/explore_item");
        material_item_Prefabs = Battle_Tool.Find_Prefabs<material_item>("material_item");// Resources.Load<material_item>("Prefabs/panel_bag/material_item");
        explore_map = Find<Image>("explore_map");
        explore_map.gameObject.SetActive(true);
        pos_map = Find<Transform>("explore_map/Buttons_map");
        info = Find<Text>("Income/Viewport/info");

        physicalStrength = Find<Text>("explore_map/Title/Slider/physicalStrength");
        physicalStrengthSlider = Find<Slider>("explore_map/Title/Slider");
        displayPhysical();

        for (int i = 0; i < 3; i++)
        {
            Transform btn = Instantiate(pos_item_Prefabs, pos_btn);
            btn_item_Dic.Add(i, btn);
        }
        ClearObject(function_pos_btn);
        for (int i = 0; i < function_btn_list.Length; i++)
        {
            btn_item btn_item = Instantiate(btn_item_Prefabs, function_pos_btn);
            btn_item.Show(i, function_btn_list[i]);
            btn_item.GetComponent<Button>().onClick.AddListener(delegate { FunctionButton(btn_item); });
        }
        button_map = new List<btn_item>();
        for (int i = 0; i < 4; i++)
        {
            btn_item btn_item = Instantiate(btn_item_Prefabs, pos_map);
            btn_item.Show(i, "");
            btn_item.GetComponent<Button>().onClick.AddListener(delegate { MapButton(btn_item); });
            button_map.Add(btn_item);
        }
        explore_map.gameObject.SetActive(false);

    }
 
    
    /// <summary>
    /// 显示探索体力
    /// </summary>
    private void displayPhysical()
    {
        int number = Tool_State.GetSetMapState(setting_path);
        int max= 100 + Tool_State.Value_playerprobabilit(enum_skill_attribute_list.体质);
        physicalStrengthSlider.maxValue = max;
        physicalStrengthSlider.value = max - number;
        physicalStrength.text = "当前体力值为" + (max - number) + "/" + max;
    }


    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="btn_item"></param>
    private void MapButton(btn_item btn_item)
    {
        Obtain_Explore(btn_item.index);
    }
    public override void Show() 
    {
        base.Show();
        Base_Show();
    }
    public void Hide()
    {
        foreach (var index in btn_item_Dic.Keys)
        {
            ClearObject(btn_item_Dic[index]);
        }
        ClearObject(pos_Items);
        explore_map.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
    private void Base_Show()
    {
        ClearObject(pos_Items);
        int max = SumSave.crt_world.World_Lv / 30 + 1;
        max = Mathf.Min(max, 3);
        List<db_pet_vo_old> list = SumSave.crt_pet_Old.Set();
         
        int number = 0;
        for (int i = 0; i < list.Count; i++)
        {   if (list[i].pet_state == "2")
            {
                foreach (var index in btn_item_Dic.Keys)
                {
                    if (btn_item_Dic[index].childCount == 0)
                    {
                        explore_item item = Instantiate(explore_item_Prefabs, btn_item_Dic[index]);
                        item.Init(list[i]);
                        item.GetComponent<Button>().onClick.AddListener(() => { Obtain_Pet(item); });
                        if (crt_explore == null) Obtain_Pet(item);
                        number++;
                        break;
                    }
                }

            }
        }
        if (number < max)
        {
            for (int i = number; i < max; i++)
            {
                bool exist = true;
                foreach (var index in btn_item_Dic.Keys)
                {
                    if (exist)
                    {
                        if (btn_item_Dic[index].childCount == 0)
                        {
                            exist = false;
                            btn_item btn_item = Instantiate(btn_item_Prefabs, btn_item_Dic[index]);
                            btn_item.Show(i, "空位");
                            btn_item.GetComponent<Button>().onClick.AddListener(() => { Alert_Dec.Show("请上阵探索宠物"); });
                            break;
                        }
                    }
                    
                }
            }
        }
    }
    /// <summary>
    /// 打开宠物列表
    /// </summary>
    /// <param name="item"></param>
    private void Obtain_Pet(explore_item item)
    {
        if (crt_explore != null) crt_explore.Selected = false;
        crt_explore = item;
        crt_explore.Selected = true;
        GainRewards();
    }
    private void Update()
    {
        GainRewards();
    }

    /// <summary>
    /// 获取奖励列表
    /// </summary>
    private void GainRewards()
    {
        if (crt_explore == null)
        {
            return;
        }
        int maxtime = (SumSave.crt_world.World_Lv * 2 + 5) * 60;//单位 分钟
        Dictionary<string, string> dic = SumSave.crt_explore.Set();
        db_pet_vo_old vo = crt_explore.SetData();
        if (dic.ContainsKey(vo.petName + " " + vo.startHatchingTime))
        {
            //获取收益列表
            string value = dic[vo.petName + " " + vo.startHatchingTime];
            string[] data = value.Split(",");
            //获取最大时间长度
            //获取已经完成收益时间
            int time = Battle_Tool.SettlementTransport(data[1], data[0]);//(Convert.ToDateTime(data[1]) - Convert.ToDateTime(data[0])).Minutes;
            UpExplorationTime(maxtime, time);

            //计算收益
            string[] Explore_list = vo.pet_explore.Split("&");//获取该地图的奖励列表
            string[] values = data[2].Split(".");//Regex.Split(data[2], "[].");// 
            Dictionary<string, int> dic2 = new Dictionary<string, int>();
            for (int i = 0; i < values.Length; i++)
            {
                string[] value2 = values[i].Split(" ");
                if (value2.Length > 1)
                {
                    if (!dic2.ContainsKey(value2[0]))
                        dic2.Add(value2[0], 0);
                    dic2[value2[0]] += int.Parse(value2[1]);
                }
            }
            time = Battle_Tool.SettlementTransport(data[1]);// (int)((SumSave.nowtime - Convert.ToDateTime(data[1])).Minutes);
            maxtime = Mathf.Min(maxtime, Battle_Tool.SettlementTransport(data[0]));// (int)((SumSave.nowtime - Convert.ToDateTime(data[0])).Minutes));
            //time = 100;
            for (int i = 0; i < time; i += 6)
            {
                //获取收益列表
                dic2 = GainRewards(Explore_list, maxtime / 3600 + 3, dic2);
            }
            ClearObject(pos_Items);
            //更新数据
            string data_2 = "";
            foreach (string item in dic2.Keys)
            {
                Instantiate(material_item_Prefabs, pos_Items).Init((item, dic2[item]));
                data_2 += (data_2 == "" ? "" : ".") + item + " " + dic2[item];
            }
            data[1] = SumSave.nowtime > DateTime.Now ? SumSave.nowtime.ToString() : DateTime.Now.ToString();
            data[2] = data_2;// string.Join(".", dic2);
            //dic[vo.petName + " " + vo.startHatchingTime] = string.Join(",", data);
            SumSave.crt_explore.SetValues(vo.petName + " " + vo.startHatchingTime, string.Join(",", data));
            Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_pet_explore,
        SumSave.crt_explore.Set_Uptade_String(), SumSave.crt_explore.Get_Update_Character());
        }
    }

    private void UpExplorationTime(int maxtime, int time)
    {
        if (time >= maxtime)
        {
            info.text = "当前探索时间已满";
        }
        else
        {
            info.text = "当前探索时间 " + Show_Color.Red(time) + " 分钟/Max " + Show_Color.Red((SumSave.crt_world.World_Lv * 2 + 5) * 60) + " 分钟";
        }
    }

    /// <summary>
    /// 获取概率
    /// </summary>
    /// <param name="data"></param>
    /// <param name="index"></param>
    /// <param name="value"></param>
    private Dictionary<string, int> GainRewards(string[] data,int index, Dictionary<string, int> value)
    {
        index = Mathf.Min(data.Length, index);
        string random = data[Random.Range(0, index)];
        string[] odds = random.Split(" ");
        string[] odds2 = odds[odds.Length - 1].Split("/");
        if (Random.Range(0, int.Parse(odds2[1])) < int.Parse(odds2[0]))
        {
           if(!value.ContainsKey(odds[0]))
                value.Add(odds[0], 0);
            value[odds[0]] += int.Parse(odds[1]);
        }
        return value;
    }
    /// <summary>
    /// 按钮具体功能
    /// </summary>
    /// <param name="btn_item"></param>
    private void FunctionButton(btn_item btn_item)
    {
        switch (function_btn_list[btn_item.index])
        {
            case "收获":
                Harvest();
                break;
            case "探索":
                Show_Harvest();
                break;
        }
    }

    private void Show_Harvest()
    {
        explore_map.gameObject.SetActive(true);
        ///功能按键初始化
        Init();
    }
    /// <summary>
    /// 收获物品
    /// </summary>
    private void Harvest()
    {
        if (crt_explore == null)
        {
            Alert_Dec.Show("请选择收货的宠物");
            return;
        }
        Dictionary<string, string> dic = SumSave.crt_explore.Set();
        db_pet_vo_old vo = crt_explore.SetData();
        if (dic.ContainsKey(vo.petName + " " + vo.startHatchingTime))
        {
            //获取收益列表
            string value = dic[vo.petName + " " + vo.startHatchingTime];
            string[] data = value.Split(",");
            string[] values = data[2].Split(".");//Regex.Split(data[2], "[].");// 
            Dictionary<string, int> dic2 = new Dictionary<string, int>();
            int max = 1;
            for (int i = 0; i < values.Length; i++)
            {
                string[] value2 = values[i].Split(" ");
                if (value2.Length > 1)
                {
                    if (!dic2.ContainsKey(value2[0]))
                        dic2.Add(value2[0], 0);
                    dic2[value2[0]] += int.Parse(value2[1]);
                    max+= int.Parse(value2[1]);
                }
            }
            SumSave.crt_explore.DeleteValues(vo.petName + " " + vo.startHatchingTime);
            SumSave.crt_bag_resources.Get(dic2,max);
            Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_pet_explore,
                SumSave.crt_explore.Set_Uptade_String(), SumSave.crt_explore.Get_Update_Character());
            //写入数据库
            Game_Omphalos.i.Wirte_ResourcesList(Emun_Resources_List.material_value, 
                SumSave.crt_bag_resources.GetData());
            //更新宠物状态
            List<db_pet_vo_old> list = SumSave.crt_pet_Old.Set();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].petName == vo.petName && list[i].startHatchingTime == vo.startHatchingTime)
                {
                    list[i].pet_state = "0";
                    SumSave.crt_pet_Old.Get();
                }
            }
            Alert_Icon.Show(dic2);
            foreach (var index in btn_item_Dic.Keys)
            {
                ClearObject(btn_item_Dic[index]);
            }
            Base_Show();

            SendNotification(NotiList.Refresh_Max_Hero_Attribute);
        }
    }

    /// <summary>
    /// 初始化探索列表
    /// </summary>
    /// <param name="data"></param>
    public void Init()
    {
       for(int i=0;i< button_map.Count; i++)//随机添加地图名称
       {
            int r = Random.Range(0, SumSave.db_pet_explore.Count);
            button_map[i].Show(r, SumSave.db_pet_explore[r].petExploreMapName);
       }
        displayPhysical();
    }
    /// <summary>
    /// 点击探索
    /// </summary>
    private void Obtain_Explore(int index)
    {
        explore = SumSave.db_pet_explore[index].petExploreMapName;//获得探索地图的名字

        int number = Tool_State.GetSetMapState(setting_path);

        IsExploring = 100 + Tool_State.Value_playerprobabilit(enum_skill_attribute_list.体质)
            - number - exhausting;//判断体力是否足够

        if (IsExploring>= 0 && SumSave.db_pet_explore_dic.TryGetValue(explore, out user_pet_explore_vo vo)) //判断次数并且更具名字找到该地图的信息
        {
            SendNotification(NotiList.Read_Mysql_Base_Time);
            if (SumSave.openMysql)
            {
                Alert_Dec.Show("网络连接失败");
                return;
            }
            string[] Explore_list = vo.petEvent_reward.Split("&");//获取该地图的奖励列表

            int r = 0;
            while(true)
            {
                r++;
                string[] data = Explore_list[Random.Range(0, Explore_list.Length)].Split(" ");//根据空格拆分奖励列表
                string[] odds = data[2].Split("/");
                if (Random.Range(0, int.Parse(odds[1])) < int.Parse(odds[0]))//判断是否获得奖励
                {
                    GainRewards(data);
                    return;
                }
               

                if (r >= 1000)
                {
                    data = Explore_list[0].Split(" ");
                    GainRewards(data);
                    return;
                }
                  
            }
            Game_Omphalos.i.archive();
        }
        else Alert_Dec.Show("探索次数不足");
    }

    /// <summary>
    /// 获得奖励并发送消息
    /// </summary>
    /// <param name="data"></param>
    private void GainRewards(string[] data)
    {
        int i = Random.Range(1, int.Parse(data[1]) + 1);//随机获得奖励数量
        //消耗体力
        int value = Tool_State.GetSetMapState(setting_path) + exhausting;
        SumSave.crt_needlist.SetMap((setting_path, value));
        NGetRewards(data, i);
        Alert_Dec.Show("探索收益 " + data[0] + " x " + i);
        Init();
    }
    /// <summary>
    /// 获得奖励
    /// </summary>
    /// <param name="data"></param>
    /// <param name="i"></param>
    private static void NGetRewards(string[] data, int i)
    {
        switch (int.Parse(data[3]))
        {
            case 1:
                //获得材料技能书神器
                int random = Random.Range(1, 100);
                int number = i;
                int maxnumber = number + Random.Range(1, 100);
                Battle_Tool.Obtain_Resources(Obtain_Int.Add(1, data[0], new int[] { number + random, random }), maxnumber);
                //Battle_Tool.Obtain_Resources(data[0], i);
                break;
            case 2:
                //获得货币
                Battle_Tool.Obtain_Unit((currency_unit)Enum.Parse(typeof(currency_unit), data[0]), i,2);
                break;
            case 3:
                //获得皮肤
                SumSave.old_crt_hero.hero_value += (SumSave.old_crt_hero.hero_value == "" ? "" : ",") + data[0];
                Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_hero, new string[] { Battle_Tool.GetStr(SumSave.old_crt_hero.hero_value) },
                    new string[] { "hero_value" });
                break;
            case 4:
                //获得灵气

                Battle_Tool.Obtain_Unit(currency_unit.灵气, i);

                //SumSave.crt_world.Set(i);
                //SumSave.crt_world.AddValue_lists(i);
                //Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_world, SumSave.crt_world.Set_Uptade_String(), SumSave.crt_world.Get_Update_Character());
                break;
              


        }


    }


}
