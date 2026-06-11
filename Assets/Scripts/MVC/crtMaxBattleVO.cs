
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections.Generic;

public class crtMaxBattleVO
{
    public ObscuredInt  id;
    /// <summary>
    /// 名称
    /// </summary>
    public string crt_name;

    public Hero_Type hero_type;
    /// <summary>
    /// 职业
    /// </summary>
    public Battle_Game_Type type;
    /// <summary>
    /// 经验值
    /// </summary>
    public ObscuredLong exp;
    /// <summary>
    /// 等级
    /// </summary>
    public ObscuredInt lv;
    /// <summary>
    /// 经验加成
    /// </summary>
    public readonly ObscuredInt  exp_bonus;
    /// <summary>
    /// 金币加成
    /// </summary>
    public readonly ObscuredInt  gold_bonus;
    /// <summary>
    /// 掉落加成
    /// </summary>
    public readonly ObscuredInt  drop_bonus;
    /// <summary>
    /// 品质加成
    /// </summary>
    public readonly ObscuredInt  quality_bonus;

    public readonly ObscuredInt  boss_cd;
    /// <summary>
    /// 携带技能
    /// </summary>
    public int skill_id;
    /// <summary>
    /// 弹道数量
    /// </summary>
    public int skill_number;
    /// <summary>
    /// 技能等级
    /// </summary>
    public int skill_level;
    /// <summary>
    /// 属性
    /// </summary>
    public FinalBattleValueVO data;

    public Dictionary<enum_talent_offect_list, int> hero_talentList;
    public crtMaxBattleVO(ObscuredInt  exp_bonus, ObscuredInt  gold_bonus, ObscuredInt  drop_bonus ,ObscuredInt  quality_bonus ,ObscuredInt  boss_cd)
    { 
        this.exp_bonus= exp_bonus;
        this.gold_bonus = gold_bonus;
        this.drop_bonus = drop_bonus;
        this.quality_bonus = quality_bonus;
        this.boss_cd = boss_cd;
    }
}
