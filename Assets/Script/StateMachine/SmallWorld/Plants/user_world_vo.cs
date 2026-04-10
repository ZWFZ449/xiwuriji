using Common;
using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class user_world_vo : Base_VO
{
    /// <summary>
    /// 等级
    /// </summary>
    public int World_Lv=1;
    /// <summary>
    /// 0 最后刷新时间 1灵气值
    /// </summary>
    private List<string> value_lists = new List<string>();
    public string user_value;

    public void Init()
    {
        string[] spilts = user_value.Split('&');
        if (spilts.Length > 1)
        {
            for (int i = 0; i < spilts.Length; i++)
            {
                value_lists.Add(spilts[i]);
            }
        }
    }
    /// <summary>
    /// 获取值
    /// </summary>
    /// <returns></returns>
    public List<string> Get()
    { 
     return value_lists;
    }
    /// <summary>
    /// 获取值
    /// </summary>
    /// <param name="value"></param>
    public void Set(int value,bool exist = true)
    {
        if(exist) value_lists[0] = SumSave.nowtime.ToString();
        //value_lists[1] =(int.Parse(value_lists[1])+value).ToString();
        value_lists[1]= value.ToString();
        VerifyMaximum();
    }

    /// <summary>
    /// 增加灵气值
    /// </summary>
    /// <param name="value"></param>
    public void AddValue_lists(int value, bool exist = true)
    {
        //Debug.LogError("溯源");
        if (exist) value_lists[0] = SumSave.nowtime.ToString();
        value_lists[1] = (int.Parse(value_lists[1]) + value).ToString();
        VerifyMaximum();
    }

    /// <summary>
    /// 验证灵气是否为最大值
    /// </summary>
    private void VerifyMaximum()
    {
        int max = SumSave.db_lvs_old.word_lv_max_value[SumSave.crt_world.World_Lv] + Tool_State.Value_playerprobabilit(enum_skill_attribute_list.灵气上限);
        if (int.Parse(value_lists[1]) >= max)
        {
            value_lists[1] = max.ToString();
        }
    }

    public string Set_data()
    {
        string dec = "";
        for (int i = 0; i < value_lists.Count; i++)
        {
            if(i!=0)
            {
                dec += "&";
            }
            dec += value_lists[i] ;
        }
        return dec;
    }
    /// <summary>
    /// 初始化
    /// </summary>
    /// <returns></returns>
    private string InitValue()
    {
        user_value = SumSave.nowtime + "&" + "0";
        Init();
        return user_value;
    }
    /// <summary>
    /// 初始化
    /// </summary>
    /// <returns></returns>
    public override string[] Set_Instace_String()
    {
        return new string[]
        {
            GetStr(0),
            GetStr(SumSave.crt_user.uid),
            GetStr(World_Lv),
            GetStr(InitValue())
        };
    }


    public override string[] Get_Update_Character()
    {
        return new string[]
        {
            "World_Lv",
            "user_value"
        };
    }


    public override string[] Set_Uptade_String()
    {
        return new string[]
        {
            GetStr(World_Lv),
            GetStr(Set_data()),

        };
    }


}
