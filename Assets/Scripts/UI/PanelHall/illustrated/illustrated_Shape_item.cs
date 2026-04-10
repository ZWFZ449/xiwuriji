using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class illustrated_Shape_item : Base_Mono
{
    private Text info;
    /// <summary>
    /// 编号
    /// </summary>
    public db_illustrated_vo crt;
    private void Awake()
    {
        info = Find<Text>("info");
    }

    public void Init(string value, db_illustrated_vo _index)
    { 
        info.text = value;
        crt = _index;
    }
}
