using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using UI;
using MVC;

/// <summary>
/// Weighted：
/// </summary>
public class WeightedItem
{
    public object prizedraw;
    public int Weight; // 权重
    public WeightedItem(object prizedraw, int weight)
    { 
        this.prizedraw = prizedraw;
        this.Weight = weight;
    }
}

public class WeightedRandomPicker
{
    private List<WeightedItem> items;
    private List<int> cumulativeWeights; // 权重的累计和

    public WeightedRandomPicker(List<WeightedItem> items)
    {
        this.items = items;
        this.cumulativeWeights = new List<int>();

        // 计算累计权重
        int cumulativeWeight = 0;
        foreach (var item in items)
        {
            cumulativeWeight += item.Weight;
            cumulativeWeights.Add(cumulativeWeight);
        }
    }

    // 获取一个随机选择的物品
    public WeightedItem GetRandomItem()
    {
        int totalWeight = cumulativeWeights[cumulativeWeights.Count - 1]; // 最后一个值是所有物品权重的总和
        int randomValue = UnityEngine.Random.Range(0, totalWeight); // 生成一个0到总权重之间的随机数
        // 根据随机值选择物品
        for (int i = 0; i < cumulativeWeights.Count; i++)
        {
            if (randomValue < cumulativeWeights[i]) // 如果随机值小于当前累计权重，就选中该物品
            {
                return items[i];
            }
        }
        return null; // 这行理论上不会被触发，防止编译警告
    }
    /// <summary>
    /// 显示概率
    /// </summary>
    /// <returns></returns>
    public string Show_Success_rate()
    {
        Dictionary<object, int> dic = new Dictionary<object, int>();
        int totalWeight = cumulativeWeights[cumulativeWeights.Count - 1];
        foreach (var item in items)
        {
            if (!dic.ContainsKey(item.prizedraw))
            { 
                dic.Add(item.prizedraw, item.Weight);
            }
            else dic[item.prizedraw] += item.Weight;
        }
        string str = "";
        foreach (var item in dic.Keys)
        {
            str+= item + ":获取概率 " + (dic[item] * 100.0f / totalWeight).ToString("f2") + "%\n";
        }
        return str;
    }
}
