using CodeStage.AntiCheat.ObscuredTypes;
using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityColorPresets;
using Random = UnityEngine.Random;

public enum equip_job
{ 
不限,
战士,
法师,
道士
}
public enum equip_btn_list
{ 
穿戴,
出售,
锁定,
回收,
存入,
取出,
脱下,
分解
}

public class hero_equip_item : Base_Mono
{
    private TMP_Text title_name, show_base_need;

    private Transform m_info_brom,m_dream_bag_brom,m_btn_brom;

    private equip_type_info_item equip_type_info_item_prefab;

    private equip_show_info_item equip_show_info_item_prefab;

    private dream_BagItem dream_BagItem_prefab;

    private dream_BagItem data;

    private btn_item btn_item_prefab;

    private enum replace_state
    { 
    穿戴,
    取下,
    出售,
    }

    //private 
    private void Awake()
    {
        title_name=Find<TMP_Text>("title_name/info/info");
        show_base_need = Find<TMP_Text>("show_base_need/info");
        m_info_brom = Find<Transform>("info_brom/Scroll View/Viewport/Content");
        equip_type_info_item_prefab = Tool_UI.Find_Prefabs<equip_type_info_item>("equip_type_info_item");
        equip_show_info_item_prefab = Tool_UI.Find_Prefabs<equip_show_info_item>("equip_show_info_item");
        m_dream_bag_brom= Find<Transform>("show_icon");
        dream_BagItem_prefab = Tool_UI.Find_Prefabs<dream_BagItem>("dream_bagitem");
        m_btn_brom = Find<Transform>("btn_brom/Scroll View/Viewport/Content");
        btn_item_prefab = Tool_UI.Find_Prefabs<btn_item>("btn_item");
    }

    public void Init(dream_BagItem crt_data,Panel_BagType BagType)
    {
        data = crt_data;
        List<equip_btn_list> btn_list = new List<equip_btn_list>();
        ClearObject(m_btn_brom);

        switch (BagType)
        { 
            case Panel_BagType.装备:
                btn_list.Add(equip_btn_list.穿戴);
                btn_list.Add(equip_btn_list.出售);
                btn_list.Add(equip_btn_list.锁定);
                btn_list.Add(equip_btn_list.分解);
                break;
            case Panel_BagType.存入仓库:
                btn_list.Add(equip_btn_list.存入);
                break;
            case Panel_BagType.取出仓库:
                btn_list.Add(equip_btn_list.取出);
                break;
            case Panel_BagType.已装备:
                btn_list.Add(equip_btn_list.脱下); 
                break;

        }
        Init_btn(btn_list);
        int lv = 1;// int.Parse(data.Data.user_value.Split(' ')[2]);
        if(data.Data.user_value != null)lv= int.Parse(data.Data.user_value.Split(' ')[2]);
        title_name.text = "[" + (enum_equip_quality_list)(lv) + "]" + data.Data.Name;
        //title_name.color = Show_Color.Set_Color((Color_list)lv);
        show_base_need.text = "装备类型 " + data.Data.StdMode + "\n职业限定 " + (equip_job)data.Data.job + "\n需要等级 " + data.Data.need_lv;
        baseInfo();
    }

