using CodeStage.AntiCheat.ObscuredTypes;
using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityColorPresets;
using Random = UnityEngine.Random;
/// <summary>
/// 转生
/// </summary>
public class offect_reincarnation : Base_Mono
{
    private Button reset_talent;

    private TMP_Text info;
    private void Awake()
    {
        reset_talent = Find<Button>("reset_talent");
        reset_talent.onClick.AddListener(ResetTalent);
        info=Find<TMP_Text>("Scroll View/Viewport/Content/info/Text (TMP)");
    }

    private void OnEnable()
    {
        if (SumSave.crtHero.lv < 30 && SumSave.crtHero.zs_lv == 0)
        {
            Hide();
            Alert_Dec.Show("当前等级不足60级无法转生");
            return;
        }
        init();
    }
    private void init()
    {
        string dec = "";
        for (int i = 0; i < SumSave.db_reincarnation_list.Count; i++)
        {
            if (SumSave.db_reincarnation_list[i].reincarnation_lv == SumSave.crtHero.zs_lv)
            { 
                db_reincarnation_vo vo = SumSave.db_reincarnation_list[i];
                dec += vo.reincarnation_name + "\n";
                dec += "需求\n";
                for (int j = 0; j < vo.reincarnation_need.Count; j++)
                {
                    List<string> need = ArrayHelper.Get_Split<string>(vo.reincarnation_need[j], ' ');
                    if (need.Count == 3)
                    {
                        switch (int.Parse(need[0]))
                        {
                            case 1://需要等级
                                dec += Show_Color.Set_String("转生次数 " + need[1] + " Lv." + need[2], GameColors.ManaBar);
                                break;
                           case 2://需要需求
                                dec += Show_Color.Set_String("\n" + need[1] + " * " + need[2], GameColors.ManaBar);
                                break;
                           case 3://需要技能
                                dec += Show_Color.Set_String("\n" + need[2] + " " + ((currency_unit)int.Parse(need[1])), GameColors.ManaBar);
                                break;
                            default:
                                break;
                        }
                    } 
                }
                dec += "\n转生加成\n";
                for (int j = 0; j < vo.reincarnation_cost.Count; j++)
                {
                    enum_equip_entry_list e = vo.reincarnation_cost[j].Item1;
                    int value = vo.reincarnation_cost[j].Item2;
                    switch (e)
                    {
                        case enum_equip_entry_list.生命值:
                        case enum_equip_entry_list.魔法值:

                            dec += Show_Color.Set_String(e + " : " + value * 5, GameColors.EnergyBar);
                            break;
                        case enum_equip_entry_list.物理防御:
                        case enum_equip_entry_list.魔法防御:
                        case enum_equip_entry_list.物理攻击:
                        case enum_equip_entry_list.魔法攻击:
                        case enum_equip_entry_list.道术攻击:
                            dec+= Show_Color.Set_String(e + " : " + "0 - " + value, GameColors.EnergyBar);
                            break;
                        case enum_equip_entry_list.每秒回血:
                        case enum_equip_entry_list.每秒回蓝:
                        case enum_equip_entry_list.真实伤害:
                        case enum_equip_entry_list.吸收伤害:
                        case enum_equip_entry_list.幸运:
                        case enum_equip_entry_list.命中:
                        case enum_equip_entry_list.闪避:
                            dec+= Show_Color.Set_String(e + " : " + value, GameColors.EnergyBar);
                            break;
                        case enum_equip_entry_list.物理下防:
                            e = enum_equip_entry_list.物理防御;
                            dec+= Show_Color.Set_String(e + " : " + value + " - 0", GameColors.EnergyBar);
                            break;
                        case enum_equip_entry_list.魔法下防:
                            e = enum_equip_entry_list.魔法防御;
                            dec+= Show_Color.Set_String(e + " : " + value + " - 0", GameColors.EnergyBar);
                            break;
                        case enum_equip_entry_list.物理下攻:
                            e = enum_equip_entry_list.物理攻击;
                            dec+= Show_Color.Set_String(e + " : " + value + " - 0", GameColors.EnergyBar);
                            break;
                        case enum_equip_entry_list.魔法下攻:
                            e = enum_equip_entry_list.魔法攻击;
                            dec+= Show_Color.Set_String(e + " : " + value + " - 0", GameColors.EnergyBar);
                            break;
                        case enum_equip_entry_list.道术下攻:
                            e = enum_equip_entry_list.道术攻击;
                            dec+= Show_Color.Set_String(e + " : " + value + " - 0", GameColors.EnergyBar);
                            break;
                        case enum_equip_entry_list.生命属性:
                        case enum_equip_entry_list.魔法属性:
                        case enum_equip_entry_list.防御属性:
                        case enum_equip_entry_list.魔防属性:
                        case enum_equip_entry_list.物攻属性:
                        case enum_equip_entry_list.魔攻属性:
                        case enum_equip_entry_list.道攻属性:
                        case enum_equip_entry_list.攻击速度:
                        case enum_equip_entry_list.攻击范围:
                        case enum_equip_entry_list.暴击属性:
                        case enum_equip_entry_list.暴击伤害:
                        case enum_equip_entry_list.物伤减免:
                        case enum_equip_entry_list.魔伤减免:
                        case enum_equip_entry_list.怪物爆率:
                        case enum_equip_entry_list.极品爆率:
                        case enum_equip_entry_list.经验加成:
                        case enum_equip_entry_list.金币掉落:
                            dec+= Show_Color.Set_String(e + " : " + value + " %", GameColors.EnergyBar);
                            break;
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
                            dec+= Show_Color.Set_String(e + " : Lv." + value, GameColors.EnergyBar);
                            break;
                        default:
                            if ((int)e >= 1000)//附加技能
                            {
                                string skill_name = "";
                                skill_name = ArrayHelper.Find(SumSave.db_skills, x => x.id == (((int)e) - 1000)).show_name;
                                dec += Show_Color.Set_String(skill_name + " : Lv." + value, GameColors.EnergyBar);
                            }
                            break;
                    }
                    dec += "\n";
                }
            }
        }
        info.text= dec;
    }

