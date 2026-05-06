using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVC;
using Common;
using System;
using System.Security.Cryptography;
using Random = UnityEngine.Random;
using Components;
/// <summary>
/// 战斗工具类
/// </summary>
public static class Battle_Tool
{
    /// <summary>
    /// 五行天命
    /// </summary>
    private static Dictionary<int, int> base_life_types = new Dictionary<int, int>();
    /// <summary>
    /// 获取资源
    /// </summary>
    /// <param name="resources_name">名称</param>
    /// <param name="index">数量指针</param>
    /// <param name="isverify">是否取消检测</param>
    public static void Obtain_Resources(int index, in int maxnumber, bool isverify = false)
    {
        SumSave.crt_bags.Get(Obtain_Int.Get(index), maxnumber, isverify);
    }
    /// <summary>
    /// 判断职业名称
    /// </summary>
    /// <returns></returns>
    public static string Obtain_Talent_Name()
    {
        string name = "";
        if(SumSave.crtHero.job==0||SumSave.crtHero.SelectPos==-1)return name;
        if (SumSave.crtHero.job != 0)
        {
            //
            for (int i = 0; i < SumSave.db_player_talent_types.Count; i++)
            {
                if (SumSave.crtHero.job == SumSave.db_player_talent_types[i].talent_type_job)
                {
                    return SumSave.db_player_talent_types[i].talent_type_name[SumSave.crtHero.SelectPos-1];
                }
            }
        }
        return name;
    }
    /// <summary>
    /// 获取货币
    /// </summary>
    /// <param name="unit">单位</param>
    /// <param name="value">值</param>
    public static void Dream_Obtain_Unit(currency_unit unit, long value,string verification)
    {
        List<long> list = ArrayHelper.Get_Split<long>(verification, ',');
        if (value == list[0] - list[1])
        {
            SumSave.crt_user_unit.verify_data(unit, value);
        }
    }
    public static void Dream_Obtain_Resources(int index, in int maxnumber, bool isverify = false)
    {
        SumSave.crt_bags.Get(Obtain_Int.Get(index), maxnumber, isverify);
    }

    private static Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
    /// <summary>
    /// 查找预制体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="prefabName"></param>
    /// <returns></returns>
    public static T Find_Prefabs<T>(string prefabName)
    {
        if (!prefabs.ContainsKey(prefabName))
        {
            GameObject obj = Resources.Load<GameObject>("Prefabs/prefab/" + prefabName);
            prefabs.Add(prefabName, obj);
        }
        return prefabs[prefabName].GetComponent<T>();
    }
    public static string Equip_User_Value(string[] infos)
    {
        string user_value = "";
        for(int i= 0; i < infos.Length; i++)
        {
            user_value+=(user_value == "" ? "" : " ") + infos[i];
        }
        return user_value;
    }
    /// <summary>
    /// 获取五行类型
    /// </summary>
    /// <returns></returns>
    public static Dictionary<int, int> Get_Life_Type()
    {
        Init_Life_type();
        return base_life_types;
    }
    /// <summary>
    /// 初始化五行天命
    /// </summary>
    public static void Init_Life_type()
    {
        base_life_types = new Dictionary<int, int>();

        for (int i = 0; i < SumSave.old_crt_hero.tianming_Platform.Length; i++)
        {
            if (base_life_types.ContainsKey(SumSave.old_crt_hero.tianming_Platform[i]))
            {
                base_life_types[SumSave.old_crt_hero.tianming_Platform[i]]++;
            }
            else
            {
                base_life_types.Add(SumSave.old_crt_hero.tianming_Platform[i], 1);
            }
        }
    }

    public static int Judging_Five_Elements(int[] life_type,int[] tagert_life)
    {
        int value = 0;
        for (int i = 0; i < life_type.Length; i++)
            for (int j = 0; j < tagert_life.Length; j++)
            {

            }
        return value ;
    }
    /// <summary>
    /// 获取基准值
    /// </summary>
    /// <param name="base_value"></param>
    /// <returns></returns>
    public static int Alchemy_limit(int base_value)
    {
        int value = base_value / 10;
        if (SumSave.crt_MaxHero_okd.Lv >= 30) 
        {
            value += (SumSave.crt_MaxHero_okd.Lv - 20) / 2 * base_value / 100;
        }
        value =(int) MathF.Min(value, base_value);
        return value;
    } 
    /// <summary>
    /// 测试随机数
    /// </summary>
    /// <returns></returns>
    public static int random()
    {
        return Random.Range(100, 200);
    }

    /// <summary>
    /// 获取字符串
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    public static string GetStr(object o)
    {
        return "'" + o + "'";
    } 


    
    /// <summary>
    /// 获取经验
    /// </summary>
    /// <param name="exp"></param>
    /// <param name="state">1为打怪收益2为确定性收益</param>
    public static void Obtain_Exp(long exp,int state=1)
    {
        SumSave.crtMaxBattle.exp += exp;
        SumSave.crtHero.exp += exp;
        //升级
        if (SumSave.db_lvs.ContainsKey(SumSave.crtMaxBattle.lv))
        {
            if (SumSave.crtMaxBattle.exp >= SumSave.db_lvs[SumSave.crtMaxBattle.lv].exp)
            { 
               SumSave.crtMaxBattle.exp -= SumSave.db_lvs[SumSave.crtMaxBattle.lv].exp;
               SumSave.crtHero.exp -= SumSave.db_lvs[SumSave.crtMaxBattle.lv].exp;
               SumSave.crtMaxBattle.lv += 1;
               SumSave.crtHero.lv += 1;
            }
        }
        SumSave.crtHero.MysqlData();
    }

