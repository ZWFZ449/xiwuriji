using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 获取int类型值
/// </summary>
public static class Obtain_Int 
{
    /// <summary>
    /// 获取指针类型值
    /// </summary>
    private static Dictionary<int,int[]> dic = new Dictionary<int, int[]>();
    /// <summary>
    /// 获取物品指针
    /// </summary>
    private static Dictionary<int, object> resources = new Dictionary<int, object>();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int Add(int key, object resources_name, int[] value)
    {
        int index = key;
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
    /// 获取int类型值
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static Dictionary<string,int> Get(int key)
    {
        Dictionary<string, int> valuePairs = new Dictionary<string, int>();
        valuePairs.Add(resources[key].ToString(), dic[key][0] - dic[key][1]);
        dic.Remove(key);
        resources.Remove(key);
        return valuePairs;

    }
}
