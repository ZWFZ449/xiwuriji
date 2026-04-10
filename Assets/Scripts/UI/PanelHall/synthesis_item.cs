using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class synthesis_item : Base_Mono
{
    private Image icon;

    private Text info;

    private void Awake()
    {
        icon = Find<Image>("icon/icon");
        info = Find<Text>("show_info/info");
    }

    public void Init(string value)
    {
        icon.sprite = UI.UI_Manager.I.GetEquipSprite("icon/", value);
        info.text = value;
    }
    private db_synthesis_vo data;
    public db_synthesis_vo Data
    {
        get { return data; }
        set { data = value; Init(data.synthesis_name); } 
    }
}
