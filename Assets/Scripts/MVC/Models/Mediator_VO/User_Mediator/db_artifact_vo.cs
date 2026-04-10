using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class db_artifact_vo : Base_VO
{
    /// <summary>
    /// 神器名称
    /// </summary>
    public readonly string arrifact_name;
    /// <summary>
    /// 神器ID
    /// </summary>
    public readonly int arrifact_type;
    /// <summary>
    /// 神器激活需求
    /// </summary>
    public readonly string[] Artifact_open_needs;
    /// <summary>
    /// 神器升级需求
    /// </summary>
    public readonly string[] arrifact_needs;
    /// <summary>
    /// 神器效果
    /// </summary>
    public readonly string[] arrifact_effects;
    /// <summary>
    /// 神器描述
    /// </summary>
    public readonly string Artifact_dec;
    /// <summary>
    /// 最大等级
    /// </summary>
    public readonly int Artifact_MaxLv;

    public db_artifact_vo(string arrifact_name, string[] artifact_open_needs, string[] arrifact_needs, string[] arrifact_effects, int arrifact_type, string artifact_dec, int artifact_MaxLv)
    {
        this.arrifact_name = arrifact_name;
        Artifact_open_needs = artifact_open_needs;
        this.arrifact_needs = arrifact_needs;
        this.arrifact_effects = arrifact_effects;
        this.arrifact_type = arrifact_type;
        Artifact_dec = artifact_dec;
        Artifact_MaxLv = artifact_MaxLv;
    }
}
