using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dream_user_towerbabel_vo : Base_VO
{

    public string towerbabel_max;

    public string towerbabel_skill;

    public string towerbabel_artifact;

    public string towerbabel_value;

    public string backup;

    private Dictionary<string, db_towerbabel_vo> dict_skill = new Dictionary<string, db_towerbabel_vo>();

    private Dictionary<string, db_towerbabel_vo> dict_artifact = new Dictionary<string, db_towerbabel_vo>();

    private List<int> list_number = new List<int>();
    public void Init()
    {
        Init_Data(dict_skill, towerbabel_skill);
        Init_Data(dict_artifact, towerbabel_artifact);
        list_number = ArrayHelper.Get_Split<int>(towerbabel_max, ',');
    }
    /// <summary>
    /// 获取数据
    /// </summary>
    /// <param name="dict"></param>
    /// <param name="str"></param>
    private void Init_Data(Dictionary<string, db_towerbabel_vo> dict, string str)
    {
        List<string> list = ArrayHelper.Get_Split<string>(str, '|');
        for (int i = 0; i < list.Count; i++)
        {
            List<string> list2 = ArrayHelper.Get_Split<string>(list[i], ',');
            if (list2.Count == 2)
            {
                db_towerbabel_vo vo = ArrayHelper.Find(SumSave.db_towerbabels, e => e.id == int.Parse(list2[0]));
                if (vo != null)
                {
                    vo.user_lv = int.Parse(list2[1]);
                    if (!dict.ContainsKey(vo.TowerBabel_name))
                        dict.Add(vo.TowerBabel_name, vo);
                }
            }
        }
    }

    public override string[] Set_Instace_String()
    {
        towerbabel_max = "";
        towerbabel_skill = "";
        towerbabel_artifact = "";
        towerbabel_value = "";
        backup = "";
        Init();
        return new string[]
        {
            GetStr(0),
            GetStr(SumSave.uid),
            GetStr(towerbabel_max),
            GetStr(towerbabel_skill),
            GetStr(towerbabel_artifact),
            GetStr(towerbabel_value),
            GetStr(backup)
        };
    }
    public override string[] Set_Uptade_String()
    {
        return
           new string[]
           {
                GetStr(GetMax()),
                GetStr(GetDataSkill(dict_skill)),
                GetStr(GetDataSkill(dict_artifact)),
           };
    }

    private string GetMax()
    {
        string dec = "";
        for (int i = 0; i < list_number.Count; i++)
        { 
            dec += list_number[i] + ",";
        }    
        return dec;
    }

    public override string[] Get_Update_Character()
    {
        return
            new string[]
            {
                "towerbabel_max",
                "towerbabel_skill",
                "towerbabel_artifact",
            };
    }

    /// <summary>
    /// 获取技能
    /// </summary>
    public Dictionary<string, db_towerbabel_vo> GetSkill { get { return dict_skill; } }
    public Dictionary<string, db_towerbabel_vo> GetArtifact { get { return dict_artifact; } }

    public List<int> GetListNumber { get { return list_number; } }

    public void SetMax(int index,int value)
    {
        if (list_number.Count > index)
        {
            list_number[index] = (int)MathF.Max(value, list_number[index]);

        }
        else
        { 
            list_number.Add(value);
        }
        MysqlData();
    }
    public void SetSKill(db_towerbabel_vo vo)
    {
        Setdata(dict_skill, vo);
    }
    public void SetArtifact(db_towerbabel_vo vo)
    {
        Setdata(dict_artifact, vo);
    }
    private void Setdata(Dictionary<string, db_towerbabel_vo> dict, db_towerbabel_vo vo)
    {
        if (!dict.ContainsKey(vo.TowerBabel_name))
        {
            dict.Add(vo.TowerBabel_name, vo);
        }
        else
        {
            dict[vo.TowerBabel_name] = vo;
        }
        MysqlData();
    }
    private string GetDataSkill(Dictionary<string, db_towerbabel_vo> dict)
    { 
        string str = "";
        foreach (var item in dict)
        {
            str+= item.Value.id + "," + item.Value.user_lv + "|";
        }
        return str;
    }

    public override void MysqlData()
    {
        //写入数据
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.dream_user_towerbabel, Set_Uptade_String(), Get_Update_Character());
        base.MysqlData();
    }
}
