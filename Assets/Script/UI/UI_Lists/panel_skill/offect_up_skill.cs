using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class offect_up_skill : Base_Mono
{
    /// <summary>
    /// 功能按键
    /// </summary>
    private Transform crt_attack_skill, crt_special_skill;

    private btn_item btn_Item_parfabs;

    private oldskill_offect_item skill_item_parfabs;
    /// <summary>
    /// 当前技能
    /// </summary>
    private base_skill_vo user_skill;

    private Button close;
    
    private void Awake()
    {
        crt_attack_skill=Find<Transform>("attack_skill");
        crt_special_skill = Find<Transform>("special_skill");
        btn_Item_parfabs = Battle_Tool.Find_Prefabs<btn_item>("btn_item"); //Resources.Load<btn_item>("Prefabs/base_tool/btn_item");
        skill_item_parfabs = Battle_Tool.Find_Prefabs<oldskill_offect_item>("skill_offect_item"); //Resources.Load<skill_offect_item>("Prefabs/panel_skill/skill_offect_item");
        close = Find<Button>("close_button");
        close.onClick.AddListener(() => { On_Close(); });

    }
    /// <summary>
    /// 关闭
    /// </summary>
    private void On_Close()
    {
        gameObject.SetActive(false);
    }

    private void Init()
    {
        for (int i = crt_attack_skill.childCount - 1; i >= 0; i--)
        {
            Destroy(crt_attack_skill.GetChild(i).gameObject);
        }
        for (int i = crt_special_skill.childCount - 1; i >= 0; i--)
        {
            Destroy(crt_special_skill.GetChild(i).gameObject);
        }

        List<int> attack_numbers = new List<int>() { 1, 2, 3, 4 };
        List<int> special_numbers = new List<int>() { 1, 2 };

        for (int j = 0; j < attack_numbers.Count; j++)
        {
            bool exist = true;
            for (int i = 0; i < SumSave.oldcrt_skills.Count; i++)
            {
                if (int.Parse(SumSave.oldcrt_skills[i].user_values[2]) == attack_numbers[j])
                {
                    if ((skill_btn_list)SumSave.oldcrt_skills[i].skill_type == skill_btn_list.战斗)
                    {
                        exist = false;
                        oldskill_offect_item item = Instantiate(skill_item_parfabs, crt_attack_skill);
                        item.Data = SumSave.oldcrt_skills[i];
                        item.GetComponent<Button>().onClick.AddListener(() => { On_Click(item); });
                        continue;
                    }
                    

                }

            }
            if (exist)
            { 
                btn_item item = Instantiate(btn_Item_parfabs, crt_attack_skill);
                item.Show(attack_numbers[j], attack_numbers[j] + "战斗位");
                item.GetComponent<Button>().onClick.AddListener(() => { On_AttackClick(item); });
            }
        }

        for (int j = 0; j < special_numbers.Count; j++)
        {
            bool exist = true;
            for (int i = 0; i < SumSave.oldcrt_skills.Count; i++)
            {
                if (int.Parse(SumSave.oldcrt_skills[i].user_values[2]) == special_numbers[j])
                {
                    if ((skill_btn_list)SumSave.oldcrt_skills[i].skill_type == skill_btn_list.秘笈)
                    {
                        exist = false;
                        oldskill_offect_item item = Instantiate(skill_item_parfabs, crt_special_skill);
                        item.Data = SumSave.oldcrt_skills[i];
                        item.GetComponent<Button>().onClick.AddListener(() => { On_Click(item); });
                        continue;
                    }
                }

            }
            if (exist)
            {
                btn_item item = Instantiate(btn_Item_parfabs, crt_special_skill);
                item.Show(attack_numbers[j], attack_numbers[j] + "秘笈位");
                item.GetComponent<Button>().onClick.AddListener(() => { On_SpecialClick(item); });
            }
        }

    }

    private void On_AttackClick(btn_item item)
    {
        if ((skill_btn_list)user_skill.skill_type == skill_btn_list.战斗)
        {
            On_Click(item);
        }
        else Alert_Dec.Show("该技能只能装载战斗技能");
    }

    private void On_SpecialClick(btn_item item)
    {
        if ((skill_btn_list)user_skill.skill_type == skill_btn_list.秘笈)
        {
            On_Click(item);
        }else Alert_Dec.Show("该技能只能装载秘笈");
    }

    /// <summary>
    /// 设置特殊技能
    /// </summary>
    /// <param name="item"></param>
    private void On_Click(btn_item item)
    {
        bring_skill(item.index);
        Alert_Dec.Show("装载技能成功");
        Show(user_skill);
    }

    private void bring_skill(int index)
    {
        user_skill.user_values[2]= index.ToString();
        user_skill.user_value = ArrayHelper.Data_Encryption(user_skill.user_values);
        Game_Omphalos.i.Wirte_ResourcesList(Emun_Resources_List.skill_value, SumSave.oldcrt_skills);
        SendNotification(NotiList.Refresh_Max_Hero_Attribute);
    }
    /// <summary>
    /// 切换技能
    /// </summary>
    /// <param name="item"></param>
    private void On_Click(oldskill_offect_item item)
    {
        int pos = int.Parse(item.Data.user_values[2]);
        item.Data.user_values[2] = user_skill.user_values[2];
        item.Data.user_value = ArrayHelper.Data_Encryption(item.Data.user_values);
        bring_skill(pos);
        Alert_Dec.Show("技能切换成功");
        Show(user_skill);
    }

    public void Show(base_skill_vo skill)
    {
        Init();
        user_skill = skill;
       
    }
}
