using Common;
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
 魔法
}
public class db_suit_vo : Base_VO
{
    public readonly string suit_name;
    public readonly int suit_type;
    public readonly int suit_number;
    /// <summary>
    /// (需要数量，属性类型，属性加成)
    /// </summary>
    public List<(int, int, int)> suit_list { get { return Dic_suit_list[SumSave.crtHero.zs_lv]; } }

    public Dictionary<int, List<(int, int, int)>> Dic_suit_list;

    public db_suit_vo(string suit_name, int suit_number, int suit_type, string v)
    {
        this.suit_name = suit_name;
        this.suit_number = suit_number;
        this.suit_type = suit_type;
        Init(v);
    }

    public void Init(string value)
    {
        Dic_suit_list = new Dictionary<int, List<(int, int, int)>>();
        List<string> valuelists = ArrayHelper.Get_Split<string>(value, '|');
        for (int j = 0; j < valuelists.Count; j++)
        {
            string[] values = valuelists[j].Split('&');
            List<(int, int, int)> suit_lists = new List<(int, int, int)>();
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i].Length > 1)
                {
                    string[] temp = values[i].Split(' ');
                    if (temp.Length == 3)
                    {
                        (int, int, int) temp1 = (int.Parse(temp[0]), int.Parse(temp[1]), int.Parse(temp[2]));
                        suit_lists.Add(temp1);
                    }
                }
            }
            Dic_suit_list.Add(j + 1, suit_lists);
        }

    }
}
