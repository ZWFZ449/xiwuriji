using System.Collections.Generic;
using TarenaMVC;
using UI;

namespace MVC
{
    /// <summary>
    ///  角色系统的中介者
    /// </summary>
    public class Hero_Mediator : Mediator
    {
        /// <summary>
        ///  NAME
        /// </summary>
        public new const string NAME = "Hero_Mediator";
        /// <summary>
        ///  构造函数
        /// </summary>
        public Hero_Mediator()
        {
            this.MediatorName = NAME;
            // 获取heroProxy
            hero = AppFacade.I.RetrieveProxy(Hero_Proxy.NAME) as Hero_Proxy;

        }
        /// <summary>
        /// HeroProxy
        /// </summary>
        Hero_Proxy hero;

        /// <summary>
        ///  监听的消息列表
        /// </summary>
        /// <returns></returns>
        public override string[] ListNotificationInterests()
        {
            return new string[]
            {
                
            };
        }
        /// <summary>
        ///  处理角色系统相关消息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public override void HandleNotification(string name, object data)
        {
            switch (name)
            {
                default:
                    break;
            }
        }
    }
}
