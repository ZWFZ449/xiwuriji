using UnityEngine;
using System.Collections;
using MVC;
using System;
#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

public class iOSAppLifecycleMonitor : MonoBehaviour
{
    private bool isQuitting = false;

    // Unity 生命周期回调
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            // 应用进入后台
            Debug.Log("🟡 iOS 应用进入后台");
            OnAppEnterBackground();
        }
        else
        {
            // 应用回到前台
            Debug.Log("🟢 iOS 应用回到前台");
            OnAppEnterForeground();
        }
    }

    private void OnApplicationQuit()
    {
        // 应用完全关闭
        Debug.Log("🔴 iOS 应用完全关闭");
        isQuitting = true;
        OnAppWillTerminate();
    }

    private void OnDestroy()
    {
        if (!isQuitting)
        {
            // 非正常关闭（如内存不足被系统回收）
            Debug.Log("⚠️ 应用被系统销毁");
            OnAppWillBeDestroyed();
        }
    }

    // 应用进入后台
    private void OnAppEnterBackground()
    {
        Game_Omphalos.i.archive();
        //// 暂停游戏逻辑
        //Time.timeScale = 0;

        //// 暂停音频
        //AudioListener.pause = true;

        //// 保存游戏状态
        //SaveGameState();

        //// 发送分析事件
        //SendAnalytics("app_background");
    }

    // 应用回到前台
    private void OnAppEnterForeground()
    {
        //// 恢复游戏逻辑
        //Time.timeScale = 1;

        //// 恢复音频
        //AudioListener.pause = false;

        //// 加载游戏状态
        //LoadGameState();

        //// 发送分析事件
        //SendAnalytics("app_foreground");
    }

    // 应用即将终止
    private void OnAppWillTerminate()
    {
        // 紧急保存数据
        EmergencySave();

        // 清理资源
        //CleanupResources();

        // 发送分析事件
        //SendAnalytics("app_terminate");
    }

    private void EmergencySave()
    {
        Game_Omphalos.i.archive();
    }

    // 应用被系统销毁
    private void OnAppWillBeDestroyed()
    {
        // 尝试保存数据
        //TrySaveBeforeDestroy();
        EmergencySave();
    }
}