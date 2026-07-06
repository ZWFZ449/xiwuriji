using CodeStage.AntiCheat.ObscuredTypes;
using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using static UnityColorPresets;
using Random = UnityEngine.Random;


/// <summary>
/// 转生
/// </summary>
public class offect_reincarnation : Base_Mono
{
    private enum zs_unit
    { 
      转生,
      炼药,
      炼体,
      炼气,
      炼神,
    }
    private Button reset_talent, crate_talent;

    private Transform m_btn_brom;

    private btn_item btn_item_prefab;

    private TMP_Text info;

    private Image offect;

    private TMP_Dropdown m_drop_down;

    private TMP_Text offect_info,offect_need;

    private Button close_offect;

    private Button confirm_crate_btn;
    /// <summary>
    /// 当前选中
    /// </summary>
    private zs_unit crt_zs_unit;
    /// <summary>
    /// 获取列表
    /// </summary>
    private List<string> offect_list;
    private void Awake()
    {
        reset_talent = Find<Button>("reset_list/reset_talent");
        crate_talent = Find<Button>("reset_list/crate_talent");
        reset_talent.onClick.AddListener(ResetTalent);
        crate_talent.onClick.AddListener(CrateTalent);
        info =Find<TMP_Text>("Scroll View/Viewport/Content/info");
        btn_item_prefab = Tool_UI.Find_Prefabs<btn_item>("btn_item");
        m_btn_brom = Find<Transform>("btn_list/Viewport/Content");
        offect = Find<Image>("offect");
        m_drop_down = Find<TMP_Dropdown>("offect/bg/result_Dropdown");
        offect_info = Find<TMP_Text>("offect/bg/info");
        close_offect = Find<Button>("offect/close_button");
        close_offect.onClick.AddListener(() => { offect.gameObject.SetActive(false); });
        confirm_crate_btn = Find<Button>("offect/bg/confirm_crate_btn");
        confirm_crate_btn.onClick.AddListener(() => { confirm_crate(); });
        offect_need = Find<TMP_Text>("offect/bg/need");
    }
    /// <summary>
    /// 初始化按钮
    /// </summary>
    private void confirm_crate()
    {
        string need_currency = "重置内容"+Show_Color.Red(offect_list[m_drop_down.value]);
        switch (crt_zs_unit)
        {
            case zs_unit.转生:
                break;
            case zs_unit.炼药:
                need_currency += "\n需消耗" + (10000 * SumSave.crtHero.zs_lvs) + "" + currency_unit.元宝 + "\n" + (5000 * SumSave.crtHero.zs_lvs) + "" + currency_unit.转生积分;
                break;
            case zs_unit.炼体:
                need_currency = "\n需消耗" + (10000 * SumSave.crtHero.zs_lvs) + "" + currency_unit.元宝;
                break;
            case zs_unit.炼气:
                break;
            case zs_unit.炼神:
                break;
        }
        Alert.Show("重置"+ offect_list[m_drop_down.value], need_currency+"\n是否重置", Confirm_crate);

    }

    private void Confirm_crate(object arg0)
    {
        Clear_Condition();
        switch (crt_zs_unit)
        {
            case zs_unit.转生:
                break;
            case zs_unit.炼药:
                Need_Condition(currency_unit.元宝, (10000 * SumSave.crtHero.zs_lvs));
                Need_Condition(currency_unit.转生积分, (5000 * SumSave.crtHero.zs_lvs));
                if (Return_Condition())
                { 
                    if(m_drop_down.value < SumSave.crt_zs.crt_medicine.Count)
                        SumSave.crt_zs.crt_medicine[m_drop_down.value] = 0;
                    SumSave.crt_zs.MysqlData();
                    offect.gameObject.SetActive(false);
                    medicineinit();
                    Alert_Dec.Show("重置成功");
                }else Alert_Dec.Show("重置失败");
                break;
            case zs_unit.炼体:
                Need_Condition(currency_unit.元宝, (10000 * SumSave.crtHero.zs_lvs));
                if (Return_Condition())
                { 
                    if (m_drop_down.value < SumSave.crt_zs.crt_Refinement.Count)
                        SumSave.crt_zs.crt_Refinement[m_drop_down.value] = 0;
                    SumSave.crt_zs.MysqlData();
                    offect.gameObject.SetActive(false);
                    RefinementInit();
                    Alert_Dec.Show("重置成功");
                }else Alert_Dec.Show("重置失败");
                break;
            case zs_unit.炼气:
                break;
            case zs_unit.炼神:
                break;
        }
    }

