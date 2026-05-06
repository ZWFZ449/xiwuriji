using Common;
using MVC;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class tool_Categoryt : MonoBehaviour
{

    public static tool_Categoryt Tool;
    public T Find<T>(string name)
    {
        if (transform.Find(name) == null)
        {
            Debug.LogError(this + " 子对象: " + name + " 没有找到!");
            return default(T);
        }
        return transform.Find(name).GetComponent<T>();
    }
   
    /// <summary>
    /// 获取数据列表
    /// </summary>
    /// <param name="bag"></param>
    public static Bag_Base_VO Read_BaseBag(string user_value)
    {
        string[] splits = user_value.Split(' ');
        if (splits.Length > 1)
        {
            foreach (var item in SumSave.db_stditems)
            {
                if (item.Name == splits[0])
                {
                    Bag_Base_VO bag_base = new Bag_Base_VO(item.hp, item.mp, item.ac, item.ac2, item.mac, item.mac2, item.dc, item.dc2, item.sc, item.sc2, item.mc, item.mc2);
                    bag_base.Name = item.Name;
                    bag_base.StdMode = item.StdMode;
                    bag_base.Shape = item.Shape;
                    bag_base.job = item.job;
                    bag_base.need_lv = item.need_lv;
                    bag_base.equip_lv = item.equip_lv;
                    bag_base.price = item.price;
                    bag_base.suit = item.suit;
                    bag_base.user_value = user_value;
                    return bag_base;
                }
            }
        }
        return null;
    }
    public static Bag_Base_VO crate_equip(string bag_name, bool boss=false, bool isSuperlative=false, user_map_vo map = null)
    {
        Bag_Base_VO bag = new Bag_Base_VO();
       
        return bag;
    }

    /// <summary>
    /// 获取随机数
    /// </summary>
    /// <returns></returns>
    public static int Obtain_Random()
    { 
        return Random.Range(1, 10000);
    }
    /// <summary>
    /// 获取装备品质
    /// </summary>
    /// <param name="boss"></param>
    /// <returns></returns>
    public static int Quality(bool boss = false)
    {
        int random = Random.Range(10000 * SumSave.titleLucky / 100, 100001);
        int[] needs = new int[] { 0, 23000, 43000, 61000, 75000, 90000, 99600, 100001 };
        int result = 1;
        for (int i = 0; i < needs.Length; i++)
        {
            if (random > needs[i] + (boss ? 0 : 390)) result = Mathf.Max(result, i + 1);
        }
        if (boss)
        {
            result = Mathf.Max(4, result);

            if (Random.Range(0, 100) < 10)
            {
                result = 7;
            }
        }
        result = Math.Clamp(result, 1, 7);
        return result; 
    }
}
