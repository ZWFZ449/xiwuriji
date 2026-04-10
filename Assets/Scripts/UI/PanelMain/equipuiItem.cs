
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace MVC
{
    /// <summary>
    ///  装备数据
    /// </summary>
    public class equipuiItem : Base_Mono
    {
        /// <summary>
        /// 装备弹出框
        /// </summary>
        private panel_hero_equip  panel_Hero_Equip;

        private Dream_Panel_Pet panel_Pet;
        /// <summary>
        /// 预制件
        /// </summary>
        private dream_BagItem BagItemPrefabs;
        /// <summary>
        /// 装备类型
        /// </summary>
        private equip_type_list equip_type_index;
        private Image show_info;

        private dream_BagItem crt_bag;
        private void Awake()
        {
            show_info = GetComponent<Image>();
            panel_Hero_Equip = UI_Manager.I.GetPanel<panel_hero_equip>();
            panel_Pet = UI_Manager.I.GetPanel<Dream_Panel_Pet>();
            BagItemPrefabs = Tool_UI.Find_Prefabs<dream_BagItem>("dream_BagItem");
            GetComponent<Button>().onClick.AddListener(ShowEquip);
        }

        public void Insance_Crate(equip_type_list _equip_type_index)
        {
            equip_type_index= _equip_type_index;
            show_info.sprite = UI_Manager.I.GetEquipSprite("UI/panel/panelmain/equip", _equip_type_index + "");
        }
        public void Initialize()
        {
            data = null;
            crt_bag= null;
            ClearObject(this.transform);
            //for (int i = transform.childCount - 1; i >= 0; i--)
            //{
            //    Destroy(transform.GetChild(i).gameObject);
            //}
        }

        /// <summary>
        /// 显示装备信息
        /// </summary>
        private void ShowEquip()
        {
            
            if (data != null)
            {
                panel_Hero_Equip.Show();
                panel_Hero_Equip.Select_Bag(crt_bag,Panel_BagType.已装备);
            }
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
                dream_BagItem item = Instantiate(BagItemPrefabs, transform);
                item.Data = data;
                item.GetComponent<Button>().onClick.AddListener(() => { AudioManager.Instance.playAudio(ClipEnum.购买物品); ShowEquip(); });
                crt_bag = item;
            }
            get
            {
                return data;
            }
        }


        private void ShowPetEquip()
        {

            if (pet_data != null)
            {
                panel_Pet.Show();
                panel_Pet.InitPet(crt_bag, Panel_BagType.已装备);
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
                dream_BagItem item = Instantiate(BagItemPrefabs, transform);
                item.Pet_Data = Pet_Data;
                item.GetComponent<Button>().onClick.AddListener(() => { AudioManager.Instance.playAudio(ClipEnum.购买物品); ShowPetEquip(); });
                crt_bag = item;
            }
            get
            {
                return pet_data;
            }
        }
    }
}