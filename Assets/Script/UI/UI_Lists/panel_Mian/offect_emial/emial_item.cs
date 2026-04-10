using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class emial_item : Base_Mono
{
    /// <summary>
    /// 展示信息
    /// </summary>
    private Text info;

    private Image state;

    public db_mail_vo crt_mail;
    private void Awake()
    {
        info = Find<Text>("info");
        state = Find<Image>("state");
    }

    /// <summary>
    /// 显示设置
    /// </summary>
    /// <param name="str"></param>
    public void SetInfo(db_mail_vo str,bool exist)
    {
        crt_mail = str;
        info.text=str.uid=="-1"?"系统邮件":"玩家邮件";
        info.text += "\n" + str.mail_time;
        state.gameObject.SetActive(exist);
    }
}
