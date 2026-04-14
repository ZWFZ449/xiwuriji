using Common;
using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class global_promotion_vo : Base_VO
{

    private string key;
    private string key_value;
    /// <summary>
    /// 推广id
    /// </summary>
    private string promotion_value;
    /// <summary>
    /// 推广收益金额
    /// </summary>
    private int promotion_moeny;
    /// <summary>
    /// 推广数量
    /// </summary>
    public int promotion_number;

    public void Init(string promotion_value, int promotion_moeny,string key, string key_value)
    { 
        this.promotion_value = promotion_value;
        this.promotion_moeny = promotion_moeny;
        this.key = key;
        this.key_value = key_value;
    }

    public override string[] Set_Instace_String()
    {
        Init("", 0, "uid", SumSave.crt_user.uid);
        return new string[] { 
            GetStr(0),
            GetStr(key_value),
            GetStr(promotion_value),
            GetStr(promotion_moeny)
        };
    }

    public override string[] Get_Update_Character()
    {
        return new string[] {
            "promotion_value",
            "promotion_moeny"
        };
    }
    public override string[] Set_Uptade_String()
    {
        return new string[] {
            GetStr(promotion_value),
            GetStr(promotion_moeny)
        };
    }

    public string GetPromotion_value { get { return promotion_value; } }

    public int GetPromotion_moeny { get { return promotion_moeny; } }

    public void SetPromotion_value(string promotion_value) { this.promotion_value = promotion_value; MysqlData(); }
    public void SetPromotion_moeny(int promotion_moeny) { this.promotion_moeny += promotion_moeny; MysqlData(); }

    public override void MysqlData()
    {
        base.MysqlData();
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto,
    Mysql_Table_Name.global_promotion,
    Set_Uptade_String(),
    Get_Update_Character(),
    key,
    key_value);
        

    }
}
