using System;
using System.Collections.Generic;
using System.Reflection;

namespace MVC
{
    /// <summary>
    /// 数据标准结构
    /// </summary>
    public class Base_VO
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public void SendNotification(string name, object data = null)
        {
            AppFacade.I.SendNotification(name, data);
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Iint()
        { 
        
        }

        /// <summary>
        ///  写入网络初始化数据
        /// </summary>
        /// <returns></returns>
        public virtual string[] Set_Instace_String()
        {
            return new string[] { };
        }

        /// <summary>
        ///  写入刷新数据
        /// </summary>
        /// <returns></returns>
        public virtual string[] Set_Uptade_String()
        {
            return new string[] { };
        }  
        /// <summary>
        /// 读取更新字符
        /// </summary>
        /// <returns></returns>
        public virtual string[] Get_Update_Character()
        {
            return new string[] { };
        }
        /// <summary>
        /// 解析
        /// </summary>
        public virtual int[] Data_Decrypt(string base_value)
        {
            string[] Splits = base_value.Split(' ');

            int[] output = new int[Splits.Length];

            if (output.Length <= 1) return output;

            for (int i = 0; i < Splits.Length; i++)
            {
                output[i] = int.Parse(Splits[i]);
            }

            return output;

        }
        /// <summary>
        /// 写入数据库
        /// </summary>
        public virtual void MysqlData()
        { 
        
        }
        /// <summary>
        /// 数据组合
        /// </summary>
        /// <param name="base_value"></param>
        /// <returns></returns>
        public virtual string data_combination(int[] base_value)
        {
            string output = "";

            for (int i = 0; i < base_value.Length; i++)
            {
                output += base_value[i].ToString() + (i == base_value.Length - 1 ? "" : " ");
            }

            return output;

        }
        /// <summary>
        ///  前后添加单引号
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public virtual string GetStr(object o)
        {
            return "'" + o + "'";
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetPropertyValue(string value)
        {
            // 根据传入的值拆分为字符串数组
            string[] values = value.Split(',');
            // 获取当前对象的所有属性信息
            PropertyInfo[] propertyInfos = this.GetType().GetProperties();
            int index = 0;
            // 遍历所有属性并根据类型设置对应的属性值
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                //Debug.Log(values[index]);
                if (propertyInfo.PropertyType == typeof(string))
                {
                    // 将对应位置的值赋给字符串类型的属性
                    propertyInfo.SetValue(this, values[index]);
                }
                else if (propertyInfo.PropertyType == typeof(int))
                {
                    // 将对应位置的值转换为整数并赋给整数类型的属性
                    if (values.Length - 1 >= index)
                        propertyInfo.SetValue(this, int.Parse(values[index]));
                }
                else if (propertyInfo.PropertyType == typeof(float))
                {
                    // 将对应位置的值转换为浮点数并赋给浮点数类型的属性
                    propertyInfo.SetValue(this, float.Parse(values[index]));
                }
                else if (propertyInfo.PropertyType == typeof(bool))
                {
                    // 将对应位置的值转换为布尔型并赋给布尔型属性
                    propertyInfo.SetValue(this, Convert.ToBoolean(values[index]));
                }
                ++index;
            }
        }

        /// <summary>
        /// 写入数据库
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual string GetPropertyValue(Base_VO item)
        {
            // 获取此对象的所有属性信息
            PropertyInfo[] propertyInfos = this.GetType().GetProperties();

            string propertyStr = "";

            // 遍历所有属性，获取属性值并拼接成字符串
            for (int i = 0; i < propertyInfos.Length; i++)
            {
                //string name = propertyInfos[i].Name;

                propertyStr += propertyInfos[i].GetValue(item);

                // 如果不是最后一个属性，则添加逗号分隔符
                if (i != propertyInfos.Length - 1)
                    propertyStr += ",";
            }
            // 返回属性值字符串
            return propertyStr;
        }
        /// <summary>
        /// 写入数据库
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="all_list"></param>
        /// <returns></returns>
        protected string OnWirte<T>(List<T> all_list) where T : Base_VO
        {
            string value = "";

            foreach (T item in all_list)
            {
                value += item.GetPropertyValue(item);

                value += ';';
            }
            return value;

        }
    }

}