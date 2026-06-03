using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using CodeStage.AntiCheat.ObscuredTypes;
using Common;
using MVC;
using MySql.Data.MySqlClient;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 读取数据
/// </summary>
public static class ReadDb
{

    public static db_weather Read_weather(MySqlDataReader reader)
    {
        ObscuredInt _weather_index= reader.GetInt32(reader.GetOrdinal("weather_index"));
        string _weather_type= reader.GetString(reader.GetOrdinal("weather_type"));
        string _life_value= reader.GetString(reader.GetOrdinal("life_value"));
        ObscuredInt _probability= reader.GetInt32(reader.GetOrdinal("probability"));
        return new db_weather(_weather_index, _weather_type, _life_value, _probability);
    }

    public static db_lv_vo Read_lv(MySqlDataReader reader)
    { 
        return new db_lv_vo(reader.GetInt32(reader.GetOrdinal("lv")), reader.GetInt64(reader.GetOrdinal("exp")));
    } 
    public static db_formula_vo Read_formula(MySqlDataReader reader)
    {
        ObscuredInt _formula_type= reader.GetInt32(reader.GetOrdinal("formula_type"));
        string _formula_result= reader.GetString(reader.GetOrdinal("formula_result"));
        string _formula_need= reader.GetString(reader.GetOrdinal("formula_need"));
        return new db_formula_vo(_formula_type, _formula_result, _formula_need);
    }


    public static db_suit_vo Read_suit(MySqlDataReader reader)
    {
        string _suit_name = reader.GetString(reader.GetOrdinal("suit_name"));
        ObscuredInt _suit_number = reader.GetInt32(reader.GetOrdinal("suit_number"));
        ObscuredInt _suit_type = reader.GetInt32(reader.GetOrdinal("suit_type"));
        return new db_suit_vo(_suit_name, _suit_number, _suit_type, reader.GetString(reader.GetOrdinal("suit_value")));
    }



    public static user_world_boss Read_world_boss(MySqlDataReader reader)
    {
        ObscuredInt _damage = reader.GetInt32(reader.GetOrdinal("damage"));
        DateTime _datetime = Convert.ToDateTime(reader.GetString(reader.GetOrdinal("datetime")));
        ObscuredInt _par = reader.GetInt32(reader.GetOrdinal("par"));
        string _uid = reader.GetString(reader.GetOrdinal("uid"));
        return new user_world_boss(_damage, _datetime, _par, _uid);
    }

    public static db_fate_vo Read_fate(MySqlDataReader reader)
    {
        ObscuredInt _fate_id = reader.GetInt32(reader.GetOrdinal("fate_index"));
        string _fate_value = reader.GetString(reader.GetOrdinal("fate_value"));
        return new db_fate_vo(_fate_id, _fate_value);
    }

    public static user_base_vo Read_user_base(MySqlDataReader reader)
    {
        user_base_vo item = new user_base_vo();
        item.uid= reader.GetString(reader.GetOrdinal("uid"));
        item.RegisterDate= Convert.ToDateTime(reader.GetString(reader.GetOrdinal("RegisterDate")));
        item.Nowdate= Convert.ToDateTime(reader.GetString(reader.GetOrdinal("Nowdate")));
        item.par= reader.GetInt32(reader.GetOrdinal("par"));
        item.isdownloadable = reader.GetInt32(reader.GetOrdinal("isdownloadable"));
        return item;
    } 
 
    public static global_promotion_vo Read(MySqlDataReader reader, global_promotion_vo item,string uid)
    {
        string value = reader.GetString(reader.GetOrdinal("promotion_value"));
        ObscuredInt moeny= reader.GetInt32(reader.GetOrdinal("promotion_moeny"));
        item.Init(value, moeny,"uid", uid);
        return item;
    }

