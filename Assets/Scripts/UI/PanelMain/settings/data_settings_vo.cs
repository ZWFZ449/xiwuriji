using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class data_settings_vo : Base_VO 
{
    /// <summary>
    /// 药品数据 0枚举 1物品名称 2设置百分比
    /// </summary>
    private string medicine_settings = "0 中瓶血药 80,1 中瓶蓝药 60";
    /// <summary>
    /// 设置标准
    /// </summary>
    public List<(medicineType, string, int)> medicine_settings_list = new List<(medicineType, string, int)>();
    /// <summary>
    /// 基础设置
    /// </summary>
    public int[] base_settings = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public override void Iint()
    {
        base.Iint();
        string [] medicine_settings_arr = medicine_settings.Split(',');
        for (int i = 0; i < medicine_settings_arr.Length; i++)
        { 
            string [] medicine_settings_arr2 = medicine_settings_arr[i].Split(' ');
            medicine_settings_list.Add(((medicineType)(int.Parse(medicine_settings_arr2[0])), medicine_settings_arr2[1], int.Parse(medicine_settings_arr2[2]))); 
        }
    }
    /// <summary>
    /// 设置标准
    /// </summary>
    public data_settings_vo()
    {
        Iint();
    }


}
