using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MVC;
using UI;
using System;

public class info_btn_item : Base_Mono
{

    private Text base_info;

    private Bag_Base_VO crt_bag;
    private void Awake()
    {
        base_info =  GetComponent<Text>();
        base_info.gameObject.AddComponent<Button>().onClick.AddListener(()=> { Select_Btn(crt_bag); });
    }

    public void SetInfo(string info,Bag_Base_VO vo)                                                                 
    {
        base_info.text = info;
        crt_bag = vo;
    }
    /// <summary>
    /// 点击按钮
    /// </summary>
    /// <param name="vo"></param>
    private void Select_Btn(Bag_Base_VO vo)
    {
        if (vo != null)
        { 
        Debug.Log("点击按钮");
        }
    }

    public void SetInfo(string info)
    { 
        base_info.text = info;
        crt_bag = null;
    }

}
