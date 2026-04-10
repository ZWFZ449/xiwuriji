using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class explore_item : Base_Mono
{
    private Image icon;

    private db_pet_vo_old crt_pet;

    private void Awake()
    {
        icon = Find<Image>("icon");

    }
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="pet"></param>
    public void Init(db_pet_vo_old pet)
    { 
        crt_pet = pet;
        icon.sprite = UI.UI_Manager.I.GetEquipSprite("UI/pet/", pet.petName);
    }

    /// <summary>
    /// 获取当前数据
    /// </summary>
    /// <returns></returns>
    public db_pet_vo_old SetData()
    {
        return crt_pet;
    }

    public bool Selected
    {
        set {GetComponent<Image>().color = value ? Color.red : Color.white; }
    }
}
