using Common;
using Components;
using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Analytics;

namespace MVC
{
    /// <summary>
    /// 控制定时器
    /// </summary>
    public class Game_Omphalos : Base_Mono
    {
        public static Game_Omphalos i;
        private void Awake()
        {
            i= this;
            ///登录时禁止unity收集信息
            Analytics.enabled = false;//禁用Unity自带的Analytics服务，使其不会在收集用户数据，保护用户隐私
            Analytics.deviceStatsEnabled = false;//禁用了Unity收集用户设备的性能数据，比如CPU型号，内存使用情况等，保护用户隐私
            Analytics.initializeOnStartup = false;//禁止Unity自动初始化，完全自主控制Unity收集用户数据的行为
            Analytics.limitUserTracking = false;//禁止Unity对用户的追踪，保护用户隐私，一般这个选项是为了遵守隐私政策
            PerformanceReporting.enabled = false;//禁止Unity收集应用程序性能数据的报告，比如崩溃报告，性能下降等
            AppFacade.I.Startup();
            panelBattle = UI_Manager.I.GetPanel<PanelBattle>();
            panelMian = UI_Manager.I.GetPanel<PanelMian>();
        }
        private List<Base_Wirte_VO> wirtes = new List<Base_Wirte_VO>();

        private static PanelBattle panelBattle;

        private static PanelMian panelMian;
        /// <summary>
        /// 屏保开关显示时间
        /// </summary>
        private int show_Screensaver_time_state = 0;
        /// <summary>
        /// 开启定时器 
        /// </summary>
        public void activation()
        {
            InvokeRepeating("CountTime", 1, 1);
            InvokeRepeating("Read_User_Ranks", 600, 600);

        }
         
