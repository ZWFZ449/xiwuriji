using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Hero_Resources_BtnType
{ 
    出售,
}
public class hero_Resources_item : Base_Mono
{
    private TMP_Text info;

    private Transform m_btn_brom,m_showIcon_brom;

    private material_item material_item_prefab;

    private btn_item btn_item_prefab;

    private (string, int) data;

    private Dictionary<Hero_Resources_BtnType, btn_item> btn_item_dic = new Dictionary<Hero_Resources_BtnType, btn_item>();
    private void Awake()
    {
        info=Find<TMP_Text>("info/info");
        m_btn_brom = Find<Transform>("btn_brom");  
        m_showIcon_brom= Find<Transform>("icon");  
        material_item_prefab = Tool_UI.Find_Prefabs<material_item>("material_item");
        btn_item_prefab = Tool_UI.Find_Prefabs<btn_item>("btn_item");
        for (int i = 0; i < Enum.GetNames(typeof(Hero_Resources_BtnType)).Length; i++)
        {
            btn_item btn = Instantiate(btn_item_prefab, m_btn_brom);
            btn.Show(i, (Hero_Resources_BtnType)i);
            btn.GetComponent<Button>().onClick.AddListener(() => { OnClick(btn); });
            btn_item_dic.Add((Hero_Resources_BtnType)i, btn);
        }
    }

    private void OnClick(btn_item btn)
    {
        switch ((Hero_Resources_BtnType)(btn.index))
        {
            case Hero_Resources_BtnType.出售:
                sell();
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 出售
    /// </summary>
    private void sell()
    {
        if (data.Item2 > 0)
        {
            Bag_Base_VO bag = ArrayHelper.Find(SumSave.db_stditems, e => e.Name == data.Item1);
            if (bag != null)
            {
                Clear_Condition();
                Need_Condition(data.Item1, data.Item2);
                if (Return_Condition())
                {
                    long moeny = bag.price * data.Item2;
                    Battle_Tool.Dream_Obtain_Unit(currency_unit.金币, moeny, Obtain_Int.Add_unit(moeny));
                    Alert_Dec.Show("出售成功,获得" + currency_unit.金币 + " * " + moeny);
                    Hide(true);
                }
            }
            else Alert_Dec.Show("物品错误,请联系管理");
        }
        else Alert_Dec.Show("该物品无法出售");
       
    }

    private void Hide(bool isRefresh = false)
    {
        gameObject.SetActive(false);
        if (isRefresh)
        {
            transform.parent.parent.parent.parent.SendMessage("Refresh"); 
        }
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public void Init((string, int) crt_data, Panel_BagType panel_BagType)
    {
        data = crt_data;
        info.text = crt_data.Item1 + "\n" + Tool_State.Show_Stditems(crt_data.Item1);
        ClearObject(m_showIcon_brom);
        Instantiate(material_item_prefab, m_showIcon_brom).Init(data);
        ShowBtn(panel_BagType);
    }

    private void ShowBtn(Panel_BagType panel_BagType)
    {
        foreach (var btn in btn_item_dic)
        { 
            btn.Value.gameObject.SetActive(false);
        }
        switch (panel_BagType)
        {
            case Panel_BagType.装备:
                break;
            case Panel_BagType.灵宠:
                break;
            case Panel_BagType.宝石:
                break;
            case Panel_BagType.材料:
                foreach (var btn in btn_item_dic)
                {
                    switch ((Hero_Resources_BtnType)(btn.Value.index))
                    {
                        case Hero_Resources_BtnType.出售:
                            btn.Value.gameObject.SetActive(true);
                            break;
                        default:
                            break;
                    }
                }
                break;
            case Panel_BagType.存入仓库:
                break;
            case Panel_BagType.取出仓库:
                break;
            case Panel_BagType.一键出售:
                break;
            case Panel_BagType.已装备:
                break;
            case Panel_BagType.展示:
                break;
            default:
                break;
        }
    }
    public void Init(Bag_Base_VO crt_data, Panel_BagType panel_BagType)
    {
        info.text = crt_data.Name;
        ClearObject(m_showIcon_brom);
        Instantiate(material_item_prefab, m_showIcon_brom).Init((crt_data.Name, 1));
        ShowBtn(panel_BagType);
    }
}