    public static db_setting_vo Read(MySqlDataReader reader, db_setting_vo item)
    {
        item.setting_value = reader.GetString(reader.GetOrdinal("setting_value"));
        item.setting_type = reader.GetInt32(reader.GetOrdinal("setting_type"));
        return item;
    }
    public static global_gift_vo Read(MySqlDataReader reader, global_gift_vo item)
    {

        item.Init(
            reader.GetInt32(reader.GetOrdinal("id")),
            reader.GetInt32(reader.GetOrdinal("gift_type")),
            reader.GetInt32(reader.GetOrdinal("gift_par")),
            reader.GetString(reader.GetOrdinal("gift_selectkey")),
            reader.GetString(reader.GetOrdinal("gift_value")),
            reader.GetInt32(reader.GetOrdinal("gift_state")),
            reader.GetInt32(reader.GetOrdinal("Gift_Points"))
            );
        return item;
    }
    public static global_battle_info_VO Read(MySqlDataReader reader, global_battle_info_VO item)
    {
        item.value = reader.GetString(reader.GetOrdinal("user_value"));
        return item;
    }
    public static data_global_gift_vo Read(MySqlDataReader reader, data_global_gift_vo item)
    {
        item.Init(
            ArrayHelper.Get_Split<string>(reader.GetString(reader.GetOrdinal("gift_value")), ','),
            reader.GetInt32(reader.GetOrdinal("gift_points"))
            );
        return item;
    }
    
    public static user_player_Buff Read(MySqlDataReader reader, user_player_Buff item)
    {
        item.player_baff = reader.GetString(reader.GetOrdinal("player_Buff"));
        item.SplitBuff();
        return item;
    }

    public static user_collect_vo Read(MySqlDataReader reader, user_collect_vo item)
    {
        item.collect_value = reader.GetString(reader.GetOrdinal("collect_value"));
        //item.collect_suit_value = reader.GetString(reader.GetOrdinal("collect_suit_value"));
        return item;
    } 

    public static bag_seed_vo Read(MySqlDataReader reader, bag_seed_vo item)
    {
        item.user_value = reader.GetString(reader.GetOrdinal("user_value"));
        item.formula_value = reader.GetString(reader.GetOrdinal("formula_value"));
        item.use_value = reader.GetString(reader.GetOrdinal("user_use_value"));
        item.Init();
        return item;
    }


    public static db_signin_vo Read_signin(MySqlDataReader reader)
    {
        ObscuredInt _index = reader.GetInt32(reader.GetOrdinal("index"));
        string _value = reader.GetString(reader.GetOrdinal("value"));
        return new db_signin_vo(_index, _value);
    }
    public static user_signin_vo Read(MySqlDataReader reader, user_signin_vo item)
    {
        item.now_time = Convert.ToDateTime(reader.GetString(reader.GetOrdinal("now_time")));
        item.number = reader.GetInt32(reader.GetOrdinal("number"));
        //item.user_value = reader.GetString(reader.GetOrdinal("user_value"));
        item.max_number= reader.GetInt32(reader.GetOrdinal("max_number"));
        item.Init(reader.GetString(reader.GetOrdinal("user_value")));
        return item;
    }
    
    public static db_seed_vo Read_seed(MySqlDataReader reader)
    {
        string type = reader.GetString(reader.GetOrdinal("type"));
        ObscuredInt sequence = reader.GetInt32(reader.GetOrdinal("sequence"));
        string seed_name = reader.GetString(reader.GetOrdinal("seed_name"));
        string seed_formula = reader.GetString(reader.GetOrdinal("seed_formula"));
        string pill = reader.GetString(reader.GetOrdinal("pill"));
        string formula = reader.GetString(reader.GetOrdinal("formula"));
        string pill_effect = reader.GetString(reader.GetOrdinal("pill_effect"));
        ObscuredInt Weight = reader.GetInt32(reader.GetOrdinal("Weight"));
        ObscuredInt seed_number = reader.GetInt32(reader.GetOrdinal("seed_number"));
        ObscuredInt rule = reader.GetInt32(reader.GetOrdinal("rule"));
        ObscuredInt dicdictionary_index = reader.GetInt32(reader.GetOrdinal("dicdictionary_index"));
        ObscuredInt limit = reader.GetInt32(reader.GetOrdinal("limit"));
        return new db_seed_vo(type, sequence, seed_name, seed_formula, pill, formula, pill_effect, Weight, seed_number, rule,dicdictionary_index, limit);
    }