    /// <summary>
    /// 转生
    /// </summary>
    private void ResetTalent()
    {

    }

    private void Hide()
    {
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }

    /// <summary>
    /// 重置职业
    /// </summary>
    private void ResetJob()
    {
        if (SumSave.crtHero.job == 0)
        {
            Alert_Dec.Show("当前职业为默认职业无法重置");
            return;
        }
        List<long> Units = SumSave.crt_user_unit.Set();
        int buy = 5000;
        if (Units[(int)currency_unit.元宝] >= buy)
        {
            Battle_Tool.Dream_Obtain_Unit(currency_unit.元宝, -buy, Obtain_Int.Add_unit(-buy));
            Dictionary<string, int> dic = new Dictionary<string, int>();
            for (int i = 0; i < SumSave.crtHero.talent.Count; i++)
            {
                int lv = SumSave.crtHero.talent[i].Item2;
                for (int j = 0; j < lv; j++)
                {
                    if (!dic.ContainsKey(SumSave.crtHero.talent[i].Item1.ralent_need_uplv_value[j]))
                    {
                        dic.Add(SumSave.crtHero.talent[i].Item1.ralent_need_uplv_value[j], SumSave.crtHero.talent[i].Item1.ralent_need_uplv[j]);
                    }
                    else
                    {
                        dic[SumSave.crtHero.talent[i].Item1.ralent_need_uplv_value[j]] += SumSave.crtHero.talent[i].Item1.ralent_need_uplv[j];
                    }
                }
            }
            string dec = "重置返还";
            foreach (var item in dic.Keys)
            {
                dec += "\n" + item + " " + dic[item];
                ObscuredInt  random = Random.Range(1, 1000);
                ObscuredInt  maxnumber = dic[item] + Random.Range(1, 1000);
                Battle_Tool.Dream_Obtain_Resources(Obtain_Int.Add(1, item, new ObscuredInt [] { dic[item] + random, random }), maxnumber);
            }
            SumSave.crtHero.talent.Clear();
            SumSave.crtHero.SelectPos = -1;
            SumSave.crtHero.job = 0;
            //刷新数据
            SendNotification(NotiList.Refresh_Max_Hero_Attribute);
            SumSave.crtHero.MysqlData();
            Alert.Show("重置职业成功", dec);
            Hide();
        }
        else Alert_Dec.Show("元宝不足");
    }
}
