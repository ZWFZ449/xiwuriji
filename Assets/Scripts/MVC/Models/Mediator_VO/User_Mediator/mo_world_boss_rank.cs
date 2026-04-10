using Common;
using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mo_world_boss_rank : Base_VO
{
    /// <summary>
    /// 试炼塔排行榜
    /// </summary>
    public string Ranking_value;
    /// <summary>
    /// 试炼塔排行榜列表
    /// </summary>
    public List<(string,string,long)> lists = new List<(string,string,long)>();
    /// <summary>
    /// 网络连接
    /// </summary>
    public bool open_mysql = true;

    /// <summary>
    /// 初始化排行榜列表
    /// </summary>
    public void InitLists()
    {
        open_mysql = false;
        string[] strs = Ranking_value.Split(';');
        foreach (string item in strs)
        {
            if (item != "")
            {
                string[] str = item.Split(',');
                lists.Add((str[0], str[1], long.Parse(str[2])));
            }
        }
    }


    /// <summary>
    /// 合并boss排行榜
    /// </summary>
    /// <returns></returns>
    public string SetData()
    {
        Ranking_value = "";
        foreach ((string, string, long) item in lists)
        {
            if (Ranking_value != "")
            {
                Ranking_value += ';';
            }
            Ranking_value += item.Item1 + ',' + item.Item2 + ',' + item.Item3;
        }
            
      return Ranking_value;
    }

    public override string[] Set_Instace_String()
    {
        return
            new string[]
            {
                GetStr(0),
                GetStr(SumSave.par),
                GetStr("")
            };
    }

    public override string[] Get_Update_Character()
    {
        return new string[] { "value" };
    }

    public override string[] Set_Uptade_String()
    {
        return new string[] { GetStr(SetData()) };
    }
}
