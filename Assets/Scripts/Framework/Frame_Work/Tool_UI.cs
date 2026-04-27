using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tool_UI
{
    /// <summary>
    /// 获取预制体存储
    /// </summary>
    private static Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
    /// <summary>
    /// 查找预制体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="prefabName"></param>
    /// <returns></returns>
    public static T Find_Prefabs<T>(string prefabName)
    {
        if (!prefabs.ContainsKey(prefabName))
        {
            GameObject obj = Resources.Load<GameObject>("UI/Prefabs/" + prefabName);
            prefabs.Add(prefabName, obj);
        }
        return prefabs[prefabName].GetComponent<T>();
    }
    /// <summary>
    /// 根据16进制颜色码转换成Color
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    private static Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

    public static string Obtain_Talent_Name()
    { 
        string name = "";

        return name;
    }
    /// <summary>
    /// 根据路径获取图集
    /// </summary>
    /// <param name="path"></param>
    /// <param name="subpath"></param>
    public static Sprite Obtain_Sprite(string path, string subpath)
    {
        if(sprites.ContainsKey(subpath))return sprites[subpath];
        Sprite[] all = Resources.LoadAll<Sprite>(path);
        foreach (var s in all)
        {
            if (s.name == subpath)
            { 
                sprites.Add(subpath, s);
            }
        }
        if (sprites.ContainsKey(subpath)) return sprites[subpath];
        else return null;
    }
    /// <summary>
    /// 统一时间格式
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static string ToStandardFormat(DateTime time)
    {
        return UnifiedDateTime.ToString(time);
    }
}