    /// <summary>
    /// 重置
    /// </summary>
    private void CrateTalent()
    {
        switch (crt_zs_unit)
        {
            case zs_unit.转生:
                Alert.Show("重置转生", "是否重置转生,转生后清空全部炼药炼体加成", confirm_Crate_zs);
                break;
            case zs_unit.炼药:
            case zs_unit.炼体:
                offect_show();
                break;
        }
    }

    private void offect_show()
    {
        offect_list = new List<string>();
        offect.gameObject.SetActive(true);
        offect_info.text = crt_zs_unit + "重置";
        switch (crt_zs_unit)
        {
            case zs_unit.转生:
                break;
            case zs_unit.炼药:
                for (int i = 0; i < Enum.GetNames(typeof(medicine_type)).Length; i++)
                {
                    offect_list.Add(Enum.GetNames(typeof(medicine_type))[i]);
                    if(i== Enum.GetNames(typeof(medicine_type)).Length-1)
                        offect_list.Add(Enum.GetNames(typeof(medicine_type))[i]);
                }

                offect_need.text = "重置指定内容需消耗" + (10000 * SumSave.crtHero.zs_lvs) + "" + currency_unit.元宝 + "\n" + (5000 * SumSave.crtHero.zs_lvs) + "" + currency_unit.转生积分;
                break;
            case zs_unit.炼体:
                for (int i = 0; i < Enum.GetNames(typeof(Refinement_type)).Length; i++)
                {
                    offect_list.Add(Enum.GetNames(typeof(Refinement_type))[i]);
                    if (i == Enum.GetNames(typeof(Refinement_type)).Length - 1)
                        offect_list.Add(Enum.GetNames(typeof(Refinement_type))[i]);
                }
                offect_need.text = "重置指定内容需消耗" + (10000 * SumSave.crtHero.zs_lvs) + "" + currency_unit.元宝;
                break;
            case zs_unit.炼气:
                break;
            case zs_unit.炼神:
                break;
        }
        m_drop_down.ClearOptions();
        m_drop_down.AddOptions(offect_list);

    }

    private void confirm_Crate_zs(object arg0)
    {
        SumSave.crtHero.zs_lvs = 1;
        SumSave.crt_zs.zs_medicine_max = 0;
        SumSave.crt_zs.zs_Refinement_max = 0;
        SumSave.crt_zs.crt_medicine.Clear();
        SumSave.crt_zs.crt_Refinement.Clear();
        SumSave.crt_zs.MysqlData();
        SendNotification(NotiList.Refresh_Max_Hero_Attribute);
        SumSave.crtHero.MysqlData();
        UI_Manager.I.GetPanel<PanelMian>().Show();
        Alert.Show("重置转生成功", "请重启游戏");
        Game_Omphalos.i.archive();
        UI_Manager.I.GetPanel<PanelBattle>().Close();
    }

