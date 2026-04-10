using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class Monster :RolesManage
    {
        [Header("是否为中间怪")]
        public bool IsCentre = true;

        #region 状态
        public MonsterStateMachine stateMachine { get; private set; }//状态机
        public Monster_Move moveState { get; private set; }//移动状态
        public Monster_Idle idleState { get; private set; }//空闲状态

        public Monster_Attack attackState { get; private set; }//攻击状态
        #endregion
        protected override void Awake()
        {
            base.Awake();
            #region 状态初始化
            stateMachine = new MonsterStateMachine();
            moveState=new Monster_Move(this,stateMachine,"Move");
            idleState = new Monster_Idle(this, stateMachine, "Idle");
            attackState = new Monster_Attack(this, stateMachine, "Attack");
            #endregion
        }

        protected override void Start()
        {
            base.Start();
            stateMachine.Initialized(idleState);
        }

        protected override void Update()
        {
            base.Update();
            stateMachine.currentState.Update();
        }

        /// <summary>
        /// 数据传输
        /// </summary>
        /// <param name="attack_speed"></param>
        /// <param name="attack_distance"></param>
        /// <param name="move_speed"></param>
        /// <param name="_tatgetObg"></param>
        /// <param name="battle"></param>
        public override void Init(BattleAttack battle, BattleHealth _tatgetObg)
        {
            base.Init(battle,_tatgetObg); 

           
        }


        /// <summary>
        /// 动画状态
        /// </summary>
        public override void Animator_State(Arrow_Type arrowType)
        {
            base.Animator_State(arrowType);
            switch (arrowType)
            {
                case Arrow_Type.idle:
                    stateMachine.ChangeState(idleState);
                    break;
                case Arrow_Type.move:
                    stateMachine.ChangeState(moveState);
                    break;
                case Arrow_Type.attack:
                    stateMachine.ChangeState(attackState);
                    break;
                default:
                    break;
            }
        }

    }

}



