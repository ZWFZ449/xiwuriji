using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class db_dec : Base_VO
{
    /// <summary>
    /// 需显示消息的预制体名称
    /// </summary>
    public readonly string panel_index;
    /// <summary>
    /// 显示消息的标题
    /// </summary>
    public readonly string title;
    /// <summary>
    /// 显示消息的内容
    /// </summary>
    public readonly string dec;



    public db_dec(string _panel_index, string _title, string _dec)
    {
        this.panel_index = _panel_index;
        this.title = _title;
        this.dec = _dec;
    }

}
