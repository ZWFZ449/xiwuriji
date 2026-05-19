using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
/// <summary>
/// buff
/// </summary>
public class offect_buff : Base_Mono
{


    private Button rechargeGift, buff_exp;

    private TMP_Text info;

    private input_offect input_offect_prefab, crt_input_offect;

    private void Awake()
    {
        rechargeGift = Find<Button>("rechargeGift");
        rechargeGift.onClick.AddListener(OnRechargeGift);
        buff_exp = Find<Button>("buff_exp");
        buff_exp.onClick.AddListener(OnBuffExp);
        info = Find<TMP_Text>("other_buff/info");
        input_offect_prefab = Tool_UI.Find_Prefabs<input_offect>("input_offect");
    }

    private void OnBuffExp()
    {
        if (crt_input_offect == null)
        {
            crt_input_offect = Instantiate(input_offect_prefab, transform);
            crt_input_offect.GetConfirm.onClick.AddListener(() => { confirm(); });
        }
        crt_input_offect.gameObject.SetActive(true);
        crt_input_offect.Init("激活双倍buff",  "请输入数量"); //common_items_list.双倍经验卷轴
    }

    private void confirm()
    {
        if (crt_input_offect.GetInput != "")
        {
            if (int.TryParse(crt_input_offect.GetInput, out int result))
            {
                // 是纯数字（可转为 int）
                int number = int.Parse(crt_input_offect.GetInput);
                if (number <= 0)
                {
                    Alert_Dec.Show("请输入正确的数字");
                    return;
                }
                Clear_Condition();
                Need_Condition(common_Buff.双倍经验卷轴, number); 
                if (Return_Condition())
                {
                    // 激活buff
                    SumSave.crt_user_unit.AddBuff(common_Buff.双倍经验卷轴.ToString(), Tool_UI.ToStandardFormat(SumSave.nowtime), number);
                    SendNotification(NotiList.Refresh_Max_Hero_Attribute);
                    Alert_Dec.Show("激活成功");
                    Init();
                }else Alert_Dec.Show("激活失败,材料不足"); 
            }else Alert_Dec.Show("请输入正确的数字");
        }
        else Alert_Dec.Show("请输入正确的数字");
    }

    private void OnRechargeGift()
    {
        List<(string, string, int)> buffs = SumSave.crt_user_unit.GetBuff;
        for (int i = 0; i < buffs.Count; i++)
        {
            if (buffs[i].Item1 == common_Buff.狂欢.ToString())
            {
                int spanSeconds = Battle_Tool.SettlementTransport(buffs[i].Item2, 3);
                int time = buffs[i].Item3 - spanSeconds;//剩余时间
                int number = 24 - time;
                number = (int)MathF.Min(24, number);
                SumSave.crt_user_unit.AddBuff(common_Buff.狂欢.ToString(), Tool_UI.ToStandardFormat(SumSave.nowtime), number);
                Init();
                Alert_Dec.Show("狂欢已激活");
                SendNotification(NotiList.Refresh_Max_Hero_Attribute);
                return;
            }
        }
        SumSave.crt_user_unit.AddBuff(common_Buff.狂欢.ToString(), Tool_UI.ToStandardFormat(SumSave.nowtime), 24);
        SendNotification(NotiList.Refresh_Max_Hero_Attribute);
        Init();
    }

    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        List<(string,string,int)> buffs = SumSave.crt_user_unit.GetBuff;
        string dec = "当前buff列表\n";
        for (int i = 0; i < buffs.Count; i++)
        {
            int spanSeconds = Battle_Tool.SettlementTransport(buffs[i].Item2, 3);
            int time = buffs[i].Item3 - spanSeconds;//剩余时间
            if (buffs[i].Item1 == common_Buff.双倍经验卷轴.ToString())
            {
                if (time > 0)
                    dec += $"双倍经验卷轴 * {time}小时\n";
            }
            else if (buffs[i].Item1 == common_Buff.月卡.ToString())
            {
                if (buffs[i].Item3 >= 99999)
                {
                    dec += $"{buffs[i].Item1} * 永久\n";
                }
                else
                if (time > 0)
                {
                    dec += $"{buffs[i].Item1} * {time}小时\n";
                }
            }
            else
            {
                if (time > 0)
                { 
                    dec += $"{buffs[i].Item1} * {time}小时\n";
                }
            }
        }
        info.text= dec;
    }
}
