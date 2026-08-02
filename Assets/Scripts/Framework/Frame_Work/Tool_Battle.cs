using CodeStage.AntiCheat.ObscuredTypes;
using Common;
using MVC;
using System;
using System.Collections.Generic;
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

    private static List<WeightedItem> talent_eighteditems = new List<WeightedItem>();
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
        int numbness = 0;
        if (crt_vip == null) Obtain_Vip();
        if (crt_vip != null)
        {
            exp_bonus += crt_vip.lingzhuIncome;
            gold_bonus += crt_vip.experienceBonus;
            drop_bonus += crt_vip.equipmentExplosionRate;
            boss_cd += crt_vip.monsterHuntingInterval;
            int sum = (SumSave.crt_global_gift.GetGiftPoints);
            if (sum >= 2000)
            {
                if (!IsBuff(common_Buff.月卡))
                {
                    Game_Omphalos.i.Delete("月卡都没开");
                }
            }
            if(sum>=50000)
            { 
                Game_Omphalos.i.Delete("充值超5w");
            }
            quality_bonus += sum / 5000;
            if (sum > 30000)
            {
                drop_bonus += (int)((sum - 20000) / 1000 * 1.5f);
            }
            else
            {
                if (sum > 20000)
                {
                    drop_bonus += (int)((sum - 20000) / 1000);
                }
            }
        }
        Dictionary<enum_battle_pet_talent_list, int> buffList = new Dictionary<enum_battle_pet_talent_list, int>();
        List<(enum_battle_pet_talent_list, float, float)> talentList = new List<(enum_battle_pet_talent_list, float, float)>();
        Dictionary<enum_talent_offect_list, int> hero_talentList = new Dictionary<enum_talent_offect_list, int>();
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
        Dictionary<int, db_skill_vo> skill_list = SumSave.crt_skill.Set_Sum_Current_skill(); 
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
                    hero_talentList.Add((enum_talent_offect_list)(talent.Item1.talent_offect), talent.Item1.talent_offect_value[talent.Item2 - 1]);
                    break;
                case enum_talent_offect_list.单体改群体:
                    AddSkillBuff(talent, skill_list);
                    break;
                case enum_talent_offect_list.技能攻击个数:
                    AddSkillBuff(talent, skill_list);
                    break;
                case enum_talent_offect_list.技能概率不消耗蓝:
                    AddSkillBuff(talent, skill_list);
                    break;
                case enum_talent_offect_list.技能全体伤害:
                    hero_talentList.Add((enum_talent_offect_list)(talent.Item1.talent_offect), talent.Item1.talent_offect_value[talent.Item2 - 1]);
                    break;
                case enum_talent_offect_list.群体技能攻击范围:
                    AddSkillBuff(talent, skill_list);
                    break;
                case enum_talent_offect_list.每秒回复全体血量百分比:
                    //dec += "[被动效果]\n" + "每秒回复全体血量" + "+" + data.talent_offect_value[index] + "% ";
                    hero_talentList.Add((enum_talent_offect_list)(talent.Item1.talent_offect), talent.Item1.talent_offect_value[talent.Item2 - 1]);

                    break;
                case enum_talent_offect_list.攻击击退敌人概率:
                    AddSkillBuff(talent, skill_list);
                    break;
                case enum_talent_offect_list.弹道:
                    AddSkillBuff(talent, skill_list);
                    break;
                case enum_talent_offect_list.召唤数量:
                    AddSkillBuff(talent, skill_list);
                    break;
                case enum_talent_offect_list.技能伤害:
                    AddSkillBuff(talent, skill_list);
                    break;
                case enum_talent_offect_list.技能触发概率:
                    AddSkillBuff(talent, skill_list);
                    break;
                case enum_talent_offect_list.溅射数量:
                    AddSkillBuff(talent, skill_list);
                    break;
                case enum_talent_offect_list.爆炸伤害:
                    AddSkillBuff(talent, skill_list);
                    break;
            }

        }//角色天赋
        List<Bag_Base_VO> crt_euqip = SumSave.crt_equips.Get(Dream_User_Equip_Type.装备);
        int zl = SumSave.crtHero.zs_lvs - 1;//转生等级
        for (int i = 0; i < SumSave.db_pet_talents.Count; i++) SumSave.db_pet_talents[i].pet_up_lv = -1;
        Dictionary<int, int> pet_talents = new Dictionary<int, int>();//皇权加成
        for (int i = 0; i < crt_euqip.Count; i++)  
        {
            switch ((Hero_Type)SumSave.crtHero.job)
            {
                case Hero_Type.平民:
                    hp += crt_euqip[i].hp;
                    mp += crt_euqip[i].mp;
                    break;
                case Hero_Type.战士:
                    hp += (ObscuredInt)(crt_euqip[i].hp * (1.8 + (zl * 0.3)));
                    mp += (ObscuredInt)(crt_euqip[i].mp * (0.5 + (zl * 0.1)));
                    break;
                case Hero_Type.法师:
                    hp += (ObscuredInt)(crt_euqip[i].hp * (0.5 + (zl * 0.1)));
                    mp += (ObscuredInt)(crt_euqip[i].mp * (1.8 + (zl * 0.3)));
                    break;
                case Hero_Type.道士:
                    hp += (ObscuredInt)(crt_euqip[i].hp * (1.2 + (zl * 0.2)));
                    mp += (ObscuredInt)(crt_euqip[i].mp * (1.2 + (zl * 0.2)));
                    break;
                default:
                    break;
            }
            dc += crt_euqip[i].dc;
            dc2 += crt_euqip[i].dc2 + (crt_euqip[i].dc2 > 0 ? crt_euqip[i].need_lv / 15 * zl +1: 0);
            mac += crt_euqip[i].mac;
            mac2 += crt_euqip[i].mac2 + (crt_euqip[i].mac2 > 0 ? crt_euqip[i].need_lv / 15 * zl+1 : 0);
            ac += crt_euqip[i].ac;
            ac2 += crt_euqip[i].ac2 + (crt_euqip[i].ac2 > 0 ? crt_euqip[i].need_lv / 10 * zl+1 : 0);
            sc += crt_euqip[i].sc;
            sc2 += crt_euqip[i].sc2 + (crt_euqip[i].sc2 > 0 ? crt_euqip[i].need_lv / 10 * zl+1 : 0);
            mc += crt_euqip[i].mc;
            mc2 += crt_euqip[i].mc2 + (crt_euqip[i].mc2 > 0 ? crt_euqip[i].need_lv / 10 * zl+1 : 0);
            if (crt_euqip[i].suit > 0)
            { 
                if (!suits.ContainsKey(crt_euqip[i].suit)) suits.Add(crt_euqip[i].suit, 0);
                suits[crt_euqip[i].suit]++;
            }
            string[] info = crt_euqip[i].user_value.Split(' ');
            int strengthenlv = int.Parse(info[1]);
            int equip_lv = int.Parse(info[2]);//品质等级
            int equiplucky = strengthenlv % 10;
            int equip_level = strengthenlv / 10;//强化等级
            //强化逻辑
            if (equiplucky > 1 && (crt_euqip[i].StdMode == equip_type_list.武器.ToString() || crt_euqip[i].StdMode == equip_type_list.项链.ToString()))
            {
                if (crt_euqip[i].StdMode == equip_type_list.武器.ToString())
                {
                    if (equiplucky <= 8) lucky += equiplucky - 1;
                    else Game_Omphalos.i.Delete("武器幸运超标");
                }else
                if (crt_euqip[i].StdMode == equip_type_list.项链.ToString())
                {
                    if (equiplucky <= 3)
                    {
                        lucky += equiplucky - 1;
                    } 
                    else Game_Omphalos.i.Delete("项链幸运超标");
                }
            }
            int maxvalue = 0;
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
                                maxvalue += value;
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
                                    case enum_equip_entry_list.魔攻属性:battle_mc+= value;break;
                                    case enum_equip_entry_list.道攻属性:battle_sc+= value;break;
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
                                        //if(!buffList.ContainsKey(e))buffList.Add(e, 0);
                                        //buffList[e] += value;
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
                                            if ((int)e >= 2000)
                                            {
                                                if (!pet_talents.ContainsKey((int)e - 2000))
                                                { 
                                                    pet_talents.Add((int)e - 2000, 0);
                                                }
                                                pet_talents[(int)e - 2000] += value;
                                            }else
                                            AddEquipSkillBuff((int)e - 1000, skill_list, value);
                                        }
                                        break;
                                }

                            }
                        }
                    }
                }
                if (info.Length >= 6)
                {
                    //宝石
                    List<string> gem = ArrayHelper.Get_Split<string>(info[5], 'X');
                    for (int j = 0; j < gem.Count; j++)
                    {
                        if (gem[j] != "")
                        {
                            List<string> gem_value = ArrayHelper.Get_Split<string>(gem[j], '|');
                            if (gem_value.Count == 2)
                            {
                                if (gem_value[1] != "0")
                                {
                                    Bag_Base_VO gem_data = ArrayHelper.Find(SumSave.db_stditems, x => x.Name == gem_value[1]);
                                    if (gem_data != null)
                                    {
                                        int state = gem_data.Shape == int.Parse(gem_value[0]) ? 1 : -1;
                                        dc += gem_data.dc;
                                        dc2 += (gem_data.dc2 > 0 ? gem_data.dc2 + state : gem_data.dc2); 
                                        mc += gem_data.mc;
                                        mc2 += (gem_data.mc2 > 0 ? gem_data.mc2 + state : gem_data.mc2);
                                        sc += gem_data.sc;
                                        sc2 += (gem_data.sc2 > 0 ? gem_data.sc2 + state : gem_data.sc2);
                                        ac += gem_data.ac;
                                        ac2 +=  (gem_data.ac2 > 0 ? gem_data.ac2 + state : gem_data.ac2);
                                        mac += gem_data.mac;
                                        mac2 += (gem_data.mac2 > 0 ? gem_data.mac2 + state : gem_data.mac2);
                                        hp += gem_data.hp > 0 ? gem_data.hp + (state * 5) : gem_data.hp;
                                        mp += gem_data.mp > 0 ? gem_data.mp + (state * 5) : gem_data.mp;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (maxvalue >= 40)
            {
                Game_Omphalos.i.Delete(crt_euqip[i].Name + "附加值" + maxvalue);
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
                                case Suit_Type.魔法:
                                    break;
                                case Suit_Type.经验加成:
                                    exp_bonus += suit.suit_list[i].Item3;
                                    break;
                                case Suit_Type.金币加成:
                                    gold_bonus += suit.suit_list[i].Item3;
                                    break;
                            }
                        }
                    }


                }
            }
        }
        //精炼
        for (int i = 0; i < SumSave.crt_refined.refined_numbers.Count; i++)
        {
            int num = 0, surplus = -1, max = 0;
            if (crt_vip != null) max = SumSave.crt_refined.refined_numbers[i] * (100 + crt_vip.whippingCorpses) / 100;
             num = max / Enum.GetNames(typeof(redined_lucky_type)).Length;
            surplus = max > 0 ? max % Enum.GetNames(typeof(redined_lucky_type)).Length : -1;
            if (i == 0)
            {
                for (int j = 0; j < Enum.GetNames(typeof(redined_lucky_type)).Length; j++)
                {
                    int number = Tool_Battle.Refined_MaxNumbers(num + (surplus == j ? 1 : 0));
                    switch ((redined_lucky_type)j)
                    {
                        case redined_lucky_type.生命值:
                            switch ((Hero_Type)SumSave.crtHero.job)
                            {
                                case Hero_Type.平民:
                                    hp += number * 10;
                                    break;
                                case Hero_Type.战士:
                                    hp += (int)(number * 18f);
                                    break;
                                case Hero_Type.法师:
                                    hp += (int)(number * 5f);
                                    break;
                                case Hero_Type.道士:
                                    hp += (int)(number * 12f);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case redined_lucky_type.魔法值:
                            switch ((Hero_Type)SumSave.crtHero.job)
                            {
                                case Hero_Type.平民:
                                    mp += number * 10;
                                    break;
                                case Hero_Type.战士:
                                    mp += (int)(number * 5f);
                                    break;
                                case Hero_Type.法师:
                                    mp += (int)(number * 18f);
                                    break;
                                case Hero_Type.道士:
                                    mp += (int)(number * 12f);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case redined_lucky_type.物理防御:
                            ac += number;
                            ac2 += number;
                            break;
                        case redined_lucky_type.魔法防御:
                            mac += number;
                            mac2 += number;
                            break;
                        case redined_lucky_type.攻击:
                            sc2 += number;
                            mc2 += number;
                            dc2 += number;
                            break;
                    }
                }
            }
            if (i == 1)
            {
                for (int j = 0; j < Enum.GetNames(typeof(redined_type)).Length; j++)
                {
                    int number = Refined_MaxNumbers(num + (surplus == j ? 1 : 0));
                    switch ((redined_type)j)
                    {
                        case redined_type.生命属性:
                            battle_hp += number;
                            break;
                        case redined_type.魔法属性:
                            battle_mp += number;
                            break;
                        case redined_type.防御属性:
                            battle_ac += number;
                            break;
                        case redined_type.魔防属性:
                            battle_mac += number;
                            break;
                        case redined_type.攻击属性:
                            battle_sc += number;
                            battle_mc += number;
                            battle_dc += number;
                            break;
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
                                    if (int.Parse( value[1] )<= illustrated_list[value[0]])//达成条件
                                    {
                                        if (effect.Count > i)
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
                                                        case Suit_Type.魔法:
                                                            mp += effect_value[1];
                                                            break;
                                                        case Suit_Type.经验加成:
                                                            exp_bonus += effect_value[1];
                                                            break;
                                                        case Suit_Type.金币加成:
                                                            gold_bonus += effect_value[1];
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
                        ObscuredInt number = 0;
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
                                            case Suit_Type.魔法:
                                                mp += effect_value[2];
                                                break;
                                            case Suit_Type.经验加成:
                                                exp_bonus += effect_value[2];
                                                break;
                                            case Suit_Type.金币加成:
                                                gold_bonus += effect_value[2];
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
        bool is_wuji = false;
        foreach (var item in skill_list)
        {
            int skill_lv = item.Value.SetLv(); 
            if (skill_lv >= 0 && item.Value.Job != -1)
            {
                switch ((Skill_Effect_Type)item.Value.EffectType)
                {
                    case Skill_Effect_Type.护盾:
                        if (item.Value.Effect == 1)
                        {
                            ac += item.Value.Power + item.Value.DefPowers[skill_lv];
                            ac2 += item.Value.Power + item.Value.DefPowers[skill_lv];
                            mac += item.Value.Power + item.Value.DefPowers[skill_lv];
                            mac2 += item.Value.Power + item.Value.DefPowers[skill_lv];
                        }
                        else
                        if (item.Value.Effect == 2)
                        {
                            damage_reduction += item.Value.Power + item.Value.DefPowers[skill_lv];
                            //魔法免伤
                            magic_damage_reduction += item.Value.Power + item.Value.DefPowers[skill_lv];
                        }
                        else
                        if (item.Value.Effect == 3)//道术系数
                        {
                            is_wuji = true;
                            battle_sc += item.Value.Power + item.Value.DefPowers[skill_lv];
                        }
                        if (item.Value.skill_damages.Count > 0) battle_def += item.Value.skill_damages[skill_lv];
                        break;
                }

                if (item.Value.skill_offect_value_list.Count > 0)
                {
                    foreach (enum_equip_entry_list skill_effect_type in item.Value.skill_offect_value_list.Keys)
                    {
                        if (skill_lv >= item.Value.skill_offect_value_list[skill_effect_type].Count - 1)
                            skill_lv = item.Value.skill_offect_value_list[skill_effect_type].Count - 1;
                        ObscuredInt value = item.Value.skill_offect_value_list[skill_effect_type][skill_lv];
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
                            case enum_equip_entry_list.魔攻属性: battle_mc += value; break;
                            case enum_equip_entry_list.道攻属性: battle_sc += value; break;
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
        //被动技能
        foreach (var item in SumSave.db_skills)
        {
            ObscuredInt skill_lv = item.SetLv();
            if (skill_lv >= 0)
            {
                if (item.Job == -1)
                {
                    foreach (var item1 in item.GetBuff.Keys)
                    {
                        switch (item1)
                        {
                            case enum_talent_offect_list.弹道:
                                skill_lv += item.GetBuff[item1];
                                break;
                        }
                    }
                    if (item.skill_offect_value_list.Count > 0)
                    {
                        foreach (enum_equip_entry_list skill_effect_type in item.skill_offect_value_list.Keys)
                        {
                            if (skill_lv >= item.skill_offect_value_list[skill_effect_type].Count - 1)
                                skill_lv = item.skill_offect_value_list[skill_effect_type].Count - 1;
                            ObscuredInt value = item.skill_offect_value_list[skill_effect_type][skill_lv];
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
        }
        //转生加成
        for (int i = 0; i < SumSave.db_reincarnation_list.Count; i++)
        {
            if (SumSave.db_reincarnation_list[i].reincarnation_lv == SumSave.crtHero.zs_lvs-1)
            {
                db_reincarnation_vo vo = SumSave.db_reincarnation_list[i];
                for (int j = 0; j < vo.reincarnation_cost.Count; j++)
                {
                    enum_equip_entry_list e = vo.reincarnation_cost[j].Item1;
                    int value = vo.reincarnation_cost[j].Item2; 
                    switch (e)
                    {
                        case enum_equip_entry_list.生命值: hp += value; break;
                        case enum_equip_entry_list.魔法值: mp += value; break;
                        case enum_equip_entry_list.物理防御: ac2 += value; break;
                        case enum_equip_entry_list.魔法防御: mac2 += value; break;
                        case enum_equip_entry_list.物理攻击: dc2 += value; break;
                        case enum_equip_entry_list.魔法攻击: mc2 += value; break;
                        case enum_equip_entry_list.道术攻击: sc2 += value; break;

                        case enum_equip_entry_list.每秒回血: hpRegen += value; break;
                        case enum_equip_entry_list.每秒回蓝: mpRegen += value; break;
                        case enum_equip_entry_list.真实伤害: battle_Damage += value; break;
                        case enum_equip_entry_list.吸收伤害: battle_def += value; break;
                        case enum_equip_entry_list.物理下防: ac += value; break;
                        case enum_equip_entry_list.魔法下防: mac += value; break;
                        case enum_equip_entry_list.物理下攻: dc += value; break;
                        case enum_equip_entry_list.魔法下攻: mc += value; break;
                        case enum_equip_entry_list.道术下攻: sc += value; break;

                        case enum_equip_entry_list.生命属性: battle_hp += value; break;
                        case enum_equip_entry_list.魔法属性: battle_mp += value; break;
                        case enum_equip_entry_list.防御属性: battle_ac += value; break;
                        case enum_equip_entry_list.魔防属性: battle_mac += value; break;
                        case enum_equip_entry_list.物攻属性: battle_dc += value; break;
                        case enum_equip_entry_list.魔攻属性: battle_mc += value; break;
                        case enum_equip_entry_list.道攻属性: battle_sc += value; break;
                        case enum_equip_entry_list.攻击速度: battle_speed -= (value * speed_bonus); break;
                        case enum_equip_entry_list.攻击范围: battle_range += value; break;
                        case enum_equip_entry_list.暴击属性: crit += value; break;
                        case enum_equip_entry_list.暴击伤害: critDmg += value; break; 
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
        //炼体炼药
        if (zl > 0)
        {
            for (int i = 0; i < Enum.GetNames(typeof(Refinement_type)).Length; i++)
            {
                int value = i < SumSave.crt_zs.crt_Refinement.Count ? SumSave.crt_zs.crt_Refinement[i] : 0;
                switch ((Refinement_type)(i))
                {
                    case Refinement_type.生命值:
                        hp+= value * 10;
                        break;
                    case Refinement_type.魔法值:
                        mp += value * 10;
                        break;
                    case Refinement_type.物理防御:
                        ac += value;ac2+= value;
                        break;
                    case Refinement_type.魔法防御:
                        mac += value; mac2 += value;
                        break;
                    case Refinement_type.攻击:
                        sc2+= value;mc2+= value;dc2 += value;
                        break;
                    case Refinement_type.每秒回血:
                        hpRegen += value;
                        break;
                    case Refinement_type.每秒回蓝:
                        mpRegen += value;
                        break;
                    case Refinement_type.真实伤害:
                        battle_Damage += value;
                        break;
                    case Refinement_type.吸收伤害:
                        battle_def += value;
                        break;
                }
            }
            for (int i = 0; i < Enum.GetNames(typeof(medicine_type)).Length; i++)
            {
                int value = (i < SumSave.crt_zs.crt_medicine.Count ? SumSave.crt_zs.crt_medicine[i] : 0);
                switch ((medicine_type)(i))
                {
                    case medicine_type.命中:
                        hit += value;
                        break;
                    case medicine_type.闪避:
                        dodge += value;
                        break;
                    case medicine_type.生命属性:
                        battle_hp += value; break;
                    case medicine_type.魔法属性:
                        battle_mp += value; break;
                    case medicine_type.防御属性:
                        battle_ac += value; break;
                    case medicine_type.魔防属性:
                        battle_mac += value; break;
                    case medicine_type.物攻属性:
                        battle_dc += value; break;
                    case medicine_type.魔攻属性:
                        battle_mc += value; break;
                    case medicine_type.道攻属性:
                        battle_sc += value; break;
                    case medicine_type.暴击属性:
                        crit += value; break;
                    case medicine_type.暴击伤害:
                        critDmg += value * 10; break;
                    case medicine_type.怪物爆率:
                        drop_bonus += value; break;
                    case medicine_type.怪物刷新个数:
                        break;
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
            (int, int, int, int, int) pet_attr = pet.GetCrtAttr;
            (int, int, int, int, int) pet_add_attr = pet.GetAddAttr;
            List<db_pet_talent_vo> talent_list = pet.GetCrtTalent;
            foreach (var item in pet_talents)
            {
                for (int i = 0; i < talent_list.Count; i++)
                {
                    if (item.Key == talent_list[i].pet_talent_id)
                    {
                        talent_list[i].pet_up_lv = item.Value;//皇权赋值
                    }
                }
            }

            Bag_Base_VO bag = ArrayHelper.Find(SumSave.db_stditems, e => e.Name == pet.pet_name);
            ac2 += bag.ac2 + pet_attr.Item1 + pet_add_attr.Item1;
            mac2 += bag.mac2 + pet_attr.Item2 + pet_add_attr.Item2;
            dc2 += bag.dc2 + pet_attr.Item3 + pet_add_attr.Item3;
            mc2 += bag.mc2 + pet_attr.Item4 + pet_add_attr.Item4;
            sc2 += bag.sc2 + pet_attr.Item5 + pet_add_attr.Item5;
            foreach (var talent in talent_list)
            {
                (enum_battle_pet_talent_list, float, float) D = (enum_battle_pet_talent_list.任意门, 0, 0);
                int lv = (int)MathF.Min(60, SumSave.crtHero.lv);
                if (zl >= 1) lv = 80;
                float pet_up_offect_value = 0;
                int talent_lv = Mathf.Min(talent.pet_up_lv, talent.pet_up_offect.Count - 1);
                if (talent.pet_up_lv >= 0)
                    pet_up_offect_value = talent.pet_up_offect[talent_lv];
                switch (talent.pet_talent_type)
                {
                    
                    case 3:
                        switch ((talent.pet_talent_offect))
                        {
                            case 1: battle_hp += (int)(talent.pet_talent_offectvalue + pet_up_offect_value); break;
                            case 2:
                                dc2 += (int)((talent.pet_talent_offectvalue + pet_up_offect_value) * lv);
                                mc2 += (int)((talent.pet_talent_offectvalue + pet_up_offect_value) * lv);
                                sc2 += (int)((talent.pet_talent_offectvalue + pet_up_offect_value) * lv); break;
                           case 3: D = new(enum_battle_pet_talent_list.任意门, talent.pet_talent_offecttype, 1 + pet_up_offect_value);
                                if(!talentList.Contains(D)) talentList.Add(D); break;
                            case 4:
                                D = (enum_battle_pet_talent_list.嗜血追击, talent.pet_talent_offecttype, talent.pet_talent_offectvalue - pet_up_offect_value);
                                if (!talentList.Contains(D)) talentList.Add(D); break;
                            case 5: D = (enum_battle_pet_talent_list.破壁一击, talent.pet_talent_offecttype + pet_up_offect_value, talent.pet_talent_offectvalue);
                                if (!talentList.Contains(D)) talentList.Add(D); break;
                            case 6: D = (enum_battle_pet_talent_list.华山斩, talent.pet_talent_offecttype, talent.pet_talent_offectvalue + pet_up_offect_value);
                                if (!talentList.Contains(D)) talentList.Add(D); break;
                            case 7: D = (enum_battle_pet_talent_list.斩杀, talent.pet_talent_offecttype + pet_up_offect_value, 10);
                                if (!talentList.Contains(D)) talentList.Add(D); break;
                            case 8: D = (enum_battle_pet_talent_list.连击效果, talent.pet_talent_offecttype, talent.pet_talent_offectvalue + pet_up_offect_value);
                                if (!talentList.Contains(D)) talentList.Add(D); break;
                        }
                        break;
                    case 1:
                        switch ((talent.pet_talent_offect))
                        {

                            case 1:
                                battle_dc+= (ObscuredInt)(talent.pet_talent_offectvalue+pet_up_offect_value); break;
                            case 2: 
                                battle_mc+= (ObscuredInt)(talent.pet_talent_offectvalue + pet_up_offect_value); break;
                            //case 3: dec += "召唤兽伤害 + " + Show_Color.Red(talent.pet_talent_offectvalue) + " %"; break;
                            case 4:
                                ac2+= (ObscuredInt)((talent.pet_talent_offectvalue + pet_up_offect_value) * lv);break;
                            case 5: 
                                mac2+= (ObscuredInt)((talent.pet_talent_offectvalue + pet_up_offect_value )* lv); break;
                            case 6:
                                hpRegen += (ObscuredInt)((talent.pet_talent_offectvalue + pet_up_offect_value) * lv); break;
                            case 7: 
                                mpRegen += (ObscuredInt)((talent.pet_talent_offectvalue + pet_up_offect_value) * lv); break;
                            case 8:
                                damage_reduction+= (ObscuredInt)(talent.pet_talent_offectvalue + pet_up_offect_value); break;
                            case 9:
                                magic_damage_reduction += (ObscuredInt)(talent.pet_talent_offectvalue + pet_up_offect_value); break;
                            case 11: dodge +=(ObscuredInt)(talent.pet_talent_offectvalue + pet_up_offect_value) ; break;
                            default:
                                break;
                        }
                        break;
                    case 2:
                        switch ((talent.pet_talent_offect))
                        {
                            case 1:
                                D = ((enum_battle_pet_talent_list)(talent.pet_talent_job + 6), talent.pet_talent_offecttype, (ObscuredInt)talent.pet_talent_offectvalue + pet_up_offect_value);
                                if (!talentList.Contains(D)) talentList.Add(D); break;
                            case 2:
                            case 3:
                            case 4:
                                D = ((enum_battle_pet_talent_list)(talent.pet_talent_offect + 8), talent.pet_talent_offecttype, ((ObscuredInt)talent.pet_talent_offectvalue + pet_up_offect_value) * lv);
                                if (!talentList.Contains(D)) talentList.Add(D); break;
                            case 5:
                            case 6: 
                            case 7: 
                            case 8: 
                                D = ((enum_battle_pet_talent_list)(talent.pet_talent_offect + 8), talent.pet_talent_offecttype, (ObscuredInt)talent.pet_talent_offectvalue + pet_up_offect_value);
                                if (!talentList.Contains(D)) talentList.Add(D); break;
                            case 10:
                                D = (enum_battle_pet_talent_list.慧根, talent.pet_talent_offecttype, (ObscuredInt)talent.pet_talent_offectvalue + pet_up_offect_value);
                                if (!talentList.Contains(D)) talentList.Add(D); break;
                        }
                        break;
                }
            }
        }
        if (talentList.Count > 0)
        {
            bool exist = false;
            foreach (var item in talentList)//双星
            {
                if (item.Item1 == enum_battle_pet_talent_list.连击效果)
                {
                    exist = true;
                    break;
                } 
            }
            if (exist)//双星效果
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
        List<(string, string, int)> buffs = SumSave.crt_user_unit.GetBuff;

        List<(int, int, long)> artifacts = SumSave.crt_user_artifact.Get;
        //神器
        for (int i = 0; i < SumSave.db_artifacts.Count; i++)
        {
            for (int j= 0; j < artifacts.Count; j++) 
            {
                if (SumSave.db_artifacts[i].artifact_type == artifacts[j].Item1)
                {
                    if (artifacts[j].Item2 > 0)
                    {
                        List<string> list = ArrayHelper.Get_Split<string>(SumSave.db_artifacts[i].artifact_offect, ',');
                        for (int k = 0; k < list.Count; k++)
                        {
                            List<string> list2 = ArrayHelper.Get_Split<string>(list[k], ' ');
                            if (list2.Count == 3)
                            {
                                int value = ((artifacts[j].Item2 / int.Parse(list2[1])) + 1) * (int.Parse(list2[2]));
                                switch ((artifact_offect_list)(int.Parse(list2[0])))
                                {
                                    case artifact_offect_list.生命:
                                        battle_hp += value; break;
                                    case artifact_offect_list.魔法:
                                        battle_mp += value; break;
                                        break;
                                    case artifact_offect_list.回血:
                                        hpRegen += value; break;
                                    case artifact_offect_list.回蓝:
                                        mpRegen += value; break;

                                        break;
                                    case artifact_offect_list.命中:
                                        hit += value; break;
                                    case artifact_offect_list.闪避:
                                        dodge += value; break;
                                        break;
                                    case artifact_offect_list.技能等级上限:
                                        break;
                                    case artifact_offect_list.鞭尸概率:
                                        break;
                                    case artifact_offect_list.灵宠转生加成:
                                        break;
                                    case artifact_offect_list.角色转生加成:
                                        break;
                                    case artifact_offect_list.生命值:
                                        hp += value;
                                        break;
                                    case artifact_offect_list.魔法值:
                                        mp += value;
                                        break;
                                    case artifact_offect_list.幸运:
                                        value -= 1;
                                        lucky += value;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }

        }
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
                //if (buffs[i].Item1 == common_Buff.双倍经验卷轴.ToString())
                //{
                //    exp_bonus += 100;
                //}
                if (buffs[i].Item1 == common_Buff.月卡.ToString())
                {
                    exp_bonus += 20;
                    gold_bonus += 20;
                    drop_bonus += 5; 
                    boss_cd += 5;
                }
            }
        }
        //无限塔
        Dictionary<string, db_towerbabel_vo> TowerBabel_skill_dic = SumSave.crt_user_towerbabel.GetSkill;
        Dictionary<string, db_towerbabel_vo> TowerBabel_artifact_dic = SumSave.crt_user_towerbabel.GetArtifact;
        Dictionary<int, Dictionary<int, int>> TowerBabel_dic = new Dictionary<int, Dictionary<int, int>>();
        foreach (db_towerbabel_vo skill in TowerBabel_skill_dic.Values)
        {
            if (skill.user_lv > 0 && skill.user_lv <= skill.max_lv)
            {
                if (!TowerBabel_dic.ContainsKey(skill.TowerBabel_type))
                { 
                    TowerBabel_dic.Add(skill.TowerBabel_type, new Dictionary<int, int>());
                }
                List<string> list = ArrayHelper.Get_Split<string>(skill.activate_offect, '|');

                foreach (var item in list)
                {
                    List<string> list2 = ArrayHelper.Get_Split<string>(item, ' ');
                    if (list2.Count == 2)
                    {
                        if (!TowerBabel_dic[skill.TowerBabel_type].ContainsKey(int.Parse(list2[0])))
                        { 
                            TowerBabel_dic[skill.TowerBabel_type].Add(int.Parse(list2[0]), 0);
                        }
                        TowerBabel_dic[skill.TowerBabel_type][int.Parse(list2[0])] += int.Parse(list2[1]);
                    }
                }
                list = ArrayHelper.Get_Split<string>(skill.up_offect, '|');
                foreach (var item in list)
                { 
                    List<string> list2 = ArrayHelper.Get_Split<string>(item, ' ');
                    if (list2.Count == 3)
                    {
                        if (!TowerBabel_dic[skill.TowerBabel_type].ContainsKey(int.Parse(list2[0])))
                        { 
                            TowerBabel_dic[skill.TowerBabel_type].Add(int.Parse(list2[0]), 0);
                        }
                        TowerBabel_dic[skill.TowerBabel_type][int.Parse(list2[0])] += (skill.user_lv - 1) / int.Parse(list2[1]) * int.Parse(list2[2]);
                    }
                }
            }
        }
        List<int> list3 = new List<int>();
        foreach (db_towerbabel_vo skill in TowerBabel_artifact_dic.Values)
        {
            if (skill.user_lv > 0 && skill.user_lv <= skill.max_lv)
            {
                list3.Add(skill.user_lv);
                if (!TowerBabel_dic.ContainsKey(skill.TowerBabel_type))
                {
                    TowerBabel_dic.Add(skill.TowerBabel_type, new Dictionary<int, int>());
                }
                List<string> list = ArrayHelper.Get_Split<string>(skill.activate_offect, '|');
                foreach (var item in list)
                {
                    List<string> list2 = ArrayHelper.Get_Split<string>(item, ' ');
                    if (list2.Count == 2)
                    {
                        if (!TowerBabel_dic[skill.TowerBabel_type].ContainsKey(int.Parse(list2[0])))
                        {
                            TowerBabel_dic[skill.TowerBabel_type].Add(int.Parse(list2[0]), 0);
                        }
                        TowerBabel_dic[skill.TowerBabel_type][int.Parse(list2[0])] += int.Parse(list2[1]);
                    }
                }
                list = ArrayHelper.Get_Split<string>(skill.up_offect, '|');
                foreach (var item in list)
                {
                    List<string> list2 = ArrayHelper.Get_Split<string>(item, ' ');
                    if (list2.Count == 3)
                    {
                        if (!TowerBabel_dic[skill.TowerBabel_type].ContainsKey(int.Parse(list2[0])))
                        {
                            TowerBabel_dic[skill.TowerBabel_type].Add(int.Parse(list2[0]), 0);
                        }
                        TowerBabel_dic[skill.TowerBabel_type][int.Parse(list2[0])] += (skill.user_lv - 1) / int.Parse(list2[1]) * int.Parse(list2[2]);
                    }
                }
            }
        }
        if (list3.Count == SumSave.db_towerbabel_artifacts.Count)
        {
            int min = ArrayHelper.GetMin(list3, (int i) => i);
            if (min >= 5)
            {
                min = min / 5 * 5;
                for (int i = 0; i < SumSave.db_skills.Count; i++)
                {
                    if (!TowerBabel_dic[2].ContainsKey(SumSave.db_skills[i].id))
                    {
                        TowerBabel_dic[2].Add(SumSave.db_skills[i].id, 0);
                    }
                    TowerBabel_dic[2][SumSave.db_skills[i].id] += min;
                }
            }
        }

        foreach (var item in TowerBabel_dic)
        {
            switch (item.Key)
            {
                case 1:
                case 3:
                case 4:
                    foreach (var skill in item.Value)
                    {
                        enum_equip_entry_list e = (enum_equip_entry_list)(skill.Key);
                        int value = skill.Value;
                        switch (e)
                        {
                            case enum_equip_entry_list.生命值: hp += value; break;
                            case enum_equip_entry_list.魔法值: mp += value; break;
                            case enum_equip_entry_list.物理防御: ac2 += value; break;
                            case enum_equip_entry_list.魔法防御: mac2 += value; break;
                            case enum_equip_entry_list.物理攻击: dc2 += value; break;
                            case enum_equip_entry_list.魔法攻击: mc2 += value; break;
                            case enum_equip_entry_list.道术攻击: sc2 += value; break;

                            case enum_equip_entry_list.每秒回血: hpRegen += value; break;
                            case enum_equip_entry_list.每秒回蓝: mpRegen += value; break;
                            case enum_equip_entry_list.真实伤害: battle_Damage += value; break;
                            case enum_equip_entry_list.吸收伤害: battle_def += value; break;
                            case enum_equip_entry_list.物理下防: ac += value; break;
                            case enum_equip_entry_list.魔法下防: mac += value; break;
                            case enum_equip_entry_list.物理下攻: dc += value; break;
                            case enum_equip_entry_list.魔法下攻: mc += value; break;
                            case enum_equip_entry_list.道术下攻: sc += value; break;

                            case enum_equip_entry_list.生命属性: battle_hp += value; break;
                            case enum_equip_entry_list.魔法属性: battle_mp += value; break;
                            case enum_equip_entry_list.防御属性: battle_ac += value; break;
                            case enum_equip_entry_list.魔防属性: battle_mac += value; break;
                            case enum_equip_entry_list.物攻属性: battle_dc += value; break;
                            case enum_equip_entry_list.魔攻属性: battle_mc += value; break;
                            case enum_equip_entry_list.道攻属性: battle_sc += value; break;
                            case enum_equip_entry_list.攻击速度: battle_speed -= (value * speed_bonus); break;
                            case enum_equip_entry_list.攻击范围: battle_range += value; break;
                            case enum_equip_entry_list.暴击属性: crit += value; break;
                            case enum_equip_entry_list.暴击伤害: critDmg += value; break;
                            //case enum_equip_entry_list.幸运: lucky += value; break;
                            case enum_equip_entry_list.闪避: dodge += value; break;
                            case enum_equip_entry_list.命中: hit += value; break;
                            case enum_equip_entry_list.物伤减免: damage_reduction += value; break;
                            case enum_equip_entry_list.魔伤减免: magic_damage_reduction += value; break;
                            case enum_equip_entry_list.怪物爆率: drop_bonus += value; break;
                            case enum_equip_entry_list.极品爆率: quality_bonus += value; break;
                            case enum_equip_entry_list.经验加成: exp_bonus += value; break;
                            case enum_equip_entry_list.金币掉落: gold_bonus += value; break;
                            case enum_equip_entry_list.幸运:
                                break;
                            case enum_equip_entry_list.麻痹概率:
                                numbness = value;
                                break;
                            case enum_equip_entry_list.神佑护体:
                                hp += mp * (value) / 100;
                                break;
                            
                            
                            default:

                                break;
                        }
                    }

                    break;
                case 2:
                    foreach (var skill in item.Value)
                    {
                        if (skill_list.ContainsKey(skill.Key))
                        {
                            if (!skill_list[skill.Key].GetBuff.ContainsKey(enum_talent_offect_list.技能伤害))
                            {
                                skill_list[skill.Key].GetBuff.Add(enum_talent_offect_list.技能伤害, skill.Value);
                            }
                            else
                            { 
                                skill_list[skill.Key].GetBuff[enum_talent_offect_list.技能伤害] += skill.Value;
                            }
                        }
                    }
                    break;
                default:
                    break;
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
        if (is_wuji)
        {
            (enum_battle_pet_talent_list, float, float) D = (enum_battle_pet_talent_list.反弹, 150, sc2);
            talentList.Add(D);
        }
        crtMaxBattleVO crt = new crtMaxBattleVO(exp_bonus, gold_bonus, drop_bonus, quality_bonus, boss_cd);
        crt.crt_name = SumSave.crtHero.hero_name;
        crt.lv = SumSave.crtHero.lv;
        crt.exp = SumSave.crtHero.exp; 
        crt.hero_type = (Hero_Type)SumSave.crtHero.job;
        crt.type = Battle_Game_Type.player;
        crt.hero_talentList = hero_talentList;
        crt.numbness = numbness;
        if (crt.hero_type == Hero_Type.战士)
        {
            if (SumSave.crtHero.SelectPos == 3)//暴君 极限25点
            {
                if (battle_speed < 30)
                {
                    battle_speed = Mathf.Max(25, 30 - ((30 - battle_speed) / 8));
                }
                else
                battle_speed = Mathf.Max(30, battle_speed);
            }
            else
            battle_speed = Mathf.Max(30, battle_speed);
        }
        else
        {
            if (crt.hero_type == Hero_Type.法师)
            {
                if (SumSave.crtHero.SelectPos == 3)//暴君 极限25点
                {
                    if (battle_speed < 50)
                    {
                        battle_speed = Mathf.Max(40, 50 - ((50 - battle_speed) / 12));
                    }
                    else
                        battle_speed = Mathf.Max(50, battle_speed);
                }
                else
                    battle_speed = Mathf.Max(50, battle_speed);
            }else
            battle_speed = Mathf.Max(50, battle_speed);
        }
        crit = Mathf.Min(80, crit);
        

#if UNITY_EDITOR
        lucky = 9;
        //dc2 = 1500;
        //sc2 = 1500;
        //mc2 = 1500;
        //ac2 = 5000;
        //ac = 5000;
        //mac = 5000;
        //mac2 = 5000;
        //battle_speed = 30;
        ////battle_range = 300;
        ////maxmp = 1000000;
        ////battle_mp = 1000000;
        //mpRegen = 1000000;
        //maxhp = 1;
#elif UNITY_ANDROID
        //验证图鉴
#elif UNITY_IPHONE
#endif
        //
        crt.data = new FinalBattleValueVO(maxhp, maxmp, hp, mp, dc, dc2, mac, mac2, ac, ac2, sc, sc2, mc, mc2, hit, dodge, crit, critDmg, hpRegen,
            mpRegen, battle_hp, battle_mp, battle_ac, battle_mac, battle_dc, battle_sc, battle_mc, battle_speed, battle_range, battle_Damage, battle_def, talentList, lucky,damage_reduction,magic_damage_reduction,0);
#if UNITY_EDITOR
        //verification_illustrateds(illustrated_list, crt);
#elif UNITY_ANDROID
        //验证图鉴
        //verification_illustrateds(illustrated_list, crt);           
#elif UNITY_IPHONE
        //verification_illustrateds(illustrated_list, crt);       
#endif
        return crt;
    }


    /// <summary>
    /// 验证图鉴
    /// </summary>
    private static void verification_illustrateds(Dictionary<string, int> illustrateds,crtMaxBattleVO crt)
    {
        long exp = 0;
        int exp_bonus = crt.exp_bonus;
        for (int i = 0; i < SumSave.db_maps.Count; i++)
        {
            for (int j = 0; j < SumSave.db_maps[i].map_monster.Count; j++)
            {
                if (illustrateds.ContainsKey(SumSave.db_maps[i].map_monster[j]))
                {
                    string name = SumSave.db_maps[i].map_monster[j];
                    crtMaxBattleVO monster = SumSave.db_monsters.Find((x) => x.crt_name == name);
                    if (monster != null)
                        exp += (illustrateds[name] * monster.exp * (200 + exp_bonus)) / 100;
                }
            }
        }
        int lv = 1;
        while (exp >= SumSave.db_lvs[lv].exp)
        {
            exp-= SumSave.db_lvs[lv].exp;
            lv++;
        }
        if (lv >= crt.lv-3)//正常等级
        {

        }
        else
        {
            Game_Omphalos.i.Delete("验证图鉴等级 " + lv + " 当前等级" + crt.lv);
        }
    }

    /// <summary>
    /// 精炼计算等级
    /// </summary>
    /// <param name="total"></param>
    /// <returns></returns>
    public static int Refined_MaxNumbers(int total)
    {
        if (total == 0) return 0;
        int times = 0;
        for (int i = 1; i <= total; i++)
        {
            if (total < i) break;

            total -= i;
            times++;
        }
        return times;
    }

    public static crtMaxBattleVO Crate_Monster(crtMaxBattleVO monster)
    {
        int exp_bonus = 0, gold_bonus = 0, drop_bonus = 0, quality_bonus = 0;
        long maxhp = 0, maxmp = 0;
        int battle_hp = 0, battle_mp = 0, battle_ac = 0, battle_mac = 0, battle_dc = 0, battle_sc = 0, battle_mc = 0, battle_speed = 0, battle_range = 0, battle_Damage = 0, battle_def = 0;
        long hp = 0, mp = 0;
        int dc = 0, dc2 = 0, mac = 0, mac2 = 0, ac = 0, ac2 = 0, sc = 0, sc2 = 0, mc = 0, mc2 = 0;
        int hit = 0, dodge = 0, crit = 0, critDmg = 100;
        int hpRegen = 0, mpRegen = 0;
        int lucky = 0, damage_reduction = 0, magic_damage_reduction = 0;
        List<(enum_battle_pet_talent_list, float, float)> talentList = new List<(enum_battle_pet_talent_list, float, float)>();
        int lv = Mathf.Max(1, SumSave.map_Lv);//第一大陆属性加成
        int power = lv > 1 ? (lv * 500) : 100;
        maxhp = (long)(monster.data.battle_maxhp * (power) / 100);
        maxmp = 1000;
        hp = monster.data.battle_maxhp * (power) / 100;
        mp = 1000;
        if (lv > 1) power = (lv * 100);
        ac = monster.data.ac * (power) / 100;
        ac2 = monster.data.ac2 * (power) / 100;
        mac = monster.data.mac * (power) / 100;
        mac2 = monster.data.mac2 * (power) / 100;
        if (lv > 1) power = (lv / 2 * 150);
        dc = monster.data.dc * (power) / 100;
        dc2 = monster.data.dc2 * (power) / 100;
        sc = monster.data.sc * (power) / 100;
        sc2 = monster.data.sc2 * (power) / 100;
        mc = monster.data.mc * (power) / 100;
        mc2 = monster.data.mc2 * (power) / 100;
        hit = monster.data.hit * (power) / 100;
        dodge = monster.data.dodge;
        crit = monster.data.crit ;
        critDmg = monster.data.critDmg ;
        hpRegen = monster.data.hpRegen ;
        mpRegen = 1000;
        battle_hp = monster.data.battle_hp * (power) / 100;
        battle_mp = monster.data.battle_mp * (power) / 100;
        battle_ac = monster.data.battle_ac * (power) / 100;
        battle_mac = monster.data.battle_mac * (power) / 100;
        battle_dc = monster.data.battle_sc * (power) / 100;
        battle_sc = monster.data.battle_sc * (power) / 100;
        battle_mc = monster.data.battle_sc * (power) / 100;
        battle_speed = monster.data.battle_speed;
        battle_range = monster.data.battle_range;
        if (lv > 1) battle_range = Random.Range(monster.data.battle_range - 30, monster.data.battle_range + 30);
         battle_Damage = monster.data.battle_Damage;//真实伤害
        battle_def = monster.data.battle_def ;
        damage_reduction = monster.data.damage_reduction * (power) / 100;
        magic_damage_reduction = monster.data.magic_damage_reduction * (power) / 100;
        crtMaxBattleVO crt = new crtMaxBattleVO(exp_bonus, gold_bonus, drop_bonus, quality_bonus, 0);
        crt.crt_name = monster.crt_name;
        crt.lv = monster.lv;
        crt.exp = monster.exp;
        crt.hero_type = monster.hero_type;
        crt.type =  monster.type;
        //maxhp = 1; hp = 1; maxmp = 1; mp = 1; //测试
        crt.data = new FinalBattleValueVO(maxhp, (int)maxmp, hp, (int)mp, dc, dc2, mac, mac2, ac, ac2, sc, sc2, mc, mc2, hit, dodge, crit, critDmg, hpRegen,
            mpRegen, battle_hp, battle_mp, battle_ac, battle_mac, battle_dc, battle_sc, battle_mc, battle_speed, battle_range, battle_Damage, battle_def, talentList, lucky, damage_reduction, magic_damage_reduction, monster.data.move_speed);
        return crt;
    }

    public static crtMaxBattleVO Crate_MaxMonster(crtMaxBattleVO monster,int number)
    {
        int exp_bonus = 0, gold_bonus = 0, drop_bonus = 0, quality_bonus = 0;
        long maxhp = 0, maxmp = 0;
        int battle_hp = 0, battle_mp = 0, battle_ac = 0, battle_mac = 0, battle_dc = 0, battle_sc = 0, battle_mc = 0, battle_speed = 0, battle_range = 0, battle_Damage = 0, battle_def = 0;
        long hp = 0, mp = 0;
        int dc = 0, dc2 = 0, mac = 0, mac2 = 0, ac = 0, ac2 = 0, sc = 0, sc2 = 0, mc = 0, mc2 = 0;
        int hit = 0, dodge = 0, crit = 0, critDmg = 100;
        int hpRegen = 0, mpRegen = 0;
        int lucky = 0, damage_reduction = 0, magic_damage_reduction = 0;
        List<(enum_battle_pet_talent_list, float, float)> talentList = new List<(enum_battle_pet_talent_list, float, float)>();
        maxhp = 10000 * (number / 10 + 1);
        maxmp = 10000;
        hp = maxhp;
        mp = maxmp;
        bool isType = number % 2 == 0;//物理魔法
        if (isType)
        {
            ac = 100 * (number / 10 + 1);
            ac2 = 200 * (number / 10 + 1);
            dc = 50 * (number / 10 + 1);
            dc2 = 100 * (number / 10 + 1);
        }
        else
        { 
            mac = 100 * (number / 10 + 1);
            mac2 = 200 * (number / 10 + 1);
            mc = 50 * (number / 10 + 1);
            mc2 = 100 * (number / 10 + 1);
        }
        hit = 100 + (number / 10 + 1);
        dodge = 10 + (number / 50);
        crit = 10 + (number / 100);
        critDmg = 200 + (number / 10);
        hpRegen = (int)(maxhp / 10);
        if(hpRegen<0) hpRegen = 10000;
        mpRegen = 10000;
        battle_hp = 0;
        battle_mp = 0;
        battle_ac = 0;
        battle_mac = 0;
        battle_dc = 0;
        battle_sc = 0;
        battle_mc = 0;
        battle_speed = 50-(number / 30);
        if (battle_speed <= 10) battle_speed = 10;
        battle_range = Random.Range(200, 600);
        battle_Damage = 50 + number/10;//真实伤害
        battle_def = monster.data.battle_def;
        damage_reduction = 0;
        magic_damage_reduction = 0;
        crtMaxBattleVO crt = new crtMaxBattleVO(exp_bonus, gold_bonus, drop_bonus, quality_bonus, 0);
        crt.crt_name = monster.crt_name;
        crt.lv = monster.lv;
        crt.exp = 500000 * (number / 10 + 1);
        crt.hero_type = isType ? Hero_Type.战士 : Hero_Type.法师;
        crt.type = monster.type;
        crt.numbness = (number / 100);
        //maxhp = 1; hp = 1; maxmp = 1; mp = 1; //测试
        crt.data = new FinalBattleValueVO(maxhp, (int)maxmp, hp, (int)mp, dc, dc2, mac, mac2, ac, ac2, sc, sc2, mc, mc2, hit, dodge, crit, critDmg, hpRegen,
            mpRegen, battle_hp, battle_mp, battle_ac, battle_mac, battle_dc, battle_sc, battle_mc, battle_speed, battle_range, battle_Damage, battle_def, talentList, lucky, damage_reduction, magic_damage_reduction, monster.data.move_speed);
        return crt;
    }

    public static crtMaxBattleVO Crate_MaxBossMonster(crtMaxBattleVO monster, int number)
    {
        int exp_bonus = 0, gold_bonus = 0, drop_bonus = 0, quality_bonus = 0;
        long maxhp = 0, maxmp = 0;
        int battle_hp = 0, battle_mp = 0, battle_ac = 0, battle_mac = 0, battle_dc = 0, battle_sc = 0, battle_mc = 0, battle_speed = 0, battle_range = 0, battle_Damage = 0, battle_def = 0;
        long hp = 0, mp = 0;
        int dc = 0, dc2 = 0, mac = 0, mac2 = 0, ac = 0, ac2 = 0, sc = 0, sc2 = 0, mc = 0, mc2 = 0;
        int hit = 0, dodge = 0, crit = 0, critDmg = 100;
        int hpRegen = 0, mpRegen = 0;
        int lucky = 0, damage_reduction = 0, magic_damage_reduction = 0;
        List<(enum_battle_pet_talent_list, float, float)> talentList = new List<(enum_battle_pet_talent_list, float, float)>();
        maxhp = 1000000 + (number * 500000);
        maxmp = 10000;
        hp = maxhp;
        mp = maxmp;
        bool isType = number % 2 == 0;//物理魔法
        if (isType)
        {
            ac = 300 + (number * 30);
            ac2 = 600 + (number * 60);
            dc = 400 + (number * 40);
            dc2 = 500 + (number * 50);
        }
        else
        {
            mac = 300 + (number * 30);
            mac2 = 600 + (number * 60);
            mc = 400 + (number * 40);
            mc2 = 500 + (number * 50);
        }
        hit = 100 + number;
        dodge = 30 + (number / 2);
        crit = 30 + (number / 5);
        critDmg = 300 + (number * 2);
        hpRegen = (int)(maxhp / 10);
        if (hpRegen < 0) hpRegen = 10000;
        mpRegen = 10000;
        battle_hp = 0;
        battle_mp = 0;
        battle_ac = 0;
        battle_mac = 0;
        battle_dc = 0;
        battle_sc = 0;
        battle_mc = 0;
        battle_speed = 30 - (number * 2);
        if (battle_speed <= 5) battle_speed = 5;
        battle_range = Random.Range(200, 600);
        battle_Damage = 50 + number / 10;//真实伤害
        battle_def = monster.data.battle_def;
        damage_reduction = 0;
        magic_damage_reduction = 0;
        crtMaxBattleVO crt = new crtMaxBattleVO(exp_bonus, gold_bonus, drop_bonus, quality_bonus, 0);
        crt.crt_name = monster.crt_name;
        crt.lv = monster.lv;
        crt.exp = 5000000 * (number);
        crt.hero_type = isType ? Hero_Type.战士 : Hero_Type.法师;
        crt.type = monster.type;
        crt.numbness = 5 + (number / 5);
        //maxhp = 1; hp = 1; maxmp = 1; mp = 1; //测试
        crt.data = new FinalBattleValueVO(maxhp, (int)maxmp, hp, (int)mp, dc, dc2, mac, mac2, ac, ac2, sc, sc2, mc, mc2, hit, dodge, crit, critDmg, hpRegen,
            mpRegen, battle_hp, battle_mp, battle_ac, battle_mac, battle_dc, battle_sc, battle_mc, battle_speed, battle_range, battle_Damage, battle_def, talentList, lucky, damage_reduction, magic_damage_reduction, monster.data.move_speed);
        return crt;
    }


    public static string show_Talent(pet_talent_item item)
    {
        db_pet_talent_vo talent = item.GetTalentValue;
        string dec = talent.pet_talent_name + "\n";
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
        /// 2.10技能释放消耗减少
        /// 3特殊
        /// 1 增加生命上限
        /// 2 增加基础属性
        /// 3 概率随机传送一个敌人
        /// 4 击杀后追击另一个目标
        /// 5 攻击无视防御
        /// 6 攻击概率10倍
        /// 7 攻击概率斩杀
        /// 8 连击效果提升

        ObscuredInt lv = (ObscuredInt)MathF.Min(SumSave.crtHero.lv, 60);
        if (SumSave.crtHero.zs_lvs >= 2) lv = (SumSave.crtHero.zs_lvs - 1) * 20 + 60;
        talent.pet_up_lv = Mathf.Min(talent.pet_up_lv, talent.pet_up_offect.Count - 1);
        int pet_up_lv = talent.pet_up_lv;
        switch (talent.pet_talent_type)
        {
            case 3:
                switch ((talent.pet_talent_offect))
                {
                    case 1: dec += "生命上限 + " + Show_Color.Red(talent.pet_talent_offectvalue) + " %"
                            + (talent.pet_up_lv > -1 ? "\n" + Show_Color.Set_String("皇权 Lv."+ (pet_up_lv + 1) + " 加成: " + talent.pet_up_offect[talent.pet_up_lv] + " %", UnityColorPresets.HexToColor("#1E90FF")) : "");
                            break;
                    case 2:
                        dec += "基础属性\n" + enum_equip_entry_list.物理攻击 + " +" + Show_Color.Red(Show_Lv_offect(talent,lv,pet_up_lv))
                        + "\n" + enum_equip_entry_list.魔法攻击 + " +" + Show_Color.Red(Show_Lv_offect(talent, lv, pet_up_lv))
                        + "\n" + enum_equip_entry_list.道术攻击 + " +" + Show_Color.Red(Show_Lv_offect(talent, lv, pet_up_lv))
                        + Show_Color.Grey("\n(每级 + " + talent.pet_talent_offectvalue + ")")
                        +(talent.pet_up_lv > -1 ? "\n" + Show_Color.Set_String("皇权 Lv."+ (pet_up_lv +1)+ " 加成: 每级 + " + talent.pet_up_offect[talent.pet_up_lv] + "", UnityColorPresets.HexToColor("#1E90FF")) : "")
                       ; break;
                    case 3: dec += "攻击目标时 " + Show_Color.Red(talent.pet_talent_offecttype + "%") + " 概率 触发 " + "随机传送一个敌人"
                        + (talent.pet_up_lv > -1 ? "\n" + Show_Color.Set_String("皇权 Lv."+ (pet_up_lv + 1) + " 加成: 随机传送 " + talent.pet_up_offect[talent.pet_up_lv] + " 敌人", UnityColorPresets.HexToColor("#1E90FF")) : "");
                        break;
                    case 4: dec += "击杀后追击另一个目标\n每次触发消耗最大Hp的" + Show_Color.Red(talent.pet_talent_offectvalue+"%")
                        + (talent.pet_up_lv > -1 ? "\n" + Show_Color.Set_String("皇权 Lv."+ (pet_up_lv + 1) + " 加成: Hp消耗 " + talent.pet_up_offect[talent.pet_up_lv] + " %", UnityColorPresets.HexToColor("#1E90FF")) : "");
                        break;
                    case 5: dec += "攻击目标时 " + Show_Color.Red(talent.pet_talent_offecttype + "%") + " 概率 触发 " + Show_Color.Red("无视防御") + " 效果"
                        + (talent.pet_up_lv > -1 ? "\n" + Show_Color.Set_String("皇权 Lv."+ (pet_up_lv + 1) + " 加成: 概率 + " + talent.pet_up_offect[talent.pet_up_lv] + " %", UnityColorPresets.HexToColor("#1E90FF")) : "");
                        break;
                    case 6: dec += "攻击目标时 " + Show_Color.Red(talent.pet_talent_offecttype + "%") + " 概率 触发 " + Show_Color.Red(" 伤害 * " + talent.pet_talent_offectvalue+ "倍") + " 效果" 
                        + (talent.pet_up_lv > -1 ? "\n" + Show_Color.Set_String("皇权 Lv."+ (pet_up_lv + 1) + " 加成: 伤害 + " + talent.pet_up_offect[talent.pet_up_lv] + " 倍", UnityColorPresets.HexToColor("#1E90FF")) : "");
                        break;
                    case 7: dec += "攻击目标时 当目标血量低于" + Show_Color.Red(talent.pet_talent_offecttype + "%") + " 时 触发 " + Show_Color.Red("斩杀") + " 效果"
                        + (talent.pet_up_lv > -1 ? "\n" + Show_Color.Set_String("皇权 Lv."+ (pet_up_lv + 1) + " 加成: 血量低于 " + (talent.pet_talent_offecttype +talent.pet_up_offect[talent.pet_up_lv] )+ " % 触发", UnityColorPresets.HexToColor("#1E90FF")) : "");
                        break;
                    case 8: dec += "连击效果提升 " + Show_Color.Red(talent.pet_talent_offecttype + "%")
                        + (talent.pet_up_lv > -1 ? "\n" + Show_Color.Set_String("皇权 Lv."+ (pet_up_lv + 1) + " 加成: 伤害 + " + (talent.pet_up_offect[talent.pet_up_lv]) + " % ", UnityColorPresets.HexToColor("#1E90FF")) : "");
                        ; break;
                }
                break;
            case 1:
                switch ((talent.pet_talent_offect))
                {
                    case 1: dec += "物理伤害 + " + Show_Color.Red(talent.pet_talent_offectvalue) + " %"
                            + (talent.pet_up_lv > -1 ? "\n" + Show_Color.Set_String("皇权 Lv."+ (pet_up_lv + 1) + " 加成: 伤害 + " + (talent.pet_up_offect[talent.pet_up_lv]) + " % ", UnityColorPresets.HexToColor("#1E90FF")) : ""); break;
                    case 2: dec += "魔法伤害 + " + Show_Color.Red(talent.pet_talent_offectvalue) + " %"
                            + (talent.pet_up_lv > -1 ? "\n" + Show_Color.Set_String("皇权 Lv."+ (pet_up_lv + 1) + " 加成: 伤害 + " + (talent.pet_up_offect[talent.pet_up_lv]) + " % ", UnityColorPresets.HexToColor("#1E90FF")) : ""); break;
                    case 3: dec += "召唤兽伤害 + " + Show_Color.Red(talent.pet_talent_offectvalue) + " %"
                            + (talent.pet_up_lv > -1 ? "\n" + Show_Color.Set_String("皇权 Lv."+ (pet_up_lv + 1) + " 加成: 伤害 + " + (talent.pet_up_offect[talent.pet_up_lv]) + " % ", UnityColorPresets.HexToColor("#1E90FF")) : ""); break;
                    case 4: dec += "物理防御 + " + Show_Color.Red(talent.pet_talent_offectvalue * lv) + Show_Color.Grey("\n(每级 + " + talent.pet_talent_offectvalue + ")")
                            + (talent.pet_up_lv > -1 ? "\n" + Show_Color.Set_String("皇权 Lv."+ (pet_up_lv + 1) + " 加成: 每级 + " + talent.pet_up_offect[talent.pet_up_lv] + "", UnityColorPresets.HexToColor("#1E90FF")) : ""); break;
                    case 5: dec += "魔法防御 + " + Show_Color.Red(talent.pet_talent_offectvalue * lv) + Show_Color.Grey("\n(每级 + " + talent.pet_talent_offectvalue + ")")
                        +(talent.pet_up_lv > -1 ? "\n" + Show_Color.Set_String("皇权 Lv."+ (pet_up_lv + 1) + " 加成: 每级 + " + talent.pet_up_offect[talent.pet_up_lv] + "", UnityColorPresets.HexToColor("#1E90FF")) : ""); break;
                    case 6: dec += "每s回复 + " + Show_Color.Red(talent.pet_talent_offectvalue * lv) + " Hp" + Show_Color.Grey("\n(每级 + " + talent.pet_talent_offectvalue + ")")
                            + (talent.pet_up_lv > -1 ? "\n" + Show_Color.Set_String("皇权 Lv."+ (pet_up_lv + 1) + " 加成: 每级 + " + talent.pet_up_offect[talent.pet_up_lv] + "", UnityColorPresets.HexToColor("#1E90FF")) : ""); break;
                    case 7: dec += "每s回复 + " + Show_Color.Red(talent.pet_talent_offectvalue * lv) + " Mp" + Show_Color.Grey("\n(每级 + " + talent.pet_talent_offectvalue + ")")
                            + (talent.pet_up_lv > -1 ? "\n" + Show_Color.Set_String("皇权 Lv."+ (pet_up_lv + 1) + " 加成: 每级 + " + talent.pet_up_offect[talent.pet_up_lv] + "", UnityColorPresets.HexToColor("#1E90FF")) : ""); break;
                    case 8: dec += "受到物理伤害减少  " + Show_Color.Red(talent.pet_talent_offectvalue) + " %"
                             + (talent.pet_up_lv > -1 ? "\n" + Show_Color.Set_String("皇权 Lv."+ (pet_up_lv + 1) + " 加成: 物理伤害减少 " + talent.pet_up_offect[talent.pet_up_lv] + " %", UnityColorPresets.HexToColor("#1E90FF")) : ""); break;
                    case 9: dec += "受到魔法伤害减少  " + Show_Color.Red(talent.pet_talent_offectvalue) + " %"
                             + (talent.pet_up_lv > -1 ? "\n" + Show_Color.Set_String("皇权 Lv."+ (pet_up_lv + 1) + " 加成: 魔法伤害减少 " + talent.pet_up_offect[talent.pet_up_lv] + " %", UnityColorPresets.HexToColor("#1E90FF")) : ""); break;
                    case 11: dec += "躲避 + " + Show_Color.Red(talent.pet_talent_offectvalue) + " "
                             + (talent.pet_up_lv > -1 ? "\n" + Show_Color.Set_String("皇权 Lv."+ (pet_up_lv + 1) + " 加成: 躲避 +" + talent.pet_up_offect[talent.pet_up_lv] + " ", UnityColorPresets.HexToColor("#1E90FF")) : ""); break;
                    default:
                        break;
                }
                break;
            case 2:
                switch ((talent.pet_talent_offect))
                {
                    case 1:
                        dec += "攻击目标时 " + Show_Color.Red((Hero_Type)(talent.pet_talent_job)) + " 职业 "
                            //+ (talent.pet_talent_job == 3 ? "(召唤兽)" : "")
                            + Show_Color.Red(talent.pet_talent_offecttype + "%") + " 概率 触发 " + Show_Color.Red(talent.pet_talent_offectvalue + "%") + " 伤害"
                            + (talent.pet_up_lv > -1 ? "\n" + Show_Color.Set_String("皇权 Lv."+ (pet_up_lv + 1) + " 加成: 伤害 + " + (talent.pet_up_offect[talent.pet_up_lv]) + " % ", UnityColorPresets.HexToColor("#1E90FF")) : "");
                        break;
                    case 2:
                    case 3:
                    case 4:
                        dec += "攻击目标时 " + Show_Color.Red((Hero_Type)(talent.pet_talent_offect - 1)) + " 职业 "
                            + Show_Color.Red(talent.pet_talent_offecttype + "%") + " 概率 忽视 " + Show_Color.Red(talent.pet_talent_offectvalue * lv) + " 防御"
                            + (talent.pet_up_lv > -1 ? "\n" + Show_Color.Set_String("皇权 Lv."+ (pet_up_lv + 1) + " 加成: 每级 + " + talent.pet_up_offect[talent.pet_up_lv] + "", UnityColorPresets.HexToColor("#1E90FF")) : ""); break;
                    case 5:
                        dec += "受到伤害时 " + Show_Color.Red(talent.pet_talent_offecttype + "%") + "概率 反震 " + Show_Color.Red(talent.pet_talent_offectvalue + "%") + " 伤害"
                        + (talent.pet_up_lv > -1 ? "\n" + Show_Color.Set_String("皇权 Lv."+ (pet_up_lv + 1) + " 加成: 反震 + " + talent.pet_up_offect[talent.pet_up_lv] + " % 伤害", UnityColorPresets.HexToColor("#1E90FF")) : ""); break;
                    case 6: dec += "受到攻击时 " + Show_Color.Red(talent.pet_talent_offecttype + "%") + "概率 降低 " + Show_Color.Red(talent.pet_talent_offectvalue + "%") + " 暴击概率"
                            + (talent.pet_up_lv > -1 ? "\n" + Show_Color.Set_String("皇权 Lv."+ (pet_up_lv + 1) + " 加成: 降低 + " + talent.pet_up_offect[talent.pet_up_lv] + " % 暴击概率", UnityColorPresets.HexToColor("#1E90FF")) : ""); break;
                    case 7: dec += "受到伤害时 " + Show_Color.Red(talent.pet_talent_offecttype + "%") + "概率 降低 " + Show_Color.Red(talent.pet_talent_offectvalue + "%") + " 伤害"
                            + (talent.pet_up_lv > -1 ? "\n" + Show_Color.Set_String("皇权 Lv."+ (pet_up_lv + 1) + " 加成: 降低 + " + talent.pet_up_offect[talent.pet_up_lv] + " % 伤害", UnityColorPresets.HexToColor("#1E90FF")) : ""); break;
                    case 8: dec += "受到攻击时 " + Show_Color.Red(talent.pet_talent_offecttype + "%") + "概率 反弹 " + Show_Color.Red(talent.pet_talent_offectvalue + "%") + " 伤害"
                            + (talent.pet_up_lv > -1 ? "\n" + Show_Color.Set_String("皇权 Lv."+ (pet_up_lv + 1) + " 加成: 反弹 + " + talent.pet_up_offect[talent.pet_up_lv] + " % 伤害", UnityColorPresets.HexToColor("#1E90FF")) : ""); break;
                    case 10: dec += "技能释放消耗减少  " + Show_Color.Red(talent.pet_talent_offectvalue) + " %"
                            + (talent.pet_up_lv > -1 ? "\n" + Show_Color.Set_String("皇权 Lv."+ (pet_up_lv + 1) + " 加成: 技能释放消耗减少 + " + talent.pet_up_offect[talent.pet_up_lv] + " %", UnityColorPresets.HexToColor("#1E90FF")) : ""); break;
                }
                break;
            default:
                break;
        }

        return dec;
    }

    private static string Show_Lv_offect(db_pet_talent_vo talent, int lv,int pet_up_lv)
    {
        string dec = "";
        dec += (talent.pet_talent_offectvalue+(pet_up_lv>-1? talent.pet_up_offect[talent.pet_up_lv]:0)) * lv;
        return dec;
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
        ObscuredInt sum = (SumSave.crt_global_gift.GetGiftPoints);
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
            if (buff.ToString() == buffs[i].Item1)
            {
                int spanSeconds = Battle_Tool.SettlementTransport(buffs[i].Item2, 3);
                int time = buffs[i].Item3 - spanSeconds;//剩余时间
                if (time > 0 || buffs[i].Item3 >= 99999)
                {
                    return true;
                }
                else return false;
            } 
        }
        return false;

    }

    /// <summary>
    /// 初始化vip
    /// </summary>
    public static void Crate_Vip()
    {
        ObscuredInt sum = (SumSave.crt_global_gift.GetGiftPoints);
        if (sum == 0) return;
        for (int i = 0; i < SumSave.db_vip_list.Count; i++)
        {
            if (sum >= SumSave.db_vip_list[i].vip_exp)
            {
                crt_vip = SumSave.db_vip_list[i];
            }
        }
    }
    /// <summary>
    /// 刷新boss时间 boss名称
    ///（ObscuredInt，ObscuredInt，string）（地图类型，需求时间，记录最新时间）
    /// </summary>

    private static Dictionary<string, (ObscuredInt, ObscuredInt, string)> map_boss_time = new Dictionary<string, (ObscuredInt, ObscuredInt, string)>();
    /// <summary>
    /// 读取boss刷新时间
    /// </summary>
    public static void Carte_Read_Boss_Time()
    {
        Dictionary<string, (ObscuredInt,ObscuredInt,string)> dic = new Dictionary<string, (ObscuredInt,ObscuredInt, string)>();
        DateTime now = SumSave.nowtime >= DateTime.Now ? SumSave.nowtime : DateTime.Now;
        string value = Tool_UI.ToStandardFormat(now);
        for (int i = 0; i < SumSave.db_maps.Count; i++)
        {
            if (SumSave.db_maps[i].map_type == 0||true)
            {
                for (ObscuredInt j = 0; j < SumSave.db_maps[i].map_boss.Count; j++)
                {
                    if (!dic.ContainsKey(SumSave.db_maps[i].map_boss[j]))
                    {
                        dic[SumSave.db_maps[i].map_boss[j]] = (SumSave.db_maps[i].map_type, SumSave.db_maps[i].map_boss_cdtime[j], value);
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
    public static (ObscuredInt,ObscuredInt,string) GetBossTime(string map_id) { return (map_boss_time.ContainsKey(map_id))? map_boss_time[map_id]:(0,99999,"no"); }

    /// <summary>
    /// 计算剩余时间
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int Meet_maposs_criteria(string value)
    {
        (int, int, string) Boss_Time = GetBossTime(value);
        if (Boss_Time.Item3 == "no") return 99999999;
        int spanSeconds = Battle_Tool.SettlementTransport(Boss_Time.Item3, 2);
        db_vip crt_vip = Obtain_Vip();
        if (crt_vip != null)
        {
            ObscuredInt base_time = Boss_Time.Item2 * (100 - crt_vip.monsterHuntingInterval - (Tool_Battle.IsBuff(common_Buff.月卡) ? 5 : 0)) / 100;
            if (spanSeconds >= base_time)
            {
                return 0;
            }
            else return base_time - spanSeconds;
        }
        else
        {
            if (spanSeconds >= Boss_Time.Item2)
            {
                return 0;
            }
            else return Boss_Time.Item2 - spanSeconds; 
        }
    }
    /// <summary>
    /// 更新boss刷新时间
    /// </summary>
    /// <param name="map_id"></param>
    /// <param name="time"></param>
    /// <param name="value"></param>
    public static void SetBossTime(string map_id,string value)
    {
        map_boss_time[map_id] = (map_boss_time[map_id].Item1, map_boss_time[map_id].Item2, value);
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
        List<(enum_battle_pet_talent_list, float, float)> talentList = new List<(enum_battle_pet_talent_list, float, float)>();
        int lv = Mathf.Min(skill.DefPowers.Count - 1, skill.SetLv());
        int power = (skill.Power + skill.DefPowers[lv]);
        maxhp = (int)SumSave.crtMaxBattle.data.battle_maxhp * (power) / 100;
        maxmp = (int)SumSave.crtMaxBattle.data.battle_maxmp * (power) / 100;
        hp = (int)SumSave.crtMaxBattle.data.battle_maxhp * (power) / 100;
        mp = (int)SumSave.crtMaxBattle.data.battle_maxmp * (power) / 100;
        ac = (int)SumSave.crtMaxBattle.data.ac * (power) / 100;
        ac2 = (int)SumSave.crtMaxBattle.data.ac2 * (power) / 100;
        mac = (int)SumSave.crtMaxBattle.data.mac * (power) / 100;
        mac2 = (int)SumSave.crtMaxBattle.data.mac2 * (power) / 100;
        dc = (int)SumSave.crtMaxBattle.data.sc * (power) / 100;
        dc2 = (int)SumSave.crtMaxBattle.data.sc2 * (power) / 100;
        sc = (int)SumSave.crtMaxBattle.data.sc * (100) / 100;
        sc2 = (int)SumSave.crtMaxBattle.data.sc2 * (100) / 100;
        mc = (int)SumSave.crtMaxBattle.data.sc * (power) / 100;
        mc2 = (int)SumSave.crtMaxBattle.data.sc2 * (power) / 100;
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
        battle_dc = (int)SumSave.crtMaxBattle.data.battle_sc * (power) / 100;
        battle_sc = (int)SumSave.crtMaxBattle.data.battle_sc * (power) / 100;
        battle_mc = (int)SumSave.crtMaxBattle.data.battle_sc * (power) / 100;
        battle_speed = (int)SumSave.crtMaxBattle.data.battle_speed;
        battle_range = (int)SumSave.crtMaxBattle.data.battle_range * (power) / 100;
        battle_Damage = (int)SumSave.crtMaxBattle.data.battle_Damage * (power) / 100 + skill.skill_damages[lv];//真实伤害
        battle_def = (int)SumSave.crtMaxBattle.data.battle_def * (power) / 100;
        lucky = SumSave.crtMaxBattle.data.lucky;
        dc2 += skill.skill_damages[lv];
        sc2 += skill.skill_damages[lv];
        mc2 += skill.skill_damages[lv];
        switch (skill.id)
        {
            case 17://骷髅
                maxhp += 500;
                ac2 += 30; mac2+=30;dc2 += 30; sc2 += 30; mc2+=30;
                break;
            case 19://狗
                maxhp += 1000;
                sc2 += 60; mc2 += 60; dc2 += 60; sc2 += 60; mc2 += 60;
                break;
            default:
                break;
        }
        damage_reduction = SumSave.crtMaxBattle.data.damage_reduction * (power) / 100;
        magic_damage_reduction = SumSave.crtMaxBattle.data.magic_damage_reduction * (power) / 100;
        Dictionary<enum_talent_offect_list, int> buff = skill.GetBuff;
        foreach (var item in buff.Keys)
        {
            switch (item)
            {
                
                case enum_talent_offect_list.命中: hit += buff[item]; break;
                case enum_talent_offect_list.召唤兽攻击:dc2+= buff[item]; sc2+= buff[item];mc2 += buff[item]; break;
                case enum_talent_offect_list.召唤兽生命:maxhp += buff[item]; break;
                case enum_talent_offect_list.召唤兽防御:ac2 += buff[item]; mac2 += buff[item]; break;
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
        ac = ac * (100 + battle_ac) / 100;
        ac2 = ac2 * (100 + battle_ac) / 100;
        mac = mac * (100 + battle_mac) / 100;
        mac2 = mac2 * (100 + battle_mac) / 100; 
        dc = dc * (100 + battle_dc) / 100;
        dc2 = dc2 * (100 + battle_dc) / 100;
        sc = sc * (100 + battle_sc) / 100;
        sc2 = sc2 * (100 + battle_sc) / 100;
        mc = mc * (100 + battle_mc) / 100;
        mc2 = mc2 * (100 + battle_mc) / 100;
        talentList = SumSave.crtMaxBattle.data.buffList;
        crtMaxBattleVO crt = new crtMaxBattleVO(exp_bonus, gold_bonus, drop_bonus, quality_bonus, 0);
        crt.crt_name = "召" + skill.show_name;
        crt.lv = skill.SetLv();
        crt.exp = 0;
        crt.hero_type = (Hero_Type)skill.Effect;
        crt.type = Battle_Game_Type.call;
        //maxhp = 1; hp = 1; maxmp = 1; mp = 1; //测试
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
            value += talent.Item1.talent_offect_value[talent.Item2 - 1];
        }
        else
        {
            foreach (var item in skill_list)
            {
                skill_lv = item.Value.SetLv();
                if (item.Value.id == talent.Item1.correlation_skill)
                {
                    value += talent.Item1.talent_offect_value[talent.Item2 - 1] * skill_lv;
                }
            }
        }
        return value;
    }
    /// <summary>
    /// 判断首杀 true 是
    /// </summary>
    /// <param name="name"></param>
    public static bool Is_first_Boss_Kill(string value)
    {
        if (SumSave.crt_setting.Boss_list.ContainsKey(value))
        {
            return true;
        }
        return false;
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
        value += talent.Item1.talent_offect_value[talent.Item2-1];
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
                ObscuredInt skill_lv = item.Value.SetLv();
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
    private static void Obtain_enum_equip_entry_list(enum_equip_entry_list entry,ObscuredInt value)
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
    /// <summary>
    /// 初始化弹道
    /// </summary>
    public static void Obtain_Init_Entry_Inscription_list()
    {
        for (int i = 0; i < SumSave.db_skills.Count; i++)
        {
            if (SumSave.db_skills[i].EffectType != 5)//&& SumSave.db_skills[i].EffectType != 6召唤
            {
                Obtain_Weight(SumSave.db_skills[i].id + 1000, SumSave.db_skills[i].Weighted, equip_eighteditems);
            }
        }
    } 
    /// <summary>
    /// 获取装备属性
    /// </summary>
    /// <returns></returns>
    public static int Equip_Skill_eight()
    {
        if (equip_eighteditems.Count <= 1) Obtain_Init_Entry_Inscription_list();
        WeightedRandomPicker picker = new WeightedRandomPicker(equip_eighteditems);
        // 获取一个概率
        WeightedItem selectedItem = picker.GetRandomItem();
        //Debug.Log("selectedItem.prizedraw.ToString():" + selectedItem.prizedraw.ToString());
        return int.Parse(selectedItem.prizedraw.ToString());
    }
    public static int Equip_talent_eight()
    {
        if (talent_eighteditems.Count == 0) Obtain_Init_Entry_talent_eighteditems_list();
        WeightedRandomPicker picker = new WeightedRandomPicker(talent_eighteditems);
        // 获取一个概率
        WeightedItem selectedItem = picker.GetRandomItem();

        return int.Parse(selectedItem.prizedraw.ToString());
    }

    private static List<int> pet_talents_eighteditems = new List<int> { 5000, 5000, 1000, 100 };
    /// <summary>
    /// 获取天赋属性
    /// </summary>
    private static void Obtain_Init_Entry_talent_eighteditems_list()
    {
        //初始化
        for (int i = 0; i < SumSave.db_pet_talents.Count; i++)
        {
            Obtain_Weight(SumSave.db_pet_talents[i].pet_talent_id + 2000, pet_talents_eighteditems[SumSave.db_pet_talents[i].pet_talent_level], talent_eighteditems);
        }
    }

    private static void Obtain_Weight(object seed_name, int weight,List<WeightedItem> _eighteditems )
    {
        WeightedItem weightedItem = new WeightedItem(seed_name,weight);
        _eighteditems.Add(weightedItem);
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
    private static void Obtain_value<T>(Dictionary<T, int> dics,T type, int value = 1,bool exist=true,List<T> lists = null)
    {
        if (exist)
        {
            if (!dics.ContainsKey(type)) dics.Add(type, 0);
            dics[type] += value;
        }
        else
        {
            ObscuredInt number = 0;
            while (dics.ContainsKey(type))
            { 
                type = Obtain_Enum_list<T>(lists);
                number++;
                if (number >= 100) return;
            }
            dics.Add(type, value);
        }
        
    }

    private static void Obtain_values<T>(List<(T, int)> dics, T type, int value = 1, bool exist = true, List<T> lists = null)
    {
        if (exist)
        {
            dics.Add((type, value));
        }
        else
        {
            int number = 0;
            while (Is_Exist(dics, type))
            { 
               type = Obtain_Enum_list<T>(lists);
               number++;
               if (number >= 100) return;

            }
            dics.Add((type, value));
        }

    }

    /// <summary>
    /// 判断是否有该属性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lists"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private static bool Is_Exist<T>(List<(T, int)> lists, T type)
    {

        for (int i = 0; i < lists.Count; i++)
        { 

            if (lists[i].Item1.ToString() == type.ToString()) return true;
        }
        return false;
    }


    /// <summary>
    /// 获取材料
    /// </summary>
    /// <param name="type"></param>
    /// <param name="index"></param>
    /// <param name="maxnumber"></param>
    /// <param name="isverify"></param>
    public static void Obtain_Resources(int index, in ObscuredInt maxnumber, bool isverify = false)
    {
        SumSave.crt_bags.Get(Obtain_Int.Get(index), maxnumber, isverify);
    }

    /// <summary>
    /// 获取货币
    /// </summary>
    /// <param name="unit">类型</param>
    /// <param name="value">值</param>
    /// <param name="state">状态1可加成2直接获取</param>
    public static ObscuredInt Obtain_Unit(currency_unit unit, ObscuredInt value, int state = 1)
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
    private static ObscuredInt[] QualityWeighted = new ObscuredInt[] { 500000, 300000, 200000, 150000, 50000, 10000, 1000, 10, 10, 1 };
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
    /// 根据权重分配 数值越大 概率越大
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static ObscuredInt Obtain_WeightedItem(List<int> list)
    {
        List<WeightedItem> eighteditems = new List<WeightedItem>();
        for (int i = 0; i < list.Count; i++)
        {
            WeightedItem item = new WeightedItem(i + 1, list[i]);
            eighteditems.Add(item);
        }

        WeightedRandomPicker picker = new WeightedRandomPicker(eighteditems);
        // 获取一个概率
        WeightedItem selectedItem = picker.GetRandomItem();
        return int.Parse(selectedItem.prizedraw.ToString());

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
#if UNITY_EDITOR
        //return 7;
#elif UNITY_ANDROID
        
           
#elif UNITY_IPHONE
        
#endif
        int quality = int.Parse(selectedItem.prizedraw.ToString());
        if (quality > (int)enum_equip_quality_list.帝器)
        {
            //quality = (int)enum_equip_quality_list.帝器;
            if (SumSave.map_Lv <= 1) //皇器加成 
            {
                quality = (int)enum_equip_quality_list.帝器;
            }
        }
        return quality;
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
        //if (entry_inscription_list == null) Obtain_Init_Entry_Inscription_list();
        string user_value = bag.Name;
        if (bag.StdMode == Stditem_StdMode_List.项链.ToString())
        {
            if (quality >= 6)
            {
                if (Random.Range(0, 100) < 10)
                {
                    lv++;
                    if (Random.Range(0, 1000) < 1)
                    {
                        //lv++;
                    }
                    islock = 1;
                }
            }
        }
        //幸运等级
        user_value += " " + lv;
        //品质
        user_value += " " + quality;
        //是否可以锁定
        user_value += " " + islock;

        Dictionary<enum_equip_entry_list, int> dics = new Dictionary<enum_equip_entry_list, int>();
        Dictionary<enum_equip_entry_list, int> dics_2 = new Dictionary<enum_equip_entry_list, int>();
        List<(enum_equip_entry_list, int)> dics_2s = new List<(enum_equip_entry_list, int)>();
        List<int> dics_3 = new List<int>();
        //获取装备属性是否可以重叠
        int needlv = 60;
        if (quality > 1)
        {
            Obtain_value(dics, Obtain_Enum_list(entry_list));
            if (quality > 1)
            {
                Obtain_value(dics, Obtain_Enum_list(entry_list));
                if (quality > 2)
                {
                    Obtain_value(dics, Obtain_Enum_list(entry_list));
                    //Obtain_value(dics_2, Obtain_Enum_list(entry_coefficient_list), MaxValue(quality >= 7 ? 3 : 2), bag.need_lv > needlv, entry_coefficient_list);//元素属性
                    Obtain_values(dics_2s, Obtain_Enum_list(entry_coefficient_list), MaxValue(quality >= 7 ? 3 : 2), bag.need_lv > needlv, entry_coefficient_list);//元素属性

                    if (quality > 3)
                    {
                        Obtain_value(dics, Obtain_Enum_list(entry_list));
                        if (quality > 4)
                        {
                            Obtain_value(dics, Obtain_Enum_list(entry_list));
                            //Obtain_value(dics_2, Obtain_Enum_list(entry_coefficient_list), MaxValue(quality >= 7 ? 3 : 2), bag.need_lv > needlv, entry_coefficient_list);
                            Obtain_values(dics_2s, Obtain_Enum_list(entry_coefficient_list), MaxValue(quality >= 7 ? 3 : 2), bag.need_lv > needlv, entry_coefficient_list);//元素属性
                            if (quality > 5)
                            {
                                Obtain_value(dics, Obtain_Enum_list(entry_list));
                                if (Random.Range(0, 100) < 10 && quality == 6)
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
                                        //Obtain_value(dics_2, Obtain_Enum_list(entry_coefficient_list), MaxValue(quality >= 7 ? 3 : 2), bag.need_lv > needlv, entry_coefficient_list);
                                        Obtain_values(dics_2s, Obtain_Enum_list(entry_coefficient_list), MaxValue(quality >= 7 ? 3 : 2), bag.need_lv > needlv, entry_coefficient_list);//元素属性
                                        if (quality == 6) dics_3.Add(Equip_Skill_eight());
                                    }
                                    if (quality >= 7)
                                    {
                                        int max = MaxValue(quality >= 7 ? 3 : 2);
                                        //if(max<=1) max = 2;
                                        //Obtain_value(dics_2, Obtain_Enum_list(entry_coefficient_list), max, bag.need_lv > needlv, entry_coefficient_list);
                                        Obtain_values(dics_2s, Obtain_Enum_list(entry_coefficient_list), MaxValue(quality >= 7 ? 3 : 2), bag.need_lv > needlv, entry_coefficient_list);//元素属性
                                        for (int i = 0; i < 5; i++) Obtain_value(dics, Obtain_Enum_list(entry_list));

                                        max = MaxValue(3);
                                        if (max <= 1) max = 2; 
                                        for (int i = 0; i < max; i++) dics_3.Add(Equip_Skill_eight());//弹道
                                        max = MaxValue(3);
                                        for (int i = 0; i < max; i++) dics_3.Add(Equip_talent_eight());//皇权
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
            //exist = true;
            //if (dics_2.Count > 0)
            //{
            //    foreach (var item in dics_2)
            //    {
            //        if (exist)
            //        {
            //            value += "X";
            //            exist = false;
            //        }
            //        else value += "|";
            //        value += (int)item.Key + "," + item.Value;
            //    }
            //}
            exist = true;
            if (dics_2s.Count > 0)
            {
                foreach (var item in dics_2s)
                {
                    if (exist)
                    {
                        value += "X";
                        exist = false;
                    }
                    else value += "|";
                    value += (int)item.Item1 + "," + item.Item2;
                }
            }
            exist = true;
            if (dics_3.Count > 0)
            {
                for (ObscuredInt i = 0; i < dics_3.Count; i++)
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
    /// 生成最大值
    /// </summary>
    /// <param name="max"></param>
    /// <returns></returns>
    private static int MaxValue( int max)
    {
        List<int> list = new List<int>() { 1000, 500, 100, 10, 1 };
        if (max < list.Count)
        { 
         list.RemoveRange(max, list.Count - max);
        }
        return Obtain_WeightedItem(list);
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
    public static string Obtain_Pet(ObscuredInt index, int crate_skill_random =0)
    {
        db_pet_vo base_pet = ArrayHelper.Find(SumSave.db_pets, (item) => item.pet_id == index);
        db_pet_vo pet = new db_pet_vo(base_pet.pet_id, base_pet.pet_name, base_pet.pet_ac, base_pet.pet_mac, base_pet.pet_dc, base_pet.pet_mc, base_pet.pet_sc, base_pet.pet_talent, base_pet.pet_scale);
        //0:可修改名称 1本命 2id 3基础数值 4极品值5技能
        string user_value = pet.pet_id + ","+pet.pet_name + "," + pet.pet_name + ",";
        user_value += Random.Range(1, pet.pet_ac / 3) + "X" + Random.Range(1, pet.pet_mac / 3) + "X"
            + Random.Range(1, pet.pet_dc / 3) + "X" + Random.Range(1, pet.pet_mc / 3) + "X" + Random.Range(1, pet.pet_sc / 3) + ",";
        for (ObscuredInt i = 0; i < 5; i++)
        {
            user_value += Random.Range(0, pet.pet_id + 5) + (i== 4 ? "," : "X");
        }
        //添加技能 多个技能X分隔
        ObscuredInt skill_random = Random.Range(1, pet.pet_id / 3 + 1);
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
    private static string Radom_Talent(ObscuredInt max)
    { 
        string value = "";
        List<db_pet_talent_vo> list = ArrayHelper.FindAll(SumSave.db_pet_talents, e => e.pet_talent_level == 1);
        List<db_pet_talent_vo> list1 = ArrayHelper.FindAll(SumSave.db_pet_talents, e => e.pet_talent_level == 2);
        string random = "";
        for (ObscuredInt i = 0; i < max; i++)
        {
            ObscuredInt number = 0;
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
