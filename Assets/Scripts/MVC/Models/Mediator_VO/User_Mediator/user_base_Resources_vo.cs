using Common;
using MVC;
using System;

public class user_base_Resources_vo:Base_VO
{
    /// <summary>
    /// 当前时间
    /// </summary>
    public DateTime now_time;
    /// <summary>
    /// 地图索引 
    /// </summary>
    public string user_map_index;
    /// <summary>
    /// 技能列表
    /// </summary>
    public string skill_value;
    /// <summary>
    /// 仓库列表
    /// </summary>
    public string house_value;
    /// <summary>
    /// 背包列表
    /// </summary>
    public string bag_value;
    /// <summary>
    /// 材料列表
    /// </summary>
    public string material_value;
    /// <summary>
    /// 装备列表
    /// </summary>
    public string equip_value;
    /// <summary>
    /// 页码
    /// </summary>
    public int[] pages;
    /// <summary>
    /// 获取时间
    /// </summary>
    /// <returns></returns>
    private string Get_Time()
    {
        if (SumSave.nowtime > now_time)
        {
            now_time = SumSave.nowtime;
        }
        return now_time.ToString();
    }
    public override string[] Set_Instace_String()
    {
        return new string[]
        {
        GetStr(0),
        GetStr(SumSave.crt_user.uid),        
        GetStr(now_time),
        GetStr(SumSave.read_lose_map[0].map_name),
        GetStr(skill_value),
        GetStr(house_value),
        GetStr(bag_value),
        GetStr(material_value),
        GetStr(equip_value),
        GetStr(ArrayHelper.Data_Encryption(pages))
        };
    }
    public void SetPage(int type)
    { 
       pages[type]++;
        MysqlData(); 
    }
    public override void MysqlData()
    {
        base.MysqlData();  
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto,
            Mysql_Table_Name.mo_user_value, SumSave.crt_resources.Set_Uptade_String(), SumSave.crt_resources.Get_Update_Character());

    }

    public override string[] Get_Update_Character()
    {
        return new string[] 
        { 
            "now_time",
            "user_map_index",
            "skill_value", 
            "house_value",
            "bag_value", 
            "material_value", 
            "equip_value",
            "page_value"
        };
    }
    public override string[] Set_Uptade_String()
    {
        return new string[]
        {
         GetStr(Get_Time()),
         GetStr(user_map_index),
         GetStr(skill_value),
         GetStr(house_value),
         GetStr(bag_value),
         GetStr(material_value),
         GetStr(equip_value),
         GetStr(ArrayHelper.Data_Encryption(pages))
        };
    }
}
    
