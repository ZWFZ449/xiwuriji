
using TarenaMVC;
using System.Collections.Generic;

namespace MVC
{
    /// <summary>
    ///  用户系统的中介者
    /// </summary>
    public class User_Mediator : Mediator
    {
        /// <summary>
        ///  NAME
        /// </summary>
        public new const string NAME = "User_Mediator";

        /// <summary>
        /// 数据库
        /// </summary>
        private User_Proxy user;

        public User_Mediator()
        {
            this.MediatorName = NAME;
            // 获取UserProxy
            user = AppFacade.I.RetrieveProxy(User_Proxy.NAME) as User_Proxy; 
 
        }
        /// <summary>
        ///  监听的消息列表
        /// </summary>
        /// <returns></returns>
        public override string[] ListNotificationInterests()
        {
            return new string[] {
                NotiList.User_Login,
                NotiList.Execute_Write,
                NotiList.Refresh_Max_Hero_Attribute,
                NotiList.read_Obtain_Info,
                NotiList.Read_global_promotion,
                NotiList.Add_global_promotion,
                NotiList.Read_Global_Gift,
                NotiList.Delete


            };
        }
        /// <summary>
        ///  处理消息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public override void HandleNotification(string name, object data)
        {
            switch (name)
            {
                case NotiList.User_Login:
                    user.User_Login();
                    Game_Omphalos.i.activation();
                    break;
                    case NotiList.Refresh_Max_Hero_Attribute:
                    user.Refresh_Max_Hero_Attribute();
                    break;
                case NotiList.Execute_Write:
                    user.Execute_Write(data as List<Base_Wirte_VO>);
                    break;
                case NotiList.read_Obtain_Info:
                    user.read_Obtain_Info();
                    break;
                case NotiList.Read_global_promotion:
                    user.Read_global_promotion();
                    break;
                case NotiList.Read_Global_Gift:
                    user.Read_Global_Gift(data.ToString());
                    break;
                    case NotiList.Add_global_promotion:
                    user.Add_global_promotion(data);
                    break;
                    case NotiList.Delete:
                    user.Delete();
                    break;
                default:
                    break;
            }
        }
    }
}
