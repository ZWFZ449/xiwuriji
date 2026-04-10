
using UnityEngine;
namespace MVC
{
    public class PlayMovementController : MonoBehaviour
    {
        
        /// <summary>
        /// 攻击对象
        /// </summary>
        public BattleHealth target;

        /// <summary>
        /// 攻击状态
        /// </summary>
        public bool Battle_state = false;

        private Rigidbody2D rb2D;

        /// <summary>
        /// 攻击距离 攻击速度
        /// </summary>
        private int battle_Attackdistance, battle_AttackSpeed, battle_move;
        /// <summary>
        /// 自身速度
        /// </summary>
        public float AttackSpeed = 0;

        public bool taunt_state = false;

        // Start is called before the first frame update
        void Start()
        {
            rb2D = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            if (target != null ) 
            {
                if (!target.gameObject.activeInHierarchy)
                {
                    target = null;

                    //GetComponent<battle_item>().Lose_Terget();
                } 
                else MoveCharacter();
            }
        }

        public void Instantiate(BattleAttack battle, bool  exist = true)
        {

            //battle_Attackdistance = (battle.Data.battle_Attackdistance + 5) * 10;

            //battle_AttackSpeed = battle.Data.battle_AttackSpeed;

            //if (exist) battle_move = battle.Data.battle_attackmove;

            //AttackSpeed = battle.Data.battle_AttackSpeed;

            //taunt_state = false;
        }

        public void anto(BattleHealth health)
        {
            target = health;
        }

        /// <summary>
        /// 战斗状态
        /// </summary>
        public void anto_State()
        {
            Battle_state = false;

            AttackSpeed = 0;
        }
        /// <summary>
        /// 击退敌人
        /// </summary>
        /// <param name="distance"></param>
        public void Battle_State(int distance)
        {
            Battle_state = false;

            Vector2 s = target.transform.position - transform.position;

            target.GetComponent<Rigidbody2D>().position += s * distance * 10 * Time.fixedDeltaTime;

            Movement(target.transform);
        }
        /// <summary>
        /// 限定边界
        /// </summary>
        /// <param name="screenPoint"></param>
        private void Movement(Transform screenPoint)
        {
            if (screenPoint.position.x > Screen.width)
            {
                screenPoint.position = new Vector2(0, screenPoint.position.y);
            }
            else
            if (screenPoint.position.x < 0)
            {
                screenPoint.position = new Vector2(Screen.width, screenPoint.position.y);
            }

            if (screenPoint.position.y > Screen.height)
            {
                screenPoint.position = new Vector2(screenPoint.position.x, 0);
            }
            else
            if (screenPoint.position.y < 0)
            {
                screenPoint.position = new Vector2(screenPoint.position.x, Screen.height);
            }

        }

        public void Battle_State(int distance,Transform monster_target,int direction = 1)
        {
            Battle_state = false;

            Vector2 s = monster_target.transform.position - transform.position;

            monster_target.GetComponent<Rigidbody2D>().position += s * direction * distance * 10 * Time.fixedDeltaTime;

            Movement(monster_target);
        }

        public void Battle_State(Transform monster_target)
        {
            monster_target.transform.position = transform.transform.position;
        }

        public void Battle_taunt_State(Transform monster_target)
        { 
            taunt_state = true; 
        }

        private void MoveCharacter()
        {
            if (Vector2.Distance(transform.position, target.transform.position) > (taunt_state ? 0: battle_Attackdistance))
            {
                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, battle_move * Time.deltaTime);
            }
            else
            {
                //if (taunt_state) transform.position = Vector2.MoveTowards(transform.position, target.transform.position, battle_move * Time.deltaTime);

                if (AttackSpeed < battle_AttackSpeed)
                {
                    AttackSpeed += 4;
                }
                else Battle_state = true;
            }
        }
    }

}

