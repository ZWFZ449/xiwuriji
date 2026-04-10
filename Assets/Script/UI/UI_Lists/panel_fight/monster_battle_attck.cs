using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class monster_battle_attck : BattleAttack
{

    public override void Awake()
    {
        base.Awake();
        icon = Find<Image>("Appearance/profilePicture");

    }
    public override void OnAuto()
    {
        base.OnAuto();
        //判断技能
        BaseAttack();
    }
}
