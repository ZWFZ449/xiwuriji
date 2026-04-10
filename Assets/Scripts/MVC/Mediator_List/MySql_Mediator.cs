
using System.Collections.Generic;
using TarenaMVC;

namespace MVC
{
    /// <summary>
    /// 网络存档
    /// </summary>
    public class MySql_Mediator : Mediator
    {
        /// <summary>
        ///  NAME
        /// </summary>
        public new const string NAME = "MySql_Mediator";

        private MySql_Proxy MySql;
        /// <summary>
        ///  构造函数
        /// </summary>
        public MySql_Mediator()
        {
            this.MediatorName = NAME;

            MySql = AppFacade.I.RetrieveProxy(MySql_Proxy.NAME) as MySql_Proxy;
        }
        /// <summary>
        ///  监听的消息列表
        /// </summary>
        /// <returns></returns>
        public override string[] ListNotificationInterests()
        {
            return new string[] {
                NotiList.Read_Instace,
                NotiList.Mysql_close

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
                case NotiList.Read_Instace:
                    MySql.Read_Instace();
                    break;
                    case NotiList.Mysql_close:
                    MySql.close_ApplicationFocus();
                    break;
              
            }
        }
        }
}