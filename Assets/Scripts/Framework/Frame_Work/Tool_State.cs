using Common;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using MVC;
using System.Text;
/// <summary>
/// 状态类列表
/// </summary>
public enum State_List
{
    经验丹 = 1,
    历练丹,
    月卡,
    天气,
    至尊卡,

}
/// <summary>
/// 验证状态
/// </summary>
public  static class Tool_State 
{
    private static Dictionary<State_List,bool> state_list = new Dictionary<State_List, bool>();

    /// <summary>
    /// 验证状态
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public static bool IsState(State_List state)
    {
        self_inspection();
        if (state_list.ContainsKey(state))
        { 
            return state_list[state];
        }
        return state_list[state];
    }
    /// <summary>
    /// 获取临时状态
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public static int GetSetMapState(string state)
    {
        
        List<(string, int)> list = SumSave.crt_needlist.SetMap();
        List<Bag_Base_VO> sell_list = new List<Bag_Base_VO>();
        int number = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Item1 == state)
            {
                number = list[i].Item2;
            }
        }
        return number;
    }

    /// <summary>
    /// 介绍物品
    /// </summary>
    /// <param name="item1"></param>
    /// <returns></returns>
    public static string Show_Stditems(string item1)
    {
        string dec="";
        Bag_Base_VO item = ArrayHelper.Find(SumSave.db_stditems, e => e.Name == item1);
        if (item != null)
        {
            Stditem_StdMode_List type = ToEnum<Stditem_StdMode_List>(item.StdMode);
            switch (type)
            {
                case Stditem_StdMode_List.衣服:
                    break;
                case Stditem_StdMode_List.武器:
                    break;
                case Stditem_StdMode_List.戒指:
                    break;
                case Stditem_StdMode_List.项链:
                    break;
                case Stditem_StdMode_List.手镯:
                    break;
                case Stditem_StdMode_List.头盔:
                    break;
                case Stditem_StdMode_List.靴子:
                    break;
                case Stditem_StdMode_List.腰带:
                    break;
                case Stditem_StdMode_List.勋章:
                    break;
                case Stditem_StdMode_List.遗物_钳:
                    break;
                case Stditem_StdMode_List.遗物_足:
                    break;
                case Stditem_StdMode_List.消耗品:
                    if (item.hp > 0) dec += "生命回复 " + Show_Color.Orange(item.hp) + " ";
                    if (item.mp > 0) dec += (dec == "" ? "" : "\n") + "魔法回复 " + Show_Color.Green(item.mp) + " ";
                    break;
                case Stditem_StdMode_List.材料:
                    dec += item.dec;
                    break;
                case Stditem_StdMode_List.nothing:
                    break;
                case Stditem_StdMode_List.货币:
                    dec += item.dec;

                    break;
                case Stditem_StdMode_List.宝石:
                    dec += item.dec;

                    break;
            }
            dec+="\n"+item.dec;
        }
        return dec;
    }
    /// <summary>
    /// 获取string和枚举的对应关系
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static T ToEnum<T>(this string value, T defaultValue = default) where T : struct
    {
        if (string.IsNullOrWhiteSpace(value)) return defaultValue;
        return Enum.TryParse(value, true, out T result) ? result : defaultValue;
    }
    /// <summary>
    /// 获取buff加成
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static int Value_playerprobabilit(enum_equip_entry_list type)
    {
        int value = 100;
        switch (type)
        {
            case enum_equip_entry_list.怪物爆率:
                value = SumSave.crtMaxBattle.drop_bonus;
                break;
            case enum_equip_entry_list.极品爆率:
                value= SumSave.crtMaxBattle.quality_bonus;
                break;
            case enum_equip_entry_list.经验加成:
                value = SumSave.crtMaxBattle.exp_bonus;
                break;
            case enum_equip_entry_list.金币掉落:
                value = SumSave.crtMaxBattle.gold_bonus;
                break;
        }
        return value;
    }


    /// <summary>
    /// 激活状态
    /// </summary>
    /// <param name="state"></param>
    public static void activation_State(State_List state)
    {
        if (state_list.ContainsKey(state))
        {
             state_list[state]=true;
        }else state_list.Add(state, true);
    }
    /// <summary>
    /// 每次进入地图验证
    /// </summary>
    public static void self_inspection()
    {

        if(state_list.Count != Enum.GetNames(typeof(State_List)).Length)
        {
            for (int i = 1; i < Enum.GetNames(typeof(State_List)).Length + 1; i++)
            {
                state_list.Add((State_List)(i), false);
            }
        }

        for (int i = 1; i < state_list.Count + 1; i++)
        {
            if (SumSave.crt_player_buff.player_Buffs.Count > 0)
            {
                foreach (var item in SumSave.crt_player_buff.player_Buffs)
                {
                    (DateTime, int, float, int) time = item.Value;
                    if (time.Item4 == i)//
                    {
                        int remainingTime = Battle_Tool.SettlementTransport((time.Item1).ToString("yyyy-MM-dd HH:mm:ss"), 2);
                        if (remainingTime < time.Item2 * 60)//有效期内
                        {
                            state_list[(State_List)(i)] = true;
                        }
                        else
                        {
                            SumSave.crt_player_buff.player_Buffs.Remove(item.Key);
                            return;
                        }

                    }

                }
            }

        }
    }
}
