
using CodeStage.AntiCheat.ObscuredTypes;
using Common;
using MVC;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Read_Mysql 
{

    public static db_base_par Read_base_par(MySqlDataReader reader)
    {
        ObscuredInt _index = reader.GetInt32(reader.GetOrdinal("par"));
        DateTime opentime = Convert.ToDateTime(reader.GetString(reader.GetOrdinal("time")));
        ObscuredInt openstate = reader.GetInt32(reader.GetOrdinal("openstate"));
        ObscuredInt device = reader.GetInt32(reader.GetOrdinal("device"));
        string par_name = reader.GetString(reader.GetOrdinal("show_name"));
        return new db_base_par(_index, opentime, openstate, device, par_name);

    }
    public static db_map_vo Read(MySqlDataReader reader)
    {
        return new db_map_vo
            (reader.GetInt32(reader.GetOrdinal("id")),
            reader.GetString(reader.GetOrdinal("map_name")),
            reader.GetInt32(reader.GetOrdinal("map_type")),
            reader.GetInt32(reader.GetOrdinal("map_lv")),
            ArrayHelper.Get_Split<string>(reader.GetString(reader.GetOrdinal("map_monster")), ','),
            ArrayHelper.Get_Split<string>(reader.GetString(reader.GetOrdinal("map_boss")), ','),
            ArrayHelper.Get_Split(reader.GetString(reader.GetOrdinal("map_boss_cdtime")), ','),
            ArrayHelper.Get_Split<float>(reader.GetString(reader.GetOrdinal("map_cd")), ','),
            reader.GetString(reader.GetOrdinal("map_base_drop")),
            reader.GetString(reader.GetOrdinal("map_drop")),
            reader.GetString(reader.GetOrdinal("drop_value")),
            ArrayHelper.Get_Split(reader.GetString(reader.GetOrdinal("map_crate_number_monster")), ','),
            ArrayHelper.Get_Split(reader.GetString(reader.GetOrdinal("map_max_number_monster")), ','),
            ArrayHelper.Get_Split(reader.GetString(reader.GetOrdinal("map_add_number_monster")), ','),
            reader.GetString(reader.GetOrdinal("map_intensity_drop")),
            ArrayHelper.Get_Split(reader.GetString(reader.GetOrdinal("map_crate_boss_condition")), ','),
            ArrayHelper.Get_Split<int>(reader.GetString(reader.GetOrdinal("moeny")), ','),
            reader.GetString(reader.GetOrdinal("map_lv_drop")),
            reader.GetString(reader.GetOrdinal("map_boss_lv_drop")),
            reader.GetString(reader.GetOrdinal("map_lv_intensity_drop")) 
            );
    }
    public static db_dec Read_dec(MySqlDataReader reader)
    {
        string _panel_index = reader.GetString(reader.GetOrdinal("panel_index"));
        string _title = reader.GetString(reader.GetOrdinal("title"));
        string _dec = reader.GetString(reader.GetOrdinal("dec"));
        return new db_dec(_panel_index, _title, _dec);
    }
    public static Bag_Base_VO Read_stditems(MySqlDataReader reader)
    {
        Bag_Base_VO item = new Bag_Base_VO(
            reader.GetInt32(reader.GetOrdinal("hp")),
            reader.GetInt32(reader.GetOrdinal("mp")),
            reader.GetInt32(reader.GetOrdinal("ac")),
            reader.GetInt32(reader.GetOrdinal("ac2")),
            reader.GetInt32(reader.GetOrdinal("mac")),
            reader.GetInt32(reader.GetOrdinal("mac2")),
            reader.GetInt32(reader.GetOrdinal("dc")),
            reader.GetInt32(reader.GetOrdinal("dc2")),
            reader.GetInt32(reader.GetOrdinal("sc")),
            reader.GetInt32(reader.GetOrdinal("sc2")),
            reader.GetInt32(reader.GetOrdinal("mc")),
            reader.GetInt32(reader.GetOrdinal("mc2"))  
            );
        item.Name = reader.GetString(reader.GetOrdinal("Name"));
        item.show_name = reader.GetString(reader.GetOrdinal("show_name"));
        item.StdMode = reader.GetString(reader.GetOrdinal("StdMode"));
        item.Shape= reader.GetInt32(reader.GetOrdinal("Shape"));
        item.need_lv = reader.GetInt32(reader.GetOrdinal("need_lv"));
        item.price = reader.GetInt32(reader.GetOrdinal("price"));
        item.job= reader.GetInt32(reader.GetOrdinal("job"));
        item.suit = reader.GetInt32(reader.GetOrdinal("suit"));  
        item.dec = reader.GetString(reader.GetOrdinal("dec"));
        return item;
    }
    public static db_Hero_VO Read_Hero(MySqlDataReader reader)
    {
        /*
         * public readonly ObscuredInt id;
    public readonly string type;
    public readonly ObscuredInt inithp;
    public readonly ObscuredInt hp;
    public readonly ObscuredInt uphp;
    public readonly ObscuredInt initmp;
    public readonly ObscuredInt mp;
    public readonly ObscuredInt upmp;
    public readonly ObscuredInt initac;
    public readonly ObscuredInt ac;
    public readonly ObscuredInt upac;
    public readonly ObscuredInt initmac;
    public readonly ObscuredInt mac;
    public readonly ObscuredInt upmac;
    public readonly ObscuredInt initdc;
    public readonly ObscuredInt dc;
    public readonly ObscuredInt updc;
    public readonly ObscuredInt initdc2;
    public readonly ObscuredInt dc2;
    public readonly ObscuredInt updc2;
    public readonly ObscuredInt initsc;
    public readonly ObscuredInt sc;
    public readonly ObscuredInt upsc;
    public readonly ObscuredInt initsc2;
    public readonly ObscuredInt sc2;
    public readonly ObscuredInt upsc2;
    public readonly ObscuredInt initmc;
    public readonly ObscuredInt mc;
    public readonly ObscuredInt upmc;
    public readonly ObscuredInt initmc2;
    public readonly ObscuredInt mc2;
    public readonly ObscuredInt upmc2;
    public readonly ObscuredInt inithit;
    public readonly ObscuredInt hit;
    public readonly ObscuredInt uphit;
    public readonly ObscuredInt initdodge;
    public readonly ObscuredInt dodge;
    public readonly ObscuredInt updodge;
    public readonly ObscuredInt initcrit;
    public readonly ObscuredInt crit;
    public readonly ObscuredInt upcrit;
    public readonly ObscuredInt initcritDmg;
    public readonly ObscuredInt critDmg;
    public readonly ObscuredInt upcritDmg;
    public readonly ObscuredInt initmac2;
    public readonly ObscuredInt mac2;
    public readonly ObscuredInt upmac2;
    public readonly ObscuredInt initac2;
    public readonly ObscuredInt ac2;
    public readonly ObscuredInt upac2;
    public readonly ObscuredInt initspeed;
    public readonly ObscuredInt speed;
    public readonly ObscuredInt upspeed;
    public readonly ObscuredInt intrange;
    public readonly ObscuredInt range;
    public readonly ObscuredInt uprange;

         */
        return new db_Hero_VO(reader.GetInt32(reader.GetOrdinal("id")), 
            reader.GetString(reader.GetOrdinal("type")),
            reader.GetInt32(reader.GetOrdinal("inithp")),
            reader.GetInt32(reader.GetOrdinal("hp")),
            reader.GetInt32(reader.GetOrdinal("uphp")),
            reader.GetInt32(reader.GetOrdinal("initmp")),
            reader.GetInt32(reader.GetOrdinal("mp")),
            reader.GetInt32(reader.GetOrdinal("upmp")),
            reader.GetInt32(reader.GetOrdinal("initac")),
            reader.GetInt32(reader.GetOrdinal("ac")),
            reader.GetInt32(reader.GetOrdinal("upac")),
            reader.GetInt32(reader.GetOrdinal("initmac")),
            reader.GetInt32(reader.GetOrdinal("mac")),
            reader.GetInt32(reader.GetOrdinal("upmac")),
            reader.GetInt32(reader.GetOrdinal("initdc")),
            reader.GetInt32(reader.GetOrdinal("dc")),
            reader.GetInt32(reader.GetOrdinal("updc")),
            reader.GetInt32(reader.GetOrdinal("initdc2")),
            reader.GetInt32(reader.GetOrdinal("dc2")),
            reader.GetInt32(reader.GetOrdinal("updc2")),
            reader.GetInt32(reader.GetOrdinal("initsc")),
            reader.GetInt32(reader.GetOrdinal("sc")),
            reader.GetInt32(reader.GetOrdinal("upsc")),
            reader.GetInt32(reader.GetOrdinal("initsc2")),
            reader.GetInt32(reader.GetOrdinal("sc2")),
            reader.GetInt32(reader.GetOrdinal("upsc2")),
            reader.GetInt32(reader.GetOrdinal("initmc")),
            reader.GetInt32(reader.GetOrdinal("mc")),
            reader.GetInt32(reader.GetOrdinal("upmc")),
            reader.GetInt32(reader.GetOrdinal("initmc2")),
            reader.GetInt32(reader.GetOrdinal("mc2")),
            reader.GetInt32(reader.GetOrdinal("upmc2")),
            reader.GetInt32(reader.GetOrdinal("inithit")),
            reader.GetInt32(reader.GetOrdinal("hit")),
            reader.GetInt32(reader.GetOrdinal("uphit")),
            reader.GetInt32(reader.GetOrdinal("initdodge")),
            reader.GetInt32(reader.GetOrdinal("dodge")),
            reader.GetInt32(reader.GetOrdinal("updodge")),
            reader.GetInt32(reader.GetOrdinal("initcrit")),
            reader.GetInt32(reader.GetOrdinal("crit")),
            reader.GetInt32(reader.GetOrdinal("upcrit")),
            reader.GetInt32(reader.GetOrdinal("initcritDmg")),
            reader.GetInt32(reader.GetOrdinal("critDmg")),
            reader.GetInt32(reader.GetOrdinal("upcritDmg")),
            reader.GetInt32(reader.GetOrdinal("initmac2")),
            reader.GetInt32(reader.GetOrdinal("mac2")),
            reader.GetInt32(reader.GetOrdinal("upmac2")),
            reader.GetInt32(reader.GetOrdinal("initac2")),
            reader.GetInt32(reader.GetOrdinal("ac2")),
            reader.GetInt32(reader.GetOrdinal("upac2")),
            reader.GetInt32(reader.GetOrdinal("initspeed")),
            reader.GetInt32(reader.GetOrdinal("speed")),
            reader.GetInt32(reader.GetOrdinal("upspeed")),
            reader.GetInt32(reader.GetOrdinal("intrange")),
            reader.GetInt32(reader.GetOrdinal("range")),
            reader.GetInt32(reader.GetOrdinal("uprange")),
            reader.GetString(reader.GetOrdinal("initskill")));

    }

}
