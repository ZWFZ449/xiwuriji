using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class Skill_Collision : MonoBehaviour
    {
        private Rigidbody2D rb;
        private float MoveSpeed = 10f;//移动速度
        /// <summary>
        /// 目标
        /// </summary>
        private BattleHealth TatgetPosition;//目标位置
        private Transform Axle;//转向轴
        private base_skill_vo skill;//技能伤害
        private bool is_collider = false;//是否碰撞

        private Animator anim;//火球动画
        private skill_pos_type SkillPosType;//技能释放类型
        /// <summary>
        /// 自身造成伤害
        /// </summary>
        private BattleAttack attack;

        private bool isExplosion= false;//是否爆炸
        /// <summary>
        /// 是否回收
        /// </summary>
        private bool isPushObjectToPool = false;
        private void Awake()
        {
            rb= GetComponent<Rigidbody2D>();
            Axle = GetComponentInChildren<Transform>();
            anim= GetComponentInChildren<Animator>().transform.GetComponentInChildren<Animator>();
        }
        private void Start()
        {
           
        }
        private void Update()
        {
            if(SkillPosType == skill_pos_type.move)
            TargetMove(TatgetPosition.transform.position, MoveSpeed);
            if (isExplosion)
            {
                rb.velocity = Vector2.zero;
            }
            if (SkillPosType == skill_pos_type.situ)
            {
                is_collider = false;
            }
            else if (SkillPosType == skill_pos_type.oneself)
            {
                is_collider = false;
            }

            ///怪物死亡后回收技能
            if(TatgetPosition.HP <= 0&&SkillPosType != skill_pos_type.oneself)
            {
                PushObjectToPool();
            }

        }



        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (is_collider)
            {
                if (collision.gameObject.tag == "Moster"&& SkillPosType== skill_pos_type.move)
                {
                    if (TatgetPosition.gameObject.activeInHierarchy && TatgetPosition.HP > 0)
                    {
                        if (DamageTextManager.Instance == null)
                        {
                            Debug.LogError("DamageTextManager instance is null!");
                            return;
                        }
                        is_collider = false;
                        transform.parent.SendMessage("skill_damage", skill);
                        PushObjectToPool();
                    }

                }
            }
        }
            /// <summary>
            /// 对自己释放的技能效果播放动画
            /// </summary>
        private IEnumerator WaitForAnimationEnd()
        {
            rb.velocity = Vector2.zero;
            // AnimatorStateInfo animStateInfo = anim.GetCurrentAnimatorStateInfo(0);
            // 等待动画播放完成
            //yield return new WaitForSeconds(animStateInfo.length);
            float time = 1f;
            if(skill.skill_damage_type==4||skill.skill_damage_type == 5||skill.skill_damage_type == 8)
            {
                time = skill.skill_cd-1;
            }
            transform.parent.SendMessage("skill_damage", skill);
            yield return new WaitForSeconds(time);
            // 将对象返回对象池
            PushObjectToPool();
        }

        /// <summary>
        /// 关闭后回收自己
        /// </summary>
        private void OnDisable()
        {
            PushObjectToPool();
        }
        /// <summary>
        /// 回收引用
        /// </summary>
        private void PushObjectToPool()
        {

            if (isPushObjectToPool)
            {
                isPushObjectToPool = false;
                StopAllCoroutines();
                ObjectPoolManager.instance.PushObjectToPool(skill.skillname, this.gameObject);

            }
        }


        /// <summary>
        /// 动画播放完成销毁返回对象池 对敌人造成伤害
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitForExplosionEnd()
        {
            rb.velocity = Vector2.zero;
            AnimatorStateInfo animStateInfo = anim.GetCurrentAnimatorStateInfo(0);
            // 等待动画播放完成
            yield return new WaitForSeconds(animStateInfo.length);
            // 将对象返回对象池
            PushObjectToPool();
            attack.skill_damage(skill);

        }

        public void TargetMove(Vector2 targetPosition,float MoveSpeed)//平移
        {
            Vector2 direction = Direction(targetPosition);

            // 施加追踪力（物理驱动）
            rb.AddForce(direction * MoveSpeed, ForceMode2D.Impulse);

            // 计算所需旋转角度
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;//目标角度
            Axle.rotation = Quaternion.AngleAxis(targetAngle, Vector3.forward);
           

        }

        public Vector2 Direction( Vector2 targetPosition)//判断方向
        {
            Vector2 direction = targetPosition - rb.position;
            direction.Normalize();
            return direction;
        }
        public void SetSkillTarget(BattleHealth _tatgetObg, base_skill_vo base_skill)//找到目标
        {
            TatgetPosition = _tatgetObg;
            skill = base_skill;
        }
        /// <summary>
        /// 技能碰撞
        /// </summary>
        /// <param name="_skill">技能</param>
        /// <param name="_attack">自身</param>
        /// <param name="_target">目标</param>
        /// <param name="_skill_pos_type">技能效果</param>
        public void Init(base_skill_vo _skill,BattleAttack _attack,BattleHealth _target, skill_pos_type _skill_pos_type)//初始化
        {
            is_collider = true;
            skill = _skill;
            attack = _attack;
            TatgetPosition =_target;
            SkillPosType= _skill_pos_type;
            isPushObjectToPool = true;
            switch (_skill_pos_type)
            {
                case skill_pos_type.move:
                    break;
                case skill_pos_type.situ:
                    StartCoroutine(WaitForExplosionEnd());
                    break;
                case skill_pos_type.oneself:
                    StartCoroutine(WaitForAnimationEnd());

                    break;
                default:
                    break;
            }
            AudioManager.Instance.playAudio(ClipEnum.释放雷电术);

        }
    }
}

