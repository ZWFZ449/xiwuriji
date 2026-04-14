using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dream_setting_item : Base_Mono
{
    private Dropdown dropdown;

    private InputField inputField;

    private Text unit;
    /// <summary>
    /// 药品类型
    /// </summary>
    private List<string> dropdown_list;
    /// <summary>
    /// 当前设置
    /// </summary>
    private (int,int,string,int) currentItem;

    private void Awake()
    {
        dropdown=Find<Dropdown>("Dropdown");
        dropdown.onValueChanged.AddListener(OnDropdownChange);
        inputField=Find<InputField>("InputField");
        inputField.onValueChanged.AddListener(OnInputFieldChange);
        unit = Find<Text>("InputField/unit");
    }
    /// <summary>
    /// 下拉框内容
    /// </summary>
    /// <param name="arg0"></param>
    private void OnDropdownChange(int arg0)
    {
        currentItem.Item3 = dropdown_list[arg0];
        send();
    }

    private void send()
    { 
    transform.parent.parent.parent.parent.parent.parent.SendMessage("DreamSetting", currentItem);
    }
    /// <summary>
    /// 输入框内容
    /// </summary>
    /// <param name="arg0"></param>
    private void OnInputFieldChange(string input)
    {
        string cleanInput = System.Text.RegularExpressions.Regex.Replace(input, @"[^\d%]", "");
        if (cleanInput != "")
        {
            int value = int.Parse(cleanInput);
            //自动召唤boss不限制次数
            if (currentItem.Item1 != 3)
            {
                if (value > 100) value = 100;
            }
            if (value < 0) value = 0;
            inputField.text = value.ToString();
            currentItem.Item4 = value;
            send();
        }
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public void Init(medicineType type,string dream,int input=60)
    {
        dropdown.options.Clear();
        dropdown_list = new List<string>();
        unit.text = "%";
        for (int i = 0; i < SumSave.db_stditems.Count; i++)
        {
            if (SumSave.db_stditems[i].StdMode==Stditem_StdMode_List.消耗品.ToString() &&SumSave.db_stditems[i].Shape == (int)type)
            {
                dropdown_list.Add(SumSave.db_stditems[i].Name);
            }
        }
        dropdown.AddOptions(dropdown_list);
        currentItem = (2,(int)type, dream, input);
        for (int i = 0; i < dropdown_list.Count; i++)
        {
            if (dropdown_list[i] == dream)
            { 
                dropdown.value = i;
            }
        }
        inputField.text = input.ToString();
    }
    /// <summary>
    /// 回收
    /// </summary>
    /// <param name="type"></param>
    /// <param name="input"></param>
    public void Init(enum_equip_quality_list type,int input = 60)
    {
        dropdown.options.Clear();
        dropdown_list = new List<string>();
        dropdown.interactable = false;//禁用下拉框
        unit.text = "级";
        dropdown_list.Add("自动回收 " + Show_Color.Red(type) + " 品质装备" + "回收等级:");
        dropdown.AddOptions(dropdown_list);
        currentItem = (1,(int)type, "", input);
        inputField.text = input.ToString();
    }
    /// <summary>
    /// 自动召唤boss
    /// </summary>
    /// <param name="boss"></param>
    /// <param name="input"></param>
    public void Init(string boss, int input = 0)
    {
        dropdown.options.Clear();
        dropdown_list = new List<string>();
        dropdown.interactable = false;
        unit.text = "次";
        dropdown_list.Add("自动召唤击杀 " + Show_Color.Red(boss) + " 次数:");
        dropdown.AddOptions(dropdown_list);
        currentItem = (3, 3, boss, input);
        inputField.text = input.ToString();
    }

}
