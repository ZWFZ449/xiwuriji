using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Tool_Battle 
{
    /// <summary>
    /// 附加属性
    /// </summary>
    private static List<enum_equip_entry_list> entry_list;
    /// <summary>
    /// 元素属性
    /// </summary>
    private static List<enum_equip_entry_list> entry_coefficient_list;
    /// <summary>
    /// 附魔属性
    /// </summary>
    private static List<enum_equip_entry_list> entry_inscription_list;

    private static List<WeightedItem> equip_eighteditems = new List<WeightedItem>();
    /// <summary>
    /// 获取属性
    /// </summary>
    /// <returns></returns>
    public static crtMaxBattleVO InitPlayerMaxBattle()
    {
        int exp_bonus = 0, gold_bonus = 0, drop_bonus = 0, quality_bonus = 0,boss_cd=0;
        int maxhp = 0, maxmp = 0;
        int battle_hp = 0, battle_mp = 0, battle_ac = 0, battle_mac = 0, battle_dc = 0, battle_sc = 0, battle_mc = 0, battle_speed = 0, battle_range = 0, battle_Damage = 0, battle_def = 0;
        int hp = 0, mp = 0, dc = 0, dc2 = 0, mac = 0, mac2 = 0, ac = 0, ac2 = 0, sc = 0, sc2 = 0, mc = 0, mc2 = 0;
        int hit = 0, dodge = 0, crit = 0, critDmg = 100;
        int hpRegen = 0, mpRegen = 0;
        int lucky = 0, damage_reduction=0,magic_damage_reduction=0;
        Dictionary<enum_equip_entry_list, int> buffList = new Dictionary<enum_equip_entry_list, int>();
        List<(enum_battle_pet_talent_list,int,int)> talentList = new List<(enum_battle_pet_talent_list, int, int)>();
        Dictionary<int, int> suits = new Dictionary<int, int>();//套装
        int speed_bonus = 1;
        if (SumSave.crtHero.job != 1) speed_bonus = 2;
        foreach (var item in SumSave.db_heros)
        {
            if (item.id == SumSave.crtHero.job)
            {
                hp += item.inithp + (item.hp * SumSave.crtHero.lv / (item.uphp + 1));
                mp += item.initmp + (item.mp * SumSave.crtHero.lv / (item.upmp + 1));
                dc += item.initdc + (item.dc * SumSave.crtHero.lv / (item.updc + 1));
                dc2 += item.initdc2 + (item.dc2 * SumSave.crtHero.lv / (item.updc2 + 1));
                mac += item.initmac + (item.mac * SumSave.crtHero.lv / (item.upmac + 1));
                mac2 += item.initmac2 + (item.mac2 * SumSave.crtHero.lv / (item.upmac2 + 1));
                ac += item.initac + (item.ac * SumSave.crtHero.lv / (item.upac + 1));
                ac2 += item.initac2 + (item.ac2 * SumSave.crtHero.lv / (item.upac2 + 1));
                sc += item.initsc + (item.sc * SumSave.crtHero.lv / (item.upsc + 1));
                sc2 += item.initsc2 + (item.sc2 * SumSave.crtHero.lv / (item.upsc2 + 1));
                mc += item.initmc + (item.mc * SumSave.crtHero.lv / (item.upmc + 1));
                mc2 += item.initmc2 + (item.mc2 * SumSave.crtHero.lv / (item.upmc2 + 1));
                hit += item.inithit+ (item.hit * SumSave.crtHero.lv / (item.uphit + 1));
                dodge += item.initdodge + (item.dodge * SumSave.crtHero.lv / (item.updodge + 1));
                crit += item.initcrit + (item.crit * SumSave.crtHero.lv / (item.upcrit + 1));
                critDmg += item.initcritDmg + (item.critDmg * SumSave.crtHero.lv / (item.upcritDmg + 1));
                battle_speed = item.initspeed - (item.speed * SumSave.crtHero.lv / (item.upspeed + 1));
                battle_range += item.initrange + (item.range * SumSave.crtHero.lv / (item.uprange + 1));
            }
        }
        Dictionary<int, db_skill_vo> skill_list = SumSave.crt_skill.Set_Current_skill();
        //初始化临时buff
        foreach (var item in skill_list) item.Value.ClearBuff();
        foreach (var talent in SumSave.crtHero.talent)
        {
            switch ((enum_talent_offect_list)(talent.Item1.talent_offect))
            {
                case enum_talent_offect_list.生命: battle_hp += tool_talent_value(talent, skill_list); break;
                case enum_talent_offect_list.攻击: battle_dc += tool_talent_value(talent, skill_list); break;
                case enum_talent_offect_list.魔法: battle_mc += tool_talent_value(talent, skill_list); break;
                case enum_talent_offect_list.道术: battle_sc += tool_talent_value(talent, skill_list); break;
                case enum_talent_offect_list.防御: battle_ac += tool_talent_value(talent, skill_list); battle_mac += tool_talent_value(talent, skill_list); break;
                    
                case enum_talent_offect_list.攻击速度:
                    battle_speed -= tool_talent_value(talent, skill_list) * speed_bonus; break;
                case enum_talent_offect_list.物理攻击:
                    dc2 += tool_talent_value(talent, skill_list); break;
                case enum_talent_offect_list.魔法攻击:
                    mc2 += tool_talent_value(talent, skill_list); break;
                case enum_talent_offect_list.道术攻击:
                    sc2 += tool_talent_value(talent, skill_list); break;
                case enum_talent_offect_list.防御值:
                    ac2 += tool_talent_value(talent, skill_list); mac2 += tool_talent_value(talent, skill_list); break;
                case enum_talent_offect_list.躲避:
                    dodge += tool_talent_value(talent, skill_list); break;
                case enum_talent_offect_list.命中:
                    hit += tool_talent_value(talent, skill_list); break;
                case enum_talent_offect_list.技能:
                    break;
                case enum_talent_offect_list.附加攻击: battle_dc += tool_talent_value(talent, skill_list); break;
                case enum_talent_offect_list.附加魔法: battle_mc += tool_talent_value(talent, skill_list); break;
                case enum_talent_offect_list.附加道术: battle_sc += tool_talent_value(talent, skill_list); break;
                case enum_talent_offect_list.附加双防: battle_ac += tool_talent_value(talent, skill_list); battle_mac += tool_talent_value(talent, skill_list); break;
                case enum_talent_offect_list.附加回血: 
                case enum_talent_offect_list.附加伤害:
                case enum_talent_offect_list.附加攻击范围:
                case enum_talent_offect_list.无视防御:
                case enum_talent_offect_list.召唤兽:
                case enum_talent_offect_list.召唤兽攻击:
                case enum_talent_offect_list.召唤兽生命:
                case enum_talent_offect_list.召唤兽防御:
                case enum_talent_offect_list.召唤兽速度:
                case enum_talent_offect_list.召唤兽死亡爆炸:
                    AddSkillBuff(talent, skill_list);
                    //dec += "[战斗效果]\n召唤兽死亡时对周围目标造成最大生命值" + "+" + data.talent_offect_value[index] + "%的伤害";
                    break;
                case enum_talent_offect_list.特殊效果:
                    break;
                case enum_talent_offect_list.临时伤害:
                case enum_talent_offect_list.临时防御:
                case enum_talent_offect_list.临时速度:
                    //dec += "[战斗效果]\n自身生命值每降低10%" + " \n获得 " + (enum_talent_offect_list)data.talent_offect + "+" + data.talent_offect_value[index] + "%";
                    break;
                case enum_talent_offect_list.单体改群体:
                    AddSkillBuff(talent, skill_list);
                    //skill_name = SumSave.db_skills.Find(x => x.id == data.correlation_skill).show_name;
                    //dec += "[战斗效果]\n" + Show_Color.Yellow(skill_name) + "变为群体技能" +
                    //    "\n群体技能攻击伤害 = " + skill_name + " 的 " + data.talent_offect_value[index] + "%";
                    break;
                case enum_talent_offect_list.技能攻击个数:
                    AddSkillBuff(talent, skill_list);

                    //skill_name = SumSave.db_skills.Find(x => x.id == data.correlation_skill).show_name;
                    //dec += "[战斗效果]\n" + Show_Color.Yellow(skill_name) + " 的攻击次数变为 " + data.talent_offect_value[index] + "";
                    break;
                case enum_talent_offect_list.技能概率不消耗蓝:
                    AddSkillBuff(talent, skill_list);
                    //if (data.correlation_skill == -1)
                    //{
                    //    dec += "[战斗效果]\n" + "释放技能" + " " + data.talent_offect_value[index] + "% 概率不消耗魔法";
                    //}
                    //else
                    //{
                    //    skill_name = SumSave.db_skills.Find(x => x.id == data.correlation_skill).show_name;
                    //    dec += "[战斗效果]\n" + "释放技能 " + Show_Color.Yellow(skill_name) + " " + data.talent_offect_value[index] + "%概率不消耗魔法";

                    //}
                    break;
                case enum_talent_offect_list.技能全体伤害:
                    break;
                case enum_talent_offect_list.群体技能攻击范围:
                    AddSkillBuff(talent, skill_list);
                    //if (data.correlation_skill == -1)
                    //{
                    //    dec += "[战斗效果]\n" + "技能攻击范围" + "+" + data.talent_offect_value[index] + "% ";
                    //}
                    //else
                    //{
                    //    skill_name = SumSave.db_skills.Find(x => x.id == data.correlation_skill).show_name;
                    //    dec += "[战斗效果]\n" + "技能 " + Show_Color.Yellow(skill_name) + " 攻击范围 + " + data.talent_offect_value[index] + "%";

                    //}
                    break;
                case enum_talent_offect_list.每秒回复全体血量百分比:
                    //dec += "[被动效果]\n" + "每秒回复全体血量" + "+" + data.talent_offect_value[index] + "% ";
                    break;
                case enum_talent_offect_list.攻击击退敌人概率:
                    AddSkillBuff(talent, skill_list);
                    //if (data.correlation_skill == -1)
                    //{
                    //    dec += "[战斗效果]\n" + "攻击时" + data.talent_offect_value[index] + "%" + "击退敌人";
                    //}
                    //else
                    //{
                    //    skill_name = SumSave.db_skills.Find(x => x.id == data.correlation_skill).show_name;
                    //    dec += "[战斗效果]\n" + "技能 " + Show_Color.Yellow(skill_name) + " 攻击时" + data.talent_offect_value[index] + "%" + "击退敌人";

                    //}
                    break;
               case enum_talent_offect_list.弹道:
                    AddSkillBuff(talent, skill_list);
                    break;
            }

        }//角色天赋
        List<Bag_Base_VO> crt_euqip = SumSave.crt_equips.Get(Dream_User_Equip_Type.装备);
        for (int i = 0; i < crt_euqip.Count; i++)  
        {
            dc += crt_euqip[i].dc;
            dc2 += crt_euqip[i].dc2;
            mac += crt_euqip[i].mac;
            mac2 += crt_euqip[i].mac2;
            ac += crt_euqip[i].ac;
            ac2 += crt_euqip[i].ac2;
            sc += crt_euqip[i].sc;
            sc2 += crt_euqip[i].sc2;
            mc += crt_euqip[i].mc;
            mc2 += crt_euqip[i].mc2;
            if (crt_euqip[i].suit > 0)
            { 
                if (!suits.ContainsKey(crt_euqip[i].suit)) suits.Add(crt_euqip[i].suit, 0);
                suits[crt_euqip[i].suit]++;
            }
            string[] info = crt_euqip[i].user_value.Split(' ');
            int strengthenlv = int.Parse(info[1]);
            if (info.Length >= 5)
            {
                //类型
                string[] arr2 = info[4].Split('X');
                for (int j = 0; j < arr2.Length; j++)
                {
                    if (arr2[j].Length > 0)
                    {
                        string[] entry = arr2[j].Split('|');

                        for (int k = 0; k < entry.Length; k++)
                        {
                            string[] entry_arr = entry[k].Split(',');
                            if (entry_arr.Length > 1)
                            {
                                enum_equip_entry_list e = (enum_equip_entry_list)int.Parse(entry_arr[0]);
                                int value = int.Parse(entry_arr[1]);
                                switch (e)
                                {
                                    case enum_equip_entry_list.生命值: hp += value;break;
                                    case enum_equip_entry_list.魔法值:mp += value;break; 
                                    case enum_equip_entry_list.物理防御:ac2 += value;break;
                                    case enum_equip_entry_list.魔法防御:mac2 += value;break;
                                    case enum_equip_entry_list.物理攻击:dc2 += value;break;
                                    case enum_equip_entry_list.魔法攻击:mc2 += value;break;
                                    case enum_equip_entry_list.道术攻击:sc2 += value;break;

                                    case enum_equip_entry_list.每秒回血: hpRegen += value;break;
                                    case enum_equip_entry_list.每秒回蓝:mpRegen += value;break;
                                    case enum_equip_entry_list.真实伤害:battle_Damage+= value;break;
                                    case enum_equip_entry_list.吸收伤害:battle_def+= value;break;
                                    case enum_equip_entry_list.物理下防:ac += value;break;
                                    case enum_equip_entry_list.魔法下防:mac += value;break;
                                    case enum_equip_entry_list.物理下攻:dc += value;break;
                                    case enum_equip_entry_list.魔法下攻:mc += value;break;
                                    case enum_equip_entry_list.道术下攻:sc += value;break;

                                    case enum_equip_entry_list.生命属性:battle_hp+= value;break;
                                    case enum_equip_entry_list.魔法属性:battle_mp+= value;break;
                                    case enum_equip_entry_list.防御属性:battle_ac+= value;break;
                                    case enum_equip_entry_list.魔防属性:battle_mac+= value;break;
                                    case enum_equip_entry_list.物攻属性:battle_dc+= value;break;
                                    case enum_equip_entry_list.魔攻属性:battle_sc+= value;break;
                                    case enum_equip_entry_list.道攻属性:battle_mc+= value;break;
                                    case enum_equip_entry_list.攻击速度:battle_speed-= (value * speed_bonus); break;
                                    case enum_equip_entry_list.攻击范围:battle_range+= value;break;
                                    case enum_equip_entry_list.暴击属性:crit += value;break;
                                    case enum_equip_entry_list.暴击伤害:critDmg+= value;break;

                                    case enum_equip_entry_list.烈阳文:
                                    case enum_equip_entry_list.盾护文:
                                    case enum_equip_entry_list.守月文:
                                    case enum_equip_entry_list.幽狼文:
                                    case enum_equip_entry_list.神行文:
                                    case enum_equip_entry_list.怒目文:
                                    case enum_equip_entry_list.震火文:
                                    case enum_equip_entry_list.金刚文:
                                    case enum_equip_entry_list.大愈文:
                                    case enum_equip_entry_list.回春文:
                                    case enum_equip_entry_list.回心文:
                                    case enum_equip_entry_list.峰芒文:
                                    case enum_equip_entry_list.破枪文:
                                    case enum_equip_entry_list.深寒文:
                                    case enum_equip_entry_list.瑶光文:
                                        if(!buffList.ContainsKey(e))buffList.Add(e, 0);
                                        buffList[e] += value;
                                        break;
                                    case enum_equip_entry_list.幸运:lucky+= value;break;
                                    case enum_equip_entry_list.闪避:dodge+= value;break;
                                    case enum_equip_entry_list.命中:hit+= value;break;
                                    case enum_equip_entry_list.物伤减免:damage_reduction += value; break;
                                    case enum_equip_entry_list.魔伤减免:magic_damage_reduction += value; break;
                                    case enum_equip_entry_list.怪物爆率: drop_bonus += value; break;
                                    case enum_equip_entry_list.极品爆率:quality_bonus += value; break;
                                    case enum_equip_entry_list.经验加成:exp_bonus += value; break;
                                    case enum_equip_entry_list.金币掉落:gold_bonus += value; break;
                                    default:
                                        if ((int)e >= 1000)//附加技能
                                        {
                                            AddEquipSkillBuff((int)e - 1000, skill_list, value);
                                        }
                                        break;
                                }

                            }
                        }
                    }
                }
            }
        }
        if (suits.Count > 0)
        {
            foreach (var item in suits)
            {
                db_suit_vo suit = ArrayHelper.Find(SumSave.db_suits, e => e.suit_type == item.Key);
                if (suit != null)
                {
                    for (int i = 0; i < suit.suit_list.Count; i++)
                    {
                        if (suit.suit_list[i].Item1 <= item.Value)
                        {
                            switch ((Suit_Type)suit.suit_list[i].Item2)
                            {
                                case Suit_Type.物理攻击:
                                    dc += suit.suit_list[i].Item3 - 1;
                                    dc2 += suit.suit_list[i].Item3;
                                    break;
                                case Suit_Type.魔法攻击:
                                    mc += suit.suit_list[i].Item3 - 1;
                                    mc2 += suit.suit_list[i].Item3;
                                    break;
                                case Suit_Type.道术攻击:
                                    sc += suit.suit_list[i].Item3 - 1;
                                    sc2 += suit.suit_list[i].Item3;
                                    break;
                                case Suit_Type.双防:
                                    ac += suit.suit_list[i].Item3 - 1;
                                    ac2 += suit.suit_list[i].Item3;
                                    mac += suit.suit_list[i].Item3 - 1;
                                    mac2 += suit.suit_list[i].Item3;
                                    break;
                                case Suit_Type.生命:
                                    hp += suit.suit_list[i].Item3;
                                    break;
                                case Suit_Type.真实伤害:
                                    battle_Damage += suit.suit_list[i].Item3;
                                    break;
                                case Suit_Type.攻击速度:
                                    battle_speed -= (suit.suit_list[i].Item3 * speed_bonus);
                                    break;
                                case Suit_Type.物攻属性:
                                    battle_dc += suit.suit_list[i].Item3;
                                    break;
                                case Suit_Type.魔攻属性:
                                    battle_mc += suit.suit_list[i].Item3;
                                    break;
                                case Suit_Type.道攻属性:
                                    battle_sc += suit.suit_list[i].Item3;
                                    break;
                                case Suit_Type.双防属性:
                                    battle_ac += suit.suit_list[i].Item3;
                                    battle_mac += suit.suit_list[i].Item3;
                                    break;
                                case Suit_Type.伤害吸收:
                                    battle_def += suit.suit_list[i].Item3;
                                    break;
                            }
                        }
                    }


                }
            }
        }
        //图鉴加成
        Dictionary<string, int> illustrated_list = SumSave.crt_illustrated.Get_illustrated_list();
        foreach (var db_illustrated in SumSave.db_illustrateds)
        {
            for (int k = 0; k < db_illustrated.Value.Count; k++)
            {
                db_illustrated_vo data = db_illustrated.Value[k];
                List<string> list = ArrayHelper.Get_Split<string>(data.Illustrated_need, ',');
                List<string> effect = ArrayHelper.Get_Split<string>(data.Illustrated_effect, ',');
                switch ((Illustrated_Type)data.Illustrated_type)
                {
                    case Illustrated_Type.怪物图鉴:
                    case Illustrated_Type.BOSS博物馆:
                        for (int i = 0; i < list.Count; i++)
                        {
                            List<string> value = ArrayHelper.Get_Split<string>(list[i], ' ');
                            if (value.Count == 2)
                            {
                                if (illustrated_list.ContainsKey(value[0]))
                                {
                                    if (int.Parse( value[1] )== illustrated_list[value[0]])//达成条件
                                    {
                                        if (effect.Count >= i)
                                        {
                                            List<string> crt_effect = ArrayHelper.Get_Split<string>(effect[i], '&');
                                            foreach (var item1 in crt_effect)
                                            {
                                                List<int> effect_value = ArrayHelper.Get_Split<int>(item1, ' ');
                                                if (effect_value.Count == 2)
                                                {
                                                    switch ((Suit_Type)(effect_value[0]))
                                                    {
                                                        case Suit_Type.物理攻击:
                                                            dc += effect_value[1];
                                                            dc2 += effect_value[1];
                                                            break;
                                                        case Suit_Type.魔法攻击:
                                                            mc += effect_value[1];
                                                            mc2 += effect_value[1];
                                                            break;
                                                        case Suit_Type.道术攻击:
                                                            sc += effect_value[1];
                                                            sc2 += effect_value[1];
                                                            break;
                                                        case Suit_Type.双防:
                                                            ac += effect_value[1];
                                                            ac2 += effect_value[1];
                                                            mac += effect_value[1];
                                                            mac2 += effect_value[1];
                                                            break;
                                                        case Suit_Type.生命:
                                                            hp += effect_value[1];
                                                            break;
                                                        case Suit_Type.真实伤害:
                                                            battle_Damage += effect_value[1];
                                                            break;
                                                        case Suit_Type.攻击速度:
                                                            battle_speed -= (effect_value[1] * speed_bonus);
                                                            break;
                                                        case Suit_Type.物攻属性:
                                                            battle_dc += effect_value[1];
                                                            break;
                                                        case Suit_Type.魔攻属性:
                                                            battle_mc += effect_value[1];
                                                            break;
                                                        case Suit_Type.道攻属性:
                                                            battle_sc += effect_value[1];
                                                            break;
                                                        case Suit_Type.双防属性:
                                                            battle_ac += effect_value[1];
                                                            battle_mac += effect_value[1];
                                                            break;
                                                        case Suit_Type.伤害吸收:
                                                            battle_def += effect_value[1];
                                                            break;
                                                    }

                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                        break;
                    case Illustrated_Type.装备图鉴:
                        int number = 0;
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (illustrated_list.ContainsKey(list[i])) number++;
                        }
                        for (int i = 0; i < effect.Count; i++)
                        {
                            List<string> crt_effect = ArrayHelper.Get_Split<string>(effect[i], '&');
                            foreach (var item1 in crt_effect)
                            {
                                List<int> effect_value = ArrayHelper.Get_Split<int>(item1, ' ');
                                if (effect_value.Count == 3)
                                {
                                    if (number >= effect_value[0])
                                    {
                                        switch ((Suit_Type)(effect_value[1]))
                                        {
                                            case Suit_Type.物理攻击:
                                                dc += effect_value[2];
                                                dc2 += effect_value[2];
                                                break;
                                            case Suit_Type.魔法攻击:
                                                mc += effect_value[2];
                                                mc2 += effect_value[2];
                                                break;
                                            case Suit_Type.道术攻击:
                                                sc += effect_value[2];
                                                sc2 += effect_value[2];
                                                break;
                                            case Suit_Type.双防:
                                                ac += effect_value[2];
                                                ac2 += effect_value[2];
                                                mac += effect_value[2];
                                                mac2 += effect_value[2];
                                                break;
                                            case Suit_Type.生命:
                                                hp += effect_value[2];
                                                break;
                                            case Suit_Type.真实伤害:
                                                battle_Damage += effect_value[2];
                                                break;
                                            case Suit_Type.攻击速度:
                                                battle_speed -= (effect_value[2] * speed_bonus);
                                                break;
                                            case Suit_Type.物攻属性:
                                                battle_dc += effect_value[2];
                                                break;
                                            case Suit_Type.魔攻属性:
                                                battle_mc += effect_value[2];
                                                break;
                                            case Suit_Type.道攻属性:
                                                battle_sc += effect_value[2];
                                                break;
                                            case Suit_Type.双防属性:
                                                battle_ac += effect_value[2];
                                                battle_mac += effect_value[2];
                                                break;
                                            case Suit_Type.伤害吸收:
                                                battle_def += effect_value[2];
                                                break;
                                        }

                                    }

                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        
        foreach (var item in skill_list)
        {
            int skill_lv = item.Value.SetLv();
            if (skill_lv >= 0)
            {
                if (item.Value.Job == -1)
                {
                    foreach (var item1 in item.Value.GetBuff.Keys)
                    {
                        switch (item1)
                        {
                            case enum_talent_offect_list.弹道:
                                skill_lv += item.Value.GetBuff[item1];
                                break;
                        }
                    }
                }
                if (item.Value.skill_offect_value_list.Count > 0)
                {
                    foreach (enum_equip_entry_list skill_effect_type in item.Value.skill_offect_value_list.Keys)
                    {
                        if (skill_lv >= item.Value.skill_offect_value_list[skill_effect_type].Count - 1)
                            skill_lv = item.Value.skill_offect_value_list[skill_effect_type].Count - 1;
                        int value = item.Value.skill_offect_value_list[skill_effect_type][skill_lv];
                        switch (skill_effect_type)
                        {
                            case enum_equip_entry_list.生命值: hp += value; break;
                            case enum_equip_entry_list.魔法值: mp += value; break;
                            case enum_equip_entry_list.物理防御: ac += value; ac2 += value; break;
                            case enum_equip_entry_list.魔法防御: mac += value; mac2 += value; break;
                            case enum_equip_entry_list.物理攻击: dc += value; dc2 += value; break;
                            case enum_equip_entry_list.魔法攻击: mc += value; mc2 += value; break;
                            case enum_equip_entry_list.道术攻击: sc += value; sc2 += value; break;

                            case enum_equip_entry_list.每秒回血: hpRegen += value; break;
                            case enum_equip_entry_list.每秒回蓝: mpRegen += value; break;
                            case enum_equip_entry_list.真实伤害: battle_Damage += value; break;
                            case enum_equip_entry_list.吸收伤害: battle_def += value; break;

                            case enum_equip_entry_list.生命属性: battle_hp += value; break;
                            case enum_equip_entry_list.魔法属性: battle_mp += value; break;
                            case enum_equip_entry_list.防御属性: battle_ac += value; break;
                            case enum_equip_entry_list.魔防属性: battle_mac += value; break;
                            case enum_equip_entry_list.物攻属性: battle_dc += value; break;
                            case enum_equip_entry_list.魔攻属性: battle_sc += value; break;
                            case enum_equip_entry_list.道攻属性: battle_mc += value; break;
                            case enum_equip_entry_list.攻击速度: battle_speed -= (value * speed_bonus); break;
                            case enum_equip_entry_list.攻击范围: battle_range += value; break;
                            case enum_equip_entry_list.暴击属性: crit += value; break;
                            case enum_equip_entry_list.暴击伤害: critDmg += value; break;

                            case enum_equip_entry_list.烈阳文:
                            case enum_equip_entry_list.盾护文:
                            case enum_equip_entry_list.守月文:
                            case enum_equip_entry_list.幽狼文:
                            case enum_equip_entry_list.神行文:
                            case enum_equip_entry_list.怒目文:
                            case enum_equip_entry_list.震火文:
                            case enum_equip_entry_list.金刚文:
                            case enum_equip_entry_list.大愈文:
                            case enum_equip_entry_list.回春文:
                            case enum_equip_entry_list.回心文:
                            case enum_equip_entry_list.峰芒文:
                            case enum_equip_entry_list.破枪文:
                            case enum_equip_entry_list.深寒文:
                            case enum_equip_entry_list.瑶光文:
                                break;
                            case enum_equip_entry_list.幸运: lucky += value; break;
                            case enum_equip_entry_list.闪避: dodge += value; break;
                            case enum_equip_entry_list.命中: hit += value; break;
                            case enum_equip_entry_list.物伤减免: damage_reduction += value; break;
                            case enum_equip_entry_list.魔伤减免: magic_damage_reduction += value; break;
                            case enum_equip_entry_list.怪物爆率: drop_bonus += value; break;
                            case enum_equip_entry_list.极品爆率: quality_bonus += value; break;
                            case enum_equip_entry_list.经验加成: exp_bonus += value; break;
                            case enum_equip_entry_list.金币掉落: gold_bonus += value; break;
                            default:
                                break;
                        }

                    }
                }
            }
        }
        //宠物加成
        db_pet_vo pet = SumSave.crt_pet.GetPet;
        if (pet != null)
        {
            ac += 1;
            mac += 1;
            dc += 1;
            mc += 1;
            sc += 1;
            (int,int,int,int,int) pet_attr = pet.GetCrtAttr;
            (int, int, int, int, int) pet_add_attr = pet.GetAddAttr;
            List<db_pet_talent_vo> talent_list = pet.GetCrtTalent;
            Bag_Base_VO bag = ArrayHelper.Find(SumSave.db_stditems, e => e.Name == pet.pet_name);
            ac2 += bag.ac2 + pet_attr.Item1 + pet_add_attr.Item1;
            mac2 += bag.mac2 + pet_attr.Item2 + pet_add_attr.Item2;
            dc2 += bag.dc2 + pet_attr.Item3 + pet_add_attr.Item3;
            mc2 += bag.mc2 + pet_attr.Item4 + pet_add_attr.Item4;
            sc2 += bag.sc2 + pet_attr.Item5 + pet_add_attr.Item5;
            foreach (var talent in talent_list)
            {
                (enum_battle_pet_talent_list, int, int) D = (enum_battle_pet_talent_list.任意门, 0, 0);
                switch (talent.pet_talent_type)
                {
                    
                    case 3:
                        switch ((talent.pet_talent_offect))
                        {
                            case 1: battle_hp+=(int)(talent.pet_talent_offectvalue) ; break;
                            case 2:
                                dc2+= (int)(talent.pet_talent_offectvalue * SumSave.crtHero.lv);
                                mc2 += (int)(talent.pet_talent_offectvalue * SumSave.crtHero.lv);
                                sc2 += (int)(talent.pet_talent_offectvalue * SumSave.crtHero.lv); break;
                           case 3: D = new (enum_battle_pet_talent_list.任意门, talent.pet_talent_offecttype,1);
                                if(!talentList.Contains(D)) talentList.Add(D); break;
                            case 4:
                                 D = (enum_battle_pet_talent_list.嗜血追击, talent.pet_talent_offecttype, 10);
                                if (!talentList.Contains(D)) talentList.Add(D); break;
                            case 5: D = (enum_battle_pet_talent_list.破壁一击, talent.pet_talent_offecttype, 100);
                                if (!talentList.Contains(D)) talentList.Add(D); break;
                            case 6: D = (enum_battle_pet_talent_list.华山斩, talent.pet_talent_offecttype, 10);
                                if (!talentList.Contains(D)) talentList.Add(D); break;
                            case 7: D = (enum_battle_pet_talent_list.斩杀, talent.pet_talent_offecttype, 10);
                                if (!talentList.Contains(D)) talentList.Add(D); break;
                            case 8: D = (enum_battle_pet_talent_list.连击效果, talent.pet_talent_offecttype, 100);
                                if (!talentList.Contains(D)) talentList.Add(D); break;
                        }
                        break;
                    case 1:
                        switch ((talent.pet_talent_offect))
                        {

                            case 1:
                                battle_dc+= (int)(talent.pet_talent_offectvalue); break;
                            case 2: 
                                battle_mc+= (int)(talent.pet_talent_offectvalue); break;
                            //case 3: dec += "召唤兽伤害 + " + Show_Color.Red(talent.pet_talent_offectvalue) + " %"; break;
                            case 4:
                                ac2+= (int)(talent.pet_talent_offectvalue * SumSave.crtHero.lv);break;
                            case 5: 
                                mac2+= (int)(talent.pet_talent_offectvalue * SumSave.crtHero.lv); break;
                            case 6:
                                hpRegen += (int)(talent.pet_talent_offectvalue * SumSave.crtHero.lv); break;
                            case 7: 
                                mpRegen += (int)(talent.pet_talent_offectvalue * SumSave.crtHero.lv); break;
                            case 8:
                                damage_reduction+= (int)(talent.pet_talent_offectvalue); break;
                            case 9:
                                magic_damage_reduction += (int)(talent.pet_talent_offectvalue); break;
                            case 11: dodge +=(int)(talent.pet_talent_offectvalue) ; break;
                            default:
                                break;
                        }
                        break;
                    case 2:
                        switch ((talent.pet_talent_offect))
                        {
                            case 1:
                                D = ((enum_battle_pet_talent_list)(talent.pet_talent_job + 6), talent.pet_talent_offecttype, (int)talent.pet_talent_offectvalue);
                                if (!talentList.Contains(D)) talentList.Add(D); break;
                            case 2:
                            case 3:
                            case 4:
                                D = ((enum_battle_pet_talent_list)(talent.pet_talent_offect + 8), talent.pet_talent_offecttype, (int)talent.pet_talent_offectvalue * SumSave.crtHero.lv);
                                if (!talentList.Contains(D)) talentList.Add(D); break;
                            case 5:
                            case 6: 
                            case 7: 
                            case 8: 
                                D = ((enum_battle_pet_talent_list)(talent.pet_talent_offect + 8), talent.pet_talent_offecttype, (int)talent.pet_talent_offectvalue);
                                if (!talentList.Contains(D)) talentList.Add(D); break;
                            case 10:
                                D = (enum_battle_pet_talent_list.慧根, talent.pet_talent_offecttype, (int)talent.pet_talent_offectvalue);
                                if (!talentList.Contains(D)) talentList.Add(D); break;
                        }
                        break;
                }
            }
        }
        if (talentList.Count > 0)
        {
            bool exist = false;
            foreach (var item in talentList)
            {
                if (item.Item1 == enum_battle_pet_talent_list.连击效果)
                {
                    exist = true;
                    break;
                } 
            }
            if (exist)
            {
                for (int i = 0; i < talentList.Count; i++)
                {
                    if (talentList[i].Item1 == enum_battle_pet_talent_list.连击 || talentList[i].Item1 == enum_battle_pet_talent_list.法连 || talentList[i].Item1 == enum_battle_pet_talent_list.道连)
                    {
                       talentList[i] = (talentList[i].Item1, talentList[i].Item2, 100);
                    }
                }
            }
        }
        if (crt_vip == null) Obtain_Vip();
        if (crt_vip != null)
        { 
            exp_bonus += crt_vip.lingzhuIncome;
            gold_bonus += crt_vip.experienceBonus;
            drop_bonus += crt_vip.equipmentExplosionRate;
            boss_cd += crt_vip.monsterHuntingInterval;
        }
        List<(string, string, int)> buffs = SumSave.crt_user_unit.GetBuff;
        for (int i = 0; i < buffs.Count; i++)
        {
            int spanSeconds = Battle_Tool.SettlementTransport(buffs[i].Item2, 3);
            int time = buffs[i].Item3 - spanSeconds;//剩余时间
            if (time > 0|| buffs[i].Item3>=99999)
            {
                if (buffs[i].Item1 == common_Buff.狂欢.ToString())
                {
                    exp_bonus += 10;
                    gold_bonus += 10;
                }
                if (buffs[i].Item1 == common_Buff.双倍经验卷轴.ToString())
                {
                    exp_bonus += 100;
                }
                if (buffs[i].Item1 == common_Buff.月卡.ToString())
                {
                    exp_bonus += 20;
                    gold_bonus += 20;
                    drop_bonus += 5; 
                    boss_cd += 5;
                }
            }
        }
        maxhp = hp * (100 + battle_hp) / 100;
        maxmp = mp * (100 + battle_mp) / 100;
        ac= ac * (100 + battle_ac) / 100;
        ac2 = ac2 * (100 + battle_ac) / 100;
        mac = mac * (100 + battle_mac) / 100;
        mac2 = mac2 * (100 + battle_mac) / 100;
        dc = dc * (100 + battle_dc) / 100;
        dc2 = dc2 * (100 + battle_dc) / 100;
        sc = sc * (100 + battle_sc) / 100;
        sc2 = sc2 * (100 + battle_sc) / 100;
        mc = mc * (100 + battle_mc) / 100;
        mc2 = mc2 * (100 + battle_mc) / 100;
        crtMaxBattleVO crt = new crtMaxBattleVO(exp_bonus, gold_bonus, drop_bonus, quality_bonus, boss_cd);
        crt.crt_name = SumSave.crtHero.hero_name;
        crt.lv = SumSave.crtHero.lv;
        crt.exp = SumSave.crtHero.exp; 
        crt.hero_type = (Hero_Type)SumSave.crtHero.job;
        crt.type = Battle_Game_Type.player;
        crt.data = new FinalBattleValueVO(maxhp, maxmp, hp, mp, dc, dc2, mac, mac2, ac, ac2, sc, sc2, mc, mc2, hit, dodge, crit, critDmg, hpRegen,
            mpRegen, battle_hp, battle_mp, battle_ac, battle_mac, battle_dc, battle_sc, battle_mc, battle_speed, battle_range, battle_Damage, battle_def, talentList, lucky,damage_reduction,magic_damage_reduction,0);
        return crt;
    }
    /// <summary>
    /// 当前vip
    /// </summary>
    private static db_vip crt_vip;
    /// <summary>
    /// 获取当前vip等级
    /// </summary>
    /// <returns></returns>
    public static db_vip Obtain_Vip()
    {
        if (crt_vip != null) return crt_vip;
        int sum = (int.Parse)(SumSave.crt_global_gift.GetGiftPoints);
        if (sum == 0) return crt_vip;
        for (int i = 0; i < SumSave.db_vip_list.Count; i++)
        {
            if (sum >= SumSave.db_vip_list[i].vip_exp)
            {
                crt_vip = SumSave.db_vip_list[i];
            }
        }
        return crt_vip;
    }
    /// <summary>
    /// 判断是否拥有buff
    /// </summary>
    /// <param name="buff"></param>
    /// <returns></returns>
    public static bool IsBuff(common_Buff buff)
    {
        List<(string, string, int)> buffs = SumSave.crt_user_unit.GetBuff;
        for (int i = 0; i < buffs.Count; i++)
        {
            int spanSeconds = Battle_Tool.SettlementTransport(buffs[i].Item2, 3);
            int time = buffs[i].Item3 - spanSeconds;//剩余时间
            if (time > 0 || buffs[i].Item3 >= 99999)
            {
                if (buff.ToString() == buffs[i].Item1)
                {
                    return true;
                }
            }
        }
        return false;

    }
    /// <summary>
    /// 初始化vip
    /// </summary>
    public static void Crate_Vip()
    {
        int sum = (int.Parse)(SumSave.crt_global_gift.GetGiftPoints);
        if (sum == 0) return;
        for (int i = 0; i < SumSave.db_vip_list.Count; i++)
        {
            if (sum >= SumSave.db_vip_list[i].vip_exp)
            {
                crt_vip = SumSave.db_vip_list[i];
            }
        }
    }


    private static Dictionary<string, (int,string)> map_boss_time = new Dictionary<string, (int, string)>();
    /// <summary>
    /// 读取boss刷新时间
    /// </summary>
    public static void Carte_Read_Boss_Time()
    {
        Dictionary<string, (int,string)> dic = new Dictionary<string, (int, string)>();
        DateTime now = SumSave.nowtime >= DateTime.Now ? SumSave.nowtime : DateTime.Now;
        string value = Tool_UI.ToStandardFormat(now);
        for (int i = 0; i < SumSave.db_maps.Count; i++)
        {
            if (SumSave.db_maps[i].map_type == 0)
            {
                for (int j = 0; j < SumSave.db_maps[i].map_boss.Count; j++)
                {
                    if (!dic.ContainsKey(SumSave.db_maps[i].map_boss[j]))
                    {
                        //dic.Add(SumSave.db_maps[i].map_boss[j], new Dictionary<int, string>());
                        dic[SumSave.db_maps[i].map_boss[j]] = (SumSave.db_maps[i].map_boss_cdtime[j], value);
                    }
                }
            }
        }
        map_boss_time = dic;
    }
    /// <summary>
    /// 获取当前地图boss刷新时间
    /// </summary>
    /// <param name="map_id"></param>
    /// <returns></returns>
    public static (int,string) GetBossTime(string map_id) { return (map_boss_time.ContainsKey(map_id))? map_boss_time[map_id]:(99999,"no"); }
    /// <summary>
    /// 更新boss刷新时间
    /// </summary>
    /// <param name="map_id"></param>
    /// <param name="time"></param>
    /// <param name="value"></param>
    public static void SetBossTime(string map_id, int time, string value)
    { 
        map_boss_time[map_id] = (time, value);
    }
    /// <summary>
    /// 创建召唤战斗数据
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns>
    public static crtMaxBattleVO Crate_Call(db_skill_vo skill)
    {
        int exp_bonus = 0, gold_bonus = 0, drop_bonus = 0, quality_bonus = 0;
        int maxhp = 0, maxmp = 0;
        int battle_hp = 0, battle_mp = 0, battle_ac = 0, battle_mac = 0, battle_dc = 0, battle_sc = 0, battle_mc = 0, battle_speed = 0, battle_range = 0, battle_Damage = 0, battle_def = 0;
        int hp = 0, mp = 0, dc = 0, dc2 = 0, mac = 0, mac2 = 0, ac = 0, ac2 = 0, sc = 0, sc2 = 0, mc = 0, mc2 = 0;
        int hit = 0, dodge = 0, crit = 0, critDmg = 100;
        int hpRegen = 0, mpRegen = 0;
        int lucky = 0, damage_reduction = 0, magic_damage_reduction = 0;
        List<(enum_battle_pet_talent_list, int, int)> talentList = new List<(enum_battle_pet_talent_list, int, int)>();
        //str += "召唤 " + Show_Color.Set_String(crt_skill.show_name, color_list) + "\n继承" + Show_Color.Set_String((crt_skill.Power + crt_skill.DefPowers[i]) + " %属性" + " ", color_list);
        //if (crt_skill.skill_damages.Count > 0) str += "[召唤兽伤害] " + Show_Color.Set_String(crt_skill.skill_damages[i], color_list) + ";";
        int lv = Mathf.Min(skill.DefPowers.Count - 1, skill.SetLv());
        int power = (skill.Power + skill.DefPowers[lv]);
        maxhp = (int)SumSave.crtMaxBattle.data.battle_maxhp * (power) / 100;
        maxmp = (int)SumSave.crtMaxBattle.data.battle_maxmp * (power) / 100;
        hp = (int)SumSave.crtMaxBattle.data.battle_hp * (power) / 100;
        mp = (int)SumSave.crtMaxBattle.data.battle_mp * (power) / 100;
        ac = (int)SumSave.crtMaxBattle.data.ac * (power) / 100;
        ac2 = (int)SumSave.crtMaxBattle.data.ac2 * (power) / 100;
        mac = (int)SumSave.crtMaxBattle.data.mac * (power) / 100;
        mac2 = (int)SumSave.crtMaxBattle.data.mac2 * (power) / 100;
        dc = (int)SumSave.crtMaxBattle.data.dc * (power) / 100;
        dc2 = (int)SumSave.crtMaxBattle.data.dc2 * (power) / 100;
        sc = (int)SumSave.crtMaxBattle.data.sc * (power) / 100;
        sc2 = (int)SumSave.crtMaxBattle.data.sc2 * (power) / 100;
        mc = (int)SumSave.crtMaxBattle.data.mc * (power) / 100;
        mc2 = (int)SumSave.crtMaxBattle.data.mc2 * (power) / 100;
        hit = (int)SumSave.crtMaxBattle.data.hit * (power) / 100;
        dodge = (int)SumSave.crtMaxBattle.data.dodge * (power) / 100;
        crit = (int)SumSave.crtMaxBattle.data.crit * (power) / 100;
        critDmg = (int)SumSave.crtMaxBattle.data.critDmg * (power) / 100;
        hpRegen = (int)SumSave.crtMaxBattle.data.hpRegen * (power) / 100;
        mpRegen = (int)SumSave.crtMaxBattle.data.mpRegen * (power) / 100;
        battle_hp = (int)SumSave.crtMaxBattle.data.battle_hp * (power) / 100;
        battle_mp = (int)SumSave.crtMaxBattle.data.battle_mp * (power) / 100;
        battle_ac = (int)SumSave.crtMaxBattle.data.battle_ac * (power) / 100;
        battle_mac = (int)SumSave.crtMaxBattle.data.battle_mac * (power) / 100;
        battle_dc = (int)SumSave.crtMaxBattle.data.battle_dc * (power) / 100;
        battle_sc = (int)SumSave.crtMaxBattle.data.battle_sc * (power) / 100;
        battle_mc = (int)SumSave.crtMaxBattle.data.battle_mc * (power) / 100;
        battle_speed = (int)SumSave.crtMaxBattle.data.battle_speed;
        battle_range = (int)SumSave.crtMaxBattle.data.battle_range * (power) / 100;
        battle_Damage = (int)SumSave.crtMaxBattle.data.battle_Damage * (power) / 100 + skill.skill_damages[lv];//真实伤害
        battle_def = (int)SumSave.crtMaxBattle.data.battle_def * (power) / 100;
        lucky = (int)SumSave.crtMaxBattle.data.lucky * (power) / 100;
        damage_reduction = (int)SumSave.crtMaxBattle.data.damage_reduction * (power) / 100;
        magic_damage_reduction = (int)SumSave.crtMaxBattle.data.magic_damage_reduction * (power) / 100;
        Dictionary<enum_talent_offect_list, int> buff = skill.GetBuff;
        foreach (var item in buff.Keys)
        {
            switch (item)
            {
                
                case enum_talent_offect_list.命中: hit += buff[item]; break;
                    break;
                case enum_talent_offect_list.召唤兽攻击:dc2+= buff[item]; sc2+= buff[item];mc2 += buff[item]; break;
                    break;
                case enum_talent_offect_list.召唤兽生命:maxhp += buff[item]; break;
                    break;
                case enum_talent_offect_list.召唤兽防御:ac2 += buff[item]; mac2 += buff[item]; break;
                    break;
                case enum_talent_offect_list.召唤兽速度:
                    battle_speed-= buff[item];
                    break;
                case enum_talent_offect_list.召唤兽死亡爆炸:
                    break;
                case enum_talent_offect_list.特殊效果:
                    break;
                case enum_talent_offect_list.临时伤害:
                    break;
                case enum_talent_offect_list.临时防御:
                    break;
                case enum_talent_offect_list.临时速度:
                    break;
                case enum_talent_offect_list.单体改群体:
                    break;
                case enum_talent_offect_list.技能攻击个数:
                    break;
                case enum_talent_offect_list.技能概率不消耗蓝:
                    break;
                case enum_talent_offect_list.技能全体伤害:
                    break;
                case enum_talent_offect_list.群体技能攻击范围:
                    break;
                case enum_talent_offect_list.每秒回复全体血量百分比:
                    break;
                case enum_talent_offect_list.攻击击退敌人概率:
                    break;
                case enum_talent_offect_list.弹道:
                    break;
            }
        }

        crtMaxBattleVO crt = new crtMaxBattleVO(exp_bonus, gold_bonus, drop_bonus, quality_bonus);
        crt.crt_name = "召" + skill.show_name;
        crt.lv = skill.SetLv();
        crt.exp = 0;
        crt.hero_type = (Hero_Type)skill.Effect;
        crt.type = Battle_Game_Type.call;
        //maxhp = 1; hp = 1; maxmp = 1; mp = 1; 测试
        crt.data = new FinalBattleValueVO(maxhp, maxmp, hp, mp, dc, dc2, mac, mac2, ac, ac2, sc, sc2, mc, mc2, hit, dodge, crit, critDmg, hpRegen,
            mpRegen, battle_hp, battle_mp, battle_ac, battle_mac, battle_dc, battle_sc, battle_mc, battle_speed, battle_range, battle_Damage, battle_def, talentList, lucky, damage_reduction, magic_damage_reduction, 0);
        return crt;
    }
    /// <summary>
    /// 根据技能id获取技能
    /// </summary>
    /// <param name="talent"></param>
    /// <param name="skill_list"></param>
    /// <param name="skill_lv"></param>
    /// <returns></returns>
    private static int tool_talent_value((db_player_talent_vo,int) talent, Dictionary<int, db_skill_vo> skill_list, int skill_lv=0)
    {
        int value = 0;
        if (talent.Item1.correlation_skill == -1)
        {
            value += talent.Item1.talent_offect_value[talent.Item2];
        }
        else
        {
            foreach (var item in skill_list)
            {
                skill_lv = item.Value.SetLv();
                if (item.Value.id == talent.Item1.correlation_skill)
                {
                    value += talent.Item1.talent_offect_value[talent.Item2] * skill_lv;
                }
            }
        }
        return value;
    }
    /// <summary>
    /// 添加技能buff
    /// </summary>
    /// <param name="talent">天赋</param>
    /// <param name="skill_list">技能列表</param>
    /// <param name="isSkill">是否跟随技能等级</param>
    private static void AddSkillBuff((db_player_talent_vo, int) talent, Dictionary<int, db_skill_vo> skill_list,bool isSkill=false)
    {
        int value = 0;
        value += talent.Item1.talent_offect_value[talent.Item2];
        if (talent.Item1.correlation_skill == -1)
        {
            foreach (var item in skill_list)
            {
                item.Value.AddBuff((enum_talent_offect_list)(talent.Item1.talent_offect), value);
            }
        }
        else
        {
            foreach (var item in skill_list)
            {
                int skill_lv = item.Value.SetLv();
                if (item.Value.id == talent.Item1.correlation_skill)
                {
                    item.Value.AddBuff((enum_talent_offect_list)(talent.Item1.talent_offect), value * (isSkill ? skill_lv : 1));
                }
            }
        }
    }
    /// <summary>
    /// 技能附加弹道
    /// </summary>
    /// <param name="id"></param>
    /// <param name="skill_list"></param>
    private static void AddEquipSkillBuff(int id, Dictionary<int, db_skill_vo> skill_list,int value)
    {
        foreach (var item in skill_list)
        {
            if (item.Value.id == id)
            {
                item.Value.AddBuff(enum_talent_offect_list.弹道, value);
            }
        }
        
    }

    /// <summary>
    /// 判断字符串是否合法
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsValidString(string input)
    {
        // 允许：字母（大小写）、数字、下划线、常见中文
        // 正则说明：^ 开头，$ 结尾，+ 一个或多个
        Regex regex = new Regex(@"^[\w\u4e00-\u9fa5]+$");
        return regex.IsMatch(input);
    }
    private static void Obtain_enum_equip_entry_list(enum_equip_entry_list entry,int value)
    { 
    
    }
    private static void Obtain_Init_Entry_list()
    {
        /*
            * 生命值 = 14,
   魔法值 = 66,
   物理防御 = 4,
   魔法防御 = 19,
   物理攻击 = 17,
   魔法攻击 = 27,
   道术攻击 = 28,
   回血=20,
   回蓝=21,
   真伤 = 36,
   吸伤 = 37,

   物理下防=101,
   魔法下防,
   物理下攻,
   魔法下攻,
   道术下攻,
            * 
            */
        entry_list = new List<enum_equip_entry_list>();
        entry_list.Add(enum_equip_entry_list.生命值);
        entry_list.Add(enum_equip_entry_list.魔法值);
        entry_list.Add(enum_equip_entry_list.物理防御);
        entry_list.Add(enum_equip_entry_list.魔法防御);
        entry_list.Add(enum_equip_entry_list.物理攻击);
        entry_list.Add(enum_equip_entry_list.魔法攻击);
        entry_list.Add(enum_equip_entry_list.道术攻击);
        entry_list.Add(enum_equip_entry_list.每秒回血);
        entry_list.Add(enum_equip_entry_list.每秒回蓝);
        entry_list.Add(enum_equip_entry_list.真实伤害);
        entry_list.Add(enum_equip_entry_list.吸收伤害);
        entry_list.Add(enum_equip_entry_list.物理下防);
        entry_list.Add(enum_equip_entry_list.魔法下防);
        entry_list.Add(enum_equip_entry_list.物理下攻);
        entry_list.Add(enum_equip_entry_list.魔法下攻);
        entry_list.Add(enum_equip_entry_list.道术下攻);
    }

    private static void Obtain_Init_Entry_Coefficient_list()
    {
        /*
      生命属性=22,
    魔法属性=23,
    防御属性=24,
    魔防属性=25,
    物攻属性=26,
    魔攻属性=27,
    道攻属性=28,
    攻击速度=40,
    攻击范围=41,
    暴击属性=42,
    暴击伤害=43,
    命中,
    闪避,
            * 
            */
        entry_coefficient_list = new List<enum_equip_entry_list>();
        entry_coefficient_list.Add(enum_equip_entry_list.生命属性);
        entry_coefficient_list.Add(enum_equip_entry_list.魔法属性);
        entry_coefficient_list.Add(enum_equip_entry_list.防御属性);
        entry_coefficient_list.Add(enum_equip_entry_list.魔防属性);
        entry_coefficient_list.Add(enum_equip_entry_list.物攻属性);
        entry_coefficient_list.Add(enum_equip_entry_list.魔攻属性);
        entry_coefficient_list.Add(enum_equip_entry_list.道攻属性);
        entry_coefficient_list.Add(enum_equip_entry_list.攻击速度);
        entry_coefficient_list.Add(enum_equip_entry_list.攻击范围);
        entry_coefficient_list.Add(enum_equip_entry_list.暴击属性);
        entry_coefficient_list.Add(enum_equip_entry_list.暴击伤害);
        entry_coefficient_list.Add(enum_equip_entry_list.命中);
        entry_coefficient_list.Add(enum_equip_entry_list.闪避);
        entry_coefficient_list.Add(enum_equip_entry_list.金币掉落);

    }

    private static void Obtain_Init_Entry_Inscription_list()
    {
        /*
         烈阳文,
    盾护文,
    守月文,
    幽狼文,
    神行文,
    怒目文,
    震火文,
    金刚文,
    大愈文,
    回春文,
    回心文,
    峰芒文,
    破枪文,
    深寒文,
    瑶光文,
               * 
               */
        entry_inscription_list= new List<enum_equip_entry_list>();
        entry_inscription_list.Add(enum_equip_entry_list.烈阳文);
        entry_inscription_list.Add(enum_equip_entry_list.盾护文);
        entry_inscription_list.Add(enum_equip_entry_list.守月文);
        entry_inscription_list.Add(enum_equip_entry_list.幽狼文);
        entry_inscription_list.Add(enum_equip_entry_list.神行文);
        entry_inscription_list.Add(enum_equip_entry_list.怒目文);
        entry_inscription_list.Add(enum_equip_entry_list.震火文);
        entry_inscription_list.Add(enum_equip_entry_list.金刚文);
        entry_inscription_list.Add(enum_equip_entry_list.大愈文);
        entry_inscription_list.Add(enum_equip_entry_list.回春文);
        entry_inscription_list.Add(enum_equip_entry_list.回心文);
        entry_inscription_list.Add(enum_equip_entry_list.峰芒文);
        entry_inscription_list.Add(enum_equip_entry_list.破枪文);
        entry_inscription_list.Add(enum_equip_entry_list.深寒文);
        entry_inscription_list.Add(enum_equip_entry_list.瑶光文);
        for (int i = 0; i < SumSave.db_skills.Count; i++)
        {
            if (SumSave.db_skills[i].EffectType != 5 && SumSave.db_skills[i].EffectType != 6)
            {
                Obtain_Weight(SumSave.db_skills[i].id+1000, SumSave.db_skills[i].Weighted);
            }
        }
        
    }
    /// <summary>
    /// 获取装备属性
    /// </summary>
    /// <returns></returns>
    public static int Equip_Skill_eight()
    {
        if (equip_eighteditems.Count == 0) Obtain_Init_Entry_Inscription_list();
        WeightedRandomPicker picker = new WeightedRandomPicker(equip_eighteditems);
        // 获取一个概率
        WeightedItem selectedItem = picker.GetRandomItem();

        return int.Parse(selectedItem.prizedraw.ToString());
    }
    private static void Obtain_Weight(object seed_name, int weight)
    {
        WeightedItem weightedItem = new WeightedItem(seed_name,weight);
        equip_eighteditems.Add(weightedItem);
    }

    /// <summary>
    /// 获取装备类型
    /// </summary>
    /// <param name="lists"></param>
    /// <returns></returns>
    private static T Obtain_Enum_list<T>(List<T> lists)
    {
        return lists[Random.Range(0, lists.Count)];
    }
    /// <summary>
    /// 获取装备属性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dics"></param>
    /// <param name="type"></param>
    /// <param name="value"></param>
    private static void Obtain_value<T>(Dictionary<T, int> dics,T type,int value = 1,bool exist=true,List<T> lists = null)
    {
        if (exist)
        {
            if (!dics.ContainsKey(type)) dics.Add(type, 0);
            dics[type] += value;
        }
        else
        {
            int number = 0;
            while (dics.ContainsKey(type))
            { 
                type = Obtain_Enum_list<T>(lists);
                number++;
                if (number >= 100) return;
            }
            dics.Add(type, 0);
            dics[type] += value;
        }
        
    }

    /// <summary>
    /// 获取材料
    /// </summary>
    /// <param name="type"></param>
    /// <param name="index"></param>
    /// <param name="maxnumber"></param>
    /// <param name="isverify"></param>
    public static void Obtain_Resources(int index, in int maxnumber, bool isverify = false)
    {
        SumSave.crt_bags.Get(Obtain_Int.Get(index), maxnumber, isverify);
    }

    /// <summary>
    /// 获取货币
    /// </summary>
    /// <param name="unit">类型</param>
    /// <param name="value">值</param>
    /// <param name="state">状态1可加成2直接获取</param>
    public static int Obtain_Unit(currency_unit unit, int value, int state = 1)
    {
        if (state == 1)
        {
            if (unit == currency_unit.元宝)
            {
               
            }
            if (unit == currency_unit.金币)
            {
                value =value * (100 + Tool_State.Value_playerprobabilit(enum_equip_entry_list.金币掉落)) / 100;
            }
        }
        SumSave.crt_user_unit.verify_data(unit, value);

        return value;
    }

    /// <summary>
    /// 获取权重比例
    /// </summary>
    private static int[] QualityWeighted = new int[] { 5000, 3000, 2000, 1500, 500, 200, 50, 10, 10, 10 };
    /// <summary>
    /// 权重列表
    /// </summary>
    private static List<WeightedItem> eighteditems = new List<WeightedItem>();
    /// <summary>
    /// 获取权重基准
    /// </summary>
    public static void InitEighted()
    {
        eighteditems.Clear();
        for (int i = 0; i < Enum.GetNames(typeof(enum_equip_quality_list)).Length; i++)
        {
            
            WeightedItem item = new WeightedItem(i + 1, QualityWeighted[i]);
            eighteditems.Add(item);
        }
    }
    /// <summary>
    /// 获取装备品质
    /// </summary>
    /// <param name="boss"></param>
    /// <returns></returns>
    public static int Quality()
    {
        if (eighteditems.Count == 0) InitEighted();
        WeightedRandomPicker picker = new WeightedRandomPicker(eighteditems);
        // 获取一个概率
        WeightedItem selectedItem = picker.GetRandomItem();
        
        return int.Parse( selectedItem.prizedraw.ToString());
    }
    /// <summary>
    /// 获取装备数值
    /// </summary>
    /// <param name="bag"></param>
    /// <param name="lv"></param>
    /// <param name="quality"></param>
    /// <param name="islock"></param>
    /// <returns></returns>
    public static string Obtain_Equip(Bag_Base_VO bag, int lv, int quality, int islock = 0)
    {
        //获取可以被抽取的列表
        if (entry_list == null) Obtain_Init_Entry_list(); 
        if (entry_coefficient_list == null) Obtain_Init_Entry_Coefficient_list();
        if (entry_inscription_list == null) Obtain_Init_Entry_Inscription_list();
        string user_value = bag.Name;
        //强化等级
        user_value += " " + lv;
        //品质
        user_value += " " + quality;
        //是否可以锁定
        user_value += " " + islock;

        Dictionary<enum_equip_entry_list, int> dics = new Dictionary<enum_equip_entry_list, int>();
        Dictionary<enum_equip_entry_list, int> dics_2 = new Dictionary<enum_equip_entry_list, int>();
        List<int> dics_3 = new List<int>();
        if (quality > 1)
        {
            Obtain_value(dics, Obtain_Enum_list(entry_list));
            if (quality > 1)
            {
                Obtain_value(dics, Obtain_Enum_list(entry_list));
                if (quality > 2)
                {
                    Obtain_value(dics, Obtain_Enum_list(entry_list));
                    Obtain_value(dics_2, Obtain_Enum_list(entry_coefficient_list), Random.Range(1, 100) < 30 ? 2 : 1,false, entry_coefficient_list);
                    if (quality > 3)
                    {
                        Obtain_value(dics, Obtain_Enum_list(entry_list));
                        if (quality > 4)
                        {
                            Obtain_value(dics, Obtain_Enum_list(entry_list));
                            Obtain_value(dics_2, Obtain_Enum_list(entry_coefficient_list), Random.Range(1, 100) < 30 ? 2 : 1, false, entry_coefficient_list);

                            if (quality > 5)
                            {
                                Obtain_value(dics, Obtain_Enum_list(entry_list));
                                if (Random.Range(0, 100) < 10)
                                {
                                    dics_3.Add(Equip_Skill_eight());
                                }
                                if (quality >= 6)
                                {
                                    if (bag.Name == "新手剑")
                                    {
                                        dics_3.Add(1001);
                                        dics_3.Add(1002);
                                        dics_3.Add(1008);
                                        dics_3.Add(1014);
                                    }
                                    else
                                    {
                                        Obtain_value(dics, Obtain_Enum_list(entry_list));
                                        Obtain_value(dics_2, Obtain_Enum_list(entry_coefficient_list), Random.Range(1, 100) < 30 ? 2 : 1, false, entry_coefficient_list);
                                        dics_3.Add(Equip_Skill_eight());
                                    }
                                   
                                }
                            }
                        }
                    }
                }
            }
            string value = "";
            bool exist = true;
            if (dics.Count > 0)
            {
                foreach (var item in dics)
                {
                    if (exist)
                    {

                        exist = false;
                    }
                    else value += "|";
                    value += (int)item.Key + "," + item.Value ;
                }
            }
            exist = true;
            if (dics_2.Count > 0)
            {
                foreach (var item in dics_2)
                {
                    if (exist)
                    {
                        value += "X";
                        exist = false;
                    }
                    else value += "|";
                    value += (int)item.Key + "," + item.Value;
                }
            }
            exist = true;
            if (dics_3.Count > 0)
            {
                for (int i = 0; i < dics_3.Count; i++)
                {
                    if (exist)
                    {
                        value += "X";
                        exist = false;
                    }
                    else value += "|";
                    value += dics_3[i] + "," + 1;
                }
            }
            user_value+= " " + value;
        }

        return user_value;
    }

    /// <summary>
    /// 创建装备
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="lv">强化等级</param>
    /// <param name="quality">品质等级</param>
    /// <param name="islock">锁定</param>
    /// <returns></returns>
    public static Bag_Base_VO Crate_Equip(string name, int lv, int quality, int islock = 0)
    {
        Bag_Base_VO bag = new Bag_Base_VO();
        Bag_Base_VO base_bag = ArrayHelper.Find(SumSave.db_stditems, (item) => item.Name == name);
        string user_value = Obtain_Equip(base_bag, lv, quality, islock);
        bag = tool_Categoryt.Read_BaseBag(user_value);
        return bag;
    }
    /// <summary>
    /// 生成宠物
    /// </summary>
    /// <param name="pet"></param>
    /// <returns></returns>
    public static string Obtain_Pet(int index,int crate_skill_random=0)
    {
        db_pet_vo base_pet = ArrayHelper.Find(SumSave.db_pets, (item) => item.pet_id == index);
        db_pet_vo pet = new db_pet_vo(base_pet.pet_id, base_pet.pet_name, base_pet.pet_ac, base_pet.pet_mac, base_pet.pet_dc, base_pet.pet_mc, base_pet.pet_sc, base_pet.pet_talent, base_pet.pet_scale);
        //0:可修改名称 1本命 2id 3基础数值 4极品值5技能
        string user_value = pet.pet_id + ","+pet.pet_name + "," + pet.pet_name + ",";
        user_value += Random.Range(1, pet.pet_ac / 3) + "X" + Random.Range(1, pet.pet_mac / 3) + "X"
            + Random.Range(1, pet.pet_dc / 3) + "X" + Random.Range(1, pet.pet_mc / 3) + "X" + Random.Range(1, pet.pet_sc / 3) + ",";
        for (int i = 0; i < 5; i++)
        {
            user_value += Random.Range(0, pet.pet_id + 5) + (i== 4 ? "," : "X");
        }
        //添加技能 多个技能X分隔
        int skill_random = Random.Range(1, pet.pet_id / 3 + 1);
        if (crate_skill_random > 0) skill_random = crate_skill_random;
        user_value += pet.pet_talent;
        if (skill_random > 0)
        {
            user_value += Radom_Talent(skill_random);
        }
        user_value+= ",";
        return user_value;
    }
    /// <summary>
    /// 随机生成天赋
    /// </summary>
    /// <param name="max"></param>
    /// <returns></returns>
    private static string Radom_Talent(int max)
    { 
        string value = "";
        List<db_pet_talent_vo> list = ArrayHelper.FindAll(SumSave.db_pet_talents, e => e.pet_talent_level == 1);
        List<db_pet_talent_vo> list1 = ArrayHelper.FindAll(SumSave.db_pet_talents, e => e.pet_talent_level == 2);
        string random = "";
        for (int i = 0; i < max; i++)
        {
            int number = 0;
            if (Random.Range(0, 100) < 20)
            {
                random = list1[Random.Range(0, list1.Count)].pet_talent_name;
            }
            else random = list[Random.Range(0, list.Count)].pet_talent_name;

            while (value.Contains(random))
            {
                if (Random.Range(0, 100) < 20)
                {
                    random= list1[Random.Range(0, list1.Count)].pet_talent_name;
                }
                else random = list[Random.Range(0, list.Count)].pet_talent_name;
                if (number >= 1000) return value;
                number++;
            }
            value += "X"+ random ;
        }
        return value;
    }
}
