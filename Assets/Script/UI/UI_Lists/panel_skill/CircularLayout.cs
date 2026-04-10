using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode] // 支持在编辑器模式下预览效果
public class CircularLayout : MonoBehaviour
{
    [Header("布局参数")]
    [Tooltip("圆的半径")]
    public float radius = 2f;             // 圆的半径

    [Tooltip("起始角度（度）")]
    public float startAngle = 0f;         // 起始角度（以度为单位）

    [Tooltip("是否均匀分布")]
    public bool uniformDistribution = true; // 是否自动均匀分布

    [Tooltip("子物体轴向（默认XZ平面）")]
    public Vector3 axis = new Vector3(1, 0, 0); // 控制圆形平面（如XZ或XY）

    [Header("高级设置")]
    [Tooltip("动态调整半径避免重叠")]
    public bool autoAdjustRadius = false; // 根据子物体尺寸自动调整半径

    [Tooltip("子物体尺寸（用于自动半径）")]
    public Vector2 childSize = new Vector2(1, 1); // 假设子物体的大致尺寸

    private List<Transform> children = new List<Transform>();

    void Update()
    {
        UpdateChildrenList();
        ArrangeInCircle();
    }

    // 更新子物体列表
    private void UpdateChildrenList()
    {
        children.Clear();
        foreach (Transform child in transform)
        {
            if (child != transform) // 排除自身
                children.Add(child);
        }
    }

    // 核心布局逻辑
    private void ArrangeInCircle()
    {
        if (children.Count == 0) return;

        // 自动计算半径（可选）
        if (autoAdjustRadius)
        {
            float circumference = children.Count * Mathf.Max(childSize.x, childSize.y);
            radius = circumference / (2 * Mathf.PI);
        }

        // 计算角度间隔
        float angleStep = uniformDistribution ?
            360f / children.Count :
            360f / children.Count; // 可扩展为自定义间隔

        // 遍历所有子物体
        for (int i = 0; i < children.Count; i++)
        {
            Transform child = children[i];

            // 计算角度（弧度）
            float angle = (startAngle + i * angleStep) * Mathf.Deg2Rad;

            // 计算坐标
            Vector3 position = new Vector3(
                Mathf.Cos(angle) * radius * axis.x,
                Mathf.Sin(angle) * radius * axis.y,
                Mathf.Sin(angle) * radius * axis.z
            );

            // 设置子物体位置（本地坐标，便于跟随父物体移动）
            child.localPosition = position;

            // 可选：使子物体朝向中心
            // child.LookAt(transform.position);
        }
    }

    // 编辑器辅助：添加测试子物体
    [ContextMenu("添加测试子物体")]
    private void AddTestChild()
    {
        GameObject child = GameObject.CreatePrimitive(PrimitiveType.Cube);
        child.transform.SetParent(transform);
        child.name = "Child_" + (children.Count + 1);
    }

    // 编辑器辅助：重置布局
    [ContextMenu("重置布局")]
    private void ResetLayout()
    {
        UpdateChildrenList();
        ArrangeInCircle();
    }
}