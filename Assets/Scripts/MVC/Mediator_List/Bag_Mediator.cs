using System.Collections.Generic;
using TarenaMVC;

namespace MVC
{
    /// <summary>
    ///  角色系统的中介者
    /// </summary>
    public class Bag_Mediator : Mediator
    {
        /// <summary>
        ///  NAME
        /// </summary>
        public new const string NAME = "Bag_Mediator";
        /// <summary>
        ///  构造函数
        /// </summary>
        public Bag_Mediator()
        {
            this.MediatorName = NAME;
            // 获取heroProxy
            bag = AppFacade.I.RetrieveProxy(Bag_Proxy.NAME) as Bag_Proxy;
        }

        private Bag_Proxy bag;
        /// <summary>
        ///  监听的消息列表
        /// </summary>
        /// <returns></returns>
        public override string[] ListNotificationInterests()
        {
            return new string[] {
                //初始化
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