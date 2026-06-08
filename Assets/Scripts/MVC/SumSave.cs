using System;
using System.Collections.Generic;
using MVC;

namespace Common
{
    /// <summary>
    /// 中间存储结构
    /// </summary>
    public static class SumSave
    {
        #region 玩家数据中转

        /// <summary>
        /// 用户ID
        /// </summary>
        public static string uid;
        public static string Tapid ;
        /// <summary>
        /// 网络连接
        /// </summary>
        public static bool is_ToMysqlOpen = false; 
        /// <summary>
        /// 服务器几区
        /// </summary>
        public static int par = 1;
        /// <summary>
        /// 系统版本 安卓苹果
        /// </summary>
        public static int system_par = 1;
        /// <summary>
        /// 验证单次收益 0 灵珠 1历练 2魔丸 3材料获取量 4背包 5仓库 6至尊值（用于掉落绝世装备）
        /// </summary>
        public static List<int> base_setting = new List<int>() { 90000, 90000, 9000, 5000};
        /// <summary>
        /// 验证单次材料收益
        /// </summary>
        public static List<long> base_settin_uint = new List<long>();
        /// <summary>
        /// 网络时间
        /// </summary>
        public static DateTime nowtime;
        /// <summary>
        /// 本地时间
        /// </summary>
        public static DateTime local_time = DateTime.Now;
        /// <summary>
        /// 苹果账号 用来判断是否为苹果审核号
        /// </summary>
        public static string ios_account_number="";



        /// <summary>
        /// 版本控制信息
        /// </summary>
        public static user_versions crt_versions;
        /// <summary>
        /// 版本控制器
        /// </summary>
        public static bool OpenGame;
        /// <summary>
        /// 主角色数据
        /// </summary>
        public static user_base_vo crt_user;
        /// <summary>
        /// 当前玩家总属性
        /// </summary>
        public static crtMaxHeroVO crt_MaxHero_okd;
        /// <summary>
        /// 玩家Buff
        /// </summary>
        public static user_player_Buff crt_player_buff;

        /// <summary>
        /// 玩家技能
        /// </summary>
        public static List<base_skill_vo> oldcrt_skills;
        /// <summary>
        /// 角色基准值
        /// </summary>
        public static Hero_VO old_crt_hero;
        /// <summary>
        /// 货币库存
        /// </summary>
        public static user_vo crt_user_unit;
        /// <summary>
        /// 自身通行证
        /// </summary>
        public static user_pass_vo crt_pass;
        /// <summary>
        /// 资源项
        /// </summary>
        public static user_base_Resources_vo crt_resources;
        /// <summary>
        /// 设置
        /// </summary>
        public static user_base_setting_vo crt_settingold;
        /// <summary>
        /// 自身神器
        /// 
        /// </summary>
        public static user_artifact_vo crt_artifact;
        /// <summary>
        /// 设置类型
        /// </summary>
        public static user_setting_type_vo crt_setting_type;
       
         
        /// <summary>
        /// 炼丹数据
        /// </summary>
        public static bag_seed_vo crt_seeds;
        
        /// <summary>
        /// 称号提供极品率
        /// </summary>
        public static int titleLucky = 0;
        /// <summary>
        /// 排行榜
        /// </summary>
        public static rank_vo user_ranks;
        /// <summary>
        /// 无尽试炼排行榜
        /// </summary>
        public static user_endless_battle crt_endless_battle;
        /// <summary>
        /// 自身成就
        /// </summary>
        public static user_achievement_vo crt_achievement;

        /// <summary>
        /// 用户需求信息
        /// </summary>
        public static user_needlist_vo crt_needlist;

        ///<summary>
        ///收集
        ///</summary>
        public static user_collect_vo crt_collect;
        /// <summary>
        /// 自身签到数据
        /// </summary>
        public static user_signin_vo crt_signin; 
        /// <summary>
        /// 滚动消息列表
        /// </summary>
        public static List<(int, string, string)> crt_message_window; 
   
