using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 分配技能伤害
/// </summary>
public class allocation_skill_damage : Base_Mono
{
    /// <summary>
    /// 显示信息
    /// </summary>
    private Text info;
    /// <summary>
    /// 确认
    /// </summary>
    private Button confirm_btn;
    /// <summary>
    /// 分配内力
    /// </summary>
    private Slider slider;
    /// <summary>
    /// 当前分配技能
    /// </summary>
    private base_skill_vo user_skill;

    private Button close;
    private void Awake()
    {
        close = Find<Button>("close_button");
        close.onClick.AddListener(() => { gameObject.SetActive(false); });
        info = Find<Text>("base_info");
        confirm_btn = Find<Button>("confirm_btn");
        slider = Find<Slider>("Slider");
        confirm_btn.onClick.AddListener(Confirm);
        slider.onValueChanged.AddListener(OnSliderChange);
    }

    private void OnSliderChange(float arg0)
    {
        info.text = Show_Color.Red(user_skill.skillname)+ " 分配内力 " + Show_Color.Grey(arg0);
    }

    /// <summary>
    /// 确认分配
    /// </summary>
    private void Confirm()
    {
        user_skill.user_values[3]= ((int)slider.value).ToString();
        user_skill.user_value = ArrayHelper.Data_Encryption(user_skill.user_values);
        Game_Omphalos.i.Wirte_ResourcesList(Emun_Resources_List.skill_value, SumSave.oldcrt_skills);
        SendNotification(NotiList.Refresh_Max_Hero_Attribute);
        tool_Categoryt.Base_Task(1005);
        gameObject.SetActive(false);
        transform.parent.parent.SendMessage("acceptdata", user_skill);

    }

    public void Show(base_skill_vo skill)
    {
        user_skill= skill;
        info.text = "分配内力给技能" + skill.skillname;
        slider.value = 0;
        slider.maxValue = SumSave.crt_MaxHero_okd.internalforceMP;
    }
}
