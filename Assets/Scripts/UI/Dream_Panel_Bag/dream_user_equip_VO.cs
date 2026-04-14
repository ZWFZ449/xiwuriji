using Common;
using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Dream_User_Equip_Type
{ 
装备,
仓库,
法宝
}
public class dream_user_equip_VO : Base_VO
{
    private List<Bag_Base_VO> Equip_List;
    private List<Bag_Base_VO> House_List;
    private List<Bag_Base_VO> Treasure_List;
    private int Page;

    public void Init(string equipvalue, string housevalue,string treasurevalue,int page)
    {
        Equip_List = toList(equipvalue, Equip_List);
        House_List = toList(housevalue, House_List);
        Treasure_List = toList(treasurevalue, Treasure_List);
        Page = page + 60;
    }
    private List<Bag_Base_VO> toList(string value, List<Bag_Base_VO> list)
    {
        list= new List<Bag_Base_VO>();
        string[] values = value.Split(';');
        for (int i = 0; i < values.Length; i++)
        {
            if (values[i].Length > 0)
            {
                list.Add(tool_Categoryt.Read_Bag(values[i]));
            }
        }

        return list;
    }

    public override string[] Set_Instace_String()
    {
        Init("", "", "", 100);
        return new string[]
        {
            GetStr(0),
            GetStr(SumSave.uid),
            GetStr(OnWirte(Equip_List)),
            GetStr(OnWirte(House_List)),
            GetStr(OnWirte(Treasure_List)),
            GetStr(0),
            GetStr("")
        };
    }
    /// <summary>
    /// 获取 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<Bag_Base_VO> Get(Dream_User_Equip_Type type)
    {
        switch (type)
        { 
        case Dream_User_Equip_Type.装备:return Equip_List;
        case Dream_User_Equip_Type.仓库:return House_List;
        case Dream_User_Equip_Type.法宝:return Treasure_List;
        default:return null;
        }
    }
    /// <summary>
    /// 获取页数
    /// </summary>
    public int GetPage { get { return Page; } }

    public void Set(Dream_User_Equip_Type type, List<Bag_Base_VO> list)
    {
        switch (type)
        { 
            case Dream_User_Equip_Type.装备:Equip_List = list; break;
            case Dream_User_Equip_Type.仓库:House_List = list; break;
            case Dream_User_Equip_Type.法宝:Treasure_List = list; break;
        }
        MysqlData();
    }

    public override void MysqlData()
    {
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.dream_user_equip, 
        Set_Uptade_String(), Get_Update_Character());
        base.MysqlData();
    }

    public override string[] Get_Update_Character()
    {
        return new string[]
        {
            "equip_value",
            "house_value",
            "treasure_value" 
        };
    }

    public override string[] Set_Uptade_String()
    {
        return new string[]
        {
            GetStr(OnWirte(Equip_List)),
            GetStr(OnWirte(House_List)),
            GetStr(OnWirte(Treasure_List))
        };
    }
}
