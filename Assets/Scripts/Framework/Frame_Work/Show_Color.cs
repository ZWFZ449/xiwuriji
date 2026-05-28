using System.Drawing;
using UnityEngine;

namespace MVC
{
    public enum Color_list
    { 
        灰色,
        白色,
        绿色,
        蓝色,
        紫色,
        黄色,
        橙色,
        红色,
    }

    public enum Show_Color_list
    {
        red,
        green,
        blue,
        purple,
        yellow,
        orange,
        white,
        grey,
    }
    /// <summary>
	/// 颜色显示功能
	/// </summary>
	public static class Show_Color
    {
        private static System.Drawing.Color c = System.Drawing.Color.White;

        /// <summary>
        /// 设置颜色
        /// </summary>
        /// <param name="crt_c"></param>
        /// <returns></returns>
        public static UnityEngine.Color Set_Color(Color_list crt_c)
        {
            switch (crt_c)
            { 
               case Color_list.灰色:
                    c = System.Drawing.Color.Gray;
                    break;
               case Color_list.白色:
                    c = System.Drawing.Color.White;
                    break;
               case Color_list.绿色:
                    c = System.Drawing.Color.Green;
                    break;
               case Color_list.蓝色:
                    c = System.Drawing.Color.Blue;
                    break;
               case Color_list.紫色:
                    c = System.Drawing.Color.Purple;
                    break;
               case Color_list.黄色:
                    c = System.Drawing.Color.Yellow;
                    break;
               case Color_list.橙色:
                    c = System.Drawing.Color.Orange;
                    break;
               case Color_list.红色:
                    c = System.Drawing.Color.Red;
                    break;
            }
            return new UnityEngine.Color(
        c.R / 255f,
        c.G / 255f,
        c.B / 255f,
        c.A / 255f
    );
        }

        public static string Set_String(object s, UnityEngine.Color c)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(c)}>{s}</color>";
        }



        public static string Set_String(object value, Show_Color_list crt_c)
        {
            return "<color="+ crt_c + ">" + value + "</color>";
        }
        private static string value;
        /// <summary>
		/// 红色
		/// </summary>
		/// <param name="red"></param>
		/// <returns></returns>
		public static string Red<T>(T red)
        {
            value = red.ToString();
            c= System.Drawing.Color.Red;
            return "<color=red>" + red + "</color>";
        }
        /// <summary>
        /// 白色
        /// </summary>
        /// <param name="white"></param>
        /// <returns></returns>
        public static string White<T>(T white)
        {
            return "<color=white>" + white + "</color>";
        }
        /// <summary>
        /// 绿色
        /// </summary>
        /// <param name="green"></param>
        /// <returns></returns>
        public static string Green<T>(T green)
        {
            return "<color=green>" + green + "</color>";
        }
        /// <summary>
        /// 蓝色
        /// </summary>
        /// <param name="green"></param>
        /// <returns></returns>
        public static string Blue<T>(T Blue)
        {
            return "<color=blue>" + Blue + "</color>";
        }
        /// <summary>
        /// 紫色
        /// </summary>
        /// <param name="purple"></param>
        /// <returns></returns>
        public static string Purple<T>(T purple)
        {
            return "<color=purple>" + purple + "</color>";
        }
        /// <summary>
        /// 黄色
        /// </summary>
        /// <param name="yellow"></param>
        /// <returns></returns>
        public static string Yellow<T>(T yellow)
        {
            return "<color=yellow>" + yellow + "</color>";
        }
        /// <summary>
        /// 橙色
        /// </summary>
        /// <param name="orange"></param>
        /// <returns></returns>
        public static string Orange<T>(T orange)
        {
            return "<color=orange>" + orange + "</color>";
        }
        /// <summary>
        /// 黑色
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="orange"></param>
        /// <returns></returns>
        public static string Black<T>(T orange)
        {
            return "<color=black>" + orange + "</color>";
        }
        /// <summary>
        /// 粉色
        /// </summary>
        /// <param name="pink"></param>
        /// <returns></returns>
        public static string Pink<T>(T pink)
        {
            return "<color=pink>" + pink + "</color>";
        }
        /// <summary>
        /// 灰色
        /// </summary>
        /// <param name="grey"></param>
        /// <returns></returns>
        public static string Grey<T>(T grey)
        {
            return "<color=grey>" + grey + "</color>";
        } 
    }
}
