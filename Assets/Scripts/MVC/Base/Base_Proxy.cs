using Common;
using Components;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TarenaMVC;
using UI;
using UnityEngine;

namespace MVC
{
    /// <summary>
    ///  封装访问msqlSQLite数据库的功能
    /// </summary>
    public class Base_Proxy : Proxy
    {
        /// <summary>
        /// NAME
        /// </summary>
        public new const string NAME = "Base_Proxy";
        /// <summary>
        /// 验证双开
        /// </summary>
        private int user_login = 0;
        public Base_Proxy()
        {
            this.ProxyName = NAME;
        }
        /// <summary>
        /// 
        /// </summary>
        protected MysqlDbAccess MysqlDb;

        protected MySqlDataReader mysqlReader;

        /// <summary>
        ///  打开网络数据库
        /// </summary>
        public void OpenMySqlDB()
        {
            if (isClose) return;
            MysqlDb = new MysqlDbAccess();
            User_Login();
        }

        protected bool isClose = false;
        /// <summary>
        /// 验证数据
        /// </summary>
        private void User_Login()
        {
            if (MysqlDb.MysqlClose) return;//未联网
            if (SumSave.crt_user != null)
            {
                mysqlReader = MysqlDb.Select(Mysql_Table_Name.user_login, "uid", GetStr(SumSave.uid));
                if(mysqlReader == null) return;
                if (mysqlReader.HasRows)
                {
                    int login = 0;
                    while (mysqlReader.Read())
                    {
                        login = mysqlReader.GetInt32(mysqlReader.GetOrdinal("login"));
                    }
                    if (login == -1)
                    {
                        Game_Omphalos.i.Alert_Info("该用户涉嫌违规操作");
                        isClose = true;
                        CloseMySqlDB();
                    }
                    if (user_login == 0)
                    {
                        user_login = tool_Categoryt.Obtain_Random();
                        MysqlDb.UpdateInto(Mysql_Table_Name.user_login, new string[] { "login" }, new string[] { GetStr(user_login) }, "uid", GetStr(SumSave.uid));
                    }
                    else
                    {
                        //int login = 0;
                        //while (mysqlReader.Read())
                        //{
                        //    login = mysqlReader.GetInt32(mysqlReader.GetOrdinal("login"));  
                        //}
                        if (login != user_login)
                        {
                            isClose = true;
                            Game_Omphalos.i.Alert_Info("多开游戏已关闭");
                            CloseMySqlDB();
                        }
                    }
                }
                else
                {
                    user_login = tool_Categoryt.Obtain_Random();
                    MysqlDb.InsertInto(Mysql_Table_Name.user_login, new string[] { GetStr(0), GetStr(SumSave.uid), GetStr(user_login)});
                }
            }
        }

        /// <summary>
        ///  关闭网络数据库
        /// </summary>
        public void CloseMySqlDB()
        {
            if (MysqlDb != null)
            {
                MysqlDb.CloseSqlConnection();

                MysqlDb = null;

                mysqlReader = null;
            }

        }
        /// <summary>
        /// 重新打开数据库
        /// </summary>
        public virtual void close_ApplicationFocus()
        {
            MysqlDb = null;
            mysqlReader = null;
        }
      
