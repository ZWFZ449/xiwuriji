using UnityEngine;
using UnityEngine.UI; // 需要引用UI命名空间

public class ShaderHealthController : MonoBehaviour
{
    // 指向使用了我们自定义Shader材质的Image组件
    public Image circularHealthBarImage;

    // 当前血量和最大血量
    private int currentHealth = 0;
    private int maxHealth = 100;

    // 材质球的引用，用于动态修改属性
    private Material healthBarMaterial;

    private void Awake()
    {
        // 获取Image组件上的材质实例
        // 使用material属性可以确保修改的是这个对象独有的材质实例，而不是共享的原始材质
        healthBarMaterial = circularHealthBarImage.material;
        //healthBarMaterial.color= Color.gray;  
        // 初始化血量显示
        UpdateHealthBar();
    }
    /// <summary>
    /// 初始化血量
    /// </summary>
    /// <param name="_MaxHealth"></param>
    /// <param name="color"></param>
    public void Init(int _MaxHealth,Color color)
    {
        if (healthBarMaterial == null) Awake();
        healthBarMaterial.color= color;  
        maxHealth = _MaxHealth;
        currentHealth = _MaxHealth;
    }
    /// <summary>
    /// 初始化血量
    /// </summary>
    /// <param name="_MaxHealth"></param>
    public void Init(int _MaxHealth)
    {
        if (healthBarMaterial == null) Awake();
        healthBarMaterial.color = Color.red;
        maxHealth = _MaxHealth;
        currentHealth = _MaxHealth;
    }
    // 改变血量的方法
    public void ChangeHealth(float changeAmount)
    {
        Debug.Log("ChangeHealth"+changeAmount+"  "+ maxHealth);
        currentHealth = (int)Mathf.Clamp(changeAmount, 0, maxHealth);
        UpdateHealthBar();
    }

    // 更新血条显示的核心方法
    void UpdateHealthBar()
    {
        // 计算血量比例 (0到1之间)
        float healthRatio = currentHealth * 1f / maxHealth;
        // 通过Material.SetFloat方法修改Shader中的_FillAmount属性
        healthBarMaterial.SetFloat("_FillAmount", healthRatio);

        // (可选) 你也可以动态改变颜色，例如血量低时变红
         UpdateColorBasedOnHealth(healthRatio);
    }

    // (可选) 根据血量动态改变颜色
    void UpdateColorBasedOnHealth(float ratio)
    {
        Color newColor;
        if (ratio > 0.5f)
        {
            // 血量高：绿色到黄色的渐变
            newColor = Color.Lerp(Color.yellow, Color.green, (ratio - 0.5f) * 2f);
        }
        else
        {
            // 血量低：红色到黄色的渐变
            newColor = Color.Lerp(Color.red, Color.yellow, ratio * 2f);
        }
        healthBarMaterial.SetColor("_Color", newColor);
    }

    // 重要！当对象被禁用或销毁时，销毁动态创建的材质实例以避免内存泄漏
    void OnDestroy()
    {
        if (healthBarMaterial != null)
        {
            DestroyImmediate(healthBarMaterial);
        }
    }
}