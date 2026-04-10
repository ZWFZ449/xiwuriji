using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class illustrated_item : Base_Mono
{
    private Text info;
    private Image frame;
    private Image icon;
    /// <summary>
    /// 标记
    /// </summary>
    private string index;

    private float basealpha = 0.2f;
    /// <summary>
    /// 基准值
    /// </summary>
    private db_illustrated_vo crt;
    private void Awake()
    {
        info = Find<Text>("info");
        icon = Find<Image>("icon");
        frame = GetComponent<Image>();
    }

    public void InitMonster(string value,int crt_alpha,int max_alpha,db_illustrated_vo _crt)
    {
        crt = _crt;
        index = value;
        basealpha = max_alpha / 1f;
        alpha(crt_alpha);
        info.text = value;
        icon.sprite = UI.UI_Manager.I.GetEquipSprite("monster/", value);
    }

    public void InitIcon(string value,int crt_alpha,db_illustrated_vo _crt)
    {
        crt = _crt;
        index = value;
        basealpha = 1;
        alpha(crt_alpha);
        info.text = value;
        icon.sprite = UI.UI_Manager.I.GetEquipSprite("icon/", value);
    }
    public db_illustrated_vo GetCrt { get { return crt; } }
    public string GetIndex { get { return index; } }
    /// <summary>
    /// 显示状态
    /// </summary>
    /// <param name="number"></param>
    public void SetCount(int number)
    { 
        info.text=number.ToString();
        alpha(number);
    }
    /// <summary>
    /// 透明度
    /// </summary>
    /// <param name="crt_alpha"></param>
    private void alpha(int crt_alpha)
    { 
        float alpha = crt_alpha / basealpha;
        if(alpha<=0.3f) alpha = 0.3f;
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, alpha);
        if (alpha >= 1) frame.color = Color.yellow;
    }
}