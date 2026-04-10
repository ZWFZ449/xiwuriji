using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 炼丹
/// </summary>
public class Alchemy : Base_Mono
{
    /// <summary>
    /// 权重列表
    /// </summary>
    private List<WeightedItem> eighteditems = new List<WeightedItem>();
    /// <summary>
    /// 位置列表
    /// </summary>
    private Dictionary<string, Transform> pos_list = new Dictionary<string, Transform>();
    /// <summary>
    /// 传入显示标准
    /// </summary>
    private Slider slider;
    /// <summary>
    /// 显示标准值
    /// </summary>
    private Text slider_info;
    /// <summary>
    /// 选择配方
    /// </summary>
    private int select_formula = 0;
    /// <summary>
    /// 位置
    /// </summary>
    private Transform pos_materials;
    /// <summary>
    /// 确认按钮
    /// </summary>
    private Button confirm;
    /// <summary>
    /// 获取列表
    /// </summary>
    private Dropdown lists;

    private List<string> Dropdown_lists = new List<string>();

    private material_item material_item_Prefabs;
    /// <summary>
    /// 选择配方材料
    /// </summary>
    private Dictionary<string,(string,int)> Select_Materials = new Dictionary<string, (string, int)>();
    /// <summary>
    /// 显示属性
    /// </summary>
    private Button Lottery_attribute;

