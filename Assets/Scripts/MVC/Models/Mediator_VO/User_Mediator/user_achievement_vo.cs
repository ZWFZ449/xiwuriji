using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class user_achievement_vo : Base_VO
{
    /// <summary>
    /// 自身uid
    /// </summary>
    public string uid;
    /// <summary>
    /// 区
    /// </summary>
    public int par;
    /// <summary>
    /// 经验值
    /// </summary>
    public string achievement_exp;
    /// <summary>
    /// 等级
    /// </summary>
    public string achievement_lvs;

    /// <summary>
    /// 具体成就以及成就等级
    /// </summary>
    private Dictionary<string, long> user_achievements = new Dictionary<string, long>();
    private Dictionary<string, long> user_achievements_lv = new Dictionary<string, long>();
  

    /// <summary>
    /// 成就获取经验值
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, long> Set_Exp()
    {
        return user_achievements;
    }

    /// <summary>
    /// 成就增加经验值
    /// </summary>
    /// <param name="data"></param>
    /// <param name="lv"></param>
    public void increase_date_Exp(string data, long lv)
    {
        if(user_achievements.ContainsKey(data.ToString()))
        {
            user_achievements[data] += lv;
        }
        else
        {
            user_achievements.Add(data, lv);
            user_achievements_lv.Add(data, 0);
        }
        Game_Omphalos.i.GetQueue(
                   Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_achieve,
                   SumSave.crt_achievement.Set_Uptade_String(), SumSave.crt_achievement.Get_Update_Character());
    }

    /// <summary>
    /// 更新经验值
    /// </summary>
    /// <param name="data"></param>

    public void up_date_Exp(string data , long lv)
    {
        if (user_achievements.ContainsKey(data.ToString()))
        {
            user_achievements[data] = lv;
        }
        else
        {
            user_achievements.Add(data, lv);
            user_achievements_lv.Add(data, 0);
        }
        Game_Omphalos.i.GetQueue(
                   Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_achieve,
                   SumSave.crt_achievement.Set_Uptade_String(), SumSave.crt_achievement.Get_Update_Character());
    }



    public void Get_Exp(Dictionary<string, long> data)
    {
        user_achievements = data;
    }
    public void Get_lv(Dictionary<string, long> data)
    {
        user_achievements_lv = data;
    }

    /// <summary>
    /// 获取等级
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, long> Set_Lv()
    {
        return user_achievements_lv;
    }

    /// <summary>
    /// 拆解数据
    /// </summary>
    public void Init()
    {
        if(achievement_exp==""|| achievement_lvs == "")
        {
            return;
        }
        string[] strs = achievement_exp.Split('|');
        string[] lvs = achievement_lvs.Split('|');

        //if (strs.Length != lvs.Length)
        //{
        //    string str = "";
        //    bool found = Array.Exists(strs, lv => lv == str);
            
        //    if (!found)
        //    {
        //        string[] str1 = strs[i].Split(' ');
        //        str += achievement_lvs == "" ? str1[0] + " 0" : "|" + str1[0] + " 0";
        //    }
        //    achievement_lvs += achievement_lvs == "" ? str : "|" + str;
        //    lvs = achievement_lvs.Split('|');

        //}


        for (int i = 0; i < strs.Length; i++)
        {
            string[] str = strs[i].Split(' ');
           
            if (!user_achievements.ContainsKey(str[0]))
            {
                user_achievements.Add(str[0], long.Parse(str[1]));
            }
            
        }
        for (int i = 0; i < lvs.Length; i++)
        {
            string[] lv = lvs[i].Split(' ');
            if (!user_achievements_lv.ContainsKey(lv[0]))
            {
                user_achievements_lv.Add(lv[0], long.Parse(lv[1]));

            }
        }

    }

    public void Refresh()
    {
        achievement_exp = "";
        achievement_lvs = "";
        int number = 0;
        foreach (var item in user_achievements)
        {
            ++number;
            achievement_exp += item.Key + " " + item.Value + (number == user_achievements.Count ? "" : "|");
        }
        number = 0;
        foreach (var item in user_achievements_lv)
        {
            ++number;
            achievement_lvs += item.Key + " " + item.Value + (number == user_achievements_lv.Count ? "" : "|");
        }
    }
    public override string[] Set_Instace_String()
    {
        return new string[] { 
            GetStr(0), 
            GetStr(SumSave.crt_user.uid), 
            GetStr(SumSave.par), 
            GetStr(achievement_exp),
            GetStr(achievement_lvs) 
        };
    }

    public override string[] Get_Update_Character()
    {
        return new string[]
        {
            "achieve_exp",
            "achieve_lvs"
        };

    }
    public override string[] Set_Uptade_String()
    {
        return new string[]
        {
        GetStr(data_achievement_value(user_achievements)),
        GetStr(data_achievement_value(user_achievements_lv))
        };
    }

   /// <summary>
   /// 转化数据
   /// </summary>
   /// <param name="data"></param>
   /// <returns></returns>
    private string data_achievement_value(Dictionary<string,long> data)
    {
        string value = "";
        foreach (var item in data)
        {
            value+=(value == "" ? "" : "|") + item.Key + " " + item.Value;
        }
        return value;
    }
}
