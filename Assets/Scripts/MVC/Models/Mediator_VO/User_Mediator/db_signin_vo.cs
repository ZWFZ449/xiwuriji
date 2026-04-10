using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class db_signin_vo : Base_VO
{
    /// <summary>
    /// 签到天数
    /// </summary>
    public readonly int index;
    /// <summary>
    /// 奖励内容
    /// </summary>
    public readonly string value;

    public db_signin_vo(int index, string value)
    {
        this.index = index;
        this.value = value;
    }
}
