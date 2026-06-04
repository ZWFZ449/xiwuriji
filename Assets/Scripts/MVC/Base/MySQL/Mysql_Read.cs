
using CodeStage.AntiCheat.ObscuredTypes;
using Common;
using MVC;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
/// <summary>
/// 读取基准值
/// </summary>
public static class Mysql_Read
{
    /// <summary>
    /// 读取路径
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="col"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static string Select(Mysql_Table_Name tableName, string col, string values)
    {
        string query = "SELECT * FROM " + tableName + " WHERE " + col + " = " + values;

        return query;
    }
    public static void Read(DbDataReader mysqlReader)
    {
        for (int i = 0; i < mysqlReader.FieldCount; i++)
        {
            SumSave.nowtime = Convert.ToDateTime(mysqlReader[i].ToString());
            //Debug.Log("基准值读取成功" + SumSave.nowtime);
        }
    } 
    public static user_zs_vo Read(MySqlDataReader reader, user_zs_vo item)
    { 
        item.zs_medicine_max= reader.GetInt32(reader.GetOrdinal("zs_medicine_max"));
        item.zs_Refinement_max = reader.GetInt32(reader.GetOrdinal("zs_Refinement_max"));
        string[] zs_medicine_value = reader.GetString(reader.GetOrdinal("crt_medicine")).Split(',');
        //List<int> zs_medicine = ArrayHelper.Get_Split<int>(zs_medicine_value, ',');
        item.crt_medicine = new List<ObscuredInt>();
        for (int i = 0; i < zs_medicine_value.Length; i++)
        { 
            if (zs_medicine_value[i] != "")
            item.crt_medicine.Add(int.Parse(zs_medicine_value[i]));
        }
        string[] zs_Refinement_value = reader.GetString(reader.GetOrdinal("crt_Refinement")).Split(',');// ArrayHelper.Get_Split<int>(reader.GetString(reader.GetOrdinal("crt_Refinement")), ',');
        item.crt_Refinement = new List<ObscuredInt>();
        for (int i = 0; i < zs_Refinement_value.Length; i++)
        {
            if (zs_Refinement_value[i] != "")
            item.crt_Refinement.Add(int.Parse(zs_Refinement_value[i]));
        }
        item.medicine_exp = reader.GetInt32(reader.GetOrdinal("medicine_exp"));
        return item;
    }
    public static user_vo Read(MySqlDataReader reader, user_vo item)
    {
        DateTime nowtime = Convert.ToDateTime(reader.GetString(reader.GetOrdinal("nowtime")));
        string buff_value= reader.GetString(reader.GetOrdinal("buff_value"));
        string value = reader.GetString(reader.GetOrdinal("value"));
        item.Init(nowtime,value, buff_value);
        return item;
    }

    public static db_synthesis_vo Read_Synthesis(MySqlDataReader reader)//
    {
        return new db_synthesis_vo
            (
            reader.GetString(reader.GetOrdinal("synthesis_name")),
            reader.GetString(reader.GetOrdinal("synthesis_type")),
            reader.GetString(reader.GetOrdinal("synthesis_need"))
            );

    }
    
    public static db_illustrated_vo Read_illustrated(MySqlDataReader reader)
    {
        return new db_illustrated_vo
        (
           reader.GetString(reader.GetOrdinal("Illustrated_name")),
           reader.GetInt32(reader.GetOrdinal("Illustrated_type")),
           reader.GetInt32(reader.GetOrdinal("Illustrated_Shape")),
           reader.GetString(reader.GetOrdinal("Illustrated_need")),
           reader.GetString(reader.GetOrdinal("Illustrated_effect"))
        );
    }
    public static db_pet_vo Read_Pet(MySqlDataReader reader)//
    {
        return new db_pet_vo
                (
            reader.GetInt32(reader.GetOrdinal("pet_id")),
            reader.GetString(reader.GetOrdinal("pet_name")),
            reader.GetInt32(reader.GetOrdinal("pet_ac")),
            reader.GetInt32(reader.GetOrdinal("pet_mac")),
            reader.GetInt32(reader.GetOrdinal("pet_dc")),
            reader.GetInt32(reader.GetOrdinal("pet_mc")),
            reader.GetInt32(reader.GetOrdinal("pet_sc")),
            reader.GetString(reader.GetOrdinal("pet_talent")),
            reader.GetFloat(reader.GetOrdinal("pet_scale"))
            );
    }
    /// <summary>
    /// 读取宠物技能
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public static db_pet_talent_vo Read_Pet_Talent(MySqlDataReader reader)//
    {
        return new db_pet_talent_vo
                (
            reader.GetInt32(reader.GetOrdinal("id")),
            reader.GetString(reader.GetOrdinal("pet_talent_name")),
            reader.GetInt32(reader.GetOrdinal("pet_talent_level")),
            reader.GetInt32(reader.GetOrdinal("pet_talent_job")),
            reader.GetInt32(reader.GetOrdinal("pet_talent_type")),
            reader.GetInt32(reader.GetOrdinal("pet_talent_offect")),
            reader.GetInt32(reader.GetOrdinal("pet_talent_offecttype")),
            reader.GetFloat(reader.GetOrdinal("pet_talent_offectvalue")),
            ArrayHelper.Get_Split<float>(reader.GetString(reader.GetOrdinal("talent_up_value")), ' ')
            );
    }

