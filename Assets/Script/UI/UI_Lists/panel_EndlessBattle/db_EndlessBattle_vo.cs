using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MVC
{
    /// <summary>
    /// 无尽模式收益列表
    /// </summary>
    public class db_EndlessBattle_vo : Base_VO
    {
        /// <summary>
        /// 物品列表
        /// </summary>
        public readonly string[] goods;
        /// <summary>
        /// 收益标准
        /// </summary>
        public readonly int need_number;

        public readonly int max_number;

        public db_EndlessBattle_vo(string[] goods, int need_number,int max_number)
        { 
            this.goods = goods;
            this.need_number = need_number;
            this.max_number = max_number;
        }
    }

}
