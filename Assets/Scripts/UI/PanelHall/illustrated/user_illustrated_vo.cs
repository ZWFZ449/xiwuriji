using Common;
using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class user_illustrated_vo : Base_VO
{
    private string illustrated_value;

    private Dictionary<string,int> illustrated_dic = new Dictionary<string, int>();

    //public user_illustrated_vo(string illustrated_value)
    //{ 
    //    this.illustrated_value = illustrated_value;
    //    List<string> lists = ArrayHelper.Get_Split<string>(illustrated_value, ',');
    //    for (int i = 0; i < lists.Count; i++)
    //    { 
    //        List<string> list = ArrayHelper.Get_Split<string>(lists[i], ':');
    //        if (list.Count == 2)
    //        {
    //            if (!illustrated_dic.ContainsKey(list[0]))
    //            { 
    //               illustrated_dic.Add(list[0], int.Parse(list[1]));
    //            }
    //        }
    //    }
    //}

    public void Iint(string illustrated_value)
    {
        this.illustrated_value = illustrated_value;
        List<string> lists = ArrayHelper.Get_Split<string>(illustrated_value, ',');
        for (int i = 0; i < lists.Count; i++)
        {
            List<string> list = ArrayHelper.Get_Split<string>(lists[i], ':');
            if (list.Count == 2)
            {
                if (!illustrated_dic.ContainsKey(list[0]))
                {
                    illustrated_dic.Add(list[0], int.Parse(list[1]));
                }
            }
        }
    }

    public Dictionary<string, int> Get_illustrated_list()
    {
        return illustrated_dic;
    }

    public void Add_illustrated_list(string illustrated_value,int number=1)
    {
        if (illustrated_dic.ContainsKey(illustrated_value))
            { 
                illustrated_dic[illustrated_value] += number;
        }else illustrated_dic.Add(illustrated_value, number);
        MysqlData();
    }
    public override string[] Set_Instace_String()
    {
        return new string[]
        {
            GetStr(0),
            GetStr(SumSave.crt_user.uid),
            GetStr(GetData())
        };
    }

    public override string[] Get_Update_Character()
    {
        return new string[]
        {
        "illustrated_value"
        };
    }

    public override string[] Set_Uptade_String()
    {
        return new string[]
        {
            GetStr(GetData())
        };
    }
    private string GetData()
    { 
        string dec="";

        foreach (var item in illustrated_dic.Keys)
        {
            dec += item + ":" + illustrated_dic[item] + ",";
        }
        return dec;
    }
    public override void MysqlData()
    {
        base.MysqlData();
        //发送数据到服务器

        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.dream_user_illustrated, Set_Uptade_String(),
               Get_Update_Character());
    }
}