        public void Show_Screensaver()
        {
            show_Screensaver_time_state = 0;
        }
        /// <summary>
        /// 刷新指令
        /// </summary>
        /// <param name="dream_user_bag"></param>
        public static void Refresh(Mysql_Table_Name index= Mysql_Table_Name.mo_user_hero)
        {
            switch (index)
            {
                case Mysql_Table_Name.mo_user_hero:
                case Mysql_Table_Name.dream_user_bag://刷新背包数据
                case Mysql_Table_Name.dream_user_pet:
                    if (panelBattle.gameObject.activeInHierarchy) panelBattle.Refresh();
                    break;
                case Mysql_Table_Name.Dream_Users:
                case Mysql_Table_Name.dream_user_gift:
                    if (panelMian.gameObject.activeInHierarchy) panelMian.Show_unit();
                    break;
            }
        }
        /// <summary>
        /// 设置全局数据
        /// </summary>
        /// <param name="info"></param>
        /// <param name="bag"></param>
        public static void global_battle_info(string info, Bag_Base_VO bag = null)
        {
            global_battle_info_VO vo = new global_battle_info_VO();
            vo.SetData(Tool_UI.ToStandardFormat(SumSave.nowtime) + " " + SumSave.crtHero.hero_name + " " + info, bag);
            SumSave.global_battle_info.Add(vo);
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="dec"></param>
        public void Alert_Info(string dec)
        {
            Alert.Show("异常提示", dec);
        }
        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="dec"></param>
        public void Alert_Show(string dec)
        {
            Alert_Dec.Show(dec);
        }
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="log"></param>
        public void LogList(string log)
        {
            //写入日志
            SendNotification(NotiList.loglist, log);
        }
        /// <summary>

        /// 时间计数器
        /// </summary>
        private int performTime=0;
        /// <summary>
        /// 写入数据 每10s写入一次
        /// </summary>
        private void CountTime()
        {
            opentime();
            performTime+=1;
            if (SumSave.crt_setting.user_data_settings.Count >= 10 && SumSave.crt_setting.user_data_settings[9] == 1)
             {
                show_Screensaver_time_state++;
                if (show_Screensaver_time_state >= 300)
                {
                    Alert_Screensaver.show_Screensaver();
                }
            }
            monitor_plant();
            ////显示屏幕保护
            //if (SumSave.data_settings.base_settings[8]==0)
            //{
            //    show_Screensaver_time_state++;
            //    //显示屏保
            //    if (show_Screensaver_time_state >= 300)
            //    {
            //        show_Screensaver_time_state = 0;
            //    } 
            //}
            if (performTime>=600)
            {
                performTime = 0;
                Read_User_Ranks();
            }
            panelMian.Show_Golbal_Info_list();
        }
     
        /// <summary>
        /// 每s刷新一次
        /// </summary>
        private void opentime()
        {
            if (SumSave.nowtime >= SumSave.local_time) SumSave.local_time = SumSave.nowtime;
            SumSave.local_time = SumSave.local_time.AddSeconds(1);
            SumSave.nowtime = SumSave.nowtime.AddSeconds(1);
            //archive();

        }
        /// <summary>
        /// 存档
        /// </summary>
        public void archive()
        {
            //Alert_Dec.Show("存档中");
            SendNotification(NotiList.Mysql_close);
            SendNotification(NotiList.Execute_Write, wirtes);
        }
        /// <summary>
        /// 监控收益
        /// </summary>
        private void monitor_plant()
        {

        }

        /// <summary>
        /// 每10分钟刷新一次排行榜
        /// </summary>
        private void Read_User_Ranks()
        {
            //定时存档数据
            archive();
            //每日任务 在线时长
            //Battle_Tool.validate_rank();
            //Tool_State.self_inspection();//10分钟验证一次状态
        }
        public void Delete(string dec)
        { 
          SendNotification(NotiList.Account, SumSave.nowtime+" "+SumSave.crt_user.uid+" "+  dec);
        }

        /// <summary>
        /// 查询队列
        /// </summary>
        /// <param name="type">函数公式</param>
        /// <param name="tableName">调用列表</param>
        /// <param name="sql">写入值</param>
        /// /// <param name="sql_names">序列名</param>
        public void GetQueue(Mysql_Type type, Mysql_Table_Name tableName, string[] sql, string[] sql_names = null, string selectkey="uid", string selectvalue="")
        {
            foreach (var item in wirtes)
            {
                if (item.type == type)
                {
                    if (item.tableName == tableName && type != Mysql_Type.InsertInto)
                    {
                        //执行合并
                        item.columnValues = sql;
                        item.exist = true;
                        return;
                    }
                }
            }
            //获取新列表
            Base_Wirte_VO vo = new Base_Wirte_VO();
            vo.type = type;
            vo.tableName = tableName;
            vo.columnNames = sql_names;
            vo.columnValues = sql;
            if (type != Mysql_Type.InsertInto)
            {
                vo.selectkey = selectkey;
                vo.selectvalue = selectvalue == "" ? SumSave.uid : selectvalue;
            }
            vo.exist = true;
            wirtes.Add(vo);
        }
        /// <summary>
        /// 主账户
        /// </summary>
        private string[] Accout;
        /// <summary>
        /// 写入角色id
        /// </summary>
        /// <param name="vs"></param>
        public void Crate_Accout(string[] vs)
        {
            Accout= vs;
        }
        /// <summary>
        /// 写入tap账户
        /// </summary>
        public void Wirte_Tap()
        {
            SendNotification(NotiList.Read_Crate_Uid, Accout);
        }
        /// <summary>
        /// 写入苹果账户
        /// </summary>
        public void Wirte_Iphone()
        {
            SendNotification(NotiList.Read_Crate_IPhone_Uid, Accout);
        }

        /// <summary>
        /// 执行写入队列
        /// </summary>
        public void SetWrite(Mysql_Type type, Mysql_Table_Name tableName, string[] sql)
        {
            for (int i = 0; i < wirtes.Count; i++)
            {
                if (wirtes[i].type == type)
                {
                    if (wirtes[i].tableName == tableName)
                    {
                        wirtes[i].columnValues = sql;
                        wirtes[i].exist = true;
                        return;
                    }
                }
            }

        }
        /// <summary>
        /// 调用写入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="all_list"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public string Instance_Wirte<T>(List<T> all_list, int max = 999) where T : Base_VO
        {
            string value = "";

            foreach (T item in all_list)
            {
                if (max > 0)
                {
                    max--;
                    value += item.GetPropertyValue(item);

                    value += ';';
                }
                else return value;
            }
            return value;
        }

        /// <summary>
        /// 写入资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <param name="all_list"></param>
        public void Wirte_ResourcesList<T>(Emun_Resources_List index, List<T> all_list) where T : Base_VO
        {
            string value= OnWirte(all_list);
            switch (index)
            {
                case Emun_Resources_List.skill_value:
                    SumSave.crt_resources.skill_value = value;
                    break;
                case Emun_Resources_List.bag_value:
                    SumSave.crt_resources.bag_value = value;
                    break;
                case Emun_Resources_List.equip_value:
                    SumSave.crt_resources.equip_value = value;
                    break;
                case Emun_Resources_List.material_value:
                    SumSave.crt_resources.material_value = value;
                    break;
                case Emun_Resources_List.house_value:
                    SumSave.crt_resources.house_value = value;
                    break;
            }
            GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_value, SumSave.crt_resources.Set_Uptade_String(), SumSave.crt_resources.Get_Update_Character());
        }

        /// <summary>
        /// 写入材料资源
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Wirte_ResourcesList(Emun_Resources_List index,string value) 
        {
            switch (index)
            {
                case Emun_Resources_List.skill_value:
                    SumSave.crt_resources.skill_value = value;
                    break;
                case Emun_Resources_List.bag_value:
                    SumSave.crt_resources.bag_value = value;
                    break;
                case Emun_Resources_List.equip_value:
                    SumSave.crt_resources.equip_value = value;
                    break;
                case Emun_Resources_List.material_value:
                    SumSave.crt_resources.material_value = value;
                    break;
            }
            GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_value, SumSave.crt_resources.Set_Uptade_String(), SumSave.crt_resources.Get_Update_Character());
        }


