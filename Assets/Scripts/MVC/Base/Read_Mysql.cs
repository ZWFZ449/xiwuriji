
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
        int _index = reader.GetInt32(reader.GetOrdinal("par"));
        DateTime opentime = Convert.ToDateTime(reader.GetString(reader.GetOrdinal("time")));
        int openstate = reader.GetInt32(reader.GetOrdinal("openstate"));
        int device = reader.GetInt32(reader.GetOrdinal("device"));
        string par_name = reader.GetString(reader.GetOrdinal("show_name"));
        return new db_base_par(_index, opentime, openstate, device, par_name);

    }
    public static db_map_vo Read(MySqlDataReader reader)
    {
        //reader.GetInt32(reader.GetOrdinal("id"));
        //Debug.Log(reader.GetString(reader.GetOrdinal("map_name")));
        //    reader.GetInt32(reader.GetOrdinal("map_type"));
        //    reader.GetInt32(reader.GetOrdinal("map_lv"));
        //ArrayHelper.Get_Split<string>(reader.GetString(reader.GetOrdinal("map_monster")), ',');
        //ArrayHelper.Get_Split<string>(reader.GetString(reader.GetOrdinal("map_boss")), ',');
        //    ArrayHelper.Get_Split<int>(reader.GetString(reader.GetOrdinal("map_boss_cdtime")), ',');
        //ArrayHelper.Get_Split<float>(reader.GetString(reader.GetOrdinal("map_cd")), ',');
        //reader.GetString(reader.GetOrdinal("map_base_drop"));
        //reader.GetString(reader.GetOrdinal("map_drop"));
        //reader.GetString(reader.GetOrdinal("drop_value"));
        //ArrayHelper.Get_Split<int>(reader.GetString(reader.GetOrdinal("map_crate_number_monster")), ',');
        //ArrayHelper.Get_Split<int>(reader.GetString(reader.GetOrdinal("map_max_number_monster")), ',');
        //ArrayHelper.Get_Split<int>(reader.GetString(reader.GetOrdinal("map_add_number_monster")), ',');
        //reader.GetString(reader.GetOrdinal("map_intensity_drop"));
        //ArrayHelper.Get_Split<int>(reader.GetString(reader.GetOrdinal("map_crate_boss_condition")), ',');
        return new db_map_vo
            (reader.GetInt32(reader.GetOrdinal("id")),
            reader.GetString(reader.GetOrdinal("map_name")),
            reader.GetInt32(reader.GetOrdinal("map_type")),
            reader.GetInt32(reader.GetOrdinal("map_lv")),
            ArrayHelper.Get_Split<string>(reader.GetString(reader.GetOrdinal("map_monster")), ','),
            ArrayHelper.Get_Split<string>(reader.GetString(reader.GetOrdinal("map_boss")), ','),
            ArrayHelper.Get_Split<int>(reader.GetString(reader.GetOrdinal("map_boss_cdtime")), ','),
            ArrayHelper.Get_Split<float>(reader.GetString(reader.GetOrdinal("map_cd")), ','),
            reader.GetString(reader.GetOrdinal("map_base_drop")),
            reader.GetString(reader.GetOrdinal("map_drop")),
            reader.GetString(reader.GetOrdinal("drop_value")),
            ArrayHelper.Get_Split<int>(reader.GetString(reader.GetOrdinal("map_crate_number_monster")), ','),
            ArrayHelper.Get_Split<int>(reader.GetString(reader.GetOrdinal("map_max_number_monster")), ','),
            ArrayHelper.Get_Split<int>(reader.GetString(reader.GetOrdinal("map_add_number_monster")), ','),
            reader.GetString(reader.GetOrdinal("map_intensity_drop")),
            ArrayHelper.Get_Split<int>(reader.GetString(reader.GetOrdinal("map_crate_boss_condition")), ',')

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
         * public readonly int id;
    public readonly string type;
    public readonly int inithp;
    public readonly int hp;
    public readonly int uphp;
    public readonly int initmp;
    public readonly int mp;
    public readonly int upmp;
    public readonly int initac;
    public readonly int ac;
    public readonly int upac;
    public readonly int initmac;
    public readonly int mac;
    public readonly int upmac;
    public readonly int initdc;
    public readonly int dc;
    public readonly int updc;
    public readonly int initdc2;
    public readonly int dc2;
    public readonly int updc2;
    public readonly int initsc;
    public readonly int sc;
    public readonly int upsc;
    public readonly int initsc2;
    public readonly int sc2;
    public readonly int upsc2;
    public readonly int initmc;
    public readonly int mc;
    public readonly int upmc;
    public readonly int initmc2;
    public readonly int mc2;
    public readonly int upmc2;
    public readonly int inithit;
    public readonly int hit;
    public readonly int uphit;
    public readonly int initdodge;
    public readonly int dodge;
    public readonly int updodge;
    public readonly int initcrit;
    public readonly int crit;
    public readonly int upcrit;
    public readonly int initcritDmg;
    public readonly int critDmg;
    public readonly int upcritDmg;
    public readonly int initmac2;
    public readonly int mac2;
    public readonly int upmac2;
    public readonly int initac2;
    public readonly int ac2;
    public readonly int upac2;
    public readonly int initspeed;
    public readonly int speed;
    public readonly int upspeed;
    public readonly int intrange;
    public readonly int range;
    public readonly int uprange;

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
