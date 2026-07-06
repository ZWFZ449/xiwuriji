using CodeStage.AntiCheat.ObscuredTypes;
using MVC;
using System.Collections.Generic;
using UnityEngine;

public class db_map_vo 
{
    public readonly ObscuredInt map_id;
    /// <summary>
    /// 地图ID
    /// </summary>
    public readonly string map_name;
    /// <summary>
    /// 地图类型1普通2副本3活动
    /// </summary>
    public readonly ObscuredInt map_type;
    /// <summary>
    /// 地图等级
    /// </summary>
    public readonly ObscuredInt map_lv;

    public readonly List<string> map_monster;
    /// <summary>
    /// 地图boss
    /// </summary>
    public readonly List<string> map_boss;
    /// <summary>
    /// 地图boss刷新条件
    /// </summary>
    public readonly List<ObscuredInt> map_crate_boss_condition;
    /// <summary>
    /// 地图boss刷新时间
    /// </summary>
    public readonly List<ObscuredInt> map_boss_cdtime;
    /// <summary>
    /// 怪物刷新cd
    /// </summary>
    public readonly List<float> map_cd;

    public readonly string map_base_drop, map_drop, drop_value;
    /// <summary>
    /// 地图基础属性 刷新数量
    /// </summary>
    public readonly List<ObscuredInt> map_crate_number_monster, map_max_number_monster, map_add_number_monster;
    /// <summary>
    /// 地图强度boss收益列表 格式 1,灵宠秘籍 100/1000|2,灵宠秘籍 100/1000
    /// |分隔强度
    /// ,分隔物品列表
    /// </summary>
    public readonly Dictionary<ObscuredInt,string> map_intensity_drop;

    public readonly List<int> base_moenys = new List<int>();

    public readonly string map_lv_drop, map_boss_lv_drop;
    public readonly Dictionary<ObscuredInt, string> map_lv_intensity_drop;

    public db_map_vo(ObscuredInt map_id,string map_name,ObscuredInt map_type, ObscuredInt map_lv,List<string> map_monster, List<string> map_boss, List<ObscuredInt> map_boss_cdtime, List<float> map_cd,string map_base_drop,string map_drop,string drop_value,
        List<ObscuredInt> map_crate_number_monster, List<ObscuredInt> map_max_number_monster, List<ObscuredInt> map_add_number_monster, string map_intensity_drop,List<ObscuredInt> map_crate_boss_condition,List<int> base_moenys,
        string map_lv_drop, string map_boss_lv_drop, string map_lv_intensity_drop
        )
    { 
        this.map_id = map_id;
        this.map_name = map_name;
        this.map_lv = map_lv;
        this.map_monster = map_monster;
        this.map_boss = map_boss;
        this.map_boss_cdtime = map_boss_cdtime;
        this.map_cd = map_cd;
        this.map_type = map_type;
        this.map_base_drop = map_base_drop;
        this.map_drop = map_drop;
        this.drop_value = drop_value;
        this.map_crate_number_monster = map_crate_number_monster;
        this.map_max_number_monster = map_max_number_monster;
        this.map_add_number_monster = map_add_number_monster;
        Dictionary<ObscuredInt, string> dic=new Dictionary<ObscuredInt, string>();
        string[] map_intensity_drop_arr = map_intensity_drop.Split('|');
        for (int i = 0; i < map_intensity_drop_arr.Length; i++)
        {
            if (map_intensity_drop_arr[i].Length > 0)
            {
                string[] map_intensity_drop_arr1 = map_intensity_drop_arr[i].Split(',');
                if (map_intensity_drop_arr1.Length == 2)
                { 
                    dic.Add(int.Parse(map_intensity_drop_arr1[0]), map_intensity_drop_arr1[1]);
                }
            }
        }
        this.map_intensity_drop = dic;
        this.map_crate_boss_condition = map_crate_boss_condition;
        this.base_moenys = base_moenys;
        this.map_lv_drop = map_lv_drop;
        this.map_boss_lv_drop = map_boss_lv_drop;
        Dictionary<ObscuredInt, string> dic1 = new Dictionary<ObscuredInt, string>();
        string[] map_lv_intensity_drop_arr = map_lv_intensity_drop.Split('|');
        for (int i = 0; i < map_lv_intensity_drop_arr.Length; i++)
        {
            if (map_lv_intensity_drop_arr[i].Length > 0)
            { 
                string[] map_lv_intensity_drop_arr1 = map_lv_intensity_drop_arr[i].Split(',');
                //Debug.Log("map_lv_intensity_drop_arr1:" + map_name + " " + map_lv_intensity_drop_arr1[0]);
                if (map_lv_intensity_drop_arr1.Length == 2)
                { 
                    dic1.Add(int.Parse(map_lv_intensity_drop_arr1[0]), map_lv_intensity_drop_arr1[1]);
                }
            }
        }
        this.map_lv_intensity_drop = dic1;
    }

    private ObscuredInt map_intensity = 1;
    /// <summary>
    /// 设置地图强度
    /// </summary>
    /// <param name="map_intensity"></param>
    public void SetMapIntensity(ObscuredInt map_intensity)
    { 
        this.map_intensity = map_intensity;
    }
    /// <summary>
    /// 获取地图强度掉落
    /// </summary>
    public ObscuredInt GetMapIntensityDrop { get { return map_intensity; } }
}
