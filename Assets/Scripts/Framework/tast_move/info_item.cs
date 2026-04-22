using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVC;
using UnityEngine.UI;
using TMPro;

public class info_item : Base_Mono
{
    private TMP_Text base_type, base_value;
    private void Awake()
    {
        base_value = Find<TMP_Text>("base_value");
        base_type = Find<TMP_Text>("base_type");
    }
    /// <summary>
    /// 显示信息
    /// </summary>
    /// <param name="type">类型</param>
    /// <param name="value">值</param>
    public void Show(object type, object value)
    { 
        base_type.text = type.ToString();
        base_value.text = value.ToString();
    }

    public void SetInfo(object type, object value)
    {
        base_type.text = type.ToString();
        base_value.text = value.ToString();
    }
    public void SetPetInfo(object type, object value)
    {
        base_type.text = type.ToString();
        //base_type.color = UnityColorPresets.HexToColor("#ffe400");
        base_value.text = value.ToString();
    }
    public void SetInfo(object type, object value, Color C)
    {
        base_type.text = type.ToString();
        base_type.color = C;
        base_value.text = value.ToString();
    }
}
