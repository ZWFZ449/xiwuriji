#if UNITY_EDITOR && UNITY_IOS
using TMPro;
using UnityEngine;
using System.Collections;

public class iOSDynamicTMPFont : MonoBehaviour
{
    [SerializeField] private TMP_Text targetText;
    
    private void Start()
    {
        ApplyiOSSystemFont();
    }
    
    private void ApplyiOSSystemFont()
    {
        StartCoroutine(LoadiOSFontCoroutine());
    }
    
    private IEnumerator LoadiOSFontCoroutine()
    {
        // iOS 推荐的中文字体（按优先级）
        string[] iosChineseFonts = 
        {
            "PingFangSC-Regular",      // 苹方简体
            "PingFangSC-Light",        // 苹方细体
            "Heiti SC",                // 黑体简体
            "STHeitiSC-Light",         // 华文黑体
            "Hiragino Sans GB",        // 冬青黑体
            "Arial Unicode MS"         // Arial Unicode（包含中文）
        };
        
        foreach (string fontName in iosChineseFonts)
        {
            Debug.Log($"尝试加载系统字体: {fontName}");
            
            // 创建动态字体
            Font dynamicFont = Font.CreateDynamicFontFromOSFont(fontName, 16);
            
            if (dynamicFont != null)
            {
                Debug.Log($"✅ 成功加载: {fontName}");
                
                // 创建TMP字体资产
                TMP_FontAsset fontAsset = TMP_FontAsset.CreateFontAsset(dynamicFont);
                
                if (fontAsset != null)
                {
                    // 包含基本字符
                    fontAsset.TryAddCharacters("测试中文ABCD123");
                    
                    // 应用到TextMeshPro
                    if (targetText != null)
                    {
                        targetText.font = fontAsset;
                        targetText.ForceMeshUpdate();
                    }
                    
                    Debug.Log($"✅ TextMeshPro字体应用成功");
                    yield break;
                }
            }
            
            yield return null;
        }
        
        Debug.LogError("❌ 无法加载iOS系统字体");
    }
}
#endif