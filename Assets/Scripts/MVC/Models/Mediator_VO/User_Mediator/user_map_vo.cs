using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class user_map_vo : Base_VO
{
    /// <summary>
    /// 地图编号
    /// </summary>
    public int map_index;
    /// <summary>
    /// 地图类型
    /// </summary>
    public int map_type;
    /// <summary>
    /// 地图名称
    /// </summary>
    public string map_name;
    /// <summary>
    /// 地图解锁等级
    /// </summary>
    public int need_lv;
    /// <summary>
    /// 需求门票
    /// </summary>
    public string need_Required;
    /// <summary>
    /// 收益列表
    /// </summary>
    public string ProfitList;
    /// <summary>
    /// 怪物列表
    /// </summary>
    public string monster_list;
    /// <summary>
    /// 地图五行
    /// </summary>
    public int map_life;
    /// <summary>
    /// 独立掉落
    /// </summary>
    public string Independent_Drop;

}
