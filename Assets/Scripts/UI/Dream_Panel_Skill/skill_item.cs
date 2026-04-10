using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class skill_item : Base_Mono
{
    private Image icon;
    private Text info;
    private db_skill_vo data;

    private void Awake()
    {
        icon=Find<Image>("bg/icon");
        info=Find<Text>("info");
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
                        case enum_talent_offect_list.µ¯µÀ:
                            talent_lv += data.GetBuff[item1];
                            break;
                    }
                }
            }
            info.text += "Lv." + (skill_Lv_Type)(lv + talent_lv) + "(" + data.SetExp() + "/" + data.skill_up_lv[lv] + ")" + "\n";
        }
    }

    public db_skill_vo GetSkill()
    { 
       return data;
    }
}
