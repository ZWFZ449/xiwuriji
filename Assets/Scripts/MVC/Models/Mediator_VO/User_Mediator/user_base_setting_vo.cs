using Common;
using MVC;
using System.Collections.Generic;

public class user_base_setting_vo : Base_VO
{
    /// <summary>
    /// 0回收等级 1回收品质 
    /// </summary>
    public List<int> user_setting;
    public string user_value;
    public void Init()
    {
        user_setting = new List<int>();
        string[] str = user_value.Split(' ');
        for (int i = 0; i < str.Length; i++)
        {
            if (!string.IsNullOrEmpty(str[i]))
                user_setting.Add(int.Parse(str[i]));
        }
    }
    private string Setdata()
    {
        string value="";
        for (int i = 0; i < user_setting.Count; i++)
        {
            value+= (value == ""?"":" ")+ user_setting[i].ToString();
        }
        return value;
    }
   
    public override string[] Set_Instace_String()
    {
        return 
            new string[] { 
                GetStr(0),
                GetStr(SumSave.crt_user.uid),
                GetStr(user_value),

                };
    }
    /// <summary>
    /// 读取更新
    /// </summary>
    /// <returns></returns>
    public override string[] Get_Update_Character()
    {
        return new string[] {
            "setting_value",
        };
    }
    /// <summary>
    /// 写入更新
    /// </summary>
    /// <returns></returns>
    public override string[] Set_Uptade_String()
    {
        return new string[] {
            GetStr(Setdata())
        };
    }
}
