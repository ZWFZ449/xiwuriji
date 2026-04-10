using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class skill_offect_item : Base_Mono
{
    /// <summary>
    /// 技能索引
    /// </summary>
    public int index;
    /// <summary>
    /// 基础信息
    /// </summary>
    private Text info;
    /// <summary>
    /// 战斗倒计时
    /// </summary>
    private Text WaitTime;
    private Image item_icon;
    private void Awake()
    {
        info = Find<Text>("info");
        WaitTime = Find<Text>("WaitTime");
        item_icon = GetComponent<Image>();
    }
    private db_skill_vo data;
     
    private void baseInfo()
    {
        item_icon.sprite = UI.UI_Manager.I.GetEquipSprite("skill/base_icon/", data.show_name);
        info.text = "";
        //info.text = data.show_name + "\n" + (skill_Lv_Type)data.SetLv();
    }
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="_index"></param>
    /// <param name="_data"></param>
    public void Init(int _index,db_skill_vo _data)
    { 
        data= _data;
        index = _index;
        baseInfo();
    }
    /// <summary>
    /// 获取
    /// </summary>
    public db_skill_vo GetData { get { return data; } }
    /// <summary>
    /// 需要的MP
    /// </summary>
    public int Get_Mp { get { return data.spells[data.SetLv()]; } } 
   
}