    private void OnEnable()
    {
        InitBtn();
        is_Anto = true;
        if (SumSave.crtHero.lv < 30 && SumSave.crtHero.zs_lvs == 1)
        {
            Hide();
            Alert_Dec.Show("当前等级不足30级无法查看转生");
            return;
        }
    }
    private void init()
    {
        string dec = "";
        for (int i = 0; i < SumSave.db_reincarnation_list.Count; i++)
        {
            if (SumSave.db_reincarnation_list[i].reincarnation_lv == SumSave.crtHero.zs_lvs)
            {
                db_reincarnation_vo vo = SumSave.db_reincarnation_list[i];
                dec += vo.reincarnation_name + "\n";
                dec += "最低转生等级 " + vo.need_lv + " - 最高转生等级 " + vo.need_maxLv;
                for (int j = 0; j < vo.reincarnation_need.Count; j++)
                {
                    List<string> need = ArrayHelper.Get_Split<string>(vo.reincarnation_need[j], ' ');
                    if (need.Count == 3)
                    {
                        switch (int.Parse(need[0]))
                        {
                            case 1://需要等级
                                dec += Show_Color.Set_String("转生次数 " + need[1] + " Lv." + need[2], GameColors.ManaBar);
                                break;
                           case 2://需要需求
                                dec += Show_Color.Set_String("\n" + need[1] + " * " + need[2], GameColors.ManaBar);
                                break;
                           case 3://需要技能
                                dec += Show_Color.Set_String("\n" + need[2] + " " + ((currency_unit)int.Parse(need[1])), GameColors.ManaBar);
                                break;
                            default:
                                break;
                        }
                    } 
                }
                if (SumSave.crtHero.zs_lvs > 1) dec += Show_Color.Red("\n炼体炼药满级可以继续转生");
                dec += Show_Color.Red("\n怪物增强,谨慎转生"); 
                dec += "\n转生收益\n";
                dec += Show_Color.Set_String("炼体 + " + vo.result_minRefinement + " - " + vo.result_maxRefinement, GameColors.RageBar);
                dec += "\n" + Show_Color.Set_String("炼药 + " + vo.result_minmedicine + " - " + vo.result_maxmedicine, GameColors.RageBar);
                dec += "\n转生加成\n";
                for (int j = 0; j < vo.reincarnation_cost.Count; j++)
                {
                    enum_equip_entry_list e = vo.reincarnation_cost[j].Item1;
                    int value = vo.reincarnation_cost[j].Item2;
                    switch (e)
                    {
                        case enum_equip_entry_list.生命值:
                        case enum_equip_entry_list.魔法值:

                            dec += Show_Color.Set_String(e + " : " + value * 5, GameColors.EnergyBar);
                            break;
                        case enum_equip_entry_list.物理防御:
                        case enum_equip_entry_list.魔法防御:
                        case enum_equip_entry_list.物理攻击:
                        case enum_equip_entry_list.魔法攻击:
                        case enum_equip_entry_list.道术攻击:
                            dec+= Show_Color.Set_String(e + " : " + "0 - " + value, GameColors.EnergyBar);
                            break;
                        case enum_equip_entry_list.每秒回血:
                        case enum_equip_entry_list.每秒回蓝:
                        case enum_equip_entry_list.真实伤害:
                        case enum_equip_entry_list.吸收伤害:
                        case enum_equip_entry_list.幸运:
                        case enum_equip_entry_list.命中:
                        case enum_equip_entry_list.闪避:
                            dec+= Show_Color.Set_String(e + " : " + value, GameColors.EnergyBar);
                            break;
                        case enum_equip_entry_list.物理下防:
                            e = enum_equip_entry_list.物理防御;
                            dec+= Show_Color.Set_String(e + " : " + value + " - 0", GameColors.EnergyBar);
                            break;
                        case enum_equip_entry_list.魔法下防:
                            e = enum_equip_entry_list.魔法防御;
                            dec+= Show_Color.Set_String(e + " : " + value + " - 0", GameColors.EnergyBar);
                            break;
                        case enum_equip_entry_list.物理下攻:
                            e = enum_equip_entry_list.物理攻击;
                            dec+= Show_Color.Set_String(e + " : " + value + " - 0", GameColors.EnergyBar);
                            break;
                        case enum_equip_entry_list.魔法下攻:
                            e = enum_equip_entry_list.魔法攻击;
                            dec+= Show_Color.Set_String(e + " : " + value + " - 0", GameColors.EnergyBar);
                            break;
                        case enum_equip_entry_list.道术下攻:
                            e = enum_equip_entry_list.道术攻击;
                            dec+= Show_Color.Set_String(e + " : " + value + " - 0", GameColors.EnergyBar);
                            break;
                        case enum_equip_entry_list.生命属性:
                        case enum_equip_entry_list.魔法属性:
                        case enum_equip_entry_list.防御属性:
                        case enum_equip_entry_list.魔防属性:
                        case enum_equip_entry_list.物攻属性:
                        case enum_equip_entry_list.魔攻属性:
                        case enum_equip_entry_list.道攻属性:
                        case enum_equip_entry_list.攻击速度:
                        case enum_equip_entry_list.攻击范围:
                        case enum_equip_entry_list.暴击属性:
                        case enum_equip_entry_list.暴击伤害:
                        case enum_equip_entry_list.物伤减免:
                        case enum_equip_entry_list.魔伤减免:
                        case enum_equip_entry_list.怪物爆率:
                        case enum_equip_entry_list.极品爆率:
                        case enum_equip_entry_list.经验加成:
                        case enum_equip_entry_list.金币掉落:
                            dec+= Show_Color.Set_String(e + " : " + value + " %", GameColors.EnergyBar);
                            break;
                        case enum_equip_entry_list.烈阳文:
                        case enum_equip_entry_list.盾护文:
                        case enum_equip_entry_list.守月文:
                        case enum_equip_entry_list.幽狼文:
                        case enum_equip_entry_list.神行文:
                        case enum_equip_entry_list.怒目文:
                        case enum_equip_entry_list.震火文:
                        case enum_equip_entry_list.金刚文:
                        case enum_equip_entry_list.大愈文:
                        case enum_equip_entry_list.回春文:
                        case enum_equip_entry_list.回心文:
                        case enum_equip_entry_list.峰芒文:
                        case enum_equip_entry_list.破枪文:
                        case enum_equip_entry_list.深寒文:
                        case enum_equip_entry_list.瑶光文:
                            dec+= Show_Color.Set_String(e + " : Lv." + value, GameColors.EnergyBar);
                            break;
                        default:
                            if ((int)e >= 1000)//附加技能
                            {
                                string skill_name = "";
                                skill_name = ArrayHelper.Find(SumSave.db_skills, x => x.id == (((int)e) - 1000)).show_name;
                                dec += Show_Color.Set_String(skill_name + " : Lv." + value, GameColors.EnergyBar);
                            }
                            break;
                    }
                    dec += "\n";
                }
            }
        }
        if (dec == "")
        {
            dec += "当前无可用转生";
        }
        info.text= dec;
    }
    private void RefinementInit()
    {
        string dec = "";
        dec += "炼体加成";
        int max = 0;
        for (int i = 0; i < SumSave.crt_zs.crt_Refinement.Count; i++) max += SumSave.crt_zs.crt_Refinement[i];
        dec += "\n当前可获取最大永久属性 " + max + " / " + SumSave.crt_zs.zs_Refinement_max;
        dec += "\n炼体加成\n";
        for (int i = 0; i < Enum.GetNames(typeof(Refinement_type)).Length; i++)
        {
            int value = i < SumSave.crt_zs.crt_Refinement.Count ? SumSave.crt_zs.crt_Refinement[i] : 0;
            switch ((Refinement_type)(i))
            {
                case Refinement_type.生命值:
                    value = value * 10;
                    break;
                case Refinement_type.魔法值:
                    value = value * 10;
                    break;
                case Refinement_type.物理防御:
                    break;
                case Refinement_type.魔法防御:
                    break;
                case Refinement_type.攻击:
                    break;
                case Refinement_type.每秒回血:
                    break;
                case Refinement_type.每秒回蓝:
                    break;
                case Refinement_type.真实伤害:
                    break;
                case Refinement_type.吸收伤害:
                    break;
            }
            dec += Show_Color.Set_String((Refinement_type)(i) + " + " + value + "", GameColors.ManaBar);
            dec += "\n";
        }
        info.text = dec;
    }
    /// <summary>
    /// 炼药
    /// </summary>
    private void medicineinit()
    {
        string dec = "";
        dec += "炼药成功率100%\n获得永久属性概率1%";
        int max = 0;
        for (int i = 0; i < SumSave.crt_zs.crt_medicine.Count; i++) max+= SumSave.crt_zs.crt_medicine[i];
        dec += "\n当前可获取最大永久属性 " + max + " / " + SumSave.crt_zs.zs_medicine_max;
        dec += "\n炼药加成\n";
        for (int i = 0; i < Enum.GetNames(typeof(medicine_type)).Length; i++)
        {
            string value = (i < SumSave.crt_zs.crt_medicine.Count ? SumSave.crt_zs.crt_medicine[i] : 0) + "";
            switch ((medicine_type)(i))
            {
                case medicine_type.命中:
                    break;
                case medicine_type.闪避:
                    break;
                case medicine_type.生命属性:
                case medicine_type.魔法属性:
                case medicine_type.防御属性:
                case medicine_type.魔防属性:
                case medicine_type.物攻属性:
                case medicine_type.魔攻属性:
                case medicine_type.道攻属性:
                case medicine_type.暴击属性:
                case medicine_type.暴击伤害:
                case medicine_type.怪物爆率:
                    if ((medicine_type)(i) == medicine_type.暴击伤害)
                    { 
                        value=(int.Parse(value) * 10+"");
                    }
                    value += "%";
                    break;
                case medicine_type.怪物刷新个数:
                    value += "个";
                    break;
            }
            dec += Show_Color.Set_String((medicine_type)(i) + " + " + value + "", GameColors.ManaBar);
            dec+= "\n";
        }
        info.text = dec;
    }

