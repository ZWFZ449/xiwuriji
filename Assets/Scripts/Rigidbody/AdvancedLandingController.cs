using UnityEngine;

public class AdvancedLandingController : MonoBehaviour
{
    private Rigidbody rb;
    private Collider objCollider;
    private bool hasLanded = false;

    [Header("检测设置")]
    public string groundTag = "Ground";
    public LayerMask groundLayer = 1; // Default layer

    [Header("调试选项")]
    public bool showDebugLogs = true;
    public Color debugColor = Color.green;

    void Start()
    {
        // 获取必要组件
        rb = GetComponent<Rigidbody>();
        objCollider = GetComponent<Collider>();

        // 检查组件
        if (rb == null)
        {
            Debug.LogError("需要 Rigidbody 组件！", this);
            enabled = false;
            return;
        }

        if (objCollider == null)
        {
            Debug.LogError("需要 Collider 组件！", this);
            enabled = false;
            return;
        }

        if (showDebugLogs)
            Debug.Log("降落控制器已启动", this);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("触发器检测");
        if (hasLanded) return;

        // 多种检测方式
        bool isGround = other.CompareTag(groundTag) ||
                       other.gameObject.layer == groundLayer;

        if (isGround)
        {
            if (showDebugLogs)
                Debug.Log($"检测到地面: {other.name}", this);

            StopMovement();
            hasLanded = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("碰撞检测");
        if (hasLanded) return;

        // 也检测普通碰撞
        bool isGround = collision.gameObject.CompareTag(groundTag) ||
                       collision.gameObject.layer == groundLayer;

        if (isGround)
        {
            if (showDebugLogs)
                Debug.Log($"碰撞到地面: {collision.gameObject.name}", this);

            StopMovement();
            hasLanded = true;
        }
    }

    void StopMovement()
    {
        // 停止物理运动
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 可选：完全停止物理模拟
        rb.isKinematic = true;
        rb.detectCollisions = false;

        if (showDebugLogs)
            Debug.Log("物体已停止运动", this);
    }

    // 调试绘制
    void OnDrawGizmosSelected()
    {
        if (objCollider == null)
            objCollider = GetComponent<Collider>();

        if (objCollider != null)
        {
            Gizmos.color = debugColor;
            Gizmos.DrawWireCube(transform.position, objCollider.bounds.size);
        }
    }
}