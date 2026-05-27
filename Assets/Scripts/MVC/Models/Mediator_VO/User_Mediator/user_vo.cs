using Common;
using MVC;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class user_vo : Base_VO
{

    /// <summary>
    /// 0 金币 1 元宝 2boss积分
    /// </summary>
    private List<long> list = new List<long>();
    private List<long> verify_list = new List<long>();

    private DateTime nowtime;
    /// <summary>
    /// buff数据 内容 开始时间 开始时长
    /// </summary>
    private List<(string, string, int)> buffList = new List<(string, string, int)>();
    private int index = -1;
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="value"></param>
    public void Init(DateTime time, string value, string buff_value)
    {
        nowtime = time;
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
        List<string> buff = ArrayHelper.Get_Split<string>(buff_value, (';'));
        for (int i = 0; i < buff.Count; i++)
        {
            List<string> buff_str = ArrayHelper.Get_Split<string>(buff[i], ('+'));
            if (buff_str.Count == 3)
            { 
                buffList.Add((buff_str[0], buff_str[1], int.Parse(buff_str[2])));
            }
        }
    }
    public List<long> Set()
    { 
      return list;
    }

    public List<(string, string, int)> GetBuff {  get { return buffList;} }


    /// <summary>
    /// 加入buff效果
    /// </summary>
    /// <param name="buff_name"></param>
    /// <param name="buff_time"></param>
    /// <param name="buff_time_long"></param>
    public void AddBuff(string buff_name, string buff_time, int buff_time_long)
    {
        for (int i = 0; i < buffList.Count; i++)
        {
            if (buffList[i].Item1 == buff_name)
            {
                //判断是否在有效期
                int spanSeconds = Battle_Tool.SettlementTransport(buffList[i].Item2, 3);
                int time = buffList[i].Item3 - spanSeconds;//剩余时间
                if (time > 0)
                {
                    buffList[i] = (buff_name, buffList[i].Item2, buffList[i].Item3 + buff_time_long);
                }
                else
                { 
                    //重置有效期
                    buffList[i] = (buff_name, buff_time, buff_time_long);
                }
                MysqlData();
                return;
            }
        }
        buffList.Add((buff_name, buff_time, buff_time_long));
        MysqlData();
    }
    /// <summary>
    /// 获取当前时间
    /// </summary>
    public DateTime GetTime { get { return nowtime; } }
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
    /// 获取更新数据
    /// </summary>
    /// <returns></returns>
    private string Set_buff_data()
    { 
        string dec = "";
        for (int i = 0; i < buffList.Count; i++)
        { 
            dec += buffList[i].Item1 + "+" + buffList[i].Item2 + "+" + buffList[i].Item3 + ";";
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
            if (list[i] + index != verify_list[i])
            {
                Game_Omphalos.i.Delete(_index + " 显示数据 " + list[i] + " 验证值 " + index + " " + verify_list[i]);
                return;
            }
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
        nowtime = SumSave.nowtime >= DateTime.Now ? SumSave.nowtime : DateTime.Now;
        Game_Omphalos.i.GetQueue(
                       Mysql_Type.UpdateInto, Mysql_Table_Name.Dream_Users, Set_Uptade_String(), Get_Update_Character());
        base.MysqlData();
        Game_Omphalos.Refresh(Mysql_Table_Name.Dream_Users);
    }
    public override string[] Get_Update_Character()
    {
        return new string[] {
            "nowtime",
            "buff_value",
            "value" };
    }

    public override string[] Set_Uptade_String()
    {
        return new string[] { 
            GetStr(Tool_UI.ToStandardFormat(nowtime)),
            GetStr(Set_buff_data()),
            GetStr(Set_data()),
        
        };
    }

    public override string[] Set_Instace_String()
    {
        return new string[] {
            GetStr(0),
            GetStr(SumSave.crt_user.uid),
            GetStr(Tool_UI.ToStandardFormat(nowtime)),
            GetStr(Set_buff_data()),
            GetStr(Set_data())

        };
    }
}