    private void InitBtn()
    {
        ClearObject(m_btn_brom);
        int max = Mathf.Min(Enum.GetNames(typeof(zs_unit)).Length, SumSave.crtHero.zs_lvs == 2 ? 3 : SumSave.crtHero.zs_lvs);
        for (int i = 0; i < max; i++)
        {
            btn_item item = Instantiate(btn_item_prefab, m_btn_brom);
            item.Show(i, (zs_unit)(i));
            item.GetComponent<Button>().onClick.AddListener(() => { SelectJob(item); });
            if(i== 0) SelectJob(item);
        }
    }

    private void SelectJob(btn_item item)
    {
        crt_zs_unit = (zs_unit)item.index;
        switch ((zs_unit)item.index)
        {
            case zs_unit.转生:
                init();
                break;
            case zs_unit.炼药:
                medicineinit();
                break;
            case zs_unit.炼体:
                RefinementInit();
                break;
            case zs_unit.炼气:
                break;
            case zs_unit.炼神:
                break;
        }
    }
    /// <summary>
    /// 转生
    /// </summary>
    private void ResetTalent()
    {
        switch (crt_zs_unit)
        { 
           case zs_unit.转生:
                confirm_zs();
                break;
           case zs_unit.炼药:
                confirm_medicine();
                break;
           case zs_unit.炼体:
                confirm_refinement();
                break;
        }
    }
    /// <summary>
    /// 转生
    /// </summary>
    private void confirm_zs()
    {
        for (int i = 0; i < SumSave.db_reincarnation_list.Count; i++)
        {
            if (SumSave.db_reincarnation_list[i].reincarnation_lv == SumSave.crtHero.zs_lvs)
            {
                db_reincarnation_vo vo = SumSave.db_reincarnation_list[i];
                if (SumSave.crtHero.lv >= vo.need_lv)
                {
                    int base_lv = Mathf.Min(vo.need_maxLv, SumSave.crtHero.lv);
                    int value = base_lv - vo.need_lv;
                    int refinement = (vo.result_maxRefinement - vo.result_minRefinement) * value / (vo.need_maxLv - vo.need_lv) + vo.result_minRefinement;
                    if (refinement > vo.result_maxRefinement) refinement = vo.result_maxRefinement;
                    int medicine = (vo.result_maxmedicine - vo.result_minmedicine) * value / (vo.need_maxLv - vo.need_lv) + vo.result_minmedicine;
                    if (medicine > vo.result_maxmedicine) medicine = vo.result_maxmedicine;
                    Alert.Show("转生收益", "当前等级转生可获得\n炼体上限 " + refinement + "\n炼药上限" + medicine + "\n转生后需要重启游戏", Confirm_zs);
                }else Alert_Dec.Show("等级不足");
            }
        }

    }

