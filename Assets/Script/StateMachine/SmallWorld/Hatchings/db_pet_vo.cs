using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class db_pet_vo_old : Base_VO
{
    /// <summary>
    /// 宠物名字
    /// </summary>
    public string petName;
    /// <summary>
    /// 宠物蛋名字
    /// </summary>
    public string petEggsName;
    /// <summary>
    /// 需要孵化的时间
    /// </summary>
    public int hatchingTime;
    /// <summary>
    /// 开始孵化的时间
    /// </summary>
    public DateTime startHatchingTime;
    /// <summary>
    ///宠物蛋名字和开始孵化的时间
    /// </summary>
    private (string name, DateTime time) crt_hatching;
    /// <summary>
    /// 宠物编号
    /// </summary>
    public int hero_type ;
    /// <summary>
    /// 宠物基础属性
    /// </summary>
    public string crate_value;
    /// <summary>
    /// 宠物基础属性数组
    /// </summary>
    public List<string> crate_values=new List<string>();

    /// <summary>
    /// 宠物升级属性
    /// </summary>
    public string up_value;
    /// <summary>
    /// 宠物升级属性数组
    /// </summary>
    public List<string> up_values=new List<string>();

    /// <summary>
    /// 多少级添加一个宠物属性
    /// </summary>
    public string up_base_value;
   /// <summary>
   /// 多少级添加一个宠物属性数组
   /// </summary>
    public List<string> up_base_values = new List<string>();

    /// <summary>
    /// 宠物天赋
    /// </summary>
    public string hero_talent;
    /// <summary>
    /// 宠物等级
    /// </summary>
    public int level=1;
    /// <summary>
    /// 宠物经验
    /// </summary>
    public int exp=0;
    /// <summary>
    /// 宠物品质
    /// </summary>
    public string quality;
    /// <summary>
    /// 宠物此时的状态 0闲置1守护庄园2是探索
    /// </summary>
    public string pet_state;
    /// <summary>
    /// 探索列表
    /// </summary>
    public string pet_explore;

    public string user_value;
   


    /// <summary>
    /// 解析数据
    /// </summary>
    public void Init()
    {
        string[] str = user_value.Split('|');

        if (str.Length == 2)
        {
            crt_hatching = (str[0], (str[1] == "0") ? DateTime.Now : Convert.ToDateTime(str[1]));
        }
        else
        if (str == null)//没有数据就初始化
        {
            crt_hatching = ("0", DateTime.Now);
            Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_pet_hatching,
            SumSave.crt_hatching.Set_Uptade_String(), SumSave.crt_hatching.Get_Update_Character());
        }

    }
    /// <summary>
    /// 获得宠物数值数组
    /// </summary>
    public void GetNumerical()
    {
        crate_values= new List<string>();
        up_values = new List<string>();
        up_base_values = new List<string>();
        string[] str1 = crate_value.Split(' ');
        for (int i = 0; i < str1.Length; i++)
        {
            crate_values.Add(str1[i]);
        }

        string[] str2 = up_value.Split(' ');
        for (int i = 0; i < str2.Length; i++)
        {
            up_values.Add(str2[i]);
        }

        string[] str3 = up_base_value.Split(' ');
        for (int i = 0; i < str3.Length; i++)
        {
            up_base_values.Add(str3[i]);
        }
    }
    /// <summary>
    /// 整合属性数据
    /// </summary>
   public void Integration(List<string> crt)
   {
        crate_value= "";
        string str = "";
        for (int i = 0; i < crt.Count; i++)
        {
            str += crt[i];
       
        }
        crate_value = str;
        GetNumerical();
   }


    /// <summary>
    /// 整合数据格式
    /// </summary>
    /// <returns></returns>
    public string Set_data()
    {
        string dec = "";
        dec= crt_hatching.Item1+ "|"+ crt_hatching.Item2.ToString()+"|"+ hatchingTime;
        return dec;

    }

    public void Set_data((string name, DateTime time) crt_plants)
    {
        crt_hatching = crt_plants;
    }


    /// <summary>
    /// 获取当前孵化的宠物
    /// </summary>
    /// <returns></returns>
    public (string, DateTime) Set()
    {
        return crt_hatching;
    }

    public override string[] Set_Instace_String()
    {
        return new string[]
        {
           GetStr(0),
           GetStr(SumSave.crt_user.uid),
           GetStr(user_value), 
        };
    }

    public override string[] Get_Update_Character()
    {
        return new string[]
        {
            "user_value",
        };
    }


    public override string[] Set_Uptade_String()
    {
        return new string[]
        {
            GetStr(Set_data()),
        };
    }
    /// <summary>
    /// 转化字符串
    /// </summary>
    /// <param name="crt_pet_vo"></param>
    /// <returns></returns>
    public string IntegrationData(db_pet_vo_old crt_pet_vo)
    {
        string value = "";
        value += crt_pet_vo.petName + ",";
        value += crt_pet_vo.startHatchingTime + ",";
        value += crt_pet_vo.quality + ",";
        value += crt_pet_vo.level + ",";
        value += crt_pet_vo.exp + ",";
        value += crt_pet_vo.crate_value + "|" + crt_pet_vo.up_value + "|" + crt_pet_vo.up_base_value + ",";
        value += crt_pet_vo.pet_state;
        return value;
    }

}
