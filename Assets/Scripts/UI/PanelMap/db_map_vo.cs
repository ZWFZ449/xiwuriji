using MVC;
using System.Collections.Generic;

public class db_map_vo 
{
    public readonly int map_id;
    /// <summary>
    /// 地图ID
    /// </summary>
    public readonly string map_name;
    /// <summary>
    /// 地图类型1普通2副本3活动
    /// </summary>
    public readonly int map_type;
    /// <summary>
    /// 地图等级
    /// </summary>
    public readonly int map_lv;

    public readonly List<string> map_monster;
    /// <summary>
    /// 地图boss
    /// </summary>
    public readonly List<string> map_boss;
    /// <summary>
    /// 地图boss刷新条件
    /// </summary>
    public readonly List<int> map_crate_boss_condition;
    /// <summary>
    /// 地图boss刷新时间
    /// </summary>
    public readonly List<int> map_boss_cdtime;
    /// <summary>
    /// 怪物刷新cd
    /// </summary>
    public readonly List<float> map_cd;

    public readonly string map_base_drop, map_drop, drop_value;
    /// <summary>
    /// 地图基础属性 刷新数量
    /// </summary>
    public readonly List<int> map_crate_number_monster, map_max_number_monster, map_add_number_monster;
    /// <summary>
    /// 地图强度boss收益列表 格式 1,灵宠秘籍 100/1000|2,灵宠秘籍 100/1000
    /// |分隔强度
    /// ,分隔物品列表
    /// </summary>
    public readonly Dictionary<int,string> map_intensity_drop;

    public db_map_vo(int map_id,string map_name,int map_type, int map_lv,List<string> map_monster, List<string> map_boss, List<int> map_boss_cdtime, List<float> map_cd,string map_base_drop,string map_drop,string drop_value,
        List<int> map_crate_number_monster, List<int> map_max_number_monster, List<int> map_add_number_monster, string map_intensity_drop,List<int> map_crate_boss_condition)
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
        Dictionary<int, string> dic=new Dictionary<int, string>();
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
    }

    private int map_intensity = 1;
    /// <summary>
    /// 设置地图强度
    /// </summary>
    /// <param name="map_intensity"></param>
    public void SetMapIntensity(int map_intensity)
    { 
        this.map_intensity = map_intensity;
    }
    /// <summary>
    /// 获取地图强度掉落
    /// </summary>
    public int GetMapIntensityDrop { get { return map_intensity; } }
}
