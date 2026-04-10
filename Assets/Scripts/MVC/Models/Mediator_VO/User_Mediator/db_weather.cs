using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class db_weather : Base_VO
{
    /// <summary>
    /// 天气编号
    /// </summary>
    public readonly int weather_index;
    /// <summary>
    /// 天气名称
    /// </summary>
    public readonly string weather_type;
    /// <summary>
    /// 五行属性值
    /// </summary>
    public readonly string life_value;
    /// <summary>
    /// 五行属性值列表(对应属性，属性值)
    /// </summary>
    public List<(int, int)> life_value_list;
    /// <summary>
    /// 权重
    /// </summary>
    public readonly int probability;

    public void Init()
    {
        life_value_list = new List<(int, int)>();
        string[] life_value_arr = life_value.Split('&');
        for (int i = 0; i < life_value_arr.Length; i++)
        {
            string[] life_value_arr2 = life_value_arr[i].Split(' ');
            life_value_list.Add((int.Parse(life_value_arr2[0]), int.Parse(life_value_arr2[1])));
        }
    }
    public db_weather(int _weather_index, string _weather_type, string _life_value, int _probability)
    {
        this.weather_index = _weather_index;
        this.weather_type = _weather_type;
        this.life_value = _life_value;
        this.probability = _probability;
        this.Init();
    }
}
