using MVC;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class select_gem_btn_item : Base_Mono
{
    private Image icon, frame;

    private TMP_Text info;

    public int index;
    private void Awake()
    {
        icon=GetComponent<Image>();
        info=Find<TMP_Text>("info");
        frame = Find<Image>("frame");
    }

    public void SetData(string id,int index)
    { 
        this.index = index;
        info.text = id;
        icon.sprite = UI.UI_Manager.I.GetEquipSprite("icon/", id);
    }

    public bool Select_State { set { frame.color = value ? Color.red : Color.white; } } 
}
