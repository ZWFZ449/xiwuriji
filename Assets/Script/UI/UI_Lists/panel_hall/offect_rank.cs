using UnityEngine;
using System.Collections;
using System;
using Common;

namespace MVC
{
    /// <summary>
    /// 排行榜
    /// </summary>
    public class offect_rank : Base_Mono
    {
        private Transform crt;

        /// <summary>
        /// 预设体
        /// </summary>
        private rank_item rank_itemPrefab;

        private void Awake()
        {
            crt = Find<Transform>("Scroll View/Viewport/Content");
            rank_itemPrefab= Battle_Tool.Find_Prefabs<rank_item>("rank_item"); //Resources.Load<rank_item>("Prefabs/panel_hall/rank_item");
        }

        public override void Show()
        {
            base.Show();
            SendNotification(NotiList.Read_User_Ranks);
            GetList();
        }

        private void GetList()
        {
            ClearObject(crt);
            for (int i = 0; i < SumSave.user_ranks.lists.Count; i++)
            {
                rank_item item = Instantiate(rank_itemPrefab, crt);
                item.Data = SumSave.user_ranks.lists[i];
                item.Show_index(i + 1);
            }
        }
    }

}
