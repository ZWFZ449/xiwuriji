
public class crtMaxBattleVO
{
    public int id;
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
    public long exp;
    /// <summary>
    /// 等级
    /// </summary>
    public int lv;
    /// <summary>
    /// 经验加成
    /// </summary>
    public readonly int exp_bonus;
    /// <summary>
    /// 金币加成
    /// </summary>
    public readonly int gold_bonus;
    /// <summary>
    /// 掉落加成
    /// </summary>
    public readonly int drop_bonus;
    /// <summary>
    /// 品质加成
    /// </summary>
    public readonly int quality_bonus;

    public readonly int boss_cd;

    /// <summary>
    /// 属性
    /// </summary>
    public FinalBattleValueVO data;
    public crtMaxBattleVO(int exp_bonus, int gold_bonus, int drop_bonus ,int quality_bonus ,int boss_cd = 0)
    { 
        this.exp_bonus= exp_bonus;
        this.gold_bonus = gold_bonus;
        this.drop_bonus = drop_bonus;
        this.quality_bonus = quality_bonus;
        this.boss_cd = boss_cd;
    }
}
