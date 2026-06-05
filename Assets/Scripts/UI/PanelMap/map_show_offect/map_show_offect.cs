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
using Random = UnityEngine.Random;

public class map_show_offect : Base_Mono 
{
    private Button close;
    /// <summary>
    /// 显示地图信息
    /// </summary>
    private TMP_Text base_info;

    private Transform m_drop_borm, m_map_intensity_borm;

    private dream_BagItem dream_BagItem_Prefabs;
    /// <summary>
    /// 选中物品
    /// </summary>
    private base_map_item crt_map;
    /// <summary>
    /// 确认进入地图
    /// </summary>
    private Button map_confirm;

    private panel_hero_equip panel_hero_equip;
    /// <summary>
    /// 点击地图强度
    /// </summary>
    private select_map_lv_item select_map_lv_item_Prefab;
    /// <summary>
    /// 是否重复显示
    /// </summary>
    private Dictionary<string, int> drop_dic = new Dictionary<string, int>();
    private void Awake()
    {
        close=Find<Button>("close_button");
        base_info=Find<TMP_Text>("baseinfo/info");
        close.onClick.AddListener(()=> { Hide(); });
        m_map_intensity_borm = Find<Transform>("map_intensity_borm");
        select_map_lv_item_Prefab = Tool_UI.Find_Prefabs<select_map_lv_item>("select_map_lv_item");
        dream_BagItem_Prefabs = Tool_UI.Find_Prefabs<dream_BagItem>("dream_BagItem");
        m_drop_borm = Find<Transform>("dropList/Scroll View/Viewport/Content");
        map_confirm= Find<Button>("map_confirm");
        map_confirm.onClick.AddListener(()=> { OnClickEnterMap(); });
        panel_hero_equip = UI_Manager.I.GetPanel<panel_hero_equip>();
    }

    /// <summary>
    /// 点击进入地图
    /// </summary>
    private void OnClickEnterMap()
    {
        if (crt_map.GetMap().map_type != 0)
        {
            if (SumSave.crt_signin.GetIsValue(crt_map.GetMap().map_name) == 0)
            {
                SumSave.crt_signin.SetIsValue(crt_map.GetMap().map_name, 1);
            }
            else
            {
                Alert_Dec.Show("今日进入次数已满");
                return;
            }
        }
        else
        {
            if (SumSave.crtHero.lv < crt_map.GetMap().map_lv)
            { 
                Alert_Dec.Show("等级不足"); 
                return;
            }
        }
        transform.parent.parent.SendMessage("OnClickEnterMap", crt_map);
        Hide(); 
    }
    /// <summary>
    /// 显示地图信息
    /// </summary>
    public void Init(base_map_item map)
    {
        crt_map = map; 
        if (SumSave.crtHero.lv < crt_map.GetMap().map_lv && !Tool_Battle.IsBuff(common_Buff.月卡))
        {
            Alert_Dec.Show("等级不足,无法查看");
            Hide();
            return;
        }
        Show_Map_Intensity();
        Show_Base_Info();
    }
    private void Show_Base_Info()
    {
        Dec();
        ShowDrop();
    }
    /// <summary>
    /// 显示地图强度
    /// </summary>
    private void Show_Map_Intensity()
    {
        ClearObject(m_map_intensity_borm);
        crt_select_map_lv_item = null;
        for (int i = 0; i < crt_map.GetMap().map_boss.Count; i++)
        {
            select_map_lv_item item = Instantiate(select_map_lv_item_Prefab, m_map_intensity_borm);
            item.Init(crt_map.GetMap().map_boss[i], i + 1);
            item.GetComponent<Button>().onClick.AddListener(() => { OnClickIntensity(item); });
            //if(crt_select_map_lv_item==null) OnClickIntensity(item);
        }
    }

    private select_map_lv_item crt_select_map_lv_item;
    private void OnClickIntensity(select_map_lv_item item)
    {
        if (crt_select_map_lv_item != null) crt_select_map_lv_item.Selected = false;
        crt_select_map_lv_item = item;
        crt_select_map_lv_item.Selected = true;
        crt_map.Select_Map_Intensity(item.GetMapIntensity);
        Show_Base_Info();

#if UNITY_EDITOR
        //tool_equip();
#elif UNITY_ANDROID
        
           
#elif UNITY_IPHONE
        
#endif
    }
    /// <summary>
    /// 测试物品
    /// </summary>
    private void tool_equip()