    public static db_collect_vo Read_collect(MySqlDataReader reader)
    {
        string Name = reader.GetString(reader.GetOrdinal("Name"));
        string StdMode = reader.GetString(reader.GetOrdinal("StdMode"));
        string bonuses_type = reader.GetString(reader.GetOrdinal("Collect bonuses type"));
        string bonuses_value = reader.GetString(reader.GetOrdinal("Collect bonuses value"));
        return new db_collect_vo(Name, StdMode, bonuses_type, bonuses_value);
    }

    
    public static user_needlist_vo Read(MySqlDataReader reader, user_needlist_vo item)
    {
        item.store_value = reader.GetString(reader.GetOrdinal("store_value"));
        item.map_value = reader.GetString(reader.GetOrdinal("map_value"));
        item.fate_value = reader.GetString(reader.GetOrdinal("fate_value"));
        item.user_value= reader.GetString(reader.GetOrdinal("user_value"));
        item.Init();
        return item;
    } 
    public static db_hall_vo Read(MySqlDataReader reader, db_hall_vo item)
    {
        string otainlist= reader.GetString(reader.GetOrdinal("otainlist"));
        item.otainlist_btn = new System.Collections.Generic.List<string>();
        string[] otainlist_btn = otainlist.Split(' ');
        for (ObscuredInt i = 0; i < otainlist_btn.Length; i++)
        { 
            item.otainlist_btn.Add(otainlist_btn[i]);
        }

        string otainpanel= reader.GetString(reader.GetOrdinal("otainpanel"));
        item.otainpanel = new System.Collections.Generic.List<string>();
        string[] otainpanel_btn = otainpanel.Split(' ');
        for (ObscuredInt i = 0; i < otainpanel_btn.Length; i++)
        { 
            item.otainpanel.Add(otainpanel_btn[i]);
        }

        string maplist= reader.GetString(reader.GetOrdinal("maplist"));
        item.maplist_btn = new System.Collections.Generic.List<string>();
        string[] maplist_btn = maplist.Split(' ');
        for (ObscuredInt i = 0; i < maplist_btn.Length; i++)
        { 
            item.maplist_btn.Add(maplist_btn[i]);
        }

        string mappanel= reader.GetString(reader.GetOrdinal("mappanel"));
        item.mappanel = new System.Collections.Generic.List<string>();
        string[] mappanel_btn = mappanel.Split(' ');
        for (ObscuredInt i = 0; i < mappanel_btn.Length; i++)
        { 
            item.mappanel.Add(mappanel_btn[i]);
        }

        string herolist= reader.GetString(reader.GetOrdinal("herolist"));
        item.herolist_btn = new System.Collections.Generic.List<string>();
        string[] herolist_btn = herolist.Split(' ');
        for (ObscuredInt i = 0; i < herolist_btn.Length; i++)
        { 
            item.herolist_btn.Add(herolist_btn[i]);
        }

        string heropanel= reader.GetString(reader.GetOrdinal("heropanel"));
        item.heropanel = new System.Collections.Generic.List<string>();
        string[] heropanel_btn = heropanel.Split(' ');
        for (ObscuredInt i = 0; i < heropanel_btn.Length; i++)
        { 
            item.heropanel.Add(heropanel_btn[i]);
        }
        return item;
    } 
    public static user_illustrated_vo Read_illustrated(MySqlDataReader reader)
    {
        user_illustrated_vo item = new user_illustrated_vo();
        item.Iint(reader.GetString(reader.GetOrdinal("illustrated_value")));
        return item;
    }


    public static db_achievement_VO Read_achievement_VO(MySqlDataReader reader)
    {
        ObscuredInt achievement_type = reader.GetInt32(reader.GetOrdinal("achieve_type"));
        string achievement_value = reader.GetString(reader.GetOrdinal("achieve_value"));
        string achievement_need = reader.GetString(reader.GetOrdinal("achieve_need"));
        string[] achievement_show_lv = reader.GetString(reader.GetOrdinal("achieve_show_lv")).Split('|');
        string achievement_reward = reader.GetString(reader.GetOrdinal("achieve_reward"));
        string achievement_exchange_offect = reader.GetString(reader.GetOrdinal("achieve_exchange_offect"));
        return new db_achievement_VO(achievement_type, achievement_value, achievement_need, achievement_show_lv, achievement_reward, achievement_exchange_offect);
    }




