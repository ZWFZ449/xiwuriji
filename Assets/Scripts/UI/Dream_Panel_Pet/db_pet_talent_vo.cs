using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class db_pet_talent_vo 
{
    public readonly int pet_talent_id;

    public readonly string pet_talent_name; // 名字
    /// <summary>
    /// 天赋等级 1普通兽书 2高级兽书 3本命书
    /// </summary>
    public readonly int pet_talent_level; // 等级
    /// <summary>
    /// 作用职业 1战士 2法师 3道士召唤兽 0所有
    /// </summary>
    public readonly int pet_talent_job; // 图标
    /// <summary>
    /// 天赋类型 1作用自身 2战斗触发
    /// </summary>
    public readonly int pet_talent_type; // 类型
    /// <summary>
    /// 触发类型 
    /// 1作用自身
    /// 1.1物理伤害百分比
    /// 1.2魔法伤害百分比
    /// 1.3召唤兽伤害百分比
    /// 1.4物理防御
    /// 1.5魔法防御
    /// 1.6回复hp
    /// 1.7回复mp
    /// 1.8减少物理伤害%
    /// 1.9减少魔法伤害%
    /// 1.10技能释放消耗减少
    /// 1.11增加躲避    
    /// 2战斗触发
    /// 2.1连击  
    /// 2.2忽视物理防御
    /// 2.3忽视魔法防御
    /// 2.4忽视召唤兽防御
    /// 2.5反镇
    /// 2.6防爆
    /// 2.7招架
    /// 2.8反击

    /// 3特殊
    /// 1 增加生命上限
    /// 2 增加基础属性
    /// 3 概率随机传送一个敌人
    /// 4 击杀后追击另一个目标
    /// 5 攻击无视防御
    /// 6 攻击概率10倍
    /// 7 攻击概率斩杀
    /// 8 连击效果提升
    /// </summary>
    public readonly int pet_talent_offect; // 效果
    /// <summary>
    /// 效果类型 概率
    /// </summary>
    public readonly int pet_talent_offecttype; // 效果类型
    /// <summary>
    /// 效果值
    /// </summary>
    public readonly float pet_talent_offectvalue; // 效果值

    public db_pet_talent_vo(int pet_talent_id, string pet_talent_name, int pet_talent_level, int pet_talent_job, int pet_talent_type, int pet_talent_offect, int pet_talent_offecttype, float pet_talent_offectvalue)
    { 
        this.pet_talent_id = pet_talent_id;
        this.pet_talent_name = pet_talent_name;
        this.pet_talent_level = pet_talent_level;
        this.pet_talent_job = pet_talent_job;
        this.pet_talent_type = pet_talent_type;
        this.pet_talent_offect = pet_talent_offect;
        this.pet_talent_offecttype = pet_talent_offecttype;
        this.pet_talent_offectvalue = pet_talent_offectvalue;
        //测试图片添加新内容时测试一下
        //UI.UI_Manager.I.GetEquipSprite("UI/pet/pet_talent/", pet_talent_name);

    }
}
