using Common;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MVC
{
    /// <summary>
    ///  处理用户相关数据:登录,注册和注销
    /// </summary>
    public class MySql_Proxy : Base_Proxy
    {
        /// <summary>
        /// NAME
        /// </summary>
        public new const string NAME = "MySql_Proxy";

        public MySql_Proxy()
        {
            this.ProxyName = NAME;
        }

        public void Read_Instace()
        {
            OpenMySqlDB();
            if (MysqlDb.MysqlClose) return;//未联网
            QueryTime();
            QueryVersion();
            Read_db_par();
            Read_Db_Dec();
            Read_Db_Hall();
            Read_Db_Map();
            Read_Db_Hero();
            Read_Db_Player_TalentType();
            Read_Db_Player_Talent();
            Read_Db_stditems();
            Read_Db_Suit();
            Read_Db_Store();
            Read_Db_Magic();
            Read_Db_Monster();
            Read_Db_Lv();
            Read_Db_Pet();
            Read_Db_Pet_Talent();
            Read_Db_Synthesis();
            Read_Db_Illustrated();
            Read_Db_Chronicle();
            Read_db_vip();
            Read_Db_Setting_Aoption();
            //Read_Db_Magic();
            //
            //Read_Db_Hero();
            //Read_Db_Setting_Aoption();
            //Read_Db_artifact();
            //Read_Db_Pass();
            //Read_Db_Panlt();
            //
            //
            //Read_Db_Pet_explore();
            //
            //
            //Read_Db_Achievement();
            //Read_Db_Store();
            //Read_Db_Seed();
            //Read_Db_Collect();
            //Read_db_signin();
            //Read_db_par();
            //Read_Guide_TotalTask();
            //Read_db_Accumulatedrewards();
            //Read_Guide_Fate();
            //
            //Read_db_formula();
            //Read_db_strengthen_needlist();
            //Read_db_weather();
            //
            //Read_db_Equip_Suits();

            //ReadDb_Endless();
            CloseMySqlDB();
        }
        /// <summary>
        /// 读取vip列表
        /// </summary>
        private void Read_db_vip()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_vip);
            SumSave.db_vip_list = new List<db_vip>();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_vip_list.Add(ReadDb.Read_Vip(mysqlReader));
                }
            }
        }

        /// <summary>
        /// 大事记
        /// </summary>
        private void Read_Db_Chronicle()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_chronicle);
            SumSave.global_Chronicle = new List<(int, string)>();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.global_Chronicle.Add(Mysql_Read.Read_Chronicle(mysqlReader));
                }
            }
        }
        /// <summary>
        /// 图鉴
        /// </summary>
        private void Read_Db_Illustrated()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_illustrated);
            List<db_illustrated_vo> db_illustrateds = new List<db_illustrated_vo>();
            SumSave.db_illustrateds = new Dictionary<int, List<db_illustrated_vo>>();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                   db_illustrateds.Add(Mysql_Read.Read_illustrated(mysqlReader));
                }
            }
            Dictionary<int, List<db_illustrated_vo>> dic = new Dictionary<int, List<db_illustrated_vo>>();
            for (int i = 0; i < db_illustrateds.Count; i++)
            { 
                if(!dic.ContainsKey(db_illustrateds[i].Illustrated_type))  
                    dic.Add(db_illustrateds[i].Illustrated_type, new List<db_illustrated_vo>());
                dic[db_illustrateds[i].Illustrated_type].Add(db_illustrateds[i]);
            }
            SumSave.db_illustrateds = dic;
        }
        private void Read_Db_Synthesis()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_synthesiss); //db_synthesis
            //mysqlReader = MysqlDb.ReadGetLatest(Mysql_Table_Name.db_synthesis,0,30);
            SumSave.db_synthesis = new List<db_synthesis_vo>();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_synthesis.Add(Mysql_Read.Read_Synthesis(mysqlReader));
                }
            }
        }
        /// <summary>
        /// 读取宠物天赋数据库
        /// </summary>
        private void Read_Db_Pet_Talent()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_pet_talent);

            SumSave.db_pet_talents = new List<db_pet_talent_vo>();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_pet_talents.Add(Mysql_Read.Read_Pet_Talent(mysqlReader));
                }
            }
        }
        private void Read_Db_Player_TalentType()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_player_talent_type);

            SumSave.db_player_talent_types = new List<db_player_talent_type>();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_player_talent_types.Add(Mysql_Read.Read_Player_Talent_Type(mysqlReader));
                }
            }
        }
        private void Read_Db_Player_Talent()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_player_talent);

            SumSave.db_player_talents = new List<db_player_talent_vo>();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_player_talents.Add(Mysql_Read.Read_Player_Talent(mysqlReader));
                }
            }
        }
        /// <summary>
        /// 读取宠物表
        /// </summary>
        private void Read_Db_Pet()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_pet);

            SumSave.db_pets = new List<db_pet_vo>();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_pets.Add(Mysql_Read.Read_Pet(mysqlReader));
                }
            }
            
        }
        /// <summary>
        /// 获得天气
        /// </summary>
        public void Read_db_weather()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_weather);
            SumSave.db_weather_list = new List<db_weather>();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_weather_list.Add(ReadDb.Read_weather(mysqlReader));
                }
            }
        }
     
        /// <summary>
        /// 读取造化炉合成列表
        /// </summary>
        public void Read_db_formula()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_formula);
            SumSave.db_formula_list = new List<db_formula_vo>();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_formula_list.Add(ReadDb.Read_formula(mysqlReader));
                }
            }
        }




        /// <summary>
        /// 累计奖励
        /// </summary>
       
        /// <summary>
        /// 读取命运殿堂列表
        /// </summary>
        public void Read_Guide_Fate()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_fate);
            SumSave.db_fate_list = new List<db_fate_vo>();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_fate_list.Add(ReadDb.Read_fate(mysqlReader));
                }
            }
        }
        /// <summary>
        /// 读取大世界列表
        /// </summary>
        public void Read_Guide_TotalTask()
        {
           
        }
        /// <summary>
        /// 读取服务器
        /// </summary>
        public void Read_db_par()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_pars);

            SumSave.db_pars = new List<db_base_par>();

            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_pars.Add(Read_Mysql.Read_base_par(mysqlReader));

                }
            }
        }
        /// <summary>
        /// 读取签到信息
        /// </summary>
        private void Read_db_signin()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_signin);

            SumSave.db_Signins = new List<db_signin_vo>();

            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_Signins.Add(ReadDb.Read_signin(mysqlReader));
                }
            }
        }

        /// <summary>
        /// 获取收集信息
        /// </summary>
        private void Read_Db_Collect()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_collect);

            SumSave.db_collect_vo = new List<db_collect_vo>();

            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_collect_vo.Add(ReadDb.Read_collect(mysqlReader));
                }
            }
        }


        /// <summary>
        /// 商店数据库
        /// </summary>
        private void Read_Db_Store()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_store);

            SumSave.db_stores_list = new List<db_store_vo>();

            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_stores_list.Add(Mysql_Read.Read(mysqlReader));
                }
            }
           
        }




        /// <summary>
        /// 成就数据库
        /// </summary>
        private void Read_Db_Achievement()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_achieve);

            SumSave.db_Achievement_dic= new List<db_achievement_VO>();

            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_Achievement_dic.Add(ReadDb.Read_achievement_VO(mysqlReader));
                }
            }
        }


        /// <summary>
        /// 获取炼丹信息
        /// </summary>
        private void Read_Db_Seed()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_seed);

            SumSave.db_seeds = new List<db_seed_vo>();

            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_seeds.Add(ReadDb.Read_seed(mysqlReader));
                }
            }
        }
        /// <summary>
        /// 大厅按钮
        /// </summary>
        private void Read_Db_Hall()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_hall);

            SumSave.db_halls = new db_hall_vo();

            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_halls = (ReadDb.Read(mysqlReader, new db_hall_vo()));
                }
            }
        }

        /// <summary>
        /// 升级经验
        /// </summary>
        private void Read_Db_Lv()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_lv);
            SumSave.db_lvs = new Dictionary<int, db_lv_vo>();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    db_lv_vo item = ReadDb.Read_lv(mysqlReader);
                    if(!SumSave.db_lvs.ContainsKey(item.lv))
                        SumSave.db_lvs.Add(item.lv, item);
                }
            }
        }
        /// <summary>
        /// 具体功能消息
        /// </summary>
        private void Read_Db_Dec()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_dec);

            SumSave.db_dec = new List<db_dec>();

            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_dec.Add(Read_Mysql.Read_dec(mysqlReader));
                }
            }
        }

        private void Read_db_Equip_Suits()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_equip_suit);

           

        }

        /// <summary>
        /// 套装
        /// </summary>
        private void Read_Db_Suit()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_suit);

            SumSave.db_suits = new List<db_suit_vo>();

            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_suits.Add (Mysql_Read.Read_suit(mysqlReader));
                }
            }
        }

        /// <summary>
        /// 读取宠物探索地图
        /// </summary>
        private void Read_Db_Pet_explore()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_pet_explore);

          

        }



        /// <summary>
        /// 读取植物数据库
        /// </summary>
        private void Read_Db_Panlt()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_plant);

           

        }
        /// <summary>
        /// 读取通行证数据库
        /// </summary>
        private void Read_Db_Pass()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_pass);

            SumSave.db_pass = new List<user_pass_vo>();

            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_pass.Add(ReadDb.Read_Pass(mysqlReader, new user_pass_vo()));
                }
            }
        }

        /// <summary>
        /// 读取神器列表
        /// </summary>
        private void Read_Db_artifact()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_artifact);

            SumSave.db_Artifacts = new List<db_artifact_vo>();

            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_Artifacts.Add(ReadDb.Read_artifact_vo(mysqlReader));
                }
            }
        }

        /// <summary>
        /// 读取设置选项
        /// </summary>
        private void Read_Db_Setting_Aoption()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_setting);

            SumSave.db_sttings = new List<db_setting_vo>();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_sttings.Add(ReadDb.Read(mysqlReader, new db_setting_vo()));
                }
            }
        }
        /// <summary>
        /// 读取英雄数据库
        /// </summary>
        private void Read_Db_Hero()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_hero_type);
            SumSave.db_heros = new List<db_Hero_VO>();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_heros.Add(Read_Mysql.Read_Hero(mysqlReader));
                }
            }
        }

        /// <summary>
        /// 读取物品数据库
        /// </summary>
        private void Read_Db_stditems()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_stditems);
            SumSave.db_stditems = new List<Bag_Base_VO>();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_stditems.Add(Read_Mysql.Read_stditems(mysqlReader));
                }
            }
