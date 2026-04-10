using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using UnityEngine.UI;
using System;
using Components;
using UI;
using Random = UnityEngine.Random;

public class signIn : Base_Mono
{
    /// <summary>
    /// 签到列表
    /// </summary>
    private Transform pos_list;
    /// <summary>
    /// 签到列表
    /// </summary>
    private signln_item signln_item_prafabs;
    /// <summary>
    /// 签到信息
    /// </summary>
    private Text info;
    /// <summary>
    /// 签到按钮
    /// </summary>
    private Button btn_signln,max_signln;

    private Panel_Accumulatedrewards panel_accumulatedrewards;

    private void Awake()
    {
        pos_list=Find<Transform>("Scroll View/Viewport/Content");
        signln_item_prafabs = Battle_Tool.Find_Prefabs<signln_item>("signln_item"); //Resources.Load<signln_item>("prefabs/panel_hall/signIn/signln_item");
        info = Find<Text>("bg/info");
        btn_signln = Find<Button>("btn_signln");
        btn_signln.onClick.AddListener(OnClick_signln);
        max_signln=Find<Button>("btn_maxsignln");
        max_signln.onClick.AddListener(OnClick_maxsignln);
        panel_accumulatedrewards = UI_Manager.I.GetPanel<Panel_Accumulatedrewards>();
        ClearObject(pos_list);
        List<int> list = SumSave.crt_signin.Set();
        for (int i = 0; i < SumSave.db_Signins.Count; i++)
        {
            signln_item item = Instantiate(signln_item_prafabs, pos_list);
            while (i >= list.Count)
            {
                list.Add(0);
            }
            item.Init(i, SumSave.db_Signins[i], list[i]);
            item.GetComponent<Button>().onClick.AddListener(() => { OnClick(item); });
        }
    }

    private void OnClick_maxsignln()
    {
        panel_accumulatedrewards.Show();
        panel_accumulatedrewards.Init(2);
    }

