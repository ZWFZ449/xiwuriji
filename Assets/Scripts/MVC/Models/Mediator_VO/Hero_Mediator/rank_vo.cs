using Common;
using MVC;
using System.Collections.Generic;

public class rank_vo : Base_VO
{

    public int ranking_server;

    public string Ranking_value;

    public List<base_rank_vo> lists = new List<base_rank_vo>();

    public string SetData()
    {
        string value = "";
        foreach (base_rank_vo item in lists)
        {
            value += item.GetPropertyValue(item);

            value += ';';
        }
        return value;
    }

    public override string[] Set_Instace_String()
    {
        return
            new string[]
            {
                GetStr(0),
                GetStr(SumSave.par),
                GetStr("")
            };
    }

    public override string[] Get_Update_Character()
    {
        return new string[] { "value" };
    }

    public override string[] Set_Uptade_String()
    {
        return new string[] { GetStr(SetData())};
    }

}