using MVC;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class info_time_item : Base_Mono
{
    private TMP_Text info;
    private void Awake()
    {
        info = Find<TMP_Text>("info");
    }

    public void SetInfo(string str)
    { 
        info.text = str;
    }
}
