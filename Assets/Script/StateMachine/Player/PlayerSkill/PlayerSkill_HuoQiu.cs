using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class PlayerSkill_HuoQiu : PlayerSkillManager
    {
        public PlayerSkill_HuoQiu(Player _player, PlayerstateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
        {

        }
        public override void Enter()
        {
            base.Enter();

            

        }

        private void Init()
        {
           
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
 
