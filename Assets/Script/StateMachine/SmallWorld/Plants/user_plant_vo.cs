using Common;
using MVC;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class user_plant_vo : Base_VO
{


    //public PanltEnum plantType; // 植物类型
    //public int seedNumber;//种子数量

    /// <summary>
    /// 植物名称
    /// </summary>
    public string plantName;
    /// <summary>
    /// 种子需要成熟的时间
    /// </summary>
    public int plantTime;
    /// <summary>
    /// 种子收获材料
    /// </summary>
    public string HarvestMaterials;
    /// <summary>
    /// 种子收获数量
    /// </summary>
    public int harvestnumber;
    /// <summary>
    /// 消耗数量 为了实现偷菜玩法
    /// </summary>
    public int lossnumber;
    /// <summary>
    /// 种植等级
    /// </summary>
    public int plantLeve;
    private List<(string,DateTime)> user_plants;//种植的植物名称和成熟时间

    private List<string> user_valueS;//种植的植物名称和种植时间
    public string user_value;
    /// <summary>
    /// 解析植物信息
    /// </summary> 
    public void Init()
    {
        user_valueS= new List<string>();
        user_plants = new List<(string, DateTime)>();
        string[] str = user_value.Split('&');
        for (int i = 0; i < str.Length; i++)
        {
            user_valueS.Add(str[i]);
            if (str.Length > 0)
            {
                string[] str1 = str[i].Split('|');
                if (str1.Length == 2)
                    user_plants.Add((str1[0], (str1[0] == "0") ? SumSave.nowtime : Convert.ToDateTime(str1[1])));
            }
        }
    }
   public void Up_user_plants(List<(string, DateTime)> _user)
    {
        user_plants = _user;
    }

    /// <summary>
    /// 翻倍获取
    /// </summary>
    public int  DoubleTheAcquisition()
    {
        int number = harvestnumber-lossnumber;
        int ran =Random.Range(1,100);
        if(ran<30)
        {
            if(ran<10)
            {
                number = number * 3;
            }
            else
            {
                number = number * 2;
            }
        }
        return number;
    }



    public void Set_data(List<(string, DateTime)> crt_plants)
    {
        user_plants= crt_plants;
        MysqlData();
    }

    public override void MysqlData()
    {
        base.MysqlData();
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_plant,
            SumSave.crt_plant.Set_Uptade_String(), SumSave.crt_plant.Get_Update_Character());
    }

    public string Set_data()
    {
        string dec = "";
        foreach (var item in user_plants)
        {
            if (dec != "") dec += "&";
            dec += item.Item1 + "|" + item.Item2.ToString();
        }
        return dec;

    }


    /// <summary>
    /// 获取当前种植的植物
    /// </summary>
    /// <returns></returns>
    public List<(string, DateTime)> Set()
    {
        return user_plants;
    }

    public override string[] Set_Instace_String()
    {
        return new string[]
        {
           GetStr(0),
           GetStr(SumSave.crt_user.uid),
           GetStr(plantLeve),
           GetStr(user_value),
        };
     }
    
    public override string[] Get_Update_Character()
    {
        return new string[]
        {
           "plantLeve",
           "user_value"
           
        };
    }


    public override string[] Set_Uptade_String()
    {
        return new string[]
        {
           GetStr(plantLeve),
           GetStr(Set_data()),

        };
    }



}
