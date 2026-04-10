using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class setting_item : MonoBehaviour
{
    private Dropdown dropdown;

    private int index;
    private void Awake()
    {
        dropdown = GetComponent<Dropdown>();
    }


    /// <summary>
    /// 返回引用
    /// </summary>
    /// <param name="arg0"></param>
    private void OnValueChange(int arg0)
    {
         transform.parent.parent.parent.parent.parent.SendMessage("Select_Setting",(index,arg0));
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="type">类型</param>
    /// <param name="list">列表</param>
    /// <param name="index">当前选中</param>
    public void Init(int type,string[] list,int value)
    {
        index = type;

        for (int i = 0; i < list.Length; i++)
        {
            dropdown.AddOptions(new List<string>() { list[i] });
        }
        dropdown.value = value;
        dropdown.onValueChanged.AddListener(OnValueChange);

        if (SumSave.crt_settingold.user_setting[4] == 1)//1为静音
        {
            AudioListener.pause = true;
            AudioManager.Instance.audioSource.Stop();
        }
        else
        {
            AudioListener.pause = false;
        }

    }
}
