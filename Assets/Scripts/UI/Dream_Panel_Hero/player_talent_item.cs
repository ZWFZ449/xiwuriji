using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_talent_item : Base_Mono
{
    private Image icon;

    private Text Lv;

    public int index;

    private void Awake()
    {

        icon = GetComponent<Image>();

        Lv = Find<Text>("info");
    }

    public void Init(int _index,object value, int lv)
    {
        index= _index;
        icon.sprite = UI.UI_Manager.I.GetEquipSprite("UI/player/player_talent/", value);
        Lv.text = lv <= 0 ? "" : ("Lv." + lv);
        if (lv == 0) icon.color = Color.gray;
    }
}
