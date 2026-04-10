using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class global_info_item : MonoBehaviour
{
    private Text info;

    public global_battle_info_VO data;

    private void Awake()
    {
        info = GetComponent<Text>();
    }

    public void SetInfo(global_battle_info_VO vo)
    {
        info.text = vo.value.Split(';')[0];
        data = vo;
    }
}
