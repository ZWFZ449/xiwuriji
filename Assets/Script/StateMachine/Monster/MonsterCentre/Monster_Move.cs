using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    public class Monster_Move : Monster_Basic
    {
        public Monster_Move(Monster _monster, MonsterStateMachine _sateMachine, string _animBoolName) : base(_monster, _sateMachine, _animBoolName)
        {

        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

    

        public override void Update()
        {
            base.Update();
            if(monster.isBackstab !=1)
            {
                monster.TargetMove(monster.TargetPosition);
            }else
            {
                monster.TargetMove(new Vector2(monster.TargetPosition.x - monster._BehindDistance, monster.TargetPosition.y));
            }
           



        }
       
    }


}
