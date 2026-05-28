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
using Random = UnityEngine.Random;

public class offect_blacksmith : Base_Mono
{
    private enum blacksmith_type
    { 
    鉴定装备,
    幸运祝福,
    镶嵌宝石,
    拆卸宝石,
    幸运转移,
    洗炼装备,
    }

    private TMP_Text info, need_info;

    private Transform m_btn_brom, m_bags_brom,m_show_brom;

    private btn_item btn_item_prefab;

    private dream_BagItem dream_BagItem_prefab;

    private Button confirm;

    private Bag_Base_VO crt_bag;

    private blacksmith_type crt_type;

    private panel_hero_equip m_panel_hero_equip;

    private blacksmith_gem crt_gem;
    private void Awake()
    {
        m_btn_brom = Find<Transform>("btn_list/Viewport/Content");
        m_bags_brom = Find<Transform>("bag_list/Viewport/Content");
        btn_item_prefab = Tool_UI.Find_Prefabs<btn_item>("btn_item");
        dream_BagItem_prefab = Tool_UI.Find_Prefabs<dream_BagItem>("dream_BagItem");
        m_show_brom= Find<Transform>("icon");
        confirm = Find<Button>("confirm");
        confirm.onClick.AddListener(() => Confirm());
        info= Find<TMP_Text>("title_type_info/info");
        need_info = Find<TMP_Text>("need_info");
        m_panel_hero_equip = UI_Manager.I.GetPanel<panel_hero_equip>();
        crt_gem = Find<blacksmith_gem>("blacksmith_gem");
        Init();
    }

    private void Confirm()
    {
        if (crt_bag == null) { Alert_Dec.Show("请放入装备");return; }
        SendNotification(NotiList.Read_Mysql_Base_Time);
        if (SumSave.openMysql)
        {
            Alert_Dec.Show("网络连接失败");
            return;
        }
        Clear_Condition();
        switch (crt_type)
        {
            case blacksmith_type.鉴定装备:
                if (crt_bag.user_value != null)
                {
                    string[] info = crt_bag.user_value.Split(' ');
                    if (info.Length >= 6)
                    {
                        Need_Condition(common_items_list.鉴定符, 10);
                    }
                    else
                        Need_Condition(common_items_list.鉴定符, 2);
                    if (Return_Condition())
                    {
                        appraisal();
                        Update_Info(true);
                        SelectBagItem(crt_bag);
                        Game_Omphalos.i.archive();
                    }
                    else Alert_Dec.Show("资源不足");
                }
                break;
            case blacksmith_type.幸运祝福:
                if (crt_bag.user_value != null)
                {
                    string[] info = crt_bag.user_value.Split(' ');
                    int lucky = int.Parse(info[1]);
                    if (lucky >= 8)
                    {
                        Alert_Dec.Show("装备幸运已满");
                        return;
                    }
                    need_info.text = "幸运 " + lucky + "  需求 " + common_items_list.祝福油 + " * " + need_number[lucky];
                    Need_Condition(common_items_list.祝福油, need_number[lucky]);
                    if (Return_Condition())
                    {
                        info[1] = (lucky + 1).ToString();
                        crt_bag.user_value = Battle_Tool.Equip_User_Value(info);
                        Update_Info(true);
                        SelectBagluckyItem(crt_bag);
                    }
                    else Alert_Dec.Show("资源不足"); 
                }
                else
                {
                    Alert_Dec.Show("当前装备不可附加幸运");
                }
                break;
            case blacksmith_type.镶嵌宝石:
            case blacksmith_type.拆卸宝石:
                crt_gem.gameObject.SetActive(true);
                crt_gem.Show(crt_bag);
                break;
            case blacksmith_type.幸运转移:
                SelecttransferBagluckyItem();
                break;
            case blacksmith_type.洗炼装备:
                Need_Condition(currency_unit.Boss积分, 100);
                if (Return_Condition())
                {
                    refined_equip();
                    Update_Info(true);
                    SelectBagItem(crt_bag);
                    Game_Omphalos.i.archive();
                }
                break;
        }
    }
    /// <summary>
    /// 洗炼装备
    /// </summary>
    private void refined_equip()
    {
        string[] info = crt_bag.user_value.Split(' ');
        string user_value = Tool_Battle.Obtain_Equip(crt_bag, 1, int.Parse(info[2]));
        string[] user_values= user_value.Split(' ');
        string[] info_list= info[4].Split('X');
        string[] user_value_list = user_values[4].Split('X');
        string result = "";
        for (int i = 0; i < user_value_list.Length; i++)
        {
            bool exist = true;
            string[] entry = user_value_list[i].Split('|');
            for (int j = 0; j < entry.Length; j++)
            {
                string[] entry_arr = entry[j].Split(',');
                if (entry_arr.Length > 1)
                {
                    if (int.Parse(entry_arr[0]) >= 1000)
                    {
                        exist = false;
                        break;
                    }
                }
            }
            if (exist) result += (result == "" ? "" : "X") + user_value_list[i];
        }
        for (int i = 0; i < info_list.Length; i++)
        {
            bool exist = false;
            string[] entry = info_list[i].Split('|');
            for (int j = 0; j < entry.Length; j++)
            {
                string[] entry_arr = entry[j].Split(',');
                if (entry_arr.Length > 1)
                {
                    if (int.Parse(entry_arr[0]) >= 1000)
                    {
                        exist = true;
                        break;
                    }

                }
            }
            if (exist) result += "X" + info_list[i];
        }
        info[4] = result;
        crt_bag.user_value = Battle_Tool.Equip_User_Value(info);
    }

