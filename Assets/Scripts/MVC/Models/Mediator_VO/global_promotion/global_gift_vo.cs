using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class global_gift_vo : Base_VO
{
    public int gift_id;

    public int gift_type;

    public int gift_par;

    private string gift_value;

    private int gift_state;

    private int GiftPoints;

    public void Init(int gift_id, int gift_type,int gift_par, string gift_value, int gift_state,int GiftPoints)
    { 
        this.gift_id = gift_id;
        this.gift_type = gift_type;
        this.gift_par = gift_par;
        this.gift_value = gift_value;
        this.gift_state = gift_state;
        this.GiftPoints = GiftPoints;
    }
    /// <summary>
    /// 获取礼物内容
    /// </summary>
    public string GetGiftValue { get { return gift_value; } }
    /// <summary>
    /// 领取状态
    /// </summary>
    public int GetGiftState { get { return gift_state; } } 
    /// <summary>
    /// 获取礼物积分
    /// </summary>
    public int GetGiftPoints { get { return GiftPoints; } }

    public void SetGiftValue(int value)
    {
        gift_state = value;
        MysqlData();

    }

    public override string[] Get_Update_Character()
    {
        return
            new string[]
            {
                "gift_state"
            };
    }

    public override string[] Set_Uptade_String()
    {
        return new string[]
            {
                 GetStr(gift_state)
            };
    }

    public override void MysqlData()
    {
        base.MysqlData();
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto,
          Mysql_Table_Name.global_gift,
          Set_Uptade_String(),
          Get_Update_Character(),
          "id",
          gift_id.ToString()
            );
    }
}
