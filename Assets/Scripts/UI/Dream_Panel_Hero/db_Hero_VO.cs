using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class db_Hero_VO 
{
    public readonly int id;
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
    public readonly int initrange;
    public readonly int range;
    public readonly int uprange;
    public readonly string initskill;

    public db_Hero_VO(int id, string type, int inithp, int hp, int uphp, int initmp, int mp,
        int upmp, int initac, int ac, int upac, int initmac, int mac, int upmac, int initdc,
        int dc, int updc, int initdc2, int dc2, int updc2, int initsc, int sc, int upsc,
        int initsc2, int sc2, int upsc2, int initmc, int mc, int upmc, int initmc2, int mc2,
        int upmc2, int inithit, int hit, int uphit, int initdodge, int dodge, int updodge,
        int initcrit, int crit, int upcrit, int initcritDmg, int critDmg, int upcritDmg,
        int initmac2, int mac2, int upmac2, int initac2, int ac2, int upac2, int initspeed,
        int speed, int upspeed, int initrange, int range, int uprange,string initskill)
    { 
        this.id = id;
        this.type = type;
        this.inithp = inithp;
        this.hp = hp;
        this.uphp = uphp;
        this.initmp = initmp;
        this.mp = mp;
        this.upmp = upmp;
        this.initac = initac;
        this.ac = ac;
        this.upac = upac;
        this.initmac = initmac;
        this.mac = mac;
        this.upmac = upmac;
        this.initdc = initdc;
        this.dc = dc;
        this.updc = updc;
        this.initdc2 = initdc2;
        this.dc2 = dc2;
        this.updc2 = updc2;
        this.initsc = initsc;
        this.sc = sc;
        this.upsc = upsc;
        this.initsc2 = initsc2;
        this.sc2 = sc2;
        this.upsc2 = upsc2;
        this.initmc = initmc;
        this.mc = mc;
        this.upmc = upmc;
        this.initmc2 = initmc2;
        this.mc2 = mc2;
        this.upmc2 = upmc2;
        this.inithit = inithit;
        this.hit = hit;
        this.uphit = uphit;
        this.initdodge = initdodge;
        this.dodge = dodge;
        this.updodge = updodge;
        this.initcrit = initcrit;
        this.crit = crit;
        this.upcrit = upcrit;
        this.initcritDmg = initcritDmg;
        this.critDmg = critDmg;
        this.upcritDmg = upcritDmg;
        this.initmac2 = initmac2;
        this.mac2 = mac2;
        this.upmac2 = upmac2;
        this.initac2 = initac2;
        this.ac2 = ac2;
        this.upac2 = upac2;
        this.initspeed = initspeed;
        this.speed = speed;
        this.upspeed = upspeed;
        this.initrange = initrange;
        this.range = range;
        this.uprange = uprange;
        this.initskill = initskill;

    }
}
