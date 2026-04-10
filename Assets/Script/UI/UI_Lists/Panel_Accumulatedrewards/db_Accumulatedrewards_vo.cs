using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 累积奖励预制体
/// </summary>
public class db_Accumulatedrewards_vo : Base_VO
{
    public readonly string pass_value;
    public readonly string signin_value;
    public readonly string fate_value;
    public List<(int,string)> pass_list = new List<(int, string)>();
    public List<(int,string)> signin_list = new List<(int, string)>();
    public Dictionary<int, Dictionary<int, List<int>>> fate_dic = new Dictionary<int, Dictionary<int, List<int>>>();

    public db_Accumulatedrewards_vo(string pass_value, string signin_value, string fate_value)
    {
        this.pass_value = pass_value;
        this.signin_value = signin_value;
        this.fate_value = fate_value;
        Init();
    }

    public void Init()
    { 
        string[] pass = pass_value.Split('*');
        for (int i = 0; i < pass.Length; i++)
        {
            if (pass[i] != "")
            { 
                string[] pass2 = pass[i].Split('|');
                pass_list.Add((int.Parse(pass2[0]), pass2[1]));
            }
        }
        string[] signin = signin_value.Split('*');
        for (int i = 0; i < signin.Length; i++)
        {
            if (signin[i] != "")
            { 
                string[] signin2 = signin[i].Split('|');
                signin_list.Add((int.Parse(signin2[0]), signin2[1]));
            }
        }
        string[] fate = fate_value.Split(';');
        for (int i = 0; i < fate.Length; i++)
        {
            if (fate[i] != "")
            { 
                string[] fate2 = fate[i].Split('|');
                if(!fate_dic.ContainsKey(int.Parse(fate2[0])))
                    fate_dic.Add(int.Parse(fate2[0]), new Dictionary<int, List<int>>());
                Dictionary<int, List<int>> fate_list = new Dictionary<int, List<int>>();
                string[] fate3 = fate2[1].Split(',');
                for (int j = 0; j < fate3.Length; j++)
                {
                    if (fate3[j] != "")
                    { 
                        if(!fate_list.ContainsKey(j)) fate_list.Add(j, new List<int>());
                        string[] fate4 = fate3[j].Split(' ');
                        for (int k = 0; k < fate4.Length; k++)
                        {
                            fate_list[j].Add(int.Parse(fate4[k]));
                        }
                    }
                }
                fate_dic[int.Parse(fate2[0])] = fate_list;
            }
        }

    }
}
