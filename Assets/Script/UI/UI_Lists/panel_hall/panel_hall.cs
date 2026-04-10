using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class panel_hall : Panel_Base
{
    /// <summary>
    /// 资源，提升，地图父物体
    /// </summary>
    private Transform pos_hero, pos_map, pos_otain;
    /// <summary>
    /// 预制体组件
    /// </summary>
    private btn_item btn_item_Prefabs;
    /// <summary>
    /// 存储面板列表
    /// </summary>
    private Dictionary<string,GameObject> offect_list_dic = new Dictionary<string, GameObject>();

    private Image offect_list;

    /// <summary>
    /// offect打开类型
    /// </summary>
    private int index = -1;
    public override void Hide()
    {
        if (offect_list.gameObject.activeInHierarchy)
        {
            for (int i = offect_list.transform.childCount - 1; i >= 0; i--)//关闭区域内的UI
            {
                offect_list.transform.GetChild(i).gameObject.SetActive(false);
            }
            offect_list.gameObject.SetActive(false);
        }
        else
        base.Hide();
    }

    public override void Initialize()
    {
        base.Initialize();
        pos_hero = Find<Transform>("bg_main/panel_list/herolist/Scroll View/Viewport/Content");//
        pos_map = Find<Transform>("bg_main/panel_list/maplist/Scroll View/Viewport/Content");
        pos_otain = Find<Transform>("bg_main/panel_list/otainlist/Scroll View/Viewport/Content");
        btn_item_Prefabs = Battle_Tool.Find_Prefabs<btn_item>("btn_item");// Resources.Load<btn_item>("Prefabs/base_tool/btn_item");
        offect_list=Find<Image>("bg_main/offect_list");
        ClearObject(pos_hero);
        ClearObject(pos_map);
        ClearObject(pos_otain);
        for (int i = 0; i < SumSave.db_halls.herolist_btn.Count; i++)
        { 
            btn_item item = Instantiate(btn_item_Prefabs, pos_hero);

            item.Show(i, SumSave.db_halls.herolist_btn[i]);
            item.GetComponent<Button>().onClick.AddListener(() => { OnClickHeroItem(item); });
        }
        for (int i = 0; i < SumSave.db_halls.maplist_btn.Count; i++)
        { 
            btn_item item = Instantiate(btn_item_Prefabs, pos_map);
            item.Show(i, SumSave.db_halls.maplist_btn[i]);
            item.GetComponent<Button>().onClick.AddListener(() => { OnClickMapItem(item); });
        }
        for (int i = 0; i < SumSave.db_halls.otainlist_btn.Count; i++)
        { 
            btn_item item = Instantiate(btn_item_Prefabs, pos_otain);
            item.Show(i, SumSave.db_halls.otainlist_btn[i]);
            item.GetComponent<Button>().onClick.AddListener(() => { OnClickOtainItem(item); });
        }
    }
    /// <summary>
    /// 打开资源提升开关
    /// </summary>
    /// <param name="item"></param>
    private void OnClickOtainItem(btn_item item)
    {
        Show_GameObject(SumSave.db_halls.otainpanel[item.index], true);
    }
    /// <summary>
    /// 打开地图
    /// </summary>
    /// <param name="item"></param>
    private void OnClickMapItem(btn_item item)
    {
        Show_GameObject(SumSave.db_halls.mappanel[item.index], true);
    }
    /// <summary>
    /// 打开提升开关
    /// </summary>
    /// <param name="item"></param>
    private void OnClickHeroItem(btn_item item)
    {
        Show_GameObject(SumSave.db_halls.heropanel[item.index], true);
    }
    /// <summary>
    /// 打开开关
    /// </summary>
    /// <param name="index"></param>
    /// <param name="active"></param>
    private void Show_GameObject(string index,bool active)
    {
        offect_list.gameObject.SetActive(true);
        foreach (var item in offect_list_dic.Keys)
        {
            offect_list_dic[item].SetActive(false);
        }
        if (!offect_list_dic.ContainsKey(index))
        {
            GameObject obj = Resources.Load<GameObject>("Prefabs/panel_hall/" + index);
            obj = Instantiate(obj, offect_list.transform);
            offect_list_dic.Add(index, obj);
        }
        offect_list_dic[index].SetActive(active);
        if (active) offect_list_dic[index].GetComponent<Base_Mono>().Show();
    }
  

    public override void Show()
    {
        base.Show();
        offect_list.gameObject.SetActive(false);
    }

    protected override void Awake()
    {
        base.Awake();
    }
}
