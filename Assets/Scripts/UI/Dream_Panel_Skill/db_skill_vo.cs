using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
     * 说明文档
         *EffectType 1 单体 2群体 3自身buff 4治疗
         * effect 2.1 攻击1个，2.2攻击2个，运动攻击 定点攻击
         *        3.1攻击1次，3.2 攻击2次。
         *        4.1治疗自身 4.2治疗全体
         *        5.1增加物攻 5.2增加魔攻5.3增加物防 5.4增加魔防 5.5增加速度 5.6 增加命中 5.7增加闪避5.8增加精灵出现亲密值5.9 增加双防
         *        6.1 单个持续伤害 6.2
         *        7.1 火群伤害
         *        被动加成
         *        4级技能属性：NeedL1 种类（属性加成,判定加成,持续时间,命中加成,异常状态,buff加成,宝石加成,组件加成,攻击范围,攻速加成,优先级）
         *                   NeedL2 细分（力量6项属性/特性18项/状态等）
         *                   
         *                   NeedL3 值
         *
         *  /*种类 概率
         * 1.基础伤害 攻防 生命回复 异常状态抗性 50%
         * 2. 18项特性 加权  20%
         * 3.属性加成 6属性  3%
         * 4.命中 躲避 暴击  10%
         * 5.暴击伤害 攻击速度 5% 
         * 6.6异常状态概率   5%
         * 7.6异常状态持续时间  5%
         * 8.技能攻击多次 技能攻击多个 全体技能攻击多次 攻击多个 1%
         * 9.技能伤害 全体技能伤害 0.8%
         * 10.优先级 0.1%
         * 11.传说度 0.1%
         * 12 组件加成
         * 13 宝石加成
         */
public class db_skill_vo : Base_VO
{
    public readonly int id;
    /// <summary>
    /// 展示名称
    /// </summary>
    public readonly string show_name;
    /// <summary>
    /// 技能类型
    /// </summary>
    public readonly int EffectType;
    /// <summary>
    /// 技能种类
    /// </summary>
    public readonly int Effect;
    /// <summary>
    /// 消耗魔法列表
    /// </summary>
    public readonly List<int> spells;//消耗列表s
    /// <summary>
    /// 基本伤害
    /// </summary>
    public readonly int Power;//基本威力
    /// <summary>
    /// 升级加成
    /// </summary>
    public readonly List<int> DefPowers;//升级加成
    /// <summary>
    /// 技能携带真实伤害/伤害吸收
    /// </summary>
    public readonly List<int> skill_damages;
    /// <summary>
    /// 技能特殊效果
    /// </summary>
    public readonly Dictionary<enum_equip_entry_list,List<int>> skill_offect_value_list;
    /// <summary>
    /// 职业
    /// </summary>
    public readonly int Job;
    /// <summary>
    /// 技能移动类型
    /// </summary>
    public readonly int MoveType;
    /// <summary>
    /// 技能cd
    /// </summary>
    public readonly int Delay;
    /// <summary>
    /// 技能权重
    /// </summary>
    public readonly int Weighted;
    /// <summary>
    /// 技能升级所需等级
    /// </summary>
    public readonly int need_lv;
    /// <summary>
    /// 技能升级所需经验
    /// </summary>
    public readonly List<int> skill_up_lv;
    /// <summary>
    /// 技能效果偏移量
    /// </summary>
    public readonly List<int> offset;
    /// <summary>
    ///  自身数据
    /// </summary>
    private int lv, exp, select_pos;
    /// <summary>
    /// 附加效果
    /// </summary>
    private Dictionary<enum_talent_offect_list, int> bufflist = new Dictionary<enum_talent_offect_list, int>();
    /// <summary>
    /// 攻击范围
    /// </summary>
    public readonly int scope;

    public readonly List<int> needLvitem;
    /// <summary>
    /// 技能消耗魔法
    /// </summary>
    public int Get_Mp { get { return spells[SetLv() >= 0 ? SetLv() : 0]; ; } }

    public db_skill_vo(int id, string show_name, int EffectType, int Effect, List<int> spells, int Power,
        List<int> DefPowers, List<int> skill_damages, Dictionary<enum_equip_entry_list, List<int>> skill_offect_value, int Job, int Delay,List<int> skill_up_lv,int need_lv,int Weighted,int MoveType,
        List<int> offset,int scope,List<int> needLvitem)
    { 
        this.id = id;
        this.show_name = show_name;
        this.EffectType = EffectType; 
        this.Effect = Effect;
        this.spells = spells;
        this.Power = Power;
        this.DefPowers = DefPowers;
        this.skill_damages = skill_damages;
        this.skill_offect_value_list = skill_offect_value;
        this.Job = Job;
        this.Delay = Delay;
        this.skill_up_lv = skill_up_lv;
        this.need_lv = need_lv;
        this.Weighted = Weighted;
        this.MoveType = MoveType;
        this.offset = offset;
        this.scope = scope;
        this.needLvitem = needLvitem;
        lv = -1;
        exp = 0;
        select_pos = -1;
    }
    public void Init(int lv, int exp)
    { 
        this.lv = lv;
        this.exp = exp;
    }
    /// <summary>
    /// 读取等级
    /// </summary>
    /// <returns></returns>
    public int SetLv()
    { 
        return lv;
    }
    public int SetExp()
    { 
        return exp;
    }
    public int SetSelectPos()
    { 
        return select_pos;
    }

    public void GetExp(int exp)
    {
        if (lv < 0) return;//未激活
        if (lv >= skill_up_lv.Count-1) return;
        this.exp += exp;
        while (this.exp >= skill_up_lv[lv])
        {
            if (lv >= skill_up_lv.Count-1) return;
            this.exp -= skill_up_lv[lv];
            lv++;
        }
    }
    public void activate_skill()
    {
        lv = 0;
    }

    public void monster_lv(int lv)
    {
        this.lv = lv;
    }
    
    /// <summary>
    /// 叠加buff效果
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="value"></param>
    public void AddBuff(enum_talent_offect_list buff, int value)
    { 
        if (bufflist == null) bufflist = new Dictionary<enum_talent_offect_list, int>();
        if (!bufflist.ContainsKey(buff)) bufflist.Add(buff, value);
        else bufflist[buff] += value;
    }

    public void ClearBuff()
    { 
       bufflist.Clear();
    }
    /// <summary>
    /// 获取buff效果
    /// </summary>
    public Dictionary<enum_talent_offect_list, int> GetBuff { get { return bufflist; } }
}
