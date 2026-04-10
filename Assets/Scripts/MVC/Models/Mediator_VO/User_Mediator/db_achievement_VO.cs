using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class db_achievement_VO : Base_VO
{
    /// <summary>
    /// 成就类型
    /// </summary>
    public readonly int achievement_type;
    /// <summary>
    /// 成就名称
    /// </summary>
    public readonly string achievement_value;
    /// <summary>
    /// 成就达成条件
    /// </summary>
    public readonly string achievement_need;
    /// <summary>
    /// 成就达成条件列表
    /// </summary>
    public List<long> achievement_needs = new List<long>();
    /// <summary>
    /// 奖励类型
    /// 
    /// 0物品 1属性
   
    /// </summary>
    public readonly string achievement_reward;
    /// <summary>
    /// 收益列表
    /// </summary>
    public List<string> achievement_rewards = new List<string>();
    /// <summary>
    /// 显示等级介绍
    /// </summary>
    public string[] achievement_show_lv;
    /// <summary>
    /// 是否有兑换列表
    /// </summary>
    public readonly string achievement_exchange_offect = "";

    public db_achievement_VO(int achievement_type, string achievement_value, string achievement_need, string[] achievement_show_lv, string achievement_reward, string achievement_exchange_offect)
    {
        this.achievement_type = achievement_type;
        this.achievement_value = achievement_value;
        this.achievement_need = achievement_need;
        this.achievement_show_lv = achievement_show_lv;
        this.achievement_reward = achievement_reward;
        this.achievement_exchange_offect = achievement_exchange_offect;
        Init();
    }

    public void Init()
    {
        string[] split = achievement_need.Split('|');
        foreach (string value in split)
        {
            achievement_needs.Add(long.Parse(value));
        }
        split = achievement_reward.Split('|');
        foreach (string value in split)
        {
            achievement_rewards.Add(value);
        }
    }
}
