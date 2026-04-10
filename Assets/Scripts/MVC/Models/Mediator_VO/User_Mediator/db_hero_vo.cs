using MVC;
public class db_hero_vo : Base_VO
{
   /// <summary>
   /// 职业名称
   /// </summary>
    public string hero_name;

    public int hero_type;
    /// <summary>
    /// 职业初始化值
    /// </summary>
    public int[] crate_value;
    /// <summary>
    /// 职业升级标准
    /// </summary>
    public int[] up_base_value;
    /// <summary>
    /// 职业升级系数
    /// </summary>
    public int[] up_value;
}
