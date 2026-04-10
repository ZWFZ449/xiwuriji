

namespace MVC
{
    /// <summary>
    ///  处理角色相关数据
    /// </summary>
    public class Hero_Proxy : Base_Proxy
    {
        /// <summary>
        ///  NAME
        /// </summary>
        public new const string NAME = "Hero_Proxy";
        /// <summary>
        ///  构造函数
        /// </summary>
        public Hero_Proxy()
        {
            this.ProxyName = NAME;
        }
    }
}