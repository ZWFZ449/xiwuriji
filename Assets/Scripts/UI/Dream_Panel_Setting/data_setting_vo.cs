using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class data_setting_vo : Base_VO
{
    /// <summary>
    /// 药品设置 1类型 2名称 3标准线
    /// </summary>
    public List<(int,string,int)> medicine_list = new List<(int,string,int)>();
    /// <summary>
    /// 装备回收设置 1品质2等级
    /// </summary>
    public List<(int, int)> battle_base_list = new List<(int, int)>();

    public List<(string,int)> battle_Boss_list1 = new List<(string,int)>();
    /// <summary>
    /// boss集合 1名称 2存量 3boss设置量
    /// </summary>
    public Dictionary<string, (int, int)> Boss_list = new Dictionary<string, (int, int)>();
    /// <summary>
    /// 0音效 1自动召唤 2自动集火boss
    /// </summary>
    public List<int> user_data_settings = new List<int>();
    public void Iint(string data_base_setting, string data_medicine_setting,string data_battle_setting,string user_data_settings)
    {
        base.Iint();
        //设置装备回收
        List<string> base_setting = ArrayHelper.Get_Split<string>(data_base_setting, ',');
        for (int i = 0; i < base_setting.Count; i++)
        { 
            List<string> arr = ArrayHelper.Get_Split<string>(base_setting[i], ';');
            if (arr.Count == 2)
            { 
                (int, int) data = (int.Parse(arr[0]), int.Parse(arr[1]));
                battle_base_list.Add(data);
            }
        }
        //设置药品
        List<string> list = ArrayHelper.Get_Split<string>(data_medicine_setting, ',');
        for (int i = 0; i < list.Count; i++)
        { 
            List<string> arr = ArrayHelper.Get_Split<string>(list[i], ';');
            if (arr.Count == 3)
            {
                (int, string, int) data = (int.Parse(arr[0]), arr[1], int.Parse(arr[2]));
                medicine_list.Add(data);
            }
        }
        //自动boss
        List<string> battle_list = ArrayHelper.Get_Split<string>(data_battle_setting, ',');
        for (int i = 0; i < battle_list.Count; i++)
        { 
            List<string> arr = ArrayHelper.Get_Split<string>(battle_list[i], ';');
            if (arr.Count == 2)
            { 
                (string, int) data = (arr[0], int.Parse(arr[1]));
                //this.battle_Boss_list.Add(data);
                List<string> datalist = ArrayHelper.Get_Split<string>(data.Item1, '+');
                if (datalist.Count == 2)
                {
                    if (!Boss_list.ContainsKey(datalist[0]))
                    { 
                        Boss_list.Add(datalist[0], (int.Parse(datalist[1]), data.Item2));
                    }
                }
            }
        }
        this.user_data_settings = ArrayHelper.Get_Split<int>(user_data_settings, ',');
    }

    public override string[] Set_Instace_String()
    {
        Iint(Setting(1), Setting(2), Setting(3), Setting(4));
        return new string[]
        {
            GetStr(0),
            GetStr(SumSave.crt_user.uid),
            GetStr(GetData(1)),
            GetStr(GetData(2)),
            GetStr(GetData(3)),
            GetStr(GetData(4)),
            GetStr("")
        };
    }
    /// <summary>
    /// 设置初始化
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private string Setting(int index)
    {
        string dec = "";
        if (index == 1)
        {
            for (int i = 0; i < Enum.GetNames(typeof(enum_equip_quality_list)).Length; i++)
            {
                dec += (i + 1) + ";0,";
            }
        }

        if (index == 2)
        {
            for (int i = 0; i < Enum.GetNames(typeof(medicineType)).Length; i++)
            {
                for (int j = 0; j < SumSave.db_stditems.Count; j++)
                {
                    if (SumSave.db_stditems[j].StdMode == Stditem_StdMode_List.消耗品.ToString() && SumSave.db_stditems[j].Shape == (i+1))
                    {
                        dec += (i + 1) + ";" + SumSave.db_stditems[j].Name + ";" + (60 - (i * 10)) + ",";
                        break;
                    }
                }
            }
        }
        if (index == 3)
        {
            //dec += "稻草人[Boss]+" + SumSave.nowtime + ";0";
        }
        if (index == 4)
        {
            for (int i = 0; i < SumSave.db_sttings.Count; i++)
            {
                dec += SumSave.db_sttings[i].setting_type + ",";
            }
        }
        return dec;
    }

    private string GetData(int index)
    { 
        string dec = "";
        switch (index)
        {
            case 1:
                for (int i = 0; i < battle_base_list.Count; i++)
                { 
                    dec += battle_base_list[i].Item1 + ";" + battle_base_list[i].Item2 + ",";
                }
                break;
            case 2:
                for (int i = 0; i < medicine_list.Count; i++)
                { 
                    dec += medicine_list[i].Item1 + ";" + medicine_list[i].Item2 + ";" + medicine_list[i].Item3 + ",";
                }
                break;
            case 3:
                foreach (var item in Boss_list)
                { 
                    dec += item.Key + "+" + item.Value.Item1 + ";" + item.Value.Item2 + ",";
                }
                break;
            case 4:
                for (int i = 0; i < user_data_settings.Count; i++)
                {
                    dec += user_data_settings[i] + ",";
                }
                break;
            default:
                break;
        }
        return dec ;
    }

    public void SetData(List<(int,int,string,int)> data)
    {

        for (int i = 0; i < data.Count; i++)
        {
            switch (data[i].Item1)
            { 
                case 1:
                    bool exist = true;
                    for (int j = 0; j < battle_base_list.Count; j++)
                    {
                        if (battle_base_list[j].Item1 == data[i].Item2)
                        {
                            exist= false;
                            battle_base_list[j] = (data[i].Item2, data[i].Item4);
                        }
                    }
                    if (exist)
                    { 
                        battle_base_list.Add((data[i].Item2, data[i].Item4));
                    }
                    break;
                case 2:
                    for (int j = 0; j < medicine_list.Count; j++)
                    {
                        if (medicine_list[j].Item1 == data[i].Item2)
                        {
                            medicine_list[j] = (data[i].Item2, data[i].Item3, data[i].Item4);
                        }
                    }
                    break;
                case 3: 
                    if (!Boss_list.ContainsKey(data[i].Item3))
                    {
                        Boss_list.Add(data[i].Item3, (data[i].Item2, data[i].Item4));
                    }
                    else
                    { 
                        Boss_list[data[i].Item3] = (Boss_list[data[i].Item3].Item1, data[i].Item4);
                    }
                    break;
                case 4:
                    bool isTrue = true;
                    for (int j = 0; j < user_data_settings.Count; j++)
                    {
                        if (j == data[i].Item2)
                        {
                            isTrue = false;
                            user_data_settings[j] = data[i].Item4;
                        }
                    }
                    if (isTrue)
                    {
                        if (user_data_settings.Count <= data[i].Item2)
                        {
                            while (user_data_settings.Count <= data[i].Item2)
                            { 
                                user_data_settings.Add(1);
                            }
                            user_data_settings[data[i].Item2] = data[i].Item4;
                        } 
                    }
                    break;
            }
        }
        MysqlData();
    }

    public override string[] Get_Update_Character()
    {
        return new string[]
        {
            "data_base_setting",
            "data_medicine_setting",
            "data_battle_boss_setting",
            "data_setting",
        };
    }

    public override string[] Set_Uptade_String()//Set_Uptade_String
    {
        return new string[]
        {
            GetStr(GetData(1)),
            GetStr(GetData(2)),
            GetStr(GetData(3)),
            GetStr(GetData(4))
        };
    }

    public override void MysqlData()
    {
        base.MysqlData();
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.user_data_settings, Set_Uptade_String(), Get_Update_Character());
    }
}