    private effect_gather effect_gather;
    /// <summary>
    /// 显示成功率
    /// </summary>
    private Button show_Success_rate;
    private void Awake()
    {
        pos_materials=Find<Transform>("Scroll View/Viewport/Content");
        material_item_Prefabs = Battle_Tool.Find_Prefabs<material_item>("material_item");// Resources.Load<material_item>("Prefabs/panel_bag/material_item");
        lists = Find<Dropdown>("list");
        confirm = Find<Button>("confirm");
        confirm.onClick.AddListener(() => { btn_Confirm(); });
        effect_gather=Find<effect_gather>("effect_gather");
        effect_gather.gameObject.SetActive(false);
        slider=Find<Slider>("Slider");
        slider_info = Find<Text>("Slider/info");
        slider.onValueChanged.AddListener((value) => { Open_Slider(value); });
        slider.gameObject.SetActive(false);
        show_Success_rate = Find<Button>("offect/show_Success_rate");
        show_Success_rate.onClick.AddListener(() => { Show_Success_rate(); });
        show_Success_rate.gameObject.SetActive(false);
        Lottery_attribute= Find<Button>("Lottery_attribute");
        Lottery_attribute.onClick.AddListener(() => { show_info(); });
    }
    /// <summary>
    /// 显示属性
    /// </summary>
    private void show_info()
    {
        string dec = "";
        List<(string, List<int>)> list = SumSave.crt_seeds.GetuseList();
        foreach (var item in SumSave.db_seeds)
        {
            bool eixst = true;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Item1 == item.pill)
                {
                    eixst = false;
                    dec += "\n" + item.type + " " + Show_Color.Red(list[i].Item2[1]) + "(" + list[i].Item2[0] + "/" + Battle_Tool.Alchemy_limit(item.limit) + ")";
                }
            }
            if (eixst)
            {
                dec += Show_Color.Grey("\n" + item.type + " " + 0 + "(" + 0 + "/" + Battle_Tool.Alchemy_limit(item.limit) + ")");
            }
        }
       
        Alert.Show("丹药属性", dec);
    }

    /// <summary>
    /// 显示成功率
    /// </summary>
    private void Show_Success_rate()
    {
        string dec = "";
        if (select_formula == 0)
        {
            WeightedRandomPicker picker = new WeightedRandomPicker(eighteditems);
            dec = picker.Show_Success_rate();
        }
        else
        {
            (string, List<string>) formula = SumSave.crt_seeds.Getformulalist()[select_formula - 1];
            db_seed_vo item = ArrayHelper.Find(SumSave.db_seeds, e => e.seed_formula == formula.Item1);
            dec= item.pill + ":获取概率 " + 100+"%\n";
        }
        Alert.Show("炼丹概率", dec);

    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="value"></param>
    private void Open_Slider(float value)
    {
        slider_info.text = "可注灵天麻 " + slider.value + "（Max:" + slider.maxValue + ")";
    }

    /// <summary>
    /// 合成
    /// </summary>
    private void btn_Confirm()
    {
        foreach (var item in Select_Materials.Keys)
        {
            if (Select_Materials[item].Item2 == 0)
            {
                Alert_Dec.Show("请选择所有材料");
                return;
            }
        }
        SendNotification(NotiList.Read_Mysql_Base_Time);
        if (SumSave.openMysql)
        {
            Alert_Dec.Show("网络连接失败");
            return;
        } 
        foreach (var item in Select_Materials.Keys)
        {
            NeedConsumablesold(Select_Materials[item].Item1, Select_Materials[item].Item2);
        }
        if (slider.value > 0)
        { 
            NeedConsumablesold("天麻", (int)slider.value);
        }
        if (RefreshConsumables_old())
        {
            effect_gather.gameObject.SetActive(true);
            effect_gather.OpenEffect_Gather((int)(slider.value * 10 / (slider.maxValue + 1)) + 1);
        }
        else
            Alert_Dec.Show("材料不足");
    }

    protected void Confirm_Number(int number)
    {
        synthesis(number);
        base_Show();
    }
    /// <summary>
    /// 判断合成
    /// </summary>
    /// <param name="number"></param>
    private void synthesis(int number)
    {
        //number = 7;
        // 创建一个加权随机选择器
        WeightedRandomPicker picker = new WeightedRandomPicker(eighteditems);
        // 获取一个随机物品
        WeightedItem selectedItem = picker.GetRandomItem();
        List<string> split = new List<string>();
        db_seed_vo item = ArrayHelper.Find(SumSave.db_seeds, e => e.pill == selectedItem.prizedraw);
        int lv = number;
        if (select_formula > 0)
        { 
            (string, List<string>) formula = SumSave.crt_seeds.Getformulalist()[select_formula - 1];
            item = ArrayHelper.Find(SumSave.db_seeds, e => e.seed_formula == formula.Item1);
            lv += int.Parse(formula.Item2[0]);
            //移除丹方
            SumSave.crt_seeds.usedata(formula);
        }
        lv *= 10;
        lv = lv > 100 ? 100 : lv;//最大100
        if (number >= 5)
        {
            (string, List<string>) formula = new (item.seed_formula, new List<string>());
            formula.Item2.Add((lv / 10).ToString());
            string dec = "";
            foreach (var base_name in eighteditems)
            {
                dec += (dec == "" ? "" : " ") + ArrayHelper.Find(SumSave.db_seeds, e => e.pill == base_name.prizedraw).seed_name;
            }
            formula.Item2.Add(dec);
            //写入丹方
            SumSave.crt_seeds.Setformula(formula);
            Alert_Dec.Show("获得 " + (lv / 10) + " 级丹方 " + item.seed_formula);
        }
        split = crate_seed(item, split, lv);
        SumSave.crt_seeds.Set(split);
        Alert_Dec.Show("获得丹药 " + split[0]);
        SumSave.crt_pass.progress(5);
        AlchemyMission(split);
        Game_Omphalos.i.archive();
    }

    /// <summary>
    /// 完成炼丹任务
    /// </summary>
    private void AlchemyMission(List<string> split)
    {
        if(split[0]== "天麻丸")
        {
            tool_Categoryt.Base_Task(1029);
            tool_Categoryt.Base_Task(1092);
        }

    }
    /// <summary>
    /// 生成配方 
    /// </summary>
    /// <param name="item"></param>
    /// <param name="split"></param>
    private List<string> crate_seed(db_seed_vo item, List<string> split,int lv)
    {
        split.Add(item.pill);
        //split.Add(item.seed_name); 
        split.Add(SumSave.nowtime.ToString());
        string[] split1 = item.pill_effect.Split(' ');
        int random = int.Parse(split1[0]) + ((int.Parse(split1[1]) - int.Parse(split1[0])) * lv / 100);
        split.Add(random.ToString());
        return split;
    }

    public override void Show()
    {
        base.Show();
        base_Show();
    }
    /// <summary>
    /// 获取位置
    /// </summary>
    /// <param name="pos"></param>
    protected void Init_Pos(Alchemy_button_pos_item pos)
    {
        pos_list.Add(pos.index.ToString(), pos.transform);
        Select_Materials.Add(pos.index.ToString(), (string.Empty, 0));
    }
    private void base_Show()
    {
        for (int i = pos_materials.childCount - 1; i >= 0; i--)//清空区域内按钮
        {
            Destroy(pos_materials.GetChild(i).gameObject);
        }
        //清空列表
        foreach (var item in pos_list.Keys)
        {
            for (int i = 0; i < pos_list[item].childCount; i++)
            {
                Destroy(pos_list[item].GetChild(i).gameObject);
            }
        }
        
        Dropdown_lists.Clear();
        lists.options.Clear();
        eighteditems.Clear();
        foreach (var item in pos_list.Keys)
        {
            Select_Materials[item]= (item, 0);
        }
        Dropdown_lists.Add("请选择配方");
        List<(string, int)> list = new List<(string, int)>();
        list=SumSave.crt_bag_resources.Set();
        for (int i = 0; i < list.Count; i++)
        { 
           Bag_Base_VO item=ArrayHelper.Find(SumSave.db_stditems,e=> e.Name== list[i].Item1);
            if (item != null)
            {
                if (item.StdMode == EquipConfigTypeList.炼丹材料.ToString())
                {
                    if (list[i].Item2 > 0)
                    {
                        material_item item1 = Instantiate(material_item_Prefabs, pos_materials);
                        item1.Init(list[i]);
                        item1.GetComponent<Button>().onClick.AddListener(() => { btn_Select_Material(item1); });
                    }
                    
                }
            }
        }
        List<(string, List<string>)> formulalist = SumSave.crt_seeds.Getformulalist();
        if (formulalist.Count > 0)
        {
            for (int i = 0; i < formulalist.Count; i++)
            {
                Dropdown_lists.Add(formulalist[i].Item2[0] + "级" + formulalist[i].Item1);
            }
        }
        if (Dropdown_lists.Count > 0)
        {
            for (int i = 0; i < Dropdown_lists.Count; i++)
            { 
                lists.options.Add(new Dropdown.OptionData(Dropdown_lists[i]));
            }
        }
        lists.onValueChanged.AddListener(OnValueChange);
    }
    /// <summary>
    /// 选择配方
    /// </summary>
    /// <param name="arg0"></param>
    private void OnValueChange(int arg0)
    {
        Alert_Dec.Show("当前选择配方" + Dropdown_lists[arg0]);
        select_formula = arg0;
        Select_Formula();
    }
    /// <summary>
    /// 锁定配方
    /// </summary>
    private void Select_Formula()
    {
        foreach (string pos in pos_list.Keys)
        {
            for (int i = 0; i < pos_list[pos].childCount; i++)
            {
                Destroy(pos_list[pos].GetChild(i).gameObject);
            }
            Select_Materials[pos] = (pos, 0);
        }
        if (select_formula == 0)
        {
            return;
        }
        (string,List<string>) item = SumSave.crt_seeds.Getformulalist()[select_formula-1];
        string[] split = item.Item2[1].Split(' '); 
        List<string> list = new List<string>();
        for (int i = 0; i < split.Length; i++)
        {
            db_seed_vo seed = ArrayHelper.Find(SumSave.db_seeds, e =>e.seed_name == split[i]);
            if (seed != null)
            {
                Select_Materials[i.ToString()] = (seed.seed_name, 1);
                material_item material_Item = Instantiate(material_item_Prefabs, pos_list[i.ToString()]);
                material_Item.Init(Select_Materials[i.ToString()]);
            }
        }
        Show_Slider();
    }
    /// <summary>
    /// 权重表
    /// </summary>
    /// <param name="seed_name"></param>
    /// <param name="weight"></param>
    private void Obtain_Weight(string seed_name, int weight)
    {
        WeightedItem weightedItem = new WeightedItem(seed_name,weight);
        eighteditems.Add(weightedItem);
    }

    /// <summary>
    /// 选择材料
    /// </summary>
    /// <param name="item1"></param>
    private void btn_Select_Material(material_item item1)
    {
        if (select_formula == 0)
        {
            foreach (var item in Select_Materials.Keys)
            {
                if (Select_Materials[item].Item2 == 0)
                {
                    if (item1.GetItemData().Item2 >= 1)
                    {
                        Select_Materials[item] = (item1.GetItemData().Item1, 1);
                        if (pos_list.ContainsKey(item))
                        {
                            material_item material_Item = Instantiate(material_item_Prefabs, pos_list[item]);
                            material_Item.Init(Select_Materials[item]);
                            material_Item.GetComponent<Button>().onClick.AddListener(() => { close_Select_Material(item); });
                        }
                        item1.Init((item1.GetItemData().Item1, item1.GetItemData().Item2 - 1));
                        Show_Slider();
                        return;
                    }
                    else
                    {
                        Alert_Dec.Show("当前材料不足");
                        return;
                    } 
                }
            }
            Alert_Dec.Show("当前材料位已满");
        }
        else Alert_Dec.Show("当前已锁定配方");

    }
    /// <summary>
    /// 关闭材料
    /// </summary>
    /// <param name="material_Item"></param>
    private void close_Select_Material(string pos)
    {
        for (int i = 0; i < pos_list[pos].childCount; i++)
        { 
            Destroy(pos_list[pos].GetChild(i).gameObject);
        }
        for (int i = pos_materials.childCount - 1; i >= 0; i--)//清空区域内按钮
        {
            material_item item = pos_materials.GetChild(i).GetComponent<material_item>();
            if (item != null)
            { 
                if(item.GetItemData().Item1 == Select_Materials[pos].Item1)
                {
                    item.Init((item.GetItemData().Item1, item.GetItemData().Item2 + 1));
                    break;
                }
            }
        }
        Select_Materials[pos] = (pos, 0);
        slider.gameObject.SetActive(false);
        show_Success_rate.gameObject.SetActive(false);
    }

    /// <summary>
    /// 显示标记
    /// </summary>
    private void Show_Slider()
    {
        bool exist = true;
        int number = 0;
        foreach (var item in Select_Materials.Keys)
        {
            if (Select_Materials[item].Item2 == 0)
            {
                exist = false;
            }
        }
        if (exist)
        {
            eighteditems.Clear();
            foreach (var item in Select_Materials.Keys)
            {
                db_seed_vo seed = ArrayHelper.Find(SumSave.db_seeds, e => e.seed_name == Select_Materials[item].Item1);
                number += seed.seed_number;
                Obtain_Weight(seed.pill, seed.Weight);
            }
            show_Success_rate.gameObject.SetActive(true);
            slider.gameObject.SetActive(true);
            slider.maxValue = number;
            slider.value = 0;
            slider_info.text="可注灵天麻 "+slider.value+ "（Max:"+slider.maxValue+")";
        }
    }
}
