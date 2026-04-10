using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 强化需求
/// </summary>
public class db_strengthen_need_Vo : Base_VO
{
    /// <summary>
    /// 获取名称
    /// </summary>
    public readonly string need_value;

    public readonly int need_lv;
    /// <summary>
    /// 强化条件
    /// </summary>
    public readonly string[] need_value_list;


    public db_strengthen_need_Vo(string need_value,int need_lv, string[] need_value_list)
    { 
        this.need_value = need_value;
        this.need_lv = need_lv;
        this.need_value_list = need_value_list;
    }
}
