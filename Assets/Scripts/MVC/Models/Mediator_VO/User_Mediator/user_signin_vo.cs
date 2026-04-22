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
    private List<string> values = new List<string>();

    private Dictionary<string,int> dic = new Dictionary<string, int>();

    public int max_number;
    //public string user_value;
    public void Init(string user_value="")
    {
        if (user_value == "") return;
        List<string> values = ArrayHelper.Get_Split<string>(user_value, ';');
        if (values.Count > 0)
        {
            foreach (var item in values)
            { 
                List<string> list = ArrayHelper.Get_Split<string>(item, ' ');
                if (list.Count == 2)
                {
                    if (!dic.ContainsKey(list[0])) dic.Add(list[0], int.Parse(list[1]));
                }
            }
        }
    }
    /// <summary>
    /// 查看
    /// </summary>
    /// <returns></returns>
    public List<string> Set()
    { 
        return values;
    }
    /// <summary>
    /// 获取编号
    /// </summary>
    /// <param name="index"></param>
    public void Set(int index)
    {
        //if (index < values.Count)
        //    values[index] = 1;
        //else
        //{
        //    while (index < values.Count)
        //    {
        //        values.Add(0);
        //    }
        //    values[index] = 1;
        //}
    }
    /// <summary>
    /// 获取是否可以使用
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public int GetIsValue(string key)
    {
        if (dic.ContainsKey(key))
        {
            return dic[key];
        }
        else return 0;
    }
    /// <summary>
    /// 写入
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetIsValue(string key, int value)
    {
        if (!dic.ContainsKey(key))
        {
            dic.Add(key, value);
        }
        else
        { 
            dic[key] += value;
        }
        MysqlData();
    }

    private string DataSet()
    { 
        string value="";
        
        foreach (var item in dic)
        {
            value += item.Key + " " + item.Value + ";";
        }
        return value;
    }

    public void Clear()
    { 
        dic.Clear();
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
        GetStr(now_time.ToString()),
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
