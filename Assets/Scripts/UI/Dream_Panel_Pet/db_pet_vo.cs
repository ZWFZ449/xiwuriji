using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class db_pet_vo 
{
    public readonly int pet_id;
    public readonly string pet_name;
    public readonly int pet_ac;
    public readonly int pet_mac;
    public readonly int pet_dc;
    public readonly int pet_mc;
    public readonly int pet_sc;
    public readonly string pet_talent;
    public readonly float pet_scale;
    /// <summary>
    /// 名字
    /// </summary>
    public string crt_name; 
    /// <summary>
    /// 初始属性
    /// </summary>
    private (int,int,int,int,int) crt_attr;
    /// <summary>
    /// 成长属性
    /// </summary>
    private (int,int,int,int,int) add_attr;
    /// <summary>
    /// 技能
    /// </summary>
    private List<db_pet_talent_vo> crt_talents;
    public db_pet_vo(int pet_id, string pet_name, int pet_ac, int pet_mac, int pet_dc, int pet_mc, int pet_sc, string pet_talent, float pet_scale)
    { 
        this.pet_id = pet_id;
        this.pet_name = pet_name;
        this.pet_ac = pet_ac;
        this.pet_mac = pet_mac;
        this.pet_dc = pet_dc;
        this.pet_mc = pet_mc;
        this.pet_sc = pet_sc;
        this.pet_talent = pet_talent;
        this.pet_scale = pet_scale;
        //测试

    }
    /// <summary>
    /// 初始化属性
    /// </summary>
    /// <param name="_crt_attr"></param>
    /// <param name="_add_attr"></param>
    /// <param name="_pet_talent"></param>
    public void Init(string _crt_name, List<int> _crt_attr,List<int> _add_attr,List<string> _pet_talent)
    { 
        crt_name = _crt_name;
        crt_attr = (_crt_attr[0], _crt_attr[1], _crt_attr[2], _crt_attr[3], _crt_attr[4]);
        add_attr = (_add_attr[0], _add_attr[1], _add_attr[2], _add_attr[3], _add_attr[4]);
        crt_talents = new List<db_pet_talent_vo>();
        for (int i = 0; i < _pet_talent.Count; i++)
        {
            db_pet_talent_vo talent = ArrayHelper.Find(SumSave.db_pet_talents, e => e.pet_talent_name == (_pet_talent[i]));
            if(talent != null)
                crt_talents.Add(talent);
        }
    }

    public (int, int, int, int, int) GetCrtAttr { get { return crt_attr; } }
    public (int, int, int, int, int) GetAddAttr { get { return add_attr; } }
    public List<db_pet_talent_vo> GetCrtTalent { get { return crt_talents; } }

}
