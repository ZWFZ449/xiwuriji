using Common;
using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class user_endless_battle : Base_VO
{
    /// <summary>
    /// 排行榜数据
    /// </summary>
    public string endless_value;
    /// <summary>
    /// 排行榜列表
    /// </summary>
    public List <endlsess_battle> endless_list = new List<endlsess_battle>();
    /// <summary>
    /// 排行榜字典
    /// </summary>
   public Dictionary<string, endlsess_battle> endless_dic = new Dictionary<string, endlsess_battle>();
    /// <summary>
    /// 排行榜数据
    /// </summary>
    public class endlsess_battle
    {
        ///uid
        public string endless_uid = "";
        ///名字
        public string name = "";
        ///玩家职业
        public string type = "";
        ///击杀怪物数量
        public int num = 0;
    }

    /// <summary>
    /// 拆分无尽塔排行榜数据
    /// </summary>
    public void Split_endless()
    {
        if (endless_value != "")
        {
            string[] arr = endless_value.Split('|');
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] != "")
                {
                    string[] arr2 = arr[i].Split(',');
                    endlsess_battle data = new endlsess_battle();
                    data.endless_uid = arr2[0];
                    data.name = arr2[1];
                    data.type = arr2[2];
                    data.num = int.Parse(arr2[3]);
                    AddEndless(data);
                }
            }
            //deleteEndless();
        }
    }
    /// <summary>
    /// 添加无尽塔排行榜数据
    /// </summary>
    /// <param name="data"></param>
    /// <param name="isSave">是否进行排序且不为初始化</param>
    public void AddEndless(endlsess_battle data,bool isSave = false)
    {
        if(string.IsNullOrEmpty(data.endless_uid))
        {
            return;
        }
        if (endless_dic.ContainsKey(data.endless_uid))
        {
            if(data.num > endless_dic[data.endless_uid].num)
            {
                endless_dic[data.endless_uid] = data;
                int index = endless_list.FindIndex(x => x.endless_uid == data.endless_uid);
                if (index >= 0)
                {
                    endless_list[index] = data;
                }
            }
        }
        else
        {
          
            endless_dic.Add(data.endless_uid, data);
            endless_list.Add(data);
        }
        if(isSave)
        {
            deleteEndless();
            SendNotification(NotiList.Refresh_Endless_Tower);
        }
    }

    /// 排序无尽塔排行榜数据
    /// </summary>
    /// <param name="data"></param>
    public void deleteEndless()
    {
        // 按击杀数降序排序
        endless_list.Sort((a, b) => b.num.CompareTo(a.num));
    }
    /// <summary>
    /// 合并数组
    /// </summary>
    /// <returns></returns>
    private string SetData()
    {
        string value = "";

        foreach (var item in endless_list)
        {
            if (value != "")
            {
                value += "|";
            }
            value += item.endless_uid + "," + item.name + "," + item.type + "," + item.num ;
        }
        return value;
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
