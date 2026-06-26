using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class user_artifact_vo : Base_VO
{
    private string artifact_value;

    private List<(int,int,long)> artifact_list = new List<(int,int, long)>();

    public void Init(string artifact_value)
    {
        this.artifact_value = artifact_value;

        List<string> list = ArrayHelper.Get_Split<string>(artifact_value, ';');

        for (int i = 0; i < list.Count; i++)
        {
            List<string> list2 = ArrayHelper.Get_Split<string>(list[i], ',');
            if (list2.Count == 3)
            { 
                artifact_list.Add((int.Parse(list2[0]), int.Parse(list2[1]), long.Parse(list2[2]))); 
            }
        }
    }
    /// <summary>
    /// 读取数据
    /// </summary>
    public List<(int, int, long)> Get { get { return artifact_list; } }

    public override void MysqlData()
    {
        base.MysqlData();
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto,
            Mysql_Table_Name.dream_user_artifact,
            Set_Uptade_String(),
            Get_Update_Character());

    }
    public override string[] Get_Update_Character()
    {
        return new string[]
        {
            "artifact_value"
        };
    }
    public override string[] Set_Uptade_String()
    {
        return new string[]
        {
            GetStr(SetData())
        };
    }
    public override string[] Set_Instace_String()
    {
        return new string[]
        {
            GetStr(0),
            GetStr(SumSave.uid),
            GetStr(SetData()),
            GetStr(""),
            GetStr(""),
        };
    }
    private string SetData()
    {
        if (artifact_list.Count == 0)
        {
            artifact_list.Add((1, 0, 0));
            artifact_list.Add((2, 0, 0));

        }
        string dec= "";
        for (int i = 0; i < artifact_list.Count; i++)
        { 
            dec += artifact_list[i].Item1 + "," + artifact_list[i].Item2 + "," + artifact_list[i].Item3 + ";";
        }
        return dec;
    }
}
