using Common;
using Components;
using MVC;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 大事记
/// </summary>
public class offect_Chronicle : Base_Mono
{
    private TMP_Text info;

    private Button Delete;

    private void Awake()
    {
        info = Find<TMP_Text>("list/Viewport/info");
        Delete = Find<Button>("title_name");
        Delete.onClick.AddListener(Delete_uid);
    }
    /// <summary>
    /// 删号
    /// </summary>
    private void Delete_uid()
    {
        Alert.Show("删除账户", "请确认是否删除账户", confirmDelete);
    }

    private void confirmDelete(object arg0)
    {
        SendNotification(NotiList.Delete);
        Application.Quit(); 
    }

    private void OnEnable()
    {
        Init();
    }
    /// <summary>
    /// 显示
    /// </summary>
    private void Init()
    {
        info.text= "";
        for (int i = 0; i < SumSave.global_Chronicle.Count; i++)
        {
            info.text += SumSave.global_Chronicle[i].Item1 + "." + SumSave.global_Chronicle[i].Item2 + "\n";
        }
    }
 
    private void Hide()
    {
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }

}
