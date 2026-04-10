using System;
using System.Linq;

public static class EnumExtensions
{
    private static readonly Random random = new Random();

    // 扩展方法，直接在枚举类型上使用
    public static T GetRandomValue<T>(this T enumType) where T : Enum
    {
        var values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(random.Next(values.Length));
    }
    public static T GetRandomValue<T>() where T : Enum
    {
        var values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(random.Next(values.Length));
    }
    // 获取随机值（静态方法版本）
    public static T GetRandomEnum<T>() where T : Enum
    {
        var values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(random.Next(values.Length));
    }
}