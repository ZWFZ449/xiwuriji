using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class user_mail_vo : Base_VO
{
    public List<int> lists = new List<int>();
    public string user_value;
    public void Init()
    {
        string[] strs = user_value.Split(',');
        for (int i = 0; i < strs.Length; i++)
        { 
            if(!string.IsNullOrEmpty(strs[i]))
            lists.Add(int.Parse(strs[i]));
        }
     }

    public override string[] Set_Instace_String()
    {
        return new string[]

        {
        GetStr(0),
        GetStr(SumSave.crt_user.uid),
        GetStr(user_value)
        };
    }

    public override string[] Get_Update_Character()
    {
        return new string[] { "user_value" };

    }

    public override string[] Set_Uptade_String()
    {
        return new string[] { GetStr(GetData()) };
    }
    /// <summary>
    /// 获取数据
    /// </summary>
    /// <returns></returns>
    private string GetData()
    {
        string value = "";
        for (int i = 0; i < lists.Count; i++)
        {
            value += (value == "" ? "" : ",") + lists[i];
        }
        return value;
    }
}
