using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class user_signin_vo : Base_VO
{
    /// <summary>
    /// 上次签到时间
    /// </summary>
    public DateTime now_time;
    /// <summary>
    /// 签到数量
    /// </summary>
    public int number;
    /// <summary>
    /// 是否领取奖励
    /// </summary>
    private List<int> values = new List<int>();
    public int max_number;
    public string user_value;
    public void Init()
    { 
    
        string[] str = user_value.Split(' ');
        if (str.Length > 0)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (!string.IsNullOrEmpty(str[i]))
                    values.Add(int.Parse(str[i]));
            }
        }
    }
    /// <summary>
    /// 查看
    /// </summary>
    /// <returns></returns>
    public List<int> Set()
    { 
        return values;
    }
    /// <summary>
    /// 获取编号
    /// </summary>
    /// <param name="index"></param>
    public void Set(int index)
    {
        if (index < values.Count)
            values[index] = 1;
        else
        {
            while (index < values.Count)
            {
                values.Add(0);
            }
            values[index] = 1;
        }
    }

    private string DataSet()
    { 
        string value="";
        for (int i = 0; i < values.Count; i++)
        { 
            value += values[i].ToString() + " ";
        }
        return value;
    }

    public override string[] Get_Update_Character()
    {
        return
            new string[]
            {
                "now_time",
                "number",
                "user_value",
                "max_number"
            };
    }

    public override string[] Set_Uptade_String()
    {
        return new string[]
            {
              GetStr(now_time.ToString("yyyy-MM-dd")),
              GetStr(number),
              GetStr(DataSet()),
              GetStr(max_number)

            };
    }

    public override string[] Set_Instace_String()
    {
        return new string[]
        {
        GetStr(0),
        GetStr(SumSave.crt_user.uid),
        GetStr(now_time),
        GetStr(number),
        GetStr(DataSet()),
        GetStr(max_number)
        };
    }

    public override void MysqlData()
    {
        base.MysqlData();
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.dream_user_signin, Set_Uptade_String(),
               Get_Update_Character());
    }

}
