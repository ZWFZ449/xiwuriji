using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class map_select_reward_offect : Base_Mono
{

    private TMP_Text info, title;

    private Button close;

    private Button confirm;

    private Transform m_btn_brom;

    private btn_item btn_item_prefab;
    private void Awake()
    {
        info = Find<TMP_Text>("bg/info");
        title = Find<TMP_Text>("bg/title");
        close = Find<Button>("close_button");
        close.onClick.AddListener(delegate { gameObject.SetActive(false); });
        m_btn_brom = Find<Transform>("bg/btn_brom");
        btn_item_prefab = Tool_UI.Find_Prefabs<btn_item>("btn_item");
        confirm = Find<Button>("bg/confirm");
        ClearObject(m_btn_brom);
        for (int i = 0; i < 3; i++)
        {
            btn_item btn_item = Instantiate(btn_item_prefab, m_btn_brom);
            btn_item.Show(i, (100 + (i * 50)) + "%");
            btn_item.GetComponent<Button>().onClick.AddListener(() => SelectBtn(btn_item));
        }
    }

    /// <summary>
    /// 选择按键
    /// </summary>
    /// <param name="btn_item"></param>
    private void SelectBtn(btn_item btn_item)
    {

    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="_title"></param>
    /// <param name="_info"></param>
    public void Init(object _title, string _info)
    {
        title.text = _title.ToString();
        info.text = _info;
    }
    /// <summary>
    /// 返回按键
    /// </summary>
    public Button GetConfirm { get { return confirm; } }
   
}