    public static db_player_talent_type Read_Player_Talent_Type(MySqlDataReader reader)//
    {
        return new db_player_talent_type
                (
                        reader.GetInt32(reader.GetOrdinal("job")),
                        ArrayHelper.Get_Split<string>(reader.GetString(reader.GetOrdinal("talent_value")), ' ')
            );
    }

    public static db_player_talent_vo Read_Player_Talent(MySqlDataReader reader)//
    {
        return new db_player_talent_vo
        (
            reader.GetInt32(reader.GetOrdinal("id")),
            reader.GetString(reader.GetOrdinal("player_talent_name")),
            reader.GetInt32(reader.GetOrdinal("job")),
            reader.GetInt32(reader.GetOrdinal("talent_type")),
            reader.GetInt32(reader.GetOrdinal("talent_need_lv")),
            ArrayHelper.Get_Split<int>( reader.GetString(reader.GetOrdinal("ralent_need_uplv")),' '),
            ArrayHelper.Get_Split<string>(reader.GetString(reader.GetOrdinal("talent_need_lv_value")), ' '),
            reader.GetInt32(reader.GetOrdinal("talent_offect")),
            ArrayHelper.Get_Split<int>(reader.GetString(reader.GetOrdinal("talent_offect_value")), ' '),
            reader.GetInt32(reader.GetOrdinal("correlation_skill"))
        );
    }
    public static dream_user_pet_vo Read(MySqlDataReader reader, dream_user_pet_vo item)
    {
        item.Init(reader.GetString(reader.GetOrdinal("crt_pet")), reader.GetString(reader.GetOrdinal("sum_pet")));
        return item;
    }

    public static data_setting_vo Read(MySqlDataReader reader, data_setting_vo item)
    {
        item.Iint(reader.GetString(reader.GetOrdinal("data_base_setting")),
           reader.GetString(reader.GetOrdinal("data_medicine_setting")),
           reader.GetString(reader.GetOrdinal("data_battle_boss_setting")), 
           reader.GetString(reader.GetOrdinal("data_setting"))
           );
        return item;
    }

