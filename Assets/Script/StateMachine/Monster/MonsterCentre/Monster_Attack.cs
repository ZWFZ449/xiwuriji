using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    public class Monster_Attack : MonsterState
    {
        public Monster_Attack(Monster _monster, MonsterStateMachine _sateManage, string _animBoolName) : base(_monster, _sateManage, _animBoolName)
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
        }
    }
}

