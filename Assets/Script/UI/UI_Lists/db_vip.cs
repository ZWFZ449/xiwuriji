using CodeStage.AntiCheat.ObscuredTypes;
using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class db_vip : Base_VO
{
    /// <summary>
    /// VIP等级
    /// </summary>
    public readonly ObscuredInt  vip_lv;
    public readonly string vip_name;
    /// <summary>
    /// VIP经验
    /// </summary>
    public readonly ObscuredInt  vip_exp;
    /// <summary>
    /// 经验加成 100
    /// </summary>
    public readonly ObscuredInt  experienceBonus;
    /// <summary>
    /// 金币收益 106
    /// </summary>
    public readonly ObscuredInt  lingzhuIncome;
    /// <summary>
    /// 装备爆率 107
    /// </summary>
    public readonly ObscuredInt  equipmentExplosionRate;
    /// <summary>
    /// boss卷轴 103
    /// </summary>
    public readonly ObscuredInt  characterExperience;
    /// <summary>
    /// boss刷新减少 116
    /// </summary>
    public readonly ObscuredInt  monsterHuntingInterval;
    /// <summary>
    /// 生命回复 8
    /// </summary>
    public readonly ObscuredInt  hpRecovery;
    /// <summary>
    /// 法力回复 9
    /// </summary>
    public readonly ObscuredInt  manaRegeneration;
    /// <summary>
    /// 幸运 15
    /// </summary>
    public readonly ObscuredInt  goodFortune;
    /// <summary>
    /// 强化费用 520
    /// </summary>
    public readonly ObscuredInt  strengthenCosts;
    /// <summary>
    /// 离线间隔 521
    /// </summary>
    public readonly ObscuredInt  offlineInterval;
    /// <summary>
    /// 签到收益 522
    /// </summary>
    public readonly ObscuredInt  signInIncome;
    /// <summary>
    /// 鞭尸(双倍奖励) 506 
    /// </summary>
    public readonly ObscuredInt  whippingCorpses;
    /// <summary>
    /// 灵气上限 508
    /// </summary>
    public readonly string gift_value;

    public db_vip(ObscuredInt  vip_lv, string vip_name, ObscuredInt  vip_exp, ObscuredInt  experienceBonus, ObscuredInt  lingzhuIncome, ObscuredInt  equipmentExplosionRate, ObscuredInt  characterExperience, ObscuredInt  monsterHuntingInterval, ObscuredInt  hpRecovery, ObscuredInt  manaRegeneration, ObscuredInt  goodFortune, ObscuredInt  strengthenCosts, ObscuredInt  offlineInterval, ObscuredInt  signInIncome, ObscuredInt  whippingCorpses, string gift_value)
    {
        this.vip_lv = vip_lv;
        this.vip_name = vip_name;
        this.vip_exp = vip_exp;
        this.experienceBonus = experienceBonus;
        this.lingzhuIncome = lingzhuIncome;
        this.equipmentExplosionRate = equipmentExplosionRate;
        this.characterExperience = characterExperience;
        this.monsterHuntingInterval = monsterHuntingInterval;
        this.hpRecovery = hpRecovery;
        this.manaRegeneration = manaRegeneration;
        this.goodFortune = goodFortune;
        this.strengthenCosts = strengthenCosts;
        this.offlineInterval = offlineInterval;
        this.signInIncome = signInIncome;
        this.whippingCorpses = whippingCorpses;
        this.gift_value = gift_value;
    }
}
