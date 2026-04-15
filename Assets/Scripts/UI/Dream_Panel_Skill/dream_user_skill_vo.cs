using Common;
using MVC;
using System.Collections.Generic;

public class dream_user_skill_vo : Base_VO
{
    /// <summary>
    /// 当前技能
    /// </summary>
    private Dictionary<int, db_skill_vo> user_current_skill;
    /// <summary>
    /// 全部技能
    /// </summary>
    private Dictionary<int, db_skill_vo> user_sum_skill;
    /// <summary>
    /// 上阵技能
    /// </summary>
    private List<int> user_select_skill_type;
    /// <summary>
    /// 解析值
    /// </summary>
    private string user_current_skill_value, user_sum_skill_value, user_select_skill_type_value;
    
    public override string[] Set_Instace_String()
    {
        db_Hero_VO hero = ArrayHelper.Find(SumSave.db_heros, (x) => x.id == SumSave.crtHero.job);
        db_skill_vo skill = ArrayHelper.Find(SumSave.db_skills, (x) => x.show_name == hero.initskill);
        string select_skill_type = skill.id + " " + skill.id + " " + skill.id + " " + skill.id + " " + skill.id;
        Init(skill.id + " " + 0 + " " + 0, skill.id + " " + 0 + " " + 0, select_skill_type);
        return new string[]
        {
            GetStr(0),
            GetStr(SumSave.uid),
            GetStr(user_current_skill_value),
            GetStr(user_sum_skill_value),
            GetStr(user_select_skill_type_value)
        };
    }
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="current_skill"></param>
    /// <param name="sum_skill"></param>
    /// <param name="select_skill_type"></param>
    public void Init(string current_skill, string sum_skill, string select_skill_type)
    {
        user_current_skill = analysis_value(current_skill);
        user_current_skill_value= analysis_value(user_current_skill);
        user_sum_skill = analysis_value(sum_skill);
        user_sum_skill_value = analysis_value(user_sum_skill);
        user_select_skill_type = new List<int>();
        string[] skill_type = select_skill_type.Split(' ');
        for (int i = 0; i < skill_type.Length; i++)
        {
            if (!string.IsNullOrEmpty(skill_type[i])) user_select_skill_type.Add(int.Parse(skill_type[i]));
        }
        if (user_select_skill_type.Count <= 4)
        {
            for (int i = user_select_skill_type.Count; i < 4; i++)
            {
                db_Hero_VO hero = ArrayHelper.Find(SumSave.db_heros, (x) => x.id == SumSave.crtHero.job);
                user_select_skill_type.Add (hero.id);
            }
        }
        user_select_skill_type_value = analysis_value(user_select_skill_type);
    }
    /// <summary>
    /// 解析字符串
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private Dictionary<int, db_skill_vo> analysis_value(string value)
    {
        Dictionary<int, db_skill_vo> list = new Dictionary<int, db_skill_vo>();
        string[] skill = value.Split('&');
        for (int i = 0; i < skill.Length; i++)
        {
            if (!string.IsNullOrEmpty(skill[i]))
            {
                string[] skill_info = skill[i].Split(' ');
                {
                    if (!string.IsNullOrEmpty(skill_info[i]))
                    {
                        db_skill_vo skill_vo = ArrayHelper.Find(SumSave.db_skills, (x) => x.id.ToString() == skill_info[0]);
                        if (skill_vo != null)
                        {
                            if (skill_info.Length >= 3)
                            {
                                skill_vo.Init(int.Parse(skill_info[1]), int.Parse(skill_info[2]));
                                if (!list.ContainsKey(skill_vo.id))
                                    list.Add(skill_vo.id, skill_vo);
                            }
                        }
                    }
                }
            }
        }
        return list;
    }

