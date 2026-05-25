using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 获取ObscuredInt类型值
/// </summary>
public static class Obtain_Int 
{
    /// <summary>
    /// 获取指针类型值
    /// </summary>
    private static Dictionary<ObscuredInt,ObscuredInt[]> dic = new Dictionary<ObscuredInt, ObscuredInt[]>();
    /// <summary>
    /// 获取物品指针
    /// </summary>
    private static Dictionary<ObscuredInt, object> resources = new Dictionary<ObscuredInt, object>();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static ObscuredInt Add(ObscuredInt key, object resources_name, ObscuredInt[] value)
    {
        ObscuredInt index = key;
        if (!dic.ContainsKey(key))
        {
            dic.Add(key, value);
            resources.Add(key, resources_name);
        }
        else
        {
            while (dic.ContainsKey(index))
            {
                index++;
            }
            dic.Add(index, value);
            resources.Add(index, resources_name);
        }
        return index;
    }
    /// <summary>
    /// 隐藏指针
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string Add_unit(long key)
    {
        long index = Random.Range(100, 10000);
        string value = (key+ index)+","+index;
        return value;
    }

    /// <summary>
    /// 获取ObscuredInt类型值
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static Dictionary<string,ObscuredInt> Get(ObscuredInt key)
    {
        Dictionary<string, ObscuredInt> valuePairs = new Dictionary<string, ObscuredInt>();
        valuePairs.Add(resources[key].ToString(), dic[key][0] - dic[key][1]);
        dic.Remove(key);
        resources.Remove(key);
        return valuePairs;

    }
}
