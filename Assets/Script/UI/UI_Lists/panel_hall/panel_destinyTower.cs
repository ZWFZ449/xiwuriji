using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class panel_destinyTower : Base_Mono
{
    /// <summary>
    /// 天命显示位置，可替换的天命显示界面,替换显示位置
    /// </summary>
    private Transform tianming_image, locktianming,locktianming_image;
    /// <summary>
    /// 天命信息显示位置
    /// </summary>
    private Text tianming_Title, title_button;
    /// <summary>
    /// 刷新天命按钮 锁定按钮 切换按钮
    /// </summary>
    private Button UpButton,lockButton, switchButton;
    /// <summary>
    /// 天命预制体
    /// </summary>
    private GameObject tianming_item;
    /// <summary>
    /// 刷新消耗魔丸数量
    /// </summary>
    private int need = 20;
    /// <summary>
    /// 可切换的天命
    /// </summary>
    private int[] switch_tianming;



    private void Awake()
    {
        tianming_image = Find<Transform>("tianming_image/Viewport/Content");
        tianming_Title = Find<Text>("tianming_Title/Text");
        UpButton = Find<Button>("UpButton");
        UpButton.onClick.AddListener(UpButtonOnClick);
        title_button= Find<Text>("UpButton/title_button");
        lockButton = Find<Button>("lockButton");
        lockButton.onClick.AddListener(LockButton);
        locktianming = Find<Transform>("locktianming");
        locktianming_image = Find<Transform>("locktianming/tianming_image/Viewport/Content");
        switchButton= Find<Button>("locktianming/switchButton");
        switchButton.onClick.AddListener(SwitchButton);
        

        title_button.text = "魔丸*" + need + "刷新一次";
    }
    /// <summary>
    /// 切换天命
    /// </summary>
    private void SwitchButton()
    {
        if(SumSave.old_crt_hero.tianming_Platform.SequenceEqual(switch_tianming))
        {
            Alert_Dec.Show("当前天命相同");
        }else
        {
            SumSave.old_crt_hero.tianming_Platform = (int[])switch_tianming.Clone();
            RefreshDisplay();
            Battle_Tool.Init_Life_type();
            SendNotification(NotiList.Refresh_Max_Hero_Attribute);
        }
       
    }

    /// <summary>
    /// 锁定天命
    /// </summary>
    private void LockButton()
    { 
        if(locktianming.gameObject.activeInHierarchy)
        {
            locktianming.gameObject.SetActive(false);
            
            need = 20;
        }else
        {
            locktianming.gameObject.SetActive(true);
            switch_tianming =(int[]) SumSave.old_crt_hero.tianming_Platform.Clone();
            Refresh_and_switch_display(switch_tianming);
            need = 25;
        }
        title_button.text = "魔丸*" + need + "刷新一次";
    }

    /// <summary>
    /// 刷新切换显示
    /// </summary>
    /// <param name="tianming_Platform">天命</param>
    public void Refresh_and_switch_display(int [] tianming_Platform)
    {
        ClearObject(locktianming_image);
        for (int i = 0; i < tianming_Platform.Length; i++)
        {
            GameObject game = Resources.Load<GameObject>("Prefabs/halo/halo_" + (tianming_Platform[i] + 1));
            Instantiate(game, locktianming_image);
        }
    }

    /// <summary>
    /// 刷新天命台显示
    /// </summary>
    private void RefreshDisplay()
    {
        ClearObject(tianming_image);
        string str = "";
        str = "天命属性：";
        for (int i = 0; i < SumSave.old_crt_hero.tianming_Platform.Length; i++)
        {
            GameObject game = Resources.Load<GameObject>("Prefabs/halo/halo_" +(SumSave.old_crt_hero.tianming_Platform[i]+1));
            Instantiate(game, tianming_image);
            str += SumSave.five_element_type[SumSave.old_crt_hero.tianming_Platform[i]]+" ";
        }
        tianming_Title.text = str;
    }

    /// <summary>
    /// 刷新天命属性
    /// </summary>
    private void UpButtonOnClick()
    {
        SendNotification(NotiList.Read_Mysql_Base_Time);
        if (SumSave.openMysql)
        {
            Alert_Dec.Show("网络连接失败");
            return;
        }
        if (locktianming.gameObject.activeInHierarchy)
        {
            Alert_Dec.Show("天命已锁定");
            NeedConsumablesold(currency_unit.Boss积分, need);
            if (RefreshConsumables_old())
            {
                switch_tianming = (int[])SumSave.old_crt_hero.Uptianming_Platform().Clone();
                Refresh_and_switch_display(switch_tianming);
                Game_Omphalos.i.archive();
            }
            else
            {
                Alert_Dec.Show("魔丸不足");
            }

        }
        else
        {
            Alert_Dec.Show("天命未锁定");
            NeedConsumablesold(currency_unit.Boss积分, need);
            if (RefreshConsumables_old())
            {
                SumSave.old_crt_hero.RefreshTianming();
                RefreshDisplay();
                Battle_Tool.Init_Life_type();
                SendNotification(NotiList.Refresh_Max_Hero_Attribute);
                Game_Omphalos.i.archive();
            }
            else
            {
                Alert_Dec.Show("魔丸不足");
            }

        }


       
            
    }

    public override void Show()
    {
        base.Show();
        if (SumSave.crt_MaxHero_okd.Lv < 10 && SumSave.ios_account_number != "admin001")
        {
            Alert_Dec.Show("天命台开启等级为10级");
            gameObject.SetActive(false);
            return;
        }
        RefreshDisplay();
        locktianming.gameObject.SetActive(false);
    }
}
