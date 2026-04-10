using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class db_player_talent_vo 
{
    public readonly int talent_id;

    public readonly string talent_name;
    /// <summary>
    /// 职业 1战士 2法师 3道
    /// </summary>
    public readonly int job;
    /// <summary>
    /// 类型 战士 战神 武皇 暴君
    ///  法师 法神 法皇 法尊
    ///  道  道神 道皇 道尊
    /// </summary>
    public readonly int talent_type;
    /// <summary>
    /// 开启等级
    /// </summary>
    public readonly int talent_need_lv;
    /// <summary>
    /// 升级消耗数量
    /// </summary>
    public readonly List<int> ralent_need_uplv;
    /// <summary>
    /// 升级消耗内容
    /// </summary>
    public readonly List<string> ralent_need_uplv_value;
    /*
     * /// 100属性 生命百分比
    /// 101攻击百分比
    /// 102魔法百分比
    /// 103道术百分比
    /// 104防御百分比
    /// 106攻击速度百分比
    /// 
    /// 107 攻击
    /// 108 魔法
    /// 109 道术
    /// 110 防御
    /// 111 躲避
    /// 112 速度
    /// 113 命中
    /// 200技能
    /// 201 技能附加攻击 跟等级走
    /// 202 技能附加魔法 跟等级走
    /// 203 技能附加道术 跟等级走
    /// 204 技能附加双防御 
    /// 205 技能附加伤害
    /// 206 技能附加回血
    /// 207 技能附加攻击范围
    /// 208 无视防御
    /// 300召唤兽
    /// 301召唤兽攻击 跟着道术走
    /// 302召唤兽生命 跟着道术走
    /// 303召唤兽防御 跟着道术走
    /// 304召唤兽速度
    /// 305召唤兽死亡爆炸
    /// 400特殊效果 buff本源
    /// 401伤害 
    /// 402防御
    /// 403 速度
    /// 500 单体改群体
    /// 特殊效果
    /// 600 技能攻击个数
    /// 601 技能概率不消耗蓝
    /// 602 技能全体伤害
    /// 603 群体技能攻击范围
    /// 604 每s回复全体血量百分比
    /// 605 攻击击退敌人概率
    /// 
     */
    /// <summary>
    /// 类型

    /// </summary>
    public readonly int talent_offect;
    /// <summary>
    /// 属性加成
    /// </summary>
    public readonly List<int> talent_offect_value;

    public readonly int correlation_skill;

    public db_player_talent_vo(int talent_id,string talent_name, int job,int talent_type,int talent_need_lv, List<int> ralent_need_uplv, List<string> ralent_need_uplv_value, int talent_offect, List<int> talent_offect_value,int correlation_skill)
    { 
        this.talent_name = talent_name;
        this.talent_id = talent_id;
        this.job = job;
        this.talent_type = talent_type;
        this.talent_need_lv = talent_need_lv;
        this.ralent_need_uplv = ralent_need_uplv;
        this.talent_offect = talent_offect;
        this.talent_offect_value = talent_offect_value;
        this.correlation_skill = correlation_skill;
        this.ralent_need_uplv_value = ralent_need_uplv_value;

        //UI.UI_Manager.I.GetEquipSprite("UI/player/player_talent/", talent_name);

    }
}
