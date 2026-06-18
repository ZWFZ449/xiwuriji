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

public class offect_gem : Base_Mono
{
    private TMP_Dropdown need_Dropdown, result_Dropdown;
    /// <summary>
    /// 兑换宝石
    /// </summary>
    private Button confirm;

    private List<string> need_list= new List<string>();
    private void Awake()
    {
        need_Dropdown = Find<TMP_Dropdown>("need_Dropdown");
        result_Dropdown = Find<TMP_Dropdown>("result_Dropdown");
        confirm= Find<Button>("confirm");
        confirm.onClick.AddListener(Confirm);
        Init();
    }

    private void Confirm()
    {
        if (need_Dropdown.value == result_Dropdown.value) { Alert_Dec.Show("请选择宝石"); return; }
        Alert.Show("宝石转换", need_list[need_Dropdown.value] + "转换为" + need_list[result_Dropdown.value], ConfirmGem);
    }

    private void ConfirmGem(object arg0)
    {
        Clear_Condition();
        Need_Condition(need_list[need_Dropdown.value], 10);
        Need_Condition(currency_unit.元宝, 100);
        if (Return_Condition())
        {
            int number = 8;
            int random = Random.Range(1, 1000);
            int maxnumber = number + Random.Range(1, 1000);
            Alert_Dec.Show("获得 " + need_list[result_Dropdown.value] + " * " + number);
            Battle_Tool.Dream_Obtain_Resources(Obtain_Int.Add(1, need_list[result_Dropdown.value], new ObscuredInt[] { number + random, random }), maxnumber);
        }
        else
            Alert_Dec.Show("转换失败");
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
    }
}
