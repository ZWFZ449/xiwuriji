using UnityEngine;

public class UnityColorPresets
{
    // 游戏常用颜色预设
    public static class GameColors
    {
        // 品质颜色
        public static readonly Color Common = HexToColor("#808080");    // 灰色
        public static readonly Color Uncommon = HexToColor("#00FF00");  // 绿色
        public static readonly Color Rare = HexToColor("#1E90FF");      // 蓝色
        public static readonly Color Epic = HexToColor("#9370DB");      // 紫色
        public static readonly Color Legendary = HexToColor("#FFD700"); // 金色

        // 状态颜色
        public static readonly Color Healthy = Color.green;
        public static readonly Color Injured = Color.yellow;
        public static readonly Color Critical = Color.red;
        public static readonly Color Dead = Color.gray;

        // 阵营颜色
        public static readonly Color Alliance = HexToColor("#0066CC");  // 联盟蓝
        public static readonly Color Horde = HexToColor("#CC0000");     // 部落红
        public static readonly Color Neutral = Color.gray;

        // 资源颜色
        public static readonly Color HealthBar = HexToColor("#FF0000");  // 血条红
        public static readonly Color ManaBar = HexToColor("#1E90FF");    // 魔法蓝
        public static readonly Color EnergyBar = HexToColor("#FFFF00");  // 能量黄
        public static readonly Color RageBar = HexToColor("#FF4500");    // 怒气橙

        // 伤害数字颜色
        public static readonly Color PhysicalDamage = HexToColor("#FF8C00");  // 物理橙
        public static readonly Color MagicDamage = HexToColor("#9370DB");     // 魔法紫
        public static readonly Color TrueDamage = HexToColor("#FFFFFF");      // 真实白
        public static readonly Color Heal = HexToColor("#00FF00");           // 治疗绿
    }
    /// <summary>
    /// 将十六进制颜色代码转换为Color对象
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    public static Color HexToColor(string hex)
    {
        hex = hex.Replace("0x", "").Replace("#", "");

        byte a = 255; // 默认不透明
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        // 如果有透明度
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }

        return new Color32(r, g, b, a);
    }
    /// <summary>
    /// 将Color对象转换为十六进制颜色代码
    /// </summary>
    /// <param name="text"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string Colorize(string text, Color color)
    { 
        return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>";
    }
    /// <summary>
    /// 调整颜色亮度
    /// </summary>
    /// <param name="color"></param>
    /// <param name="factor"></param>
    /// <returns></returns>
    public static Color AdjustBrightness(Color color, float factor)
    {
        return new Color(
            Mathf.Clamp01(color.r * factor),
            Mathf.Clamp01(color.g * factor),
            Mathf.Clamp01(color.b * factor),
            color.a
        );
    }

    // 调整颜色饱和度
    /// <summary>
    /// 调整颜色饱和度
    /// </summary>
    /// <param name="color"></param>
    /// <param name="saturation"></param>
    /// <returns></returns>
    public static Color AdjustSaturation(Color color, float saturation)
    {
        float gray = color.grayscale;
        return Color.Lerp(new Color(gray, gray, gray), color, saturation);
    }

    // 生成互补色
    /// <summary>
    /// 生成互补色
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static Color GetComplementaryColor(Color color)
    {
        return new Color(1f - color.r, 1f - color.g, 1f - color.b, color.a);
    }
    // 渐变颜色
    public static Gradient CreateHealthGradient()
    {
        Gradient gradient = new Gradient();
        gradient.colorKeys = new GradientColorKey[]
        {
            new GradientColorKey(GameColors.Critical, 0f),    // 0% 红色
            new GradientColorKey(GameColors.Injured, 0.5f),   // 50% 黄色
            new GradientColorKey(GameColors.Healthy, 1f)      // 100% 绿色
        };
        return gradient;
    }

    public static Gradient CreateManaGradient()
    {
        Gradient gradient = new Gradient();
        gradient.colorKeys = new GradientColorKey[]
        {
            new GradientColorKey(HexToColor("#0000FF"), 0f),   // 深蓝
            new GradientColorKey(HexToColor("#87CEEB"), 1f)    // 浅蓝
        };
        return gradient;
    }
}