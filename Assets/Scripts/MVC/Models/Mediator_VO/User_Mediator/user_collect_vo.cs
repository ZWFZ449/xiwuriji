using Common;
using MVC;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class user_collect_vo : Base_VO
{
    /// <summary>
    /// 收集信息整合
    /// </summary>
    public string collect_value;
    /// <summary>
    /// 是否收集dic
    /// </summary>
    public Dictionary<string, int> user_collect_dic= new Dictionary<string, int>();


    /// <summary>
    /// 收集套装整合
    /// </summary>
    //public string collect_suit_value;

    /// <summary>
    /// 收集套装 (套装名，(装备名，是否收集))
    /// </summary>
    /// <returns></returns>
    //public Dictionary<string, List<(string,int)>> user_collect_suit_dic = new Dictionary<string, List<(string, int)>>();

    /// <summary>
    /// 物品收集完成并且写入数据库
    /// </summary>
    /// <param name="name"></param>
    /// <param name="num"></param>

    public void collect_complete(string name, int num=1)
    {
        //user_collect_dic[name] = num;
        if (!user_collect_dic.ContainsKey(name))
        {
            user_collect_dic.Add(name, num);
        }
        else
        {
            Debug.LogError("装备重复");
        }
        
        SumSave.crt_collect.collect_Merge();
        Game_Omphalos.i.GetQueue(
        Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_collect,
        SumSave.crt_collect.Set_Uptade_String(), SumSave.crt_collect.Get_Update_Character());
    }


    /// <summary>
    /// 解析套装收集
    /// </summary>
    //public void collect_suit_complete()
    //{
    //    string[] value = collect_suit_value.Split('|');
    //    for (int j = 0; j < value.Length; j++)
    //    {
    //        string[] value2 = value[j].Split(' ');
    //        if (!user_collect_suit_dic.ContainsKey(value2[0]))
    //        {
    //            List<(string, int)> list = new List<(string, int)>();
    //            (string, int) temp = (value2[1],int.Parse(value2[2]));
    //            list.Add(temp);
    //            user_collect_suit_dic.Add(value2[0], list);
    //        }
    //        else
    //        {
    //            user_collect_suit_dic[value2[0]].Add((value2[1], int.Parse(value2[2])));
    //        }

    //    }
    //}



        /// <summary>
        /// 找到装备中的套装并收集
        /// </summary>
    //public void findCollectSuit()
    //{
    //    for (int i = 0; i < SumSave.db_stditems.Count; i++)
    //    {
    //        if (SumSave.db_stditems[i].suit > 0)
    //        {
    //            if (!user_collect_suit_dic.ContainsKey(SumSave.db_stditems[i].suit_name))
    //            {
    //                List<(string, int)> list = new List<(string, int)>();
    //                (string, int) temp = (SumSave.db_stditems[i].Name, 0);
    //                list.Add(temp);
    //                user_collect_suit_dic.Add(SumSave.db_stditems[i].suit_name, list);
    //            }
    //            else
    //            {
    //                user_collect_suit_dic[SumSave.db_stditems[i].suit_name].Add((SumSave.db_stditems[i].Name, 0));
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// 合并套装收集
    /// </summary>
    /// <param name="name"></param>
    //public void collect_suit_Merge()
    //{
    //    collect_suit_value= "";
    //    bool isFirst = true;
    //    foreach (var item in user_collect_suit_dic)
    //    {
    //        foreach (var item2 in item.Value)
    //        {
    //            if (isFirst)
    //            {
    //                isFirst = false;
    //            }
    //            else
    //            {
    //                // 后续元素前加 "|"
    //                collect_suit_value += "|";
    //            }
    //            collect_suit_value += item.Key + " " + item2.Item1 + " " + item2.Item2;
    //        }
    //    }
    //}





    /// <summary>
    /// 合并单个装备收集
    /// </summary>
  
    public void collect_Merge()
    {
        collect_value = "";
        

        for (int j= 0; j < user_collect_dic.Count; j++)
        {
            if (j > 0)
            {
                collect_value += "|";
            }
            collect_value += user_collect_dic.Keys.ToArray()[j] + " " + user_collect_dic.Values.ToArray()[j];
        }

    }

    public override string[] Set_Instace_String()
    {
        return new string[]
        {
            GetStr(0),
            GetStr(SumSave.crt_user.uid),
            GetStr(collect_value),
     
        };

    }

    public override string[] Get_Update_Character()
    {
        return new string[] {
            "collect_value",
        
        };
    }

    public override string[] Set_Uptade_String()
    {
        return new string[]
         {
            GetStr(collect_value),
            
         };
    }
}
