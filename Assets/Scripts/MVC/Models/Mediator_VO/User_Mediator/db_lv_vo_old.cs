using Common;
using MVC;
using System.Collections.Generic;

public class db_lv_vo_old : Base_VO
{
    /// <summary>
    /// 玩家等级需求经验
    /// </summary>
    public List<long> hero_lv_list;
    /// <summary>
    /// 小世界升级需求
    /// </summary>
    public Dictionary<int, List<(string, int)> > world_lv_list_dic = new Dictionary<int, List<(string, int)>>();
    /// <summary>
    /// 小世界升级效果
    /// </summary>
    public List<int> world_offect_list ;
    /// <summary>
    /// 小世界最大存储值 更具小世界等级变化
    /// </summary>
    public List<int> word_lv_max_value;
}
