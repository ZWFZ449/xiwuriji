using UnityEngine;
using System.Collections;
using Common;

namespace MVC
{
    public class base_rank_vo : Base_VO
    {
        

        /// <summary>
        /// 几区
        /// </summary>
        public int ranking_server;
        /// <summary>
        /// 排名
        /// </summary>
        public int ranking_index;

        public string Uid;

        public string uid
        {
            get { return Uid; }

            set { Uid = value; }
        }
        /// <summary>
        /// 职业
        /// </summary>
        public string rank_type;

        public string type
        {
            get { return rank_type; }

            set { rank_type = value; }
        }        /// <summary>
                 /// 名称
                 /// </summary>
        public string rank_name;

        public string name
        {
            get { return rank_name; }

            set { rank_name = value; }
        }

        /// <summary>
        /// 等级
        /// </summary>
        public int Ranking_lv;

        public int lv
        {
            get { return Ranking_lv; }

            set { Ranking_lv = value; }
        }
        /// <summary>
        /// 战力
        /// </summary>
        public int Ranking_value;

        public int value
        {
            get { return Ranking_value; }

            set { Ranking_value = value; }
        }
        
        /// <summary>
        /// 初始化排行榜
        /// </summary>
        /// <returns></returns>
        public override string[] Set_Instace_String()
        {
            return new string[]
            {
                GetStr(0),

                GetStr(SumSave.par),

                GetStr(ranking_index),

                GetStr(SumSave.crt_user.uid),

                GetStr(rank_name),

                GetStr(Ranking_lv),

                GetStr(Ranking_value)
            };
        }
    }

}
