using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class Monster_Idle :Monster_Basic
    {
        public Monster_Idle(Monster _monster, MonsterStateMachine _sateMachine, string _animBoolName) : base(_monster, _sateMachine, _animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            monster.RbZero();

        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            

        }


    }
}

