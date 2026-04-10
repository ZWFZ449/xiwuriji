using Common;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class panel_vip : Panel_Base
{
    /// <summary>
    /// VIP信息显示位置
    /// </summary>
    private Transform Information;
    /// <summary>
    /// vip信息预制体
    /// </summary>
    private vip_effect vip_effect_obj;
    /// <summary>
    /// vip信息标题
    /// </summary>
    private List<string> Title_list = new List<string>() { "荣耀等级", "进阶荣耀点", "经验加成", "灵珠收益", "装备爆率", "人物历练", "寻怪间隔", "生命回复", "法力回复", "幸运", "强化费用", "离线间隔", "签到收益", "鞭尸概率", "灵气上限"};
    /// <summary>
    /// vip等级显示位置
    /// </summary>
    private Text show_vip_lv_text;
    private new void Awake()
    {
        Initialize();
    }
    public override void Initialize()
    {
        base.Initialize();
        Information=Find<Transform>("Information/Viewport/Content");
        vip_effect_obj = Battle_Tool.Find_Prefabs<vip_effect>("vip_effect");
        show_vip_lv_text= Find<Text>("show_vip_lv/Text");
   
    }

    /// <summary>
    /// 显示vip信息
    /// </summary>
    public void Show_Vip_Info()
    {
        ClearObject(Information);

        (int,int,string) price =SumSave.crt_accumulatedrewards.SetRecharge();
        show_vip_lv_text.text.Clone();
        if (price.Item1 == 0)
        {
            show_vip_lv_text.text = "未进入荣耀殿堂";
        }   else
        {
            
            show_vip_lv_text.text = "荣耀点:" + SumSave.crt_accumulatedrewards.SetSum_recharge() + "\n荣耀等级:" + price.Item3;//显示vip等级和经验
        }
        



        vip_effect Title = Instantiate(vip_effect_obj, Information);//实例化vip信息标题
        Title.Init(-1, Title_list);

        for (int i = 0; i < SumSave.db_vip_list.Count; i++)//实例化vip信息
        {
            vip_effect vip_effect = Instantiate(vip_effect_obj, Information);
            vip_effect.Init(i, SumSave.db_vip_list[i]);
            if((price.Item1 + 1)== SumSave.db_vip_list[i].vip_lv)
            {
                return;
            }
        }
    }



    public override void Show()
    {
        base.Show();
        Show_Vip_Info();
    }
    public override void Hide()
    {
        base.Hide();
    }
}
