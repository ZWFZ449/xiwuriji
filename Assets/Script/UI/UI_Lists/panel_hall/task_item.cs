using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class task_item : Base_Mono
{
    /// <summary>
    /// 显示信息
    /// </summary>
    private Text task_info;
    /// <summary>
    /// 状态
    /// </summary>
    private Image state;
    /// <summary>
    /// 编号
    /// </summary>
    public int index;
    private void Awake()
    {
        task_info=Find<Text>("info");
        state = Find<Image>("state");
    }

    public void Show(int i,int value, bool isFinish=false)
    {
        index = i;
        string dec = "";
        switch (index)
        {
            case 0:
                if (isFinish) value = 120;
                 dec += Show_Color.Yellow("在线120分钟") + "\n" + "进度:" + Show_Color.Blue(value + "/120")
                    + "\n" + "奖励:" + Show_Color.Red("命运金币 * 1");
                break;
            case 1:
                if (isFinish) value = 1;
                dec += Show_Color.Yellow("开启小世界种植") + "\n" + "进度:" + Show_Color.Blue(value + "/1")
                    + "\n" + "奖励:" + Show_Color.Red("命运金币 * 1");
                break;
            case 2:
                if (isFinish) value = 10;
                dec += Show_Color.Yellow("击杀10个普通Boss") + "\n" + "进度:" + Show_Color.Blue(value + "/10")
                    + "\n" + "奖励:" + Show_Color.Red("命运金币 * 1");
                break;
            case 3:
                if (isFinish) value = 10;
                dec += Show_Color.Yellow("击杀10个副本Boss") + "\n" + "进度:" + Show_Color.Blue(value + "/10")
                    + "\n" + "奖励:" + Show_Color.Red("命运金币 * 1");
                break;
            case 4:
                if (isFinish) value = 1;
                dec += Show_Color.Yellow("击杀1个深渊Boss") + "\n" + "进度:" + Show_Color.Blue(value + "/1")
                    + "\n" + "奖励:" + Show_Color.Red("命运金币 * 1");
                break;
            case 5:
                if (isFinish) value = 1;
                dec += Show_Color.Yellow("炼制丹药") + "\n" + "进度:" + Show_Color.Blue(value + "/1")
                    + "\n" + "奖励:" + Show_Color.Red("命运金币 * 1");
                break;
            default:
                break;
        }
        task_info.text = dec;
        state.gameObject.SetActive(isFinish);
    }

    public void progress(int value,bool isFinish)
    {
        string dec = "";
        switch (index)
        {
            case 0:
                if (isFinish) value = 120;
                dec += Show_Color.Yellow("在线120分钟") + "\n" + "进度:" + Show_Color.Blue( value+"/120")
                    + "\n" + "奖励:" + Show_Color.Red("命运金币 * 1");
                break;
            case 1:
                if (isFinish) value = 1;
                dec += Show_Color.Yellow("开启小世界种植") + "\n" + "进度:" + Show_Color.Blue(value+"/1")
                    + "\n" + "奖励:" + Show_Color.Red("命运金币 * 1");
                break;
            case 2:
                if (isFinish) value = 10;
                dec += Show_Color.Yellow("击杀10个普通Boss") + "\n" + "进度:" + Show_Color.Blue(value+"/10")
                    + "\n" + "奖励:" + Show_Color.Red("命运金币 * 1");
                break;
            case 3:
                if (isFinish) value = 10;
                dec += Show_Color.Yellow("击杀10个副本Boss") + "\n" + "进度:" + Show_Color.Blue(value+"/10")
                    + "\n" + "奖励:" + Show_Color.Red("命运金币 * 1");
                break;
            case 4:
                if (isFinish) value = 1;
                dec += Show_Color.Yellow("击杀1个深渊Boss") + "\n" + "进度:" + Show_Color.Blue(value+"/1")
                    + "\n" + "奖励:" + Show_Color.Red("命运金币 * 1");
                break;
            case 5:
                if (isFinish) value = 1;
                dec += Show_Color.Yellow("炼制丹药") + "\n" + "进度:" + Show_Color.Blue(value+"/1")
                    + "\n" + "奖励:" + Show_Color.Red("命运金币 * 1");
                break;
            default:
                break;
        }
        task_info.text = dec;
        state.gameObject.SetActive(isFinish);
    }
    /// <summary>
    /// 是否完成
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool State(int value)
    {
        int MaxValue = 0;
        switch (index)
        {
            case 0:
                MaxValue = 120;
                break;
            case 1:
                MaxValue = 1;
                break;
            case 2:
                MaxValue = 10;
                break;
            case 3:

                MaxValue = 10;
                break;
            case 4:
                MaxValue = 1;
                break;
            case 5:
                MaxValue = 1;
                break;
            default:
                break;
        }
        return value >= MaxValue;

    }
}
