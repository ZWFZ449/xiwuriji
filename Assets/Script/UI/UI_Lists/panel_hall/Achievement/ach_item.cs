using Common;
using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ach_item : Base_Mono
{
    private Text info;
    private bool active = false;
    private void Awake()
    {
        info = Find<Text>("info");
    }
    /// <summary>
    /// 显示信息
    /// </summary>
    /// <param name="value"></param>
    public void Show_Info(object value)
    {
        info.text = value.ToString();
    }
    private db_achievement_VO data;
    /// <summary>
    /// Data
    /// </summary>
    public db_achievement_VO Data
    {
        set
        {
            data = value;
            Init();
        }
        get
        {
            return data;
        }
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        Dictionary<string, long> dic_exp = SumSave.crt_achievement.Set_Exp();
        Dictionary<string,long> dic_lv = SumSave.crt_achievement.Set_Lv();
        //info.text = "显示成就具体信息";
        info.text = "";
        if(dic_lv.ContainsKey(data.achievement_value))
        {
            if (dic_lv[data.achievement_value] >= data.achievement_needs.Count)
            {
                info.text = data.achievement_show_lv[data.achievement_show_lv.Length - 1] + " (" + dic_exp[data.achievement_value] + "/Max)";
            }
            else
            {
                //Debug.Log("长度"+ data.achievement_show_lv.Length+"等级：" + dic_lv[data.achievement_value]);
                if (data.achievement_show_lv.Length - 1 <= dic_lv[data.achievement_value])
                {
                    info.text += data.achievement_show_lv[data.achievement_show_lv.Length - 1];
                }
                else
                {
                    if (dic_lv[data.achievement_value] == 0)
                    {
                        info.text += data.achievement_show_lv[0];
                    }
                    else
                    {
                        info.text += data.achievement_show_lv[dic_lv[data.achievement_value]];
                    }

                }
                info.text += " (" + dic_exp[data.achievement_value] + "/" + data.achievement_needs[(int)dic_lv[data.achievement_value]] + ")";
            }
        }
        else
        {
            info.text += data.achievement_show_lv[0];
            info.text += " (" +"0" + "/" + data.achievement_needs[0] + ")";
        }
        

    }
}
