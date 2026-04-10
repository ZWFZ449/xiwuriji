
using Common;
using System;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class panltItem : Panel_Base
{
    

    /// <summary>
    /// 植物名称
    /// </summary>
    public string plantName; // 植物名称
    /// <summary>
    /// 生长时间
    /// </summary>
    public int growTime;
    /// <summary>
    /// 成熟时间
    /// </summary>
    private int growTimeInt;
    /// <summary>
    /// 植物种植时间
    /// </summary>
    private DateTime currentGrowTimeDate;
    /// <summary>
    /// 是否已浇水
    /// </summary>
    public int  isWatered = 0; //0：不可浇水 1：可浇水
    /// <summary>
    /// 是否已成熟
    /// </summary>
    public int  isMature = 0;  //0：不可收获 1：可收获
    /// <summary>
    /// 植物Image
    /// </summary>
    public Image image; 
    /// <summary>
    /// 倒计时Text
    /// </summary>
    public Text CountdownText;
    /// <summary>
    /// 索引位置
    /// </summary>
    private int index = -1;

   protected override void Awake()
    {
        image = GetComponent<Image>();
        CountdownText = GetComponentInChildren<Text>();
        CountdownText.transform.gameObject.SetActive(false);
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        growTime = 0;
        isWatered = 0;
        isMature = 0;
    }


    /// <summary>
    /// 获取索引
    /// </summary>
    /// <returns></returns>
    public int Obtain_Index()
    {
        return index;
    }

    /// <summary>
    /// 设置索引
    /// </summary>
    /// <param name="_index">索引编号</param>
    /// <param name="data">数据库值</param>
    public void Init(int _index, (string,DateTime) data)
    {
        index = _index;
        plantName = data.Item1;
        currentGrowTimeDate = data.Item2;
        isWatered = 0;
        isMature = 0;
        //image.sprite = Resources.Load<Sprite>("panel_fight/panlt_土地");

        growTimeInt = (int)(SumSave.nowtime - currentGrowTimeDate).TotalSeconds;//当前时间-植物种植时间 获得植物种植到现在的时间
        if (growTimeInt <= growTime)//植物已经生长的时间小于植物需要生长的时间
        {
            growTimeInt = growTime - growTimeInt;
        }
        else
        {
            growTimeInt = -1;
            isMature = 1;
        } 
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
            CountdownText.text = growTimeInt.ToString();
        }
        else
        {
            CountdownText.text = "";
            growTimeInt = -1;
            isMature = 1;
        }


    }

    /// <summary>
    /// 设置植物数据
    /// </summary>
    /// <param name="_Data"></param>
    public void GetData(user_plant_vo _Data)
    {
        //panltType = _Data.plantType;

        plantName= _Data.plantName;
       
        CountdownText.transform.gameObject.SetActive(true);
        //image.sprite = Resources.Load<Sprite>("panel_fight/panlt_" + plantName);
    }
    /// <summary>
    /// 减少生长时间
    /// </summary>
    public void ReduceGrowthTime(int time)
    {
        Debug.Log("未浇水，" + plantName + "多久成熟" + currentGrowTimeDate);
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        currentGrowTimeDate -= timeSpan;
        growTimeInt -= time;
        
        Debug.Log("已浇水，" + plantName + "多久成熟" + currentGrowTimeDate);

    }
    /// <summary>
    /// 获取当前植物成熟时间
    /// </summary>
    /// <returns></returns>
    public DateTime GetCurrentGrowTimeDate()
    {
        return currentGrowTimeDate;
    }

    /// <summary>
    /// 转换int类型为时间
    /// </summary>
    /// <param name="timestamp"></param>
    /// <returns></returns>
    public DateTime ConvertFromUnixTimestamp(int timestamp)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return origin.AddSeconds(timestamp).ToLocalTime();
    }

}

