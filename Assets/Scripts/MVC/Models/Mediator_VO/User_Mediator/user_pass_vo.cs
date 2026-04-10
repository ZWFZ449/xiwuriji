using Common;
using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class user_pass_vo : Base_VO
{
    /// <summary>
    /// 等级
    /// </summary>
    public int lv;
    /// <summary>
    /// 第几个通行证
    /// </summary>
    public int pass_index = 0;
    /// <summary>
    /// 奖励
    /// </summary>
    public string reward;
    /// <summary>
    /// 进阶奖励
    /// </summary>
    public string uplv_reward;
    /// <summary>
    /// 等级
    /// </summary>
    public int data_lv;
    /// <summary>
    /// 进阶奖励
    /// </summary>
    public int data_uplv;
    /// <summary>
    /// 经验值
    /// </summary>
    public int data_exp;

    /// <summary>
    /// 累积完成任务
    /// </summary>
    public int Max_task_number;
    /// <summary>
    /// 任务状态
    /// </summary>
    public string day_state_value;
    /// <summary>
    /// 每日任务状态
    /// </summary>
    public List<int> day_state;
    /// <summary>
    /// 用户领取状态
    /// </summary>
    public List<int> data_day_state = new List<int>();
    ///领取状态
    private Dictionary<int,List<int>> dic_user_values = new Dictionary<int, List<int>>();
    public string user_value;
    public void Init()
    {
        string[] str = user_value.Split('|');
        day_state = new List<int>();
        data_day_state = new List<int>();
        for (int i = 0; i < str.Length; i++)
        {
            string[] str1 = str[i].Split(',');
            if (str1.Length > 1)
            {
                if(!dic_user_values.ContainsKey(int.Parse(str1[0])))dic_user_values.Add(int.Parse(str1[0]), new List<int>());

                for (int j = 1; j < str1.Length; j++)
                { 
                    if(str1[j]!="")
                    dic_user_values[int.Parse(str1[0])].Add(int.Parse(str1[j]));
                }
            }
        }
        str = day_state_value.Split('|');
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] != "")
            {
                data_day_state.Add(int.Parse(str[i]));
                day_state.Add(0);
            }
        }
    }



    /// <summary>
    /// 返回用户领取状态
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, List<int>> Set()
    {
        return dic_user_values;

    }

    public override void MysqlData()
    {
        base.MysqlData();
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_pass, SumSave.crt_pass.Set_Uptade_String(), SumSave.crt_pass.Get_Update_Character());
    }
    /// <summary>
    /// 清空通行证每日任务
    /// </summary>
    public void clear_data()
    {
        for (int i = 0; i < data_day_state.Count; i++)
        {
            data_day_state[i] = 0;
        }
        MysqlData();
    }


    /// <summary>
    /// 获取任务状态
    /// </summary>
    /// <returns></returns>
    public List<int> Get_day_state()
    { 
     return data_day_state;
    }
    /// <summary>
    /// 设置用户领取状态
    /// </summary>
    /// <param name="list"></param>
    public void Get(Dictionary<int, List<int>> list)
    {
        dic_user_values = list;
        MysqlData();
    }
    /// <summary>
    /// 写入状态
    /// </summary>
    /// <param name="list"></param>
    public void Get(int index)
    {
        data_day_state[index] = -1;
        MysqlData();
    }

    public void progress(int index)
    { 
        if(data_day_state[index]!=-1)
        data_day_state[index]++;
        MysqlData();
    }

    /// <summary>
    /// 写入领取
    /// </summary>
    /// <returns></returns>
    private string Set_data()
    {
        string dec = "";

        foreach (var item in dic_user_values.Keys)
        {
            if (dec != "") dec += "|";
            dec += item + ",";
            foreach (var list in dic_user_values[item])
            {
                dec+= list + ",";
            }
        }


        return dec;
    }

    /// <summary>
    /// 写入每日任务
    /// </summary>
    /// <returns></returns>
    private string Set_day_state_value()
    {
        string dec = "";

        for (int i = 0; i < data_day_state.Count; i++)
        {
            dec += (dec == "" ? "" : "|") + data_day_state[i];
        }
        return dec;
    }

    public override string[] Set_Instace_String()
    {
        return new string[]
        {
        GetStr(0),
        GetStr(SumSave.crt_user.uid),
        GetStr(data_lv),
        GetStr(data_exp),
        GetStr(Max_task_number),
        GetStr(Set_data()),
        GetStr(Set_day_state_value())
        };
    }

    public override string[] Get_Update_Character()
    {
        return new string[]
        {
        "pass_lv",
        "pass_exp",
        "Max_task_number",
        "user_value",
        "day_state_value"
        };
    }

    public override string[] Set_Uptade_String()
    {
        return new string[]
        {
        GetStr(data_lv),
        GetStr(data_exp),
        GetStr(Max_task_number),
        GetStr(Set_data()),
        GetStr(Set_day_state_value())
        };
    }
}
