using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    public class MonsterStateMachine
    {
        public MonsterState currentState;//当前状态

        public void Initialized(MonsterState _monsterState)//初始化状态
        {
            currentState = _monsterState;
            currentState.Enter();
        }

        public void ChangeState(MonsterState _newState)//改变状态
        {
            currentState.Exit();
            currentState = _newState;
            currentState.Enter();

        }


    }

}
