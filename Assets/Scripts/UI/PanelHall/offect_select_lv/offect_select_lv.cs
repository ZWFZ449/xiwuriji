using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class offect_select_lv : Base_Mono
{
    private Transform m_pos;
    
    private btn_item btn_item_prefab;

    private Button confirm;

    private enum select_type
    {
        原始大陆,
        第一大陆,
        第二大陆,
        第三大陆,
    }

    private void Awake()
    {
        m_pos=Find<Transform>("Scroll View/Viewport/Content");
        btn_item_prefab = Tool_UI.Find_Prefabs<btn_item>("btn_item");
        init();
    }
    /// <summary>
    /// 初始化
    /// </summary>
    private void init()
    {
        ClearObject(m_pos);
        for (int i = 0; i < Enum.GetNames(typeof(select_type)).Length; i++)
        {
            btn_item btn_item = Instantiate(btn_item_prefab, m_pos);
            btn_item.Show(i, (select_type)i);
            btn_item.GetComponent<Button>().onClick.AddListener(() => { OnClickBtn(btn_item); });
        }
    }


    private void OnEnable()
    {
        if (SumSave.crtHero.zs_lvs <= 1)
        { 
            Alert_Dec.Show("当前转生等级不足，无法选择难度");
            Hide();
            return;
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }

    /// <summary>
    /// 选择难度
    /// </summary>
    /// <param name="btn_item"></param>
    private void OnClickBtn(btn_item item)
    {
        switch ((select_type)item.index)
        {
            case select_type.原始大陆:
                if (SumSave.map_Lv == 1)
                { 
                    Alert_Dec.Show("当前已经是原始大陆");
                    return;
                }
                Alert.Show((select_type)item.index + "", "是否返回" + select_type.原始大陆, OnConfirm, item.index);
                break;
            case select_type.第一大陆:
                if (SumSave.map_Lv == 2)
                {
                    Alert_Dec.Show("当前已经是第一大陆");
                    return;
                }
                Alert.Show((select_type)item.index + "", "是否进入" + select_type.第一大陆, OnConfirm,item.index);
                break;
            case select_type.第二大陆:
                Alert_Dec.Show("无尽塔层数不足,无法开启");
                break;
            case select_type.第三大陆:
                Alert_Dec.Show("无尽塔层数不足,无法开启");
                break;
        }
    }

    private void OnConfirm(object arg0)
    {
        SumSave.map_Lv = (int)arg0 + 1;
        UI_Manager.I.GetPanel<PanelBattle>().Close();
        Alert_Dec.Show("进入" + (select_type)arg0 + "成功");
        Alert_Dec.Show("地图已重置,请重新开启");
    }
}
