using MVC;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class buff_item : Base_Mono
{
    private TMP_Text info;
    private Image icon;

    private void Awake()
    {
        info = Find<TMP_Text>("info");
        icon = GetComponent<Image>();
    }

    public void Init(string path,string info)
    { 
        icon.sprite = UI.UI_Manager.I.GetEquipSprite("UI/buff/", path);
        this.info.text = info;
    }
}
