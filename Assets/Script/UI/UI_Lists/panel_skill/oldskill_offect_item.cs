using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class oldskill_offect_item : Base_Mono
{
    /// <summary>
    /// 基础信息
    /// </summary>
    private Text info;
    /// <summary>
    /// 战斗倒计时
    /// </summary>
    private Text WaitTime;
    private Image item_icon;
    private void Awake()
    {
        info = Find<Text>("info");
        WaitTime = Find<Text>("WaitTime");
        item_icon = GetComponent<Image>();
    }
    private base_skill_vo data;

    /// <summary>
    /// Data
    /// </summary>
    public base_skill_vo Data
    {
        set
        {
            data = value;
            if (data == null) return;
            if (data.skill_type == 1)
                item_icon.sprite = UI.UI_Manager.I.GetEquipSprite("skill/", data.skillname);
            else item_icon.sprite = UI.UI_Manager.I.GetEquipSprite("icon/", data.skillname);
            info.text = data.skillname;
        }
        get
        {
            return data;
        }
    }

    public void Battle()
    {
        info.text="";
        StartCoroutine(Skill_WaitTime());
    }
    /// <summary>
    /// 是否可以释放
    /// </summary>
    /// <returns></returns>
    public bool IsState()
    {
        return data.battle_CD <= 0;
    }
    /// <summary>
    /// 刷新冷却
    /// </summary>
    /// <returns></returns>
    private IEnumerator Skill_WaitTime()
    {
        data.battle_CD = data.skill_cd;
        info.text = "";
        float base_time = 0.1f;
        while (data.battle_CD > 0)
        {
            data.battle_CD -= base_time;
            WaitTime.text = data.battle_CD.ToString("0.0")+"S";
            if(data.battle_CD<=0)data.battle_CD = 0;
            yield return new WaitForSeconds(base_time);
        }
        WaitTime.text = "";
        //info.text= data.skillname;
    }
    public void Refresh()
    {
        item_icon.sprite = UI.UI_Manager.I.GetEquipSprite("icon/", data.skillname);
        if (data.skill_type == 1) item_icon.sprite = UI.UI_Manager.I.GetEquipSprite("skill/", data.skillname);
        info.text = Data.skillname + "Lv." + Data.user_values[1];
        //item_frame.gameObject.SetActive(data.skillpos != 0);
    }

}
