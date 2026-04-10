using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class user_greenhand_vo : Base_VO
{
    /// <summary>
    /// 当前任务编号
    /// </summary>
    public int crt_task;
    public List<int> task_list;
    public bool state = false;
    /// <summary>
    ///  任务进度
    /// </summary>
    public int crt_progress = 0;
    public string user_value;
    public void Init()
    {
        string[] parts = user_value?.Split(',') ?? Array.Empty<string>();
        task_list = new List<int>();
        for (int i = 0; i < parts.Length; i++)
        { if(parts[i] != "")
            task_list.Add(int.Parse(parts[i]));
        }
    }

    public override string[] Set_Instace_String()
    {
        return new string[]

        {
        GetStr(0),
        GetStr(SumSave.crt_user.uid),
        GetStr(crt_task),
        GetStr(data_set())
        };
    }

    public override string[] Get_Update_Character()
    {
        return new string[] { "crt_task", "valuelist" };
    }
    public override string[] Set_Uptade_String()
    {
        return new string[]

        {
            GetStr(crt_task),
            GetStr(data_set())
        };
    }

    private string data_set()
    {
        string value = "";
        for (int i = 0; i < task_list.Count; i++)
        { 
            value += (value == ""?"":",")+ task_list[i];
        }
        return value;
    }
}
