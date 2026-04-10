using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class db_player_talent_type
{
    /// <summary>
    /// 职业 战法道
    /// </summary>
    public readonly int talent_type_job;
    /// <summary>
    /// 类型名称
    /// </summary>
    public readonly List<string> talent_type_name = new List<string>();

    public db_player_talent_type(int talent_type_job, List<string> talent_type_name)
    { 
        this.talent_type_job = talent_type_job;

        this.talent_type_name = talent_type_name;
    }

}
