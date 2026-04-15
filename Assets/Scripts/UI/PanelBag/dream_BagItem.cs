using System;
using Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MVC
{


    /// <summary>
    ///  用户背包数据Item
    /// </summary>
    public class dream_BagItem : Base_Mono
    {
        private Image item_icon, item_frame, lock_On;
        //private TMP_Text info;
        private Transform receive;
        private void Awake()
        {
            item_icon = Find<Image>("icon");
            item_frame = Find<Image>("frame");
            //info = Find<TMP_Text>("info/info");
            lock_On = Find<Image>("icon/lock");
            receive = Find<Transform>("receive");
            receive.gameObject.SetActive(false);
        }


        /// <summary>
        /// 使图标透明化
        /// </summary>
        public void Transparent()
        {
            if (item_icon == null) return;

            // 获取当前颜色
            Color currentColor = item_icon.color;

            // 修改Alpha值（确保在0~1范围内）
            currentColor.a = Mathf.Clamp01(0.5f);

            // 应用新的颜色
            item_icon.color = currentColor;
        }
        /// <summary>
        /// 显示已领取图标
        /// </summary>
        public void showReceive()
        {
            receive.gameObject.SetActive(true);
        }
        private Bag_Base_VO data;

        /// <summary>
        /// Data
        /// </summary>
        public Bag_Base_VO Data
        {
            set
            {
                data = value;
                if (data == null) return;
                if (item_icon == null) return;
                if (data.StdMode == equip_type_list.灵宠.ToString()) 
                {
                    return;
                }
                item_icon.sprite = UI.UI_Manager.I.GetEquipSprite("icon/", data.Name);
                if (data.user_value != null)
                {
                    string[] info_str = data.user_value.Split(' ');
                    int lv = int.Parse(info_str[2]);
                    int islock = int.Parse(info_str[3]);
                    lock_On.gameObject.SetActive(islock == 1);
                    if (lv <= 5)
                    {
                        item_frame.sprite = UI.UI_Manager.I.GetEquipSprite("UI/frame/", lv.ToString());
                        item_frame.color = Color.white;
                    }
                    else
                    {
                        item_frame.sprite = UI.UI_Manager.I.GetEquipSprite("UI/frame/", "5");
                        item_frame.color = Color.white;
                        Instantiate(Resources.Load<GameObject>("UI/frame/frame/" + lv), item_frame.transform);
                    }
                }
            }
            get
            {
                return data;
            }
        }

        private void Show_Pet()
        {
            item_frame.sprite = UI.UI_Manager.I.GetEquipSprite("UI/frame/", "5");
            item_frame.color = Color.white;
            Instantiate(Resources.Load<GameObject>("UI/frame/frame/" + 6), item_frame.transform);
            Instantiate(Resources.Load<GameObject>("UI/Prefabs/panel_pet/" + pet_data.pet_id), item_frame.transform);
            item_icon.gameObject.SetActive(false);
        }

        private db_pet_vo pet_data;

        /// <summary>
        /// Data
        /// </summary>
        public db_pet_vo Pet_Data
        {
            set
            {
                pet_data = value;
                if (Pet_Data == null) return;
                Show_Pet();
            }
            get
            {
                return pet_data;
            }
        }
    }
}