using MVC;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class global_info_item : Base_Mono
{
    private TMP_Text info;

    public global_battle_info_VO data;

    private void Awake()
    {
        info = Find<TMP_Text>("info");
    }

    public void SetInfo(global_battle_info_VO vo)
    {
        info.text = vo.value.Split(';')[0];
        data = vo;
    }
}
