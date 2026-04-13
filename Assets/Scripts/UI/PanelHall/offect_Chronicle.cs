using Common;
using Components;
using MVC;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class offect_Chronicle : Base_Mono
{
    private Text info;

    private void Awake()
    {
        info = Find<Text>("list/Viewport/info");
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
