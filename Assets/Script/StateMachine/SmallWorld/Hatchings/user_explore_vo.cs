using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class user_explore_vo : Base_VO
{
    /// <summary>
    /// 0 名字+" "+ 孵化时间为锚点，1开始时间 2收益节点 3收益（名称+数量）
    /// </summary>
    private Dictionary<string,string> dic;
    public string user_value;
    public void Init()
    { 
        dic = new Dictionary<string, string>();
        string[] str = user_value.Split('&');
        if (str.Length > 0)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != "")
                { 
                    string[] strs = str[i].Split('|');
                    if (strs.Length > 1)
                    { 
                        dic.Add(strs[0], strs[1]);
                    }
                }
            }
        }
    }

    public Dictionary<string, string> Set()
    {
        return dic;
    }
    /// <summary>
    /// 写入数据
    /// </summary>
    /// <param name="index"></param>
    /// <param name="value"></param>
    public void SetValues(string index,string value)
    {
        if (dic.ContainsKey(index))
        {
            dic[index] = value;
        }
        else
        { 
            dic.Add(index, value);
        }
    }
    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="index"></param>
    public void DeleteValues(string index)
    {
        if (dic.ContainsKey(index))
        { 
           dic.Remove(index);
        }
    }

    public override string[] Get_Update_Character()
    {
        return new string[] { "user_value" };
    }


    public override string[] Set_Uptade_String()
    {
        return new string[] { GetStr(data_Set()) };
    }
    /// <summary>
    /// 写入数据
    /// </summary>
    /// <returns></returns>
    private string data_Set()
    {
        string value = "";
        foreach (var item in dic.Keys)
        {
            value += (value == "" ? "" : "&") + item + "|" + dic[item];
        }
        return value;
    }

    public override string[] Set_Instace_String()
    {
        return
            new string[]
            { GetStr(0),
            GetStr(SumSave.crt_user.uid),
            GetStr(data_Set())
            };
    }
}
