using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class oldskill_item : Base_Mono
{
    private Image item_icon, item_frame;
    private Text info;
    private void Awake()
    {
        item_icon = Find<Image>("frame/icon");
        item_frame = Find<Image>("frame/state");
        info = Find<Text>("info");
    }

    private base_skill_vo data;

    /// <summary>
    /// Data
    /// </summary>
    public base_skill_vo Data
    {
        set
        {
            data = value;
            if (data == null) return;
            Refresh();
        }
        get
        {
            return data;
        }
    }
    /// <summary>
    /// 刷新一下
    /// </summary>
    public void Refresh()
    {
        item_icon.sprite = UI.UI_Manager.I.GetEquipSprite("icon/", data.skillname);
        if(data.skill_type==1) item_icon.sprite = UI.UI_Manager.I.GetEquipSprite("skill/", data.skillname);
        info.text = Data.skillname + "Lv." + Data.user_values[1];
        item_frame.gameObject.SetActive(data.skillpos != 0);
    }

}
