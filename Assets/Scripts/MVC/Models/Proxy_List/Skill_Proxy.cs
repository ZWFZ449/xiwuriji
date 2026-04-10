using System;
using System.Collections.Generic;
using Common;

namespace MVC
{
    /// <summary>
    ///  处理技能相关数据
    /// </summary>
    public class Skill_Proxy : Base_Proxy
    {
        /// <summary>
        ///  NAME
        /// </summary>
        public new const string NAME = "Skill_Proxy";

        /// <summary>
        ///  构造函数
        /// </summary>
        public Skill_Proxy()
        {
            this.ProxyName = NAME;
        }
    }
}