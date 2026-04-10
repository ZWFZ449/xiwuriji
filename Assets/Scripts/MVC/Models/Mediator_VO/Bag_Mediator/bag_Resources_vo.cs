using Common;
using MVC;
using System.Collections.Generic;
using UnityEngine;

public class bag_Resources_vo : Base_VO
{
    private List<(string,int)> list = new List<(string,int)>();
    private List<(string, int)> verify_list = new List<(string, int)>();
    private int index = -1;
    public void Init(string value)
    {
        index = Random.Range(1, 1000);
        list = new List<(string, int)>();
        string[] artifact_value_array = value.Split(',');
        if (artifact_value_array.Length >= 1)
        {
            for (int i = 0; i < artifact_value_array.Length; i++)
            {
                string[] artifact_array = artifact_value_array[i].Split(' ');
                if (artifact_array.Length > 1)
                {
                    list.Add((artifact_array[0], int.Parse(artifact_array[1])));
                    verify_list.Add((artifact_array[0], int.Parse(artifact_array[1]) + index));
                }
            }
        }
    }
    /// <summary>
    /// 读取标准
    /// </summary>
    /// <returns></returns>
    public List<(string, int)> Set()
    {
        return list;
    }
    /// <summary>
    /// 传入参数
    /// </summary>
    /// <param name="list"></param>
    public void Get(Dictionary<string, int> dec,int maxnumber,bool exist = false)
    {
        bool crate_state = false;
        //验证数据
        for (int i = 0; i < list.Count; i++)
        {
            //原始数据发生改变
            if (list[i].Item2 + index != verify_list[i].Item2)
            {
                //验证数据
                Game_Omphalos.i.Delete(list[i].Item1 + " 显示数据 " + list[i].Item2 + " 验证值 " + index + " " + verify_list[i].Item2);
            }
        }
        //写入数据
        foreach (var item in dec.Keys)
        {
            crate_state = true;
            for (int i = 0; i < list.Count; i++)
            {
                if (item == list[i].Item1)
                {
                    crate_state = false;
                    if (exist)
                    {
                        if (dec[item] > maxnumber)
                        {
                            Game_Omphalos.i.Delete(item + " 单次获取 " + list[i].Item2 + " 验证值 " + index + " " + verify_list[i].Item2 + " 单次最大值" + maxnumber);
                        }
                    }
                    else
                    {
                        if (dec[item] > SumSave.base_setting[3])
                        {
                            Game_Omphalos.i.Delete(item + " 单次获取 " + list[i].Item2 + " 验证值 " + index + " " + verify_list[i].Item2);
                        }
                    }
                    list[i] = (item, dec[item] + list[i].Item2);
                    verify_list[i] = (item, verify_list[i].Item2 + dec[item]);
                }
            }
            if (crate_state)
            {
                if (dec[item] > maxnumber)
                {
                    Game_Omphalos.i.Delete(item + "当前值" + dec[item] + " 单次最大值 " + maxnumber);
                }
                list.Add((item, dec[item]));
                verify_list.Add((item, dec[item] + index));
            }
        }
    }
    /// <summary>
    /// 转换格式
    /// </summary>
    /// <returns></returns>
    public string GetData()
    {
        string data_list= "";


        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i].Item2 <= 0)
            {
                list.RemoveAt(i);
                verify_list.RemoveAt(i);
            }
        }

        for (int i = 0; i < list.Count; i++)
        {
            data_list += (i == 0 ? "" : ",") + (list[i].Item1 + " " + list[i].Item2);
        }
        return data_list;
    }
}
