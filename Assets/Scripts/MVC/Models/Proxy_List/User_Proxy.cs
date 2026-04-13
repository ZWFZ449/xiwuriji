using UnityEngine;
using Common;
using System;
using System.Collections.Generic;

namespace MVC
{
    /// <summary>
    ///  处理用户相关数据:登录,注册和注销
    /// </summary>
    public class User_Proxy : Base_Proxy
    {
        /// <summary>
        /// NAME
        /// </summary>
        public new const string NAME = "User_Proxy";

        public User_Proxy()
        {
            this.ProxyName = NAME;
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="list"></param>
        public void Execute_Write(List<Base_Wirte_VO> list)
        {
            OpenMySqlDB();
            //写入数据
            ExecuteWrite(list);

            CloseMySqlDB();
        }

        public void User_Login()
        {
            OpenMySqlDB();
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.Dream_user_base, "uid", GetStr(SumSave.uid));
            SumSave.crt_user = new user_base_vo();
            if (mysqlReader == null) return;
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_user = ReadDb.Read_user_base(mysqlReader);
                }
                SumSave.crt_user.Nowdate = SumSave.nowtime;
                Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.Dream_user_base, SumSave.crt_user.Set_Uptade_String(), SumSave.crt_user.Get_Update_Character());
            }
            else
            {
                SumSave.crt_user.uid = SumSave.uid;
                SumSave.crt_user.Nowdate = DateTime.Now;
                SumSave.crt_user.RegisterDate = DateTime.Now;
                SumSave.crt_user.par = SumSave.par;
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.Dream_user_base, SumSave.crt_user.Set_Instace_String());
            }
            Read_Instace();
            CloseMySqlDB();
        }

        private void Read_Instace()
        {
            Read_User_Unit();
            Read_User_Hero();
            Read_user_bag();
            Read_user_Equip();
            Read_User_Skill();
            Read_User_Pet();
            Read_User_Setting();
            Read_Signin();
            Read_illustrated();
            Read_global_gift();
            refresh_Max_Hero_Attribute();
            Read_global_battle_info();
        }
        /// <summary>
        /// 读取礼包
        /// </summary>
        private void Read_global_gift()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.dream_user_gift, "uid", GetStr(SumSave.crt_user.uid));
            SumSave.crt_global_gift = new data_global_gift_vo();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_global_gift=(ReadDb.Read(mysqlReader, new data_global_gift_vo()));
                }
            }
            else Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.dream_user_gift, SumSave.crt_global_gift.Set_Instace_String());
        }
        /// <summary>
        /// 交换数据
        /// </summary>
        public void read_Obtain_Info()
        {
            OpenMySqlDB();
            Read_global_battle_info();
            CloseMySqlDB();
        }
        private void Read_global_battle_info()
        {
            mysqlReader = MysqlDb.ReadGetLatest(Mysql_Table_Name.global_battle_info, "par", GetStr(SumSave.par), 100);
            SumSave.global_battle_info = new List<global_battle_info_VO>();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.global_battle_info.Add ( ReadDb.Read(mysqlReader,new global_battle_info_VO()));
                }
            }
        }

        /// <summary>
        /// 读取图鉴
        /// </summary>
        private void Read_illustrated()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.dream_user_illustrated, "uid", GetStr(SumSave.crt_user.uid));
            SumSave.crt_illustrated = new user_illustrated_vo();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_illustrated = ReadDb.Read_illustrated(mysqlReader);
                }
            }
            else//为空的话初始化数据
            {
                SumSave.crt_illustrated.Set_Instace_String();
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.dream_user_illustrated, SumSave.crt_illustrated.Set_Instace_String());
            }
        }
        /// <summary>
        /// 读取签到
        /// </summary>
        private void Read_Signin()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.dream_user_signin, "uid", GetStr(SumSave.crt_user.uid));
            SumSave.crt_signin = new user_signin_vo();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_signin = ReadDb.Read(mysqlReader, new user_signin_vo());
                }
            }
            else//为空的话初始化数据
            {
                SumSave.crt_signin.now_time = Convert.ToDateTime(SumSave.nowtime.AddDays(-1).ToString("yyyy-MM-dd"));
                SumSave.crt_signin.number = 0;
                SumSave.crt_signin.user_value = "";
                SumSave.crt_signin.Init();
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.dream_user_signin, SumSave.crt_signin.Set_Instace_String());
            }
        }
        /// <summary>
        /// 读取设置数据
        /// </summary>
        private void Read_User_Setting()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.user_data_setting, "uid", GetStr(SumSave.crt_user.uid));
            SumSave.crt_setting = new data_setting_vo();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_setting = Mysql_Read.Read(mysqlReader, new data_setting_vo());
                }
            }
            else Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.user_data_setting, SumSave.crt_setting.Set_Instace_String());
        }

        private void Read_User_Pet()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.dream_user_pet, "uid", GetStr(SumSave.crt_user.uid));
            SumSave.crt_pet = new dream_user_pet_vo();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_pet = Mysql_Read.Read(mysqlReader, new dream_user_pet_vo());
                }
            } 
            else
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.dream_user_pet, SumSave.crt_pet.Set_Instace_String());
        }

        private void Read_User_Unit()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.Dream_User, "uid", GetStr(SumSave.crt_user.uid));
            SumSave.crt_user_unit = new user_vo();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_user_unit = Mysql_Read.Read(mysqlReader, new user_vo());
                }
            }
            else
            {
                SumSave.crt_user_unit.Init("100000,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0");
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.Dream_User, SumSave.crt_user_unit.Set_Instace_String());
            }
        }
        private void Read_User_Skill()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.dream_user_skill, "uid", GetStr(SumSave.crt_user.uid));//读取角色信息
            SumSave.crt_skill = new dream_user_skill_vo();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_skill = Mysql_Read.ReadUserSkill(mysqlReader, new dream_user_skill_vo());
                }
            }
            else
            {
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.dream_user_skill, SumSave.crt_skill.Set_Instace_String());
            }
        }



        /// <summary>
        /// 刷新英雄属性
        /// </summary>
        private void refresh_Max_Hero_Attribute()
        {
            SumSave.crtMaxBattle = Tool_Battle.InitPlayerMaxBattle();
            Game_Omphalos.Refresh();
        }
        /// <summary>
        /// 刷新英雄属性
        /// </summary>
        public void Refresh_Max_Hero_Attribute()
        {
            refresh_Max_Hero_Attribute();
        }
        private void Read_user_Equip()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.dream_user_equip, "uid", GetStr(SumSave.crt_user.uid));//读取角色信息
            SumSave.crt_equips = new dream_user_equip_VO();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_equips = Mysql_Read.Read(mysqlReader, new dream_user_equip_VO());
                }
            }
            else
            {
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.dream_user_equip, SumSave.crt_equips.Set_Instace_String());
            }
        }
        private void Read_user_bag()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.dream_user_bag, "uid", GetStr(SumSave.crt_user.uid));//读取角色信息
            SumSave.crt_bags = new dream_user_bag_VO();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_bags = Mysql_Read.Read(mysqlReader, new dream_user_bag_VO());
                }
            }
            else
            { 
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.dream_user_bag, SumSave.crt_bags.Set_Instace_String());
            }
        }

        private void Read_User_Hero()
        {
            mysqlReader = MysqlDb.SelectWhere(Mysql_Table_Name.dream_base_hero, new string[] { "par", "uid" }, new string[] { "=", "=" },
                new string[] { SumSave.par.ToString(), SumSave.crt_user.uid });
            SumSave.crtHero = new Dream_User_Hero_VO();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crtHero = Mysql_Read.Read(mysqlReader, new Dream_User_Hero_VO());
                }
            }
            else
            {
               
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.dream_base_hero, SumSave.crtHero.Set_Instace_String());
            }
        }
    }
}