using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class db_base_par : Base_VO
{
    /// <summary>
    /// 选中区域
    /// </summary>
    public readonly int index;
    /// <summary>
    /// 开启时间
    /// </summary>
    public DateTime opentime;

    /// <summary>
    /// 开启状态 1开启2关闭
    /// </summary>
    public readonly int openstate;
    /// <summary>
    /// 设备类型
    /// </summary>
    public readonly int device;
    /// <summary>
    /// 显示名称
    /// </summary>
    public readonly string par_name;

    public db_base_par(int index, DateTime opentime, int openstate, int device, string par_name)
    {
        this.index = index;
        this.opentime = opentime;
        this.openstate = openstate;
        this.device = device;
        this.par_name = par_name;
    }
}
