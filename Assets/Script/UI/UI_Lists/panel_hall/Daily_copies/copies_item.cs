using Common;
using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class copies_item : Base_Mono
{
    /// <summary>
    /// 显示图片
    /// </summary>
    private Image icon;
    /// <summary>
    /// 显示信息
    /// </summary>
    private Text info,base_name;

    public user_map_vo index;

    private int number, maxnumber;
    /// <summary>
    /// 五行类型
    /// </summary>
    private string[] five_element_type = { "土", "火", "水", "木", "金" };
    private void Awake()
    {
        icon=Find<Image>("bg/icon");
        base_name = Find<Text>("bg/icon/name");
        info =Find<Text>("info");
    }

    public void Init(user_map_vo map, int _number, int _maxnumber)
    {
        index = map;
        number = _number;
        maxnumber = _maxnumber;
        info.text = map.map_name + "(" + number + "/" + maxnumber + ")";
        icon.sprite = Resources.Load<Sprite>("Prefabs/monsters/" + map.monster_list);
 
        base_name.text = "(" + five_element_type[map.map_life-1] + ")" + "[Boss]" + map.monster_list;

    }

    /// <summary>
    /// 秘境界面显示
    /// </summary>
    /// <param name="map"></param>
    public void InitSecretRealm(user_map_vo map,int _num)
    {
        index = map;
        info.text = map.map_name+"("+_num+")";
        icon.sprite = Resources.Load<Sprite>("Prefabs/monsters/" + map.monster_list);

        base_name.text = "(" + five_element_type[map.map_life - 1] + ")" + "[Boss]" + map.monster_list;

    }



    /// <summary>
    /// 显示信息
    /// </summary>
    /// <returns></returns>
    public string ShowInfo()
    {
        return "\n 挑战次数 " + number + "/" + maxnumber + " 次";
    }
    /// <summary>
    /// 是否可以挑战
    /// </summary>
    /// <returns></returns>
    public bool IsSate()
    {
        return number < maxnumber;
    }
    /// <summary>
    /// 刷新状态
    /// </summary>
    public void updatestate()
    {
        number++;
        info.text = index.map_name + "(" + number + "/" + maxnumber + ")";

    }
}
