using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class global_battle_info_VO : Base_VO
{
    public string value;
    public override string[] Set_Instace_String()
    {
        return new string[]
        {
        GetStr(0),
        GetStr(SumSave.par),
        GetStr(SumSave.crt_user.uid),
        GetStr(value)
        };
    }

    public void SetData(string str,Bag_Base_VO bag=null)
    {
        value += bag == null ? str : (str + ";" + bag.user_value);
        Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.global_battle_info, Set_Instace_String());
    }
}
