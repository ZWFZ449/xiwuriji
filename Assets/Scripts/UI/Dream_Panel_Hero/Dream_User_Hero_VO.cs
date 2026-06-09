using Common;
using MVC;
using System;
using System.Collections.Generic;

public class Dream_User_Hero_VO : Base_VO
{
    public int par;
    //public string uid;
    public string heroId;
    public string hero_name;
    private int _job;
    public int job
    { 
       get { return _job; }
       set { _job = value; if(SumSave.crt_skill != null) SumSave.crt_skill.Select_Job(); }
    }
    public int lv;
    public long exp;
    public List<(db_player_talent_vo,int)> talent;
    public int SelectPos;
    public int zs_lvs;
    public string user_value;

    public override void Iint()
    {
        base.Iint();
    }

    public override string[] Set_Instace_String()
    {
        par = SumSave.par;
        heroId = Guid.NewGuid().ToString("N"); 
        hero_name = GetNameHelper.GetManName();
        job = 0;
        lv = 1;
        exp = 0;
        talent = new List<(db_player_talent_vo, int)>();
        SelectPos = -1;
        zs_lvs = 1;
        user_value = "";
        return new string[]
        {
            GetStr(0),
            GetStr(par),
            GetStr(SumSave.uid),
            GetStr(heroId),
            GetStr(hero_name),
            GetStr(job),
            GetStr(lv),
            GetStr(exp),
            GetStr(Gettalent()),
            GetStr(SelectPos),
            GetStr(zs_lvs),
            GetStr(user_value)
        };
    }

    public override string[] Get_Update_Character()
    {
        return new string[]
        {
            "hero_name",
            "job",
            "Lv",
            "Exp",
            "SelectPos",
            "zs_lv",
            "Talent",
        };
    }
    public override string[] Set_Uptade_String()
    {
        return new string[]
            {
                GetStr(hero_name),
                GetStr(_job),
                GetStr(lv),
                GetStr(exp),
                GetStr(SelectPos),
                GetStr(zs_lvs),
                GetStr(Gettalent()),
            };
    }
    public override void MysqlData()
    {
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.dream_base_hero, Set_Uptade_String(), Get_Update_Character());
        base.MysqlData();
        //写入数据
    }

    /// <summary>
    /// 获取天赋
    /// </summary>
    /// <returns></returns>
    private string Gettalent()
    { 
        string str = "";
        foreach (var item in talent)
        {
            str += item.Item1.talent_name + "," + item.Item2 + ";";
        }
        return str;
    }
}
