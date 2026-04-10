using Common;
using MVC;
public class user_setting_type_vo : Base_VO
{
    /// <summary>
    /// 设置id
    /// </summary>
    public int id_setting;

    /// <summary>
    /// 设置类型
    /// </summary>
    /// <returns></returns>
    public string type_setting;
    /// <summary>
    /// 设置选项
    /// </summary>
    /// <returns></returns>
    public string option_setting;
 

    public override string[] Set_Instace_String()
    {
        return
            new string[] {
                GetStr(0),
                GetStr(type_setting),
                GetStr(option_setting),

                };
    }
    /// <summary>
    /// 读取更新
    /// </summary>
    /// <returns></returns>
    public override string[] Get_Update_Character()
    {
        return new string[] {
            "type_setting",
            "option_setting",
        };
    }
    /// <summary>
    /// 写入更新
    /// </summary>
    /// <returns></returns>
    public override string[] Set_Uptade_String()
    {
        return new string[] {
           GetStr(type_setting),
           GetStr(option_setting)
        };
    }
}
