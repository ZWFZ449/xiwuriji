using UnityEngine;

public class FullScreenFix : MonoBehaviour
{
    void Start()
    {
        // 1. 强制全屏
        Screen.fullScreen = true;

        // 2. 强制不限制渲染区域（相当于旧版的 Render Outside Safe Area）
#if UNITY_IOS
        // 获取当前的屏幕安全区
        Rect safeArea = Screen.safeArea;

        // 获取屏幕的完整矩形
        Rect fullScreenRect = new Rect(0, 0, Screen.currentResolution.width, Screen.currentResolution.height);

        // 强制将屏幕分辨率设置为全屏大小（这会绕过安全区限制）
        // 注意：这可能需要配合设置游戏的渲染分辨率
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);

        // 另一种方式是直接调整 Canvas 的 Scaler，但这通常用于 UI
        // 对于游戏画面，最粗暴有效的是：
        //UnityEngine.iOS.Device.hideHomeButton = true; // 隐藏底部横条
#endif
    }
}