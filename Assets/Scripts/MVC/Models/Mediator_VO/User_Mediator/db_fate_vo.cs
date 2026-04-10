using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class db_fate_vo : Base_VO
{
    public int fate_id;

    /// <summary>
    /// 命运殿堂具体物品
    /// </summary>
    public readonly string fate_value;
    /// <summary>
    /// 命运殿堂物品列表0名字1代表分类（1材料技能书神器2魔丸3皮肤）2单次抽取数量3最大抽取数量4权重）
    /// </summary>
    public List<(string, int, int, int, int)> fate_value_list= new List<(string, int, int, int, int)>();

    public db_fate_vo(int fate_id, string fate_value)
    {
        this.fate_id = fate_id;
        this.fate_value = fate_value;
        Init();
    }

    internal void Init()
    {
       string[] fate_value_array = fate_value.Split('|');
        foreach (var item in fate_value_array)
        {
           string[] fate_value_list_array = item.Split('*');
           fate_value_list.Add((fate_value_list_array[0], int.Parse(fate_value_list_array[1]), int.Parse(fate_value_list_array[2]), int.Parse(fate_value_list_array[3]), int.Parse(fate_value_list_array[4])));

        }

    }


}
