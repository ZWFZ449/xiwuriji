using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class Daily_copies : Base_Mono
{
    private Transform pos_crtmap;
    private copies_item copies_item_Prefabs;
    /// <summary>
    /// 战斗地图
    /// </summary>
    private panel_fight fight_panel;

    private void Awake()
    {
        pos_crtmap=Find<Transform>("Scroll View/Viewport/Content");
        copies_item_Prefabs = Battle_Tool.Find_Prefabs<copies_item>("copies_item");
        fight_panel = UI_Manager.I.GetPanel<panel_fight>();
    }
    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="item"></param>
    private void OnClick(copies_item item)
    {
        string dec = "";
        dec = "是否进入"+item.index.map_name+"";
        if (item.index.need_Required != "")
        {
            dec += "\n 挑战门票 " + item.index.need_Required + " * 1";
        }
        dec += item.ShowInfo();
        Alert.Show("挑战副本", dec, confirm,item);
    }
    /// <summary>
    /// 确认进入
    /// </summary>
    /// <param name="arg0"></param>
    private void confirm(object arg0)
    {
        SendNotification(NotiList.Read_Mysql_Base_Time);
        if (SumSave.openMysql)
        {
            Alert_Dec.Show("获取网络失败");
            return;
        }
        copies_item item = (arg0 as copies_item);
        if (item.IsSate())
        {
            if (item.index.need_Required != "")
            {
                NeedConsumablesold(item.index.need_Required, 1);
                if (RefreshConsumables_old())
                {
                    Open_Map(item);
                }
            }else Open_Map(item);
        }
        else Alert_Dec.Show("挑战次数不足");
        
    }

    /// <summary>
    /// 进入地图
    /// </summary>
    /// <param name="item"></param>
    private void Open_Map(copies_item item)
    {
        bool exist = true;
        List<(string, int)> list = SumSave.crt_needlist.SetMap();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Item1 == item.index.map_name)
            {
                exist = false;
                list[i] = (list[i].Item1, list[i].Item2 +1);
                SumSave.crt_needlist.SetMap(list[i]);
                break;
            }
        }
        if (exist) SumSave.crt_needlist.SetMap((item.index.map_name, 1));
        //写入数据
       
        fight_panel.Show();
        fight_panel.Open_Map(item.index,true);
        item.updatestate();
        EnterTheReplicaTask(item);
        Game_Omphalos.i.archive();
    }

    /// <summary>
    /// 进入副本任务
    /// </summary>
    private void EnterTheReplicaTask(copies_item item)
    {
        if(item.index.map_name == "灵珠副本")
        {
            tool_Categoryt.Base_Task(1069);
        }
        if (item.index.map_name == "经验副本")
        {
            tool_Categoryt.Base_Task(1070);
        }
        if (item.index.map_name == "魔种副本")
        {
            tool_Categoryt.Base_Task(1071);
        }
        if (item.index.map_name == "经验副本")
        {
            tool_Categoryt.Base_Task(1072);
        }
        if (item.index.map_name == "灵气副本")
        {
            tool_Categoryt.Base_Task(1073);
        }
        if (item.index.map_name == "魔丸副本")
        {
            tool_Categoryt.Base_Task(1074);
        }
        if (item.index.map_name == "历练副本")
        {
            tool_Categoryt.Base_Task(1075);
        }
    }
    public override void Show()
    {
        base.Show();
        if (SumSave.crt_MaxHero_okd.Lv < 30 && SumSave.ios_account_number != "admin001")
        {
            Alert_Dec.Show("副本开启等级为30级");
            gameObject.SetActive(false);
            return;
        }
        base_Show();
    }
    /// <summary>
    /// 初始化
    /// </summary>
    private void base_Show()
    {
        ClearObject(pos_crtmap);
        List<(string, int)> list = SumSave.crt_needlist.SetMap();
        int maxnumber = SumSave.crt_MaxHero_okd.Lv / 100 + 1;
        for (int i = SumSave.read_lose_map.Count - 1; i > 0; i--)
        {
            if (SumSave.read_lose_map[i].map_type == 4&& SumSave.read_lose_map[i].need_Required=="")
            {
                int number = 0;
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[j].Item1 == SumSave.read_lose_map[i].map_name)
                    { 
                        number = list[j].Item2;
                        break;
                    }
                }
                copies_item item = Instantiate(copies_item_Prefabs, pos_crtmap);
                item.Init(SumSave.read_lose_map[i], number, maxnumber);
                item.GetComponent<Button>().onClick.AddListener(() => { OnClick(item); });
            }
        }
    }
}
