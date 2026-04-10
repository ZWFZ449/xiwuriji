using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class illustrated_type_item : Base_Mono
{
    private Text info;
    /// <summary>
    /// 编号
    /// </summary>
    public int index;
    private void Awake()
    {
        info = Find<Text>("info");
    }

    public void Init(object value,int _index)
    {
        info.text = value.ToString();
        index = _index;
    }
}
