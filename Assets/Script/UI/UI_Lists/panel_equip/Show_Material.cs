using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 显示材料
/// </summary>
public class Show_Material : Base_Mono
{
    /// <summary>
    /// 材料名字 信息
    /// </summary>
    private Text show_name, base_info;
    /// <summary>
    /// 使用 丢弃
    /// </summary>
    private Button confirm, discard;
    /// 成品数据
    /// </summary>
    private (string, List<string>) data_seedmaterial;
    /// 丹方数据
    /// </summary>
    private (string, List<string>) data_material;
    /// <summary>
    /// 技能书
    /// </summary>
    private (string,int) data;

    private material_item material_item_Prefabs;

    private Transform pos_icon;
    /// <summary>
    /// 2丹方1丹药
    /// </summary>
    private int type = 1;

    private void Awake()
    {
        show_name=Find<Text>("show_name/info");
        base_info = Find<Text>("base_info/info");
        confirm = Find<Button>("btn_list/confirm");
        discard = Find<Button>("btn_list/discard");
        confirm.onClick.AddListener(() => { Confirm(); });
        discard.onClick.AddListener(() => { Discard(); });
        pos_icon = Find<Transform>("iocn");
        material_item_Prefabs = Battle_Tool.Find_Prefabs<material_item>("material_item"); //Resources.Load<material_item>("Prefabs/panel_bag/material_item");
    }
    /// <summary>
    /// 丢弃
    /// </summary>
    private void Discard()
    {
        if (data.Item2 > 0)
        {
            Dictionary<string,int> dic = new Dictionary<string, int>();
            dic.Add(data.Item1, -data.Item2);
            SumSave.crt_bag_resources.Get(dic,1);
            Game_Omphalos.i.Wirte_ResourcesList(Emun_Resources_List.material_value, SumSave.crt_bag_resources.GetData());
        }
        else
        {
            SumSave.crt_seeds.usedata(type == 2 ? data_seedmaterial : data_material, type);
            Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_seed, SumSave.crt_seeds.Set_Uptade_String(), SumSave.crt_seeds.Get_Update_Character());
        }
        Alert_Dec.Show("丢弃成功");
        hide();
    }
    /// <summary>
    /// 关闭
    /// </summary>
    private void hide()
    {
        transform.parent.parent.SendMessage("Refresh");
    }

    /// <summary>
    /// 确认
    /// </summary>
    private void Confirm()
    {
        if (data.Item2 > 0)
        {
            Bag_Base_VO bag = ArrayHelper.Find(SumSave.db_stditems, e => e.Name == data.Item1);
            switch ((EquipConfigTypeList)Enum.Parse(typeof(EquipConfigTypeList), bag.StdMode))
            {
                case EquipConfigTypeList.秘笈:
                case EquipConfigTypeList.战斗技能:
                case EquipConfigTypeList.特殊技能:
                    foreach (var item in SumSave.oldcrt_skills)
                    {
                        if (item.skillname == bag.Name)
                        { 
                            Alert_Dec.Show("已拥有该技能");
                            tool_Categoryt.Base_Task(1003);
                            return;
                        }
                    }
                    SumSave.oldcrt_skills.Add(tool_Categoryt.crate_skill(data.Item1));//添加技能
                    Game_Omphalos.i.Wirte_ResourcesList(Emun_Resources_List.skill_value, SumSave.oldcrt_skills);
                    Alert_Dec.Show("获得技能 " + data.Item1);
                    break;
                case EquipConfigTypeList.宠物技能:
                    break;
                default:
                    break;
            }
            Dictionary<string, int> dic = new Dictionary<string, int>();
            dic.Add(data.Item1, -data.Item2);
            SumSave.crt_bag_resources.Get(dic,1);
            Game_Omphalos.i.Wirte_ResourcesList(Emun_Resources_List.material_value, SumSave.crt_bag_resources.GetData());
        }
        else
        {
            (string, List<int>) data = new(data_seedmaterial.Item1, new List<int>());
            data.Item2.Add(1);
            data.Item2.Add(int.Parse(data_seedmaterial.Item2[1]));
            SumSave.crt_seeds.usedata(data_seedmaterial, type);
            SumSave.crt_seeds.Setuse(data);
            SendNotification(NotiList.Refresh_Max_Hero_Attribute);
        }
        Alert_Dec.Show("使用成功");
        hide();
    }
    /// <summary>
    /// 查看配方
    /// </summary>
    /// <param name="data_formulamaterial"></param>
    public void Init((string, List<string>) data)
    {
        Init_Show(false);
        Instance_Show(data.Item1);
        data_material = data;
        type = 1;
        discard.gameObject.SetActive(true);
        show_name.text = data.Item1;
        base_info.text = "丹方等级" + "Lv." + data.Item2[0]
            + "\n" + "丹方配表 " + data.Item2[1];
    }
    /// <summary>
    /// 初始化 材料背包
    /// </summary>
    /// <param name="bag_Resources"></param>
    public void Init((string, int) bag_Resources)
    {
        Init_Show(false);
        Instance_Show(bag_Resources.Item1);
        Bag_Base_VO bag = ArrayHelper.Find(SumSave.db_stditems, e => e.Name == bag_Resources.Item1);
        show_name.text = bag.Name;
        base_info.text = bag.dec;
        base_info.text += "\n存量 ： " + bag_Resources.Item2;

        switch ((EquipConfigTypeList)Enum.Parse(typeof(EquipConfigTypeList), bag.StdMode))
        {
            case EquipConfigTypeList.秘笈:
            case EquipConfigTypeList.宠物技能:
            case EquipConfigTypeList.战斗技能:
            case EquipConfigTypeList.特殊技能:
                data = bag_Resources;
                Init_Show(true);
                break;
            case EquipConfigTypeList.宠物蛋:
                data = bag_Resources;
                discard.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 初始化 丹药背包
    /// </summary>
    /// <param name="data"></param>
    public void Init_Seed((string, List<string>) data)
    {
        data_seedmaterial = data;
        Instance_Show(data.Item1);
        type = 2;
        Init_Show(true);
        show_name.text = data.Item1;
        db_seed_vo item = ArrayHelper.Find(SumSave.db_seeds, e => e.pill == data.Item1);
        string dec= "丹药效果" + "\n";
        dec += Show_Color.Red(item.type + " " + data.Item2[1]) + "(Max" + item.pill_effect.Split(' ')[1] + ")"
        + "\n丹药创造时间" + data.Item2[0];
        List<(string,List<int>)> list = SumSave.crt_seeds.GetuseList();
        int limit = Battle_Tool.Alchemy_limit(item.limit);
        bool exsit = true;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Item1 == data.Item1)
            {
                exsit= false;
                dec += "\n" + "累积效果 " + item.type + " " + list[i].Item2[1] ;
                dec += "\n" + "剩余次数 " + list[i].Item2[0]+"/"+ limit;
                //是否还可以吃
                confirm.gameObject.SetActive(list[i].Item2[0] < limit);
            }
        }
        if (exsit)
        {
            dec += "\n" + "累积效果 " + item.type + " " + 0;
            dec += "\n" + "剩余次数 " + 0 + "/" + limit;
        }

        base_info.text = dec;
    }
    /// <summary>
    /// 显示基础信息
    /// </summary>
    /// <param name="name"></param>
    private void Instance_Show(string name )
    { 
        Instantiate(material_item_Prefabs, pos_icon).Init((name, 1));
    }
    /// <summary>
    /// 显示按键
    /// </summary>
    /// <param name="exist"></param>
    private void Init_Show(bool exist)
    {
        confirm.gameObject.SetActive(exist);
        discard.gameObject.SetActive(exist);
    }

    public override void Show()
    {
        base.Show();
    }
}
