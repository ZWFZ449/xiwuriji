using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    public class PlayerState
    {
        protected Player player;
        protected PlayerstateMachine stateMachine;
        protected string animBoolName;
        protected float startTime=0;
        public PlayerState(Player _player, PlayerstateMachine _playerStateMachine, string _animBoolName)
        {
            this.player = _player;
            this.stateMachine = _playerStateMachine;
            this.animBoolName = _animBoolName;
        }


        public virtual void Enter()//进入
        {
            //player.anim.SetBool(animBoolName, true);
            player.FlipControl(player.Direction(player.TargetPosition));
        }

        public virtual void Update()//更新
        {
           
        }
        public virtual void Exit()//退出
        {
            //player.anim.SetBool(animBoolName, false);

        }
    }

}
