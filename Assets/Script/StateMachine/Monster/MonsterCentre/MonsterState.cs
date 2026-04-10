using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class MonsterState
    {
        public Monster monster;
        public MonsterStateMachine stateMachine;
        public string animBoolName;
        protected float startTime = 0;


        public MonsterState(Monster _monster, MonsterStateMachine _sateManage, string _animBoolName)
        {
            this.monster = _monster;
            this.stateMachine = _sateManage;
            this.animBoolName = _animBoolName;
        }


        public virtual void Enter()
        {
            //monster.anim.SetBool(animBoolName, true);
           
        }

        
        public virtual void Update()
        {

        }

        public virtual void Exit()
        {
            //monster.anim.SetBool(animBoolName, false);
        }

    }
}
