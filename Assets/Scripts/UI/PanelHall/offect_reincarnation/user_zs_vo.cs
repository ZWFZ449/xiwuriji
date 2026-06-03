using CodeStage.AntiCheat.ObscuredTypes;
using Common;
using MVC;
using System.Collections.Generic;

public class user_zs_vo : Base_VO
{
    /// <summary>
    /// 当前炼体最大值
    /// </summary>
    public ObscuredInt zs_Refinement_max;
    /// <summary>
    /// 当前炼药最大值
    /// </summary>
    public ObscuredInt zs_medicine_max;
    /// <summary>
    /// 当前炼药值
    /// </summary>
    public List<ObscuredInt> crt_medicine;
    /// <summary>
    /// 炼药经验值
    /// </summary>
    public ObscuredInt medicine_exp;
    /// <summary>
    ///  当前炼体值
    /// </summary>
    public List<ObscuredInt> crt_Refinement;


    public override string[] Set_Instace_String()
    {
        return new string[]
        {
        GetStr(0),
        GetStr(SumSave.uid),
        GetStr(zs_Refinement_max),
        GetStr(zs_medicine_max),
        GetStr("0"),
        GetStr(medicine_exp),
        GetStr("0")
        };
    }

    public override string[] Get_Update_Character()
    {
        return new string[]
        {
        "zs_Refinement_max",
        "zs_medicine_max",
        "crt_medicine",
        "medicine_exp",
        "crt_Refinement"
        };
    }

    public override string[] Set_Uptade_String()
    {
        return new string[]
        {
        GetStr(zs_Refinement_max),
        GetStr(zs_medicine_max),
        GetStr(string.Join(",", crt_medicine)),
        GetStr(medicine_exp),
        GetStr(string.Join(",", crt_Refinement))
        };
    }

    public override void MysqlData()
    {
        base.MysqlData();
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.dream_user_zs, Set_Uptade_String(), Get_Update_Character());

    }
}
