using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class offect_Artifact : Base_Mono
{
    /// <summary>
    /// 神器位置
    /// </summary>
    private Transform pos_artifact, crt_btn;

    private btn_item btn_itm_prefabs;
    /// <summary>
    /// 当前按键
    /// </summary>
    private artifact_item crt_artifact;
    /// <summary>
    /// 神器列表
    /// </summary>
    private artifact_item artifact_itm_prefabs;
    /// <summary>
    /// 按键
    /// </summary>
    private List<string> btn_list = new List<string>() { "神器", "宝物", "奇物", "灵物" };
    /// <summary>
    /// 当前选择页面
    /// </summary>
    private int index = 1;

    private artifact_offect offect;
    protected void Awake()
    {
        Initialize();
    }
    public void Hide()
    {

    }
    public void Initialize()
    {
        crt_btn = Find<Transform>("bg_main/btn_list");
        pos_artifact= Find<Transform>("bg_main/Scroll View/Viewport/Content");
        btn_itm_prefabs = Battle_Tool.Find_Prefabs<btn_item>("btn_item"); //Resources.Load<btn_item>("Prefabs/base_tool/btn_item");
        artifact_itm_prefabs = Battle_Tool.Find_Prefabs<artifact_item>("artifact_item"); //Resources.Load<artifact_item>("Prefabs/panel_Artifact/artifact_item");
        offect = Find<artifact_offect>("bg_main/artifact_offect");
        for (int i = 0; i < btn_list.Count; i++)
        {
            btn_item btn = Instantiate(btn_itm_prefabs, crt_btn);
            btn.Show(i + 1, btn_list[i]);
            btn.GetComponent<Button>().onClick.AddListener(() => { OnBtnClick(btn); });
        }
    }
    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="btn"></param>
    private void OnBtnClick(btn_item btn)
    {
        AudioManager.Instance.playAudio(ClipEnum.使用物品);
        index= btn.index;
        Show_List();
    }
    public override void Show()
    {
        base.Show();
        Show_List();

    }
    /// <summary>
    /// 显示神器列表
    /// </summary>
    private void Show_List()
    {
        for (int i = pos_artifact.childCount - 1; i >= 0; i--)
        {
            Destroy(pos_artifact.GetChild(i).gameObject);
        }
        for (int i = 0; i < SumSave.db_Artifacts.Count; i++)
        {
            if (SumSave.db_Artifacts[i].arrifact_type == index)
            {
                artifact_item item = Instantiate(artifact_itm_prefabs, pos_artifact);
                item.Data = SumSave.db_Artifacts[i];
                List<(string, int)> list = SumSave.crt_artifact.Set();
                (string, int) index = ArrayHelper.Find(list, e => e.Item1 == SumSave.db_Artifacts[i].arrifact_name);
                if (index.Item2 > 0) item.Set(index.Item2);
                item.GetComponent<Button>().onClick.AddListener(delegate { Select_artifact(item); });
            }
           
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    private void Select_artifact(artifact_item item)
    {
        crt_artifact = item;
        offect.gameObject.SetActive(true);
        offect.set_artifact(item);
    }
}
