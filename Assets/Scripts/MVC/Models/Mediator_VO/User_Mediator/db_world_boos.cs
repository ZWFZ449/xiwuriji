using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class db_world_boos : Base_VO
{
    /// <summary>
    /// 当前世界boss名称
    /// </summary>
    public string name;
    /// <summary>
    /// 当前区服玩家总体的伤害
    /// </summary>
    private long maxHp;
    /// <summary>
    /// 期数
    /// </summary>
    public int number;
    /// <summary>
    /// 世界Boss造成伤害等级
    /// </summary>
    public string DamageLevel_value;
    /// <summary>
    /// 世界Boss造成伤害等级
    /// </summary>
    public List<(int,long)> DamageLevel_List= new List<(int, long)>();
    /// <summary>
    /// 最后一个玩家挑战的时间
    /// </summary>
    public string UpTime;
    /// <summary>
    /// 世界boss血量基准值
    /// </summary>
    public long BossHpBasic = 1000000;


    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        SplitDamageLevel();
    }

    /// <summary>
    /// 拆分世界Boss造成伤害等级
    /// </summary>
    public void SplitDamageLevel()
    {
        string[] DamageLevel = DamageLevel_value.Split('|');
        for (int i = 0; i < DamageLevel.Length; i++)
        {
            string[] DamageLevel_ = DamageLevel[i].Split(' ');
            DamageLevel_List.Add((int.Parse(DamageLevel_[0]), long.Parse(DamageLevel_[1])));
        }
    }



    /// <summary>
    /// 获取当前区服玩家总体的伤害
    /// </summary>
    /// <returns></returns>
    public long Get()
    {
        return maxHp;
    }
    /// <summary>
    /// 添加当前区服玩家总体的伤害
    /// </summary>
    /// <param name="finalDamage"></param>
    public void Set(long finalDamage)
    {
        maxHp -= finalDamage;
        UpTime = SumSave.nowtime.ToString("yyyy-MM-dd HH:mm:ss");
    }
    
    /// <summary>
    /// 初始化当前区服玩家总体的伤害
    /// </summary>
    /// <param name="_FinalDamage"></param>
    public void InitFinalDamage(long _FinalDamage)
    {
        maxHp = _FinalDamage;
    }

    public override string[] Set_Instace_String()
    {
        return new string[]
        {
            GetStr(0),
            GetStr(SumSave.crt_user.par),
            GetStr(name),
            GetStr(maxHp),
            GetStr(number)
        };
    }

    public override string[] Get_Update_Character()
    {
        return new string[] {
            "maxHp",
            "number",
            "UpTime"
        };

    }

    public override string[] Set_Uptade_String()
    {
        return new string[]
         {
             GetStr(maxHp),
             GetStr(number),
             GetStr(UpTime),
         };
    }

}
