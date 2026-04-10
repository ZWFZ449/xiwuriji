using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class user_vo : Base_VO
{

    /// <summary>
    /// 0 金币 1 元宝 2boss积分
    /// </summary>
    private List<long> list = new List<long>();
    private List<long> verify_list = new List<long>();


    private int index = -1;
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="value"></param>
    public void Init(string value)
    {
        index = Random.Range(1, 1000);
        string[] str = value.Split(',');
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i].Length > 0)
            {
                list.Add(long.Parse(str[i]));
                verify_list.Add(long.Parse(str[i]) + index);
            }
        }
    }
    public List<long> Set()
    { 
      return list;
    }

    private string Set_data()
    {

        string dec = "";

        for (int i = 0; i < list.Count; i++)
        {
            dec += list[i] + ",";
        }
        return dec;
    }

    /// <summary>
    /// 验证数据
    /// </summary>
    public void verify_data(currency_unit _index,long value)
    {
        for (int i = 0; i < list.Count; i++)
        {
            //原始数据未发生改变
            if (list[i] + index == verify_list[i])
            {
            }
            else Game_Omphalos.i.Delete(_index + " 显示数据 " + list[i] + " 验证值 " + index + " " + verify_list[i]);
        }

        list[(int)_index] += value;
        verify_list[(int)_index] += value;
        MysqlData();
        /*作弊检测 先注销
        switch (_index)
        {
            case currency_unit.金币:
                
                break;
            default:
                if (value >= SumSave.base_settin_uint[(int)_index - 1])
                {
                    Game_Omphalos.i.Delete("获得" + _index + value);
                }
                else
                {
                    if (value > 0) Combat_statistics.AddPoint(value);
                    list[(int)_index] += value;
                    verify_list[(int)_index] += value;
                    if (_index == currency_unit.试炼积分)
                    { 
                    Debug.Log("试炼积分"+ list[(int)_index]);
                    }
                    MysqlData();
                }
                break;
        }
        */
        
    }

    public override void MysqlData()
    {
        Game_Omphalos.i.GetQueue(
                       Mysql_Type.UpdateInto, Mysql_Table_Name.Dream_User, Set_Uptade_String(), Get_Update_Character());
        base.MysqlData();
    }
    public override string[] Get_Update_Character()
    {
        return new string[] { "value" };
    }

    public override string[] Set_Uptade_String()
    {
        return new string[] { GetStr(Set_data()) };
    }

    public override string[] Set_Instace_String()
    {
        return new string[] {
            GetStr(0),
            GetStr(SumSave.crt_user.uid),
            GetStr(Set_data())
        };
    }
}
