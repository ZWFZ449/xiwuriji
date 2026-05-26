using CodeStage.AntiCheat.ObscuredTypes;
using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class offect_house : Base_Mono
{

    private enum Offect_House_State
    { 
    背包扩容,
    仓库扩容,
    合成金条,
    兑换元宝,
    }
    private Transform m_btn_brom;

    private btn_item btn_item_prefab;

    private Offect_House_State crt_btn;
    private void Awake()
    {
        m_btn_brom = Find<Transform>("btn_list/Viewport/Content");
        btn_item_prefab = Tool_UI.Find_Prefabs<btn_item>("btn_item");
        init();
    }

    private void init()
    {
        ClearObject(m_btn_brom);
        for (int i = 0; i < Enum.GetNames(typeof(Offect_House_State)).Length; i++)
        {
            btn_item btn_item = Instantiate(btn_item_prefab, m_btn_brom);
            btn_item.Show(i, (Offect_House_State)i);
            btn_item.GetComponent<Button>().onClick.AddListener(() => SelectBtn(btn_item));
        }
    }

    private void SelectBtn(btn_item btn_item)
    {
        crt_btn= (Offect_House_State)btn_item.index;
        switch (crt_btn)
        {
            case Offect_House_State.背包扩容:
                int bag = SumSave.crt_bags.Get_Page;
                if (bag >= 360)
                {
                    Alert_Dec.Show("背包格已满");
                    return;
                }
                else
                {
                    ObscuredLong moeny = 0;
                    moeny = ((bag - 120) / 10 + 2) * 1000000;
                    Alert.Show("背包扩容", "是否花费" + Battle_Tool.FormatNumberToChineseUnit(moeny) + currency_unit.金币 + "扩容背包", confirm_bag, moeny);
                }
                break;
            case Offect_House_State.仓库扩容:
                int house = SumSave.crt_equips.GetPage;
                if (house >= 180)
                {
                    Alert_Dec.Show("仓库格已满");
                    return;
                }
                else
                {
                    ObscuredLong moeny = 0;
                    moeny = ((house - 60) / 10 + 1) * 1000;
                    Alert.Show("仓库扩容", "是否花费" + Battle_Tool.FormatNumberToChineseUnit(moeny) + currency_unit.元宝 + "扩容仓库", confirm_house, moeny);
                }
                break;
            case Offect_House_State.合成金条:
                ObscuredLong needmoeny = 13000000;
                Alert.Show("合成金条", "是否花费" + Battle_Tool.FormatNumberToChineseUnit(needmoeny) + currency_unit.金币 + "合成\n金条 * 10", Synthetic_gold, needmoeny);

                break;
            case Offect_House_State.兑换元宝:
                ObscuredLong need_moeny = 13000000;
                Alert.Show("兑换元宝", "是否花费" + Battle_Tool.FormatNumberToChineseUnit(need_moeny) + currency_unit.金币 + "兑换\n元宝 * 1000", Synthetic_sycee, need_moeny);
                break;
            default:
                break;
        }
    }

    private void Synthetic_sycee(object arg0)
    {
        ObscuredLong moeny = (ObscuredLong)arg0;
        if (moeny < 0) return;
        Clear_Condition();
        Need_Condition(currency_unit.金币, moeny);
        if (Return_Condition())
        {
            ObscuredLong number = 1000;
            Battle_Tool.Dream_Obtain_Unit(currency_unit.元宝, number, Obtain_Int.Add_unit(number));
            Alert_Dec.Show("获得 " + currency_unit.元宝 + " * " + number);
        }
        else Alert_Dec.Show("兑换失败");
    }

    private void Synthetic_gold(object arg0)
    {
        ObscuredLong moeny = (ObscuredLong)arg0;
        if (moeny < 0) return;
        Clear_Condition();
        Need_Condition(currency_unit.金币, moeny);
        if (Return_Condition())
        {
            string path = "金条";
            ObscuredInt number = 10;
            ObscuredInt random = Random.Range(1, 1000);
            ObscuredInt maxnumber = number + Random.Range(1, 1000);
            Alert_Dec.Show("获得 " + path + " * " + number);
            Battle_Tool.Dream_Obtain_Resources(Obtain_Int.Add(1, path, new ObscuredInt[] { number + random, random }), maxnumber);
        }
        else Alert_Dec.Show("合成失败");
    }

    private void confirm_house(object arg0)
    {
        ObscuredLong moeny = (ObscuredLong)arg0;
        if (moeny < 0) return;
        Clear_Condition();
        Need_Condition(currency_unit.元宝, moeny);
        if (Return_Condition())
        {
            SumSave.crt_equips.SetPage(SumSave.crt_equips.GetPage + 1);
            Alert_Dec.Show("仓库扩容成功");
        }
        else Alert_Dec.Show("仓库扩容失败");
    }

    /// <summary>
    /// 确认背包扩容
    /// </summary>
    /// <param name="arg0"></param>
    private void confirm_bag(object arg0)
    {
        ObscuredLong moeny = (ObscuredLong)arg0;
        if (moeny < 0) return;
        Clear_Condition();
        Need_Condition(currency_unit.金币, moeny);
        if (Return_Condition())
        {
            SumSave.crt_bags.Set_Page(SumSave.crt_bags.Get_Page + 1);
            Alert_Dec.Show("背包扩容成功");
        }
        else Alert_Dec.Show("背包扩容失败");
    }

    private void OnEnable()
    {
        if (SumSave.crtHero.lv <= 30)
        {
            Alert_Dec.Show("合成功能在10级开放");
            Hide();
        }
    }
    private void Hide()
    {
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }

}
