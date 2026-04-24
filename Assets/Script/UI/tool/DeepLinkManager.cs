using Common;
using Components;
using MVC;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DeepLinkManager : MonoBehaviour
{
    public static DeepLinkManager Instance;
    public string LastDeepLinkUrl;

    void Awake()
    {
        // 单例模式，防止重复创建
        if (Instance == null)
        {
            Instance = this;
            Application.deepLinkActivated += OnDeepLinkActivated;
            DontDestroyOnLoad(gameObject);

            // 处理冷启动（App 未运行时的点击）
            if (!string.IsNullOrEmpty(Application.absoluteURL))
            {
                OnDeepLinkActivated(Application.absoluteURL);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnDeepLinkActivated(string url)
    {
        // 保存链接，供其他模块读取
        LastDeepLinkUrl = url;
        // 解析 URL（示例：legendmemoir://event/siege）
        if (url.StartsWith("dreamingoflegend://event/"))
        {
            string eventName = url.Replace("dreamingoflegend://event/", "");
            Debug.Log("接收到DeepLink指令: " + eventName);
            // 根据 eventName 跳转到对应活动
            switch (eventName)
            {
                case "dreamingoflegend": 
                    if (SumSave.db_pars == null)
                    {
                        if (isActiveAndEnabled)
                        {
                            Debug.Log("等待读取");
                            //Game_Omphalos.Init_Alert("新服冲级", "活动时间4月28日-5月14日\n玩家等级达到30级可获得珍稀灵宠 白虎\n玩家等级达到40级可获得珍稀灵宠 巨象\n点击确认报名参加");
                            //Alert.Show("新服冲级", "活动时间4月28日-5月14日\n玩家等级达到30级可获得珍稀灵宠 白虎\n玩家等级达到40级可获得珍稀灵宠 巨象\n点击确认报名参加");
                        }
                        //else Alert.Show("新服冲级", "活动时间4月28日-5月14日\n玩家等级达到30级可获得珍稀灵宠 白虎\n玩家等级达到40级可获得珍稀灵宠 巨象\n点击确认报名参加");
                    }
                    else Alert.Show("新服冲级", "活动时间4月28日-5月14日\n玩家等级达到30级可获得珍稀灵宠 白虎\n玩家等级达到40级可获得珍稀灵宠 巨象\n点击确认报名参加");
                    break;
                case "dark_dungeon":
                    // 跳转到暗黑副本页
                    break;
                default:
                    Debug.Log($"未知活动: {eventName}");
                    break;
            }
        }
    } 
}