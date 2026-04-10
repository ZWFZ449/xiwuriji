
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
            switch (name)
            {
                case NotiList.Read_Crate_Uid:
                    user.Read_Crate_Uid(data as string[]);
                    break;
                case NotiList.Read_Crate_IPhone_Uid:
                    user.Read_Crate_IPhone_Uid(data as string[]);
                    break;
                case NotiList.Read_Crate_IPhone_logoff:
                    user.Read_Crate_IPhone_logoff(data as string[]);
                    break;
                case NotiList.Read_Crate_world_boss_Login:
                    user.Read_Crate_world_boss_Login();
                    break;
                case NotiList.Read_Crate_world_boss_update:
                    user.Read_Crate_world_boss_update();
                    break;
                case NotiList.Read_Crate_RecordAndClearWorldBoss:
                    user.Read_Crate_RecordAndClearWorldBoss();
                    break;
                case NotiList.User_Login:
                    user.User_Login();
                    Game_Omphalos.i.activation();
                    break;
                    case NotiList.Execute_Write:
                    user.Execute_Write(data as List<Base_Wirte_VO>);
                    break;
                case NotiList.Refresh_User_Setting:
                    user.Refresh_User_Setting(data as user_base_setting_vo);
                    break;
                case NotiList.Refresh_Max_Hero_Attribute:
                    //user.Refresh_Max_Hero_Attribute();
                    break;
                case NotiList.Delete:
                    user.Delete(data.ToString());
                    break;
                    case NotiList.loglist:
                    user.loglist(data.ToString());
                    break;
                case NotiList.Read_User_Ranks:
                    user.Read_User_Rank();
                    break;
                case NotiList.Read_EndlessBattle:
                    user.Read_read_EndlessBattle();
                    break;
                case NotiList.Read_Message_Window:
                    user.Read_user_messageWindow();
                    break;
                case NotiList.Read_Huser_MessageWindow:
                    user.Refres_huser_messageWindow(data.ToString());
                    break;
                case NotiList.Read_Mail:
                    user.Read_Mail();
                    break;
                    case NotiList.Read_Trial_Tower:
                    user.Read_Trial_Tower();
                    break;
                    case NotiList.Refresh_Trial_Tower:
                    user.Refresh_Trial_Tower(int.Parse(data.ToString()));
                    break;
                case NotiList.Refresh_Rank:
                    user.Refresh_Rank();
                    break;
                case NotiList.Refresh_Endless_Tower:
                    user.Refresh_Endless_Tower();
                    break;
                    case NotiList.Mysql_close:
                    user.close_ApplicationFocus();
                    break;
                    case NotiList.Read_Mysql_Base_Time:
                    user.Read_Mysql_Base_Time();
                    break;
            }
        }
    }

}
