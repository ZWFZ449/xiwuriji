
using System.Collections.Generic;
using TarenaMVC;

namespace MVC
{
    public class User_Instace_Mediator : Mediator
    {
        /// <summary>
        ///  NAME
        /// </summary>
        public new const string NAME = "DSFSDFSDFSDF1";// "User_Instace_Mediator";

        private User_Instace_Proxy user;
        /// <summary>
        ///  构造函数
        /// </summary>
        public User_Instace_Mediator()
        {
            this.MediatorName = NAME;

            user = AppFacade.I.RetrieveProxy(User_Instace_Proxy.NAME) as User_Instace_Proxy;
        }

        public override string[] ListNotificationInterests()
        {
            return new string[]
            {
                ////NotiList.User_Login,
                ////NotiList.Execute_Write,
                //NotiList.Refresh_User_Setting,
                ////NotiList.Refresh_Max_Hero_Attribute,
                //NotiList.Delete,
                //NotiList.loglist,
                //NotiList.Read_User_Ranks,
                //NotiList.Read_Crate_Uid,
                //NotiList.Read_Crate_IPhone_Uid,
                //NotiList.Read_Message_Window,
                //NotiList.Read_Huser_MessageWindow,
                //NotiList.Read_Mail,
                //NotiList.Read_Crate_IPhone_logoff,
                //NotiList.Read_Crate_world_boss_Login,
                //NotiList.Read_Crate_world_boss_update,
                //NotiList.Read_Crate_RecordAndClearWorldBoss,
                //NotiList.Read_Trial_Tower,
                //NotiList.Refresh_Trial_Tower,
                //NotiList.Refresh_Rank,
                //NotiList.Read_EndlessBattle,
                //NotiList.Refresh_Endless_Tower,
                //NotiList.Mysql_close,
                //NotiList.Read_Mysql_Base_Time
            };

        }

        /// <summary>
        /// 处理通知
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public override void HandleNotification(string name, object data)
        {

        }
    }

}
