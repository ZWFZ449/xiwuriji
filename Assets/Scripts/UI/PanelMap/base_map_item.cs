using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class base_map_item : Base_Mono 
{
    private Text map_name, map_lv, map_boss, map_boss_cdtime;
    private Button b_map_name, b_map_lv, b_map_boss, b_map_boss_cdtime;
    /// <summary>
    /// 当前地图数据
    /// </summary>
    private db_map_vo crt_map;
    /// <summary>
    /// 地图强度默认1级
    /// </summary>
    private int map_intensity = 1;
    private void Awake()
    {
        map_name=Find<Text>("map_name/info");
        map_lv = Find<Text>("map_lv/info");
        map_boss = Find<Text>("map_boss/info");
        map_boss_cdtime = Find<Text>("map_boss_cdtime/info");
        b_map_name = Find<Button>("map_name");
        b_map_lv = Find<Button>("map_lv");
        b_map_boss = Find<Button>("map_boss");
        b_map_boss_cdtime = Find<Button>("map_boss_cdtime");
        b_map_name.onClick.AddListener(OnClickMap);
        b_map_lv.onClick.AddListener(OnClickMap);
        b_map_boss.onClick.AddListener(OnClickMap);
        b_map_boss_cdtime.onClick.AddListener(OnClickMap);
    }

    /// <summary>
    /// 地图点击事件
    /// </summary>
    private void OnClickMap()
    {
        transform.parent.parent.parent.parent.parent.parent.SendMessage("OnClickBaseMap", this);
    }
    /// <summary>
    /// 获取当前地图数据
    /// </summary>
    /// <returns></returns>
    public db_map_vo GetMap()
    { 
      return crt_map;
    }
    /// <summary>
    /// 初始化地图数据
    /// </summary>
    /// <param name="map"></param>
    public void Init(db_map_vo map)
    {
        crt_map = map;
        map_name.text= map.map_name;
        map_lv.text = map.map_lv.ToString();
        map_boss.text = map.map_boss[map_intensity-1];
        //map_boss_cdtime.text = map.map_boss_cdtime[map_intensity - 1].ToString();
    }
    /// <summary>
    /// 设置地图强度
    /// </summary>
    /// <param name="intensity"></param>
    public void Select_Map_Intensity(int intensity)
    { 
       map_intensity = intensity;
    }
    /// <summary>
    /// 获取地图强度
    /// </summary>
    public int GetMap_Intensity { get { return map_intensity; } }


}
