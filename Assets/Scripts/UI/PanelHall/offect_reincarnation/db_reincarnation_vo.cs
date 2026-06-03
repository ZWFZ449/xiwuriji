using CodeStage.AntiCheat.ObscuredTypes;
using Common;
using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class db_reincarnation_vo : Base_VO
{
    public readonly ObscuredInt reincarnation_lv;

    public readonly string reincarnation_name;

    public readonly List<string> reincarnation_need;

    public readonly ObscuredInt need_lv, need_maxLv, result_maxRefinement, result_maxmedicine, result_minRefinement, result_minmedicine;

    public readonly List<(enum_equip_entry_list, ObscuredInt)> reincarnation_cost;

    public db_reincarnation_vo(int reincarnation_lv, string reincarnation_name, string reincarnation_need, string reincarnation_cost, int need_lv,int need_maxLv, int result_maxRefinement, int result_maxmedicine, int result_minRefinement, int result_minmedicine)
    { 
    
        this.reincarnation_lv = reincarnation_lv;
        this.reincarnation_name = reincarnation_name;
        this.reincarnation_need = ArrayHelper.Get_Split<string>(reincarnation_need, ';');
        List<string> cost = ArrayHelper.Get_Split<string>(reincarnation_cost, ';');
        this.reincarnation_cost = new List<(enum_equip_entry_list, ObscuredInt)>();

        for (int i = 0; i < cost.Count; i++)
        {
            List<string> cost_split = ArrayHelper.Get_Split<string>(cost[i], '|');
            this.reincarnation_cost.Add(((enum_equip_entry_list)(int.Parse(cost_split[0])), int.Parse(cost_split[1])));
        }
        this.need_lv = need_lv;
        this.need_maxLv = need_maxLv;
        this.result_maxRefinement = result_maxRefinement;
        this.result_maxmedicine = result_maxmedicine;
        this.result_minRefinement = result_minRefinement;
        this.result_minmedicine = result_minmedicine;
    }
 }
