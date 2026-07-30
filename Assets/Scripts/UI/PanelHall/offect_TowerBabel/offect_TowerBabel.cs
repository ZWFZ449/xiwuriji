using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class offect_TowerBabel : Base_Mono
{
    protected enum TowerType
    { 
        通天之路,
        通天秘笈,
        通天秘宝,
    }


    protected enum towerMap
    { 
        无尽塔,
        无限塔,
        神秘塔
    }
    private Transform m_btn_brom;

    private btn_item btn_item_prefab;

    private Image bg_offect;

    private TowerBabel_map m_towerBabel_map;

    private Button close_btn;

    private void Awake()
    {
        bg_offect= Find<Image>("offect");
        close_btn= Find<Button>("offect/close_button");
        close_btn.onClick.AddListener(() => { Close(); });
        m_btn_brom = Find<Transform>("Scroll View/Viewport/Content");
        btn_item_prefab = Tool_UI.Find_Prefabs<btn_item>("btn_item");
        m_towerBabel_map = Find<TowerBabel_map>("offect/TowerBabel_map"); 
        Init();
    }

    private void Close()
    {
        if (m_towerBabel_map.gameObject.activeSelf)
        {
            m_towerBabel_map.gameObject.SetActive(false);
        }
        bg_offect.gameObject.SetActive(false);
    }

    private void Init()
    {
        ClearObject(m_btn_brom);
        for (int i = 0; i < Enum.GetNames(typeof(TowerType)).Length; i++)
        {
            btn_item btn_item = Instantiate(btn_item_prefab, m_btn_brom);
            btn_item.Show(i, (TowerType)i);
            btn_item.GetComponent<Button>().onClick.AddListener(() => SelectBtn(btn_item));
        }
    }

    public override void Show()
    {
        base.Show();
        if (SumSave.crtHero.zs_lvs <= 1||true)
        {
            Alert_Dec.Show("1转后开启");
            Hide();
            return;
        }
    }
    private void SelectBtn(btn_item btn_item)
    {
        bg_offect.gameObject.SetActive(true);
        switch ((TowerType)btn_item.index)
        {
            case TowerType.通天之路:
                m_towerBabel_map.gameObject.SetActive(true);
                m_towerBabel_map.InitShow();
                break;
            case TowerType.通天秘笈:
                break;
            case TowerType.通天秘宝:
                break;
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }

}