    private void Init_btn(List<equip_btn_list> btn_list)
    {
        
        for (int i = 0; i < btn_list.Count; i++)
        { 
            btn_item btn = Instantiate(btn_item_prefab, m_btn_brom);
            btn.Show((int)(btn_list[i]), btn_list[i]);
            btn.GetComponent<Button>().onClick.AddListener(() => { btn_click(btn); });
        }
    }
    /// <summary>
    /// 功能按钮
    /// </summary>
    /// <param name="equip_btn_list"></param>
    private void btn_click(btn_item btn)
    {
        switch ((equip_btn_list)btn.index)
        {
            case equip_btn_list.穿戴:
                OnWear();
                break;
            case equip_btn_list.出售:
                OnSell();
                break;
            case equip_btn_list.锁定:
                OnLock();
                break;
            case equip_btn_list.回收:
                break;
            case equip_btn_list.存入:
                deposit();
                break;
            case equip_btn_list.取出:
                takeout();
                break;
            case equip_btn_list.脱下:
                takeoff();
                break;
            case equip_btn_list.分解:
                decompose();
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 分解
    /// </summary>
    private void decompose()
    {
        string[] info_str = data.Data.user_value.Split(' ');
        int lv= int.Parse(info_str[2]);
        if (lv >= 7)
        {
            ObscuredInt number = 1;
            ObscuredInt random = Random.Range(1, 1000);
            ObscuredInt maxnumber = number + Random.Range(1, 1000);
            Battle_Tool.Dream_Obtain_Resources(Obtain_Int.Add(1, common_items_list.皇级碎片, new ObscuredInt[] { number + random, random }), maxnumber);
            Alert_Dec.Show("分解成功");
            SumSave.crt_bags.Remove_Bag_List(data.Data);
            Hide(true);
        }
        else
        {
            Alert_Dec.Show("装备等级不足，无法分解");
            return;
        }
         
        SumSave.crt_bags.MysqlData();
        Hide(true);
    }

    /// <summary>
    /// 取出
    /// </summary>
    private void takeout()
    {
        List<Bag_Base_VO> equips = SumSave.crt_equips.Get(Dream_User_Equip_Type.仓库);
        equips.Remove(data.Data);
        SumSave.crt_equips.Set(Dream_User_Equip_Type.仓库, equips);
        SumSave.crt_bags.Set_Bag_List(data.Data);
        Hide(true);
    }
    /// <summary>
    /// 存入
    /// </summary>
    private void deposit()
    {
        List<Bag_Base_VO> equips = SumSave.crt_equips.Get(Dream_User_Equip_Type.仓库);
        if (equips.Count >= SumSave.crt_equips.GetPage)
        {
            Alert_Dec.Show("仓库已满，无法存入");
            return;
        }
        equips.Add(data.Data);
        SumSave.crt_equips.MysqlData();
        List<Bag_Base_VO> bags = SumSave.crt_bags.Get_Bag_List();
        bags.Remove(data.Data);
        SumSave.crt_equips.Set(Dream_User_Equip_Type.仓库, equips);
        SumSave.crt_bags.Set_Bag_List(bags);
        Hide(true);
    }

    /// <summary>
    /// 锁定
    /// </summary>
    private void OnLock()
    {
        string[] info_str = data.Data.user_value.Split(' ');
        info_str[3] = info_str[3] == "1" ? "0" : "1";
        data.Data.user_value = Battle_Tool.Equip_User_Value(info_str);
        switch (info_str[3])
        {
            case "1": Alert_Dec.Show("装备锁定成功"); break;
            case "0": Alert_Dec.Show("装备解锁成功"); break;
            default:
                break;
        }
        SumSave.crt_bags.MysqlData();
        Hide(true);
    }

    /// <summary>
    /// 出售
    /// </summary>
    private void OnSell()
    {
        if (int.Parse(data.Data.user_value.Split(' ')[3]) == 0)
        {
            int moeny = data.Data.price;
            int sycee = 0;
            if (moeny > 0)
            {
                int lv = int.Parse(data.Data.user_value.Split(' ')[2]);
                moeny = moeny * lv / Enum.GetValues(typeof(enum_equip_quality_list)).Cast<int>().Max();
                if (moeny > 0)
                {
                    if (lv >= 5) sycee += data.Data.need_lv / 7 * (lv - 5) + 1;
                    Battle_Tool.Dream_Obtain_Unit(currency_unit.金币, moeny, Obtain_Int.Add_unit(moeny));
                    if (sycee > 0)
                        Battle_Tool.Dream_Obtain_Unit(currency_unit.元宝, sycee, Obtain_Int.Add_unit(sycee));
                    Alert.Show("一键出售", "出售成功,\n获得 " + currency_unit.金币 + " * " + moeny + "\n" + currency_unit.元宝 + " * " + sycee);
                    SumSave.crt_bags.Remove_Bag_List(data.Data);
                    Hide(true);
                }

            }
        }else Alert_Dec.Show("该装备已锁定，无法出售");
        
    }

    /// <summary>
    /// 脱下
    /// </summary>
    private void takeoff()
    {
        List<Bag_Base_VO> equips = SumSave.crt_equips.Get(Dream_User_Equip_Type.装备);
        List<Bag_Base_VO> bags = SumSave.crt_bags.Get_Bag_List();
        equips.Remove(data.Data);
        bags.Add(data.Data);
        Refresh(bags, equips);
        SendNotification(NotiList.Refresh_Max_Hero_Attribute);
        //transform.parent.parent.parent.parent.SendMessage("RefreshMain");
        Hide(true);
    }

    private void Refresh(List<Bag_Base_VO> bags, List<Bag_Base_VO> equips)
    {
        SumSave.crt_equips.Set(Dream_User_Equip_Type.装备, equips);
        SumSave.crt_bags.Set_Bag_List(bags);
    }

    private void OnWear()
    {
        if (data.Data.need_lv > SumSave.crtHero.lv && SumSave.crtHero.zs_lvs == 1)
        {
            Alert_Dec.Show("等级不足，无法装备");
            return;
        }
        if (!(data.Data.job == 0 || data.Data.job == SumSave.crtHero.job))
        {
            Alert_Dec.Show("职业不符，无法装备");
            return;
        }
        List<Bag_Base_VO> equips = SumSave.crt_equips.Get(Dream_User_Equip_Type.装备);
        List<Bag_Base_VO> bags = SumSave.crt_bags.Get_Bag_List();
        List<Bag_Base_VO> crt_bags = new List<Bag_Base_VO>();

        foreach (Bag_Base_VO item in equips)
        {
            if (item.StdMode == data.Data.StdMode) crt_bags.Add(item);
        }
        if (crt_bags.Count > 0)
        {
            if (data.Data.StdMode == Stditem_StdMode_List.戒指.ToString() || data.Data.StdMode == Stditem_StdMode_List.手镯.ToString())
            {
                if (crt_bags.Count >= 2)
                {
                    if (crt_bags.Count > 2)
                    {
                        //BUG将当前装备全部取下
                        for (int i = 0; i < crt_bags.Count; i++)
                        {
                            replace(bags, equips, crt_bags[i], replace_state.取下);
                        }
                    }
                    else
                    {
                        //替换
                        replace(bags, equips, crt_bags[0], replace_state.取下);
                    }
                }
                else
                {
                    //直接装备
                }
            }
            else
            {
                if (crt_bags.Count >= 1)
                {
                    if (crt_bags.Count > 1)
                    {
                        //BUG将当前装备全部取下
                        for (int i = 0; i < crt_bags.Count; i++)
                        {
                            replace(bags, equips, crt_bags[i], replace_state.取下);
                        }
                    }
                    else
                    {
                        //替换
                        replace(bags, equips, crt_bags[0], replace_state.取下);
                    }
                }
            }
        }
        replace(bags, equips, data.Data, replace_state.穿戴);
        Refresh(bags, equips);
        SendNotification(NotiList.Refresh_Max_Hero_Attribute);
        Hide(true);
    }
    /// <summary>
    /// 替换装备
    /// </summary>
    /// <param name="bags">背包</param>
    /// <param name="equips">穿戴</param>
    /// <param name="bag">当前选中装备</param>
    private void replace(List<Bag_Base_VO> bags, List<Bag_Base_VO> equips,Bag_Base_VO bag, replace_state state)
    {
        switch (state)
        {
            case replace_state.穿戴:
                bags.Remove(bag);
                equips.Add(bag);
                break;
            case replace_state.取下:
                bags.Add(bag);
                equips.Remove(bag);
                break;
            default:
                break;
        }
    }

    private void Hide(bool isRefresh=false)
    { 
        gameObject.SetActive(false);
        if (isRefresh)
        {
            transform.parent.parent.parent.parent.SendMessage("Refresh");
        }
    }

    /// <summary>
    /// 基础属性
    /// </summary>
    private void baseInfo()
    {
        ClearObject(m_info_brom);
        ClearObject(m_dream_bag_brom);
        Instantiate(dream_BagItem_prefab, m_dream_bag_brom).Data = data.Data;
        Color c = HexToColor("#ffffff");
        Get().Init(("[" + enum_equip_basetype_list.基础属性 + "]"), c);
        float hp_coefficient = 1f, mp_coefficient = 1f;
        int zl = SumSave.crtHero.zs_lvs - 1;//转生等级

        switch ((Hero_Type)SumSave.crtHero.job)
        {

            case Hero_Type.平民:
                hp_coefficient = 1f;
                mp_coefficient = 1f;
                break;
            case Hero_Type.战士:
                hp_coefficient = 1.8f + (zl * 0.3f);
                mp_coefficient = 0.5f + (zl * 0.1f);
                break;
            case Hero_Type.法师:
                hp_coefficient = 0.5f + (zl * 0.1f);
                mp_coefficient = 1.8f + (zl * 0.3f);
                break;
            case Hero_Type.道士:
                hp_coefficient = 1.2f + (zl * 0.2f);
                mp_coefficient = 1.2f + (zl * 0.2f);
                break;
            default:
                break;
        }

        if (data.Data.hp > 0)
        {
            int hp = (int)(data.Data.hp * hp_coefficient);
            equip_show_info_item itemValue = Instantiate(equip_show_info_item_prefab, m_info_brom);
            itemValue.Init(enum_equip_basetype_list.基础属性, enum_equip_entry_list.生命值, hp, c);
        }
        if (data.Data.mp > 0)
        {
            int mp = (int)(data.Data.mp * mp_coefficient);
            equip_show_info_item itemValue = Instantiate(equip_show_info_item_prefab, m_info_brom);
            itemValue.Init(enum_equip_basetype_list.基础属性, enum_equip_entry_list.魔法值, mp, c);
        }
        if (data.Data.ac > 0 || data.Data.ac2 > 0)
        {
            equip_show_info_item itemValue = Instantiate(equip_show_info_item_prefab, m_info_brom);
            string dec= data.Data.ac + " - " + data.Data.ac2;
            if (zl > 0&& data.Data.ac2>0)
            {
                dec += "[ + " + (data.Data.need_lv / 15 + 1) * zl + "]";
            }
            itemValue.Init(enum_equip_basetype_list.基础属性, enum_equip_entry_list.物理防御, dec, c);
        }
        if (data.Data.mac > 0 || data.Data.mac2 > 0)
        {
            equip_show_info_item itemValue = Instantiate(equip_show_info_item_prefab, m_info_brom);
            string dec = data.Data.mac + " - " + data.Data.mac2;
            if (zl > 0 && data.Data.mac2 > 0)
            {
                dec += "[ + " + (data.Data.need_lv / 15 + 1) * zl + "]";
            }
            itemValue.Init(enum_equip_basetype_list.基础属性, enum_equip_entry_list.魔法防御, dec, c);
        }
        if (data.Data.dc > 0 || data.Data.dc2 > 0)
        {
            equip_show_info_item itemValue = Instantiate(equip_show_info_item_prefab, m_info_brom);
            string dec = data.Data.dc + " - " + data.Data.dc2;
            if (zl > 0 && data.Data.dc2 > 0)
            {
                dec += "[ + " + (data.Data.need_lv / 10 + 1) * zl + "]";
            }
            itemValue.Init(enum_equip_basetype_list.基础属性, enum_equip_entry_list.物理攻击, dec, c);
        }
        if (data.Data.mc > 0 || data.Data.mc2 > 0)
        {
            equip_show_info_item itemValue = Instantiate(equip_show_info_item_prefab, m_info_brom);
            string dec = data.Data.mc + " - " + data.Data.mc2;
            if (zl > 0 && data.Data.mc2 > 0)
            {
                dec += "[ + " + (data.Data.need_lv / 10 + 1) * zl + "]";
            }
            itemValue.Init(enum_equip_basetype_list.基础属性, enum_equip_entry_list.魔法攻击, dec, c);
        }
        if (data.Data.sc > 0 || data.Data.sc2 > 0)
        {
            equip_show_info_item itemValue = Instantiate(equip_show_info_item_prefab, m_info_brom);
            string dec = data.Data.sc + " - " + data.Data.sc2;
            if (zl > 0 && data.Data.sc2 > 0)
            {
                dec += "[ + " + (data.Data.need_lv / 10 + 1) * zl + "]";
            }
            itemValue.Init(enum_equip_basetype_list.基础属性, enum_equip_entry_list.道术攻击, dec, c);
        }
        if (data.Data.user_value != null)
        {
            string[] info = data.Data.user_value.Split(' ');
            int lucky = int.Parse(info[1]);
            int quilty = int.Parse(info[2]);
            int islock = int.Parse(info[3]);
            if (lucky > 1 &&( data.Data.StdMode == equip_type_list.武器.ToString() || data.Data.StdMode == equip_type_list.项链.ToString()))
            {
                c = GameColors.Legendary;
                Get().Init(("[幸运] + " + (lucky - 1)), c);
            }
            int refined_number = 0;
            if (info.Length >= 5)
            {
                //类型
                string[] arr2 = info[4].Split('X');
                List<equip_show_info_item> talents = new List<equip_show_info_item>();
                for (int i = 0; i < arr2.Length; i++)
                {
                    if (arr2[i].Length > 0)
                    {
                        switch ((enum_equip_basetype_list)i)
                        {
                            case enum_equip_basetype_list.基础属性:
                                c = HexToColor("#ffffff");
                                break;
                            case enum_equip_basetype_list.附加属性:
                                c = HexToColor("#70ff69");
                                Get().Init(("[" + enum_equip_basetype_list.附加属性 + "]"), c);
                                break;
                            case enum_equip_basetype_list.元素属性:
                                c = HexToColor("#00ffff");
                                Get().Init(("[" + enum_equip_basetype_list.元素属性 + "]"), c);
                                break;
                            case enum_equip_basetype_list.铭文属性:
                                c = HexToColor("#ffff00");
                                Get().Init(("[" + enum_equip_basetype_list.铭文属性 + "]"), c);
                                break;
                            default:
                                break;
                        }
                        string[] entry = arr2[i].Split('|');
                        for (int j = 0; j < entry.Length; j++)
                        {
                            string[] entry_arr = entry[j].Split(',');
                            if (entry_arr.Length > 1)
                            {
                                equip_show_info_item itemValue = Instantiate(equip_show_info_item_prefab, m_info_brom);
                                enum_equip_entry_list e = (enum_equip_entry_list)int.Parse(entry_arr[0]);
                                int value = int.Parse(entry_arr[1]);
                                switch (e)
                                {
                                    case enum_equip_entry_list.生命值:
                                    case enum_equip_entry_list.魔法值:
                                        itemValue.Init((enum_equip_basetype_list)i, e, " " + value * 5, c);
                                        break;
                                    case enum_equip_entry_list.物理防御:
                                    case enum_equip_entry_list.魔法防御:
                                    case enum_equip_entry_list.物理攻击:
                                    case enum_equip_entry_list.魔法攻击:
                                    case enum_equip_entry_list.道术攻击:
                                        itemValue.Init((enum_equip_basetype_list)i, e, "0 - " + value, c);
                                        break;
                                    case enum_equip_entry_list.每秒回血:
                                    case enum_equip_entry_list.每秒回蓝:
                                    case enum_equip_entry_list.真实伤害:
                                    case enum_equip_entry_list.吸收伤害:
                                    case enum_equip_entry_list.幸运:
                                    case enum_equip_entry_list.命中:
                                    case enum_equip_entry_list.闪避:
                                        itemValue.Init((enum_equip_basetype_list)i, e, " " + value, c);
                                        break;
                                    case enum_equip_entry_list.物理下防:
                                        e = enum_equip_entry_list.物理防御;
                                        itemValue.Init((enum_equip_basetype_list)i, e, value + " - 0", c);
                                        break;
                                    case enum_equip_entry_list.魔法下防:
                                        e = enum_equip_entry_list.魔法防御;
                                        itemValue.Init((enum_equip_basetype_list)i, e, value + " - 0", c);
                                        break;
                                    case enum_equip_entry_list.物理下攻:
                                        e = enum_equip_entry_list.物理攻击;
                                        itemValue.Init((enum_equip_basetype_list)i, e, value + " - 0", c);
                                        break;
                                    case enum_equip_entry_list.魔法下攻:
                                        e = enum_equip_entry_list.魔法攻击;
                                        itemValue.Init((enum_equip_basetype_list)i, e, value + " - 0", c);
                                        break;
                                    case enum_equip_entry_list.道术下攻:
                                        e = enum_equip_entry_list.道术攻击;
                                        itemValue.Init((enum_equip_basetype_list)i, e, value + " - 0", c);
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
                                        itemValue.Init((enum_equip_basetype_list)i, e, value + " %", c);
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
                                        itemValue.Init((enum_equip_basetype_list)i, e, "Lv." + value, c);
                                        break;
                                    case enum_equip_entry_list.洗炼次数:
                                        itemValue.gameObject.SetActive(false);
                                        refined_number = value;
                                        break;
                                    default:
                                        if ((int)e >= 1000)//附加技能
                                        {
                                            if ((int)e >= 2000)//天赋
                                            {
                                                string talent = "";
                                                talent = ArrayHelper.Find(SumSave.db_pet_talents, x => x.pet_talent_id == (((int)e) - 2000)).pet_talent_name;
                                                itemValue.Init_7(talent, value);
                                                talents.Add(itemValue);
                                            }
                                            else
                                            {
                                                if (e != enum_equip_entry_list.洗炼次数)
                                                {
                                                    string skill_name = "";
                                                    skill_name = ArrayHelper.Find(SumSave.db_skills, x => x.id == (((int)e) - 1000)).show_name;
                                                    itemValue.Init(skill_name, value);
                                                }
                                            }
                                        }
                                        break;
                                }

                            }
                        }

                    }
                }

                if (quilty >= 7)
                {
                    refined_number = 5 - refined_number;
                    if (refined_number <= 0) refined_number = 0;
                    c = HexToColor("#FFFFFF");
                    Get().Init(("[可洗炼次数+ " + (refined_number)+ "]"), c);
                }
                //宝石
                if (info.Length >= 6)
                {
                    c= GameColors.PhysicalDamage;
                    Get().Init("[宝石属性]", c);
                    //宝石
                    List<string> gem = ArrayHelper.Get_Split<string>(info[5], 'X');
                    for (int i = 0; i < gem.Count; i++)
                    {
                        if (gem[i] != "")
                        {
                            List<string> gem_value = ArrayHelper.Get_Split<string>(gem[i], '|');
                            if (gem_value.Count == 2)
                            {
                                if (gem_value[1] != "0")
                                {
                                    Bag_Base_VO gem_data = ArrayHelper.Find(SumSave.db_stditems, x => x.Name == gem_value[1]);
                                    if (gem_data != null)
                                    {
                                        Show_Info_Base(gem_data, gem_data.Shape == int.Parse(gem_value[0]));
                                    }
                                }
                                else
                                {
                                    Bag_Base_VO gem_data = ArrayHelper.Find(SumSave.db_stditems, x => x.StdMode == "材料" && x.Shape == int.Parse(gem_value[0]));

                                    if (gem_data != null)
                                    {
                                        c = GameColors.Common;
                                        Get().Init(("[最优宝石]  " + gem_data.Name), c);
                                    }
                                }
                            }
                        }
                    }
                }
                //皇权
                if (talents.Count > 0)
                {
                    c = HexToColor("#fa5151");
                    Get().Init(("[" + enum_equip_basetype_list.皇权属性 + "]"), c);
                    for (int i = 0; i < talents.Count; i++)
                    {
                        talents[i].transform.SetAsLastSibling();
                    }
                }
            }
          
        }
        if (data.Data.suit > 0)
        {
            db_suit_vo suit = ArrayHelper.Find(SumSave.db_suits, e => e.suit_type == data.Data.suit);
            if (suit != null)
            {            
                //套装
                int number = 0;
                List<Bag_Base_VO> equips = SumSave.crt_equips.Get(Dream_User_Equip_Type.装备);
                foreach (var item in equips)
                {
                    if (item.suit == data.Data.suit) number++;
                }
                c = HexToColor("#ffa9fe");
                Get().Init(("[" + suit.suit_name + "]"), c);
                for (int i = 0; i < suit.suit_list.Count; i++)
                {
                    c = HexToColor("#ffd8ab");
                    if (suit.suit_list[i].Item1 > number)
                    { 
                        c = Color.gray;
                    }
                    Get().Init(Info_Suit(suit.suit_list[i]), c);
                }
            }
        }
    }

    private void Show_Info_Base(Bag_Base_VO data,bool is_gem)
    {
        Color c = is_gem ? GameColors.Legendary : GameColors.Healthy;
        equip_show_info_item itemValue = Instantiate(equip_show_info_item_prefab, m_info_brom);
        string dec="";
        if (data.hp > 0)
        {
            dec += enum_equip_entry_list.生命值 + ":" + (data.hp + (is_gem ? 10 : -10));
            //itemValue.Init(enum_equip_basetype_list.铭文属性, enum_equip_entry_list.生命值, (data.hp + (is_gem ? 10 : -10)), c);
        }
        if (data.mp > 0)
        {
            dec += enum_equip_entry_list.魔法值 + ":" + (data.mp + (is_gem ? 5 : -5));
            //itemValue.Init(enum_equip_basetype_list.铭文属性, enum_equip_entry_list.魔法值, data.mp + (is_gem ? 5 : -5), c);
        }
        if (data.ac > 0 || data.ac2 > 0)
        {
            dec += enum_equip_entry_list.物理防御 + ":" + (data.ac + " - " + (data.ac2 + (is_gem ? 1 : -1)));
            //itemValue.Init(enum_equip_basetype_list.铭文属性, enum_equip_entry_list.物理防御, data.ac + " - " + (data.ac2 + (is_gem ? 1 : -1)), c);
        }
        if (data.mac > 0 || data.mac2 > 0)
        {
            dec += enum_equip_entry_list.魔法防御 + ":" + (data.mac + " - " + (data.mac2 + (is_gem ? 1 : -1)));
            //itemValue.Init(enum_equip_basetype_list.铭文属性, enum_equip_entry_list.魔法防御, data.mac + " - " + (data.mac2 + (is_gem ? 1 : -1)), c);
        }
        if (data.dc > 0 || data.dc2 > 0)
        {
            dec += enum_equip_entry_list.物理攻击 + ":" + (data.dc + " - " + (data.dc2 + (is_gem ? 1 : -1)));
            //itemValue.Init(enum_equip_basetype_list.铭文属性, enum_equip_entry_list.物理攻击, data.dc + " - " + (data.dc2 + (is_gem ? 1 : -1)), c);
        }
        if (data.mc > 0 || data.mc2 > 0)
        {
            dec += enum_equip_entry_list.魔法攻击 + ":" + (data.mc + " - " + ((data.mc2 + (is_gem ? 1 : -1))));
            //itemValue.Init(enum_equip_basetype_list.铭文属性, enum_equip_entry_list.魔法攻击, data.mc + " - " + ((data.mc2 + (is_gem ? 1 : -1))), c);
        }
        if (data.sc > 0 || data.sc2 > 0)
        {
            dec += enum_equip_entry_list.道术攻击 + ":" + (data.sc + " - " + (data.sc2 + (is_gem ? 1 : -1)));
            //itemValue.Init(enum_equip_basetype_list.铭文属性, enum_equip_entry_list.道术攻击, data.sc + " - " + (data.sc2 + (is_gem ? 1 : -1)), c);
        }
        itemValue.Init(data.Name, dec, c);
    }
    /// <summary>
    /// 生成套装信息
    /// </summary>
    /// <param name="suit"></param>
    /// <returns></returns>
    private string Info_Suit((int,int,int) suit)
    {
        string value = "(" + suit.Item1 + "件) " + (Suit_Type)suit.Item2;
        switch ((Suit_Type)suit.Item2)
        {
            case Suit_Type.物理攻击:
            case Suit_Type.魔法攻击:
            case Suit_Type.道术攻击:
            case Suit_Type.双防:
                value += " : " + (suit.Item3 - 1) + " - " + suit.Item3;
                break;
            case Suit_Type.生命:
            case Suit_Type.伤害吸收:
            case Suit_Type.真实伤害:
                value += " : " + suit.Item3;
                break;
            case Suit_Type.攻击速度:
            case Suit_Type.物攻属性:
            case Suit_Type.魔攻属性:
            case Suit_Type.道攻属性:
            case Suit_Type.双防属性:
                value += " : " + suit.Item3 + " %";
                break;
            case Suit_Type.魔法:
                break;
            case Suit_Type.经验加成:
            case Suit_Type.金币加成:
                value += " : " + suit.Item3 + " %";
                break;
        }
        return value;
    }
    /// <summary>
    /// 生成装备类型
    /// </summary>
    /// <returns></returns>
    private equip_type_info_item Get()
    {
        return Instantiate(equip_type_info_item_prefab, m_info_brom);
    }
}