    /// <summary>
    /// 每日签到
    /// </summary>
    private void OnClick_signln()
    {
        if ((SumSave.nowtime - SumSave.crt_signin.now_time).Days >= 1)
        {
            SumSave.crt_signin.now_time = Convert.ToDateTime(SumSave.nowtime.ToString("yyyy-MM-dd"));
            SumSave.crt_signin.number++;
            SumSave.crt_signin.max_number++;
            Clear();
            Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.dream_user_signin, SumSave.crt_signin.Set_Uptade_String(),
                SumSave.crt_signin.Get_Update_Character());
            Alert_Dec.Show("签到成功");
            SumSave.crt_pass.clear_data();
            long num = 1000000 * SumSave.crt_signin.number;
            if(num>=100000000)//限制为1亿
            {
                num = 100000000;
            }
            SumSave.crt_user_unit.verify_data(currency_unit.金币, num);

            string dec = "\n签到奖励\n";
            dec+="灵珠 * "+ Battle_Tool.FormatNumberToChineseUnit(num) +"\n";
            dec += "荣耀点 * 2\n";

            if (Tool_State.IsState(State_List.月卡))
            {
                SumSave.crt_user_unit.verify_data(currency_unit.Boss积分, 30);
                SumSave.crt_accumulatedrewards.Set(2, 4);
                dec += Show_Color.Red("月卡奖励\n") + "1.魔丸 * 30\n2.荣耀点 + 2\n";
            }
            else
            {
                SumSave.crt_accumulatedrewards.Set(2, 2);//不是月卡给2点荣耀点
            } 
            if (Tool_State.IsState(State_List.至尊卡))
            {
                int random = Random.Range(1, 100);
                int number = 2;
                int maxnumber = number + Random.Range(1, 100);
                Battle_Tool.Obtain_Resources(Obtain_Int.Add(1, "命运金币", new int[] { number + random, random }), maxnumber);
                //Battle_Tool.Obtain_Resources("命运金币", 2);
                dec += Show_Color.Red("至尊卡奖励\n") + "命运金币 + 2\n";
            }
            dec += "通行证已重置\n";
            dec += "宝箱已重置\n";
            dec += "每日副本已重置\n";
            dec += "世界boss已重置\n";
            Alert.Show("签到奖励", dec);
            SignInTask();
            base_show();
        }
        else Alert_Dec.Show("今日已签到");
    }
    

        /// <summary>
        /// 完成签到任务
        /// </summary>
    private void SignInTask()
    {
        tool_Categoryt.Base_Task(1026);
    }

    /// <summary>
    /// 重置需求
    /// </summary>
    private void Clear()
    {
        //累积vip经验
        //SumSave.crt_accumulatedrewards.Set(2, 2);
        //限购商店,地图次数刷新
        SumSave.crt_needlist.DailyClear();
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="item"></param>
    private void OnClick(signln_item item)
    {
        int index = item.Set();
        List<int> list = SumSave.crt_signin.Set();
        if (index<= list.Count && list[index] == 1)
        {
            Alert_Dec.Show("已领取奖励");
            return;
        } 
        db_signin_vo vo = SumSave.db_Signins[index];
        if (SumSave.crt_signin.number < vo.index)
        {
            Alert_Dec.Show("未满足领取条件") ;
            return;
        }
        string[] strs = vo.value.Split('*');
        string dec = strs[0] + "*" + strs[1];
        if (strs[0] == "灵珠")
        {
            for (int i = 0; i < SumSave.db_vip_list.Count; i++)
            {
                if (SumSave.db_vip_list[i].vip_lv == SumSave.crt_accumulatedrewards.SetRecharge().Item1)
                {
                    dec = strs[0] + "*" + Battle_Tool.FormatNumberToChineseUnit(long.Parse(strs[1]))+ 
                        Show_Color.Red("\n（荣耀加成：+" + Battle_Tool.FormatNumberToChineseUnit(int.Parse(strs[1]) * (SumSave.db_vip_list[i].signInIncome / 100)) + ")");

                    strs[1] = (int.Parse(strs[1]) + (int.Parse(strs[1]) * (SumSave.db_vip_list[i].signInIncome / 100))).ToString();
                }
            }
           
        }
        else
        {
            dec = strs[0] + "*" + strs[1];
        }

        Alert.Show("领取奖励", "确认是否领取" + dec, Confirmreceipt, item);
    }
    /// <summary>
    /// 领取奖励
    /// </summary>
    /// <param name="arg0"></param>
    private void Confirmreceipt(object arg0)
    {
        signln_item item = arg0 as signln_item;
        int index = item.Set();
        string[]strs = SumSave.db_Signins[index].value.Split('*');
        if(strs[0]=="灵珠")
        {
            for (int i = 0; i < SumSave.db_vip_list.Count; i++)
            {
                if (SumSave.db_vip_list[i].vip_lv == SumSave.crt_accumulatedrewards.SetRecharge().Item1)
                {
                    strs[1] = (int.Parse(strs[1])+(int.Parse(strs[1])* (SumSave.db_vip_list[i].signInIncome / 100))).ToString();
                }
            }
            string str = strs[0] + "*" + strs[1]+"*" + strs[2];
            Battle_Tool.Obtain_result(str);
        }else
        {
            Battle_Tool.Obtain_result(SumSave.db_Signins[index].value);
        }

        Alert_Dec.Show("领取成功");
        SumSave.crt_signin.Set(index);
        item.Init(index, SumSave.db_Signins[index], 1);
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.dream_user_signin, SumSave.crt_signin.Set_Uptade_String(),
    SumSave.crt_signin.Get_Update_Character());
    }

    public override void Show()
    {
        base.Show();
        base_show();
    }
    /// <summary>
    /// 显示基础信息
    /// </summary>
    private void base_show()
    {
        string dec = "签到奖励";
        dec += "\n累积签到天数 " + "* " + SumSave.crt_signin.number + " 天";
        dec += "\n签到获得" + currency_unit.金币 + "* " + (1000000 * (SumSave.crt_signin.number + 1));
        info.text = dec;

        if ((SumSave.nowtime - SumSave.crt_signin.now_time).Days < 1)
        {
            SignInTask();
        }
    }
}