    public static user_achievement_vo Read(MySqlDataReader reader, user_achievement_vo item)
    {
        item.achievement_exp = reader.GetString(reader.GetOrdinal("achieve_exp"));
        item.achievement_lvs = reader.GetString(reader.GetOrdinal("achieve_lvs"));
        #region 更换现有玩家成就词条
        //if (item.achievement_exp == "" && item.achievement_lvs == "")
        //{
        //    return item;
        //}
        //string[] str = item.achievement_exp.Split("|");
        //string[] strsTemp = str[0].Split(" ");
        //if (strsTemp[0] != Common.SumSave.db_Achievement_dic[0].achievement_value)  //如果为旧数据库词条 只需检测第一条 一条不符则全部不符
        //{
        //    string[] lv = item.achievement_lvs.Split("|");
        //    List<string[]> expList = new List<string[]>();  //expList[0][0]为第一词条的名称 expList[0][1]为第一词条的值 依次类推
        //    List<string[]> lvList = new List<string[]>();
        //    for (ObscuredInt j = 0; j < lv.Length; j++)
        //    {
        //        expList.Add(str[j].Split(" "));
        //        lvList.Add(lv[j].Split(" "));
        //    }
        //    if (Common.SumSave.db_Achievement_dic.Count != expList.Count)
        //    {
        //        ObscuredInt temp = 0; //从17开始需要 跳过3个索引
        //        for (ObscuredInt i = 0; i < Common.SumSave.db_Achievement_dic.Count - 3; i++) //这里可以直接减去3个雪域地图
        //        {
        //            if (i == 17 || i == 18 || i == 19) { temp = 3; }//部分玩家没有雪域地图
        //            if (Common.SumSave.db_Achievement_dic[i + temp].achievement_value != expList[i][0])
        //            {
        //                expList[i][0] = Common.SumSave.db_Achievement_dic[i + temp].achievement_value;
        //                lvList[i][0] = Common.SumSave.db_Achievement_dic[i + temp].achievement_value;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        for (ObscuredInt i = 0; i < Common.SumSave.db_Achievement_dic.Count; i++)
        //        {
        //            if (Common.SumSave.db_Achievement_dic[i].achievement_value != expList[i][0])
        //            {
        //                expList[i][0] = Common.SumSave.db_Achievement_dic[i].achievement_value;
        //                lvList[i][0] = Common.SumSave.db_Achievement_dic[i].achievement_value;
        //            }
        //        }
        //    }

        //    item.achievement_exp = "";
        //    item.achievement_lvs = "";
        //    for (ObscuredInt i = 0; i < str.Length; i++)
        //    {
        //        item.achievement_exp += i == 0 ? expList[i][0] + " " + expList[i][1] : "|" + expList[i][0] + " " + expList[i][1];
        //        item.achievement_lvs += i == 0 ? lvList[i][0] + " " + lvList[i][1] : "|" + lvList[i][0] + " " + lvList[i][1];
        //    }
        //}
        #endregion
        item.Init();
        return item;
    }

    public static db_reincarnation_vo Read_Reincarnation(MySqlDataReader reader)
    {
        return new db_reincarnation_vo(
            reader.GetInt32(reader.GetOrdinal("reincarnation_lv")),
            reader.GetString(reader.GetOrdinal("reincarnation_name")),
            reader.GetString(reader.GetOrdinal("reincarnation_need")),
            reader.GetString(reader.GetOrdinal("reincarnation_effect")),
            reader.GetInt32(reader.GetOrdinal("need_lv")),
            reader.GetInt32(reader.GetOrdinal("need_maxLv")),
            reader.GetInt32(reader.GetOrdinal("result_maxRefinement")),
            reader.GetInt32(reader.GetOrdinal("result_maxmedicine")),
             reader.GetInt32(reader.GetOrdinal("result_minRefinement")),
            reader.GetInt32(reader.GetOrdinal("result_minmedicine"))
            );
    }
   