        /// <summary>
        /// 世界boss排行榜
        /// </summary>
        public static mo_world_boss_rank crt_world_boss_rank;
        /// <summary>
        /// 试练塔排行榜
        /// </summary>
        public static mo_world_boss_rank crt_Trial_Tower_rank;
        /// <summary>
        /// 世界boss伤害
        /// </summary>
        public static user_world_boss crt_world_boss_hurt;

        #endregion

        #region 配置db文件
        /// <summary>
        /// 套装属性
        /// </summary>
        public static List<db_suit_vo> db_suits;
        /// <summary>
        /// 具体功能消息
        /// </summary>
        public static List<db_dec> db_dec;
        /// <summary>
        /// 当前世界boss信息
        /// </summary>
        public static db_world_boos db_world_boos = new db_world_boos();
        /// <summary>
        /// 配置通行证
        /// </summary>
        public static List<user_pass_vo> db_pass;
        /// <summary>
        /// 服务器列表
        /// </summary>
        public static List<db_base_par> db_pars;
        /// <summary>
        /// 签到奖励列表
        /// </summary>
        public static List<db_signin_vo> db_Signins;
        /// <summary>
        /// 技能列表
        /// </summary>
        public static List<base_skill_vo> old_db_skills;
        ///<summary>
        /// 设置类型字典
        /// </summary>
        public static List<db_setting_vo> db_sttings;
        /// <summary>
        /// 神器列表
        /// </summary>
        public static List<db_artifact_vo> db_Artifacts;
        
        /// <summary>
        /// 全服玩家的boss伤害
        /// </summary>
        public static List<user_world_boss> db_world_boss_hurt;
        /// <summary>
        /// 天气列表
        /// </summary>
        public static List<db_weather> db_weather_list;

       
        /// <summary>
        /// 激活角色列表
        /// </summary>
        public static List<db_hero_vo> old_db_heros;
     
        /// <summary>
        /// 读取怪物数据
        /// </summary>
        public static List<crtMaxHeroVO> olddb_monsters;
         
        /// <summary>
        /// 种子炼丹列表
        /// </summary>
        public static List<db_seed_vo> db_seeds;
        /// <summary>
        /// 升级列表
        /// </summary>
        public static db_lv_vo_old db_lvs_old;
        /// <summary>
        /// 大厅列表
        /// </summary>
        public static db_hall_vo db_halls;
        /// <summary>
        /// 地图列表
        /// </summary>
        public static List<user_map_vo> read_lose_map;
        /// <summary>
        /// 商店物品列表
        /// </summary>
        public static List<db_store_vo> db_stores_list;
        /// <summary>
        /// 限定商店物品字典
        /// </summary>
        public static Dictionary<string, db_store_vo> db_stores_dic=new Dictionary<string, db_store_vo>();
        /// <summary>
        /// 成就物品字典
        /// </summary>
        public static List<db_achievement_VO> db_Achievement_dic;
        /// <summary>
        /// 收集物品列表
        /// </summary>
        public static List<db_collect_vo> db_collect_vo; 
        /// 命运殿堂列表
        /// </summary>
        public static List<db_fate_vo> db_fate_list;
        /// <summary>
        /// VIP列表
        /// </summary>
        public static List<db_vip> db_vip_list;
        /// <summary>
        /// 转生
        /// </summary>
        public static List<db_reincarnation_vo> db_reincarnation_list;
        /// <summary>
        /// 造化炉合成列表
        /// </summary>
        public static List<db_formula_vo> db_formula_list;
        

        #endregion

