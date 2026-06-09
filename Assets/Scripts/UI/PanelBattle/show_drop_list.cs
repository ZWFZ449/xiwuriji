using CodeStage.AntiCheat.ObscuredTypes;
using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class show_drop_list : Base_Mono
{
    private Transform m_drop_borm;
    private dream_BagItem dream_BagItem_Prefabs;
    private TMP_Text drop_show_name;
    private Button close_button, confirm;
    /// <summary>
    /// 掉落列表
    /// </summary>
    private Dictionary<int, (string, List<Bag_Base_VO>)> drop_list = new Dictionary<int, (string, List<Bag_Base_VO>)>();
    private int index = 0;
    private int crt_index = -1;

    private panel_hero_equip panel_hero_equip;
    /// <summary>
    /// 消息
    /// </summary>
    private List<string> dic = new List<string>();
    /// <summary>
    /// 掉落字典
    /// </summary>
    private Dictionary<string,(int,db_map_vo)> dic_map_drop = new Dictionary<string, (int, db_map_vo)>();
    /// <summary>
    /// 击杀对象
    /// </summary>
    private string monster_name;
    private enum Drop_Type
    { 
        逐个掉落,
        固定掉落,
        随机掉落
    }
    private void Awake()
    {
        m_drop_borm = Find<Transform>("Scroll View/Viewport/Content");
        dream_BagItem_Prefabs = Tool_UI.Find_Prefabs<dream_BagItem>("dream_BagItem");
        drop_show_name = Find<TMP_Text>("drop_name/info/info");
        close_button = Find<Button>("close_button");
        close_button.onClick.AddListener(() => { Hide(); });
        confirm = Find<Button>("confirm");
        confirm.onClick.AddListener(() => { ReadList(); });
        panel_hero_equip = UI_Manager.I.GetPanel<panel_hero_equip>();
    }
        /// <summary>
        ///  查看掉落
        /// </summary>
    private void ReadList()
    {
        drop_list.Remove(crt_index);
        if (drop_list.Count == 0)
        {
            Hide();
        }
        else 
        {
            crt_index++;
            Drop_list();

        }
    }

    private void Hide()
    {
        index = 0;
        crt_index = -1;
        drop_list.Clear();
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 掉落
    /// </summary>
    /// <param name="monster"></param>
    /// <returns></returns>
    public List<string> Init(BaseBattleAttack monster)
    {
        dic.Clear();
        monster_name = monster.Data.crt_name;
        if (!dic_map_drop.ContainsKey(monster.Data.crt_name))
        {
            for (int i = 0; i < SumSave.db_maps.Count; i++)
            {
                switch (monster.Data.type)
                {
                    case Battle_Game_Type.monster:
                        for (int j = 0; j < SumSave.db_maps[i].map_monster.Count; j++)
                        {
                            if (SumSave.db_maps[i].map_monster[j] == monster.Data.crt_name)//存在列表
                            {
                                dic_map_drop.Add(monster.Data.crt_name, (0, SumSave.db_maps[i]));
                                break;
                            }
                        }
                        break;
                    case Battle_Game_Type.Boss:
                    case Battle_Game_Type.Activity_Monster:
                        for (int j = 0; j < SumSave.db_maps[i].map_boss.Count; j++)
                        {
                            if (SumSave.db_maps[i].map_boss[j] == monster.Data.crt_name)//存在列表
                            {
                                dic_map_drop.Add(monster.Data.crt_name, (j+1, SumSave.db_maps[i]));
                                break;
                            }
                        }
                        break;
                }
            }
        }
        if (dic_map_drop.ContainsKey(monster.Data.crt_name))
            judgment(dic_map_drop[monster.Data.crt_name]);
        return dic;
    }
    /// <summary>
    /// 判断掉落内容
    /// </summary>
    /// <param name="data"></param>
    private void judgment((int, db_map_vo) data)
    {
        //int moeny = data.Item2.base_moenys[data.Item2.GetMapIntensityDrop - 1];
        //moeny *= (100 + SumSave.crtMaxBattle.gold_bonus) / 100;
        //dic.Add("获得 " + currency_unit.金币 + " * " + moeny);
        //Battle_Tool.Dream_Obtain_Unit(currency_unit.金币, moeny, Obtain_Int.Add_unit(moeny));
        switch (data.Item1)
        {
            case 0:
                Show_Bag(data.Item2.map_drop, Drop_Type.随机掉落, data.Item2);
                break;
            case 1:
            case 2:
            case 3:
                index++;
                if (!drop_list.ContainsKey(index))
                {
                    drop_list.Add(index, (data.Item2.map_boss[data.Item1-1] + "\n" + Show_Color.Red("击杀时刻:" + DateTime.Now), new List<Bag_Base_VO>()));
                }
                if (data.Item2.map_intensity_drop.Count > 0)
                {
                    foreach (var item in data.Item2.map_intensity_drop.Keys)
                    {
                        if (data.Item1 >= item)
                        {
                            Show_Bag(data.Item2.map_intensity_drop[item], Drop_Type.逐个掉落, data.Item2);
                        }
                    }
                }
                Show_Bag(data.Item2.drop_value, Drop_Type.逐个掉落, data.Item2);
                Show_Bag(data.Item2.map_drop, Drop_Type.固定掉落, data.Item2);
                if (!gameObject.activeSelf)
                {
                    gameObject.SetActive(true);
                    Drop_list();
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 掉落列表
    /// </summary>
    /// <param name="map"></param>
    private void Drop_list()
    {
        if (crt_index == -1) crt_index = Obtain_Index();
        else
        if (crt_index < drop_list.Count - 1)
        {
            crt_index++;
        }
        else crt_index = Obtain_Index();
        (string, List<Bag_Base_VO>) data = drop_list[crt_index];
        drop_show_name.text = data.Item1;
        ClearObject(m_drop_borm);
        for (int i = 0; i < data.Item2.Count; i++)
        {
            dream_BagItem item = Instantiate(dream_BagItem_Prefabs, m_drop_borm);
            item.Data = data.Item2[i];
            item.GetComponent<Button>().onClick.AddListener(() => { OnClick(item); });
        }
    }

    private int Obtain_Index()
    {
        foreach (var item in drop_list.Keys)
        {
            return item;
        }
        return 0;
    }
    private void Show_Bag(string value, Drop_Type type,db_map_vo crt_map)
    {
        string[] values = value.Split(';');
        switch (type)
        {
            case Drop_Type.逐个掉落:
                for (int i = 0; i < values.Length; i++)
                {
                    Obtain_Drop(values[i],type,crt_map.map_lv);
                }
                break;
            case Drop_Type.固定掉落:
                int sum = (int)Random.Range(9, 9 + (crt_map.map_id * 2) + (crt_map.map_cd[crt_map.GetMapIntensityDrop - 1] / 10));
                if (!Tool_Battle.Is_first_Boss_Kill(crt_map.map_boss[crt_map.GetMapIntensityDrop - 1])) sum *= 2;
                else
                {
                    if (Random.Range(0, 100) < 1) sum *= 2;
                }
                while (sum >= drop_list[index].Item2.Count)
                { 
                    Obtain_Drop(values[Random.Range(0, values.Length)], type, crt_map.map_lv);
                }
                break;
            case Drop_Type.随机掉落:
                int number = Tool_Battle.Quality() / 2 + 1;
                for (int i = 0; i < number; i++)
                {
                    Obtain_Drop(values[Random.Range(0, values.Length)], type, crt_map.map_lv);
                }
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 掉落物品
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private void Obtain_Drop(string value, Drop_Type type,int lv)
    {
        Bag_Base_VO data;
        string[] drop_value_info = value.Split(' ');
        if (drop_value_info.Length > 1)
        {
            string[] probability = drop_value_info[0].Split('/');
            if (probability.Length > 1)
            {
                int random = Random.Range(0, int.Parse(probability[1]));
                int probability_value = int.Parse(probability[0]);
                if (type != Drop_Type.逐个掉落) probability_value = probability_value * (100 + SumSave.crtMaxBattle.drop_bonus) / 100;
                if (random <= probability_value)
                {
                    data = ArrayHelper.Find(SumSave.db_stditems, e => e.Name == drop_value_info[drop_value_info.Length - 1]);
                    if (data != null)
                    {
                        string user_value = Tool_Battle.Obtain_Equip(data, 1, Quality(data));
                        Bag_Base_VO value_data = tool_Categoryt.Read_BaseBag(user_value);
                        SetData(value_data,lv);
                        if (type != Drop_Type.随机掉落) drop_list[index].Item2.Add(value_data);
                    }
                }
            }
        } 
    }
    private int Quality(Bag_Base_VO data)
    { 
        int quality = 1;
        Stditem_StdMode_List equip_Type = Tool_State.ToEnum(data.StdMode, Stditem_StdMode_List.nothing);
        if (equip_Type == Stditem_StdMode_List.nothing) return quality;
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
                quality = Tool_Battle.Quality();
                break;
            case Stditem_StdMode_List.消耗品:
            case Stditem_StdMode_List.材料:
            case Stditem_StdMode_List.货币:
                quality = 1;
                break;
        }
        return quality;
    }
    /// <summary>
    /// 写入数据库
    /// </summary>
    /// <param name="data"></param>
    private void SetData(Bag_Base_VO data,int lv)
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
                if (Recycling_judgment(data)) 
                {
                    SumSave.crt_bags.Set_Bag_List(data);
                } 
                break;
            case Stditem_StdMode_List.消耗品:
            case Stditem_StdMode_List.材料:
                ObscuredInt  number = 1;
                ObscuredInt  random = Random.Range(1, 1000);
                ObscuredInt  maxnumber = number + Random.Range(1, 1000);
                dic.Add("获得 " + data.Name + " * " + number);
                Battle_Tool.Dream_Obtain_Resources(Obtain_Int.Add(1, data.Name, new ObscuredInt [] { number + random, random }), maxnumber);
                break;
            case Stditem_StdMode_List.nothing:
                break;
            case Stditem_StdMode_List.货币:
                currency_unit unit = Tool_State.ToEnum(data.Name, currency_unit.金币);
                ObscuredLong moeny = 1;
                switch (unit)
                {
                    case currency_unit.金币:
                        moeny = moeny_needs[lv / 5] * (100 + SumSave.crtMaxBattle.gold_bonus) / 100;
                        break;
                    case currency_unit.元宝:
                        break;
                    case currency_unit.Boss积分:
                        break;
                    case currency_unit.转生积分:
                        break;
                    case currency_unit.试炼积分:
                        break;
                    case currency_unit.灵气:
                        break;
                    default:
                        break;
                }
                dic.Add("获得 " + unit + " * " + moeny);
                Battle_Tool.Dream_Obtain_Unit(unit, moeny, Obtain_Int.Add_unit(moeny));
                break;
        }
    }
    private List<int> moeny_needs = new List<int>() { 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000, 2000, 3000, 3500, 4000, 5000, 5000, 5000, 5000 };
    /// <summary>
    /// 回收判断
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private bool Recycling_judgment(Bag_Base_VO data)
    {
        bool exist = false;
        if (SumSave.crt_bags.Get_Bag_List().Count >= SumSave.crt_bags.Get_Page)
        {
            dic.Add("获取 "  + data.Name + " 失败,背包已满;");
            return false;
        }
        if (data.StdMode == Stditem_StdMode_List.项链.ToString())
        {
            string[] infos = data.user_value.Split(' '); 
            int lucky = 1;
            if (infos.Length > 2)
            {
                lucky = int.Parse(infos[1]);
                if (lucky > 1)
                {
                    dic.Add(Show_Color.Red("获取 " + (enum_equip_quality_list)(int.Parse(infos[2])) + " " + data.Name + ""));
                    return true;
                }
            }
        }
        if (SumSave.crt_setting.user_data_settings.Count >= 6 && SumSave.crt_setting.user_data_settings[5] == 1)//回收非本职业
        {
            if (data.job != SumSave.crtHero.job && data.job != 0)
            {
                recycle(data, true);
                return false;
            }
        }
        if (SumSave.crt_setting.user_data_settings.Count >= 7 && SumSave.crt_setting.user_data_settings[6] == 1)//保留20级以上的需求为0的装备
        {
            if (data.job  == 0 && data.need_lv >= 20)
            {
                string[] infos = data.user_value.Split(' ');
                int lv  = 1;
                if (infos.Length > 2)
                {
                    lv = int.Parse(infos[2]);
                    if (lv >= 6)
                    {
                        return true;
                    }
                }
            }
        }

        string[] info_str = data.user_value.Split(' ');
        if (info_str.Length > 2)
        {
            int lv = int.Parse(info_str[2]);
            //测试
            //Game_Omphalos.global_battle_info("击杀 " + monster_name + " 获得 " + (enum_equip_quality_list)lv + " " + data.Name, data);
            if (lv >= 6)//上传公共消息
            {
                Game_Omphalos.global_battle_info("击杀 " + monster_name + " 获得 " + (enum_equip_quality_list)lv + " " + data.Name, data);
            }
            foreach (var item in SumSave.crt_setting.battle_base_list)
            {
                if (item.Item1 == lv)//判断回收等级
                {
                    if (data.need_lv >= item.Item2)
                    {
                        exist = true;
                        dic.Add("获得 " + (enum_equip_quality_list)lv + " " + data.Name );
                        return exist;
                    }
                    else
                    {
                        recycle(data);
                        //int moeny = data.price;
                        //dic.Add("回收 " + (enum_equip_quality_list)lv + data.Name + " 获得 " + currency_unit.金币 + " * " + moeny);
                        //Battle_Tool.Dream_Obtain_Unit(currency_unit.金币, moeny, Obtain_Int.Add_unit(moeny));
                        //if (lv >= 5)
                        //{
                        //    int sycee = 0;
                        //    sycee += data.need_lv / 7 * (lv - 5) + 1;
                        //    dic.Add("回收 " + (enum_equip_quality_list)lv + data.Name + " 获得 " + currency_unit.元宝 + " * "+ sycee);
                        //    Battle_Tool.Dream_Obtain_Unit(currency_unit.元宝, sycee, Obtain_Int.Add_unit(sycee));
                        //}
                        return exist;
                    }
                }
            }
        }
       return exist;
    }

    private void recycle(Bag_Base_VO data,bool exist=false)
    {
        int moeny = data.price;
        string[] info_str = data.user_value.Split(' ');
        int lv = 1;
        if (info_str.Length > 2) lv = int.Parse(info_str[2]);
        //if (SumSave.crtHero.zs_lv > 1 || SumSave.crtHero.lv >= 60)
        moeny = moeny * lv / Enum.GetValues(typeof(enum_equip_quality_list)).Cast<int>().Max();
        dic.Add( (exist?"职业回收" :"回收 ") + (enum_equip_quality_list)lv + data.Name + " 获得 " + currency_unit.金币 + " * " + moeny);
        Battle_Tool.Dream_Obtain_Unit(currency_unit.金币, moeny, Obtain_Int.Add_unit(moeny));
        if (lv >= 5)
        {
            int sycee = 0;
            sycee += data.need_lv / 7 * (lv - 5) + 1;
            dic.Add((exist ? "职业回收" : "回收 ") + (enum_equip_quality_list)lv + data.Name + " 获得 " + currency_unit.元宝 + " * " + sycee); 
            Battle_Tool.Dream_Obtain_Unit(currency_unit.元宝, sycee, Obtain_Int.Add_unit(sycee));
        }
    }
    private void OnClick(dream_BagItem item)
    {
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
                panel_hero_equip.Select_Resources(item, Panel_BagType.展示);
                break;
        }

    }
}
