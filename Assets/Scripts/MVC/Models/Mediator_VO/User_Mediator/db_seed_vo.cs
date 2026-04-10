using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 种子数据
/// </summary>
public class db_seed_vo : Base_VO
{
    public string type;
    /// <summary>
    /// 编号
    /// </summary>
    public readonly int sequence;
    /// <summary>
    /// 材料名称
    /// </summary>
    public readonly string seed_name;
    /// <summary>
    /// 配方
    /// </summary>
    public readonly string seed_formula;
    /// <summary>
    /// 成名名称
    /// </summary>
    public readonly string pill;
    /// <summary>
    /// 合成公式
    /// </summary>
    public readonly string formula;
    /// <summary>
    /// 药品加成效果最小值最大值
    /// </summary>
    public readonly string pill_effect;
    /// <summary>
    /// 注灵需要数量
    /// </summary>
    public readonly int seed_number=10;
    /// <summary>
    /// 权重
    /// </summary>
    public readonly int Weight;
    /// <summary>
    /// 定义类型
    /// </summary>
    public readonly int rule;
    /// <summary>
    /// 累积可以吞的次数
    /// </summary>
    public readonly int limit;
    /// <summary>
    /// 对应字典编号
    /// </summary>
    public readonly int dicdictionary_index;

    public db_seed_vo(string type, int sequence, string seed_name, string seed_formula, string pill, string formula, string pill_effect, int weight, int seed_number, int rule, int dicdictionary_index, int limit)
    {
        this.type = type;
        this.sequence = sequence;
        this.seed_name = seed_name;
        this.seed_formula = seed_formula;
        this.pill = pill;
        this.formula = formula;
        this.pill_effect = pill_effect;
        Weight = weight;
        this.seed_number = seed_number;
        this.rule = rule;
        this.dicdictionary_index = dicdictionary_index;
        this.limit = limit;
    }
}
