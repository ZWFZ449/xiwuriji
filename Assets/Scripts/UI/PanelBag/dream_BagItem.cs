using System;
using System.Collections.Generic;
using Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MVC
{
    /// <summary>
    ///  用户背包数据Item
    /// </summary>
    public class dream_BagItem : Base_Mono
    {
        private Image item_icon, item_frame, lock_On;

        private Text info;

        private Transform receive, m_gem_brom;

        private GameObject Image_text;

        private GridLayoutGroup gridLayoutGroup;
        private void Awake()
        {
            item_icon = Find<Image>("icon");
            item_frame = Find<Image>("frame");
            info = Find<Text>("info");
            lock_On = Find<Image>("icon/lock");
            receive = Find<Transform>("receive");
            receive.gameObject.SetActive(false);
            m_gem_brom= Find<Transform>("gem_brom");
            Image_text = Resources.Load<GameObject>("Prefabs/panel_text/Image_text");
            gridLayoutGroup = Find<GridLayoutGroup>("gem_brom");
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
                    int lucky = int.Parse(info_str[1]);
                    if (lucky > 1 && (data.StdMode == equip_type_list.武器.ToString() || data.StdMode == equip_type_list.项链.ToString()))
                    {
                        info.text = (lucky - 1).ToString();
                    }
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
                        if (info_str.Length >= 6)
                        {
                            //RectTransform rectTransform = GetComponent<RectTransform>();
                            //float width = rectTransform.rect.width;
                            //Debug.Log("width:" + width);
                            //gridLayoutGroup.cellSize = new Vector2(width / 3, width / 3);
                            List<string> gem = ArrayHelper.Get_Split<string>(info_str[5], 'X');
                            for (int i = 0; i < gem.Count; i++)
                            {
                                if (gem[i] != "")
                                {
                                    List<string> gem_value = ArrayHelper.Get_Split<string>(gem[i], '|');
                                    if (gem_value.Count == 2)
                                    {
                                        if (gem_value[1] != "0")
                                        {
                                            Bag_Base_VO gem_data = ArrayHelper.Find(SumSave.db_stditems, x => x.Name == gem_value[1]);
                                            if (gem_data != null)
                                            {
                                                Instantiate(Image_text, m_gem_brom).GetComponent<Image>().sprite = UI.UI_Manager.I.GetEquipSprite("icon/", gem_data.Name);
                                            }
                                        }
                                        else
                                        {
                                            Bag_Base_VO gem_data = ArrayHelper.Find(SumSave.db_stditems, x => x.StdMode == "材料" && x.Shape == int.Parse(gem_value[0]));
                                            GameObject go =Instantiate(Image_text, m_gem_brom);
                                            go.GetComponent<Image>().sprite = UI.UI_Manager.I.GetEquipSprite("icon/", gem_data.Name);
                                            go.GetComponent<Image>().color = Color.gray;
                                        }
                                    }
                                }
                            }
                        }
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
            if (pet_data.crt_name != pet_data.pet_name)
            {
                item_icon.gameObject.SetActive(true);
                Color color = item_icon.color;
                color.a = 0f;  // 透明度设为0（完全透明）
                item_icon.color = color;
                lock_On.gameObject.SetActive(true);
            }
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