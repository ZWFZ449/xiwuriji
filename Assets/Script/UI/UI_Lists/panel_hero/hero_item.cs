using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hero_item : Base_Mono
{
    private Text base_info;
    private Image item_icon;
    private Transform skin;

    /// <summary>
    /// 角色皮肤预制体
    /// </summary>
    private GameObject skin_prefabs = null;
    private void Awake()
    {
        base_info = Find<Text>("base_info");
        skin = Find<Transform>("bg_icon/skin");
    }


    private db_hero_vo data;

    /// <summary>
    /// Data
    /// </summary>
    public db_hero_vo Data
    {
        set
        {
            data = value;

            if (data == null) return;

            base_info.text = data.hero_name;
            for (int j = 0; j < SumSave.old_db_heros.Count; j++)
            {
                if (data.hero_name == SumSave.old_db_heros[j].hero_name)
                { 
                    skin_prefabs = Resources.Load<GameObject>("Prefabs/Skins/within_" + SumSave.old_db_heros[j].hero_name.ToString());
                    Instantiate(skin_prefabs, skin);
                    return;
                }
            }
            //for (int j = 0; j < Enum.GetNames(typeof(enum_skin_state)).Length; j++)
            //{
            //    if (data.hero_name == ((enum_skin_state)(j + 1)).ToString())
            //    {
            //        skin_prefabs = Resources.Load<GameObject>("Prefabs/Skins/within_" + data.hero_name.ToString());
            //        Instantiate(skin_prefabs, skin);
            //        return;
            //    }
            //}
            //Debug.LogError("enum_attribute_list枚举类中没有该皮肤:" + data.hero_name.ToString());


            //读取显示图标
        }
        get
        {
            return data;
        }
    }

    public db_hero_vo SetData()
    {
        return data;
    }
}
