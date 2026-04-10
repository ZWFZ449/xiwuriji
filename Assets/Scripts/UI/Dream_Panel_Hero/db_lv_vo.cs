
public class db_lv_vo
{
    /// <summary>
    /// 等级
    /// </summary>
    public readonly int lv;
    /// <summary>
    /// 所需经验
    /// </summary>
    public readonly long exp;

    public db_lv_vo(int lv, long exp)
    { 
        this.lv = lv;
        this.exp = exp;
    }
}
