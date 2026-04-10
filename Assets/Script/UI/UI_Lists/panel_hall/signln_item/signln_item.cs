using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class signln_item : Base_Mono
{
    private db_signin_vo crt_vo;
    private Text info;
    private Image icon,state;
    private int index;
    private void Awake()
    {
        info=Find<Text>("info");
        icon = Find<Image>("icon");
        state = Find<Image>("state");
    }

    /// <summary>
    /// 获取状态
    /// </summary>
    /// <param name="_index"></param>
    /// <param name="db_signin_vo"></param>
    /// <param name="state"></param>
    public void Init(int _index, db_signin_vo db_signin_vo,int _state)
    {
        crt_vo=db_signin_vo;
        string[] strs = crt_vo.value.Split('*');
        string dec = "";// strs[0] + "*" + strs[1];
        dec += "" + crt_vo.index + "天奖励";
        info.text = dec;
        state.gameObject.SetActive(_state == 1);
        index = _index;
        icon.sprite = UI.UI_Manager.I.GetEquipSprite("icon/", strs[0]);
    }
    /// <summary>
    /// 获取编号索引
    /// </summary>
    /// <returns></returns>
    public int Set()
    {
        return index;
    }
}