    {
        //return;
        //测试模式
        foreach (var item in drop_dic)
        {
            Bag_Base_VO data = ArrayHelper.Find(SumSave.db_stditems, e => e.Name == item.Key);
            if (data != null)
            {
                string user_value = Tool_Battle.Obtain_Equip(data, 1, 7);
                Bag_Base_VO base_data = tool_Categoryt.Read_BaseBag(user_value);
                SetData(base_data);
            }
        }
    }
    private void SetData(Bag_Base_VO data)
    {
        Stditem_StdMode_List equip_Type = Tool_State.ToEnum(data.StdMode, Stditem_StdMode_List.nothing);
        if (equip_Type == Stditem_StdMode_List.nothing) return;
        switch (equip_Type)
        {
            case Stditem_StdMode_List.衣服:
            case Stditem_StdMode_List.武器:
            case Stditem_StdMode_List.戒指:
            case Stditem_StdMode_List.项链:
            case Stditem_StdMode_List.手镯:
            case Stditem_StdMode_List.头盔:
            case Stditem_StdMode_List.靴子:
            case Stditem_StdMode_List.腰带:
            case Stditem_StdMode_List.勋章:
            case Stditem_StdMode_List.遗物_钳:
            case Stditem_StdMode_List.遗物_足:
                SumSave.crt_bags.Set_Bag_List(data);

                break;
            case Stditem_StdMode_List.消耗品:
            case Stditem_StdMode_List.材料:
                ObscuredInt  number = 1;
                ObscuredInt  random = Random.Range(1, 1000);
                ObscuredInt  maxnumber = number + Random.Range(1, 1000);
                Battle_Tool.Dream_Obtain_Resources(Obtain_Int.Add(1, data.Name, new ObscuredInt [] { number + random, random }), maxnumber);
                break;
            case Stditem_StdMode_List.nothing:
                break;
            case Stditem_StdMode_List.货币:
                currency_unit unit = Tool_State.ToEnum(data.StdMode, currency_unit.金币);
                int moeny = 1;
                switch (unit)
                {
                    case currency_unit.金币:
                        moeny = 100;
                        break;
                    case currency_unit.元宝:
                        break;
                    case currency_unit.Boss积分:
                        break;
                    case currency_unit.试炼积分:
                        break;
                    case currency_unit.灵气:
                        break;
                    default:
                        break;
                }
                Battle_Tool.Dream_Obtain_Unit(unit, moeny, Obtain_Int.Add_unit(moeny));
                break;
        }
    }

