using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerBabel_skill_item : Base_Mono
{

    private Image icon, item_frame;

    private TMP_Text info;
    private void Awake()
    {
        icon = Find<Image>("icon");
        info = Find<TMP_Text>("info");
        item_frame = GetComponent<Image>();
    }

    private db_towerbabel_vo data;

    /// <summary>
    /// Data
    /// </summary>
    public db_towerbabel_vo Data
    {
        set
        {
            data = value;
            if (data == null) return;
            Show_Skill();
        }
        get
        {
            return data;
        }
    }

    public void refresh()
    {
        Show_Skill();
    }
    private void Show_Skill()
    {
        icon.sprite = UI.UI_Manager.I.GetEquipSprite("towerbabel/", data.TowerBabel_name);
        Transparent(icon, data.user_lv == 0 ? 0.3f : 1f);
        item_frame.sprite = UI.UI_Manager.I.GetEquipSprite("UI/frame/", data.user_lv / 2 + 1);
        Transparent(item_frame, data.user_lv == 0 ? 0.3f : 1f);
    }
    public void Transparent(Image _icon, float alpha)
    {
        if (_icon == null) return;
        // 获取当前颜色
        Color currentColor = _icon.color;
        // 修改Alpha值（确保在0~1范围内）
        currentColor.a = Mathf.Clamp01(alpha);
        // 应用新的颜色
        _icon.color = currentColor;
    }
}
