using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class offect_blacksmith : Base_Mono
{
    private enum blacksmith_type
    { 
    鉴定装备,
    幸运祝福,
    镶嵌宝石,
    拆卸宝石
    }

    private TMP_Text info;

    private Transform m_btn_brom, m_bags_brom,m_show_brom;

    private btn_item btn_item_prefab;

    private dream_BagItem dream_BagItem_prefab;

    private Button confirm;

    private Bag_Base_VO crt_bag;

    private blacksmith_type crt_type;
    private void Awake()
    {
        m_btn_brom = Find<Transform>("btn_list/Viewport/Content");
        m_bags_brom = Find<Transform>("bag_list/Viewport/Content");
        btn_item_prefab = Tool_UI.Find_Prefabs<btn_item>("btn_item");
        dream_BagItem_prefab = Tool_UI.Find_Prefabs<dream_BagItem>("dream_BagItem");
        m_show_brom= Find<Transform>("icon");
        confirm = Find<Button>("confirm");
        info= Find<TMP_Text>("title_type_info/info");
        Init();
    }
    private void OnEnable()
    {
        if (SumSave.crtHero.lv < 30)
        {
            Alert_Dec.Show("铁匠铺功能在30级开放");
            Hide();
        }
    }

    private void Init()
    {
        ClearObject(m_btn_brom);
        for (int i = 0; i < Enum.GetNames(typeof(blacksmith_type)).Length; i++)
        {
            btn_item btn_item = Instantiate(btn_item_prefab, m_btn_brom);
            btn_item.Show(i, (blacksmith_type)i);
            btn_item.GetComponent<Button>().onClick.AddListener(() => SelectBtn(btn_item));
        }
    }
    /// <summary>
    /// 选择按钮
    /// </summary>
    /// <param name="btn_item"></param>
    private void SelectBtn(btn_item btn_item)
    {
        info.text = (blacksmith_type)btn_item.index + "";
        crt_type = (blacksmith_type)btn_item.index;
        Base_Show((blacksmith_type)btn_item.index);
    }

    private void Base_Show(blacksmith_type index)
    {
        ClearObject(m_bags_brom);
        crt_bag = null;
        ClearObject(m_show_brom);
        List<Bag_Base_VO> baglist = SumSave.crt_bags.Get_Bag_List();
        switch (index)
        {
            case blacksmith_type.鉴定装备:
                for (int i = 0; i < baglist.Count; i++)
                {
                    if (baglist[i].need_lv >= 30)
                    { 
                        dream_BagItem bagItem = Instantiate(dream_BagItem_prefab, m_bags_brom);
                        bagItem.Data = baglist[i];
                        bagItem.GetComponent<Button>().onClick.AddListener(() => SelectBagItem(bagItem));
                    }
                }
                break; 
            case blacksmith_type.幸运祝福:
                for (int i = 0; i < baglist.Count; i++)
                {
                    if (baglist[i].need_lv >= 30 && baglist[i].StdMode == equip_type_list.武器.ToString())
                    {
                        dream_BagItem bagItem = Instantiate(dream_BagItem_prefab, m_bags_brom);
                        bagItem.Data = baglist[i];
                        bagItem.GetComponent<Button>().onClick.AddListener(() => SelectBagItem(bagItem));
                    }
                }
                break;
            case blacksmith_type.镶嵌宝石:
                break;
            case blacksmith_type.拆卸宝石:
                break;
        }
    }
    /// <summary>
    /// 选择背包物品
    /// </summary>
    /// <param name="bagItem"></param>
    private void SelectBagItem(dream_BagItem bagItem)
    {

    }

    private void Hide()
    {
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }
}
