using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityColorPresets;
using Random = UnityEngine.Random;

public class offect_Fame : Base_Mono
{
    private Transform m_btn_brom;

    private btn_item btn_item_prefab;

    private Transform m_gift_borm;

    private material_item p_material_item_prefab;


    private Button confirm;

    private TMP_Text info;
    /// <summary>
    /// 选中vip
    /// </summary>
    private db_vip vip;
    private void Awake()
    {
        m_btn_brom = Find<Transform>("btn_list/Viewport/Content");
        info = Find<TMP_Text>("fame_list/Viewport/Text");
        btn_item_prefab = Tool_UI.Find_Prefabs<btn_item>("btn_item");
        m_gift_borm = Find<Transform>("rechargeGift/Scroll View/Viewport/Content");
        p_material_item_prefab = Tool_UI.Find_Prefabs<material_item>("material_item");
        confirm = Find<Button>("rechargeGift/btn/recharge");
        confirm.onClick.AddListener(delegate { OnClick_confirm(); });
        init();
    }
    /// <summary>
    /// 领取礼包
    /// </summary>
    private void OnClick_confirm()
    {
        if (vip != null)
        {
            if ((int.Parse)(SumSave.crt_global_gift.GetGiftPoints) >= vip.vip_exp)
            {
                SumSave.crt_global_gift.SetGiftS(vip.vip_name);
                List<string> list = ArrayHelper.Get_Split<string>(vip.gift_value, ',');
                for (int i = 0; i < list.Count; i++)
                {
                    List<string> gift_values = ArrayHelper.Get_Split<string>(list[i], ' ');
                    if (gift_values.Count == 3)
                    {
                        switch (int.Parse(gift_values[0]))
                        {
                            case 1:
                                pet_list pet = Tool_State.ToEnum(gift_values[1], pet_list.麋鹿);
                                SumSave.crt_pet.AddPet(pet);
                                Alert_Dec.Show("获得 灵宠：" + pet.ToString());
                                break;
                            case 2:
                                Bag_Base_VO synthesis_value = ArrayHelper.Find(SumSave.db_stditems, e => e.Name == gift_values[1]);
                                if (synthesis_value != null)
                                {
                                    synthesis_value.user_value = Tool_Battle.Obtain_Equip(synthesis_value, 1, 1);
                                    synthesis_value = tool_Categoryt.Read_Bag(synthesis_value.user_value);
                                    SumSave.crt_bags.Set_Bag_List(synthesis_value);
                                    Alert_Dec.Show("获得 "+ synthesis_value.StdMode + "：" + synthesis_value.Name);
                                }
                                break;
                            case 3:
                                int number = int.Parse(gift_values[2]);
                                int random = Random.Range(1, 1000);
                                int maxnumber = number + Random.Range(1, 1000);
                                Alert_Dec.Show("获得: " + gift_values[1] + " * " + number);
                                Battle_Tool.Dream_Obtain_Resources(Obtain_Int.Add(1, gift_values[1], new int[] { number + random, random }), maxnumber);
                                break;
                            default:
                                break;
                        }
                    }
                }
                Show_Info(vip, true);
            }
        }
    }

    private void init()
    {
        int sum = (int.Parse)(SumSave.crt_global_gift.GetGiftPoints);
        for (int i = 0; i < SumSave.db_vip_list.Count; i++)
        { 
            btn_item btn = Instantiate(btn_item_prefab, m_btn_brom);
            btn.Show(SumSave.db_vip_list[i].vip_lv, SumSave.db_vip_list[i].vip_name);
            btn.GetComponent<Button>().onClick.AddListener(delegate { OnClick(btn); });
            if (i == 0) OnClick(btn);
            if (sum >= SumSave.db_vip_list[i].vip_exp)
            {
                OnClick(btn);
            }
        }
    }
    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="btn"></param>
    private void OnClick(btn_item btn)
    {
        int sum = (int.Parse)(SumSave.crt_global_gift.GetGiftPoints);
        for (int i = 0; i < SumSave.db_vip_list.Count; i++)
        {
            if (btn.index == SumSave.db_vip_list[i].vip_lv)
            {
                info.text = Show_Info(SumSave.db_vip_list[i], sum >= SumSave.db_vip_list[i].vip_exp);
            }
        }
    }
    /// <summary>
    /// 显示信息
    /// </summary>
    /// <param name="vip"></param>
    /// <param name="isOpen"></param>
    /// <returns></returns>
    private string Show_Info(db_vip vip,bool isOpen)
    {
        string dec = vip.vip_name + "\n";
        this.vip = vip;
        dec += "荣耀积分(" + SumSave.crt_global_gift.GetGiftPoints + "/" + vip.vip_exp + ")\n";

        dec += enum_equip_entry_list.金币掉落 + " + " + Colorize(vip.lingzhuIncome + "%\n", GameColors.Uncommon);
        dec += enum_equip_entry_list.经验加成 + " + " + Colorize(vip.experienceBonus + "%\n", GameColors.Uncommon);
        dec+= enum_equip_entry_list.怪物爆率 + " + " + Colorize( vip.equipmentExplosionRate + "%\n", GameColors.Uncommon);
        dec += "Boss刷新时间" + " - " + Colorize(-vip.monsterHuntingInterval + "%\n", GameColors.Uncommon);
        dec += "签到" + " " + Colorize(common_items_list.Boss召唤卷轴 + " * "+vip.characterExperience + "\n", GameColors.Uncommon);
        dec += "签到" + " " + Colorize(common_items_list.双倍经验卷轴 + " * " + vip.characterExperience + "\n", GameColors.Uncommon);
        if (isOpen)
        {
            confirm.gameObject.SetActive(!SumSave.crt_global_gift.IsHaveGift(vip.vip_name));
            dec += Colorize("已激活", GameColors.Horde);
        } 
        else dec += "未激活";
        List<string> list = ArrayHelper.Get_Split<string>(vip.gift_value, ',');
        ClearObject(m_gift_borm);
        for (int i = 0; i < list.Count; i++)
        {
            List<string> gift_values = ArrayHelper.Get_Split<string>(list[i], ' ');
            if (gift_values.Count == 3)
            {
                material_item item = Instantiate(p_material_item_prefab, m_gift_borm);
                item.Init((gift_values[1], int.Parse(gift_values[2])));
            }
        }
        
        return dec;

    }
}
