using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    public class PlayerstateMachine
    {
        public PlayerState currentState;//当前状态

        public void Initialized(PlayerState _playerState)//初始化状态
        {
            currentState = _playerState;
            currentState.Enter();
        }

        public void ChangeState(PlayerState _newState)//改变状态
        {
            currentState.Exit();
            currentState = _newState;
            currentState.Enter();

        }

    }

}
