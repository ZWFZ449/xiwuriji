using Common;
using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace StateMachine
{
    public class Player : RolesManage
    {
        #region 状态
        public PlayerstateMachine stateMachine { get; private set; }//状态机
        public Player_Idle idleState { get; private set; }//空闲状态
        public Player_Move moveState { get; private set; }//移动状态
        public Player_Attack attackState { get; private set; }//攻击状态
        public PlayerSkillManager skillManagerState { get; private set; }//技能管理器
        public PlayerSkill_HuoQiu skillHuoQiuState { get; private set; }//火球技能

        #endregion

        #region 技能
        [SerializeField] public Transform skillStoragePos;//技能储存位置
        public GameObject skills_HuoQiu;//火球技能
        public float skill_damage = 123f;//火球伤害
        public float skill_release_pro = 30f;//技能释放概率

        #endregion

      
        protected override void Awake()
        {
            base.Awake();
            #region 状态初始化
            stateMachine = new PlayerstateMachine();
            idleState = new Player_Idle(this, stateMachine, "Idle");
            moveState = new Player_Move(this, stateMachine, "Move");
            attackState = new Player_Attack(this, stateMachine, "Attack");
            skillManagerState = new PlayerSkillManager(this, stateMachine, "Skill");
            skillHuoQiuState = new PlayerSkill_HuoQiu(this, stateMachine, "Skill");
            #endregion

            #region 技能初始化
            skills_HuoQiu = Resources.Load<GameObject>("Prefabs/panel_skill/Skill_Effects/HuoQiu");
            skillStoragePos=GameObject.Find("Skills").transform;
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
        public override void Init( BattleAttack battle, BattleHealth _tatgetObg)
        {
            base.Init( battle, _tatgetObg);
            AttackDistance = SumSave.crt_MaxHero_okd.attack_distance;
        }


    }
}