#if UNITY_EDITOR
            Battle_Tool.tool_item();
#elif UNITY_ANDROID

#elif UNITY_IPHONE
            
#endif
        }
        /// <summary>
        /// 读取技能数据库
        /// </summary>
        private void Read_Db_Magic()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_magic);
            SumSave.db_skills = new List<db_skill_vo>();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_skills.Add(Mysql_Read.ReadSkill(mysqlReader));
                }
            }
        }
        /// <summary>
        /// 读取怪物数据库
        /// </summary>
        private void Read_Db_Monster()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_monster);
            SumSave.db_monsters = new List<crtMaxBattleVO>();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_monsters.Add(Mysql_Read.Read_Monster(mysqlReader));
                }
            }
        }
        /// <summary>
        /// 读取地图数据库
        /// </summary>
        private void Read_Db_Map()
        {
            mysqlReader = MysqlDb.ReadFullTable(Mysql_Table_Name.db_map);
            SumSave.db_maps = new List<db_map_vo>();
            if (mysqlReader.HasRows)
            {
                while (mysqlReader.Read())
                {
                    SumSave.db_maps.Add(Read_Mysql.Read(mysqlReader));
                }
            }
            //ArrayHelper.Ascending(SumSave.db_maps, e => e.map_lv);
            //Battle_Tool.tool_map();
            Tool_Battle.Carte_Read_Boss_Time();
            //Carte_Read_Boss_Time();
        }
        /// <summary>
        /// 创建地图开启时间
        /// </summary>
        private void Carte_Read_Boss_Time()
        {
            Dictionary<string,Dictionary<int, DateTime>> dic = new Dictionary<string, Dictionary<int, DateTime>>();
            DateTime now = SumSave.nowtime >= DateTime.Now ? SumSave.nowtime : DateTime.Now;
            for (int i = 0; i < SumSave.db_maps.Count; i++)
            {
                if (SumSave.db_maps[i].map_type == 0)
                {
                    for (int j = 0; j < SumSave.db_maps[i].map_boss.Count; j++)
                    {
                        if (!dic.ContainsKey(SumSave.db_maps[i].map_boss[j]))
                        {
                            dic.Add(SumSave.db_maps[i].map_boss[j], new Dictionary<int, DateTime>());
                        }
                        dic[SumSave.db_maps[i].map_boss[j]].Add(SumSave.db_maps[i].map_boss_cdtime[j], now);
                    }
                }
            }
        }
    }
}
