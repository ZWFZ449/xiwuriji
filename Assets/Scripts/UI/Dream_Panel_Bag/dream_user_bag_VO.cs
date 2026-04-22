using Common;
using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum dream_user_bag_Type
{ 
    nothing=-1,
    装备,
    消耗品,
    材料
}
public class dream_user_bag_VO : Base_VO
{
    /// <summary>
    /// 背包材料
    /// </summary>
    private bag_Resources_vo resources_List;
    /// <summary>
    /// 背包药品
    /// </summary>
    //private bag_Resources_vo drug_List;
    /// <summary>
    /// 背包道具
    /// </summary>
    private List<Bag_Base_VO> bag_List;

    private int Bag_page = 120;

    public void Init(string bag_valueS,string resources_value,string drug_value,int page)
    {
        resources_List = new bag_Resources_vo();
        bag_List = new List<Bag_Base_VO>();
        string[] bag_value = bag_valueS.Split(';');
        for (int i = 0; i < bag_value.Length; i++)
        {
            if (bag_value[i].Length > 0)
            {
                Bag_Base_VO bag = tool_Categoryt.Read_BaseBag(bag_value[i]);
                if (bag != null) bag_List.Add(bag);
            }
        }
        resources_List.Init(resources_value);
        Bag_page = page;
    }
    /// <summary>
    /// 获取背包数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<(string, int)> Set()
    {
        return resources_List.Set();
    }

    public void Get(Dictionary<string, int> dec, int maxnumber, bool exist = false)
    {
        resources_List.Get(dec, maxnumber, exist);
        Game_Omphalos.Refresh(Mysql_Table_Name.dream_user_bag);
        MysqlData();

    }
    /// <summary>
    /// 获取背包装备
    /// </summary>
    /// <returns></returns>
    public List<Bag_Base_VO> Get_Bag_List()
    { 
        return bag_List;
    }

    public void Set_Bag_List(Bag_Base_VO bag)
    { 
        bag_List.Add(bag);
        MysqlData();
    }
    public void Remove_Bag_List(Bag_Base_VO bag)
    {
        bag_List.Remove(bag);
        MysqlData();
    }

    public int Get_Page { get { return Bag_page; } }
    public void Set_Bag_List(List<Bag_Base_VO> bag)
    {
        bag_List = bag;
        MysqlData();
    }

    public override void MysqlData()
    {
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.dream_user_bag,
        Set_Uptade_String(), Get_Update_Character());
        base.MysqlData();
    }
    public override string[] Get_Update_Character()
    {
        return new string[]
        {
            "bag_value",
            "resources_value",
            "drug_value",
        };
    }

    public override string[] Set_Uptade_String()
    {
        return new string[]
       {
            GetStr(OnWirte(bag_List)),
            GetStr(resources_List.GetData()),
            GetStr(""),
       };
    }
    public override string[] Set_Instace_String()
    {
        Init("", "", "", 120);
        return new string[]
        {
            GetStr(0),
            GetStr(SumSave.uid),
            GetStr(OnWirte(bag_List)),
            GetStr(resources_List.GetData()),
            GetStr(""),
            GetStr(Bag_page),
            GetStr("")
        };
    }
}
