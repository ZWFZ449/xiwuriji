using Common;
using Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepLinkBuffer : MonoBehaviour
{
    // 静态列表，用于存储冷启动时的指令
    private static List<string> pendingDeepLinks = new List<string>();

    // 标记项目是否已准备好处理指令
    private static bool isAppReady = false;

    void Awake()
    {
        DontDestroyOnLoad(this);
        Application.deepLinkActivated += OnDeepLinkActivated;

        // 处理之前缓存的指令
        if (pendingDeepLinks.Count > 0)
        {
            StartCoroutine(ProcessPendingDeepLinks()); 
        }
    }

    void Start()
    {
        // 标记 App 已准备就绪
        isAppReady = true;
        Debug.Log("✅ App 已准备就绪，可处理 Deep Link");
    }

    public void OnDeepLinkActivated(string url)
    {
        Debug.Log($"📨 收到 Deep Link: {url}");

        if (!isAppReady)
        {
            // 如果 App 未就绪，存入缓冲区
            pendingDeepLinks.Add(url);
            Debug.Log($"⏳ Deep Link 已缓存，等待 App 初始化: {url}");
        }
        else
        {
            // 立即处理
            ProcessDeepLinkImmediately(url);
        }
    }

    IEnumerator ProcessPendingDeepLinks()
    {
        // 等待一帧，确保所有组件初始化完成
        yield return null;

        Debug.Log($"🔄 开始处理缓存的 {pendingDeepLinks.Count} 个 Deep Link");

        foreach (string url in pendingDeepLinks)
        {
            ProcessDeepLinkImmediately(url);
            yield return null; // 可选的帧间隔
        }

        pendingDeepLinks.Clear();
    }

    void ProcessDeepLinkImmediately(string url)
    {
        // 你的实际处理逻辑
        Debug.Log($"✅ 处理 Deep Link: {url}");

        if (url.StartsWith("dreamingoflegend://event/"))
        {
            string eventName = url.Replace("dreamingoflegend://event/", "");

            // 这里调用你的 Alert 或场景切换逻辑
            // 注意：此时确保所有 UI 系统已初始化
            switch (eventName)
            {
                case "dreamingoflegend":
                    if (SumSave.db_pars == null)
                    {
                        if (isActiveAndEnabled)
                        {
                            //Debug.Log("等待读取");
                            Alert.Show("新服冲级", "活动时间4月28日-5月14日\n玩家等级达到30级可获得珍稀灵宠 白虎\n玩家等级达到40级可获得珍稀灵宠 巨象\n点击确认报名参加");
                        }
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