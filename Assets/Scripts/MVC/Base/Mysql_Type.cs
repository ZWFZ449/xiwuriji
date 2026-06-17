using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 执行命令
/// </summary>
public enum Mysql_Type
{
    MySqlDataReader,//获取时间
    InsertInto,//初始化
    UpdateInto,//更新
    Delete//删除
}

/// <summary>
/// 表名
/// </summary>
public enum Mysql_Table_Name
{
    db_monster,
    db_stditems,
    db_magic,
    db_maps,
    db_heros,//标准参数
    db_setting,//标准设置信息
    db_artifact,//标准神器
    mo_user_base,//用户基础信息
    mo_user_value,//用户资源信息
    mo_user_hero,//用户英雄信息
    mo_user_setting,//用户设置信息
    mo_user_artifact,//用户神器信息
    mo_user,//用户货币信息
    loglist,//日志
    user_login,//用户登录
    db_pass,//通行证
    mo_user_pass,//用户通行证
    mo_user_plant,//用户种植信息
    db_plant,//种植信息
    db_pet,//宠物信息
    mo_user_pet_hatching,//用户宠物孵化信息
    mo_user_pet_explore,//用户宠物探险信息
    mo_user_pet,//用户宠物信息
    db_pet_explore,//宠物探险信息
    mo_user_world,
    db_lv,//等级信息
    user_rank,//排行榜
    db_hall,//大厅信息
    mo_user_achieve,//用户成就信息
    db_achieve,//成就数据信息
    db_store,//商店信息
    db_seed,//炼丹种子信息
    mo_user_seed,//用户炼丹种子信息
    mo_user_needlist,//用户需求信息
    db_collect,//收集信息
    mo_user_collect,//用户收集完成信息
    db_signin,//签到信息
    //mo_user_signin,
    mo_user_tap,//tap登录
    mo_user_iphone,//苹果登录
    db_pars,//服务器列表
    user_message_window,//消息窗口
    db_basetask,//大世界信息
    mo_user_greenhandguide,//新手引导
    user_player_buff,//玩家buff
    user_emial,//自身邮件
    server_mail,//当前邮件
    history_server_mail,//历史邮件
    db_accumulatedrewards,//累计奖励
    mo_user_rewards_state,//用户累计奖励状态
    db_fate,//命运殿堂
    db_vips,//vip信息
    db_world_boss,//世界boss
    user_world_boss_rank,//世界boss排行榜
    user_world_boss,//世界boss伤害
    history_world_boss,//世界boss历史伤害
    db_formula,//造化炉合成信息
    db_suits,//套装信息
    db_dec,//具体功能消息
    versions_task,//版本信息
    db_weather,//天气信息
    user_trial_towers,//试炼塔
    user_world_boss_copy1,//世界boss测试
    db_endlessbattle,//无尽试炼
    user_endless_battle,//无尽试炼排行榜
    db_strengthen_needlist,//强化需求信息
    db_equip_suit,//装备套装信息
    mysql_time,//时间信息




    db_hero_type,//英雄类型
    db_player_talents,//玩家天赋
    db_player_talent_type,//玩家天赋类型
    db_pet_talent,//宠物天赋
    Dream_Users,//用户货币信息
    Dream_user_base,//用户基础信息
    dream_base_hero,//用户英雄信息
    dream_user_bag,//用户背包信息
    dream_user_equip,//用户装备信息
    dream_user_skill,//用户技能信息
    dream_user_pet,//用户宠物信息
    user_data_settings,//用户设置信息
    dream_user_signin,//用户签到信息
    db_synthesiss,//合成信息
    db_illustrated,//图鉴信息
    dream_user_illustrated,//用户图鉴信息
    global_battle_info,//全局战斗信息
    db_chronicle,//大事记
    dream_user_gift,//用户礼物信息
    global_promotion,//全局推广
    global_gift,//全局礼物
    history_global_gift,//历史全局礼物
    global_uid,//全局uid
    global_account,//全局账号
    db_reincarnation,//转生信息
    dream_user_zs,//用户转生信息
    dream_user_refineds,//用户精炼信息
}
