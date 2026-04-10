using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class db_collect_vo : Base_VO
{
    /// <summary>
    /// 需要收集物品的名字
    /// </summary>
    public readonly string Name;

    /// <summary>
    /// 需要收集物品的类型
    /// </summary>
    public readonly string StdMode;

    /// <summary>
    /// 收集完成增加的属性
    /// </summary>
    public readonly string bonuses_type;

    /// <summary>
    /// 收集完成增加的属性数组
    /// </summary>
    public string[] bonuses_types;

    /// <summary>
    /// 收集完成增加的属性值
    /// </summary>
    public readonly string bonuses_value;

    /// <summary>
    /// 收集完成增加的属性值数组
    /// </summary>
    public string[] bonuses_values;

    public db_collect_vo(string name, string stdMode, string bonuses_type, string bonuses_value)
    {
        Name = name;
        StdMode = stdMode;
        this.bonuses_type = bonuses_type;
        this.bonuses_value = bonuses_value;
        Init();
    }

    public void Init()
    {
        bonuses_types = bonuses_type.Split(' ');
        bonuses_values= bonuses_value.Split(' ');
    }
}