    private void ShowDrop()
    {
        ClearObject(m_drop_borm);
        drop_dic.Clear();
        db_map_vo map = crt_map.GetMap();
        if (map.map_intensity_drop.Count > 0)
        {
            List<string> drop_value = new List<string>();
            foreach (var item in map.map_intensity_drop.Keys)
            {
                if (crt_map.GetMap_Intensity >= item)
                {
                    drop_value.Add(map.map_intensity_drop[item]);
                }
            }
            for (int i = drop_value.Count - 1; i >= 0; i--)
            { 
                Show_Bag(drop_value[i]);
            }
        }
        Show_Bag(map.drop_value);
        Show_Bag(map.map_drop);
    }
    /// <summary>
    /// 显示背包
    /// </summary>
    /// <param name="bag_name"></param> 
    private void Show_Bag(string value)
    {
        string[] values = value.Split(';');
        if (values.Length > 0)
        {
            foreach (var drop_value in values)
            {
                string[] drop_value_info = drop_value.Split(' ');
                if (drop_value_info.Length > 1)
                {
                    Bag_Base_VO bag = ArrayHelper.Find(SumSave.db_stditems, e => e.Name == drop_value_info[drop_value_info.Length - 1]);
                    if (bag != null)
                    {
                        if (!drop_dic.ContainsKey(bag.Name))
                        {
                            dream_BagItem item = Instantiate(dream_BagItem_Prefabs, m_drop_borm);
                            item.Data = bag;
                            item.GetComponent<Button>().onClick.AddListener(() => { OnClick(item); });
                            drop_dic.Add(bag.Name, 1);
                        }
                        
                    }
                }
            }
        }
    }
    /// <summary>
    /// 显示信息
    /// </summary>
    /// <param name="item"></param>
    private void OnClick(dream_BagItem item)
    {
        if (SumSave.crtHero.lv < crt_map.GetMap().map_lv && !Tool_Battle.IsBuff(common_Buff.月卡))
        { 
            Alert_Dec.Show("等级不足,无法查看");
            return;
        }
        Stditem_StdMode_List equip_Type = Tool_State.ToEnum(item.Data.StdMode, Stditem_StdMode_List.nothing);
        if (equip_Type == Stditem_StdMode_List.nothing) return;
        panel_hero_equip.Show();
        switch (equip_Type)
        {
            case Stditem_StdMode_List.衣服:
            case Stditem_StdMode_List.武器:
            case Stditem_StdMode_List.戒指:
            case Stditem_StdMode_List.项链:
            case Stditem_StdMode_List.手镯:
            case Stditem_StdMode_List.头盔:
            case Stditem_StdMode_List.靴子:
            case Stditem_StdMode_List.腰带:
            case Stditem_StdMode_List.勋章:
            case Stditem_StdMode_List.遗物_钳:
            case Stditem_StdMode_List.遗物_足:
                panel_hero_equip.Select_Bag(item, Panel_BagType.展示);
                break;
            case Stditem_StdMode_List.消耗品:
            case Stditem_StdMode_List.材料:
            case Stditem_StdMode_List.货币:
                panel_hero_equip.Select_Resources(item, Panel_BagType.展示);
                break;
            case Stditem_StdMode_List.nothing:
                break;
        }
    }
    /// <summary>
    /// 显示信息 倒计时
    /// </summary>
    string map_info,crt_info;
    private void Dec()
    {
        StopAllCoroutines();
        db_map_vo map = crt_map.GetMap();
        map_info = map.map_name +
            "\n刷怪频率:" + map.map_add_number_monster[crt_map.GetMap_Intensity - 1] + "个/" + map.map_cd[crt_map.GetMap_Intensity - 1] + "s" +
            "\nBoss:" + map.map_boss[crt_map.GetMap_Intensity - 1] +
            "\nBoss前置击杀:" + map.map_crate_boss_condition[crt_map.GetMap_Intensity - 1] +
            "\nBoss刷新:" + map.map_boss_cdtime[crt_map.GetMap_Intensity - 1] + "s";
        if (map.map_type != 0)
        {
            crt_info = Show_Color.Green("每日一次"); 
        }
        else
        {
            if (SumSave.crt_setting.Boss_list.ContainsKey(map.map_boss[crt_map.GetMap_Intensity - 1]))
            {
                map_info += Show_Color.Green("\n存量 " + SumSave.crt_setting.Boss_list[map.map_boss[crt_map.GetMap_Intensity - 1]].Item1) + "";

            }
            //for (int i = 0; i < SumSave.crt_setting.battle_Boss_list.Count; i++)
            //{
            //    (string, int) boss = SumSave.crt_setting.battle_Boss_list[i];
            //    List<string> list = ArrayHelper.Get_Split<string>(boss.Item1, '+');
            //    if (list.Count == 2)
            //    {
            //        if (list[0] == map.map_boss[crt_map.GetMap_Intensity - 1])
            //            map_info += Show_Color.Green("\n存量 " + list[1]) + "";
            //    }
            //}
            int spanSeconds = Tool_Battle.Meet_maposs_criteria(map.map_boss[crt_map.GetMap_Intensity - 1]);
            crt_info = "Boss倒计时:" + Show_Color.Green(ConvertSecondsToHHMMSS(spanSeconds));
            if (spanSeconds > 0)
            {
                StartCoroutine(Game_WaitTime(spanSeconds));
            }
        }
        
        base_info.text = map_info + "\n" + crt_info;
    }

    private IEnumerator Game_WaitTime(int time)
    {

        while (time > 0)
        { 
            time--;
            crt_info = "Boss倒计时:" + Show_Color.Green(ConvertSecondsToHHMMSS(time)); 
            base_info.text = map_info + "\n" + crt_info;
            yield return new WaitForSeconds(1f);
        }
    }

    private void Hide()
    {
        AudioManager.Instance.playAudio(ClipEnum.购买物品);
        gameObject.SetActive(false);
        //transform.parent.gameObject.SetActive(false);
        StopAllCoroutines();
    }
}
