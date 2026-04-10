using Common;
using MVC;
using System.Collections.Generic;

public class user_Accumulatedrewards_vo : Base_VO
{ 
    /// <summary>
    /// 1通行证累积2签到累积3充值累积奖励
    /// </summary>
    private Dictionary<int,List<int>> accumulated_rewards;
    /// <summary>
    /// 真实充值
    /// </summary>
    private int Real_recharge;
    /// <summary>
    /// 累积充值
    /// </summary>
    private int sum_recharge;
    public string user_value;

    public user_Accumulatedrewards_vo(string user_value, int real_recharge, int sum_recharge)
    {
        this.user_value = user_value;
        Real_recharge = real_recharge;
        this.sum_recharge = sum_recharge;
        Init(real_recharge, sum_recharge);
    }

    public void Init(int Realrecharge,int sumrecharge)
    {
        Real_recharge= Realrecharge;
        sum_recharge = sumrecharge;
        accumulated_rewards = new Dictionary<int, List<int>>();
        string[] str = user_value.Split('|');
        for (int i = 0; i < str.Length; i++)
        { 
            string[] str1 = str[i].Split(',');
            if (str1.Length > 1)
            {
                if (!accumulated_rewards.ContainsKey(int.Parse(str1[0])))
                {
                     List<int> list=new List<int>();
                    list.Add(0);
                    accumulated_rewards.Add(int.Parse(str1[0]), list);
                }
                string[] str2 = str1[1].Split(';');
                for (int j = 0; j < str2.Length; j++)
                { 
                    if(j==0)
                    {
                        accumulated_rewards[int.Parse(str1[0])][j]= int.Parse(str2[j]);
                    }
                    else
                    {
                        accumulated_rewards[int.Parse(str1[0])].Add(int.Parse(str2[j]));
                    }
                    
                }
            }
        }
    }
    /// <summary>
    /// 取出数据
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, List<int>> Set()
    {
        return accumulated_rewards;
    }

    /// <summary>
    /// 得到累计充值的值
    /// </summary>
    /// <returns></returns>
    public int SetSum_recharge()
    {
        return sum_recharge;
    }

    /// <summary>
    /// 取出累计充值的值 0等级 1经验 2名字
    /// </summary>
    /// <returns></returns>
    public (int, int, string) SetRecharge()
    {
        (int, int, string) vip = (0, 0, "");
        for (int i = SumSave.db_vip_list.Count; i > 0; i--)//实例化vip信息
        {
            if (sum_recharge >= SumSave.db_vip_list[i-1].vip_exp)
            {
                vip.Item1= SumSave.db_vip_list[i-1].vip_lv;
                vip.Item2 = SumSave.db_vip_list[i-1].vip_exp;
                vip.Item3 = SumSave.db_vip_list[i-1].vip_name;
               return vip;
            }
        }
        return vip;
    }

    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="index">1真实2累积</param>
    /// <returns></returns>
    public int Set(int index)
    {
        int value = 0;
        switch (index)
        {
            case 1:value= Real_recharge; break;
            case 2:value = sum_recharge; break;
            default:break;
        }
        return value;
    }
    /// <summary>
    /// 写入
    /// </summary>
    /// <param name="index">1真实2累积</param>
    /// <param name="value"></param>
    public void Set(int index,int value)
    {
        switch (index)
        {
            case 1: 
                Real_recharge += value;
                sum_recharge += value;
                break;
            case 2: sum_recharge += value; break;
            default: break;
        }
        MysqlData();
    }

    public override void MysqlData()
    {
        base.MysqlData();
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_rewards_state,
SumSave.crt_accumulatedrewards.Set_Uptade_String(), SumSave.crt_accumulatedrewards.Get_Update_Character());
    }
    public override string[] Set_Instace_String()
    {
        return new string[]
        {
            GetStr(0),
            GetStr(SumSave.crt_user.uid),
            GetStr(DataSet()),
            GetStr(Real_recharge),
            GetStr(sum_recharge)
        };
    }
    /// <summary>
    /// 写入
    /// </summary>
    /// <param name="value"></param>
    public void Set(Dictionary<int, List<int>> value)
    {
        accumulated_rewards = value;
        MysqlData();
    }

    public override string[] Get_Update_Character()
    {

        return new string[] { "Real_recharge", "sum_recharge", "accumulated_rewards" };
    }

    public override string[] Set_Uptade_String()
    {
        return new string[] 
        {
            GetStr(Real_recharge),
            GetStr(sum_recharge),
            GetStr(DataSet()) };
    }
    /// <summary>
    /// 写入数据
    /// </summary>
    /// <returns></returns>
    private string DataSet()
    {
        string value = "";
        foreach (int index in accumulated_rewards.Keys)
        {
            value += (value == "" ? "" : "|");
            value += index + ",";
            for (int i = 0; i < accumulated_rewards[index].Count; i++)
            {
                value+= (i == 0 ? "" : ";") + accumulated_rewards[index][i];
            }
        }
        return value;
    }
}
