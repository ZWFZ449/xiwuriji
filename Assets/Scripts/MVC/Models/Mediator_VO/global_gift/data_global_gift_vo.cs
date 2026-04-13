using Common;
using MVC;
using System.Collections.Generic;
using UnityEngine;

public class data_global_gift_vo : Base_VO
{
     private List<string> giftS;

    private int gift_points;

    public void Init(List<string> giftS,int gift_points)
    { 
        this.giftS = giftS;
        this.gift_points = gift_points;
    }

    public override string[] Set_Instace_String()
    {
        giftS= new List<string>();
        gift_points = 0;
        return new string[]
            {
            GetStr(0),
            GetStr(SumSave.crt_user.uid),
            GetStr(write()),
            GetStr(gift_points),            
            };
    }

    public override string[] Get_Update_Character()
    {
        return new string[]
            {
                "gift_value",
                "gift_points"
            };
    }

    public override string[] Set_Uptade_String()
    {
        return new string[]
        {
            GetStr(write()),
            GetStr(gift_points),
        };
    }

    private string write()
    { 
        string str = "";
        foreach (var item in giftS)
        { 
            str += item + ",";
        }
        return str;
    }
    /// <summary>
    /// 添加礼包
    /// </summary>
    /// <param name="value"></param>
    public void SetGiftS(string value)
    {
        string normalized = value.ToUpperInvariant();
        giftS.Add(normalized);
        MysqlData();
    }
    /// <summary>
    /// 判断是否拥有礼包
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool IsHaveGift(string value)
    {
        string normalized = value.ToUpperInvariant();
        return giftS.Contains(normalized);
    }
    /// <summary>
    /// 获取累充 切断变量
    /// </summary>
    public string GetGiftPoints { get { return gift_points.ToString(); } }
    /// <summary>
    /// 写入累充金额
    /// </summary>
    /// <param name="value"></param>
    public void SetGiftPoints(int value)
    { 
        gift_points += value;
        MysqlData();
    }

    public override void MysqlData()
    {
        base.MysqlData();
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.dream_user_gift, Set_Uptade_String(), Get_Update_Character());

    }
}
