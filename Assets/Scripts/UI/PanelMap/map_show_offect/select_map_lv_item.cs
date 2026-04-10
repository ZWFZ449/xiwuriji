using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MVC
{
    public class select_map_lv_item : Base_Mono
    {
        /// <summary>
        /// 地图图标
        /// </summary>
        private Image icon;
        /// <summary>
        /// 地图强度
        /// </summary>
        private int map_intensity;

        private void Awake()
        {
            icon = GetComponent<Image>();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="value">icon</param>
        /// <param name="lv">强度</param>
        public void Init(string value,int lv)
        {
            icon.sprite = UI.UI_Manager.I.GetEquipSprite("monster/", value);
            map_intensity = lv;
        }
        /// <summary>
        /// 获取地图强度
        /// </summary>
        public int GetMapIntensity { get { return map_intensity; } }
        /// <summary>
        /// 是否被选中
        /// </summary>
        public bool Selected { set { GetComponent<Image>().color = value?Color.red:Color.white; } }
    }

}