    private void Confirm_zs(object arg0)
    {
        for (int i = 0; i < SumSave.db_reincarnation_list.Count; i++)
        {
            if (SumSave.db_reincarnation_list[i].reincarnation_lv == SumSave.crtHero.zs_lvs)
            {
                db_reincarnation_vo vo = SumSave.db_reincarnation_list[i];
                if (SumSave.crtHero.lv >= vo.need_lv)
                {
                    Clear_Condition();
                    if (SumSave.crtHero.zs_lvs > 1)
                    {
                        int max = 0;
                        for (int j = 0; j < SumSave.crt_zs.crt_Refinement.Count; j++) max += SumSave.crt_zs.crt_Refinement[j];
                        int basemax = (int)MathF.Min(300 * ((SumSave.crtHero.zs_lvs - 1)), SumSave.crt_zs.zs_Refinement_max);
                        if (max < basemax)
                        {
                            Alert_Dec.Show("炼体不足");
                            return;
                        }
                        max = 0;
                        for (int j = 0; j < SumSave.crt_zs.crt_medicine.Count; j++) max += SumSave.crt_zs.crt_medicine[j];
                        basemax = (int)MathF.Min(30 * ((SumSave.crtHero.zs_lvs - 1)), SumSave.crt_zs.zs_medicine_max);
                        if (max < basemax)
                        { 
                            Alert_Dec.Show("炼药不足");
                            return;
                        }
                    }
                    for (int j = 0; j < vo.reincarnation_need.Count; j++)
                    {
                        List<string> need = ArrayHelper.Get_Split<string>(vo.reincarnation_need[j], ' ');
                        if (need.Count == 3)
                        {
                            switch (int.Parse(need[0]))
                            {
                                case 1://需要等级
                                    break;
                                case 2://需要需求
                                    VIP_Need_Condition(need[1], int.Parse(need[2]));
                                    //Need_Condition(need[1], int.Parse(need[2]));
                                    break;
                                case 3://需要技能
                                    VIP_Need_Condition((currency_unit)int.Parse(need[1]), int.Parse(need[2]));
                                    //Need_Condition((currency_unit)int.Parse(need[1]), int.Parse(need[2]));
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    //转生
                    if (Return_Condition())
                    {
                        int base_lv = Mathf.Min(vo.need_maxLv, SumSave.crtHero.lv);
                        int value = base_lv - vo.need_lv;
                        int refinement = (vo.result_maxRefinement - vo.result_minRefinement) * value / (vo.need_maxLv - vo.need_lv) + vo.result_minRefinement;
                        if (refinement > vo.result_maxRefinement) refinement = vo.result_maxRefinement;
                        int medicine = (vo.result_maxmedicine - vo.result_minmedicine) * value / (vo.need_maxLv - vo.need_lv) + vo.result_minmedicine;
                        if (medicine > vo.result_maxmedicine) medicine = vo.result_maxmedicine;
                        SumSave.crt_zs.zs_medicine_max += medicine;
                        SumSave.crt_zs.zs_Refinement_max += refinement;
                        SumSave.crt_zs.MysqlData();
                        SumSave.crtHero.zs_lvs++;
                        SumSave.crtHero.lv = 30;
                        SumSave.crtHero.exp = 0;
                        SendNotification(NotiList.Refresh_Max_Hero_Attribute);
                        SumSave.crtHero.MysqlData();
                        UI_Manager.I.GetPanel<PanelMian>().Show();
                        Alert_Dec.Show("转生成功");
                        Alert_Dec.Show("开启炼药成功");
                        Alert_Dec.Show("开启炼体成功");
                        Alert.Show("转生成功","请重启游戏");
                        Game_Omphalos.i.archive();
                        UI_Manager.I.GetPanel<PanelBattle>().Close();
                    }
                    else Alert_Dec.Show("转生失败");
                }
            }
        }
    }

    private void VIP_Need_Condition(object arg0, int number)
    {
        db_vip crt_vip = Tool_Battle.Obtain_Vip();
        if (crt_vip != null)
        {
            if (crt_vip.strengthenCosts > 0)
            { 
                number= (int)(number * (100-crt_vip.strengthenCosts)/100);
                number = Mathf.Max(1, number);
            }
        }
        Need_Condition(arg0, number);
    }

    /// <summary>
    /// 强化炼体
    /// </summary>
    private void confirm_refinement()
    {
        int max = 0,basemax = 0;
        for (int i = 0; i < SumSave.crt_zs.crt_Refinement.Count; i++) max += SumSave.crt_zs.crt_Refinement[i];
        if (max >= SumSave.crt_zs.zs_Refinement_max)
        {
            Alert_Dec.Show("当前属性已满");
            return;
        }
        string dec = "";
        for (int i = 0; i < SumSave.db_reincarnation_list.Count; i++)
        {
            if (SumSave.crtHero.zs_lvs > SumSave.db_reincarnation_list[i].reincarnation_lv)
            {
                basemax += SumSave.db_reincarnation_list[i].result_maxRefinement;
            }
        }
        if (SumSave.crt_zs.zs_Refinement_max > basemax)
        {
            if (max > (SumSave.crt_zs.zs_Refinement_max - basemax))
            {
                if (max > basemax)//大于基准值 维持在基准值
                {
                    max -= (SumSave.crt_zs.zs_Refinement_max - basemax);
                    max = (int)MathF.Max(basemax, max);
                }
                else//小于基准值 维持在当前值
                {
                    max -= (SumSave.crt_zs.zs_Refinement_max - basemax);
                    max = (int)MathF.Max((SumSave.crt_zs.zs_Refinement_max - basemax), max);
                }
            }
        }
        max = (int)MathF.Max(0, max);
        //max = (int)MathF.Min(150 * ((SumSave.crtHero.zs_lvs - 1)), max);
        int number = (max / 20 + 1) * 10;
        if(SumSave.crtHero.zs_lvs ==2)
        dec += "强化锻体需要\n" + number + "黑铁矿石" + "\n" + (number * 2) + "金条";
        else
        if (SumSave.crtHero.zs_lvs == 3)
        dec += "强化锻体需要\n" + (number/2) + common_items_list.转生石 + "\n" + number + common_items_list.黑铁精矿 + "\n" + (number * 2) + common_items_list.金条;
        if (SumSave.crtHero.zs_lvs == 4)
            dec += "强化锻体需要\n" + (number) + common_items_list.转生石 + "\n" + (number * 2) + common_items_list.黑铁精矿 + "\n" + (number * 4) + common_items_list.金条;
        Alert.Show(crt_zs_unit.ToString(), dec, Confirm_refinement, number);
    }
    /// <summary>
    /// 确认强化锻体
    /// </summary>
    /// <param name="arg0"></param>
    private void Confirm_refinement(object arg0)
    {
        int number= (int)arg0;
        Clear_Condition();
        switch (SumSave.crtHero.zs_lvs)
        {
            case 2:
                VIP_Need_Condition(common_items_list.金条, number * 2);
                VIP_Need_Condition(common_items_list.黑铁矿石, number);
                break;
            case 3:
                VIP_Need_Condition(common_items_list.转生石, number / 2);
                VIP_Need_Condition(common_items_list.黑铁精矿, number);
                Need_Condition(common_items_list.金条, number * 2);
                break;
            case 4:
                VIP_Need_Condition(common_items_list.转生石, number);
                VIP_Need_Condition(common_items_list.黑铁精矿, number * 2);
                Need_Condition(common_items_list.金条, number * 4);
                break;
            default:
                Need_Condition("未开放", 99999);
                break;
        }
        if (Return_Condition())
        {
            Refinement_type item = EnumExtensions.GetRandomEnum<Refinement_type>();
            while ((int)item >= SumSave.crt_zs.crt_Refinement.Count)
            {
                SumSave.crt_zs.crt_Refinement.Add(0);
            }
            SumSave.crt_zs.crt_Refinement[(int)item]++;
            SumSave.crt_zs.crt_Refinement[(int)item] = (int)MathF.Min(SumSave.crt_zs.zs_Refinement_max / 5, SumSave.crt_zs.crt_Refinement[(int)item]);
            Alert_Dec.Show("恭喜获得永久属性 " + item);
            SumSave.crt_zs.MysqlData();
            RefinementInit();
        }
        else
        {
            Alert_Dec.Show("条件不足");
        }
    }
    int number = 0;
    /// <summary>
    /// 强化炼药
    /// </summary>
    private void confirm_medicine()
    {
        int max = 0;
        for (int i = 0; i < SumSave.crt_zs.crt_medicine.Count; i++) max += SumSave.crt_zs.crt_medicine[i];
        string dec = "";
        switch (SumSave.crtHero.zs_lvs)
        {
            case 2:
                dec += "炼药需要" + common_items_list.人参 + " * 10" + "\n" + common_items_list.金条 + " * 1";

                break;
            case 3:
                dec += "炼药需要" + common_items_list.人参精华 + " * 1" + "\n" + common_items_list.金条 + " * 10";
                break;
            case 4:
                dec += "炼药需要" + common_items_list.人参精华 + " * 5" + "\n" + common_items_list.金条 + " * 30";
                break;
            default:
                break;
        }
        if (number >= 5) { 
            dec += "\n自动开启连续炼药,连续10次\n点击取消 继续单次炼药";
            Alert.Show(crt_zs_unit.ToString(), dec, Anto_Confirm_medicine, max, Confirm_medicine,false);

        }
        else
        Alert.Show(crt_zs_unit.ToString(), dec, Confirm_medicine, max);
       
    }

    private bool is_Anto = true;
    /// <summary>
    /// 自动确认炼药
    /// </summary>
    /// <param name="arg0"></param>
    private void Anto_Confirm_medicine(object arg0)
    {
        if (!is_Anto) { Alert_Dec.Show("炼药中,请等待");return; }
        StartCoroutine(Game_BossTime((int)arg0));

    }
    private IEnumerator Game_BossTime(int number)
    {
        is_Anto = false;
        int max = 10;
        while (max > 0)
        {
            max--;
            Alert_Dec.Show("自动炼药中");
            Confirm_medicine(number);
            yield return new WaitForSeconds(1f);
        }
        Alert_Dec.Show("炼药结束");
        is_Anto = true;
    }


    private void Confirm_medicine(object arg0)
    {
        number++;
        int max = (int)arg0;
        SumSave.crt_zs.medicine_exp++;
        bool eixst=true;
        Clear_Condition();
        switch (SumSave.crtHero.zs_lvs)
        {
            case 2:
                VIP_Need_Condition(common_items_list.人参, 10);
                VIP_Need_Condition(common_items_list.金条, 1);
                //Need_Condition(common_items_list.人参, 10);
                //Need_Condition(common_items_list.金条, 1);
                break;
            case 3:
                VIP_Need_Condition(common_items_list.人参精华, 1);
                VIP_Need_Condition(common_items_list.金条, 10);
                //Need_Condition(common_items_list.人参精华, 1);
                //Need_Condition(common_items_list.金条, 10);
                break;
            case 4:
                VIP_Need_Condition(common_items_list.人参精华, 5);
                VIP_Need_Condition(common_items_list.金条, 30);
                break;
            default:
                Need_Condition("未开放", 99999);
                break;
        }
        if (!Return_Condition())
        {
            Alert_Dec.Show("条件不足");
            return;
        }
        if (max < SumSave.crt_zs.zs_medicine_max)
        {
            if (Random.Range(0, 100) < 1)
            {
                eixst = false;
                //成功
                medicine_type item = EnumExtensions.GetRandomEnum<medicine_type>();
                while ((int)item >= SumSave.crt_zs.crt_medicine.Count)
                {
                    SumSave.crt_zs.crt_medicine.Add(0);
                }
                if (item == medicine_type.怪物刷新个数)
                {
                    item = medicine_type.命中;
                }
                SumSave.crt_zs.crt_medicine[(int)item]++;
                if (SumSave.crt_zs.crt_medicine[(int)item] > SumSave.crt_zs.zs_medicine_max / 3)
                {
                    Alert_Dec.Show("恭喜获得永久属性 " + item + " 当前属性已满");
                    SumSave.crt_zs.crt_medicine[(int)item] = SumSave.crt_zs.zs_medicine_max / 3;
                }
                else
                    Alert_Dec.Show("恭喜获得永久属性 " + item);
                medicineinit();
            }
        }
        if (eixst)
        {
            List<Bag_Base_VO> list = ArrayHelper.FindAll(SumSave.db_stditems, e => e.StdMode == Stditem_StdMode_List.消耗品.ToString());
            Bag_Base_VO item = list[Random.Range(0, list.Count)];
            ObscuredInt number = 100;
            ObscuredInt random = Random.Range(1, 1000);
            ObscuredInt maxnumber = number + Random.Range(1, 1000);
            Alert_Dec.Show("恭喜获得 " + item.Name + " * " + number);
            Battle_Tool.Dream_Obtain_Resources(Obtain_Int.Add(1, item.Name, new ObscuredInt[] { number + random, random }), maxnumber);
        }
        SumSave.crt_zs.MysqlData();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }
}
