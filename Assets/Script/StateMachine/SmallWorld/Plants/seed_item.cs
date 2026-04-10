using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class seed_item : Base_Mono
{
    /// <summary>
    /// 当前种子数据
    /// </summary>
    public user_plant_vo db_plant;

    private Image icon;

    private Text info;
    /// <summary>
    /// 索引位置
    /// </summary>
    public int index = -1;

    private DateTime crt_time;

    private int growTimeInt = 0;
    /// <summary>
    /// 可以收获
    /// </summary>
    private int isMature = 0;

    private bool exist = true;
    private void Awake()
    {
        icon = Find<Image>("icon");
        info = Find<Text>("info");

    }
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="db_plant_">基准参数</param>
    /// <param name="_index">编号</param>
    /// <param name="_crt_time">种植时间</param>
    public void Init(user_plant_vo db_plant_,DateTime _crt_time)
    {
        exist = false;
        db_plant = db_plant_;
        crt_time = _crt_time;
        icon.gameObject.SetActive(true);
        icon.sprite = UI.UI_Manager.I.GetEquipSprite("icon/", db_plant.plantName);
        GetList();
    }

    private void GetList()
    {
        growTimeInt = Battle_Tool.SettlementTransport(crt_time.ToString(),2);// (int)(SumSave.nowtime - crt_time).TotalSeconds;//当前时间-植物种植时间 获得植物种植到现在的时间
        if (growTimeInt <= db_plant.plantTime)//植物已经生长的时间小于植物需要生长的时间
        {
            growTimeInt = db_plant.plantTime - growTimeInt;
            info.text = ConvertSecondsToHHMMSS(growTimeInt);

        }
        else
        {
            icon.sprite = UI.UI_Manager.I.GetEquipSprite("icon/", db_plant.HarvestMaterials);
            info.text = "已成熟";
            growTimeInt = -1;
            isMature = 1;
        }
    }
    /// <summary>
    /// 是否为空地
    /// </summary>
    /// <returns></returns>
    public bool state()
    { 
        return exist;
    }
    /// <summary>
    /// 是否可以收获
    /// </summary>
    /// <returns></returns>
    public bool isMatured()
    { 
        return isMature == 1;
    }

    public void updata_time(DateTime _crt_time)
    {
        crt_time= _crt_time;
        GetList();
    }
    /// <summary>
    /// 刷新状态
    /// </summary>
    /// <param name="time"></param>
    public void Fixed_Update(int time)//显示植物倒计时
    {
        if (growTimeInt > 0)
        {
            growTimeInt -= time;
            info.text = ConvertSecondsToHHMMSS(growTimeInt);
        }
        else
        {
            if (exist)
            {
                info.text = "可播种";
                growTimeInt = -1;
            }
            else
            {
                icon.sprite = UI.UI_Manager.I.GetEquipSprite("icon/", db_plant.HarvestMaterials);
                info.text = "已成熟";
                growTimeInt = -1;
                isMature = 1;
            }

        }
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public void Clear()
    { 
        db_plant = null;
        growTimeInt = 0;
        icon.gameObject.SetActive(false);
        isMature = 0;
        exist = true;
    }
}