    private List<int> appraisal_list;
    /// <summary>
    /// 鉴定
    /// </summary>
    private void appraisal() 
    {
        List<int> list = new List<int>() { 1000, 500, 100 };
        int number = Tool_Battle.Obtain_WeightedItem(list);
        if (appraisal_list == null)
        { 
            appraisal_list = new List<int>();
            for (int i = 0; i < SumSave.db_stditems.Count; i++)
            {
                if (SumSave.db_stditems[i].StdMode == Stditem_StdMode_List.材料.ToString())
                {
                    if (SumSave.db_stditems[i].Shape != 0)
                    {
                        if (!appraisal_list.Contains(SumSave.db_stditems[i].Shape))
                        { 
                            appraisal_list.Add(SumSave.db_stditems[i].Shape);
                        }
                    }
                }
            }
        }
        string value= "";
        for (int i = 0; i < number; i++)
        { 
            int random = appraisal_list[Random.Range(0, appraisal_list.Count)];
            value += (value == "" ? "" : "X") + random + "|0";
        }
        //value += "";
        if (crt_bag.user_value != null)
        {
            string[] info = crt_bag.user_value.Split(' ');
            if (info.Length >= 6)
            {
                info[5] = value;
                crt_bag.user_value = Battle_Tool.Equip_User_Value(info);
            }
            else
            { 
               List<string> info_list = new List<string>();
                for (int i = 0; i < info.Length; i++)
                { 
                    info_list.Add(info[i]);
                }
                info_list.Add(value);
                crt_bag.user_value = string.Join(" ", info_list.ToArray());
            }
        }
    }
    /// <summary>
    /// 刷新
    /// </summary>
    /// <param name="exist"></param>
    private void Update_Info(bool exist = false)
    {
        SumSave.crt_bags.MysqlData();
        Base_Show(crt_type);
        if (exist)
        {
            dream_BagItem bagItem = new dream_BagItem();
            bagItem.Data = crt_bag;
            m_panel_hero_equip.Show();
            m_panel_hero_equip.Select_Bag(bagItem, Panel_BagType.展示);
        }
    }

    protected void refresh_gem(Bag_Base_VO bag)
    {
        SelectBagItem(bag);
        Base_Show(crt_type);

    }
    private void OnEnable()
    {
        if (SumSave.crtHero.lv < 30)
        {
            Alert_Dec.Show("铁匠铺功能在30级开放");
            Hide();
        }
    }

    private void Init()
    {
        ClearObject(m_btn_brom);
        for (int i = 0; i < Enum.GetNames(typeof(blacksmith_type)).Length; i++)
        {
            btn_item btn_item = Instantiate(btn_item_prefab, m_btn_brom);
            btn_item.Show(i, (blacksmith_type)i);
            btn_item.GetComponent<Button>().onClick.AddListener(() => SelectBtn(btn_item));
        }
    }
    /// <summary>
    /// 选择按钮
    /// </summary>
    /// <param name="btn_item"></param>
    private void SelectBtn(btn_item btn_item)
    {
        info.text = (blacksmith_type)btn_item.index + "";
        crt_type = (blacksmith_type)btn_item.index;
        Base_Show((blacksmith_type)btn_item.index);
    }