    /// <summary>
    /// 获取加成buff
    /// </summary>
    /// <param name="index">1经验 2历练3月卡</param>
    /// <returns></returns>
    public static int IsBuff(int index)
    {
        int base_value = 0;

        foreach (var item in SumSave.crt_player_buff.player_Buffs)
        {
            (DateTime, int, float, int) time = item.Value;
            if (index == time.Item4)
            {
                if (SettlementTransport((time.Item1).ToString("yyyy-MM-dd HH:mm:ss")) < time.Item2)
                {
                    base_value = (int)(time.Item3 * 100 - 100);
                }
                //else
                //{
                //    SumSave.crt_player_buff.player_Buffs.Remove(item.Key);
                //    Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.user_player_buff, SumSave.crt_player_buff.Set_Uptade_String(), SumSave.crt_player_buff.Get_Update_Character());//角色丹药Buff更新数据库
                //    break;
                //}
            }
        }
        return base_value;
    }

    /// <summary>
    /// 计算时间 现
    /// </summary>
    /// <param name="time">记录时间</param>
    /// <param name="type">获取 1分钟 2秒钟3小时 4天</param>
    /// <returns></returns>
    public static int SettlementTransport(string time, int type = 1)
    {
        if (time == null || time == "") return -1;

        TimeSpan span;
        int spanNumber = 0;
        int value = 0;
        span = SumSave.nowtime - Convert.ToDateTime(time);
        if (type == 1)//计算分钟
            spanNumber = span.Minutes + span.Hours * 60 + span.Days * 60 * 24 + value;
        else if (type == 2)//计算秒
            spanNumber = span.Seconds + span.Minutes * 60 + span.Hours * 60 * 60 + span.Days * 60 * 60 * 24 + (value * 60);
        else if (type == 3)//计算小时
            spanNumber = span.Hours + span.Days * 24;
        else if (type == 4)//计算天
            spanNumber = span.Days;

        if (spanNumber > 0)
        {

            //计算时间差值
            return spanNumber;
        }
        else return 0;
    }

    /// <summary>
    /// 计算两个时间的差值
    /// </summary>
    /// <param name="time">时间1</param>
    /// <param name="time2">时间2</param>
    /// <param name="type">获取 1分钟 2秒钟3小时 4天</param>
    /// <returns></returns>
    public static int SettlementTransport(string time, string time2, int type = 1 )
    {
        if (time == null || time == "") return -1;

        TimeSpan span;

        int spanNumber = 0;
        span = Convert.ToDateTime(time) - Convert.ToDateTime(time2);
        if (type == 1)//计算分钟
            spanNumber = span.Minutes + span.Hours * 60 + span.Days * 60 * 24;
        else if (type == 2)//计算秒
            spanNumber = span.Seconds + span.Minutes * 60 + span.Hours * 60 * 60 + span.Days * 60 * 60 * 24;
        else if (type == 3)//计算小时
            spanNumber = span.Hours + span.Days * 24;
        else if (type == 4)//计算天
            spanNumber = span.Days;
        if (spanNumber > 0)
        {
            //if (type == 3) spanNumber = span.Days + 1;
            //计算时间差值
            return spanNumber;
        }
        else return 0;


    }