        #region 功能文件
        /// <summary>
        /// 五行类型
        /// </summary>
        public static string[] five_element_type = { "土", "火", "水", "木", "金" };
        ///// <summary>
        ///// 怪物对象池
        ///// </summary>
        //public static List<BattleHealth> battleMonsterHealths1 = new List<BattleHealth>();
        ///// <summary>
        ///// 玩家对象池
        ///// </summary>
        //public static List<BattleHealth> battleHeroHealths1 = new List<BattleHealth>();
        /// <summary>
        /// 战斗刷新时间
        /// </summary>
        public static float WaitTime = 5f; 
        /// <summary>
        /// 判断网络开关
        /// </summary>
        public static bool openMysql = false;

        #endregion

        /// <summary>
        /// 设置文件
        /// </summary>
        public static data_settings_vo data_settings = new data_settings_vo();
        /// <summary>
        /// 读取地图
        /// </summary>
        public static List<db_map_vo> db_maps;
        /// <summary>
        /// 读取怪物
        /// </summary>
        public static List<crtMaxBattleVO> db_monsters;
        /// <summary>
        /// 读取英雄
        /// </summary>
        public static List<db_Hero_VO> db_heros;
        /// <summary>
        /// 物品列表
        /// </summary>
        public static List<Bag_Base_VO> db_stditems;
        /// <summary>
        /// 获取技能列表
        /// </summary>
        public static List<db_skill_vo> db_skills;

        /// <summary>
        /// 升级需求
        /// </summary>
        public static Dictionary<int,db_lv_vo> db_lvs;
        /// <summary>
        /// 宠物列表
        /// </summary>
        public static List<db_pet_vo> db_pets;
        /// <summary>
        /// 宠物技能
        /// </summary>
        public static List<db_pet_talent_vo> db_pet_talents;

        /// <summary>
        /// 玩家职业天赋
        /// </summary>
        public static List<db_player_talent_vo>  db_player_talents;

        /// <summary>
        /// 玩家天赋类型
        /// </summary>
        public static List<db_player_talent_type>  db_player_talent_types;
        /// <summary>
        /// 合成列表
        /// </summary>
        public static List<db_synthesis_vo> db_synthesis;
        /// <summary>
        /// 图鉴信息
        /// </summary>
        public static Dictionary<int, List<db_illustrated_vo>> db_illustrateds;
        /// <summary>
        /// 自身图鉴
        /// </summary>
        public static user_illustrated_vo crt_illustrated;
        ///// <summary>
        ///// 当前材料
        ///// </summary>
        public static bag_Resources_vo crt_bag_resources;
        /// <summary>
        /// 玩家属性
        /// </summary>
        public static crtMaxBattleVO crtMaxBattle;
        /// <summary>
        /// 精炼属性
        /// </summary>
        public static user_refined_vo crt_refined;
        /// <summary>
        /// 玩家信息
        /// </summary>
        public static Dream_User_Hero_VO crtHero;
        /// <summary>
        /// 获取背包数据
        /// </summary>
        public static dream_user_bag_VO crt_bags;
        /// <summary>
        /// 获取装备数据
        /// </summary>
        public static dream_user_equip_VO crt_equips;
        /// <summary>
        /// 获取技能数据
        /// </summary>
        public static dream_user_skill_vo crt_skill;
        /// <summary>
        /// 获取宠物数据
        /// </summary>
        public static dream_user_pet_vo crt_pet;
        /// <summary>
        /// 设置数据
        /// </summary>
        public static data_setting_vo crt_setting;
        /// <summary>
        /// 获取全局数据
        /// </summary>
        public static List<global_battle_info_VO> global_battle_info;
        /// <summary>
        /// 领取礼包vip状态
        /// </summary>
        public static data_global_gift_vo crt_global_gift;
        /// <summary>
        /// 大事记
        /// </summary>
        public static List<(int,string)> global_Chronicle;

        /// <summary>
        /// 获取推广
        /// </summary>
        public static global_promotion_vo crt_global_promotion;
        /// <summary>
        /// 获取礼包码
        /// </summary>
        public static global_gift_vo global_gift;

        public static user_zs_vo crt_zs;
    }
}
