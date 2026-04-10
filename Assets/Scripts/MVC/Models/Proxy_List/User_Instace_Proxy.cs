using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MVC
{
    public class User_Instace_Proxy : Base_Proxy
    {
        /// <summary>
        ///  NAME
        /// </summary>
        public new const string NAME = "User_Instace_Proxy";
        private const string BaseUserID = "验证信息";
        /// <summary>
        ///  构造函数
        /// </summary>
        public User_Instace_Proxy()
        {
            this.ProxyName = NAME;
        }
        /// <summary>
        /// 登录
        /// </summary>
        public void User_Login()
        {
            OpenMySqlDB();
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.mo_user_base, "uid", GetStr(SumSave.uid));
            SumSave.crt_user = new user_base_vo();
            if (mysqlReader == null) return;
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                { 
                    SumSave.crt_user = ReadDb.Read_user_base(mysqlReader);
                }
                SumSave.crt_user.Nowdate = DateTime.Now;
                Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_base, SumSave.crt_user.Set_Uptade_String(), SumSave.crt_user.Get_Update_Character());
            }
            else
            {
                SumSave.crt_user.uid = SumSave.uid;// Guid.NewGuid().ToString("N");// new user_base_vo(, DateTime.Now, DateTime.Now, SumSave.par); //Guid.NewGuid().ToString("N");
                SumSave.crt_user.Nowdate = DateTime.Now;
                SumSave.crt_user.RegisterDate = DateTime.Now;
                SumSave.crt_user.par = SumSave.par;
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.mo_user_base, SumSave.crt_user.Set_Instace_String());
            }
            Read_Instace();
            CloseMySqlDB();
        }
        public void Read_Crate_IPhone_Uid(string[] id)
        {
            IPhone_Login(id);
        }
        /// <summary>
        /// 苹果端注销账号
        /// </summary>
        /// <param name="id"></param>
        public void Read_Crate_IPhone_logoff(string[] id)
        {
            IPhone_Login(id);
        }


        /// <summary>
        /// 苹果端登录
        /// </summary>
        /// <param name="id"></param>
        private void IPhone_Login(string[] id)
        {
            OpenMySqlDB();
            mysqlReader = MysqlDb.SelectWhere(Mysql_Table_Name.mo_user_iphone, new string[] { "par", "account", "password" }, new string[] { "=", "=", "=" },
                new string[] { SumSave.par.ToString(), id[0], id[1] });
            string crt_verify = "";
            if (id[0] == "admin001")//苹果测试账户
            {
                SumSave.uid = Guid.NewGuid().ToString("N");
                Game_Omphalos.i.Alert_Show("创建角色成功");
                crt_verify = Guid.NewGuid().ToString("N");
                Wirte_Id(crt_verify);
                ///新用户
                MysqlDb.InsertInto(Mysql_Table_Name.mo_user_iphone, new string[] {
                    GetStr(0), GetStr(SumSave.par), GetStr(id[0]), GetStr(id[1]), GetStr(SumSave.uid), GetStr(crt_verify)});

                SumSave.ios_account_number= id[0];

                Login();
                Game_Omphalos.i.Alert_Show("登录成功");
                CloseMySqlDB();
                return;
            }

            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    crt_verify= mysqlReader.GetString(mysqlReader.GetOrdinal("verify"));
                    SumSave.uid = mysqlReader.GetString(mysqlReader.GetOrdinal("uid"));
                }
              
            }
            else
            {
                mysqlReader = MysqlDb.SelectWhere(Mysql_Table_Name.mo_user_iphone, new string[] { "par", "account" }, new string[] { "=", "=" },
                new string[] { SumSave.par.ToString(), id[0]});
                if (mysqlReader.HasRows)
                {
                    Game_Omphalos.i.Alert_Info("账号已存在");
                    CloseMySqlDB();
                    return;
                }
                SumSave.uid = Guid.NewGuid().ToString("N");
                Game_Omphalos.i.Alert_Show("创建角色成功");
                crt_verify = Guid.NewGuid().ToString("N");
                Wirte_Id(crt_verify);
                ///新用户
                MysqlDb.InsertInto(Mysql_Table_Name.mo_user_iphone, new string[] { 
                    GetStr(0), GetStr(SumSave.par), GetStr(id[0]), GetStr(id[1]), GetStr(SumSave.uid), GetStr(crt_verify)});
            }
            if (!Crt_Verify(crt_verify))
            {
                SumSave.uid = null;
                Game_Omphalos.i.Alert_Info("查询不到设备信息,请联系管理\nqq 386246268");
            }
            else
            {
                Login();
                Game_Omphalos.i.Alert_Show("登录成功");
            } 
            CloseMySqlDB();
        }
        /// <summary>
        /// 读取试练塔排行榜
        /// </summary>
        public void Read_Trial_Tower()
        {
            //Program.Read_path_Mysql(Mysql_Table_Name.user_trial_towers);
            OpenMySqlDB();
            read_Trial_Tower();
            CloseMySqlDB();
        }

        public void Read_Mysql_Base_Time()
        {
            OpenMySqlDB();
            QueryTime();
            CloseMySqlDB();
        }

        private void read_Trial_Tower()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.user_trial_towers, "par", GetStr(SumSave.par));
            SumSave.crt_Trial_Tower_rank = new mo_world_boss_rank();
            if (mysqlReader == null) return;
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    //获取等级
                    SumSave.crt_Trial_Tower_rank.Ranking_value = mysqlReader.GetString(mysqlReader.GetOrdinal("value"));
                    SumSave.crt_Trial_Tower_rank.InitLists();
                }
            }
            else
            {
                MysqlDb.InsertInto(Mysql_Table_Name.user_trial_towers, SumSave.crt_Trial_Tower_rank.Set_Instace_String());
            }
        }
        /// <summary>
        /// 刷新排行榜
        /// </summary>
        public void Refresh_Rank()
        {
            //List<Base_Wirte_VO> list = new List<Base_Wirte_VO>();
            //list.Add(Mysql_Read.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.user_rank, SumSave.user_ranks.Set_Uptade_String(), SumSave.user_ranks.Get_Update_Character()));//角色丹药Buff更新数据
            ////Program.MysqlMain(list);
            OpenMySqlDB();
            if (MysqlDb.MysqlClose) return;
            MysqlDb.UpdateInto(Mysql_Table_Name.user_rank, SumSave.user_ranks.Get_Update_Character(), SumSave.user_ranks.Set_Uptade_String(),
                "par", GetStr(SumSave.par));
            CloseMySqlDB();
        }

        public void Refresh_Endless_Tower()
        {
            //List<Base_Wirte_VO> list = new List<Base_Wirte_VO>();
            //list.Add(Mysql_Read.GetQueue(Mysql_Type.UpdateInto, 
            //    Mysql_Table_Name.user_endless_battle, SumSave.crt_endless_battle.Set_Uptade_String(),
            //    SumSave.crt_endless_battle.Get_Update_Character()));
            //Program.MysqlMain(list);
            OpenMySqlDB();
            MysqlDb.UpdateInto(Mysql_Table_Name.user_endless_battle, SumSave.crt_endless_battle.Get_Update_Character(), SumSave.crt_endless_battle.Set_Uptade_String(),
               "par", GetStr(SumSave.par));
            CloseMySqlDB();
        }


        /// <summary>
        /// 刷新试练塔排行榜
        /// </summary>
        public void Refresh_Trial_Tower(int trial_storey)
        {
            OpenMySqlDB();
            read_Trial_Tower();
            bool exist = true;
            for (int i = 0; i < SumSave.crt_Trial_Tower_rank.lists.Count; i++)
            {
                if (SumSave.crt_Trial_Tower_rank.lists[i].Item1 == SumSave.crt_user.uid)
                {
                    exist = false;
                    SumSave.crt_Trial_Tower_rank.lists[i] = (SumSave.crt_user.uid, SumSave.crt_MaxHero_okd.show_name, trial_storey);
                }
            }
            if (exist) SumSave.crt_Trial_Tower_rank.lists.Add((SumSave.crt_user.uid, SumSave.crt_MaxHero_okd.show_name, trial_storey));
            SumSave.crt_Trial_Tower_rank.lists = ArrayHelper.OrderDescding(SumSave.crt_Trial_Tower_rank.lists, x => x.Item3);
            //List<Base_Wirte_VO> list = new List<Base_Wirte_VO>();
            //list.Add(Mysql_Read.GetQueue(Mysql_Type.UpdateInto,
            //    Mysql_Table_Name.user_trial_towers, SumSave.crt_Trial_Tower_rank.Set_Uptade_String(),
            //    SumSave.crt_Trial_Tower_rank.Get_Update_Character()));
            ////Program.MysqlMain(list);
            MysqlDb.UpdateInto(Mysql_Table_Name.user_trial_towers, SumSave.crt_Trial_Tower_rank.Get_Update_Character(), SumSave.crt_Trial_Tower_rank.Set_Uptade_String(), "par", GetStr(SumSave.par));
            CloseMySqlDB();
        }


        /// <summary>
        /// 获得世界boss进度
        /// </summary>
        /// <param name="id"></param>
        public void Read_Crate_world_boss_Login()
        {
            world_boss_Login();
        }

        /// <summary>
        /// 获取世界boss进度数据
        /// </summary>
        /// <param name="BossName"></param>

        private void world_boss_Login()
        {
            OpenMySqlDB();
            if (MysqlDb.MysqlClose) return;
            mysqlReader= MysqlDb.Select(Mysql_Table_Name.db_world_boss, "par", GetStr(SumSave.par));
            if (mysqlReader == null) return;
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_world_boos.name = mysqlReader.GetString(mysqlReader.GetOrdinal("boos_Name"));
                    SumSave.db_world_boos.InitFinalDamage(long.Parse(mysqlReader.GetString(mysqlReader.GetOrdinal("maxhp"))));
                    SumSave.db_world_boos.number= mysqlReader.GetInt32(mysqlReader.GetOrdinal("number")); 
                    SumSave.db_world_boos.DamageLevel_value=mysqlReader.GetString(mysqlReader.GetOrdinal("DamageLevel"));
                    SumSave.db_world_boos.UpTime = mysqlReader.GetString(mysqlReader.GetOrdinal("UpTime"));
                    SumSave.db_world_boos.Init();
                }
            }else
            {
                SumSave.db_world_boos.name = "墟界法王";
                SumSave.db_world_boos.number = 1;
                SumSave.db_world_boos.InitFinalDamage(SumSave.db_world_boos.BossHpBasic* SumSave.db_world_boos.number);
                SumSave.db_world_boos.DamageLevel_value = "1 1|2 3000|3 6000|4 10000|5 20000|6 35000|7 100000|8 200000";
                SumSave.db_world_boos.UpTime = SumSave.nowtime.ToString("yyyy-MM-dd HH:mm:ss");
                SumSave.db_world_boos.Init();
                MysqlDb.InsertInto(Mysql_Table_Name.db_world_boss, SumSave.db_world_boos.Set_Instace_String());
            }
            CloseMySqlDB();
        }
        /// <summary>
        /// 更新世界boss进度
        /// </summary>
        public void Read_Crate_world_boss_update()
        {
            world_boss_update();
        }
        /// <summary>
        /// 更新世界boss进度
        /// </summary>
        private void world_boss_update()
        {
            //List<Base_Wirte_VO> list = new List<Base_Wirte_VO>();
            //list.Add(Mysql_Read.GetQueue(Mysql_Type.UpdateInto,
            //    Mysql_Table_Name.db_world_boss, SumSave.db_world_boos.Set_Uptade_String(),
            //    SumSave.db_world_boos.Get_Update_Character()));
            //Program.MysqlMain(list);
            OpenMySqlDB();
            MysqlDb.UpdateInto(Mysql_Table_Name.db_world_boss, SumSave.db_world_boos.Get_Update_Character(), SumSave.db_world_boos.Set_Uptade_String(), "par", GetStr(SumSave.par));
            CloseMySqlDB();
        }


        /// <summary>
        /// 读取实例
        /// </summary>
        /// <param name="crt_verify"></param>
        /// <returns></returns>
        private bool Crt_Verify(string crt_verify)
        {
            if(crt_verify == "99") return true;
            bool exsit = false;
            if (PlayerPrefs.HasKey(BaseUserID))
            {
                string[] password = PlayerPrefs.GetString(BaseUserID).Split(' ');
                foreach (var item in password)
                {
                    if (item == crt_verify)
                    {
                        return true;
                    }
                }
            }
            if (crt_verify == "-1")//更换设备
            {
                exsit = true;
                crt_verify = Guid.NewGuid().ToString("N");
                Wirte_Id(crt_verify);
                MysqlDb.UpdateInto(Mysql_Table_Name.mo_user_iphone, new string[] {
                     "verify"}, new string[] { GetStr(crt_verify) }, "uid", GetStr(SumSave.uid));
            }
            return exsit;
        }
        private void Wirte_Id(string crt_verify)
        {
            string password = "";
            if (PlayerPrefs.HasKey(BaseUserID))
            {
                password = PlayerPrefs.GetString(BaseUserID);
            }
            password += (password == "" ? "" : " ") + crt_verify;
            PlayerPrefs.SetString(BaseUserID, password);
        }

        /// <summary>
        /// 记录并清空世界boss伤害
        /// </summary>
        public void Read_Crate_RecordAndClearWorldBoss()
        {
            RecordAndClearWorldBoss();
        }

        /// <summary>
        /// 记录并清空世界boss伤害
        /// </summary>
        private void RecordAndClearWorldBoss()
        {
            OpenMySqlDB();
            Read_db_world_boss();
            for (int i = 0; i < SumSave.db_world_boss_hurt.Count; i++)
            {
                if (SumSave.db_world_boss_hurt[i].par==SumSave.par)
                {
                    MysqlDb.InsertInto(Mysql_Table_Name.history_world_boss, SumSave.db_world_boss_hurt[i].Set_Instace_String(SumSave.db_world_boss_hurt[i].uid));
                    MysqlDb.Delete(Mysql_Table_Name.user_world_boss_copy1, new string[]{"uid","par" },
                        new string[] { GetStr(SumSave.db_world_boss_hurt[i].uid), GetStr(SumSave.db_world_boss_hurt[i].par) });
                }
            }
            SumSave.db_world_boss_hurt.Clear();
            SumSave.crt_world_boss_hurt = new user_world_boss(0, SumSave.nowtime, SumSave.par, SumSave.uid);
            SumSave.crt_world_boss_hurt.damage = 0;
            SumSave.crt_world_boss_hurt.datetime = SumSave.nowtime;
            //Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.user_world_boss, SumSave.crt_world_boss_hurt.Set_Instace_String());
            Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.user_world_boss_copy1, SumSave.crt_world_boss_hurt.Set_Instace_String());

            CloseMySqlDB();
        }

        /// <summary>
        /// 获得全服玩家的世界Boss伤害
        /// </summary>
        public void Read_db_world_boss()
        {
            mysqlReader = MysqlDb.SelectWhere(Mysql_Table_Name.user_world_boss_copy1, new string[] { "par" }, new string[] { "=" },
              new string[] { SumSave.par.ToString() });

            SumSave.db_world_boss_hurt = new List<user_world_boss>();
            if (mysqlReader == null) return;
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_world_boss_hurt.Add(ReadDb.Read_world_boss(mysqlReader));
                }
            }
        }



        public void Read_Crate_Uid(string[] id)
        {
            Tap_Login(id);
        }
        /// <summary>
        /// 读取TapId
        /// </summary>
        /// 
        private void Tap_Login(string[] id)
        {
            OpenMySqlDB();
            mysqlReader = MysqlDb.SelectWhere(Mysql_Table_Name.mo_user_tap, new string[] { "par", "Tapid" }, new string[] { "=", "=" }, new string[] { SumSave.par.ToString(), id[0] });
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.uid = mysqlReader.GetString(mysqlReader.GetOrdinal("uid"));
                }
            }
            else
            {
                SumSave.uid = Guid.NewGuid().ToString("N");
                ///新用户
                MysqlDb.InsertInto(Mysql_Table_Name.mo_user_tap, new string[] { GetStr(0), GetStr(SumSave.par), GetStr(id[0]), GetStr(id[1]), GetStr(SumSave.uid) });

            }
            CloseMySqlDB();
            Login();
        }
        /// <summary>
        /// 登录
        /// </summary>
        private void Login()
        {
            UI_Manager.Instance.GetPanel<panel_login>().Login();
        }

        //初始化文件
        private void Init()
        {
            //SumSave.crt_bag = new List<Bag_Base_VO>();
            //SumSave.crt_euqip = new List<Bag_Base_VO>();
            //SumSave.crt_skills = new List<base_skill_vo>();
            //SumSave.crt_bag_resources = new bag_Resources_vo();
            //SumSave.crt_house = new List<Bag_Base_VO>();
            //if (SumSave.crt_resources.bag_value.Length > 0)
            //{ 
            //    string[] bag_value = SumSave.crt_resources.bag_value.Split(';');
            //    for (int i = 0; i < bag_value.Length; i++)
            //    {
            //        if (bag_value[i].Length > 0)
            //        {
            //            SumSave.crt_bag.Add(tool_Categoryt.Read_Bag(bag_value[i]));
            //        }
            //    }
                
            //}
            //if (SumSave.crt_resources.house_value.Length > 0)
            //{
            //    string[] bag_value = SumSave.crt_resources.house_value.Split(';');
            //    for (int i = 0; i < bag_value.Length; i++)
            //    {
            //        if (bag_value[i].Length > 0)
            //        {
            //            SumSave.crt_house.Add(tool_Categoryt.Read_Bag(bag_value[i]));
            //        }
            //    }

            //}
            //if (SumSave.crt_resources.equip_value.Length > 0)
            //{
            //    string[] equip_value = SumSave.crt_resources.equip_value.Split(';');
            //    for (int i = 0; i < equip_value.Length; i++)
            //    {
            //        if (equip_value[i].Length > 0)
            //        {
            //            SumSave.crt_euqip.Add(tool_Categoryt.Read_Bag(equip_value[i]));
            //        }
            //    }
            //}
            //if (SumSave.crt_resources.skill_value.Length > 0)
            //{
            //    string[] skill_value = SumSave.crt_resources.skill_value.Split(';');
            //    for (int i = 0; i < skill_value.Length; i++)
            //    {
            //        if (skill_value[i].Length > 0)
            //        {
            //            base_skill_vo skill = new base_skill_vo();
            //            skill.user_value = skill_value[i];
            //            SumSave.crt_skills.Add(tool_Categoryt.Read_skill(skill));
            //        }
            //    }
            //}
            //SumSave.crt_bag_resources.Init(SumSave.crt_resources.material_value);
          
        }
        public void Delete(string dec)
        {
            OpenMySqlDB();
            if (!MysqlDb.MysqlClose)
            {
                MysqlDb.UpdateInto(Mysql_Table_Name.mo_user_base, new string[] { "Nowdate" }, new string[] { GetStr(dec) }, "uid", GetStr(SumSave.crt_user.uid));//login
                MysqlDb.UpdateInto(Mysql_Table_Name.user_login, new string[] { "login" }, new string[] { GetStr(-1) }, "uid", GetStr(SumSave.crt_user.uid));
            }
            CloseMySqlDB();
        }
        private List<string> log_list = new List<string>();
        public void loglist(string dec)
        {
            log_list.Add(dec);
            OpenMySqlDB();  
            if (!MysqlDb.MysqlClose)
            {
                for (int i = 0; i < log_list.Count; i++)
                {
                    MysqlDb.InsertInto(Mysql_Table_Name.loglist, new string[] { GetStr(0), GetStr(log_list[i]) });
                }
                log_list.Clear();
            }
            CloseMySqlDB();
        }

      



        /// <summary>
        /// 读取自身数据
        /// </summary>
        private void Read_Instace()
        {
            world_boss_read_User_Rank();
            read_User_Rank();
            Read_User_Unit();
            Read_User_Hero();
            Read_User_Resources();
            Read_User_Setting();
            Read_User_Artifact();
            Read_User_Pass();
            Read_User_Plant();
            Read_User_Hatching();
            Read_World();
            Read_User_Achievenment();
            Read_Needlist();
            Read_Seed();
            Read_Signin();
            Read_User_Pet();
            Read_user_Greenhand();
            Read_user_Collect();
            Read_user_player_Buff();
            Read_User_Mail();
            Read_User_Reward();
            read_EndlessBattle();
            read_Trial_Tower();
            Read_user_world_boss();
            world_boss_Login();
            refresh_Max_Hero_Attribute();
        }






        /// <summary>
        /// 读取自身世界Boss伤害
        /// </summary>
        public void Read_user_world_boss()
        {
            //mysqlReader = MysqlDb.Select(Mysql_Table_Name.user_world_boss, "uid", GetStr(SumSave.crt_user.uid));
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.user_world_boss_copy1, "uid", GetStr(SumSave.crt_user.uid));
            SumSave.crt_world_boss_hurt= new user_world_boss(0,SumSave.nowtime,SumSave.par,SumSave.uid);

            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_world_boss_hurt = ReadDb.Read_world_boss(mysqlReader);
                }

            }else
            {
                SumSave.crt_world_boss_hurt.damage=0;
                SumSave.crt_world_boss_hurt.datetime=SumSave.nowtime;
                //Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.user_world_boss, SumSave.crt_world_boss_hurt.Set_Instace_String());
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.user_world_boss_copy1, SumSave.crt_world_boss_hurt.Set_Instace_String());
            }

        }
        /// <summary>
        /// 累计奖励
        /// </summary>
        private void Read_User_Reward()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.mo_user_rewards_state, "uid", GetStr(SumSave.crt_user.uid));
            SumSave.crt_accumulatedrewards = new user_Accumulatedrewards_vo("",0,0);

            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_accumulatedrewards = ReadDb.Read_Accumulatedrewards(mysqlReader);
                }

            }
            else//为空的话初始化数据
            {
                SumSave.crt_accumulatedrewards.user_value = "";
                SumSave.crt_accumulatedrewards.Init(0,0);
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.mo_user_rewards_state, SumSave.crt_accumulatedrewards.Set_Instace_String());
            }
        }
        /// <summary>
        /// 玩家buff
        /// </summary>
        public void Read_user_player_Buff()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.user_player_buff, "uid", GetStr(SumSave.crt_user.uid));
            SumSave.crt_player_buff = new user_player_Buff();

            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_player_buff = ReadDb.Read(mysqlReader, new user_player_Buff());
                }

            }
            else//为空的话初始化数据
            {
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.user_player_buff, SumSave.crt_player_buff.Set_Instace_String());
            }

        }
        /// <summary>
        /// 邮件
        /// </summary>
        public void Read_User_Mail()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.user_emial, "uid", GetStr(SumSave.crt_user.uid));
            SumSave.CrtMail = new user_mail_vo();
            if (mysqlReader == null) return;

            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.CrtMail = ReadDb.Read(mysqlReader, new user_mail_vo());
                }

            }
            else//为空的话初始化数据
            {
                SumSave.CrtMail.user_value = "";
                SumSave.CrtMail.Init(); 
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.user_emial, SumSave.CrtMail.Set_Instace_String());
            }

        }


        /// <summary>
        /// 读取收集信息
        /// </summary>
        public void Read_user_Collect()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.mo_user_collect, "uid", GetStr(SumSave.crt_user.uid));
            SumSave.crt_collect = new user_collect_vo();


            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_collect= ReadDb.Read(mysqlReader, new user_collect_vo());
                }
                string[] Splits = SumSave.crt_collect.collect_value.Split('|');
                for (int i = 0; i < Splits.Length; i++)
                {
                    if (Splits[i].Length > 0)
                    {
                        string[] Splits2 = Splits[i].Split(' ');
                        SumSave.crt_collect.user_collect_dic.Add(Splits2[0],int.Parse(Splits2[1]));
                    }
                }
            }
            else//为空的话初始化数据
            {
                SumSave.crt_collect.collect_Merge();
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.mo_user_collect, SumSave.crt_collect.Set_Instace_String());
            }

        }



        /// <summary>
        /// 新手任务
        /// </summary>
        public void Read_user_Greenhand()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.mo_user_greenhandguide, "uid", GetStr(SumSave.crt_user.uid));
            SumSave.crt_greenhand = new user_greenhand_vo();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_greenhand = ReadDb.Read(mysqlReader, new user_greenhand_vo());
                }
            }
            else//为空的话初始化数据
            {
                SumSave.crt_greenhand.user_value = "";
                foreach (var item in SumSave.GreenhandGuide_TotalTasks.Keys)
                {
                    SumSave.crt_greenhand.crt_task = item;
                    break;
                }
                SumSave.crt_greenhand.Init();
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.mo_user_greenhandguide, SumSave.crt_greenhand.Set_Instace_String());
            }
        }


        /// <summary>
        /// 读取飘窗消息
        /// </summary>
        public void Read_user_messageWindow()
        {
            OpenMySqlDB();
            if (MysqlDb.MysqlClose) return;
            if (SumSave.crt_message_window != null) return;
            SumSave.crt_message_window = new List<(int, string, string)>();
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.user_message_window);
            if (mysqlReader == null) return;

            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_message_window.Add((mysqlReader.GetInt32(mysqlReader.GetOrdinal("id")), mysqlReader.GetString(mysqlReader.GetOrdinal("prizedraw_uid")), mysqlReader.GetString(mysqlReader.GetOrdinal("prizedraw_value"))));

                }
            }
            if (SumSave.crt_message_window.Count > 50)
            {
                int max = SumSave.crt_message_window.Count - 50;
                for (int i = 0; i < max; i++)
                {
                    MysqlDb.InsertInto(Mysql_Table_Name.user_message_window, new string[] { GetStr(0), GetStr(SumSave.crt_message_window[i].Item2), GetStr(SumSave.crt_message_window[i].Item3) });
                    MysqlDb.Delete(Mysql_Table_Name.user_message_window, new string[] { "id" }, new string[] { GetStr(SumSave.crt_message_window[i].Item1) });
                }
                for (int i = 0; i < max; i++)
                {
                    SumSave.crt_message_window.RemoveAt(0);
                }

            }
            CloseMySqlDB();
        }
        /// <summary>
        /// 写入飘窗消息
        /// </summary>
        /// <param name="value"></param>
        public void Refres_huser_messageWindow(string value)
        {
            int index = SumSave.crt_message_window.Count > 0 ? SumSave.crt_message_window[SumSave.crt_message_window.Count - 1].Item1 + 1 : 1;
            SumSave.crt_message_window.Add((index, SumSave.crt_user.uid, value));
            Read_user_messageWindow();
            index = (SumSave.crt_message_window.Count > 0 ? SumSave.crt_message_window[SumSave.crt_message_window.Count - 1].Item1 + 1 : 1);
            OpenMySqlDB();
            MysqlDb.InsertInto(Mysql_Table_Name.user_message_window, new string[] { GetStr(0), GetStr(index), GetStr(SumSave.crt_user.uid), GetStr(value) });
            CloseMySqlDB();
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
        /// 读取需求列表
        /// </summary>
        private void Read_Needlist()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.mo_user_needlist, "uid", GetStr(SumSave.crt_user.uid));
            SumSave.crt_needlist= new user_needlist_vo();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_needlist = ReadDb.Read(mysqlReader, new user_needlist_vo());
                }
            }
            else//为空的话初始化数据
            {
                SumSave.crt_needlist.store_value = "";
                SumSave.crt_needlist.map_value = "";
                SumSave.crt_needlist.fate_value = "";
                SumSave.crt_needlist.user_value = "100 100,0,0,0,0";
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.mo_user_needlist, SumSave.crt_needlist.Set_Instace_String());
            }
        }



        /// <summary>
        /// 读取炼丹数据
        /// </summary>
        private void Read_Seed()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.mo_user_seed, "uid", GetStr(SumSave.crt_user.uid));
            SumSave.crt_seeds = new bag_seed_vo();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_seeds = ReadDb.Read(mysqlReader, new bag_seed_vo());
                }
            }
            else
            {
                SumSave.crt_seeds.user_value = "";
                SumSave.crt_seeds.formula_value = "";
                SumSave.crt_seeds.use_value = "";
                SumSave.crt_seeds.Init();
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.mo_user_seed, SumSave.crt_seeds.Set_Instace_String());
            }
        }


        /// <summary>
        /// 读取世界Boss排行榜
        /// </summary>
        private void world_boss_read_User_Rank()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.user_world_boss_rank, "par", GetStr(SumSave.par));
            SumSave.crt_world_boss_rank = new mo_world_boss_rank();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    //获取等级
                    SumSave.crt_world_boss_rank.Ranking_value = mysqlReader.GetString(mysqlReader.GetOrdinal("value"));
                    SumSave.crt_world_boss_rank.InitLists();
                }

            }
            else
            {
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.user_world_boss_rank, SumSave.crt_world_boss_rank.Set_Instace_String());
            }
        }

        /// <summary>
        /// 读取无尽塔排行榜
        /// </summary>
        private void read_EndlessBattle()
        {

            mysqlReader = MysqlDb.Select(Mysql_Table_Name.user_endless_battle, "par", GetStr(SumSave.par));
            SumSave.crt_endless_battle = new user_endless_battle();
            if (mysqlReader == null) return;
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_endless_battle.endless_value = mysqlReader.GetString(mysqlReader.GetOrdinal("value"));
                }
                SumSave.crt_endless_battle.Split_endless();
            }
            else
            {
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.user_endless_battle, SumSave.crt_endless_battle.Set_Instace_String());
            }
        }


        /// <summary>
        /// 读取战力排行榜
        /// </summary>
        private void read_User_Rank()
        {
            if (MysqlDb.MysqlClose) return;
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.user_rank, "par", GetStr(SumSave.par));
            SumSave.user_ranks = new rank_vo();
            if (mysqlReader == null) return;

            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    //获取等级
                    SumSave.user_ranks.Ranking_value = mysqlReader.GetString(mysqlReader.GetOrdinal("value"));
                }
                string[] splits = SumSave.user_ranks.Ranking_value.Split(';');
                //读取排行榜
                foreach (string base_value in splits)
                {
                    if (base_value != "")
                    {
                        base_rank_vo bag_Base_VO = new base_rank_vo();
                        bag_Base_VO.SetPropertyValue(base_value);
                        //Debug.Log(bag_Base_VO.name);
                        SumSave.user_ranks.lists.Add(bag_Base_VO);
                    }
                }
            }
            else
            {
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.user_rank, SumSave.user_ranks.Set_Instace_String());
            }
        }

        /// <summary>
        /// 读取排行榜
        /// </summary>
        public void Read_User_Rank()
        {
            //Program.Read_path_Mysql(Mysql_Table_Name.user_rank);
            OpenMySqlDB();
            //if (MysqlDb.MysqlClose)
            //{
            read_User_Rank();
            //}
            CloseMySqlDB();
        }

        /// <summary>
        /// 读取无尽塔排行榜
        /// </summary>
        public void Read_read_EndlessBattle()
        {
            //Program.Read_path_Mysql(Mysql_Table_Name.user_endless_battle);
            OpenMySqlDB();
            read_EndlessBattle();  
            CloseMySqlDB();
        }
        public void Read_Mail()
        {
            OpenMySqlDB();
            SumSave.Db_Mails = new List<db_mail_vo>();
            if (MysqlDb.MysqlClose) return;//未联网
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.server_mail);
            if (mysqlReader == null) return;

            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.Db_Mails.Add(ReadDb.Read_mail(mysqlReader));
                }
            }
            CloseMySqlDB();
        }
        /// <summary>
        /// 读取自身成就
        /// </summary>
        private void Read_User_Achievenment()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.mo_user_achieve, "uid", GetStr(SumSave.crt_user.uid));
            SumSave.crt_achievement = new user_achievement_vo();
            
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_achievement = ReadDb.Read(mysqlReader, new user_achievement_vo());
                }
            }
            else
            {
             
                SumSave.crt_achievement.achievement_lvs ="";
                SumSave.crt_achievement.achievement_exp = "";
                MysqlDb.InsertInto(Mysql_Table_Name.mo_user_achieve, SumSave.crt_achievement.Set_Instace_String());
                SumSave.crt_achievement.Init();
            }
        }

        /// <summary>
        /// 获取自身宠物信息
        /// </summary>
        private void Read_User_Pet()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.mo_user_pet, "uid", GetStr(SumSave.crt_user.uid));
            SumSave.crt_pet_Old = new user_pet_vo();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_pet_Old = ReadDb.Read(mysqlReader, new user_pet_vo());
                }
            }
            else
            {  
                SumSave.crt_pet_Old.pet_value = "";
                SumSave.crt_pet_Old.Init("");
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.mo_user_pet, SumSave.crt_pet_Old.Set_Instace_String());
            }
        }



        /// <summary>
        /// 宠物孵化数据
        /// </summary>
        private void Read_User_Hatching()
        {   
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.mo_user_pet_explore, "uid", GetStr(SumSave.crt_user.uid));
            SumSave.crt_explore = new user_explore_vo();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_explore = ReadDb.Read(mysqlReader, new user_explore_vo());
                }
            }
            else
            {
                SumSave.crt_explore.user_value = "";
                SumSave.crt_explore.Init();
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.mo_user_pet_explore, SumSave.crt_explore.Set_Instace_String());
            }
        }



        /// <summary>
        /// 种植数据
        /// </summary>
        private void Read_User_Plant()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.mo_user_plant, "uid", GetStr(SumSave.crt_user.uid));
            SumSave.crt_plant = new user_plant_vo();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_plant = ReadDb.Read(mysqlReader, new user_plant_vo());
                }
            }
            else
            {
                SumSave.crt_plant.user_value = "";
                SumSave.crt_plant.Init();
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.mo_user_plant, SumSave.crt_plant.Set_Instace_String());
            }
        }


        /// <summary>
        /// 通行证
        /// </summary>
        private void Read_User_Pass()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.mo_user_pass, "uid", GetStr(SumSave.crt_user.uid));
            SumSave.crt_pass = new user_pass_vo();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_pass = ReadDb.Read(mysqlReader, new user_pass_vo());
                }
            }
            else
            {
                SumSave.crt_pass.user_value = "";
                SumSave.crt_pass.day_state_value = "0|0|0|0|0|0";
                SumSave.crt_pass.Init();
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.mo_user_pass, SumSave.crt_pass.Set_Instace_String());
            }
        }

        /// <summary>
        /// 获取基础货币
        /// </summary>
        private void Read_User_Unit()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.mo_user, "uid", GetStr(SumSave.crt_user.uid));
            SumSave.crt_user_unit = new user_vo();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_user_unit = ReadDb.Read(mysqlReader, new user_vo());
                }
            }
            else
            {
                SumSave.crt_user_unit.Init("10000,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0");
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.mo_user, SumSave.crt_user_unit.Set_Instace_String());
            }
        }
        /// <summary>
        /// 读取神器数据
        /// </summary>
        private void Read_User_Artifact()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.mo_user_artifact, "uid", GetStr(SumSave.crt_user.uid));
            SumSave.crt_artifact = new user_artifact_vo();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_artifact = ReadDb.Read(mysqlReader, new user_artifact_vo());
                }
            }
            else
            {
                SumSave.crt_artifact.artifact_value = "";
                SumSave.crt_artifact.Init();
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.mo_user_artifact, SumSave.crt_artifact.Set_Instace_String());
            }
        }
        /// <summary>
        /// 读取设置文件
        /// </summary>
        private void Read_User_Setting()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.mo_user_setting, "uid", GetStr(SumSave.crt_user.uid));
            SumSave.crt_settingold = new user_base_setting_vo();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_settingold = ReadDb.Read(mysqlReader, new user_base_setting_vo());
                }
            }
            else
            {
                SumSave.crt_settingold.user_value = "0 0 0 0 0 0 0 0 0";
                SumSave.crt_settingold.Init();
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.mo_user_setting, SumSave.crt_settingold.Set_Instace_String());
            }
            if (SumSave.crt_settingold.user_setting.Count < SumSave.db_sttings.Count)
            {
                for (int i = SumSave.crt_settingold.user_setting.Count; i < SumSave.db_sttings.Count; i++)
                { 
                    SumSave.crt_settingold.user_setting.Add(0);
                }
            }
            
        }
        /// <summary>
        /// 读取世界数据
        /// </summary>
        public void Read_World()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.mo_user_world, "uid", GetStr(SumSave.crt_user.uid));
            if (mysqlReader.HasRows)
            {
                SumSave.crt_world = new user_world_vo();
                while (mysqlReader.Read())
                {
                    SumSave.crt_world = ReadDb.Read(mysqlReader, new user_world_vo());
                }
            }

        }
        /// <summary>
        /// 写入自身数据
        /// </summary>
        /// <param name="data"></param>
        public void Refresh_User_Setting(user_base_setting_vo data)
        {
            Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_setting, data.Set_Uptade_String(), data.Get_Update_Character());
        }
        /// <summary>
        /// 刷新属性
        /// </summary>
        private void refresh_Max_Hero_Attribute()
        {
            SumSave.crt_MaxHero_okd = new crtMaxHeroVO();
            crtMaxHeroVO crt = new crtMaxHeroVO();
            crt.Lv = SumSave.old_crt_hero.hero_Lv;
            crt.show_name= SumSave.old_crt_hero.hero_name;
            ////添加皮肤属性
            for (int i = 0; i < SumSave.old_db_heros.Count; i++)
            {
                if (SumSave.old_db_heros[i].hero_name == SumSave.old_crt_hero.hero_pos)
                {
                    crt.Type=SumSave.old_db_heros[i].hero_type;
                    switch (crt.Type)
                    {
                        case 2:crt.attack_distance = 1000;break;
                        default:
                            crt.attack_distance = 500;
                            break;
                    }
                    for (int j = 0; j < SumSave.old_db_heros[i].crate_value.Length; j++)
                    {
                        int value = SumSave.old_db_heros[i].crate_value[j] + (SumSave.old_db_heros[i].up_value[j] * (SumSave.old_crt_hero.hero_Lv / SumSave.old_db_heros[i].up_base_value[j]));
                        Enum_Value(crt, j, value,1);
                    }
                }
            }
            crt.life_types = Battle_Tool.Get_Life_Type();
            //装备加成 item1数量 item2最低判断表准（强化18 ） item3 品质7 item4 最低等级
            //  List<(int, int, int,int)> crt_euqips = new List<(int, int, int, int)>() { (1,18,0,100),(2,7,0,100)};
            (int, int, int, int) crt_euqip = (0, 18, 7, 100);
            Dictionary<int,int> suits= new Dictionary<int, int>();
            //添加装备效果
            List<Bag_Base_VO> crt_euqip_list = new List<Bag_Base_VO>();
            for (int i = 0; i < crt_euqip_list.Count; i++)
            {
                Bag_Base_VO data= crt_euqip_list[i];
                string[] info = data.user_value.Split(' ');
                int strengthenlv = int.Parse(info[1]);
                int quilty = int.Parse(info[2]);
                if (data.suit > 0)
                {
                    for (int s = 0; s < SumSave.db_Equip_Suits.Count; s++)
                    {
                        if (data.Name == SumSave.db_Equip_Suits[s].equip_name)
                        {
                            for (int j = 0; j < SumSave.db_Equip_Suits[s].equip_suit.Length; j++)
                            {
                                string[] value = SumSave.db_Equip_Suits[s].equip_suit[j].Split(' ');
                                string[] value2 = SumSave.db_Equip_Suits[s].equip_uplv[j].Split(' ');
                                //套装类型
                                enum_equip_show_list suit = (enum_equip_show_list)int.Parse(value[0]);
                                //加成值
                                int suit_value = (int.Parse(value[1]) + (strengthenlv / int.Parse(value2[0])) * (int.Parse(value2[1])));
                                if (crt.equip_suit_lists.ContainsKey(suit))
                                {
                                    crt.equip_suit_lists[suit] = Mathf.Max(crt.equip_suit_lists[suit], suit_value);
                                }else crt.equip_suit_lists.Add(suit, suit_value);
                            }
                        }
                    }
                    if (!suits.ContainsKey(data.suit)) suits.Add(data.suit, 0);
                    suits[data.suit]++;
                }
                switch ((EquipConfigTypeList)Enum.Parse(typeof(EquipConfigTypeList), data.StdMode))
                {
                    case EquipConfigTypeList.武器:
                    case EquipConfigTypeList.衣服:
                    case EquipConfigTypeList.头盔:
                    case EquipConfigTypeList.项链:
                    case EquipConfigTypeList.护臂:
                    case EquipConfigTypeList.戒指:
                    case EquipConfigTypeList.手镯:
                    case EquipConfigTypeList.扳指:
                    case EquipConfigTypeList.腰带:
                    case EquipConfigTypeList.靴子:
                    //case EquipConfigTypeList.护符:
                    //case EquipConfigTypeList.灵宝:
                    //case EquipConfigTypeList.勋章:
                    //case EquipConfigTypeList.饰品:
                    //case EquipConfigTypeList.玉佩:
                    //case EquipConfigTypeList.披风:
                        crt_euqip = (crt_euqip.Item1 + 1, (int)MathF.Min(crt_euqip.Item2, strengthenlv), (int)MathF.Min(crt_euqip.Item3, quilty), (int)MathF.Min(crt_euqip.Item4, data.equip_lv));

                        break;
                    default:
                        break;
                }
                crt.damageMin += data.damgemin;
                crt.damageMax += data.damagemax;
                crt.MagicdamageMin += data.magicmin;
                crt.MagicdamageMax += data.magicmax;
                crt.DefMin += data.defmin;
                crt.DefMax += data.defmax;
                crt.MagicDefMin += data.macdefmin;
                crt.MagicDefMax += data.macdefmax;
                crt.MaxHP += data.hp;
                crt.MaxMp += data.mp;
                if (data.damgemin > 0 || data.damagemax > 0)
                {
                    crt.damageMin += Obtain_Equip_strengthenlv_Value(data, strengthenlv);
                    crt.damageMax += Obtain_Equip_strengthenlv_Value(data, strengthenlv);
                }
                if (data.magicmin > 0 || data.magicmax > 0)
                {
                    crt.MagicdamageMin += Obtain_Equip_strengthenlv_Value(data, strengthenlv);
                    crt.MagicdamageMax += Obtain_Equip_strengthenlv_Value(data, strengthenlv);
                }
                if (data.defmin > 0 || data.defmax > 0)
                {
                    crt.DefMin += Obtain_Equip_strengthenlv_Value(data, strengthenlv, 2);
                    crt.DefMax += Obtain_Equip_strengthenlv_Value(data, strengthenlv, 2);
                }
                if (data.macdefmin > 0 || data.macdefmax > 0)
                {
                    crt.MagicDefMin += Obtain_Equip_strengthenlv_Value(data, strengthenlv, 2);
                    crt.MagicDefMax += Obtain_Equip_strengthenlv_Value(data, strengthenlv, 2);
                }
                if (info.Length > 4)
                {
                    //类型
                    string[] arr = info[3].Split(',');
                    //值
                    string[] arr_value = info[4].Split(',');
                    int index = 0;
                    for (int j = 0; j < arr.Length; j++)
                    {
                        ++index;
                        if (j == 0)
                        {
                            switch (int.Parse(arr[j]))
                            {
                                case 1: crt.damageMin += int.Parse(arr_value[j]);   break;
                                case 2:
                                    crt.damageMax += int.Parse(arr_value[j]);
                                    break;
                                case 3: crt.MagicdamageMin += int.Parse(arr_value[j]); break;
                                case 4: crt.MagicdamageMax += int.Parse(arr_value[j]); break;
                                case 5: crt.DefMax+= int.Parse(arr_value[j]); break;
                                case 6: crt.MagicDefMax += int.Parse(arr_value[j]); break;
                                case 7: crt.MaxHP += int.Parse(arr_value[j]); break;
                                case 8: crt.MaxMp += int.Parse(arr_value[j]); break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            int value= int.Parse(arr_value[j]);
                            Enum_Value(crt, int.Parse(arr[j]), value);
                        }
                    }
                }
            }
            //装备强化加成 
            if (crt_euqip.Item1 >= 10)
            {
                if (crt_euqip.Item2 >= 1)
                {
                    crt.double_damage += crt_euqip.Item2 * 5;
                }
                if (crt_euqip.Item3 >= 7)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Enum_Value(crt, 30 + i, crt_euqip.Item4);
                    }
                }
            
            }
            //套装加成
            if (suits.Count > 0)
            {
                foreach (var item in suits.Keys)
                {
                    db_suit_vo suit = SumSave.db_suits.Find(s => s.suit_type == item);
                    if (suit != null)
                    {
                        for (int i = 0; i < suit.suit_list.Count; i++)
                        {
                            if (suits[item] >= suit.suit_list[i].Item1)
                            {
                                Enum_Value(crt, suit.suit_list[i].Item2, suit.suit_list[i].Item3);
                            }
                        }
                    }
                   
                }
            }
            //添加成就效果
            Dictionary<string, long> achievements_lv= SumSave.crt_achievement.Set_Lv();
            foreach (var item in achievements_lv)
            {
                if(item.Value>=1)
                {
                    string achievementName = item.Key; // 获取成就名称

                    // 在 db_Achievement_dic 中查找匹配的成就对象
                    var matchedAchievement =SumSave.db_Achievement_dic.FirstOrDefault(achievement => achievement.achievement_value == achievementName);

                    if (matchedAchievement != null)
                    { 
                        // 找到匹配的成就对象，可以在这里处理
                        string[] vare= matchedAchievement.achievement_reward.Split("|");//拆解成就对应奖励
                        string[] var =vare[item.Value - 1].Split(" ");
                        if(int.Parse(var[0])==1)//此成就奖励为属性加成
                        {
                            Enum_Value(crt, int.Parse(var[1]),int.Parse(var[2]));
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"未找到成就: {achievementName}");
                    }
                }
            }
            //添加技能效果
            foreach (base_skill_vo skill in SumSave.oldcrt_skills)
            {
                if (skill.skill_open_type.Count > 0)
                {
                    for (int i = 0; i < skill.skill_open_type.Count; i++)
                    {
                        Enum_Value(crt, skill.skill_open_type[i], skill.skill_open_value[i] * int.Parse(skill.user_values[1]) / skill.skill_max_lv);//根据技能属性类型，加成属性
                    }
                }
                if (skill.skill_pos_type.Count > 0)
                {
                    for (int i = 0; i < skill.skill_pos_type.Count; i++)
                    {
                        if (int.Parse(skill.user_values[2]) > 0)
                        {
                            Enum_Value(crt, skill.skill_pos_type[i], skill.skill_pos_value[i] * int.Parse(skill.user_values[1]) / skill.skill_max_lv);
                        }
                    }
                }
            }
            //添加神器效果
            List < (string, int) > artifacts = SumSave.crt_artifact.Set();
            if (artifacts.Count > 0)
            {
                foreach (var item in artifacts)
                { 
                    db_artifact_vo data = ArrayHelper.Find(SumSave.db_Artifacts, e => e.arrifact_name == item.Item1);
                    if (data != null)
                    {
                        int strengthenlv = item.Item2;
                        string[] splits = data.arrifact_effects;
                        if (splits.Length >= 1)
                        {
                            foreach (var base_info in splits)
                            {
                                string[] infos = base_info.Split(' ');
                                //1类型 2每一级加成 3开启等级
                                if (infos.Length >= 3)
                                {
                                    if (strengthenlv >= int.Parse(infos[2]))
                                    { 
                                        Enum_Value(crt, int.Parse(infos[0]), int.Parse(infos[1]) * strengthenlv);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //上阵探索宠物属性
            List<db_pet_vo_old> pets = SumSave.crt_pet_Old.Set();

            if (pets.Count>=0)
            {
                for (int i = 0; i < pets.Count; i++)
                {
                    if (pets[i].pet_state == "1"|| pets[i].pet_state == "2")
                    {
                        //List<string> v = SumSave.db_pet_dic[SumSave.crt_pet_list[i].petName].crate_values;//宠物基础属性
                        //List<string> va = SumSave.db_pet_dic[SumSave.crt_pet_list[i].petName].up_values;//宠物成长属性
                        for (int j = 0; j < pets[i].crate_values.Count; j++)
                        {
                            if (pets[i].crate_values[j] != "" && pets[i].up_values[j] != "")
                            {
                                int value = (int.Parse(pets[i].crate_values[j]) + (int.Parse(pets[i].up_values[j]) * pets[i].level)) * (SumSave.crt_world.World_Lv / 10 + 5) / 100;
                                Enum_Value(crt, j, value);
                            }
                        }
                    }
                }
            }
            ///vip加成
            (int,int,string) exp = SumSave.crt_accumulatedrewards.SetRecharge();
            if (exp.Item1 > 0)
            {
                for (int i = SumSave.db_vip_list.Count - 1; i >= 0; i--)
                {
                    if (exp.Item1 == SumSave.db_vip_list[i].vip_lv)
                    {
                        Enum_Value(crt, (int)enum_skill_attribute_list.经验加成, SumSave.db_vip_list[i].experienceBonus);
                        Enum_Value(crt, (int)enum_skill_attribute_list.灵珠收益, SumSave.db_vip_list[i].lingzhuIncome);
                        Enum_Value(crt, (int)enum_skill_attribute_list.装备爆率, SumSave.db_vip_list[i].equipmentExplosionRate);
                        Enum_Value(crt, (int)enum_skill_attribute_list.人物历练, SumSave.db_vip_list[i].characterExperience);
                        Enum_Value(crt, (int)enum_skill_attribute_list.寻怪间隔, SumSave.db_vip_list[i].monsterHuntingInterval);
                        Enum_Value(crt, (int)enum_skill_attribute_list.生命回复, SumSave.db_vip_list[i].hpRecovery);
                        Enum_Value(crt, (int)enum_skill_attribute_list.法力回复, SumSave.db_vip_list[i].manaRegeneration);
                        Enum_Value(crt, (int)enum_skill_attribute_list.幸运, SumSave.db_vip_list[i].goodFortune);
                        Enum_Value(crt, (int)enum_skill_attribute_list.强化费用, SumSave.db_vip_list[i].strengthenCosts);
                        Enum_Value(crt, (int)enum_skill_attribute_list.离线间隔, SumSave.db_vip_list[i].offlineInterval);
                        Enum_Value(crt, (int)enum_skill_attribute_list.签到收益, SumSave.db_vip_list[i].signInIncome);
                        Enum_Value(crt, (int)enum_skill_attribute_list.鞭尸概率, SumSave.db_vip_list[i].whippingCorpses);
                        Enum_Value(crt, (int)enum_skill_attribute_list.灵气上限, SumSave.db_vip_list[i].upperLimitOfSpiritualEnergy);
                        break;
                    }
                }
            }
            //命运神殿属性
            if (SumSave.crt_needlist.fate_value_dic.Count > 0)
            {
                Dictionary<int, Dictionary<int, List<int>>> dic = SumSave.db_Accumulatedrewards.fate_dic;
                foreach (var item in SumSave.crt_needlist.fate_value_dic)
                {
                    int number = 0;
                    Dictionary<int, List<int>> fate = dic[item.Key];
                    foreach (var item2 in item.Value)
                    {
                        number += item2.Value;
                    }
                    foreach (var index in fate.Keys)
                    {
                        if (number >= fate[index][0])
                        {
                            Enum_Value(crt, fate[index][1], fate[index][2]);
                        }
                    }
                }
            }
            //收集属性
            for (int j = 0; j < suit_Type.GetNames(typeof(suit_Type)).Length; j++)//循环所有套装
            {
                List<db_collect_vo> suit = new List<db_collect_vo>();
                for (int z = 0; z < SumSave.db_collect_vo.Count; z++)//获得该套装所有装备
                {
                    if(SumSave.db_collect_vo[z].StdMode == suit_Type.GetNames(typeof(suit_Type))[j])//收集该套装装备
                    {
                        suit.Add(SumSave.db_collect_vo[z]);
                    }
                }
                int count = 0;//套装收集计数器

                for (int x = 0; x < suit.Count; x++)//循环该套装所有装备
                {
                    if(SumSave.crt_collect.user_collect_dic.ContainsKey(suit[x].Name))//是否有数据，没有就是没收集
                    {
                        if (SumSave.crt_collect.user_collect_dic[suit[x].Name] == 1)//判断是否收集
                        {
                            count++;
                            if(count== suit.Count)//装备收集完成
                            {
                                for(int y = 0; y < suit[x].bonuses_types.Length; y++)//加成属性可能有多个
                                {
                                    Enum_Value(crt,int.Parse(suit[x].bonuses_types[y]), int.Parse(suit[x].bonuses_values[y]));//添加属性
                                }
                               
                            }
                        }
                    }
                }
            }

            for (int j = 0; j < EquipTypeList.GetNames(typeof(EquipTypeList)).Length; j++)
            {
                for (int z = 0; z < SumSave.db_collect_vo.Count; z++)
                {
                    if (SumSave.db_collect_vo[z].StdMode == EquipTypeList.GetNames(typeof(EquipTypeList))[j])//判断是否为该类型装备
                    {
                        if (SumSave.crt_collect.user_collect_dic.ContainsKey(SumSave.db_collect_vo[z].Name))//是否有数据，没有就是没收集
                        {
                            if (SumSave.crt_collect.user_collect_dic[SumSave.db_collect_vo[z].Name] == 1)//判断是否收集
                            {
                                for (int y = 0; y < SumSave.db_collect_vo[z].bonuses_types.Length; y++)//加成属性可能有多个
                                {
                                    Enum_Value(crt, int.Parse(SumSave.db_collect_vo[z].bonuses_types[y]), int.Parse(SumSave.db_collect_vo[z].bonuses_values[y]));//添加属性
                                }
                            }
                        }
                    }
                }

            }

            if (SumSave.crt_player_buff.player_Buffs.Count > 0)
            {
                Enum_Value(crt, (int)enum_skill_attribute_list.经验加成, Battle_Tool.IsBuff(1));
                Enum_Value(crt, (int)enum_skill_attribute_list.人物历练, Battle_Tool.IsBuff(2));
               ///月卡加成
                Enum_Value(crt, (int)enum_skill_attribute_list.经验加成, Battle_Tool.IsBuff(3));
                Enum_Value(crt, (int)enum_skill_attribute_list.人物历练, Battle_Tool.IsBuff(3));
                ///至尊卡加成
                Enum_Value(crt, (int)enum_skill_attribute_list.经验加成, Battle_Tool.IsBuff(5));
                Enum_Value(crt, (int)enum_skill_attribute_list.灵珠收益, Battle_Tool.IsBuff(5));
            }
            if (Tool_State.IsState(State_List.至尊卡))
            {
                Enum_Value(crt, (int)enum_skill_attribute_list.幸运, 1);
            }
                //丹药属性
            List<(string, List<int>)> seeds = SumSave.crt_seeds.GetuseList();
            if (seeds.Count > 0)
            {
                foreach (var item in seeds)
                {
                    db_seed_vo data = ArrayHelper.Find(SumSave.db_seeds, e => e.pill == item.Item1);
                    if (data != null)
                    {
                        Enum_Value(crt, data.dicdictionary_index, item.Item2[1]);
                    }
                }
            }
            if (SumSave.crt_player_buff.player_Buffs.Count > 0)
            {
                foreach (var item in SumSave.crt_player_buff.player_Buffs)
                {
                    ///月卡属性
                    (DateTime, int, float, int) time = item.Value;
                    if (3 == time.Item4)
                    {
                        if ((SumSave.nowtime - time.Item1).Minutes < time.Item2)
                        {
                            Enum_Value(crt, 116, 5);
                        }
                    }
                    ///至尊卡属性
                    if(5== time.Item4)
                    {
                        if ((SumSave.nowtime - time.Item1).Minutes < time.Item2)
                        {
                            Enum_Value(crt, 506, 1);
                        }
                    }
                }
            }
            ///标准攻击速度
            crt.attack_speed= Obtain_battle_AttackSpeed(crt.attack_speed, 100);
            //皮肤
#if UNITY_EDITOR  
            //crt.hit += 1000;
            //crt.attack_speed = 210;
            //crt.damageMax += 10000000;
            //crt.MagicdamageMax += 10000000;
#elif UNITY_ANDROID
#elif UNITY_IPHONE
#endif
            SumSave.crt_MaxHero_okd = crt;
            SumSave.crt_MaxHero_okd.Init();

            Battle_Tool.validate_rank();
        }
        /// <summary>
        /// 计算攻击速度递减公式
        /// </summary>
        /// <param name="crt_speed"></param>
        /// <param name="base_speed"></param>
        /// <returns></returns>
        private static int Obtain_battle_AttackSpeed(int crt_speed, int base_speed)
        {
            if (crt_speed >= base_speed) return crt_speed;
            int max = base_speed - crt_speed;
            int base_value = base_speed;
            int base_lv = 1;
            for (int i = 0; i < max; i++)
            {
                if (base_value >= 90 && base_value < 100) base_lv = 2;
                if (base_value >= 80 && base_value < 90) base_lv = 3;
                if (base_value >= 70 && base_value < 80) base_lv = 4;
                if (base_value >= 60 && base_value < 70) base_lv = 5;
                if (base_value < 60) base_lv += 10;
                if (i % base_lv == 0) base_value--;
            }
            base_value = (int)MathF.Max(50, base_value);
            return base_value;
        }


        /// <summary>
        /// 加成属性
        /// </summary>
        /// <param name="crt">主体</param>
        /// <param name="index">编号</param>
        /// <param name="value">值</param>
        private void Enum_Value(crtMaxHeroVO crt,int index,int value,int dic=-1) 
        {
            while (index >= crt.bufflist.Count)
            { 
                crt.bufflist.Add(0);
            }
            crt.bufflist[index] += value;
            switch ((enum_skill_attribute_list)index)
            {
                case enum_skill_attribute_list.生命值:
                    crt.MaxHP += value;
                    break;
                case enum_skill_attribute_list.法力值:
                    crt.MaxMp += value;
                    break;
                case enum_skill_attribute_list.内力值:
                    crt.internalforceMP += value;
                    break;
                case enum_skill_attribute_list.蓄力值:
                    crt.EnergyMp += value;
                    break;
                case enum_skill_attribute_list.物理防御:
                    crt.DefMax += value;
                    break;
                case enum_skill_attribute_list.魔法防御:
                    crt.MagicDefMax += value;
                    break;
                case enum_skill_attribute_list.物理攻击:
                    crt.damageMax += value;
                    break;
                case enum_skill_attribute_list.魔法攻击:
                    crt.MagicdamageMax += value;
                    break;
                case enum_skill_attribute_list.命中:
                    crt.hit += value;
                    break;
                case enum_skill_attribute_list.躲避:
                    crt.dodge += value;
                    break;
                case enum_skill_attribute_list.穿透:
                    crt.penetrate += value;
                    break;
                case enum_skill_attribute_list.格挡:
                    crt.block += value;
                    break;
                case enum_skill_attribute_list.暴击:
                    crt.crit_rate += value;
                    break;
                case enum_skill_attribute_list.幸运:
                    crt.Lucky += value;
                    break;
                case enum_skill_attribute_list.暴击伤害:
                    crt.crit_damage += value;
                    break;
                case enum_skill_attribute_list.伤害加成:
                    crt.double_damage += value;
                    break;
                case enum_skill_attribute_list.真实伤害:
                    crt.Real_harm += value;
                    break;
                case enum_skill_attribute_list.伤害减免:
                    crt.Damage_Reduction += value;
                    break;
                case enum_skill_attribute_list.伤害吸收:
                    crt.Damage_absorption += value;
                    break;
                case enum_skill_attribute_list.异常抗性:
                    crt.resistance += value;
                    break;
                case enum_skill_attribute_list.攻击速度:
                    crt.attack_speed += (value * dic);
                    break;
                case enum_skill_attribute_list.移动速度:
                    crt.move_speed += value;
                    break;
                case enum_skill_attribute_list.生命加成:
                    crt.bonus_Hp += value;
                    break;
                case enum_skill_attribute_list.法力加成:
                    crt.bonus_Mp += value;
                    break;
                case enum_skill_attribute_list.生命回复:
                    crt.Heal_Hp += value;
                    break;
                case enum_skill_attribute_list.法力回复:
                    crt.Heal_Mp += value;
                    break;
                case enum_skill_attribute_list.物攻加成:
                    crt.bonus_Damage += value;
                    break;
                case enum_skill_attribute_list.魔攻加成:
                    crt.bonus_MagicDamage += value;
                    break;
                case enum_skill_attribute_list.物防加成:
                    crt.bonus_Def += value;
                    break;
                case enum_skill_attribute_list.魔防加成:
                    crt.bonus_MagicDef += value;
                    break;
                case enum_skill_attribute_list.土属性强化:
                case enum_skill_attribute_list.火属性强化:
                case enum_skill_attribute_list.水属性强化:
                case enum_skill_attribute_list.金属性强化:
                case enum_skill_attribute_list.木属性强化:
                    crt.life[index - 30] += value;
                    break;
                case enum_skill_attribute_list.经验加成:
                    break;
                case enum_skill_attribute_list.装备掉落:
                    break;
                case enum_skill_attribute_list.极品宠物掉落:
                    break;
                case enum_skill_attribute_list.人物历练:
                    break;
                case enum_skill_attribute_list.宠物经验:
                    break;
                case enum_skill_attribute_list.内功经验:
                    break;
                case enum_skill_attribute_list.灵珠收益:
                    break;
                case enum_skill_attribute_list.装备爆率:
                    break;
                case enum_skill_attribute_list.宠物获取:
                    break;
                case enum_skill_attribute_list.云游商人折扣:
                    break;
                case enum_skill_attribute_list.祈愿收益:
                    break;
                case enum_skill_attribute_list.奇遇任务收益:
                    break;
                case enum_skill_attribute_list.游历危险躲避率:
                    break;
                case enum_skill_attribute_list.游历双倍获得率:
                    break;
                case enum_skill_attribute_list.游历时长:
                    break;
                case enum_skill_attribute_list.游历龙珠收益:
                    break;
                case enum_skill_attribute_list.寻怪间隔:
                    break;
                case enum_skill_attribute_list.宠物容量:
                    break;
                case enum_skill_attribute_list.土:
                    break;
                case enum_skill_attribute_list.火:
                    break;
                case enum_skill_attribute_list.水:
                    break;
                case enum_skill_attribute_list.木:
                    break;
                case enum_skill_attribute_list.金:
                    break;
                case enum_skill_attribute_list.五行伤害:
                    break;
                case enum_skill_attribute_list.五行伤害减少:
                    break;
                case enum_skill_attribute_list.灵力:
                    break;
                case enum_skill_attribute_list.体魄:
                    break;
                case enum_skill_attribute_list.神识:
                    break;
                case enum_skill_attribute_list.宠物攻击:
                    break;
                case enum_skill_attribute_list.宠物防御:
                    break;
                case enum_skill_attribute_list.宠物生命:
                    break;
                case enum_skill_attribute_list.宠物暴击:
                    break;
                case enum_skill_attribute_list.宠物暴击伤害:
                    break;
                case enum_skill_attribute_list.宠物暴击率:
                    break;
                case enum_skill_attribute_list.宠物攻击速度:
                    break;
                case enum_skill_attribute_list.技能伤害:
                    break;
                case enum_skill_attribute_list.燃血:
                    break;
                case enum_skill_attribute_list.灵身:
                    break;
                case enum_skill_attribute_list.连击:
                    break;
                case enum_skill_attribute_list.受到减免伤害:
                    break;
                case enum_skill_attribute_list.复活次数:
                    break;
                case enum_skill_attribute_list.幸运一击的概率:
                    break;
                case enum_skill_attribute_list.幸运一击的伤害:
                    break;
                case enum_skill_attribute_list.攻击时概率抵消伤害:
                    break;
                case enum_skill_attribute_list.被攻击时反击真实伤害:
                    break;
                case enum_skill_attribute_list.每次攻击增加伤害:
                    break;
                case enum_skill_attribute_list.治疗术效果:
                    break;
                case enum_skill_attribute_list.施毒术效果:
                    break;
                case enum_skill_attribute_list.青云门技能伤害:
                    break;
                case enum_skill_attribute_list.魔法盾效果:
                    break;
                case enum_skill_attribute_list.血刀刀法伤害:
                    break;
                default:
                    break;
            }

        }
        /// <summary>
        /// 强化属性加成
        /// </summary>
        /// <param name="data"></param>
        /// <param name="lv"></param>
        /// <returns></returns>
        private int Obtain_Equip_strengthenlv_Value(Bag_Base_VO data,int lv,float coefficient=1)
        {
            return (int)((data.need_lv * lv) / coefficient);
        }

        ///
        /// <summary>
        /// 
        /// </summary>
        public void Refresh_Max_Hero_Attribute()
        {
            refresh_Max_Hero_Attribute();
            //刷新战斗
            Game_Omphalos.Refresh();
        }

        /// <summary>
        /// 读取资源数据
        /// </summary>
        private void Read_User_Resources()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.mo_user_value, "uid", GetStr(SumSave.crt_user.uid));//读取角色信息
            SumSave.crt_resources = new user_base_Resources_vo();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.crt_resources = ReadDb.Read(mysqlReader, new user_base_Resources_vo());
                }
            }
            else
            {
                SumSave.crt_resources.now_time = DateTime.Now;
                SumSave.crt_resources.user_map_index = SumSave.read_lose_map[0].map_name;
                SumSave.crt_resources.skill_value = "";
                SumSave.crt_resources.house_value = "";
                SumSave.crt_resources.bag_value = "";
                SumSave.crt_resources.material_value = "";
                SumSave.crt_resources.equip_value = "";
                SumSave.crt_resources.pages = new int[] { 120, 60, 0, 0, 0};
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.mo_user_value, SumSave.crt_resources.Set_Instace_String());
            }
            Init();
        }

        /// <summary>
        /// 读取英雄数据
        /// </summary>
        private void Read_User_Hero()
        {
            mysqlReader = MysqlDb.Select(Mysql_Table_Name.mo_user_hero, "uid", GetStr(SumSave.crt_user.uid));
            SumSave.old_crt_hero = new Hero_VO();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.old_crt_hero = ReadDb.Read(mysqlReader, new Hero_VO());
                }
            }
            else
            {
                SumSave.old_crt_hero.hero_name = GetNameHelper.GetManName();
                SumSave.old_crt_hero.hero_value = SumSave.old_db_heros[0].hero_name;
                SumSave.old_crt_hero.hero_lv = "1";
                SumSave.old_crt_hero.hero_exp = "0";
                SumSave.old_crt_hero.hero_Lv = 1;
                SumSave.old_crt_hero.hero_Exp = 0;
                SumSave.old_crt_hero.hero_pos = SumSave.old_db_heros[0].hero_name;
                SumSave.old_crt_hero.hero_material_list = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
                SumSave.old_crt_hero.tianming_Platform = new int[5];
                SumSave.old_crt_hero.InitTianming_Platform();
                Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.mo_user_hero, SumSave.old_crt_hero.Set_Instace_String());
                //MysqlDb.InsertInto(Mysql_Table_Name.mo_user_hero, SumSave.crt_hero.Set_Instace_String());
            }
        }



        /// <summary>
        /// 导入数据
        /// </summary>
        public void Execute_Write(List<Base_Wirte_VO> list)
        {

            OpenMySqlDB();
            //写入数据
            ExecuteWrite(list);

            CloseMySqlDB();
        }
        public void Read_Locality()
        {

        }
    }
}