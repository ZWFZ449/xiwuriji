using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Suit_Type
{ 
 物理攻击,
 魔法攻击,
 道术攻击,
 双防,
 生命,
 真实伤害,
 攻击速度,
 物攻属性,
 魔攻属性,
 道攻属性,
 双防属性,
 伤害吸收,
}
public class db_suit_vo : Base_VO
{
    public readonly string suit_name;
    public readonly int suit_type;
    public readonly int suit_number;
    /// <summary>
    /// (需要数量，属性类型，属性加成)
    /// </summary>
    public List<(int,int,int)> suit_list;

    public db_suit_vo(string suit_name, int suit_number, int suit_type, string v)
    {
        this.suit_name = suit_name;
        this.suit_number = suit_number;
        this.suit_type = suit_type;
        Init(v);
    }

    public void Init(string value)
    {
        suit_list = new List<(int, int, int)>();
        string[] values= value.Split('&');
        for (int i = 0; i < values.Length; i++)
        {
            if (values[i].Length > 1)
            { 
                string[] temp = values[i].Split(' ');
                if (temp.Length == 3)
                {
                    (int, int, int) temp1 = (int.Parse(temp[0]), int.Parse(temp[1]), int.Parse(temp[2]));
                    suit_list.Add(temp1);
                }
                
            }
        }
    }
}
