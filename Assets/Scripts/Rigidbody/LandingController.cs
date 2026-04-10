using UnityEngine;

public class LandingController : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasLanded = false;

    private float maxFallSpeed = 500f;
    private float minFallSpeed = 400f;
    private float acceleration = 10f;

    private string currentGroundTag = "Moster";
    private bool isGrounded = false;
    private bool openstart= true;
    /// <summary>
    /// 目标
    /// </summary>
    private Transform Terget;
    /// <summary>
    /// 攻击范围
    /// </summary>
    private int battle_range;
    /// <summary>
    /// 移动速度
    /// </summary>
    private int move_speed;
    void Start()
    {
        // 获取刚体组件
        //rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        //ControlFallBasedOnHeight();
        if (openstart) Movement();
    }
    /// <summary>
    /// 移动
    /// </summary>
    private void Movement()
    {
        if(!Terget) return;
        float range = Vector3.Distance(Terget.position, transform.position);
        if (Vector3.Distance(Terget.position, transform.position) > battle_range)
        {
            transform.position = Vector2.MoveTowards(transform.position, Terget.position, move_speed * Time.deltaTime);
        }
    }

    void ControlFallBasedOnHeight()
    {
        if (hasLanded||isGrounded) return;
        Vector3 currentVelocity = rb.velocity;
        // 限制最大下落速度
        float fallSpeed = Mathf.Max(currentVelocity.y, -maxFallSpeed);
        //// 平滑调整到目标速度
        fallSpeed = Mathf.Lerp(fallSpeed, -minFallSpeed, acceleration * Time.fixedDeltaTime);
        //// 应用新速度
        rb.velocity = new Vector3(currentVelocity.x, fallSpeed, currentVelocity.z);
        // 计算下落高度
        //float fallHeight = startHeight - transform.position.y;
        //if (fallHeight > 0)
        //{
        //    Vector3 currentVelocity = rb.velocity;
        //    // 限制最大下落速度
        //    float fallSpeed = Mathf.Max(currentVelocity.y, -maxFallSpeed);
        //    //// 平滑调整到目标速度
        //    fallSpeed = Mathf.Lerp(fallSpeed, -minFallSpeed, acceleration * Time.fixedDeltaTime);
        //    //// 应用新速度
        //    rb.velocity = new Vector3(currentVelocity.x, fallSpeed, currentVelocity.z);
        //}
    } 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (openstart) return;
        if (collision.tag == "skill") return;
        if (collision.transform.position.y < transform.position.y) StopMovement();

        if (collision.tag == "Trigger Collider")
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (openstart) return;
        if (collision.tag == currentGroundTag)
        {
            if (collision.transform.position.y < transform.position.y)
            {
                hasLanded = false;
            }
        }
    }
    void StopMovement()
    {
        hasLanded = true;
        Vector3 currentVelocity = rb.velocity;
        rb.velocity = new Vector2(currentVelocity.x, 0f);
        rb.angularVelocity = 0f;
        // 可选：改为运动学刚体，完全停止物理模拟
        rb.isKinematic = true;
        //Debug.Log("物体已降落并停止运动");
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public void Init(int FallSpeed)
    {
        return;
        openstart= true;
        hasLanded = false;
        isGrounded= false;
        minFallSpeed = Random.Range(FallSpeed, FallSpeed * 1.5f);
        maxFallSpeed = minFallSpeed + 100;
        openstart = false;
    }

    public void OnCShase(Transform _Terget, int battle_range,int _move_speed)
    { 
        Terget= _Terget;
        this.battle_range = battle_range;
        move_speed = _move_speed;
        openstart = true;
    }
}