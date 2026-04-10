using UnityEngine;
using UnityEngine.UI;

public class CircularHealthBar : MonoBehaviour
{
    private Image healthBarImage; // 指向前景填充Image

    [Header("Health Settings")]
    private float maxHealth = 100000f;
    private float currentHealth = 100f;

    [Header("Animation Settings")]
    private float smoothTime = 0.3f; // 平滑变化的时间
    private float smoothVelocity = 0f; // 平滑变化的速率

    void Start()
    {
        // 如果没手动赋值，尝试获取子物体上的Image组件
        healthBarImage = GetComponent<Image>();
        UpdateHealthBar();
    }

    void Update()
    {
        // 平滑更新血条（可选）
        //float targetFill = currentHealth / maxHealth;
        //float currentFill = healthBarImage.fillAmount;
        //healthBarImage.fillAmount = Mathf.SmoothDamp(currentFill, targetFill, ref smoothVelocity, smoothTime);
        healthBarImage.fillAmount = currentHealth / maxHealth;
    }
    /// <summary>
    /// 初始化血条，设置最大血量
    /// </summary>
    /// <param name="_maxHealth"></param>
    public void Init(float _maxHealth)
    {
        maxHealth = _maxHealth;
        currentHealth = _maxHealth;
    }
    // 公共方法，用于改变血量
    public void ChangeHealth(float amount)
    {
        currentHealth = Mathf.Clamp(amount, 0f, maxHealth);
        // 如果不需要平滑效果，可以直接设置：
         healthBarImage.fillAmount = currentHealth / maxHealth;
    }
    /// <summary>
    /// 改变经验值
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeExp(float amount)
    {
        if(currentHealth<=0)currentHealth = maxHealth;
        currentHealth = Mathf.Clamp(currentHealth-amount, 0f, maxHealth);
        // 如果不需要平滑效果，可以直接设置：
        healthBarImage.fillAmount = currentHealth / maxHealth;
    }

    // 直接设置血量百分比
    public void UpdateHealthBar()
    {
        if (healthBarImage != null)
            healthBarImage.fillAmount = currentHealth / maxHealth;
    }
}