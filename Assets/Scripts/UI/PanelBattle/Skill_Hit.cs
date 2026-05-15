using GifImporter;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MVC
{
    public class Skill_Hit : MonoBehaviour
    {
        private GameObject hitEffect;          // 碰撞特效
        private GameObject trailEffect;        // 拖尾特效
        /// <summary>
        /// 目标
        /// </summary>
        public Transform target;
        /// <summary>
        /// 是谁释放的技能
        /// </summary>
        private BaseBattleAttack baseBattleAttack;
        /// <summary>
        /// 自身位置
        /// </summary>
        private Vector3 startPosition;
        /// <summary>
        /// 拖尾预制体
        /// </summary>
        private GameObject trailInstance;
        /// <summary>
        /// 是否已经发生碰撞伤害
        /// </summary>
        private bool hasCollided = true;
        /// <summary>
        /// 技能移动速度
        /// </summary>
        private int moveSpeed = 2500;
        /// <summary>
        /// 回收对象池
        /// </summary>
        private db_skill_vo PushObjectToPool_skill;
        /// <summary>
        /// 移动类型
        /// </summary>
        private MoveType moveType = MoveType.Homing;

        private Button gif;

        public enum MoveType
        {
            Straight,      // 直线移动
            Homing,        // 追踪目标
            Curve,         // 曲线移动
            Teleport,       // 瞬移
            oneselfTeleport //自身释放
        }
        // uit-8框架
        void Start()
        {
            gif=GetComponent<Button>();
            gif.onClick.AddListener(() => {GifPlay(); });
            startPosition = transform.position;
            // 生成拖尾特效
            if (trailEffect != null)
            {
                trailInstance = Instantiate(trailEffect, transform.position, transform.rotation);
                trailInstance.transform.SetParent(transform);
            }
        }
        /// <summary>
        /// 按下功能
        /// </summary>
        private void GifPlay()
        {
            cause_harm();
        }

        private void On_Destroy(float _lifeTime = 0)
        {
            PushObjectToPool(PushObjectToPool_skill.show_name);
        }
        /// <summary>
        /// 回收
        /// </summary>
        /// <param name="PushObjectToPoolname"></param>
        private void PushObjectToPool(string PushObjectToPoolname)
        {
            //回归对象池
            if (baseBattleAttack.gameObject.activeInHierarchy)
                this.transform.position = new Vector2(baseBattleAttack.transform.position.x, baseBattleAttack.transform.position.y);
            ObjectPoolManager.instance.PushObjectToPool(PushObjectToPoolname, this.gameObject);

        }
        /// <summary>
        /// 接受目标
        /// </summary>
        /// <param name="skill"></param>
        /// <param name="position"></param>
        public void SetTargetPosition(BaseBattleAttack _baseBattleAttack, db_skill_vo skill, Transform position)
        {
            PushObjectToPool_skill = skill;
            target = position;
            baseBattleAttack = _baseBattleAttack;
            //moveType = MoveType.Straight;
            moveType = (MoveType)(skill.MoveType);
            hasCollided = false;
            switch (moveType)
            {
                case MoveType.Straight:
                    MoveStraight();
                    break;

                case MoveType.Homing:
                    MoveHoming();
                    break;

                case MoveType.Curve:
                    MoveCurve();
                    break;

                case MoveType.Teleport:
                    transform.position = new Vector3(target.position.x + PushObjectToPool_skill.offset[0], target.position.y + PushObjectToPool_skill.offset[1], target.position.z);
                    break;
                    //yield return StartCoroutine(MoveTeleport());
                    //yield break; // 结束协程
                case MoveType.oneselfTeleport:
                    transform.position = baseBattleAttack.transform.position;
                    break;
            }

            //开始移动
            //StartCoroutine(Move());

        }
        IEnumerator Move()
        {
            while (!hasCollided && target != null)
            {
                switch (moveType)
                {
                    case MoveType.Straight:
                        MoveStraight();
                        break;

                    case MoveType.Homing:
                        MoveHoming();
                        break;

                    case MoveType.Curve:
                        MoveCurve();
                        break;

                    case MoveType.Teleport:
                        yield return StartCoroutine(MoveTeleport());
                        yield break; // 结束协程
                    case MoveType.oneselfTeleport:
                        transform.position= baseBattleAttack.transform.position;
                        break;
                }
                judgment();
                yield return null;
            }
        }

        /// <summary>
        /// 直线攻击
        /// </summary>
        void MoveStraight()
        {
            Vector3 moveDirection;
            if (target != null)
            {
                moveDirection = (target.position - transform.position).normalized;
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
                // 旋转面向移动方向
                Vector2 direction = target.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                if (Vector3.Distance(transform.position, target.position) <= 15f)
                {
                    cause_harm();
                    On_Destroy();
                }
            }
        }

        // 追踪移动
        void MoveHoming()
        {
            if (target == null) return;

            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // 平滑旋转面向目标
            //Quaternion targetRotation = Quaternion.LookRotation(direction);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        [Header("曲线设置")]
        private AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        private float curveHeight = 2f;
        // 曲线移动
        void MoveCurve()
        {
            if (target == null) return;

            float journeyLength = Vector3.Distance(startPosition, target.position);
            float distanceCovered = Vector3.Distance(startPosition, transform.position);
            float journeyFraction = distanceCovered / journeyLength;

            // 计算曲线高度
            float height = curve.Evaluate(journeyFraction) * curveHeight;

            // 计算当前位置
            Vector3 currentPos = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            currentPos.y += height;
            transform.position = currentPos;
        }

        // 瞬移移动
        IEnumerator MoveTeleport()
        {
            // 先显示准备特效
            if (trailInstance != null)
            {
                trailInstance.SetActive(true);
            }
            // 瞬移到目标位置
            if (target != null)
            {
                transform.position = new Vector3(target.position.x + PushObjectToPool_skill.offset[0], target.position.y + PushObjectToPool_skill.offset[1], target.position.z);
            }
            yield return new WaitForSeconds(0.1f);
        }

        private void cause_harm()
        {
            if (hasCollided) return;
            BaseBattleAttack healthState = target.GetComponent<BaseBattleAttack>();
            if (healthState != null)
            {
                baseBattleAttack.skill_damage(PushObjectToPool_skill); // 造成10点伤害
            }
            //StartCoroutine(MoveToCenterAndDestroy(target.transform));
            hasCollided = true;
            On_Destroy();
        }
        IEnumerator MoveToCenterAndDestroy(Transform playerTransform)
        {
            hasCollided = true;
            // 如果未指定目标，则默认移动到玩家中心
            if (target == null)
                target = playerTransform;

            // 移动直到非常接近目标点
            while (Vector3.Distance(transform.position, target.position) > 0.05f)
            {
                // 平滑地朝目标位置移动
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                yield return null; // 等待下一帧
            }
            // 可选：移动到后等待一瞬间
            yield return new WaitForSeconds(0.02f);

        }
        // Update is called once per frame
        void Update()
        {
            switch (moveType)
            {
                case MoveType.Straight:
                    MoveStraight();
                    break;

                case MoveType.Homing:
                    MoveHoming();
                    break;

                case MoveType.Curve:
                    MoveCurve();
                    break;
            }
            if (target == null)
            {
                if (!hasCollided) On_Destroy();
            }
            else
            {
                if (!target.gameObject.activeSelf)
                {
                    On_Destroy();
                }
            }
        }
        /// <summary>
        /// 判断是否碰撞
        /// </summary>
        private void judgment()
        {
            if (Vector3.Distance(transform.position, target.position) <= 15f)
            {
                if (!hasCollided) cause_harm();
            }
        }
    }

}
