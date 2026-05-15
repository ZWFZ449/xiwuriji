using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class show_boss_time : Base_Mono
{
    private Transform m_boss_time_brom;

    private info_time_item info_time_item_prefab;

    private Dictionary<string, info_time_item> dic_info_time_item = new Dictionary<string, info_time_item>();

    private Button close;
    private void Awake()
    {
        m_boss_time_brom = Find<Transform>("boss_time_brom/Viewport/Content");
        info_time_item_prefab = Tool_UI.Find_Prefabs<info_time_item>("info_time_item");
        close = Find<Button>("close_button");
        close.onClick.AddListener(() => { gameObject.SetActive(false); });
        Init();
    }

    private void Init()
    {
        if (dic_info_time_item.Count > 0) return;

        for (int i = 0; i < SumSave.db_maps.Count; i++)
        {
            if (SumSave.db_maps[i].map_type == 0)
            {
                for (int j = 0; j < SumSave.db_maps[i].map_boss.Count; j++)
                {
                    if (!dic_info_time_item.ContainsKey(SumSave.db_maps[i].map_boss[j]))
                    {
                        info_time_item info_time_item = Instantiate(info_time_item_prefab, m_boss_time_brom);
                        dic_info_time_item.Add(SumSave.db_maps[i].map_boss[j], info_time_item);
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        if (SumSave.crtHero.lv <= 20 && !Tool_Battle.IsBuff(common_Buff.月卡))
        {
            Alert_Dec.Show("等级不足,无法查看");
            gameObject.SetActive(false);
            return;
        }
        Info(); 
        StopCoroutine(UpdateBossTime());
        StartCoroutine(UpdateBossTime());
        
    }
    private void Info()
    {
        foreach (KeyValuePair<string, info_time_item> item in dic_info_time_item)
        {
            string value = item.Key;

            for (int i = 0; i < SumSave.crt_setting.battle_Boss_list.Count; i++)
            {
                (string, int) boss = SumSave.crt_setting.battle_Boss_list[i];
                List<string> list = ArrayHelper.Get_Split<string>(boss.Item1, '+');
                if (list.Count == 2)
                {
                    if (list[0] == item.Key)
                    {
                        value += " 存量 " + list[1];
                        break;
                    }
                }
            }
            value+= " 倒计时:" + ConvertSecondsToHHMMSS(Tool_Battle.Meet_maposs_criteria(item.Key));
            item.Value.SetInfo(value);
        }

    }
    private IEnumerator UpdateBossTime()
    {
        yield return new WaitForSeconds(1f);
        Info();
        StartCoroutine(UpdateBossTime());
    }
    private int Meet_maposs_criteria(string value)
    {
        (int, string) Boss_Time = Tool_Battle.GetBossTime(value);
        if (Boss_Time.Item2 == "no") return 99999999;
        int spanSeconds = Battle_Tool.SettlementTransport(Boss_Time.Item2, 2);
        db_vip crt_vip = Tool_Battle.Obtain_Vip();
        if (crt_vip != null)
        {
            int base_time = Boss_Time.Item1 * (100 - crt_vip.monsterHuntingInterval - (Tool_Battle.IsBuff(common_Buff.月卡) ? 5 : 0)) / 100;
            if (spanSeconds >= base_time)
            {
                return 0;
            }
            else return base_time - spanSeconds;
        }
        else
        {
            if (spanSeconds >= Boss_Time.Item1)
            {
                return 0;
            }
            else return Boss_Time.Item1 - spanSeconds;
        }
    }

}
