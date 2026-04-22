using Common;
using MVC;
using System.Collections.Generic;

public class dream_user_pet_vo : Base_VO
{

    /// <summary>
    /// 当前宠物
    /// </summary>
    private db_pet_vo crt_pet;

    private List<db_pet_vo> sum_pet;

    public void Init(string _crt_pet,string _sum_pet)
    {
        crt_pet = InitPet(_crt_pet, ',');
        sum_pet = new List<db_pet_vo>();
        List<string> crt_sum_pet_list = ArrayHelper.Get_Split<string>(_sum_pet, '|');
        for (int i = 0; i < crt_sum_pet_list.Count; i++)
        {
            if (crt_sum_pet_list[i] != "")
                sum_pet.Add(InitPet(crt_sum_pet_list[i], ','));
        }
    }
    private db_pet_vo InitPet(string value, char split)
    {
        if(value=="")return null;
        List<string> crt_pet_list = ArrayHelper.Get_Split<string>(value, split);
        db_pet_vo base_pet = ArrayHelper.Find(SumSave.db_pets, (item) => item.pet_id == int.Parse(crt_pet_list[0]));
        if (base_pet != null)
        {
            db_pet_vo vo = new db_pet_vo(base_pet.pet_id, base_pet.pet_name, base_pet.pet_ac, base_pet.pet_mac, base_pet.pet_dc, base_pet.pet_mc, base_pet.pet_sc, base_pet.pet_talent, base_pet.pet_scale);
            vo.Init(crt_pet_list[1],
                ArrayHelper.Get_Split<int>(crt_pet_list[3], 'X'),
                ArrayHelper.Get_Split<int>(crt_pet_list[4], 'X'),
                ArrayHelper.Get_Split<string>(crt_pet_list[5], 'X')
                );
            return vo;
        }else return null;
    }
    /// <summary>
    /// 添加宠物
    /// </summary>
    /// <param name="pet"></param>
    public void AddPet(pet_list pet)
    { 
        sum_pet.Add(InitPet(Tool_Battle.Obtain_Pet((int)pet),','));
        MysqlData();
    }
    /// <summary>
    /// 获取宠物属性
    /// </summary>
    /// <returns></returns>
    public db_pet_vo GetPet { get { return crt_pet; } }

    public db_pet_vo SetPet { set { crt_pet = value; MysqlData(); } } 
    /// <summary>
    /// 宠物列表
    /// </summary>
    public List<db_pet_vo> GetPets { get { return sum_pet; } }

    public List<db_pet_vo> SetPets { set { sum_pet = value; MysqlData(); } }
    public override string[] Set_Instace_String()
    {
        Init(Tool_Battle.Obtain_Pet(0), "");
        return new string[]
        {
            GetStr(0),
            GetStr(SumSave.crt_user.uid),
            GetStr(GetSumData(crt_pet)),
            GetStr(GetStr()),
            GetStr("")
        };
    }
    public override string[] Get_Update_Character()
    {
        return new string[]
        {
            "crt_pet",
            "sum_pet"
        };
    }
    public override string[] Set_Uptade_String()
    {
        return new string[]
        {
            GetStr(GetSumData(crt_pet)),
            GetStr(GetStr())
        };
    }
    private string GetStr()
    {
        string str = "";
        if (sum_pet.Count > 0)
        {
            for (int i = 0; i < sum_pet.Count; i++)
            {
                str+= GetSumData(sum_pet[i]) + ((i == sum_pet.Count - 1) ? "" : "|");
            }

        }
        return str;
    }
    private string GetSumData(db_pet_vo data)
    { 
        string str = "";
        str += data.pet_id + "," + data.crt_name + "," + data.pet_name + ",";
        (int,int,int,int,int) attr = data.GetCrtAttr;
        str += attr.Item1 + "X" + attr.Item2 + "X" + attr.Item3 + "X" + attr.Item4 + "X" + attr.Item5 + ",";
        (int, int, int, int, int) addarr = data.GetAddAttr;
        str += addarr.Item1 + "X" + addarr.Item2 + "X" + addarr.Item3 + "X" + addarr.Item4 + "X" + addarr.Item5 + ",";
        List<db_pet_talent_vo> Talents = data.GetCrtTalent;
        for (int i = 0; i < Talents.Count; i++)
        {
            str += Talents[i].pet_talent_name + ((i == Talents.Count - 1) ? "," : "X");
        }
        return str;
    }

    public override void MysqlData()
    {
        base.MysqlData();
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.dream_user_pet,
Set_Uptade_String(), Get_Update_Character());
    }
}
