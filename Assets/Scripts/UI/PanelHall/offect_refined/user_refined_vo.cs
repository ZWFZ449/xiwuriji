using CodeStage.AntiCheat.ObscuredTypes;
using Common;
using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class user_refined_vo : Base_VO
{
    public List<ObscuredInt> refined_numbers;

    public List<ObscuredInt> refined_maxnumbers;
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="numbers"></param>
    /// <param name="maxnumbers"></param>
    public void Init(string numbers, string maxnumbers)
    { 
        refined_numbers = new List<ObscuredInt>();
        refined_maxnumbers = new List<ObscuredInt>();
        List<int> arr = ArrayHelper.Get_Split<int>(numbers, ',');
        List<int> arr2 = ArrayHelper.Get_Split<int>(maxnumbers, ',');
        for (int i = 0; i < arr.Count; i++)
        {
            refined_numbers.Add(arr[i]);
        }
        for (int i = 0; i < arr2.Count; i++)
        { 
            refined_maxnumbers.Add(arr2[i]);
        }
    }

    public override string[] Set_Instace_String()
    {
        refined_numbers= refined_numbers ?? new List<ObscuredInt>();
        refined_maxnumbers = refined_maxnumbers ?? new List<ObscuredInt>();
        return new string[] 
        { 
            GetStr(0),
            GetStr(SumSave.uid),
            GetStr(Get_Str(refined_numbers)),
            GetStr(""),
            GetStr(""),
            GetStr(Get_Str(refined_maxnumbers)),
        };
    }

    private string Get_Str(List<ObscuredInt> list)
    { 
        string str = "";
        for (int i = 0; i < list.Count; i++)
        { 
            str += list[i] + ",";
        }
        return str;
    }

    public override string[] Get_Update_Character()
    {
        return new string[] { "refined_numbers", "refined_maxnumbers" };
    }

    public override string[] Set_Uptade_String()
    {
        return new string[]
       {
            GetStr(Get_Str(refined_numbers)),
            GetStr(Get_Str(refined_maxnumbers)),
       };
    }

    public override void MysqlData()
    {
        base.MysqlData();
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.dream_user_refineds, Set_Uptade_String(), Get_Update_Character());

    }
}
