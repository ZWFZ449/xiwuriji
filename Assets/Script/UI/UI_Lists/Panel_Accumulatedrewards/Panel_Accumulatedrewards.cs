using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;
using MVC;
using Components;
using Random = UnityEngine.Random;
/// <summary>
/// 累积奖励
/// </summary>
public class Panel_Accumulatedrewards : Panel_Base
{
    private Transform pos_list;
    private Text base_info, type_info;
    private reward_item reward_item_prefabs;
    /// <summary>
    /// 奖励类型
    /// </summary>
    private int type;
    public override void Hide()
    {
        base.Hide();
    }

    public override void Initialize()
    {
        base.Initialize();
        pos_list = Find<Transform>("bg/Scroll View/Viewport/Content");
        type_info = Find<Text>("bg/type/info");
        base_info = Find<Text>("bg/base_info/info");
        reward_item_prefabs=Battle_Tool.Find_Prefabs<reward_item>("reward_item");
    }
    public override void Show()
    {
        base.Show();
    }

    public void Init(int index)
    {
        type= index;
        if (index == 1)
        {
            type_info.text = "累积通行证奖励";
            base_info.text = "累积通行证次数 " + SumSave.crt_pass.Max_task_number;
        }
        else
        {
            if (index == 2)
            {
                type_info.text = "累积签到奖励";
                base_info.text = "累积签到天数 " + SumSave.crt_signin.max_number;
            }
        }
        GetList(index);
    }
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="index"></param>
    private void GetList(int index)
    {
        ClearObject(pos_list);
        Dictionary<int, List<int>> dic = SumSave.crt_accumulatedrewards.Set();
        List<int> list = new List<int>();
        list.Add(0);
        bool exist = true;
        if (dic.ContainsKey(index))
        {
            list= dic[index];
        }else dic.Add(index, list);
        if (index==1)
        {
            for (int i = 0; i < SumSave.db_Accumulatedrewards.pass_list.Count; i++)
            {
                exist = true;
                if (list.Count > i)
                {
                    if (list[i] == 1) exist = false;
                }
                reward_item item = Instantiate(reward_item_prefabs, pos_list);

                item.Init(i, SumSave.db_Accumulatedrewards.pass_list[i], exist, index);
            }
        }else
        if (index == 2)
        {
            for (int i = 0; i < SumSave.db_Accumulatedrewards.signin_list.Count; i++)
            {
                exist = true;
                if (list.Count > i)
                {
                    if (list[i] == 1) exist = false;
                }
                reward_item item = Instantiate(reward_item_prefabs, pos_list);
                item.Init(i, SumSave.db_Accumulatedrewards.signin_list[i], exist, index);
            }
        }
    }
    /// <summary>
    /// 奖励
    /// </summary>
    /// <param name="index"></param>
    protected void Reward(int index)
    {
        Dictionary<int, List<int>> dic = SumSave.crt_accumulatedrewards.Set();
        if (type == 1)
        {
            if (index < SumSave.db_Accumulatedrewards.pass_list.Count)
            {
                //满足领取条件
                if (SumSave.crt_pass.Max_task_number >= SumSave.db_Accumulatedrewards.pass_list[index].Item1)
                {
                    operate(SumSave.db_Accumulatedrewards.pass_list, dic, index);
                }
                else Alert_Dec.Show("未满足领取条件");
            }
        }
        if (type == 2)//签到
        {
            if (index < SumSave.db_Accumulatedrewards.signin_list.Count)
            {
                //满足领取条件
                if (SumSave.crt_signin.max_number >= SumSave.db_Accumulatedrewards.signin_list[index].Item1)
                {
                    operate(SumSave.db_Accumulatedrewards.signin_list, dic, index);
                }else Alert_Dec.Show("未满足领取条件");
            }
        }

    }
    /// <summary>
    /// 数据判断
    /// </summary>
    /// <param name="value"></param>
    /// <param name="dic"></param>
    /// <param name="index"></param>
    private void operate(List<(int, string)> value, Dictionary<int, List<int>> dic,int index)
    {
        List<int> list = new List<int>();
        if (dic.ContainsKey(type))
        {
            list = dic[type];
        }
        else dic.Add(type, list);
        while (list.Count <= index)
        {
            list.Add(0);
        }
        list[index] = 1;
        dic[type] = list;
        SetData(dic);
        Receive_Reward(value[index].Item2);
        GetList(type);
    }
    /// <summary>
    /// 领取奖励
    /// </summary>
    /// <param name="value"></param>
    private void Receive_Reward(string value)
    {
        List<(string, int)> dic = new List<(string, int)>();
        string[] list= value.Split(',');
        foreach (var item in list)
        {
            if (item == "") continue;
            string [] str = item.Split(' ');
            dic.Add((str[1], int.Parse(str[2])));
            switch (int.Parse(str[0]))
            {
                case 2://货币
                    Battle_Tool.Obtain_Unit
                    ((currency_unit)Enum.Parse(typeof(currency_unit), str[1]), int.Parse(str[2]));
                    break;
                case 1://道具
                    int random = Random.Range(1, 100);
                    int number = int.Parse(str[2]);
                    int maxnumber = number + Random.Range(1, 100);
                    Battle_Tool.Obtain_Resources(Obtain_Int.Add(1, str[1], new int[] { number + random, random }), maxnumber);
                    //Battle_Tool.Obtain_Resources(str[1], int.Parse(str[2]));
                    break;
                case 3://次数礼包
                    if (str[1] == "次数福利礼包")
                    {
                        dic.Add(("魔丸", 200));
                        dic.Add(("荣耀点", 50));
                        SumSave.crt_accumulatedrewards.Set(2, 50);
                        Battle_Tool.Obtain_Unit(currency_unit.Boss积分, 200);
                    }
                    else
                    if (str[1] == "新手福利礼包")
                    {
                        dic.Add(("魔丸", 300));
                        dic.Add(("荣耀点", 100));
                        SumSave.crt_accumulatedrewards.Set(2, 100);
                        Battle_Tool.Obtain_Unit(currency_unit.Boss积分, 300);
                    }
                    break;
                default:
                    break;
            }
        }
        Alert_Icon.Show(dic);
    
    }
    private void SetData(Dictionary<int, List<int>> dic)
    {
        SumSave.crt_accumulatedrewards.Set(dic);
    }

    protected override void Awake()
    {
        base.Awake();
    }
}
