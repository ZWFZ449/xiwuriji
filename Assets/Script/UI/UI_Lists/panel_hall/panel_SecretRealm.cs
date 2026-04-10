using Common;
using Components;
using MVC;
using System;
using System.Collections.Generic;
using System.Text;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class panel_SecretRealm : Base_Mono
{
    
    private Transform pos_crtmap;
    private copies_item copies_item_Prefabs;
    /// <summary>
    /// 战斗地图
    /// </summary>
    private panel_fight fight_panel;
    /// <summary>
    /// 信息显示面板
    /// </summary>
    private Transform base_show_info;

    /// <summary>
    /// 地图名字, 需要等级,怪物列表,门票
    /// </summary>
    private Text  map_name,monster_list, need_Required;
    /// <summary>
    /// 物品位置
    /// </summary>
    private Transform pos_life;
    /// <summary>
    /// 进入地图按钮,信息窗口关闭按钮
    /// </summary>
    private Button enter_map_button,Close_button;
    /// <summary>
    /// 当前选择地图
    /// </summary>
    private copies_item item;
    /// <summary>
    /// 物品信息预制体
    /// </summary>
    private material_item material_item_parfabs;
    /// <summary>
    /// 难度选择下拉列表
    /// </summary>
    private Dropdown dropdown;
    /// <summary>
    /// 掉落概率信息栏
    /// </summary>
    private Text drop_rate;
    /// <summary>
    /// 品质颜色
    /// </summary>
    private readonly string[] qualityColors = new string[]
   {
        "#FFFFFF", // 普通（灰色）
        "#00FF00", // 精良（绿色）
        "#00B4FF", // 完美（天蓝）
        "#800080", // 史诗（紫色）
        "#FFA500", // 传说（橙色）
        "#FF0000", // 神话（红色）
        "#FFFF00"  // 绝世（金色）
   };

    private void Awake()
    {
        pos_crtmap = Find<Transform>("Scroll View/Viewport/Content");
        copies_item_Prefabs = Battle_Tool.Find_Prefabs<copies_item>("copies_item");
        fight_panel = UI_Manager.I.GetPanel<panel_fight>();
        map_name= Find<Text>("Difficulty_info/map_name/map_name_text");

        base_show_info = Find<Transform>("Difficulty_info");

        monster_list = Find<Text>("Difficulty_info/monster_list");
        need_Required = Find<Text>("Difficulty_info/need_Required");
        pos_life = Find<Transform>("Difficulty_info/ProfitList/Scroll View/Viewport/Content");
        enter_map_button = Find<Button>("Difficulty_info/enter_map_button");
        material_item_parfabs = Battle_Tool.Find_Prefabs<material_item>("material_item");
        enter_map_button.onClick.AddListener(()=>confirm());

        dropdown= Find<Dropdown>("Difficulty_info/difficulty_selection/Dropdown");
        drop_rate = Find<Text>("Difficulty_info/difficulty_selection/Scroll View/Viewport/Content/Intr");
        Close_button= Find<Button>("Difficulty_info/Close");
        Close_button.onClick.AddListener(()=>base_show_info.gameObject.SetActive(false));
        // 清空现有选项
        dropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < Enum.GetNames(typeof(enum_equip_quality_list)).Length; i++)
        {
            options.Add(Enum.GetNames(typeof(enum_equip_quality_list))[i]); 
        }
        // 添加新选项
        dropdown.AddOptions(options);
        dropdown.onValueChanged.AddListener(OnValueChanged);
        //设置为最低级难度
        dropdown.value = 0;
        OnValueChanged(dropdown.value);
    }



    /// <summary>
    /// 选择难度
    /// </summary>
    /// <param name="arg0"></param>

    private void OnValueChanged(int arg0)
    {
        ///获得装备品质枚举
        var enumNames = Enum.GetNames(typeof(enum_equip_quality_list));
        ///获得下拉列表的值
        var selectedValue = dropdown.value;
        ///判断是否为最后一个选项
        bool isLastOption = selectedValue == enumNames.Length - 1;
        ///创建一个StringBuilder对象来构建字符串
        var stringBuilder = new StringBuilder();

        for (int i = 0; i < enumNames.Length; i++)
        {
            string percentage = GetPercentage(selectedValue, i, isLastOption);
            string colorTag = qualityColors.Length > i ? $"<color={qualityColors[i]}>" : "";
            string colorCloseTag = qualityColors.Length > i ? "</color>" : "";

            stringBuilder.Append($"{colorTag}{enumNames[i]}: {percentage}{colorCloseTag}\n");
        }

        drop_rate.text = stringBuilder.ToString();
    }
    /// <summary>
    /// 获得对应概率
    /// </summary>
    /// <param name="selectedValue"></param>
    /// <param name="currentIndex"></param>
    /// <param name="isLastOption"></param>
    /// <returns></returns>
    private string GetPercentage(int selectedValue, int currentIndex, bool isLastOption)
    {
        if (isLastOption)
        {
            return selectedValue == currentIndex ? "10%" : "0%";
        }

        if (selectedValue == currentIndex) return "50%";
        if (selectedValue - 1 == currentIndex) return "45%";
        if (selectedValue + 1 == currentIndex) return "5%";

        return "0%";
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="item"></param>
    private void OnClick(copies_item _item)
    {
        item=_item;
        base_show_info.transform.gameObject.SetActive(true);
        InitInfo(item.index);
    }


    /// <summary>
    /// 初始化副本信息
    /// </summary>
    private void InitInfo(user_map_vo map)
    {
        monster_list.text = "怪物列表： " + map.monster_list.ToString();
        need_Required.text = "门票要求： " + map.need_Required.ToString();
        map_name.text = map.map_name;
        for (int i = pos_life.childCount - 1; i >= 0; i--)//清空区域内按钮
        {
            Destroy(pos_life.GetChild(i).gameObject);
        }
        foreach (string str in map.ProfitList.Split('&'))
        {
            string[] str1 = str.Split(' ');
            material_item item = Instantiate(material_item_parfabs, pos_life);
            item.Init(((str1[0]), 0));
            item.GetComponent<Button>().onClick.AddListener(delegate { Alert.Show(str1[0], str1[0]); });
        }
    }

    /// <summary>
    /// 确认进入
    /// </summary>
    /// <param name="arg0"></param>
    private void confirm()
    {
        if(item== null)
        {
            return;
        }

        if (item.index.need_Required != "")
        {
            NeedConsumablesold(item.index.need_Required, 1);
            if (RefreshConsumables_old())
            {
                Open_Map(item);
                base_Show();
            }
            else
            {
                Alert_Dec.Show("挑战门票不足");
            }
        }
        else
        {
            Alert_Dec.Show("副本暂未开发");
        }

    }

    /// <summary>
    /// 进入地图
    /// </summary>
    /// <param name="item"></param>
    private void Open_Map(copies_item item)
    {
        fight_panel.Show();
        fight_panel.Open_SecretRealm(item.index, dropdown.value);
        item.updatestate();
    }

   
    public override void Show()
    {
        base.Show();
        //transform.gameObject.SetActive(false);
        //Alert_Dec.Show("秘境即将开放");
        //return;
        if (SumSave.crt_MaxHero_okd.Lv < 30 && SumSave.ios_account_number != "admin001")
        {
            Alert_Dec.Show("秘境开启等级为30级");
            gameObject.SetActive(false);
            return;
        }
        base_show_info.transform.gameObject.SetActive(false);
        base_Show();
    }
    /// <summary>
    /// 初始化
    /// </summary>
    private void base_Show()
    {
        ClearObject(pos_crtmap);
        List<(string, int)> list = SumSave.crt_needlist.SetMap();
        
        for (int i = SumSave.read_lose_map.Count - 1; i > 0; i--)
        {
            if (SumSave.read_lose_map[i].map_type == 8 && SumSave.old_crt_hero.hero_Lv >= SumSave.read_lose_map[i].need_lv)
            {
                int number = 0;
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[j].Item1 == SumSave.read_lose_map[i].map_name)
                    {
                        number = list[j].Item2;
                        break;
                    }
                }
                copies_item item = Instantiate(copies_item_Prefabs, pos_crtmap);
                List<(string, int)> _list = SumSave.crt_bag_resources.Set();
                int num = 0;
                 for (int j = 0; j < _list.Count; j++)
                {
                    if (_list[j].Item1 == SumSave.read_lose_map[i].need_Required)
                    {
                        num= _list[j].Item2;
                    }
                }
                item.InitSecretRealm(SumSave.read_lose_map[i],num);
                item.GetComponent<Button>().onClick.AddListener(() => { OnClick(item); });
            }
        }
    }
}
