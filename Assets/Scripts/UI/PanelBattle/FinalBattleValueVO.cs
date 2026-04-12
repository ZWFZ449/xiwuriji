using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public readonly struct FinalBattleValueVO 
{
    //测试2
    /// <summary> 
    /// 最终属性
    /// </summary>
    public readonly long battle_maxhp;
    public readonly int battle_maxmp;

    /// <summary>
    /// 基础属性
    /// </summary>
    public readonly long hp, mp;
    public readonly int dc, dc2, mac, mac2, ac, ac2, sc, sc2, mc, mc2;
    /// <summary>
    /// 二级属性
    /// </summary>
    public readonly int hit, dodge, crit, critDmg;
    /// <summary>
    /// 百分比属性
    /// </summary>
    public readonly int battle_hp, battle_mp, battle_ac, battle_mac, battle_dc, battle_sc, battle_mc, battle_speed, battle_range, battle_Damage, battle_def;
    /// <summary>
    /// 移动速度
    /// </summary>
    public readonly int move_speed;
    /// <summary>
    /// 回复
    /// </summary>
    public readonly int hpRegen, mpRegen;
    /// <summary>
    /// buff效果
    /// </summary>
    public readonly List<(enum_battle_pet_talent_list,int,int)> buffList;
    /// <summary>
    /// 幸运
    /// </summary>
    public readonly int lucky, damage_reduction, magic_damage_reduction;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="battle_maxhp">最终生命</param>
    /// <param name="battle_maxmp">最终魔法值</param>
    /// <param name="hp">加成前生命</param>
    /// <param name="mp">加成前魔法</param>
    /// <param name="dc">物理攻击下</param>
    /// <param name="dc2">物理攻击上</param>
    /// <param name="mac">魔法防御下</param>
    /// <param name="mac2">魔法防御上</param>
    /// <param name="ac">物理防御下</param>
    /// <param name="ac2">物理防御上</param>
    /// <param name="sc">魔法攻击</param>
    /// <param name="sc2">魔法攻击上</param>
    /// <param name="mc">道术攻击</param>
    /// <param name="mc2">道术攻击上</param>
    /// <param name="hit">命中初始100每一级减少1点</param>
    /// <param name="dodge">躲避</param>
    /// <param name="crit">暴击率</param>
    /// <param name="critDmg">暴击伤害</param>
    /// <param name="hpRegen">回血</param>
    /// <param name="mpRegen">回蓝</param>
    /// <param name="battle_hp">生命%</param>
    /// <param name="battle_mp">魔法%</param>
    /// <param name="battle_ac">物防%</param>
    /// <param name="battle_mac">魔防%</param>
    /// <param name="battle_dc">物攻%</param>
    /// <param name="battle_sc">魔攻%</param>
    /// <param name="battle_mc">道攻%</param>
    /// <param name="battle_speed">攻击速度初始180</param>
    /// <param name="battle_range">攻击范围</param>
    /// <param name="battle_Damage">真伤</param>
    /// <param name="battle_def">吸伤</param>
    /// <param name="buffList">铭文</param>
    /// <param name="lucky">幸运</param>
    /// <param name="damage_reduction">伤害减免</param>
    /// <param name="magic_damage_reduction">魔法伤害减免</param>
    public FinalBattleValueVO(long battle_maxhp, int battle_maxmp, long hp, int mp, int dc, int dc2, int mac,
        int mac2, int ac, int ac2, int sc, int sc2, int mc, int mc2, int hit, int dodge, int crit, int critDmg, int hpRegen, int mpRegen,
        int battle_hp, int battle_mp, int battle_ac, int battle_mac, int battle_dc, int battle_sc, int battle_mc, int battle_speed, int battle_range, int battle_Damage, int battle_def,
        List<(enum_battle_pet_talent_list, int, int)> buffList,int lucky,int damage_reduction, int magic_damage_reduction,int move_speed)
    { 
        this.battle_maxhp = battle_maxhp;
        this.battle_maxmp = battle_maxmp;
        this.hp = hp;
        this.mp = mp;
        this.dc = dc;
        this.dc2 = dc2;
        this.mac = mac;
        this.mac2 = mac2;
        this.ac = ac;
        this.ac2 = ac2;
        this.sc = sc;
        this.sc2 = sc2;
        this.mc = mc;
        this.mc2 = mc2;
        this.hit = hit;
        this.dodge = dodge;
        this.crit = crit;
        this.critDmg = critDmg;
        this.hpRegen = hpRegen;
        this.mpRegen = mpRegen;
        this.battle_hp = battle_hp;
        this.battle_mp = battle_mp;
        this.battle_ac = battle_ac;
        this.battle_mac = battle_mac;
        this.battle_dc = battle_dc;
        this.battle_sc = battle_sc;
        this.battle_mc = battle_mc;
        this.battle_speed = battle_speed;
        this.battle_range = battle_range;
        this.battle_Damage = battle_Damage;
        this.battle_def = battle_def;
        this.buffList = buffList;
        this.lucky = lucky;
        this.damage_reduction = damage_reduction;
        this.magic_damage_reduction = magic_damage_reduction;
        this.move_speed = move_speed;
    }
}


public enum Battle_Game_Type
{ 
    player,//玩家
    call,//召唤兽
    monster,//怪物
    Boss,//boss
    Activity_Monster,//活动怪物
}


public enum Hero_Type
{ 
平民,
战士,
法师,
道士,
}

public enum enum_battle_pet_talent_list
{ 
    任意门,
    嗜血追击,
    破壁一击,
    华山斩,
    斩杀,
    zero,
    连击效果,
    连击,
    法连,
    道连,
    战旗,
    法旗,
    道旗,
    反震,
    防爆,
    招架,
    反弹,
    慧根
}

