
using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 属性汇总表
/// </summary>
public class crtMaxHeroVO
{
    /// <summary>
    /// 判断怪物状态 0 代表怪物的不同加成
    /// </summary>
    public List<int> monster_attrList= new List<int>();
    /// <summary>
    /// 展示名称
    /// </summary>
    public string show_name;
    /// <summary>
    /// 回收名称
    /// </summary>
    public string Push_name;
    /// <summary>
    /// 职业 1战 2法 3道 11战伤害 12法伤害召唤
    /// </summary>
    public int Type;//角色职业
    /// <summary>
    /// 地图产出编号
    /// </summary>
    public int map_index;
    /// <summary>
    /// 状态编号 玩家-1
    /// </summary>
    public int index;
    /// <summary>
    /// 宠物编号
    /// </summary>
    public int PetPos;
    /// <summary>
    /// 等级
    /// </summary>
    public int Lv;//角色等级
    /// <summary>
    /// 经验
    /// </summary>
    public long Exp;//当前经验
    /// <summary>
    /// 声望值
    /// </summary>
    public int Point;//声望值
    /// <summary>
    /// 图标
    /// </summary>
    public string icon;
    /// <summary>
    /// 生命
    /// </summary>
    public long MaxHP;//最大生命
    /// <summary>
    /// 魔法值
    /// </summary>
    public int MaxMp;
    /// <summary>
    /// 内力值
    /// </summary>
    public int internalforceMP;
    /// <summary>
    /// 蓄力值
    /// </summary>
    public int EnergyMp;
    /// <summary>
    /// 物理防御
    /// </summary>
    public int DefMin;
    /// <summary>
    /// 物理防御
    /// </summary>
    public int DefMax;
    /// <summary>
    /// 魔法防御
    /// </summary>
    public int MagicDefMin;
    /// <summary>
    /// 魔法防御
    /// </summary>
    public int MagicDefMax;
    /// <summary>
    /// 物理伤害
    /// </summary>
    public int damageMin;
    /// <summary>
    /// 物理伤害
    /// </summary>
    public int damageMax;
    /// <summary>
    /// 魔法伤害
    /// </summary>
    public int MagicdamageMin;
    /// <summary>
    /// 魔法伤害
    /// </summary>
    public int MagicdamageMax;
    /// <summary>
    /// 命中
    /// </summary>
    public int hit;
    /// <summary>
    /// 闪避
    /// </summary>
    public int dodge;

    /// <summary>
    /// 穿透
    /// </summary>
    public int penetrate;
    /// <summary>
    /// 格挡
    /// </summary>
    public int block;
    /// <summary>
    /// 暴击
    /// </summary>
    public int crit_rate;
    /// <summary>
    /// 暴击伤害
    /// </summary>
    public int crit_damage;
    /// <summary>
    /// 伤害加成
    /// </summary>
    public int double_damage;
    /// <summary>
    /// 幸运
    /// </summary>
    public int Lucky;
    /// <summary>
    /// 真实伤害
    /// </summary>
    public int Real_harm;
    /// <summary>
    /// 伤害减免
    /// </summary>
    public int Damage_Reduction;
    /// <summary>
    /// 伤害吸收
    /// </summary>
    public int Damage_absorption;
    /// <summary>
    /// 异常抗性
    /// </summary>
    public int resistance;
    /// <summary>
    /// 移动速度
    /// </summary>
    public int move_speed;
    /// <summary>
    /// 攻击速度
    /// </summary>
    public int attack_speed;
    /// <summary>
    /// 攻击距离
    /// </summary>
    public int attack_distance;
    /// <summary>
    /// 生命加成
    /// </summary>
    public int bonus_Hp;
    /// <summary>
    /// 法力加成
    /// </summary>
    public int bonus_Mp;
    /// <summary>
    /// 物理伤害加成
    /// </summary>
    public int bonus_Damage;
    /// <summary>
    /// 魔法伤害
    /// </summary>
    public int bonus_MagicDamage;
    /// <summary>
    /// 防御加成
    /// </summary>
    public int bonus_Def;
    /// <summary>
    /// 魔法防御加成
    /// </summary>
    public int bonus_MagicDef;
    /// <summary>
    /// 生命回复
    /// </summary>
    public int Heal_Hp;
    /// <summary>
    /// 魔法回复
    /// </summary>
    public int Heal_Mp;
    /// <summary>
    /// 五行属性
    /// </summary>
    public int[] life = new int[] { 0, 0, 0, 0, 0 };
    /// <summary>
    /// 天命五行
    /// </summary>
    public Dictionary<int, int> life_types = new Dictionary<int, int>();
    /// <summary>
    /// buff加成
    /// </summary>
    public List<int> bufflist = new List<int>();
    /// <summary>
    /// 怪物掉落金币
    /// </summary>
    public int unit = 0;
    /// <summary>
    /// 战力
    /// </summary>
    public int totalPower;
    /// <summary>
    /// 技能状态1类型2效果3开启时间4剩余时间
    /// </summary>
    public List<(int, int, DateTime, float)> skill_state = new List<(int, int, DateTime, float)>();
    
    /// <summary>
    /// 判断是否为boss 1普通 2精英 3boss
    /// </summary>
    public int Monster_Lv=0;
    /// <summary>
    /// 是否为背刺怪 为1时为背刺怪
    /// </summary>
    public int isBackstab=0;
    /// <summary>
    /// 携带灵宝属性
    /// </summary>
    public Dictionary<enum_equip_show_list, int> equip_suit_lists = new Dictionary<enum_equip_show_list, int>();

    /// <summary>
    /// 显示战斗力
    /// </summary>
    public void Init()
    {
        totalPower = (int)(MaxHP / 10 + (MaxMp / 10) + internalforceMP + EnergyMp +
            DefMin + DefMax + MagicDefMin + MagicDefMax + damageMin + damageMax + MagicdamageMin + MagicdamageMax +
            hit + (dodge * 5) + (penetrate * 5) + (block * 5) + (crit_rate * 10) + crit_damage + (double_damage * 10) + (Lucky * 100) +
            (Damage_Reduction * 10) + (Damage_absorption * 10) + (resistance * 10) + move_speed + ((200 - attack_speed) * 10) + attack_distance +
            (bonus_Hp * 20) + (bonus_Mp * 20) + (bonus_Damage * 20) + (bonus_MagicDamage * 20) + (bonus_Def * 20) + (bonus_MagicDef * 20) +
            (Heal_Hp * 20) + (Heal_Mp * 20) + ((life[0] + life[1] + life[2] + life[3] + life[4]) * 20));
        Debug.Log("战斗力" + totalPower);


#if UNITY_EDITOR

        totalPower = Battle_Tool.random();

#elif UNITY_ANDROID

#elif UNITY_IPHONE
#endif
    }
}
