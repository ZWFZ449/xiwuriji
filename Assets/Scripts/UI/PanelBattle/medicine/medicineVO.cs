
using MVC;
/// <summary>
/// 药水数据
/// </summary>
public class medicineVO   
{
    /// <summary>
    /// 药水名称
    /// </summary>
    public readonly string medicine_name;
    /// <summary>
    /// 药水cd
    /// </summary>
    public readonly float medicine_cd;
    /// <summary>
    /// 药水效果
    /// </summary>
    public readonly medicineType medicine_type;

    public readonly Bag_Base_VO bag;
    /// <summary>
    /// 药水构造函数
    /// </summary>
    /// <param name="medicine_name">名称</param>
    /// <param name="medicine_cd">cd</param>
    /// <param name="medicine_type">类型1血2蓝3强效</param>
    public medicineVO(string medicine_name, float medicine_cd, medicineType medicine_type,Bag_Base_VO bag)
    { 
        this.medicine_name = medicine_name;
        this.medicine_cd = medicine_cd;
        this.medicine_type = medicine_type;
        this.bag = bag;
    }

}
/// <summary>
/// 药水类型
/// </summary>
public enum medicineType
{ 
    回血=1,
    回蓝,
    强效,
    随机
}
