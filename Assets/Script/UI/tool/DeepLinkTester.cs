using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DeepLinkTester : MonoBehaviour
{
    // 供 Inspector 面板输入的测试 URL
    //public string testUrl = "dreamingoflegend://event/dreamingoflegend";

    //// 提供给菜单项调用的静态方法
    //[MenuItem("Tools/Test Deep Link")]
    //static void TestDeepLink()
    //{
    //    // 查找场景中的 DeepLinkManager 实例
    //    //DeepLinkManager manager = FindObjectOfType<DeepLinkManager>();
    //    DeepLinkBuffer manager = FindObjectOfType<DeepLinkBuffer>(); 
    //    if (manager != null)
    //    {
    //        // 手动调用你的处理函数
    //        manager.OnDeepLinkActivated("dreamingoflegend://event/dreamingoflegend");
    //        Debug.Log("✅ 手动触发 Deep Link 测试: " + "dreamingoflegend://event/dreamingoflegend");
    //    }
    //    else
    //    {
    //        Debug.LogError("❌ 场景中未找到 DeepLinkManager 组件！");
    //    }
    //}

    //// 在编辑器中添加一个按钮（可选）
    //[MenuItem("Tools/Test Deep Link (Custom URL)")]
    //static void TestCustomDeepLink()
    //{
    //    DeepLinkManager manager = FindObjectOfType<DeepLinkManager>();
    //    if (manager != null)
    //    {
    //        //// 弹窗输入自定义 URL
    //        //string customUrl = EditorUtility.InputDialog("Test Deep Link", "Enter URL:", "dreamingoflegend://event/dreamingoflegend");
    //        //if (!string.IsNullOrEmpty(customUrl))
    //        //{
    //        //    manager.OnDeepLinkActivated(customUrl);
    //        //}
    //    }
    //}
}