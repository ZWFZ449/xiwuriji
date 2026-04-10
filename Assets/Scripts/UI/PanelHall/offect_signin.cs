using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class offect_signin : Base_Mono
{
    private Button signin_btn;

    private Text info;
    private void Awake()
    {
        signin_btn = Find<Button>("signin_btn");

        signin_btn.onClick.AddListener(signin);
        info=Find<Text>("Scroll View/Viewport/Content/info");
        init();
    }
    /// <summary>
    /// 签到
    /// </summary>
    private void signin()
    {
        if ((SumSave.nowtime - SumSave.crt_signin.now_time).Days >= 1)
        {
            SumSave.crt_signin.now_time = Convert.ToDateTime(SumSave.nowtime.ToString("yyyy-MM-dd"));
            SumSave.crt_signin.number++;
            SumSave.crt_signin.max_number++;
            SumSave.crt_signin.MysqlData();
            Clear();
            Alert_Dec.Show("签到成功");
            int money = 1000000;
            string dec = "获得" + Show_Color.Red(money) + " " + currency_unit.金币;
            Battle_Tool.Dream_Obtain_Unit(currency_unit.金币, money, Obtain_Int.Add_unit(money));
            int number = 10;
            int random = Random.Range(1, 1000);
            int maxnumber = number + Random.Range(1, 1000);
            Battle_Tool.Dream_Obtain_Resources(Obtain_Int.Add(1, common_items_list.Boss召唤卷轴, new int[] { number + random, random }), maxnumber);
            dec += "\n获得" + Show_Color.Red(number) + " " + common_items_list.Boss召唤卷轴;
            Alert.Show("签到奖励", dec);
            Hide();
        }
        else Alert_Dec.Show("今日已签到");
    }
    /// <summary>
    /// 清空活动数据 每日副本 
    /// </summary>
    private void Clear()
    {

    }

    public override void Show()
    {
        base.Show();
        init();
    }

    private void init()
    {
        info.text = "签到奖励";
        int money = 1000000;
        string dec = "\n " + Show_Color.Red(money) + " " + currency_unit.金币;
        dec += "\n " + common_items_list.Boss召唤卷轴 + "*" + Show_Color.Red(10);
        info.text += dec;
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }
}
