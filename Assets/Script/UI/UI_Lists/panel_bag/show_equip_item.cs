using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class show_equip_item : Base_Mono
{
    public EquipTypeList type;
    /// <summary>
    /// 展示类型
    /// </summary>
    private Image show_type;
    /// <summary>
    /// 预制件
    /// </summary>
    private bag_item BagItemPrefabs;

    private void Awake()
    {
        //transform.parent.parent.parent.parent.SendMessage("Instance_Pos", this);
        BagItemPrefabs = Battle_Tool.Find_Prefabs<bag_item>("bag_item");// Resources.Load<bag_item>("Prefabs/panel_bag/bag_item");
        show_type = GetComponent<Image>();
    }
    private void Start()
    {
        show_type.sprite= Resources.Load<Sprite>("panel_bag/equip_type/" + type);
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        data = null;
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
    /// <summary>
    /// 显示装备信息
    /// </summary>
    private void ShowEquip(bag_item item)
    {
        transform.parent.parent.parent.parent.SendMessage("Select_Equip", item);
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

            bag_item item = Instantiate(BagItemPrefabs, transform);
            item.Data = data;
            item.GetComponent<Button>().onClick.AddListener(() => { ShowEquip(item); });
            

        }
        get
        {
            return data;
        }
    }
}
