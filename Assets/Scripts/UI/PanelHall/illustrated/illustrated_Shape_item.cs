using MVC;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class illustrated_Shape_item : Base_Mono
{
    private TMP_Text info;
    /// <summary>
    /// 编号
    /// </summary>
    public db_illustrated_vo crt;
    private void Awake()
    {
        info = Find<TMP_Text>("info");
    }

    public void Init(string value, db_illustrated_vo _index)
    { 
        info.text = value;
        crt = _index;
    }
}
