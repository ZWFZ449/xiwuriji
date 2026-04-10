using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class input_offect
    : Base_Mono
{
    private InputField inputField;

    private Text info, title;

    private Button close;

    private Button confirm;
    private void Awake()
    {
        inputField =Find<InputField>("bg/InputField");
        info = Find<Text>("bg/info");
        title = Find<Text>("bg/title");
        close = Find<Button>("close_button");
        close.onClick.AddListener(delegate {gameObject.SetActive(false); });
        confirm = Find<Button>("bg/confirm");
        //confirm.onClick.AddListener(delegate { OnConfirm(); });
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="_title"></param>
    /// <param name="_info"></param>
    public void Init(object _title, string _info)
    {
        title.text= _title.ToString();
        info.text = _info;
    }
    /// <summary>
    /// 返回按键
    /// </summary>
    public Button GetConfirm { get { return confirm; } }
    /// <summary>
    /// 输入内容
    /// </summary>
    public string GetInput { get { return Tool_Battle.IsValidString( inputField.text)? inputField.text : "" ; } }

}