    /// <summary>
    /// 获取奖励 分解式
    /// </summary>
    /// <param name="result"></param>
    /// <param name="num"></param>
    public static void Obtain_result((string,int,int) result, int num = 1)//进阶奖励1、材料2、灵物3、灵珠4、魔丸5、皮肤6、灵气
    {
        Obtain_result(result.Item1+"*"+result.Item2+"*"+result.Item3, num);
    }
    /// <summary>
    /// 获取资源
    /// </summary>
    /// <param name="result"></param>
    /// <param name="num">获得多少次该资源</param>
    public static void Obtain_result(string result,int num=1)//进阶奖励1、材料2、灵物3、灵珠4、魔丸5、皮肤6、灵气
    {
        if (result == "0") return;
        string[] result_list = result.Split('*');//0:资源名字 1:资源数量 2:奖励类型
        switch (int.Parse(result_list[2]))
        {
            case 1://获取资源
                BuffAcquisition(result_list,num);
                break;
            case 2:
                int random= Random.Range(1, 100);
                int number= int.Parse(result_list[1]) * num;
                int maxnumber = number + Random.Range(1, 100);
                Obtain_Resources(Obtain_Int.Add(1, result_list[0], new int[] { number + random, random }), maxnumber);
                break;
            case 3:
                SumSave.crt_user_unit.verify_data(currency_unit.金币, int.Parse(result_list[1]) * num);
                break;
            case 4:
                SumSave.crt_user_unit.verify_data(currency_unit.Boss积分, int.Parse(result_list[1]) * num);
                break;
            case 5:
                SumSave.old_crt_hero.hero_value += (SumSave.old_crt_hero.hero_value == "" ? "" : ",") + result_list[0];
                Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_hero, new string[] { Battle_Tool.GetStr(SumSave.old_crt_hero.hero_value) },
                    new string[] { "hero_value" });
                break;
            case 6:
               
                break;
            case 7:

                break;
            case 8:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 获得buff
    /// </summary>
    private static void BuffAcquisition(string[] result_list,int num)
    {
        switch (result_list[0])
        {
            case "1亿灵珠":
                SumSave.crt_user_unit.verify_data(currency_unit.金币, 100000000 * int.Parse(result_list[1])*num);//获得灵珠
                break;
            case "2000历练值":
                SumSave.crt_user_unit.verify_data(currency_unit.元宝, 2000 * int.Parse(result_list[1]) * num);
                break;
            case "下品历练丹":
                //添加1.5倍的历练值
                if (SumSave.crt_player_buff.player_Buffs.ContainsKey("中品历练丹")|| SumSave.crt_player_buff.player_Buffs.ContainsKey("上品历练丹"))
                {
                    return;
                }
                AddBuff(result_list[0], 1.5f, 2, int.Parse(result_list[1])*num);
                break;
            case "中品历练丹":
                //添加2倍的历练值
                if (SumSave.crt_player_buff.player_Buffs.ContainsKey("上品历练丹"))
                {
                    return;
                }
                if (SumSave.crt_player_buff.player_Buffs.ContainsKey("下品历练丹"))
                {
                    Alert_Dec.Show("下品历练丹失效");
                    SumSave.crt_player_buff.player_Buffs.Remove("下品历练丹");
                }
                AddBuff(result_list[0], 2f, 2, int.Parse(result_list[1]) * num);
                break;
            case "上品历练丹":
                if (SumSave.crt_player_buff.player_Buffs.ContainsKey("下品历练丹"))
                {
                    Alert_Dec.Show("下品历练丹失效");
                    SumSave.crt_player_buff.player_Buffs.Remove("下品历练丹");
                }
                if (SumSave.crt_player_buff.player_Buffs.ContainsKey("中品历练丹"))
                {
                    Alert_Dec.Show("中品历练丹失效");
                    SumSave.crt_player_buff.player_Buffs.Remove("中品历练丹");
                }
                AddBuff(result_list[0], 3f, 2, int.Parse(result_list[1]) * num);
                break;
            case "下品经验丹":
                //添加1.5倍的经验值
                if (SumSave.crt_player_buff.player_Buffs.ContainsKey("中品经验丹")|| SumSave.crt_player_buff.player_Buffs.ContainsKey("上品经验丹"))
                {
                    return;
                }
                AddBuff(result_list[0], 1.5f, 1, int.Parse(result_list[1]) * num);
                break;
            case "中品经验丹":
                //添加2倍的经验值

                if (SumSave.crt_player_buff.player_Buffs.ContainsKey("上品经验丹"))
                {
                    return;
                }
                if (SumSave.crt_player_buff.player_Buffs.ContainsKey("下品经验丹"))
                {
                    Alert_Dec.Show("下品经验丹失效");
                    SumSave.crt_player_buff.player_Buffs.Remove("下品经验丹");
                }
                AddBuff(result_list[0], 2f, 1, int.Parse(result_list[1]) * num);
                break;
            case "上品经验丹":

                if (SumSave.crt_player_buff.player_Buffs.ContainsKey("下品经验丹"))
                {
                    Alert_Dec.Show("下品经验丹失效");
                    SumSave.crt_player_buff.player_Buffs.Remove("下品经验丹");
                }
                if (SumSave.crt_player_buff.player_Buffs.ContainsKey("中品经验丹"))
                {
                    Alert_Dec.Show("中品经验丹失效");
                    SumSave.crt_player_buff.player_Buffs.Remove("中品经验丹");
                }

                AddBuff(result_list[0], 3f, 1,int.Parse(result_list[1]) * num);
                break;
            default:
                int random = Random.Range(1, 100);
                int number = int.Parse(result_list[1]) * num;
                int maxnumber = number + Random.Range(1, 100);
                Obtain_Resources(Obtain_Int.Add(1, result_list[0], new int[] { number + random, random }), maxnumber);
                //Obtain_Resources(result_list[0], int.Parse(result_list[1]) * num);//获取奖励
                break;
        }
    }
    /// <summary>
    /// 添加BUff
    /// </summary>
    private static void AddBuff(string _buy_item, float effect, int icon ,int buy_num = 1)
    {
        if (SumSave.crt_player_buff.player_Buffs.ContainsKey(_buy_item))
        {
            SumSave.crt_player_buff.player_Buffs[_buy_item] =
                (SumSave.crt_player_buff.player_Buffs[_buy_item].Item1,
                SumSave.crt_player_buff.player_Buffs[_buy_item].Item2 + (60 * buy_num)
                , effect, icon);//当有时，增加buff时间
        }
        else
        {
            SumSave.crt_player_buff.player_Buffs.Add(_buy_item, (SumSave.nowtime, 60 * buy_num, effect, icon));
        }
        Tool_State.activation_State(State_List.经验丹);
        Tool_State.activation_State(State_List.历练丹);
        SendNotification(NotiList.Refresh_Max_Hero_Attribute);
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.user_player_buff, SumSave.crt_player_buff.Set_Uptade_String(), SumSave.crt_player_buff.Get_Update_Character());//角色丹药Buff更新数据库
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="name"></param>
    /// <param name="data"></param>
    public static void SendNotification(string name, object data = null)
    {
        AppFacade.I.SendNotification(name, data);
    }


    public static void tool_item()
    {
        foreach (var item in SumSave.db_stditems)
        {
            UI.UI_Manager.I.GetEquipSprite("icon/", item.Name);
        }
    }
    /// <summary>
    /// 显示货币单位
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static string FormatNumberToChineseUnit(long number, int decimalPlaces = 2)
    {
        if (number == 0) return "";
        string format = "0." + new string('#', decimalPlaces);

        if (number < 0)
        {
            return "-" + FormatNumberToChineseUnit(-number, decimalPlaces);
        }

        if (number >= 100000000)
        {
            return (number / 100000000.0).ToString(format) + "亿";
        }
        else if (number >= 10000)
        {
            return (number / 10000.0).ToString(format) + "万";
        }
        else
        {
            return number.ToString();
        }
    }
    /// <summary>
    /// 刷新排行榜
    /// </summary>
    private static void Refresh_Rank()
    {
        //SumSave.user_ranks.lists.Sort((x, y) => y.value.CompareTo(x.value));//升序排列
        SumSave.user_ranks.lists = ArrayHelper.OrderDescding(SumSave.user_ranks.lists, x => x.value);
        if (SumSave.user_ranks.lists.Count > 50)
        { 
            SumSave.user_ranks.lists.RemoveRange(50, SumSave.user_ranks.lists.Count - 50);
        }
        Game_Omphalos.i.immediately(Mysql_Table_Name.user_rank);
    }

    /// <summary>
    /// 创建排行榜
    /// </summary>
    private static  void crate_rank()
    {
        base_rank_vo rank = new base_rank_vo();
        rank.uid = SumSave.crt_user.uid;
        rank.type = SumSave.old_crt_hero.hero_pos;
        rank.name = SumSave.old_crt_hero.hero_name;
        rank.lv = SumSave.crt_MaxHero_okd.Lv;
        rank.ranking_index = 1;
        rank.value = (int)SumSave.crt_MaxHero_okd.totalPower;
        SumSave.user_ranks.lists.Add(rank);
        //排序
        Refresh_Rank();
    }

    /// <summary>
    /// 验证排行榜
    /// </summary>
    public static void validate_rank()
    {
        SendNotification(NotiList.Read_User_Ranks);
        bool exist = false;
        if (SumSave.user_ranks.lists.Count < 50)
        {
            for (int i = 0; i < SumSave.user_ranks.lists.Count; i++)
            {
                if (SumSave.user_ranks.lists[i].uid == SumSave.crt_user.uid)
                {
                    exist = true;
                    SumSave.crt_MaxHero_okd.Init();
                    //写入日志 暂时先关闭
                    //if (SumSave.crt_MaxHero.totalPower < SumSave.user_ranks.lists[i].value)//小于的情况 写入排行榜战力 且替换排行榜战力
                    //{
                    //    Game_Omphalos.i.Alert_Info($"你的战力降低了{"原战斗力" + SumSave.user_ranks.lists[i].value + " 当前" + (int)SumSave.crt_MaxHero.totalPower}");
                    //}
                    SumSave.user_ranks.lists[i].rank_name = SumSave.crt_MaxHero_okd.show_name;
                    SumSave.user_ranks.lists[i].value = (int)SumSave.crt_MaxHero_okd.totalPower;
                    SumSave.user_ranks.lists[i].lv = SumSave.crt_MaxHero_okd.Lv;
                    SumSave.user_ranks.lists[i].type = SumSave.old_crt_hero.hero_pos;// SumSave.crtHeroMaxs[0].Type;  
                }
            }
            //存在刷新 不在添加
            if (exist)
            {
                Refresh_Rank();
            }
            else crate_rank();
        }
        else if (SumSave.user_ranks.lists.Count >= 50) //50个榜已满,且自身战力大于榜上最低的一名
        {
            //自身在排行榜内 刷新属性
            for (int i = 0; i < SumSave.user_ranks.lists.Count; i++)
            {
                if (SumSave.user_ranks.lists[i].uid == SumSave.crt_user.uid)
                {
                    exist = true;
                    SumSave.crt_MaxHero_okd.Init();
                    //写入日志 暂时先关闭
                    //if (SumSave.crt_MaxHero.totalPower < SumSave.user_ranks.lists[i].value)//小于的情况 写入排行榜战力 且替换排行榜战力
                    //{
                    //    Game_Omphalos.i.Alert_Info($"你的战力降低了{"原战斗力" + SumSave.user_ranks.lists[i].value + " 当前" + (int)SumSave.crt_MaxHero.totalPower}");
                    //}
                    SumSave.user_ranks.lists[i].rank_name = SumSave.crt_MaxHero_okd.show_name;
                    SumSave.user_ranks.lists[i].value = (int)SumSave.crt_MaxHero_okd.totalPower;
                    SumSave.user_ranks.lists[i].lv = SumSave.crt_MaxHero_okd.Lv;
                    SumSave.user_ranks.lists[i].type = SumSave.old_crt_hero.hero_pos;// SumSave.crtHeroMaxs[0].Type;  
                    Refresh_Rank();
                    return;
                }
            }//自身不在排行榜内 
            if (!exist && SumSave.crt_MaxHero_okd.totalPower > SumSave.user_ranks.lists[SumSave.user_ranks.lists.Count - 1].value)
            {
                crate_rank();
            }

        }
    }
    /// <summary>
    /// 五行加成值
    /// </summary>
    private static int[] life_bonus = new int[7] { 0, 5, 10, 30, 60, 120, 120 };
    /// <summary>
    /// 获得五行加成系数
    /// </summary>
    /// <param name="life_type"></param>
    /// <returns></returns>
    public static int battle_life_bonus(int life_type)
    { 
     return life_bonus[life_type];
    }
    /// <summary>
    /// 创造怪物
    /// </summary>
    /// <param name="crt"></param>
    /// <param name="lv">1小怪2精英3boss4副本地图</param>
    public static crtMaxHeroVO crate_monster(crtMaxHeroVO crt, user_map_vo map,bool isBoss=false,int trial_storey=-1)
    {
        crtMaxHeroVO base_crt = new crtMaxHeroVO();
        base_crt.map_index = map.map_index;
        if (map.map_life != 0)
        {
            base_crt.life_types.Add(map.map_life - 1, 1);
            while (Random.Range(0, 100) > base_crt.life_types[map.map_life - 1] * 20)
            {
                base_crt.life_types[map.map_life - 1]++;
            }
            base_crt.life[map.map_life - 1] = map.need_lv * 2 * (100 + life_bonus[base_crt.life_types[map.map_life - 1]]) / 100;
        }
        
        base_crt.Monster_Lv = map.map_type;
        base_crt.Type= crt.damageMax>crt.MagicdamageMax?1:2;
        base_crt.show_name = crt.show_name;
        base_crt.index = crt.index;
        base_crt.Lv = crt.Lv;
        base_crt.icon = crt.icon;

        base_crt.unit = Random.Range(crt.Lv * 5, crt.Lv * 10) + 1;

        //标准战斗系数
        int coefficient = 1;
        if (Random.Range(0, 100) < 10)
        {
            coefficient = 2;
            //boss模版
            if (Random.Range(0, 100) < 10)
            {
                coefficient = 3;
            }
        }
        base_crt.Exp = (int)(crt.Exp * MathF.Pow(5, coefficient - 1));
        //普通地图
        if (map.map_type == 1)
        {
            //精英模版
            if (isBoss)
            {
                base_crt.Exp= (int)(crt.Exp * 10);
                base_crt.unit = base_crt.unit * 10;
                base_crt.Point = crt.index + 1;
                coefficient = 3;
            }
            base_crt.Monster_Lv = coefficient;
            if (base_crt.Monster_Lv > 1) base_crt.Point = (int)MathF.Max(base_crt.Point, crt.index * (base_crt.Monster_Lv - 1) + 1);

        }
        else if (map.map_type == 2)
        {
            if (isBoss)
            {
                base_crt.Exp = (int)(crt.Exp * 30);
                base_crt.unit = base_crt.unit * 30;
                base_crt.Point = crt.index * 2 + 1;
                coefficient = 1;
                base_crt.Monster_Lv = 3;
            }
            else
                base_crt.Monster_Lv = coefficient;
            if (base_crt.Monster_Lv > 1) base_crt.Point = (int)MathF.Max(base_crt.Point, crt.index * (base_crt.Monster_Lv - 1) + 1);

        }
        else if (map.map_type == 3)
        {
            base_crt.Exp = (int)(crt.Exp * Random.Range(51, 101));
            base_crt.unit = base_crt.unit * Random.Range(51, 101);
            base_crt.Point = crt.index * 3 + 1;
            base_crt.Monster_Lv = 3;
            coefficient = 1;
        }
        else if (map.map_type == 4)//副本地图
        {
            base_crt.Point = 0;
            base_crt.Monster_Lv = 4;
            coefficient = 1;
            if (map.map_life != 0)
            {
                base_crt.life[map.map_life - 1] += (SumSave.crt_MaxHero_okd.Lv - 30) * 2;
            }
            if (SumSave.crt_MaxHero_okd.Lv >= 40)
            {
                int lv = (SumSave.crt_MaxHero_okd.Lv - 30) / 10;
                coefficient = lv;
                
            }
        }
        if (trial_storey >= 0)
        {
            base_crt.life[(trial_storey + (trial_storey / 5)) % 5] = trial_storey * 3;
            base_crt.MaxHP = (long)((trial_storey + 1) * 100 * (Mathf.Pow(10, trial_storey / 20)));
            base_crt.MaxMp = (trial_storey + 1) * 100;
            base_crt.internalforceMP = (trial_storey + 1) * 100;
            base_crt.EnergyMp = (trial_storey + 1) * 10;
            base_crt.DefMin = (trial_storey + 1) * 10;
            base_crt.DefMax = (trial_storey + 1) * 20;
            base_crt.MagicDefMin = (trial_storey + 1) * 10;
            base_crt.MagicDefMax = (trial_storey + 1) * 20;
            base_crt.damageMin = (trial_storey + 1) * 10;
            base_crt.damageMax = (trial_storey + 1) * 20;
            base_crt.MagicdamageMin = (trial_storey + 1) * 10;
            base_crt.MagicdamageMax = (trial_storey + 1) * 20;
            base_crt.hit = (trial_storey + 1) * 10;
            base_crt.dodge = (trial_storey + 1) * 5;
            base_crt.penetrate = (trial_storey + 1) * 5;
            base_crt.block = (trial_storey + 1) * 5;
            base_crt.crit_rate = (trial_storey + 1) * 5;
            base_crt.crit_damage = 150 + (trial_storey + 1) * 5;
            base_crt.double_damage =(trial_storey + 1) * 5;
            base_crt.Lucky = (trial_storey + 1) / 10;
            base_crt.Real_harm = (trial_storey + 1) * 10;
            base_crt.Damage_Reduction = (trial_storey + 1) / 2;
            base_crt.move_speed = 100;
            base_crt.attack_speed = 300 - ((trial_storey + 1) * 2);
            base_crt.attack_distance = 100 + (trial_storey + 1) * 10;
            base_crt.Heal_Hp = (trial_storey + 1) * 10;
        }
        else
        {
            base_crt.MaxHP = (int)(crt.MaxHP * MathF.Pow(3, coefficient - 1));
            base_crt.MaxMp = crt.MaxMp;
            base_crt.internalforceMP = crt.internalforceMP;
            base_crt.EnergyMp = crt.EnergyMp;
            base_crt.DefMin = crt.DefMin * coefficient;
            base_crt.DefMax = crt.DefMax * coefficient;
            base_crt.MagicDefMin = crt.MagicDefMin * coefficient;
            base_crt.MagicDefMax = crt.MagicDefMax * coefficient;
            base_crt.damageMin = crt.damageMin * coefficient;
            base_crt.damageMax = crt.damageMax * coefficient;
            base_crt.MagicdamageMin = crt.MagicdamageMin * coefficient;
            base_crt.MagicdamageMax = crt.MagicdamageMax * coefficient;
            base_crt.hit = (crt.hit + map.need_lv) * coefficient;
            base_crt.dodge = crt.dodge * coefficient;
            base_crt.penetrate = crt.penetrate * coefficient;
            base_crt.block = crt.block * coefficient;
            base_crt.crit_rate = crt.crit_rate * coefficient;
            base_crt.crit_damage = crt.crit_damage;
            base_crt.double_damage = crt.double_damage;
            base_crt.Lucky = crt.Lucky;
            base_crt.Real_harm = crt.Real_harm;
            base_crt.Damage_Reduction = crt.Damage_Reduction;
            base_crt.Damage_absorption = crt.Damage_absorption;
            base_crt.resistance = crt.resistance;
            base_crt.move_speed = crt.move_speed;
            base_crt.attack_speed = crt.attack_speed;
            base_crt.attack_distance = crt.attack_distance;
            base_crt.bonus_Hp = crt.bonus_Hp;
            base_crt.bonus_Mp = crt.bonus_Mp;
            base_crt.bonus_Damage = crt.bonus_Damage;
            base_crt.bonus_MagicDamage = crt.bonus_MagicDamage;
            base_crt.bonus_Def = crt.bonus_Def;
            base_crt.bonus_MagicDef = crt.bonus_MagicDef;
            base_crt.Heal_Hp = crt.Heal_Hp * coefficient;
            base_crt.Heal_Mp = crt.Heal_Mp * coefficient;
        }
       
        //base_crt.monster_attrList.Add((int)state);
#if UNITY_EDITOR
        base_crt.MaxHP = 1;
#elif UNITY_ANDROID
#elif UNITY_IPHONE
#endif
        return base_crt;
    }
    /// <summary>
    /// 无尽模式怪物
    /// </summary>
    /// <param name="crt"></param>
    /// <param name="map"></param>
    /// <param name="trial_storey">挂机难度</param>
    /// <returns></returns>
    public static crtMaxHeroVO crate_monster(crtMaxHeroVO crt, user_map_vo map, int trial_storey)
    {
        crtMaxHeroVO base_crt = new crtMaxHeroVO();
        base_crt.map_index = map.map_index;
        if (map.map_life != 0)
        {
            base_crt.life_types.Add(map.map_life - 1, 1);
            while (Random.Range(0, 100) > base_crt.life_types[map.map_life - 1] * 20)
            {
                base_crt.life_types[map.map_life - 1]++;
            }
            base_crt.life[map.map_life - 1] = map.need_lv * 2 * (100 + life_bonus[base_crt.life_types[map.map_life - 1]]) / 100;
        }
        base_crt.Monster_Lv = map.map_type;
        base_crt.Type = crt.damageMax > crt.MagicdamageMax ? 1 : 2;
        base_crt.show_name = crt.show_name;
        base_crt.index = crt.index;
        base_crt.Lv = crt.Lv;
        base_crt.icon = crt.icon;

        base_crt.unit = Random.Range(crt.Lv * 5, crt.Lv * 10) + 1;

        //标准战斗系数
        int coefficient = 1;
        if (Random.Range(0, 100) < 10)
        {
            coefficient = 2;
            //boss模版
            if (Random.Range(0, 100) < 10)
            {
                coefficient = 3;
            }
        }
        base_crt.Exp = (int)(crt.Exp * MathF.Pow(5, coefficient - 1));
         
        if (trial_storey >= 0)
        {
            base_crt.life[(trial_storey + (trial_storey / 5)) % 5] = trial_storey * 3;
            //base_crt.MaxHP = (long)((trial_storey + 1) * 100 * (Mathf.Pow(10, trial_storey / 20)));
            if (trial_storey >= 60)
                base_crt.MaxHP = (long)((trial_storey + 1) * 300 * (Mathf.Pow(10, 2)));
            else
                base_crt.MaxHP = (long)((trial_storey + 1) * 100 * (Mathf.Pow(10, trial_storey / 20)));

            base_crt.MaxMp = (trial_storey + 1) * 100;
            base_crt.internalforceMP = (trial_storey + 1) * 100;
            base_crt.EnergyMp = (trial_storey + 1) * 10;
            base_crt.DefMin = (trial_storey + 1) * 10;
            base_crt.DefMax = (trial_storey + 1) * 20;
            base_crt.MagicDefMin = (trial_storey + 1) * 10;
            base_crt.MagicDefMax = (trial_storey + 1) * 20;
            base_crt.damageMin = (trial_storey + 1) * 10;
            base_crt.damageMax = (trial_storey + 1) * 20;
            base_crt.MagicdamageMin = (trial_storey + 1) * 10;
            base_crt.MagicdamageMax = (trial_storey + 1) * 20;
            base_crt.hit = (trial_storey + 1) * 10;
            base_crt.dodge = (trial_storey + 1) * 5;
            base_crt.penetrate = (trial_storey + 1) * 5;
            base_crt.block = (trial_storey + 1) * 5;
            base_crt.crit_rate = (trial_storey + 1) * 5;
            base_crt.crit_damage = 150 + (trial_storey + 1) * 5;
            base_crt.double_damage = (trial_storey + 1) * 5;
            base_crt.Lucky = (trial_storey + 1) / 10;
            base_crt.Real_harm = (trial_storey + 1) * 10;
            base_crt.Damage_Reduction = (trial_storey + 1) / 2;
            base_crt.move_speed = Random.Range(10, 30); ;
            base_crt.attack_speed = 300 - ((trial_storey + 1) * 2);
            base_crt.attack_distance = 100 + (trial_storey + 1) * 10;
            base_crt.Heal_Hp = (trial_storey + 1) * 10;
        }
        else
        {
            base_crt.MaxHP = (int)(crt.MaxHP * MathF.Pow(3, coefficient - 1));
            base_crt.MaxMp = crt.MaxMp;
            base_crt.internalforceMP = crt.internalforceMP;
            base_crt.EnergyMp = crt.EnergyMp;
            base_crt.DefMin = crt.DefMin * coefficient;
            base_crt.DefMax = crt.DefMax * coefficient;
            base_crt.MagicDefMin = crt.MagicDefMin * coefficient;
            base_crt.MagicDefMax = crt.MagicDefMax * coefficient;
            base_crt.damageMin = crt.damageMin * coefficient;
            base_crt.damageMax = crt.damageMax * coefficient;
            base_crt.MagicdamageMin = crt.MagicdamageMin * coefficient;
            base_crt.MagicdamageMax = crt.MagicdamageMax * coefficient;
            base_crt.hit = (crt.hit + map.need_lv) * coefficient;
            base_crt.dodge = crt.dodge * coefficient;
            base_crt.penetrate = crt.penetrate * coefficient;
            base_crt.block = crt.block * coefficient;
            base_crt.crit_rate = crt.crit_rate * coefficient;
            base_crt.crit_damage = crt.crit_damage;
            base_crt.double_damage = crt.double_damage;
            base_crt.Lucky = crt.Lucky;
            base_crt.Real_harm = crt.Real_harm;
            base_crt.Damage_Reduction = crt.Damage_Reduction;
            base_crt.Damage_absorption = crt.Damage_absorption;
            base_crt.resistance = crt.resistance;
            base_crt.move_speed = crt.move_speed;
            base_crt.attack_speed = crt.attack_speed;
            base_crt.attack_distance = crt.attack_distance;
            base_crt.bonus_Hp = crt.bonus_Hp;
            base_crt.bonus_Mp = crt.bonus_Mp;
            base_crt.bonus_Damage = crt.bonus_Damage;
            base_crt.bonus_MagicDamage = crt.bonus_MagicDamage;
            base_crt.bonus_Def = crt.bonus_Def;
            base_crt.bonus_MagicDef = crt.bonus_MagicDef;
            base_crt.Heal_Hp = crt.Heal_Hp * coefficient;
            base_crt.Heal_Mp = crt.Heal_Mp * coefficient;
        }
        Obtain_monster_state(base_crt, crt);
        return base_crt;
    }


    private static int[] SecretRealm_lvs = new int[] { 10, 30, 90, 120, 360, 480, 960, 1920, 4000, 4000, 4000, 4000, 4000, 4000, };
    /// <summary>
    /// 秘境boss
    /// </summary>
    /// <param name="crt"></param>
    /// <param name="map"></param>
    /// <param name="SecretRealm_lv"></param>
    /// <returns></returns>
    public static crtMaxHeroVO crate_SecretRealm_monster(crtMaxHeroVO crt, user_map_vo map, int SecretRealm_lv)
    {
        crtMaxHeroVO base_crt = new crtMaxHeroVO();
        base_crt.map_index = map.map_index;
        int life= Random.Range(0, 5);
        base_crt.life_types[life] = (SecretRealm_lv + 2) / 2;
        base_crt.life[life] = map.need_lv * 2 * (100 + life_bonus[base_crt.life_types[life]]) / 100 * SecretRealm_lvs[SecretRealm_lv] / 20;
        base_crt.Monster_Lv = map.map_type;
        base_crt.Type = crt.damageMax > crt.MagicdamageMax ? 1 : 2;
        base_crt.show_name = crt.show_name;
        base_crt.index = crt.index;
        base_crt.Lv = crt.Lv;
        base_crt.icon = crt.icon;
        base_crt.unit = Random.Range(crt.Lv * 5, crt.Lv * 10) + 1;
        //标准战斗系数
        float coefficient = 2;
        coefficient = coefficient * SecretRealm_lvs[SecretRealm_lv] / 100f;
        Debug.Log("伤害系数" + coefficient);
        coefficient=MathF.Max(coefficient, 0.1f);
        base_crt.Exp = (int)(crt.Exp * MathF.Pow(5, coefficient - 1));
        base_crt.MaxHP = (int)(crt.MaxHP * MathF.Pow(3, coefficient - 1));
        base_crt.MaxMp = crt.MaxMp;
        base_crt.internalforceMP = crt.internalforceMP;
        base_crt.EnergyMp = crt.EnergyMp;
        base_crt.DefMin = (int)(crt.DefMin * coefficient);
        base_crt.DefMax = (int)(crt.DefMax * coefficient);
        base_crt.MagicDefMin = (int)(crt.MagicDefMin * coefficient);
        base_crt.MagicDefMax = (int)(crt.MagicDefMax * coefficient);
        base_crt.damageMin = (int)(crt.damageMin * coefficient);
        base_crt.damageMax = (int)(crt.damageMax * coefficient);
        base_crt.MagicdamageMin = (int)(crt.MagicdamageMin * coefficient);
        base_crt.MagicdamageMax = (int)(crt.MagicdamageMax * coefficient);
        base_crt.hit = (int)((crt.hit + map.need_lv) * coefficient);
        base_crt.dodge = (int)(crt.dodge * coefficient);
        base_crt.penetrate = (int)(crt.penetrate * coefficient);
        base_crt.block = (int)(crt.block * coefficient);
        base_crt.crit_rate = (int)(crt.crit_rate * coefficient);
        base_crt.crit_damage = crt.crit_damage;
        base_crt.double_damage = crt.double_damage;
        base_crt.Lucky = crt.Lucky;
        base_crt.Real_harm = crt.Real_harm;
        base_crt.Damage_Reduction = crt.Damage_Reduction;
        base_crt.Damage_absorption = crt.Damage_absorption;
        base_crt.resistance = crt.resistance;
        base_crt.move_speed = crt.move_speed;
        base_crt.attack_speed = crt.attack_speed;
        base_crt.attack_distance = crt.attack_distance;
        base_crt.bonus_Hp = crt.bonus_Hp;
        base_crt.bonus_Mp = crt.bonus_Mp;
        base_crt.bonus_Damage = crt.bonus_Damage;
        base_crt.bonus_MagicDamage = crt.bonus_MagicDamage;
        base_crt.bonus_Def = crt.bonus_Def;
        base_crt.bonus_MagicDef = crt.bonus_MagicDef;
        base_crt.Heal_Hp = (int)(crt.Heal_Hp * coefficient);
        base_crt.Heal_Mp = (int)(crt.Heal_Mp * coefficient);
        Obtain_monster_state(base_crt, crt);
        //base_crt.MaxHP = 1;
        return base_crt;
    }

    /// <summary>
    /// 获得加成状态
    /// </summary>
    /// <param name="base_crt"></param>
    /// <param name="crt"></param>
    private static void Obtain_monster_state(crtMaxHeroVO base_crt,crtMaxHeroVO crt)
    {
      
    }

    /// <summary>
    /// 验证地图列表
    /// </summary>
    public static void tool_map()
    {
        for (int i = 0; i < SumSave.read_lose_map.Count; i++)
        {
            string value= SumSave.read_lose_map[i].ProfitList;
            string[] values = value.Split('&');
            if (values.Length > 1)
            {
                for (int j = 0; j < values.Length; j++)
                {
                    string[] values1 = values[j].Split(' ');
                    if (values1.Length == 3)
                    {
                        if (values1[0] != values1[2])
                            Debug.Log("配表错误 " + SumSave.read_lose_map[i].map_name + " " + values[j]);
                        else
                        {
                            Bag_Base_VO bag = ArrayHelper.Find(SumSave.db_stditems, e => e.Name == values1[0]);
                            if (bag == null) Debug.Log("连接错误 与数据库关联错误" + SumSave.read_lose_map[i].map_name + " " + values[j]);//对应的2个表格对不上
                        }
                    }
                    else Debug.Log(SumSave.read_lose_map[i].map_name + " " + values[j]);
                }
            }
            string[] monsters= SumSave.read_lose_map[i].monster_list.Split(' ');
            for (int j = 0; j < monsters.Length; j++)
            {
                if (monsters[j] != "")
                {
                    UI.UI_Manager.I.GetEquipSprite("Prefabs/monsters/", monsters[j]);
                }
            }
        }
    }



}

