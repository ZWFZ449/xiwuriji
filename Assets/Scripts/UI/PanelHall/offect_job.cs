using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class offect_job : Base_Mono
{
    private Button reset_job, reset_talent;

    private Text info;
    private void Awake()
    {
        reset_talent = Find<Button>("reset_talent");
        reset_job = Find<Button>("reset_job");
        reset_job.onClick.AddListener(ResetJob);
        reset_talent.onClick.AddListener(ResetTalent);
        info=Find<Text>("Scroll View/Viewport/Content/info");
        init();
    }

    private void init()
    {
        info.text="[重置天赋] "+"需要"+Show_Color.Red(1000)+ currency_unit.元宝
            + "\n\n[重置职业] " + "需要"+Show_Color.Red(5000)+ currency_unit.元宝+
            "\n注释:" +
            "\n1.重置天赋返还所有消耗书页" +
            "\n2.重置职业默认重置天赋";
    }

    /// <summary>
    /// 重置天赋
    /// </summary>
    private void ResetTalent()
    {
        List<long> Units = SumSave.crt_user_unit.Set();
        int buy = 1000;
        if (Units[(int)currency_unit.元宝] >= buy)
        {
            Battle_Tool.Dream_Obtain_Unit(currency_unit.元宝, -buy, Obtain_Int.Add_unit(-buy));
            Dictionary<string,int> dic = new Dictionary<string, int>();
            for (int i = 0; i < SumSave.crtHero.talent.Count; i++)
            {
                int lv= SumSave.crtHero.talent[i].Item2;
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
                dec+= "\n"+ item + " " + dic[item];
                int random = Random.Range(1, 1000);
                int maxnumber = dic[item] + Random.Range(1, 1000);
                Battle_Tool.Dream_Obtain_Resources(Obtain_Int.Add(1, item, new int[] { dic[item] + random, random }), maxnumber);
            }
            SumSave.crtHero.talent.Clear();
            SumSave.crtHero.SelectPos = -1;
            //刷新数据
            SendNotification(NotiList.Refresh_Max_Hero_Attribute);
            SumSave.crtHero.MysqlData();
            Alert.Show("重置天赋成功", dec);
            Hide();
        }
         
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
                int random = Random.Range(1, 1000);
                int maxnumber = dic[item] + Random.Range(1, 1000);
                Battle_Tool.Dream_Obtain_Resources(Obtain_Int.Add(1, item, new int[] { dic[item] + random, random }), maxnumber);
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
    }
}