    private void Base_Show(blacksmith_type index)
    {
        ClearObject(m_bags_brom);
        ClearObject(m_show_brom);
        List<Bag_Base_VO> baglist = SumSave.crt_bags.Get_Bag_List();
        switch (index)
        {
            case blacksmith_type.鉴定装备:
            case blacksmith_type.洗炼装备:
                for (int i = 0; i < baglist.Count; i++)
                {
                    if (baglist[i].need_lv >= 30)
                    {
                        if (baglist[i].user_value != null)
                        {
                            string[] info = baglist[i].user_value.Split(' ');
                            int lv = int.Parse(info[2]);
                            if (lv >= 6)
                            {
                                dream_BagItem bagItem = Instantiate(dream_BagItem_prefab, m_bags_brom);
                                bagItem.Data = baglist[i];
                                bagItem.GetComponent<Button>().onClick.AddListener(() => SelectBagItem(bagItem.Data));
                            }
                        }
                    }
                }
                break; 
            case blacksmith_type.幸运祝福:
                for (int i = 0; i < baglist.Count; i++)
                {
                    if (baglist[i].need_lv >= 30 && baglist[i].StdMode == equip_type_list.武器.ToString())
                    {
                        dream_BagItem bagItem = Instantiate(dream_BagItem_prefab, m_bags_brom);
                        bagItem.Data = baglist[i];
                        bagItem.GetComponent<Button>().onClick.AddListener(() => SelectBagluckyItem(bagItem.Data));
                    }
                }
                break;
            case blacksmith_type.镶嵌宝石:
            case blacksmith_type.拆卸宝石:
                need_info.text = "选择装备进行宝石操作";
                for (int i = 0; i < baglist.Count; i++)
                {
                    if (baglist[i].need_lv >= 30)
                    {
                        if (baglist[i].user_value != null)
                        {
                            string[] info = baglist[i].user_value.Split(' ');
                            if (info.Length >= 6)
                            {
                                dream_BagItem bagItem = Instantiate(dream_BagItem_prefab, m_bags_brom);
                                bagItem.Data = baglist[i];
                                bagItem.GetComponent<Button>().onClick.AddListener(() => SelectBagGemItem(bagItem.Data));
                            }
                        }
                    }
                }
                break;
            case blacksmith_type.幸运转移:
                need_info.text = "幸运转移消耗2000元宝";
                List<Bag_Base_VO> equips = SumSave.crt_equips.Get(Dream_User_Equip_Type.装备);
                for (int i= 0; i < equips.Count; i++)
                {
                    if (equips[i].StdMode == equip_type_list.武器.ToString())
                    {
                        for (int j= 0; j < baglist.Count; j++) 
                        {
                            if (baglist[j].StdMode == equips[i].StdMode && baglist[j].need_lv == equips[i].need_lv)
                            {
                                if (baglist[j].user_value != null)
                                {
                                    string[] info = baglist[j].user_value.Split(' ');
                                    int lv = int.Parse(info[2]);
                                    if (lv >= 6)
                                    {
                                        dream_BagItem bagItem = Instantiate(dream_BagItem_prefab, m_bags_brom);
                                        bagItem.Data = baglist[j];
                                        bagItem.GetComponent<Button>().onClick.AddListener(() => Select_Bag(bagItem.Data));
                                    }
                                }
                            }
                        }
                        return;
                    }
                }
                Alert_Dec.Show("当前没有可以幸运转移的装备");
                break;
        }
    } 
    /// <summary>
    /// 幸运转移
    /// </summary>
    /// <param name="bag_Base_VO">穿戴</param>
    /// <param name="data">背包装备</param>
    private void SelecttransferBagluckyItem( )
    {
        List<Bag_Base_VO> equips = SumSave.crt_equips.Get(Dream_User_Equip_Type.装备);
        for (int i = 0; i < equips.Count; i++)
        {
            if (equips[i].StdMode == crt_bag.StdMode && equips[i].need_lv == crt_bag.need_lv)
            {
                Bag_Base_VO equip = equips[i];
                int lucky = 1;
                if (equip.user_value != null)
                {
                    Clear_Condition();
                    Need_Condition(currency_unit.元宝, 2000);
                    if (Return_Condition())
                    {
                        string[] equipinfo = equip.user_value.Split(' ');
                        string[] baginfo = crt_bag.user_value.Split(' ');
                        lucky = int.Parse(equipinfo[1]);
                        equipinfo[1] = baginfo[1];
                        equip.user_value = Battle_Tool.Equip_User_Value(equipinfo);
                        baginfo[1] = lucky.ToString();
                        crt_bag.user_value = Battle_Tool.Equip_User_Value(baginfo);
                        Update_Info(true);
                        Alert_Dec.Show("幸运转移成功");
                        return;
                    }
                    else Alert_Dec.Show("元宝不足");
                }else Alert_Dec.Show("当前装备不可以进行幸运转移");
                return;
            }
        }
        Alert_Dec.Show("当前装备不可以进行幸运转移");
    }

