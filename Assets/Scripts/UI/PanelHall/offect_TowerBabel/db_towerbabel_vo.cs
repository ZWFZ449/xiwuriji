using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class db_towerbabel_vo : Base_VO
{
    public int id;
    public int towerbabel_index;

    public string TowerBabel_name;

    public int TowerBabel_type;

    public string activate_offect;

    public string up_offect;

    public int max_lv;

    public string need_offect;

    public string need_activate;
    /// <summary>
    /// µÈ¼¶
    /// </summary>
    public int user_lv = 0;
}