        /// <summary>
        /// 写入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="all_list"></param>
        /// <returns></returns>
        private string OnWirte<T>(List<T> all_list) where T : Base_VO
        {
            string value = "";

            foreach (T item in all_list)
            {
                value += item.GetPropertyValue(item);

                value += ';';
            }
            return value;

        }
        /// <summary>
        /// 立刻刷新
        /// </summary>
        /// <param name="user_rank"></param>
        public  void immediately(Mysql_Table_Name user_rank)
        {
            //Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.user_rank, SumSave.user_ranks.Set_Uptade_String(), SumSave.user_ranks.Get_Update_Character());
            switch (user_rank)
            {
                case Mysql_Table_Name.db_monster:
                    break;
                case Mysql_Table_Name.db_stditems:
                    break;
                case Mysql_Table_Name.db_magic:
                    break;
                case Mysql_Table_Name.db_maps:
                    break;
                case Mysql_Table_Name.db_heros:
                    break;
                case Mysql_Table_Name.db_setting:
                    break;
                case Mysql_Table_Name.db_artifact:
                    break;
                case Mysql_Table_Name.mo_user_base:
                    break;
                case Mysql_Table_Name.mo_user_value:
                    break;
                case Mysql_Table_Name.mo_user_hero:
                    break;
                case Mysql_Table_Name.mo_user_setting:
                    break;
                case Mysql_Table_Name.mo_user_artifact:
                    break;
                case Mysql_Table_Name.mo_user:
                    break;
                case Mysql_Table_Name.loglist:
                    break;
                case Mysql_Table_Name.user_login:
                    break;
                case Mysql_Table_Name.db_pass:
                    break;
                case Mysql_Table_Name.mo_user_pass:
                    break;
                case Mysql_Table_Name.mo_user_plant:
                    break;
                case Mysql_Table_Name.db_plant:
                    break;
                case Mysql_Table_Name.db_pet:
                    break;
                case Mysql_Table_Name.mo_user_pet_hatching:
                    break;
                case Mysql_Table_Name.mo_user_pet_explore:
                    break;
                case Mysql_Table_Name.mo_user_pet:
                    break;
                case Mysql_Table_Name.db_pet_explore:
                    break;
                case Mysql_Table_Name.mo_user_world:
                    break;
                case Mysql_Table_Name.db_lv:
                    break;
                case Mysql_Table_Name.user_rank:
                    SendNotification(NotiList.Refresh_Rank);
                    break;
                case Mysql_Table_Name.db_hall:
                    break;
                case Mysql_Table_Name.mo_user_achieve:
                    break;
                case Mysql_Table_Name.db_achieve:
                    break;
                case Mysql_Table_Name.db_store:
                    break;
                case Mysql_Table_Name.db_seed:
                    break;
                case Mysql_Table_Name.mo_user_seed:
                    break;
                case Mysql_Table_Name.mo_user_needlist:
                    break;
                case Mysql_Table_Name.db_collect:
                    break;
                case Mysql_Table_Name.mo_user_collect:
                    break;
                case Mysql_Table_Name.db_signin:
                    break;
                case Mysql_Table_Name.dream_user_signin:
                    break;
                case Mysql_Table_Name.mo_user_tap:
                    break;
                case Mysql_Table_Name.mo_user_iphone:
                    break;
                case Mysql_Table_Name.db_pars:
                    break;
                case Mysql_Table_Name.user_message_window:
                    break;
                case Mysql_Table_Name.db_basetask:
                    break;
                case Mysql_Table_Name.mo_user_greenhandguide:
                    break;
                case Mysql_Table_Name.user_player_buff:
                    break;
                case Mysql_Table_Name.user_emial:
                    break;
                case Mysql_Table_Name.server_mail:
                    break;
                case Mysql_Table_Name.history_server_mail:
                    break;
                case Mysql_Table_Name.db_accumulatedrewards:
                    break;
                case Mysql_Table_Name.mo_user_rewards_state:
                    break;
                case Mysql_Table_Name.db_fate:
                    break;
                case Mysql_Table_Name.db_vips:
                    break;
                case Mysql_Table_Name.db_world_boss:
                    break;
                case Mysql_Table_Name.user_world_boss_rank:
                    break;
                case Mysql_Table_Name.user_world_boss:
                    break;
                case Mysql_Table_Name.history_world_boss:
                    break;
                case Mysql_Table_Name.db_formula:
                    break;
                case Mysql_Table_Name.db_suits:
                    break;
                case Mysql_Table_Name.db_dec:
                    break;
                case Mysql_Table_Name.versions_task:
                    break;
                case Mysql_Table_Name.db_weather:
                    break;
                case Mysql_Table_Name.user_trial_towers:
                    break;
                case Mysql_Table_Name.user_world_boss_copy1:
                    break;
                default:
                    break;
            }
        }
    }

}
