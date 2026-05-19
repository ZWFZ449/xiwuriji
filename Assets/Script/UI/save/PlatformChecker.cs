using MVC;
using UnityEngine;

public class PlatformChecker : MonoBehaviour
{
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            // App 进入后台
            HandleAppPause();
        }
        else
        {
            // App 回到前台
            HandleAppResume();
        }
    }

    void HandleAppPause()
    {
        // 判断当前运行的平台
        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("安卓端：应用进入后台，执行安卓专属逻辑（如强制存档）。");
            // Android 逻辑：通常在这里强制存档，因为安卓杀后台很暴力
            Game_Omphalos.i.archive();

        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Debug.Log("iOS端：应用进入后台，执行苹果专属逻辑（如申请后台时间）。");
            // iOS 逻辑：可以申请一点后台时间来存档
            // Note: iOS 在后台存活时间很短，要尽快存档
            Game_Omphalos.i.archive();

        }
        else
        {
            // 其他平台（Editor, Windows, Mac等）
            Debug.Log("其他平台进入后台。");
            Game_Omphalos.i.archive();
        }
    }

    void HandleAppResume()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("安卓端：应用回到前台。");
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Debug.Log("iOS端：应用回到前台。");
        }
    }
}