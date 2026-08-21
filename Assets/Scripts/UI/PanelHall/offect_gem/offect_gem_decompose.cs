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
using Random = UnityEngine.Random;

public class offect_gem_decompose: Base_Mono
{
    private TMP_Dropdown need_Dropdown, result_Dropdown;
    /// <summary>
    /// 分解宝石
    /// </summary>
    private Button confirm;

    private List<string> need_list= new List<string>();
    /// <summary>
    /// 选中宝石
    /// </summary>
    private string crt_gem;

    private TMP_Text info;
    private void Awake()
    {
        need_Dropdown = Find<TMP_Dropdown>("need_Dropdown");
        need_Dropdown.onValueChanged.AddListener(OnNeedChange);
        result_Dropdown = Find<TMP_Dropdown>("result_Dropdown");
        info= Find<TMP_Text>("info");
        confirm = Find<Button>("confirm");
        confirm.onClick.AddListener(Confirm);
        Init();
    }


    private void OnNeedChange(int arg0)
    {
        crt_gem= need_list[arg0];
        info.text = "选择分解宝石为 " + crt_gem + "";

    }

    private void Confirm()
    {
        if (crt_gem==null || crt_gem=="") { Alert_Dec.Show("请选择宝石"); return;}
        string need = crt_gem.Split('宝')[0];
        List<string> gems = SumSave.crt_bags.Get_Gem_Value;
        List<string> needs= new List<string>();
        int sums = 0;
        for (int i = 0; i < gems.Count; i++)
        {
            if (gems[i].Contains(need))
            {
                string[] strs = gems[i].Split('v');
                if (strs.Length > 1)
                { 
                    int number = int.Parse(strs[1]);
                    needs.Add(gems[i]);
                    sums += (int)Mathf.Pow(2, number) * 10 - 10;
                }
            }
        }
        if (sums==0) { Alert_Dec.Show("背包内没有" + crt_gem + "宝石"); return; }
        Alert.Show("宝石分解", "背包内全部的" + crt_gem + "宝石可分解成\n" + sums + " * " + crt_gem + "\n需要" + currency_unit.元宝 + " * " + (sums * 50), ConfirmGem, needs);
    }

    private void ConfirmGem(object arg0)
    {
        List<string> gems = (List<string>)arg0;
        Clear_Condition();
        long sums = 0;
        for (int i = 0; i < gems.Count; i++)
        {
            string[] strs = gems[i].Split('v');
            if (strs.Length > 1)
            {
                int number = int.Parse(strs[1]);
                sums += (int)Mathf.Pow(2, number) * 10 - 10;
            }
        }
        Need_Condition(currency_unit.元宝, sums * 50);
        if (Return_Condition())
        {
            int number = (int)sums;
            int random = Random.Range(1, 1000);
            int maxnumber = number + Random.Range(1, 1000);
            Alert_Dec.Show("获得 " +crt_gem + " * " + number);
            Battle_Tool.Dream_Obtain_Resources(Obtain_Int.Add(1, crt_gem, new ObscuredInt[] { number + random, random }), maxnumber);
            for (int i = 0; i < gems.Count; i++)
            {
                SumSave.crt_bags.Get_Gem_Value.Remove(gems[i]);
            }
            SumSave.crt_bags.MysqlData();
        }
        else
            Alert_Dec.Show("分解失败");
    }

    private void Init()
    {
        need_list = new List<string>();
        for (int i = 0; i < SumSave.db_stditems.Count; i++)
        {
            if (SumSave.db_stditems[i].StdMode == Stditem_StdMode_List.材料.ToString() && SumSave.db_stditems[i].Shape > 0)
            {
                need_list.Add(SumSave.db_stditems[i].Name);
            }
        }
        need_list.Add(need_list[need_list.Count - 1]);
        need_Dropdown.options.Clear();
        need_Dropdown.AddOptions(need_list);
        result_Dropdown.options.Clear();
        result_Dropdown.AddOptions(need_list);
        info.text = "请选择需要分解的宝石";
    }
}