    /// <summary>
    /// 强化幸运
    /// </summary>
    /// <param name="bagItem"></param>
    private void SelectBagluckyItem(Bag_Base_VO bagItem)
    {
        Select_Bag(bagItem);
        if (crt_bag.user_value != null)
        {
            string[] info = crt_bag.user_value.Split(' ');
            int lucky = int.Parse(info[1]); 
            if (lucky >= 8)
            {
                this.info.text = "幸运已满";
                Alert_Dec.Show("装备幸运已满");
                return;
            }
            need_info.text = "幸运 " + lucky + "  需求 " + common_items_list.祝福油 + " * " + need_number[lucky];
        }
        else
        {
            Alert_Dec.Show("当前装备不可附加幸运");
        }
    }

    /// <summary>
    /// 幸运祝福需求
    /// </summary>
    private List<int> need_number = new List<int>() { 0, 10, 30, 60, 90, 120, 150, 200, 300, 500, 10000, 20000 };
    /// <summary>
    /// 选择背包物品
    /// </summary>
    /// <param name="bagItem"></param>
    private void SelectBagItem(Bag_Base_VO bagItem)
    {
        Select_Bag(bagItem);
        if (crt_bag.user_value != null)
        {
            string[] info = crt_bag.user_value.Split(' ');
            switch (crt_type)
            {
                case blacksmith_type.鉴定装备:
                    if (info.Length >= 6)
                    {
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
                                            Alert.Show("鉴定提醒", "当前装备已镶嵌宝石,进行鉴定将导致宝石损坏,请合理安排");
                                            break;
                                        }
                                    }
                                    else
                                    {
                                       
                                    }
                                }
                            }
                        }
                        need_info.text = "需求" + common_items_list.鉴定符 + " *  10";
                    }
                    else
                        need_info.text = "需求" + common_items_list.鉴定符 + " *  2";
                    break;
                case blacksmith_type.洗炼装备:
                    need_info.text = "需求" + currency_unit.Boss积分 + " *  100";
                    break;
            }
           
        }
    }

    private void SelectBagGemItem(Bag_Base_VO bagItem)
    {
        Select_Bag(bagItem);
        need_info.text = "选择装备进行宝石操作";

    }

    private void Select_Bag(Bag_Base_VO bagItem)
    {
        crt_bag = bagItem;
        ClearObject(m_show_brom);
        Instantiate(dream_BagItem_prefab, m_show_brom).Data = crt_bag;
    }

    private void Need()
    {
        //data.Data.user_value = Battle_Tool.Equip_User_Value(info_str);
        string[] info = crt_bag.user_value.Split(' ');
        switch (info[3])
        {
            case "1": Alert_Dec.Show("装备锁定成功"); break;
            case "0": Alert_Dec.Show("装备解锁成功"); break;
            default:
                break;
        }
        SumSave.crt_bags.MysqlData();
        if (info.Length >= 5)
        {
            if (info.Length >= 6)
            { 
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
                                    //Show_Info_Base(gem_data.Shape == int.Parse(gem_value[0]));
                                }
                            }
                            else
                            {
                                Bag_Base_VO gem_data = ArrayHelper.Find(SumSave.db_stditems, x => x.StdMode == "材料" && x.Shape == int.Parse(gem_value[0]));

                                //if (gem_data != null)
                                //{
                                //    c = GameColors.Common;
                                //    Get().Init(("[最优宝石] + " + gem_data.Name), c);
                                //}
                            }
                        }
                    }
                }
            }
        }
    }

    private void Hide()
    {
        crt_bag = null;
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }
}
