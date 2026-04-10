using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterController : BaseBattleAttack 
{
    public override void OnAuto()
    {
        base.OnAuto();
        if (Terget == null) Find_Terget();
        if (Terget == null) return;
        BaseAttack();
    }
}
