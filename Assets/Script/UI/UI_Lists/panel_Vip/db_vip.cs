using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class db_vip : Base_VO
{
    /// <summary>
    /// VIP等级
    /// </summary>
    public readonly int vip_lv;
    public readonly string vip_name;
    /// <summary>
    /// VIP经验
    /// </summary>
    public readonly int vip_exp;
    /// <summary>
    /// 经验加成 100
    /// </summary>
    public readonly int experienceBonus;
    /// <summary>
    /// 灵珠收益 106
    /// </summary>
    public readonly int lingzhuIncome;
    /// <summary>
    /// 装备爆率 107
    /// </summary>
    public readonly int equipmentExplosionRate;
    /// <summary>
    /// 人物历练 103
    /// </summary>
    public readonly int characterExperience;
    /// <summary>
    /// 寻怪间隔 116
    /// </summary>
    public readonly int monsterHuntingInterval;
    /// <summary>
    /// 生命回复 8
    /// </summary>
    public readonly int hpRecovery;
    /// <summary>
    /// 法力回复 9
    /// </summary>
    public readonly int manaRegeneration;
    /// <summary>
    /// 幸运 15
    /// </summary>
    public readonly int goodFortune;
    /// <summary>
    /// 强化费用 520
    /// </summary>
    public readonly int strengthenCosts;
    /// <summary>
    /// 离线间隔 521
    /// </summary>
    public readonly int offlineInterval;
    /// <summary>
    /// 签到收益 522
    /// </summary>
    public readonly int signInIncome;
    /// <summary>
    /// 鞭尸(双倍奖励) 506 
    /// </summary>
    public readonly int whippingCorpses;
    /// <summary>
    /// 灵气上限 508
    /// </summary>
    public readonly int upperLimitOfSpiritualEnergy;

    public db_vip(int vip_lv, string vip_name, int vip_exp, int experienceBonus, int lingzhuIncome, int equipmentExplosionRate, int characterExperience, int monsterHuntingInterval, int hpRecovery, int manaRegeneration, int goodFortune, int strengthenCosts, int offlineInterval, int signInIncome, int whippingCorpses, int upperLimitOfSpiritualEnergy)
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
        this.upperLimitOfSpiritualEnergy = upperLimitOfSpiritualEnergy;
    }
}
