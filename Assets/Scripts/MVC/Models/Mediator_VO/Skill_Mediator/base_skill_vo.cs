using MVC;
using System.Collections.Generic;

public class base_skill_vo : Base_VO
{
    private string User_value;
    public string user_value
    {
        get { return User_value; }

        set { User_value = value; }
    }
    /// <summary>
    /// 技能名称
    /// </summary>
    public string skillname;
    /// <summary>
    /// J技能等级
    /// </summary>
    public int skilllv;
    /// <summary>
    /// 上阵位置
    /// </summary>
    public int skillpos;

    public int skillinternalforceMP;
    /// <summary>
    /// 技能类型 1战斗2秘笈3特殊
    /// </summary>
    public int skill_type;
    /// <summary>
    /// 技能伤害类型 1物理2魔法3真伤4辅助护盾6回血7回蓝
    /// </summary>
    public int skill_damage_type;
    /// <summary>
    /// 技能最大等级
    /// </summary>
    public int skill_max_lv;
    /// <summary>
    /// 五行
    /// </summary>
    public int skill_life;
    /// <summary>
    /// 技能升级系数 [0]*mathf.pow([1],[2])
    /// </summary>
    public List<int> skill_need_coefficient;
    /// <summary>
    /// 技能激活效果 等级+技能名称 *分隔
    /// </summary>
    public List<(int,string)> skill_need_state;
    /// <summary>
    /// 激活附带效果类型
    /// 1生命值
    /// 2魔法值
    /// 3内力值
    /// 4物理攻击
    /// 5魔法攻击
    /// 6物理防御
    /// 7魔法防御
    /// 8攻击速度
    /// 9暴击率
    /// 10躲避
    /// </summary>
    public List<int> skill_open_type;
    /// <summary>
    /// 对应激活值
    /// </summary>
    public List<int> skill_open_value;
    /// <summary>
    /// 上阵效果 同激活效果
    /// </summary>
    public List<int> skill_pos_type;
    /// <summary>
    /// 上阵效果值
    /// </summary>
    public List<int> skill_pos_value;
    /// <summary>
    /// 技能伤害
    /// </summary>
    public int skill_damage;
    /// <summary>
    /// 技能升级增加伤害
    /// </summary>
    public int skill_power;
    /// <summary>
    /// 消耗法力百分比
    /// </summary>
    public int skill_spell=7;
    /// <summary>
    /// 技能cd
    /// </summary>
    public float skill_cd;
    /// <summary>
    /// 战斗cd
    /// </summary>
    public float battle_CD;
    /// <summary>
    /// 技能套装类型
    /// </summary>
    public int skill_suit_type;
    /// <summary>
    /// 技能套装效果
    /// </summary>
    public int skill_suit_value;
    /// <summary>
    /// 1技能名称 2技能等级 3技能位置 4技能内力 5技能类型 6技能伤害类型 7技能最大等级 8技能初始化升级经验 9技能升级
    /// </summary>
    public string[] user_values;
    /// <summary>
    /// 技能释放效果激活所在位置是移动攻击 还是在目标身上释放
    /// </summary>
    public int skill_state;

}
