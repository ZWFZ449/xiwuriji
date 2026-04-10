using Common;
using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 战斗统计
/// </summary>
public static class Combat_statistics 
{
    
    public static long bossnumber, elitenumber, maxnumber, exp, moeny, Point, bag, time,detead;
    /// <summary>
    /// 至尊积分
    /// </summary>
    private static int superlative = 0;
    private static List<(string, int)> SetMap;
    /// <summary>
    /// 最大获取的至尊积分
    /// </summary>
    private static int MaxSuperlative = 10;

    private static int crtsuperlative = 0;

    /// <summary>
    /// 初始化
    /// </summary>
    public static void Init()
    {
        elitenumber = 0;
        bossnumber = 0;
        maxnumber = 0;
        exp = 0;
        moeny = 0;
        bag = 0;
        time = 0;
        Point = 0;
        detead= 0;
        SetMap = SumSave.crt_needlist.SetMap();
        MaxSuperlative = Tool_State.IsState(State_List.至尊卡) ? 20 : 10;
    }
    /// <summary>
    /// 是否增加时间
    /// </summary>
    public static bool isTime = false;

    public static string Show_Info()
    {
        Obtain_Superlative();
        string info = "战斗时长：" + ConvertSecondsToHHMMSS((int)time) + "秒\n" +
            "击杀总数：" + maxnumber + "个\n" +
            "击杀精英：" + elitenumber + "个\n" +
            "击杀Boss：" + bossnumber + "个\n" +
            "经验收益：" + exp + "\n" +
            "灵珠收益：" + moeny + "\n" +
            "历练收益：" + Point + "\n" +
            "装备收益：" + bag + "个\n" +
            "死亡次数：" + detead + "次\n";// +
                                     //Show_Color.Red("至尊积分: " + maxnumber + " / 500");
        if (Tool_State.IsState(State_List.至尊卡))
        {
            info += Show_Color.Red("至尊宝箱: " + superlative + " / " + SumSave.base_setting[6]+"("+ crtsuperlative + ")");
        }
        else info += Show_Color.Yellow("宝箱: " + superlative + " / " + SumSave.base_setting[6] + "(" + crtsuperlative + ")");
        return info;
    }

    private static void Obtain_Superlative()
    {
        crtsuperlative = MaxSuperlative;
        for (int i = 0; i < SetMap.Count; i++)
        {
            if (SetMap[i].Item1 == MapStateList.至尊宝箱.ToString())
            {
                crtsuperlative = MaxSuperlative - SetMap[i].Item2;
                return;
            }
        }
    }

    public static void Time()
    {
        time++;
        //if (isTime) time++;
    }
    public static void AddBossNumber()
    {
        bossnumber++;
    }

    public static void AddEliteNumber()
    { 
        elitenumber++;
    }
    public static void AddMaxNumber()
    { 
        maxnumber++;
        superlative++;
        superlative = Mathf.Clamp(superlative, 0, SumSave.base_setting[6]);
    }
    public static void AddExp(long exp)
    { 
        Combat_statistics.exp += exp;
    }
    public static void AddMoeny(long moeny)
    { 
        Combat_statistics.moeny += moeny;
    }
    public static void AddPoint(long Point)
    { 
        Combat_statistics.Point += Point;
    }
    public static void AddBag(int bag)
    { 
        Combat_statistics.bag += bag;
    }
    public static void AddDead()
    { 
        detead++;
    }
    /// <summary>
    /// 清空至尊积分
    /// </summary>
    public static void ClearSuperlative()
    {
        superlative -= SumSave.base_setting[6];
        for (int i = 0; i < SetMap.Count; i++)
        {
            if (SetMap[i].Item1 == MapStateList.至尊宝箱.ToString())
            {
                //存在更改状态
                SumSave.crt_needlist.SetMap((SetMap[i].Item1, SetMap[i].Item2 + 1));
                return;
            }
        }
        SetMap.Add((MapStateList.至尊宝箱.ToString(), 1));
        SumSave.crt_needlist.SetMap((MapStateList.至尊宝箱.ToString(), 1));
    }
    /// <summary>
    /// 获取至尊积分状态
    /// </summary>
    /// <returns></returns>
    public static bool isSuperlative()
    {
        if (SetMap == null) SetMap = SumSave.crt_needlist.SetMap();
#if UNITY_EDITOR
        
#elif UNITY_ANDROID
#elif UNITY_IPHONE
#endif


        for (int i = 0; i < SetMap.Count; i++)
        {
            if (SetMap[i].Item1 == MapStateList.至尊宝箱.ToString())
            {
                if (SetMap[i].Item2 < MaxSuperlative)
                {
                    return superlative >= SumSave.base_setting[6];
                }
                else return false;
            }
        }
        SetMap.Add((MapStateList.至尊宝箱.ToString(), 0));
        return superlative >= SumSave.base_setting[6];
    }
    /// <summary>
    /// 离线增加至尊积分
    /// </summary>
    /// <param name="number"></param>
    public static void offline(int number)
    { 
        superlative += number;
    }
    public static string ConvertSecondsToHHMMSS(int totalSeconds)
    {
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;

        return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }

}
