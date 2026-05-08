using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class panel_hero_equip : PanelBase
{
    /// <summary>
    /// 英雄装备面板
    /// </summary>
    private Transform m_pos_brom;
    /// <summary>
    /// 英雄装备面板
    /// </summary>
    private GridLayoutGroup  m_grid_hero_equip;
    /// <summary>
    /// 英雄装备显示列表
    /// </summary>
    private hero_equip_item hero_equip_item_prefab;

    private hero_Resources_item hero_Resources_item_prefab; 

    private Dream_Panel_Bag dream_Panel_Bag;

    private PanelMian panelMian;
    public override void Hide()
    {
        base.Hide();
    }

    public override void Initialize()
    {
        base.Initialize();
        panelMian = UI_Manager.I.GetPanel<PanelMian>();
        dream_Panel_Bag = UI_Manager.I.GetPanel<Dream_Panel_Bag>();
        hero_equip_item_prefab = Tool_UI.Find_Prefabs<hero_equip_item>("hero_equip_item");
        hero_Resources_item_prefab = Tool_UI.Find_Prefabs<hero_Resources_item>("hero_Resources_item");
        m_pos_brom = Find<Transform>("Scroll View/Viewport/Content");
        m_grid_hero_equip = m_pos_brom.GetComponent<GridLayoutGroup>();
    }
    protected void Refresh()
    {
        panelMian.Show();
        dream_Panel_Bag.Show();
        Hide();
    }
    protected void RefreshMain()
    {
        panelMian.Show();
        Hide();
    }
    public override void Show()
    {
        base.Show();
        ClearObject(m_pos_brom,0);
    }

    public void Select_Bag(dream_BagItem item,Panel_BagType bagType)
    {
        m_grid_hero_equip.cellSize= new Vector2(540,1228);
        Instantiate(hero_equip_item_prefab, m_pos_brom).Init(item,bagType); 
        if (bagType != Panel_BagType.已装备)
        Equip_Compare(item);//装备对比
    }
    /// <summary>
    /// 装备对比
    /// </summary>
    /// <param name="item"></param>
    /// <param name="bagType"></param>
    private void Equip_Compare(dream_BagItem item)
    {
        if (SumSave.crt_setting.user_data_settings.Count >= 4 && SumSave.crt_setting.user_data_settings[3] == 0) return; 
        List<Bag_Base_VO> equips = SumSave.crt_equips.Get(Dream_User_Equip_Type.装备);
        for (int i = 0; i < equips.Count; i++)
        {
            if (equips[i].StdMode == item.Data.StdMode)
            {
                dream_BagItem bagItem = new dream_BagItem();
                bagItem.Data = equips[i];
                Instantiate(hero_equip_item_prefab, m_pos_brom).Init(bagItem, Panel_BagType.展示);
            }
        }
    }

    public void Select_Resources(material_item item, Panel_BagType bagType)
    {
        m_grid_hero_equip.cellSize = new Vector2(1060, 840);
        Instantiate(hero_Resources_item_prefab, m_pos_brom).Init(item.GetItemData(), bagType);
    }
    public void Select_Resources(dream_BagItem item, Panel_BagType bagType)
    {
        m_grid_hero_equip.cellSize = new Vector2(1060, 840);
        Instantiate(hero_Resources_item_prefab, m_pos_brom).Init(item.Data, bagType);
    }

    protected override void Awake()
    {
        base.Awake();
    }
}
