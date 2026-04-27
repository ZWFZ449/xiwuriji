using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class panel_setting : Panel_Base
{
    /// <summary>
    /// 预制体位置
    /// </summary>
    private Transform crt_setting;
    /// <summary>
    /// 预制体
    /// </summary>
    private setting_item setting_Item_prefab;

    private Button close;

    private string[] setting_name = { "设置1", "设置2", "设置3", "设置4", "设置5" };

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Initialize()
    {
        base.Initialize();
        crt_setting=Find<Transform>("bg_main/Scroll View/Viewport/Content");
        setting_Item_prefab= Battle_Tool.Find_Prefabs<setting_item>("setting_item");
        close=Find<Button>("bg_main/btn_close");
        close.onClick.AddListener(() => { Close_Hide(); });
        //for (int i = 0; i < SumSave.db_sttings.Count; i++)
        //{
        //    setting_item item = Instantiate(setting_Item_prefab, crt_setting);
        //    item.Init(i, SumSave.db_sttings[i].option_setting.Split(' '), SumSave.crt_settingold.user_setting[i]);
        //}
        
    }
    /// <summary>
    /// 关闭游戏
    /// </summary>
    private void Close_Hide()
    {
        Alert.Show("删除账户", "请确认是否删除当前区账户", Close_Hide1);
    }
    /// <summary>
    /// 删除账户
    /// </summary>
    /// <param name="arg0"></param>
    private void Close_Hide1(object arg0)
    {
        Alert.Show("删除账户", "再次确认是否删除当前区账户,\n删除后将锁定账户,7天内无登录后将直接删除\n如希望恢复账户,请在7天内直接使用当前账户再次登录既可激活", Close_Hide2);
    }

    private void Close_Hide2(object arg0)
    {
        Hide();
        UI_Manager.Instance.GetPanel<panel_login>().Show(); 
    }

    /// <summary>
    /// 设置
    /// </summary>
    /// <param name="data"></param>
    protected void Select_Setting((int index, int value) data)
    {
        SumSave.crt_settingold.user_setting[data.index] = data.value;
        SendNotification(NotiList.Refresh_User_Setting, SumSave.crt_settingold);
        AutomaticEquipmentRecyclingTask(data);
        if (SumSave.crt_settingold.user_setting[4] == 1)//1为静音
        {
            AudioListener.pause = true;
            AudioManager.Instance.audioSource.Stop(); 
        }
        else
        { 
            AudioListener.pause = false;
        }


        Alert_Dec.Show("设置成功");
    }

    /// <summary>
    /// 自动回收装备任务
    /// </summary>
    private  void AutomaticEquipmentRecyclingTask((int index, int value) data)
    {
        if (data.index == 0)
        {
            tool_Categoryt.Base_Task(1022);
        }

    }

    public override void Show()
    {
        base.Show();
    }
}
