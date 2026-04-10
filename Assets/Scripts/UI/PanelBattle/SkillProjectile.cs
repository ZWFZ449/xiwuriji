using UnityEngine;
using System.Collections;
using System;
using MVC;

public class SkillProjectile : MonoBehaviour
{
    [Header("技能基础设置")]
    private float moveSpeed = 2400f;         // 移动速度
    private float lifeTime = 10f;           // 生存时间（秒）

    [Header("目标设置")]
    public Transform target;              // 目标对象
    public Vector3 targetPosition;        // 目标位置
    public bool useTransformTarget = true; // 使用Transform目标还是位置目标

    [Header("移动方式")]
    public MoveType moveType = MoveType.Homing;
    public enum MoveType
    { 
        Straight,      // 直线移动
        Homing,        // 追踪目标
        Curve,         // 曲线移动
        Teleport       // 瞬移
    }

    [Header("曲线设置")]
    public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float curveHeight = 2f;

    [Header("碰撞设置")]
    public LayerMask collisionMask = -1;  // 碰撞层掩码
    public float collisionRadius = 0.5f;  // 碰撞检测半径
    private string[] targetTags = { "Moster", "Player" }; // 目标标签

    [Header("特效设置")]
    public GameObject hitEffect;          // 碰撞特效
    public GameObject trailEffect;        // 拖尾特效

    private Vector3 startPosition;
    private bool hasCollided = false;
    private GameObject trailInstance;
    /// <summary>
    /// 回收对象池
    /// </summary>
    private db_skill_vo PushObjectToPool_skill;

    void Start()
    {
        startPosition = transform.position;
        // 生成拖尾特效
        if (trailEffect != null)
        {
            trailInstance = Instantiate(trailEffect, transform.position, transform.rotation);
            trailInstance.transform.SetParent(transform);
        }
    }
    /// <summary>
    /// 延时回收
    /// </summary>
    /// <param name="_lifeTime"></param>
    private void On_Destroy(float _lifeTime=0)
    {
        PushObjectToPool(PushObjectToPool_skill.show_name);
    }
    /// <summary>
    /// 回收
    /// </summary>
    /// <param name="PushObjectToPoolname"></param>
    private void PushObjectToPool(string PushObjectToPoolname)
    {
        ObjectPoolManager.instance.PushObjectToPool(PushObjectToPoolname, this.gameObject);

    }

    void Update()
    {
        // 实时碰撞检测
        if (!hasCollided)
        {
            CheckCollision();
        }
        // 检查目标是否被销毁
        if (target != null && !target.gameObject.activeSelf)
        {
            On_Destroy();
        }
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
            }

            yield return null;
        }
        On_Destroy();
    }

    // 直线移动
    void MoveStraight()
    {
        Vector3 moveDirection;

        if (useTransformTarget && target != null)
        {
            moveDirection = (target.position - transform.position).normalized;
        }
        else
        {
            moveDirection = (targetPosition - transform.position).normalized;
        }
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // 旋转面向移动方向
        //if (moveDirection != Vector3.zero)
        //{
        //    Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        //}
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
        if (useTransformTarget && target != null)
        {
            transform.position = target.position;
        }
        else
        {
            transform.position = targetPosition;
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(MoveToCenterAndDestroy(target.transform));
        // 触发碰撞
        //OnCollision();
    }

    // 碰撞检测
    void CheckCollision()
    {
        return;
        //Collider[] hitColliders = Physics.OverlapSphere(transform.position, collisionRadius, collisionMask);

        //foreach (Collider hitCollider in hitColliders)
        //{
        //    // 检查标签
        //    foreach (string tag in targetTags)
        //    {
        //        if (hitCollider.CompareTag(tag))
        //        {
        //            // 应用到技能目标
        //            ApplySkillEffect(hitCollider.gameObject);
        //            // 触发碰撞
        //            OnCollision();
        //            return;
        //        }
        //    }
        //}
    }

    // 应用技能效果
    void ApplySkillEffect(GameObject targetObject)
    {

        BaseBattleAttack healthState = targetObject.GetComponent<BaseBattleAttack>();
        if (healthState != null)
        {
            //错误引用
            healthState.skill_damage(PushObjectToPool_skill); // 造成10点伤害
        }
    }
    // 碰撞处理
    void OnCollision()
    {
        StartCoroutine(DestroyGameObject());
        //if (hasCollided) return;
        //hasCollided = true;
        //// 销毁技能物体
        //StartCoroutine(DestroyGameObject());

    }
    /// <summary>
    /// 延迟回收
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(0.01f);
        //Destroy(gameObject);
        On_Destroy();
        if (hitEffect != null)
        {
            //Instantiate(hitEffect, target.position, target.rotation);
            Instantiate(hitEffect, target);
        }
        // 隐藏或销毁拖尾特效
        if (trailInstance != null)
        {
            trailInstance.transform.SetParent(null);
            //Destroy(trailInstance, 1f);
            On_Destroy();
        }
    }
    // 设置目标
    public void SetTarget(db_skill_vo skill, Transform newTarget, MoveType Straight=MoveType.Homing)
    {
        hasCollided= false;
        moveType = Straight;
        target = newTarget;
        useTransformTarget = true;
        PushObjectToPool_skill = skill;
    }

    public void SetTargetPosition(db_skill_vo skill, Vector3 position)
    {
        PushObjectToPool_skill = skill;
        targetPosition = position;
        useTransformTarget = false;
        //开始移动
        StartCoroutine(Move());

    }

    // 绘制碰撞范围（在Scene视图中）
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, collisionRadius);

        if (target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasCollided) return;
        if(target.transform != other.transform) return;
        hasCollided = true;
        cause_harm(other.gameObject);
        return;
       
    }
    /// <summary>
    /// 碰撞时造成伤害
    /// </summary>
    /// <param name="other"></param>
    private void cause_harm(GameObject other)
    {
        if (hasCollided) return;
        ApplySkillEffect(other);
        StartCoroutine(MoveToCenterAndDestroy(other.transform));
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
        yield return new WaitForSeconds(0.1f);
        OnCollision();
        
    }
}