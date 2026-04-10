using System;
using System.Collections.Generic;
using UnityEngine;

public class HeightBasedFallControl : MonoBehaviour
{
    private Rigidbody rb;
    private float startHeight;

    [Header("高度相关控制")]
    public float minFallSpeed = 200f;         // 最小下落速度
    public float maxFallSpeed = 500f;        // 最大下落速度
    public float heightForMaxSpeed = 200f;   // 达到最大速度所需高度
    public float acceleration = 2f;          // 加速度

    private float detectionRadius = 150f;
    private bool hasLanded = false;
    public bool showDebugLogs = false;
    public string groundTag = "Moster";     // 地面标签
    public LayerMask groundLayer = 1; // 是否是地面
    /// <summary>
    /// 检测碰撞体
    /// </summary>
    private List<Collider> hitColliders = new List<Collider>();

    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        startHeight = transform.position.y;
    }

    void FixedUpdate()
    {
        ControlFallBasedOnHeight();
    }
    /// <summary>
    /// 检测是否存在碰撞体
    /// </summary>
    /// <returns></returns>
    bool DetectCollidersInSphere()
    {
        // 检测范围内的所有碰撞体
        Collider[] hitColliders = Physics.OverlapSphere(
            transform.position,
            detectionRadius,
            groundLayer
        );
        return !(hitColliders.Length == 1);
        // 过滤和处理检测到的碰撞体
    }
    void ControlFallBasedOnHeight()
    {
        if (hitColliders.Count > 0) return;
        //if (hasLanded)
        //{
        //    hasLanded = DetectCollidersInSphere();
        //    return;
        //}
        // 计算下落高度
        float fallHeight = startHeight - transform.position.y;
        if (fallHeight > 0)
        {
            Vector3 currentVelocity = rb.velocity;
            // 限制最大下落速度
            float fallSpeed = Mathf.Max(currentVelocity.y, -maxFallSpeed);
            // 平滑调整到目标速度
            fallSpeed = Mathf.Lerp(fallSpeed, -minFallSpeed, acceleration * Time.fixedDeltaTime);
            // 应用新速度
            rb.velocity = new Vector3(currentVelocity.x, fallSpeed, currentVelocity.z);
            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < hitColliders.Count; i++)
        {
            if (hitColliders[i] == other)
            { 
                return;
            }
        }
        hitColliders.Add(other);
        if (other.tag == "Trigger Collider")
        { 
            isKinematic();
        }
        if (hasLanded) return;
        if (other.CompareTag(groundTag))
        {
            hasLanded = true;
            StopMovement();
        }
    }
    void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit");
        //删除已经离开的碰撞体
        if (hitColliders.Contains(other))
        {
            hitColliders.Remove(other);
            rb.isKinematic = false;
        }
    }
    /// <summary>
    /// 使刚体成为运动学刚体
    /// </summary>
    private void isKinematic()
    {
        rb.isKinematic = true;
        //rb.detectCollisions = false;
    }
    void StopMovement()
    {
        // 停止物理运动
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}