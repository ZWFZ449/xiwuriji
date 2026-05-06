using Common;
using Components;
using MVC;
using System;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using static UnityColorPresets;
using TMPro;
using UnityEngine;

public class offect_signin : Base_Mono
{
    private Button signin_btn;

    private TMP_Text info;

    private db_vip crt_vip;
    private void Awake()
    {
        signin_btn = Find<Button>("signin_btn");

        signin_btn.onClick.AddListener(signin);
        info=Find<TMP_Text>("Scroll View/Viewport/Content/info");
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
            SumSave.crt_signin.Clear();
            SumSave.crt_signin.MysqlData();
            Alert_Dec.Show("签到成功");
            int money = 1000000;
            SumSave.crt_global_gift.SetGiftPoints(2);
            string dec = "获得 " + Show_Color.Red(2) + " 荣耀积分";
            dec += "\n获得" + Show_Color.Red(money) + " " + currency_unit.金币;
            Battle_Tool.Dream_Obtain_Unit(currency_unit.金币, money, Obtain_Int.Add_unit(money));
            int number = 10;
            int random = Random.Range(1, 1000);
            int maxnumber = number + Random.Range(1, 1000);
            Battle_Tool.Dream_Obtain_Resources(Obtain_Int.Add(1, common_items_list.Boss召唤卷轴, new int[] { number + random, random }), maxnumber);
            dec += "\n获得" + Show_Color.Red(number) + " " + common_items_list.Boss召唤卷轴;
            if (crt_vip != null)
            {
                dec += "\n" + crt_vip.vip_name + " 奖励\n";
                dec += Colorize(currency_unit.元宝 + " * " + (crt_vip.characterExperience * 20) + "\n", GameColors.Uncommon);
                dec += Colorize(common_items_list.Boss召唤卷轴 + " * " + crt_vip.characterExperience + "\n", GameColors.Uncommon);
                //dec += Colorize(common_Buff.双倍经验卷轴 + " * " + crt_vip.characterExperience + "\n", GameColors.Uncommon);
                Battle_Tool.Dream_Obtain_Unit(currency_unit.元宝, (crt_vip.characterExperience * 20), Obtain_Int.Add_unit((crt_vip.characterExperience * 20)));
                random = Random.Range(1, 1000);
                maxnumber = crt_vip.characterExperience + Random.Range(1, 1000);
                Battle_Tool.Dream_Obtain_Resources(Obtain_Int.Add(1, common_items_list.Boss召唤卷轴, new int[] { crt_vip.characterExperience + random, random }), maxnumber);
                //random = Random.Range(1, 1000);
                //maxnumber = crt_vip.characterExperience + Random.Range(1, 1000);
                //Battle_Tool.Dream_Obtain_Resources(Obtain_Int.Add(1, common_Buff.双倍经验卷轴, new int[] { crt_vip.characterExperience + random, random }), maxnumber);
                pet_list pet = (pet_list)(crt_vip.vip_lv - 1);
                SumSave.crt_pet.AddPet(pet);
                dec += Colorize("\n获得 灵宠 " + pet.ToString() + "\n", GameColors.Uncommon);
            }
            else
            {
                pet_list pet = pet_list.麋鹿;
                SumSave.crt_pet.AddPet(pet);
                dec += Colorize("获得 灵宠 " + pet.ToString() + "\n", GameColors.Uncommon);
            }
            Alert.Show("签到奖励", dec);
            Game_Omphalos.i.archive();
            Hide();
        }
        else Alert_Dec.Show("今日已签到");
    }

    public override void Show()
    {
        base.Show();
        Obtain_Vip();
        init();
    }

    private void init()
    {
        info.text = "签到奖励";
        int money = 1000000;
        string dec = "\n " + Show_Color.Red(money) + " " + currency_unit.金币;
        dec += "\n " + common_items_list.Boss召唤卷轴 + "*" + Show_Color.Red(10);
        Color c = Tool_Battle.IsBuff(common_Buff.月卡) ? GameColors.Uncommon : GameColors.Common; 

        dec += Colorize("\n" + common_Buff.月卡 + "福利\n", c);
        dec += Colorize(enum_equip_entry_list.金币掉落 + "+20%\n", c);
        dec += Colorize(enum_equip_entry_list.经验加成 + "+20%\n", c);
        dec += Colorize(enum_equip_entry_list.怪物爆率 + "+5%\n", c);
        dec += Colorize("Boss刷新时间" + "-5%\n", c);
        dec += Colorize("提前查阅物品掉落属性\n", c);
        if (crt_vip != null)
        {
            dec += "\n" + crt_vip.vip_name + " 奖励\n";
            dec += Colorize(currency_unit.元宝 + " * " + (crt_vip.characterExperience * 20) + "\n", GameColors.Uncommon);
            dec += Colorize(common_items_list.Boss召唤卷轴 + " * " + crt_vip.characterExperience + "\n", GameColors.Uncommon);
            //dec += Colorize(common_Buff.双倍经验卷轴 + " * " + crt_vip.characterExperience + "\n", GameColors.Uncommon);
            dec += Colorize("灵宠 " + (pet_list)(crt_vip.vip_lv - 1) + "\n", GameColors.Uncommon);
        }
        else
        { 
            dec += Colorize("灵宠 " + pet_list.麋鹿 + "\n", GameColors.Uncommon);
        }

        info.text += dec;
    }

    private void Obtain_Vip()
    {
        crt_vip = Tool_Battle.Obtain_Vip();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }
}