    /// <summary>
    /// 获取上阵技能
    /// </summary>
    /// <returns></returns>
    public List<int> Set_Select_Skill_Type()
    {
        return user_select_skill_type;
    }
    /// <summary>
    /// 设置上阵技能
    /// </summary>
    public List<int> Get_Select_Skill_Type { 
        set{ 
            user_select_skill_type = value;
            user_select_skill_type_value = analysis_value(user_select_skill_type);
            MysqlData();
        } 
    }

    public Dictionary<int, db_skill_vo> Get_Current_Skill
    {
        set
        {
            user_current_skill = value;
            user_current_skill_value = analysis_value(user_current_skill);
            MysqlData();
        }
    }
    
    private string analysis_value(Dictionary<int, db_skill_vo> value)
    { 
        string str = "";
        foreach (var item in value)
        {
            str += (str == "" ? "" : "&") + item.Value.id + " " + item.Value.SetLv() + " " + item.Value.SetExp();
        }
        return str;
    }

    /// <summary>
    /// 解析技能
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    private string analysis_value(List<int> list)
    {
        string str = "";
        foreach (var item in list)
        {
            str += (str == "" ? "" : " ") + item;
        }
        return str;
    }

    public override string[] Get_Update_Character()
    {
        return new string[]
            {
            "user_current_skill",
            "user_sum_skill",
            "user_select_skill_type"
            };
    }

    public override string[] Set_Uptade_String()
    {
        return new string[]
        {
            GetStr(user_current_skill_value),
            GetStr(user_sum_skill_value),
            GetStr(user_select_skill_type_value)
        };
    }

    public override void MysqlData()
    {
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, 
            Mysql_Table_Name.dream_user_skill,
            Set_Uptade_String(), 
            Get_Update_Character());
        base.MysqlData();
    }
    /// <summary>
    /// 获取当前技能
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, db_skill_vo> Set_Current_skill()
    {
        return user_current_skill;
    }
    /// <summary>
    /// 激活技能
    /// </summary>
    /// <param name="skill"></param>
    public void activate_skill(db_skill_vo skill)
    {
        if (!user_sum_skill.ContainsKey(skill.id))
            user_sum_skill.Add(skill.id, skill);
        user_sum_skill_value = analysis_value(user_sum_skill);
        if(!user_current_skill.ContainsKey(skill.id))
        user_current_skill.Add(skill.id, skill);
        user_current_skill_value = analysis_value(user_current_skill);
        MysqlData();
    }
    /// <summary>
    /// 升级技能
    /// </summary>
    /// <param name="skill"></param>
    public void UpLv_skill(List<db_skill_vo> db_Skills)
    {
        for (int i = 0; i < db_Skills.Count; i++)
        {
            if(user_current_skill.ContainsKey(db_Skills[i].id))
                user_current_skill[db_Skills[i].id] = db_Skills[i];
            if (user_sum_skill.ContainsKey(db_Skills[i].id))
                user_sum_skill[db_Skills[i].id] = db_Skills[i];
        }
        user_current_skill_value = analysis_value(user_current_skill);
        user_sum_skill_value = analysis_value(user_sum_skill);
        MysqlData();
    }
    public void UpLv_skill()
    {
        user_current_skill_value = analysis_value(user_current_skill);
        user_sum_skill_value = analysis_value(user_sum_skill);
        MysqlData();
    }
    /// <summary>
    /// 切换职业
    /// </summary>
    public void Select_Job()
    {
        user_current_skill.Clear();
        db_Hero_VO hero = ArrayHelper.Find(SumSave.db_heros, (x) => x.id == SumSave.crtHero.job);
        db_skill_vo skill = ArrayHelper.Find(SumSave.db_skills, (x) => x.show_name == hero.initskill);
        bool exist = true;
        foreach (var item in user_sum_skill)
        {
            if (item.Value.show_name == skill.show_name) exist = false;//判断有初始技能
            if (item.Value.Job == SumSave.crtHero.job)
            {
                user_current_skill.Add(item.Key, item.Value);
            }
        }
        if (exist)
        {
            skill.activate_skill();
            activate_skill(skill);
        }
        UpLv_skill();
    }
}
