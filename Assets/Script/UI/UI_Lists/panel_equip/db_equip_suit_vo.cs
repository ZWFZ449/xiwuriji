using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 装备单独加成效果
/// </summary>
public class db_equip_suit_vo : Base_VO
{
    /// <summary>
    /// 名称
    /// </summary>
    public readonly string equip_name;
    /// <summary>
    /// 携带特效
    /// </summary>
    public readonly string[] equip_suit;
    /// <summary>
    /// 升级加成值
    /// </summary>
    public readonly string[] equip_uplv;

    public db_equip_suit_vo(string equip_name, string[] equip_suit, string[] equip_uplv)
    { 
        this.equip_name = equip_name;
        this.equip_suit = equip_suit;
        this.equip_uplv = equip_uplv;
    }

}
