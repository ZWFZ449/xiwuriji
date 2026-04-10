using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class user_world_boss : Base_VO
{
    /// <summary>
    /// 对世界boss造成的伤害
    /// </summary>
    public long damage=0;
    /// <summary>
    /// 对世界最后一次造成伤害的时间
    /// </summary>
    public  DateTime datetime;
    /// <summary>
    /// 对应区服
    /// </summary>
    public int par;
    /// <summary>
    /// 玩家id
    /// </summary>
    public string uid;

    public user_world_boss(int damage, DateTime datetime, int par, string uid)
    {
        this.damage = damage;
        this.datetime = datetime;
        this.par = par;
        this.uid = uid;
    }

    public void CauseDamage(long damage)
    {
        this.damage += damage;
        datetime = SumSave.nowtime;
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.user_world_boss_copy1,
          SumSave.crt_world_boss_hurt.Set_Uptade_String(), SumSave.crt_world_boss_hurt.Get_Update_Character());
    }

    public string[] Set_Instace_String(string uid)
    {
        return
            new string[]
            {
                GetStr(0),
                GetStr(uid),
                GetStr(SumSave.par),
                GetStr(damage),
                GetStr(datetime)
            };
    }


    public string[] Set_Instace_String()
    {
        return
            new string[]
            {
                GetStr(0),
                GetStr(SumSave.uid),
                GetStr(SumSave.par),
                GetStr(damage),
                GetStr(datetime)
            };
    }


    public override string[] Get_Update_Character()
    {
        return new string[] { "damage" ,"datetime" };
    }

    public override string[] Set_Uptade_String()
    {
        return new string[] { GetStr(damage), GetStr(datetime) };
    }

}