    public static db_vip Read_Vip(MySqlDataReader reader)
    {
        ObscuredInt vip_lv = reader.GetInt32(reader.GetOrdinal("vip_lv"));
        string vip_name = reader.GetString(reader.GetOrdinal("vip_name"));
        ObscuredInt vip_exp = reader.GetInt32(reader.GetOrdinal("vip_exp"));
        ObscuredInt experienceBonus = reader.GetInt32(reader.GetOrdinal("experienceBonus"));
        ObscuredInt lingzhuIncome = reader.GetInt32(reader.GetOrdinal("lingzhuIncome"));
        ObscuredInt equipmentExplosionRate = reader.GetInt32(reader.GetOrdinal("equipmentExplosionRate"));
        ObscuredInt characterExperience = reader.GetInt32(reader.GetOrdinal("characterExperience"));
        ObscuredInt monsterHuntingInterval = reader.GetInt32(reader.GetOrdinal("monsterHuntingInterval"));
        ObscuredInt hpRecovery = reader.GetInt32(reader.GetOrdinal("hpRecovery"));
        ObscuredInt manaRegeneration = reader.GetInt32(reader.GetOrdinal("manaRegeneration"));
        ObscuredInt goodFortune = reader.GetInt32(reader.GetOrdinal("goodFortune"));
        ObscuredInt strengthenCosts = reader.GetInt32(reader.GetOrdinal("strengthenCosts"));
        ObscuredInt offlineInterval = reader.GetInt32(reader.GetOrdinal("offlineInterval"));
        ObscuredInt signInIncome = reader.GetInt32(reader.GetOrdinal("signInIncome"));
        ObscuredInt whippingCorpses = reader.GetInt32(reader.GetOrdinal("whippingCorpses"));
        string gift_value = reader.GetString(reader.GetOrdinal("gift_value"));
        return new db_vip(vip_lv, vip_name, vip_exp, experienceBonus, lingzhuIncome, equipmentExplosionRate, characterExperience
            , monsterHuntingInterval, hpRecovery, manaRegeneration, goodFortune, strengthenCosts, offlineInterval, signInIncome,
            whippingCorpses, gift_value);
    }


 
    public static user_pass_vo Read(MySqlDataReader reader, user_pass_vo item)
    {
        item.data_lv = reader.GetInt32(reader.GetOrdinal("pass_lv"));
        item.data_exp = reader.GetInt32(reader.GetOrdinal("pass_exp"));
        item.Max_task_number= reader.GetInt32(reader.GetOrdinal("Max_task_number"));
        item.user_value = reader.GetString(reader.GetOrdinal("user_value"));
        item.day_state_value = reader.GetString(reader.GetOrdinal("day_state_value"));
        item.Init();
        return item;
    }
    public static user_pass_vo Read_Pass(MySqlDataReader reader, user_pass_vo item)
    {
        item.lv = reader.GetInt32(reader.GetOrdinal("db_lv"));
        item.pass_index= reader.GetInt32(reader.GetOrdinal("db_index"));
        item.reward = reader.GetString(reader.GetOrdinal("reward"));
        item.uplv_reward = reader.GetString(reader.GetOrdinal("uplv_reward"));
        return item;
    }
    public static user_vo Read(MySqlDataReader reader, user_vo item)
    {
        string value= reader.GetString(reader.GetOrdinal("value"));
        //item.Init(DateTime.Now,value);
        return item;
    }
    public static user_artifact_vo Read(MySqlDataReader reader, user_artifact_vo item)
    {
        item.artifact_value = reader.GetString(reader.GetOrdinal("artifact_value"));
        item.Init();
        return item;
    }

    public static db_artifact_vo Read_artifact_vo(MySqlDataReader reader)
    {
        string arrifact_name = reader.GetString(reader.GetOrdinal("Artifact_name"));
        string[] Artifact_open_needs = reader.GetString(reader.GetOrdinal("Artifact_open_need")).Split('&');
        string[] arrifact_needs = reader.GetString(reader.GetOrdinal("Artifact_need")).Split('&');
        string[] arrifact_effects = reader.GetString(reader.GetOrdinal("Artifact_effect")).Split('&');
        ObscuredInt arrifact_type = reader.GetInt32(reader.GetOrdinal("Artifact_type"));
        string Artifact_dec = reader.GetString(reader.GetOrdinal("Artifact_dec"));
        ObscuredInt Artifact_MaxLv = reader.GetInt32(reader.GetOrdinal("Artifact_MaxLv"));
        return new db_artifact_vo(arrifact_name, Artifact_open_needs, arrifact_needs, arrifact_effects, arrifact_type, Artifact_dec, Artifact_MaxLv);
    }

     
     

}