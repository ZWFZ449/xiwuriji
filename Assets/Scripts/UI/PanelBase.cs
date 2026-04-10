using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelBase : Base_Mono
{
    /// <summary>
    /// 基准位置
    /// </summary>
    protected Transform base_crt;
    /// <summary>
    /// 关闭按钮
    /// </summary>
    protected Button closeButton;
    /// <summary>
    /// 面板名称
    /// </summary>
    protected string panelName;
    protected virtual void Awake()
    {
        closeButton = Find<Button>("close_button");
        if (closeButton != null) closeButton.onClick.AddListener(Hide);
        Initialize();
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Initialize()
    {

    }
    /// <summary>
    ///  隐藏
    /// </summary>
    public virtual void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
