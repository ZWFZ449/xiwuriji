using Common;
using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class user_artifact_vo : Base_VO
{
    public string artifact_value;
    private List<(string,int)> artifact_list;
    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        artifact_list = new List<(string, int)>();
        string[] artifact_value_array = artifact_value.Split(',');
        if (artifact_value_array.Length > 1)
        {
            for (int i = 0; i < artifact_value_array.Length; i++)
            { 
                string[] artifact_array = artifact_value_array[i].Split(' ');
                if (artifact_array.Length > 1)
                artifact_list.Add((artifact_array[0], int.Parse(artifact_array[1])));
            }
        }
    }
    /// <summary>
    /// 获取
    /// </summary>
    /// <returns></returns>
    public List<(string, int)> Set()
    {
        return artifact_list;
    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="data"></param>
    public void Get((string, int) data)
    {
        bool exist = true;
        for (int i = 0; i < artifact_list.Count; i++)
        {
            if(artifact_list[i].Item1 == data.Item1)
            {
                (string, int) artifact = artifact_list[i];
                exist = false;
                artifact.Item2 = artifact_list[i].Item2 + 1;
                artifact_list[i] = artifact;

            }
        }
        if (exist) artifact_list.Add(data);
        Get();
    }
    /// <summary>
    /// 刷新
    /// </summary>
    public void Get()
    {
        artifact_value = "";
        foreach (var item in artifact_list)
        {
            artifact_value += item.Item1 + " " + item.Item2 + ",";
        }
    }

    public override string[] Set_Instace_String()
    {
        return new string[]
        {
            GetStr(0),
            GetStr(SumSave.crt_user.uid),
            GetStr(artifact_value)
        };

    }

    public override string[] Get_Update_Character()
    {
        return new string[] { "artifact_value" };
    }

    public override string[] Set_Uptade_String()
    {
        return new string[]
         {
            GetStr(artifact_value)
         };
    }
}
