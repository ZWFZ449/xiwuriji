
using Common;

namespace MVC
{
    /// <summary>
    /// GreenhandGuide_TotalTaskVO：任务的全部信息
    /// </summary>
    public class GreenhandGuide_TotalTaskVO : Base_VO
    {

        public int id;
        public string tableName = "totalgreentask";
        /// <summary>
        /// 任务描述
        /// </summary>
        public string TaskDesc;
        /// <summary>
        /// 奖励
        /// </summary>
        public string[] Award;
        /// <summary>
        /// 奖励数量
        /// </summary>
        public int[] AwardNumber;
        /// <summary>
        /// 任务类型
        /// </summary>
        public GreenhandGuideTaskType tasktype;

        /// <summary>
        /// 任务奖励标题
        /// </summary>
        public string task_dec_type;

        /// <summary>
        /// 任务奖励详情
        /// </summary>
        public string task_dec_value;
        /// <summary>
        /// 奖励类型
        /// </summary>
        public string[] AwardType;

        /// <summary>
        /// 该任务所需要的进度点
        /// </summary>
        public int progress;

        /// <summary>
        /// 任务的序号
        /// </summary>
        public int taskorder;

        /// <summary>
        /// 任务id
        /// </summary>
        public int taskid;
    }

}