    public static crtMaxBattleVO Read_Monster(MySqlDataReader reader) 
    {

        crtMaxBattleVO item = new crtMaxBattleVO(0, 0, 0, 0, 0);
        List<(enum_battle_pet_talent_list, float, float)> list = new List<(enum_battle_pet_talent_list, float, float)>();
        item.id = reader.GetInt32(reader.GetOrdinal("id"));
        item.crt_name = reader.GetString(reader.GetOrdinal("monster_name"));// SumSave.crtHero.hero_name;
        item.lv = reader.GetInt32(reader.GetOrdinal("lv"));
        item.exp = reader.GetInt32(reader.GetOrdinal("exp"));
        item.hero_type = Hero_Type.平民;
        item.type = (Battle_Game_Type)(reader.GetInt32(reader.GetOrdinal("monster_type")));
        item.skill_id = reader.GetInt32(reader.GetOrdinal("skill_id"));
        item.skill_number = reader.GetInt32(reader.GetOrdinal("skill_number"));
        item.skill_level = reader.GetInt32(reader.GetOrdinal("skill_level"));
        item.data = new FinalBattleValueVO(
             reader.GetInt64(reader.GetOrdinal("hp")),
             reader.GetInt32(reader.GetOrdinal("mp")),
             0,//reader.GetInt32(reader.GetOrdinal("hp")),
             0,// reader.GetInt32(reader.GetOrdinal("mp")),
             reader.GetInt32(reader.GetOrdinal("dc")),
             reader.GetInt32(reader.GetOrdinal("dc2")),
             reader.GetInt32(reader.GetOrdinal("mac")),
             reader.GetInt32(reader.GetOrdinal("mac2")),
             reader.GetInt32(reader.GetOrdinal("ac")),
             reader.GetInt32(reader.GetOrdinal("ac2")),
             reader.GetInt32(reader.GetOrdinal("sc")),
             reader.GetInt32(reader.GetOrdinal("sc2")),
             reader.GetInt32(reader.GetOrdinal("mc")),
             reader.GetInt32(reader.GetOrdinal("mc2")),
             reader.GetInt32(reader.GetOrdinal("hit")),
             reader.GetInt32(reader.GetOrdinal("dodge")),
             reader.GetInt32(reader.GetOrdinal("crit")),
             reader.GetInt32(reader.GetOrdinal("critDmg")),
             reader.GetInt32(reader.GetOrdinal("hpRegen")),
             0,//reader.GetInt32(reader.GetOrdinal("mpRegen")),
             0,0,0,0,0,0,0,//battle_hp, battle_mp, battle_ac, battle_mac, battle_dc, battle_sc, battle_mc,
             reader.GetInt32(reader.GetOrdinal("battle_speed")),
             reader.GetInt32(reader.GetOrdinal("battle_range")),
             0,//reader.GetInt32(reader.GetOrdinal("move_speed")),//battle_damage
             0,//reader.GetInt32(reader.GetOrdinal("battle_def")),
             list, 0, 
             reader.GetInt32(reader.GetOrdinal("damage_reduction")),
             0,//reader.GetInt32(reader.GetOrdinal("magic_damage_reduction")),
             reader.GetInt32(reader.GetOrdinal("move_speed"))
             );
        /*
         * long battle_maxhp, int battle_maxmp, long hp, int mp, int dc, int dc2, int mac,
        int mac2, int ac, int ac2, int sc, int sc2, int mc, int mc2, int hit, int dodge, int crit, int critDmg, int hpRegen, int mpRegen,
        int battle_hp, int battle_mp, int battle_ac, int battle_mac, int battle_dc, int battle_sc, int battle_mc, int battle_speed, int battle_range, int battle_Damage, int battle_def,
        List<(enum_battle_pet_talent_list, int, int)> buffList,int lucky,int damage_reduction, int magic_damage_reduction,int move_speed
         */
#if UNITY_EDITOR
        UI.UI_Manager.I.GetEquipSprite("monster/", item.crt_name);
#elif UNITY_ANDROID

#elif UNITY_IPHONE
            
#endif
        return item;
    }

