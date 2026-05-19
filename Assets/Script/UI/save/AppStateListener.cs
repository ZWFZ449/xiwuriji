using MVC;
using UnityEngine;

public class AppStateListener : MonoBehaviour
{
    // 1. 暂停状态（最重要，安卓切后台必触发）
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            // App 进入后台 / 被暂停
            Debug.Log("App Paused (Went to Background)");
            OnAppBackground();
        }
        else
        {
            // App 回到前台 / 恢复
            Debug.Log("App Resumed (Came to Foreground)");
            OnAppForeground();
        }
    }

    // 2. 焦点状态（辅助，处理弹窗遮挡）
    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            // App 获得焦点
            Debug.Log("App Focused");
        }
        else
        {
            // App 失去焦点（例如下拉通知栏）
            Debug.Log("App Lost Focus");
        }
    }

    // 3. 退出状态（兜底）
    void OnApplicationQuit()
    {
        Debug.Log("App Quit");
        OnAppBackground(); // 退出时也执行存档
    }

    // 自定义逻辑：进入后台
    void OnAppBackground()
    {
        Game_Omphalos.i.archive();
        // ✅ 在这里调用存档
        // SaveManager.Instance?.Save();

        // ✅ 暂停游戏逻辑
        Time.timeScale = 0;

        // ✅ 暂停音效
        AudioListener.pause = true;
    }

    // 自定义逻辑：回到前台
    void OnAppForeground()
    {
        // ✅ 恢复游戏逻辑
        Time.timeScale = 1;

        // ✅ 恢复音效
        AudioListener.pause = false;
    }
}