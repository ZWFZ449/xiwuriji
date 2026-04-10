using System;
using System.Collections.Generic;
using Common;

namespace MVC
{
    /// <summary>
    ///  处理背包相关数据
    /// </summary>
    public class Bag_Proxy : Base_Proxy
    {
        /// <summary>
        ///  NAME
        /// </summary>
        public new const string NAME = "Bag_Proxy";
        /// <summary>
        ///  构造函数
        /// </summary>
        public Bag_Proxy()
        {
            this.ProxyName = NAME;
        }
    }
}
