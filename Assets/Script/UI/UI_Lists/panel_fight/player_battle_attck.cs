using Common;
using MVC;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class player_battle_attck : BattleAttack
{

    /// <summary>
    /// 初始化战斗装备
    /// </summary>
    /// <param name="skills"></param>
    public override void Refresh_Skill(List<oldskill_offect_item> skills)
    {
        base.Refresh_Skill(skills);
        battle_skills = skills;
        for (int i = 0; i < skills.Count; i++)
        {
            skills[i].Battle();
        }
    }

    public override void OnAuto()
    {
        base.OnAuto();
        //判断技能
        for (int i = 0; i < battle_skills.Count; i++)
        {
            if (battle_skills[i].IsState())
            {
                int mp = (int)(battle_skills[i].Data.skill_spell * target.maxMP / 100);
                if (target.MP >= mp)
                {
                    oldskill_offect_item skill = battle_skills[i];
                    target.MP -= mp;
                    //释放技能
                    BaseAttack(battle_skills[i].Data);
                    battle_skills[i].Battle();
                    battle_skills.RemoveAt(i);
                    battle_skills.Add(skill);
                    return;
                }
            }
        }
        //player_move(开天);
        BaseAttack();

    }
    /// <summary>
    /// 释放技能
    /// </summary>
    /// <param name="data"></param>
    private void BaseAttack(base_skill_vo data)
    {
        //释放技能
        if (SumSave.crt_settingold.user_setting[5] == 0) AttackStateMachine.Skill(data);
        else skill_damage(data);
    }

}
