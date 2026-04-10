using System;
using System.Collections.Generic;
using System.Linq;

public static class UnifiedDateTime
{
    // 支持的输入格式
    private static readonly Dictionary<string, string[]> InputFormats = new Dictionary<string, string[]>
    {
        ["dateTime"] = new[]
        {
            "yyyy-MM-dd HH:mm:ss",
            "yyyy/MM/dd HH:mm:ss",
            "yyyy.MM.dd HH:mm:ss",
            "yyyy-MM-ddTHH:mm:ss",
            "yyyy-MM-dd HH:mm",
            "yyyy/MM/dd HH:mm"
        },

        ["dateOnly"] = new[]
        {
            "yyyy-MM-dd",
            "yyyy/MM/dd",
            "yyyy.MM.dd",
            "yyyy年MM月dd日",
            "MM/dd/yyyy",
            "dd/MM/yyyy"
        },

        ["compact"] = new[]
        {
            "yyyyMMddHHmmss",
            "yyyyMMddHHmm",
            "yyyyMMdd"
        }
    };

    // 默认输出格式
    public const string DefaultOutputFormat = "yyyy-MM-dd HH:mm:ss";
    /// <summary>
    /// 统一DateTime为字符串
    /// </summary>
    private static DateTime MinValue = DateTime.Now;

    /// <summary>
    /// 统一字符串为DateTime
    /// </summary>
    public static DateTime FromString(string input, string preferredFormat = null)
    {
        if (string.IsNullOrWhiteSpace(input))
            return MinValue;

        // 1. 如果指定了优先格式，先尝试
        if (!string.IsNullOrEmpty(preferredFormat))
        {
            if (DateTime.TryParseExact(input, preferredFormat,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out DateTime preferredFormattime))
            {
                return preferredFormattime;
            }
        }

        // 2. 尝试所有支持的格式
        var allFormats = InputFormats.Values.SelectMany(f => f).ToArray();
        if (DateTime.TryParseExact(input, allFormats,
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None,
            out DateTime result))
        {
            return result;
        }

        // 3. 尝试通用解析
        if (DateTime.TryParse(input, out result))
        {
            return result;
        }

        return MinValue;
    }
    /// <summary>
    /// 统一DateTime为字符串
    /// </summary>
    /// <param name="input"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static string ToString(DateTime input, string format = null)
    { 
        return string.IsNullOrWhiteSpace(format) ? input.ToString(DefaultOutputFormat) : input.ToString(format);
    }
    /// <summary>
    /// 添加自定义格式
    /// </summary>
    public static void AddCustomFormat(string format)
    {
        if (!InputFormats["dateTime"].Contains(format))
        {
            InputFormats["dateTime"] = InputFormats["dateTime"].Concat(new[] { format }).ToArray();
        }
    }
}