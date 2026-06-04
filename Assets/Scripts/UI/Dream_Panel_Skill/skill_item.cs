using MVC;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class skill_item : Base_Mono
{
    private Image icon;
    private TMP_Text info;
    private db_skill_vo data;

    private void Awake()
    {
        icon=Find<Image>("bg/icon");
        info=Find<TMP_Text>("info/info");
    }

    public void Init(db_skill_vo skill)
    { 
        data = skill;
        icon.sprite = UI.UI_Manager.I.GetEquipSprite("skill/base_icon/", skill.show_name);
        int lv = data.SetLv();
        info.text = skill.show_name + "\n";
        int talent_lv = 0;
        if (lv == -1) { info.text += (skill_Lv_Type)lv; }
        else
        {
            if (data.Job == -1)
            {
                foreach (var item1 in data.GetBuff.Keys)
                {
                    switch (item1)
                    {
                        case enum_talent_offect_list.弹道:
                            talent_lv += data.GetBuff[item1];
                            break;
                    }
                }
            }
            if (lv < data.skill_up_lv.Count)
                info.text += "Lv." + (skill_Lv_Type)(lv + talent_lv) + "(" + data.SetExp() + "/" + data.skill_up_lv[lv] + ")" + "\n";
            else info.text += "Lv." + (skill_Lv_Type)(lv + talent_lv) + "\n";
        }
    }

    public db_skill_vo GetSkill()
    { 
       return data;
    }
}
