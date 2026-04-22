using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
/// <summary>
/// 推广
/// </summary>
public class offect_promotion : Base_Mono
{

    private InputField oneself, other;
    private Button btn;
    private Text info;
    private input_offect input_offect_prefab, crt_input_offect;

    private void Awake()
    { 
        oneself = Find<InputField>("oneself/show_id");
        other = Find<InputField>("other/show_id");
        btn = Find<Button>("other/recharge");
        btn.onClick.AddListener(OnBtnClick);
        info = Find<Text>("rechargeGift/info");
        info.GetComponent<Button>().onClick.AddListener(Claim_Gift);
        input_offect_prefab = Tool_UI.Find_Prefabs<input_offect>("input_offect");
    }
    /// <summary>
    /// 领取奖励
    /// </summary>
    private void Claim_Gift()
    {
        if (SumSave.crt_global_promotion.GetPromotion_moeny > 0)
        {
            SumSave.crt_global_gift.SetGiftPoints(SumSave.crt_global_promotion.GetPromotion_moeny);
            SumSave.crt_global_promotion.SetPromotion_moeny(-SumSave.crt_global_promotion.GetPromotion_moeny);
            Init();
            Alert_Dec.Show("领取推荐收益成功");
        }
        else
        {
            show_input_offect();
        }
    }
    private void show_input_offect()
    {
        if (crt_input_offect == null)
        {
            crt_input_offect = Instantiate(input_offect_prefab, transform);
            crt_input_offect.GetConfirm.onClick.AddListener(() => { confirm(); });
        }
        crt_input_offect.gameObject.SetActive(true);
        crt_input_offect.Init("领取礼包", "请输入礼包码");
    }