    public static db_store_vo Read(MySqlDataReader reader)
    {
        int store_Type = reader.GetInt32(reader.GetOrdinal("StoreType"));
        string ItemName = reader.GetString(reader.GetOrdinal("ItemName"));
        int ItemPrice = reader.GetInt32(reader.GetOrdinal("ItemPrice"));
        int ItemMaxQuantity = reader.GetInt32(reader.GetOrdinal("ItemMaxQuantity"));
        string unit = reader.GetString(reader.GetOrdinal("unit"));
        string[] dis = reader.GetString(reader.GetOrdinal("discount")).Split(' ');
        (int, int) discount = (0, 0);
        if (dis.Length == 2)
        {
            discount = (int.Parse(dis[0]), int.Parse(dis[1]));
        }
        return new db_store_vo(store_Type, ItemName, ItemPrice, unit, discount, ItemMaxQuantity);
    }
    public static (int,string) Read_Chronicle(MySqlDataReader reader)
    {
        (int, string) item = (0, "");
        item.Item1 = reader.GetInt32(reader.GetOrdinal("id"));
        item.Item2 = reader.GetString(reader.GetOrdinal("Chronicle_value"));
        return item;
    }
    public static db_skill_vo ReadSkill(MySqlDataReader reader)
    {
        int id = reader.GetInt32(reader.GetOrdinal("id"));
        string show_name = reader.GetString(reader.GetOrdinal("show_name"));
        int EffectType = reader.GetInt32(reader.GetOrdinal("EffectType"));
        int Effect = reader.GetInt32(reader.GetOrdinal("Effect"));
        string spells = reader.GetString(reader.GetOrdinal("spells"));
        List<int> spellvalues = new List<int>();
        string[] spell = spells.Split(' ');
        for (int i = 0; i < spell.Length; i++)
        {
            if (!string.IsNullOrEmpty(spell[i])) spellvalues.Add(int.Parse(spell[i]));
        }
        int Power = reader.GetInt32(reader.GetOrdinal("Power"));
        string DefPowers = reader.GetString(reader.GetOrdinal("DefPowers"));
        string[] def = DefPowers.Split(' ');
        List<int> defpowers = new List<int>();
        for (int i = 0; i < def.Length; i++)
        { 
            if (!string.IsNullOrEmpty(def[i])) defpowers.Add(int.Parse(def[i]));
        }
        string skill_damages = reader.GetString(reader.GetOrdinal("skill_damages"));
        string[] skill = skill_damages.Split(' ');
        List<int> skill_damages_list = new List<int>();
        for (int i = 0; i < skill.Length; i++)
        { 
            if (!string.IsNullOrEmpty(skill[i])) skill_damages_list.Add(int.Parse(skill[i]));
        }
        string skill_offect_value = reader.GetString(reader.GetOrdinal("skill_offect_value"));
        Dictionary<enum_equip_entry_list, List<int>> skill_offect_value_list = new Dictionary<enum_equip_entry_list, List<int>>();
        string[] skill_offect = skill_offect_value.Split('&');
        for (int i = 0; i < skill_offect.Length; i++)
        {
            if (!string.IsNullOrEmpty(skill_offect[i]))
            {
                string[] skill_offectvalue = skill_offect[i].Split('a');
                if (!string.IsNullOrEmpty(skill_offectvalue[i]))
                {
                    enum_equip_entry_list skill_offectvalue_type = (enum_equip_entry_list)int.Parse(skill_offectvalue[0]);
                    if (!skill_offect_value_list.ContainsKey(skill_offectvalue_type))
                        skill_offect_value_list.Add(skill_offectvalue_type, new List<int>());
                    string[] skill_offectvalue_value = skill_offectvalue[1].Split(' ');
                    for (int j = 0; j < skill_offectvalue_value.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(skill_offectvalue_value[j])) skill_offect_value_list[skill_offectvalue_type].Add(int.Parse(skill_offectvalue_value[j]));
                    }
                }
            }
        }
        int Job = reader.GetInt32(reader.GetOrdinal("Job"));
        int Delay = reader.GetInt32(reader.GetOrdinal("Delay"));
        List<int> skill_up_lv = ArrayHelper.Get_Split<int>(reader.GetString(reader.GetOrdinal("skill_up_lv")), ' ');
        int needlv= reader.GetInt32(reader.GetOrdinal("skill_need_lv"));
        int Weighted = reader.GetInt32(reader.GetOrdinal("Weighted"));
        int MoveType= reader.GetInt32(reader.GetOrdinal("MoveType"));
        List<int> list = ArrayHelper.Get_Split<int>(reader.GetString(reader.GetOrdinal("offset")), ' ');
        int scope = reader.GetInt32(reader.GetOrdinal("scope"));
        string needlvitem = reader.GetString(reader.GetOrdinal("up_need"));
        List<int> needlvitemlist = ArrayHelper.Get_Split<int>(needlvitem, ' ');
        return new db_skill_vo(id, show_name, EffectType, Effect, spellvalues, Power, defpowers, skill_damages_list, skill_offect_value_list, Job, Delay, skill_up_lv, needlv, Weighted, MoveType, list,scope, needlvitemlist);
    }

    public static dream_user_skill_vo ReadUserSkill(MySqlDataReader reader, dream_user_skill_vo item)
    {
        item.Init(
            reader.GetString(reader.GetOrdinal("user_current_skill")),
            reader.GetString(reader.GetOrdinal("user_sum_skill")),
            reader.GetString(reader.GetOrdinal("user_select_skill_type"))
            );
        return item;
    }
    /// <summary>
    /// 读取试练塔
    /// </summary>
    /// <param name="mysqlReader"></param>
    public static void Read_towers(DbDataReader mysqlReader)
    {
        SumSave.crt_Trial_Tower_rank = new mo_world_boss_rank();
        SumSave.crt_Trial_Tower_rank.Ranking_value = mysqlReader.GetString(mysqlReader.GetOrdinal("value"));
        SumSave.crt_Trial_Tower_rank.InitLists();
    }
    /// <summary>
    /// 读取用户排名
    /// </summary>
    /// <param name="reader"></param>
    public static void Read_user_rank(DbDataReader mysqlReader)
    {
        SumSave.user_ranks = new rank_vo();
        SumSave.user_ranks.Ranking_value = mysqlReader.GetString(mysqlReader.GetOrdinal("value"));
        string[] splits = SumSave.user_ranks.Ranking_value.Split(';');
        //读取排行榜
        foreach (string base_value in splits)
        {
            if (base_value != "")
            {
                base_rank_vo bag_Base_VO = new base_rank_vo();
                bag_Base_VO.SetPropertyValue(base_value);
                SumSave.user_ranks.lists.Add(bag_Base_VO);
            }
        }
    }

    public static db_suit_vo Read_suit(MySqlDataReader reader)
    {
        string _suit_name = reader.GetString(reader.GetOrdinal("suit_name"));
        int _suit_number = reader.GetInt32(reader.GetOrdinal("suit_number"));
        int _suit_type = reader.GetInt32(reader.GetOrdinal("suit_type"));
        return new db_suit_vo(_suit_name, _suit_number, _suit_type, reader.GetString(reader.GetOrdinal("suit_value")));
    }

    /// <summary>
    /// 读取无尽塔排行榜
    /// </summary>
    /// <param name="reader"></param>
    public static void Read_user_endless_battle(DbDataReader mysqlReader)
    {
        SumSave.crt_endless_battle = new user_endless_battle();
        SumSave.crt_endless_battle.endless_value = mysqlReader.GetString(mysqlReader.GetOrdinal("value"));
        SumSave.crt_endless_battle.Split_endless();
    }

    public static Dream_User_Hero_VO Read(MySqlDataReader mysqlReader, Dream_User_Hero_VO item)
    {

        item.heroId= mysqlReader.GetString(mysqlReader.GetOrdinal("heroId"));
        item.hero_name= mysqlReader.GetString(mysqlReader.GetOrdinal("hero_name"));
        item.job= mysqlReader.GetInt32(mysqlReader.GetOrdinal("job"));
        item.lv= mysqlReader.GetInt32(mysqlReader.GetOrdinal("lv"));
        item.exp= mysqlReader.GetInt64(mysqlReader.GetOrdinal("exp"));
        List<string> talents = ArrayHelper.Get_Split<string>(mysqlReader.GetString(mysqlReader.GetOrdinal("talent")), ';');
        item.talent = new List<(db_player_talent_vo, int)>();
        for (int i = 0; i < talents.Count; i++)
        {
            string[] split = talents[i].Split(',');
            if (split.Length == 2)
            {
                db_player_talent_vo talent = ArrayHelper.Find(SumSave.db_player_talents, e => e.talent_name == split[0]);
                if (talent != null)
                { 
                    item.talent.Add((talent, int.Parse(split[1])));
                }
            }
        }
        item.SelectPos = mysqlReader.GetInt32(mysqlReader.GetOrdinal("SelectPos"));
        item.zs_lv= mysqlReader.GetInt32(mysqlReader.GetOrdinal("zs_lv"));
        item.user_value= mysqlReader.GetString(mysqlReader.GetOrdinal("user_value"));
        return item;
    }

    public static dream_user_bag_VO Read(MySqlDataReader mysqlReader, dream_user_bag_VO item)
    {
        item.Init(
            mysqlReader.GetString(mysqlReader.GetOrdinal("bag_value")),
            mysqlReader.GetString(mysqlReader.GetOrdinal("resources_value")),
            mysqlReader.GetString(mysqlReader.GetOrdinal("drug_value")),
            mysqlReader.GetInt32(mysqlReader.GetOrdinal("page_value"))
            );
        return item;
    }
    public static dream_user_equip_VO Read(MySqlDataReader mysqlReader, dream_user_equip_VO item)
    {
        item.Init(
            mysqlReader.GetString(mysqlReader.GetOrdinal("equip_value")),
            mysqlReader.GetString(mysqlReader.GetOrdinal("house_value")),
            mysqlReader.GetString(mysqlReader.GetOrdinal("treasure_value")),
            mysqlReader.GetInt32(mysqlReader.GetOrdinal("page_value"))
            );
        return item;
    }
    /// <summary>
    /// 获取写入格式
    /// </summary>
    /// <param name="type"></param>
    /// <param name="tableName"></param>
    /// <param name="sql"></param>
    /// <param name="sql_names"></param>
    /// <returns></returns>
    public static Base_Wirte_VO GetQueue(Mysql_Type type, Mysql_Table_Name tableName, string[] sql, string[] sql_names = null)
    {
        //获取新列表
        Base_Wirte_VO vo = new Base_Wirte_VO();
        vo.type = type;
        vo.tableName = tableName;
        vo.columnNames = sql_names;
        vo.columnValues = sql;
        vo.exist = true;
        return vo;
    }
}