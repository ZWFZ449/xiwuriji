using Common;
using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class user_needlist_vo : Base_VO
{
    /// <summary>
    /// 商店限购物品购买的次数
    /// </summary>
    public string store_value;
    /// <summary>
    /// 商店限购物品列表
    /// </summary>
    //public List<string[]> store_value_list=new List<string[]>();
    public Dictionary<string, int> store_value_dic = new Dictionary<string, int>();
    /// <summary>
    /// 地图进入次数
    /// </summary>
    public string map_value;
    /// <summary>
    /// 地图进入次数列表
    /// </summary>
    private List<(string, int)> map_value_list=new List<(string, int)>();

    /// <summary>
    /// 命运殿堂抽奖次数
    /// </summary>
    public string fate_value;
    /// <summary>
    /// 命运殿堂抽奖次数 <期数,<(物品名字，物品单抽获得的数量)，物品抽取的次数>>
    /// </summary>
    public Dictionary<int, Dictionary<(string, int), int>> fate_value_dic = new Dictionary<int, Dictionary<(string, int), int>>();

    public string user_value;

    /// <summary>
    /// 每日重置
    /// </summary>
    public void DailyClear()
    {
        store_value_dic = new Dictionary<string, int>();
        map_value_list = new List<(string, int)>();
        MysqlData();
       
    }



    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        store_Init();
        map_Init();
        fate_Init();
        user_Init();
    }
    /// <summary>
    /// 解析user_value
    /// </summary>
    public void user_Init()
    {
        //user_value_list = new List<List<string>>();
        //string[] user = user_value.Split(',');
        //for (int i = 0; i < user.Length; i++)
        //{
        //    string[] user1 = user[i].Split(' ');
        //    List<string> user_list= new List<string>();
        //    for (int x = 0; x < user1.Length; x++)
        //    {
        //        user_list.Add(user1[x]);
        //    }
            
        //    user_value_list.Add(user_list);
        //}  
    }
    /// <summary>
    /// 合并user_value
    /// </summary>
    public string user_Merge()
    {
        string item = "";
        //for (int i = 0; i < user_value_list.Count; i++)
        //{
        //    if (item != "")
        //    {
        //        item += ",";
        //    }
        //    for (int x = 0; x < user_value_list[i].Count; x++)
        //    {
        //        if (x != 0)
        //        {
        //            item += " ";
        //        }
        //        item += user_value_list[i][x];
        //    }
            
        //}
       return item;
    }


    /// <summary>
    /// 解析命运殿堂抽奖次数
    /// </summary>
    public void fate_Init()
    {
        if (fate_value == "")
        {
            return;
        }
        string[] store_list = fate_value.Split('&');//分解每一期
        for (int i = 0; i < store_list.Length; i++)
        {
            string[] fate = store_list[i].Split('|');//分解每一个物品 第一个为期数，后面为物品
            Dictionary<(string, int), int> dic = new Dictionary<(string, int), int>();
            for (int x=0;x< fate.Length;x++)
            {
                string[] fate_list = fate[x].Split(' ');//分解物品属性
                
                if (fate_list.Length == 3)//等于3为物品否则为期数
                {
                    dic.Add((fate_list[0], int.Parse(fate_list[1])), int.Parse(fate_list[2]));
                }
            }
            fate_value_dic.Add(int.Parse(fate[0]), dic);
        }
    }
    /// <summary>
    /// 合并命运殿堂抽奖次数
    /// </summary>
    /// <returns></returns> 
    private string fate_Merge()
    {
        string item = "";
        foreach (KeyValuePair<int, Dictionary<(string, int), int>> fate in fate_value_dic)
        {
            if (item != "")
            {
                item += "&";
            }
            item += fate.Key;
            foreach (KeyValuePair<(string, int), int> fate_list in fate.Value)
            {
                item += "|" + fate_list.Key.Item1 + " " + fate_list.Key.Item2 + " " + fate_list.Value;
            }
        }
        return item;
    }

    
    /// <summary>
    /// 解析商店限购物品
    /// </summary>
    public void store_Init()
    {
        string[] store_list = store_value.Split(',');
        for (int i = 0; i < store_list.Length; i++)
        {
            string[] store = store_list[i].Split(' '); 
            if(store.Length == 2)
                //store_value_list.Add(store);
            store_value_dic.Add(store[0], int.Parse(store[1]));
        }
    }

    /// <summary>
    /// 获取地图进入次数
    /// </summary>
    /// <returns></returns>
    public List<(string, int)> SetMap()
    { 
     return map_value_list;
    }
    /// <summary>
    /// 更新状态
    /// </summary>
    /// <param name="map"></param>
    public void SetMap((string, int) map)
    {
        bool exist = true;
        for (int i = 0; i < map_value_list.Count; i++)
        {
            if (map_value_list[i].Item1 == map.Item1)
            {
                exist = false;
                map_value_list[i] = map;
            }
        }
        if(exist)map_value_list.Add(map);
        MysqlData();
    }

    public override void MysqlData()
    {
        base.MysqlData();
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_needlist,
           SumSave.crt_needlist.Set_Uptade_String(), SumSave.crt_needlist.Get_Update_Character());
    }
    /// <summary>
    /// 解析进入地图次数
    /// </summary>
    public void map_Init()
    {
        string[] map_list = map_value.Split(',');
        for (int i = 0; i < map_list.Length; i++)
        {
            string[] map = map_list[i].Split(' ');
            if (map.Length == 2)
                map_value_list.Add((map[0], int.Parse(map[1])));
        }
    }
    /// <summary>
    /// 合并商店限购物品
    /// </summary>
    private string store_Merge()
    {
        string item="";
       
        foreach (KeyValuePair<string, int> store in store_value_dic)
        {
            if (item != "")
                item += ",";

            item += store.Key + " " + store.Value;
        }

        return item;
    }



    /// <summary>
    /// 合并地图进入次数
    /// </summary>
    private string map_Merge()
    {
        string item = "";
        for (int i = 0; i < map_value_list.Count; i++)
        {
            if (i >0)
            {
                item += ",";
            }
            item += map_value_list[i].Item1 + " " + map_value_list[i].Item2;
        }
        return item;
    }
  




    public override string[] Set_Instace_String()
    {
        return new string[]
        {
            GetStr(0),
            GetStr(SumSave.crt_user.uid),
            GetStr(store_value),
            GetStr(map_value),
            GetStr(fate_value),
            GetStr(user_value),

        };

    }

    public override string[] Get_Update_Character()
    {
        return new string[] {
            "store_value",
            "map_value",
            "fate_value",
            "user_value"
        };
    }

    public override string[] Set_Uptade_String()
    {
        return new string[]
         {
            GetStr(store_Merge()),
            GetStr(map_Merge()),   
            GetStr(fate_Merge()),
            GetStr(user_Merge()),
         };
    }
}