    private void confirm()
    {
        if (crt_input_offect.GetInput != "")
        {
            SendNotification(NotiList.Read_Global_Gift, crt_input_offect.GetInput);
            rename_confirm(crt_input_offect.GetInput);
        }
        else Alert_Dec.Show("请输入不含特殊符号的内容");
    }
    /// <summary>
    /// 领取确认
    /// </summary>
    /// <param name="arg0"></param>
    private void rename_confirm(string key)
    {
        //领取礼包
        if (SumSave.global_gift == null)
        {
            Alert_Dec.Show("礼包不存在或网络连接中断");
            return;
        }
        if (SumSave.global_gift.GetGiftState == 0)
        {
            Alert_Dec.Show("无效礼包");
            return;
        }
        bool exist = false;
        if (SumSave.global_gift.gift_type == 1)//全局
        {
            if (SumSave.global_gift.gift_par == SumSave.par || SumSave.global_gift.gift_par == -1)//通用礼包
            {
                if (SumSave.global_gift.GetGiftState == 1)//领取奖励的状态
                {
                    if (SumSave.crt_global_gift.IsHaveGift(key))
                    {
                        Alert_Dec.Show("礼包已领取");
                        return;
                    }
                    else
                    {
                        exist = true;
                        SumSave.crt_global_gift.SetGiftS(key);
                        Sift_value(SumSave.global_gift.GetGiftValue);
                    }
                }
            }
        }
        else if (SumSave.global_gift.gift_type == 2)//个人
        {
            if (SumSave.global_gift.GetGiftState == 1)//领取奖励的状态 
            {
                SumSave.global_gift.SetGiftValue(0);
                if (SumSave.global_gift.GetGiftPoints > 0)
                {
                    int gift_points = SumSave.global_gift.GetGiftPoints * 100;
                    Battle_Tool.Dream_Obtain_Unit(currency_unit.元宝, gift_points, Obtain_Int.Add_unit(gift_points));
                    Alert_Dec.Show("获得 " + (currency_unit.元宝)+ " + " + gift_points);
                    Alert_Dec.Show("获得荣耀积分  + " + SumSave.global_gift.GetGiftPoints);
                    SumSave.crt_global_gift.SetGiftPoints(SumSave.global_gift.GetGiftPoints);
                    if (SumSave.crt_global_promotion.GetPromotion_value != "")
                    {
                        //添加推荐人收益
                        SendNotification(NotiList.Add_global_promotion, (SumSave.crt_global_promotion.GetPromotion_value, SumSave.global_gift.GetGiftPoints / 10));
                    }
                }
                exist= true;
                Sift_value(SumSave.global_gift.GetGiftValue);
                //清空领取
            }
        }
        if (exist)
        {
            //写入历史领取
            Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto,
          Mysql_Table_Name.history_global_gift, SumSave.global_gift.Set_Instace_String()); 
            crt_input_offect.gameObject.SetActive(false);
            Game_Omphalos.i.archive();
            //Alert_Dec.Show("礼包领取成功");
        }
       
    }

    private void Sift_value(string gift_value)
    {
        List<string> list = ArrayHelper.Get_Split<string>(gift_value, ',');
        for (int i = 0; i < list.Count; i++)
        {
            List<string> gift_values = ArrayHelper.Get_Split<string>(list[i], ' ');
            if (gift_values.Count == 3)
            {
                switch (int.Parse(gift_values[0]))
                {
                    case 0://金币
                        Battle_Tool.Dream_Obtain_Unit((currency_unit)int.Parse(gift_values[1]), int.Parse(gift_values[2]), Obtain_Int.Add_unit(int.Parse(gift_values[2])));
                        Alert_Dec.Show("获得 "+ (currency_unit)int.Parse(gift_values[1])+"：" + gift_values[2]);
                        break;
                    case 1:
                        pet_list pet = Tool_State.ToEnum(gift_values[1], pet_list.麋鹿);
                        SumSave.crt_pet.AddPet(pet);
                        Alert_Dec.Show("获得 灵宠 * " + pet.ToString());
                        break;
                    case 2:
                        Bag_Base_VO synthesis_value = ArrayHelper.Find(SumSave.db_stditems, e => e.Name == gift_values[1]);
                        if (synthesis_value != null)
                        {
                            string user_value = Tool_Battle.Obtain_Equip(synthesis_value, 1, 1);
                            Bag_Base_VO synthesis = tool_Categoryt.Read_BaseBag(user_value);
                            SumSave.crt_bags.Set_Bag_List(synthesis);
                            Alert_Dec.Show("获得 " + synthesis.StdMode + " * " + synthesis.Name);
                        }
                        break;
                    case 3:
                        int number = int.Parse(gift_values[2]);
                        int random = Random.Range(1, 1000);
                        int maxnumber = number + Random.Range(1, 1000);
                        Alert_Dec.Show("获得 " + gift_values[1] + " * " + number);
                        Battle_Tool.Dream_Obtain_Resources(Obtain_Int.Add(1, gift_values[1], new int[] { number + random, random }), maxnumber);
                        break;
                    case 4:
                        string buff = gift_values[1];
                        int bufftime = int.Parse(gift_values[2]);
                        SumSave.crt_user_unit.AddBuff(buff, Tool_UI.ToStandardFormat(SumSave.nowtime), bufftime);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        oneself.text = SumSave.crt_user.uid;
        if (SumSave.crt_global_promotion.GetPromotion_value == "")
        {
            //other.text = "输入推广人Id";
        }
        else
        {
            btn.gameObject.SetActive(false);
            other.text = SumSave.crt_global_promotion.GetPromotion_value;
        }
        info.text = "我的推广数量 * " + SumSave.crt_global_promotion.promotion_number + "\n"
            + "我的推广收益 * " + SumSave.crt_global_promotion.GetPromotion_moeny;
    }

    private void OnEnable()
    {
        SendNotification(NotiList.Read_global_promotion);
        Init();
    }
    private void OnBtnClick()
    {
        if (other.text != null && other.text != "" && other.text != "输入推广人Id")
        {
            if (other.text != SumSave.crt_user.uid)
            {
                SumSave.crt_global_promotion.SetPromotion_value(other.text);
                //立刻刷新
                Game_Omphalos.i.archive();
                Init();
                Alert_Dec.Show("设置成功");
            }else Alert_Dec.Show("不能输入自己的id");
           
        }else Alert_Dec.Show("请输入正确的id");
    }
}
