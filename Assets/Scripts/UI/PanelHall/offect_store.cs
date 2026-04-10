using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Store_Type
{ 
    药铺,
    杂货铺,
    珍宝阁
}

public class offect_store : Base_Mono
{ 
    private store_item_buy show_store_item_buy;
    private Text title_name;
    private Transform m_pos_brom;
    private store_item  store_item_prefab;
    private Store_Type crt_type;
    private Button close_button;
    private void Awake()
    {
        title_name = Find<Text>("title_name/info");
        m_pos_brom = Find<Transform>("Scroll View/Viewport/Content");
        store_item_prefab= Tool_UI.Find_Prefabs<store_item>("store_item");
        close_button = Find<Button>("close_button");
        close_button.onClick.AddListener(()=> Hide());
        show_store_item_buy=Find<store_item_buy>("store_item_buy"); 
    }
    /// <summary>
    /// 关闭
    /// </summary>
    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public override void Show()
    {
        base.Show();
        show_store_item_buy.gameObject.SetActive(false);
    }
    public virtual void Init(Store_Type type)
    {
        crt_type = type;
        title_name.text= type.ToString();
        List<db_store_vo> list = ArrayHelper.FindAll(SumSave.db_stores_list, (vo) => vo.store_Type == (int)type);
        ClearObject(m_pos_brom);
        for (int i= 0; i < list.Count; i++) 
        {
            store_item item = Instantiate(store_item_prefab, m_pos_brom);
            item.Init(list[i]);
            item.GetComponent<Button>().onClick.AddListener(() => OnClick_Buy(item));
        }
    }
    /// <summary>
    /// 点击购买
    /// </summary>
    /// <param name="item"></param>
    private void OnClick_Buy(store_item item)
    {
        show_store_item_buy.gameObject.SetActive(true);
        show_store_item_buy.Init(item.Set());
    }
}
