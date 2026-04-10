using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class equip_type_info_item : Base_Mono
{
    private Text info;
    private void Awake()
    {
        info = GetComponent<Text>();
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
