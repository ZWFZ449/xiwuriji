using MVC;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class equip_type_info_item : Base_Mono
{
    private TMP_Text info;
    private void Awake()
    {
        info = Find<TMP_Text>("info");
    }
    /// <summary>
    /// 显示信息
    /// </summary>
    /// <param name="value"></param>
    public void Init(string value,Color color)
    { 
        info.text = value;
        info.color = color;
    }
}
