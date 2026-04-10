using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pet_talent_item : Base_Mono
{
    private Image icon, item_frame;

    private db_pet_talent_vo talent_value;
    private void Awake()
    {
        icon = Find<Image>("icon");
        item_frame = GetComponent<Image>();
    }

    public void Init(db_pet_talent_vo value)
    {
        talent_value = value;
        item_frame.sprite = UI.UI_Manager.I.GetEquipSprite("UI/frame/", value.pet_talent_level + 2);
        icon.sprite = UI.UI_Manager.I.GetEquipSprite("UI/pet/pet_talent/", value.pet_talent_name);
    }
    /// <summary>
    /// 获取属性值
    /// </summary>
    public db_pet_talent_vo GetTalentValue { get { return talent_value; } }
}