        /// <summary>
        ///  前后添加单引号
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        protected string GetStr(object o)
        {
            return "'" + o + "'";

        }
        /// <summary>
        /// 执行写入指令每5s执行一条写入 
        /// </summary>
        public void ExecuteWrite(List<Base_Wirte_VO> wirtes)
        {
            if (MysqlDb.MysqlClose) return;
            QueryTime();
            QueryVersion();
            if (isClose) return;
            if (wirtes.Count > 0)
            {
                for (int i = 0; i < wirtes.Count; i++)
                {
                    if (wirtes[i].exist)
                    {
                        switch (wirtes[i].type)
                        {
                            case Mysql_Type.InsertInto:
                                MysqlDb.InsertInto(wirtes[i].tableName, wirtes[i].columnValues);
                                break;
                                case Mysql_Type.UpdateInto:
                                MysqlDb.UpdateInto(wirtes[i].tableName, wirtes[i].columnNames, wirtes[i].columnValues, wirtes[i].selectkey, GetStr(wirtes[i].selectvalue));
                                break;
                            case Mysql_Type.Delete:
                                MysqlDb.Delete(wirtes[i].tableName, new string[] { "uid" }, new string[] { GetStr(SumSave.crt_user.uid)});
                                break;

                            default: break;
                        }
                        wirtes[i].exist = false;
                        //Base_Wirte_VO wirte = wirtes[i];
                        //wirtes.RemoveAt(i);
                        //wirtes.Add(wirte);
                    }
                }
                Alert_Dec.Show("数据存入成功");
            }
        }

        private string[] versions = new string[] {"0.2026.12", "0.2026.13", "0.2026.15" }; 
        /// <summary>
        /// 检测次数
        /// </summary>
        int versionsnumber = 0;
        /// <summary>
        /// 读取版本
        /// </summary>
        protected void QueryVersion()
        {
            int system_par = 1;
#if UNITY_EDITOR
            system_par = 1;
#elif UNITY_ANDROID
        system_par = 1;
           
#elif UNITY_IPHONE
        system_par = 2;
#endif

            SumSave.crt_versions = new user_versions();
            //mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.versions_task);
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.versions_task, "id", GetStr(system_par));
            if (mysqlReader == null) return;
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_versions.Version= mysqlReader.GetString(mysqlReader.GetOrdinal("Version"));
                    SumSave.crt_versions.Weather = mysqlReader.GetString(mysqlReader.GetOrdinal("Weather"));
                    SumSave.crt_versions.WeatherTime = mysqlReader.GetString(mysqlReader.GetOrdinal("WeatherTime"));
                    SumSave.crt_versions.Activity = mysqlReader.GetInt32(mysqlReader.GetOrdinal("Activity"));
                    //SumSave.crt_versions.tapversionversion = mysqlReader.GetString(mysqlReader.GetOrdinal("tapversionversion"));
                }
            }
            SumSave.OpenGame = false;
            foreach (string item in versions)
            {
                if (SumSave.crt_versions.Version == item)
                    SumSave.OpenGame = true;

            }

            if (!SumSave.OpenGame)
            {
                if (versionsnumber >= 3)
                {
                    Alert.Show("数据库连接失败", "请更新游戏");
                    CloseMySqlDB();
                    return;
                }
                versionsnumber++;
                Alert_Dec.Show("游戏版本不匹配，请更新游戏！");
            }
        }

        private void Close_Hide2()
        {
            UI_Manager.Instance.GetPanel<PanelMian>().Show();
        }
        /// <summary>
        /// 时间开关
        /// </summary>
        private bool open_time = true;
        /// <summary>
        /// 读取时间
        /// </summary>
        protected void QueryTime()
        {
            mysqlReader = MysqlDb.QueryTime();
            if (mysqlReader == null)
            {
                SumSave.is_ToMysqlOpen= false;
                return;
            }
            SumSave.is_ToMysqlOpen = true;
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    for (int i = 0; i < mysqlReader.FieldCount; i++)
                    {
                        if (open_time)
                        {
                            SumSave.nowtime = UnifiedDateTime.FromString(mysqlReader[i].ToString());
                            open_time = false;
                        }
                        else
                        { 
                          DateTime nowtime = UnifiedDateTime.FromString(mysqlReader[i].ToString());
                            int growTimeInt = Battle_Tool.SettlementTransport(SumSave.nowtime.ToString(), nowtime.ToString(), 2);
                            if (MathF.Abs( growTimeInt) > 60)
                            {
                                user_login = -1;
                            }
                        }
                        SumSave.is_ToMysqlOpen = true;
                    }
                }
            }
        }
    }
}
