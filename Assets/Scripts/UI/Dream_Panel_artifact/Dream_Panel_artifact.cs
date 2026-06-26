using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class Dream_Panel_artifact : Panel_Base
{

    private enum artifact_btn_list
    { 
        填充,
        升级,
        激活,
    }
    private Transform m_proms;

    private artifact_item artifact_item_prefab;

    private Dictionary<db_artifact_vo, artifact_item> dic_artifacts = new Dictionary<db_artifact_vo, artifact_item>();
    protected override void Awake()
    {
        base.Awake();
    }

    public override void Initialize()
    {
        base.Initialize();

        m_proms=Find<Transform>("bg/battle_btn_list/Scroll View/Viewport/Content");

        artifact_item_prefab = Tool_UI.Find_Prefabs<artifact_item>("artifact_item");

        for (int i = 0; i < SumSave.db_artifacts.Count; i++)
        {
            artifact_item item = Instantiate(artifact_item_prefab, m_proms);
            item.Initialize(SumSave.db_artifacts[i]);
            item.GetComponent<Button>().onClick.AddListener(() => { OnClickItem(item); });
            if (i <= 1) item.Set();
        }
    }

    public override void Show()
    {
        base.Show();
        Hide();
    }
    /// <summary>
    /// 点击物品
    /// </summary>
    /// <param name="item"></param>
    private void OnClickItem(artifact_item item)
    {

    }
}
