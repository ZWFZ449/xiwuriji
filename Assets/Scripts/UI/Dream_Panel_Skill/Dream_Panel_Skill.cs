using CodeStage.AntiCheat.ObscuredTypes;
using Common;
using Components;
using MVC;
using System;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;



public enum Skill_Job_Type
{ 
    普通攻击,
    物理伤害=1,
    魔法伤害,
    道术伤害,
}

public enum Skill_Effect_Type
{
    单体=1,
    群体,
    回复,
    状态,
    护盾,
    召唤
}

public enum skill_Lv_Type
{ 
  未激活=-1,
  初级,
  中级,
  高级,
  专家一重,
  专家二重,
  专家三重,
  大师一重,
  大师二重,
  大师三重,
  大师四重,
  宗师一重,
  宗师二重,
  宗师三重,
  宗师四重,
  宗师五重,
}

public class Dream_Panel_Skill : Panel_Base
{
    private enum Skill_Btn_Type
    { 
      上阵,
      升级,
      激活,
      进阶,
      宗师
    };
    private enum Skill_Type
    { 
        职业技能,
        被动技能,
    }
    private Transform p_skill_brom,p_btn_brom,p_battleshow_brom,p_select_skill_brom,p_skill_type_brom;
    private skill_item skill_item_prefab;
    private Image offect_skill, show_select_skill;
    private TMP_Text skill_name;
    private TMP_Text skill_des;
    private btn_item btn_item_prefab;
    private ScrollRect scrollRect;
    private skill_offect_item skill_offect_item_prefab;
    private db_skill_vo crt_skill;
    private Skill_Type crt_skill_type = Skill_Type.职业技能;
    public override void Hide()
    {
        if (offect_skill.gameObject.activeInHierarchy)
        {
            offect_skill.gameObject.SetActive(false);
        }else
        base.Hide();
    }

    public override void Initialize()
    {
        base.Initialize();
        p_skill_brom=Find<Transform>("bg/show_list/Viewport/Content");
        skill_item_prefab = Tool_UI.Find_Prefabs<skill_item>("skill_item");
        offect_skill= Find<Image>("bg/offect_skill");
        skill_name = Find<TMP_Text>("bg/offect_skill/skill_name/info/info");
        skill_des = Find<TMP_Text>("bg/offect_skill/skill_des/Viewport/info");
        p_btn_brom= Find<Transform>("bg/offect_skill/skill_btns");
        btn_item_prefab = Tool_UI.Find_Prefabs<btn_item>("btn_item");
        skill_offect_item_prefab= Tool_UI.Find_Prefabs<skill_offect_item>("skill_offect_item");
        scrollRect = Find<ScrollRect>("bg/offect_skill/skill_des");
        p_battleshow_brom = Find<Transform>("bg/battle_skill_list");
        show_select_skill= Find<Image>("bg/offect_skill/show_select_skill");
        show_select_skill.GetComponent<Button>().onClick.AddListener(() => { CloseShow_select_skill(); });
        p_select_skill_brom = Find<Transform>("bg/offect_skill/show_select_skill/show_select_skill_list");
        p_skill_type_brom= Find<Transform>("bg/skill_type_btn");
        for (int i = 0; i < Enum.GetNames(typeof(Skill_Type)).Length; i++)
        { 
            btn_item btn_item = Instantiate(btn_item_prefab, p_skill_type_brom);
            btn_item.Show(i, (Skill_Type)i);
            btn_item.GetComponent<Button>().onClick.AddListener(() => { Skill_Type_Click(btn_item); });
        }
        offect_skill.gameObject.SetActive(false);
    }
    /// <summary>
    /// 切换技能类型
    /// </summary>
    /// <param name="btn_item"></param>
    private void Skill_Type_Click(btn_item btn_item)
    {
        crt_skill_type = (Skill_Type)btn_item.index;
        Show();
    }

    private void Skill_Btn_Click(btn_item btn_item)
    {
        switch ((Skill_Btn_Type)btn_item.index)
        {
            case Skill_Btn_Type.上阵:
                go_into_battle();
                break;
            case Skill_Btn_Type.升级:
                Alert.Show("升级技能", "消耗背包中全部的" + Show_Color.Red( crt_skill.show_name), upLv_skill);

                break;
            case Skill_Btn_Type.激活:
                Alert.Show("激活技能", "需要" + Show_Color.Red(crt_skill.show_name), activate_skill);
                break;
            case Skill_Btn_Type.进阶:
                if (crt_skill.SetLv() < crt_skill.skill_damages.Count - 1 && crt_skill.SetLv()<=10)
                {
                    string dec= "\n天赋书页 * " + crt_skill.needLvitem[crt_skill.SetLv()] + "\n转生石 * " + crt_skill.needLvitem[crt_skill.SetLv()];
                    if (crt_skill.need_lv >= 60)
                    {
                        dec += "\n天赋精华 * " + (crt_skill.needLvitem[crt_skill.SetLv()] / 2);
                    }
                    Alert.Show("进阶技能", "需要" + Show_Color.Red(dec), advancedskill);
                }else Alert_Dec.Show("技能已满级");
                break;
            case Skill_Btn_Type.宗师:
                int max = 9 + MaxLv();
                if (crt_skill.Job == -1) max = 12;
                if (IsNeed(max))
                {
                    string dec = "";
                    if (crt_skill.Job == -1)//被动技能
                    {
                        dec = "\n" + common_items_list.被动精华 + " * " + ((crt_skill.SetLv() - 8) * 50);
                        dec += "\n" + common_items_list.强者证明 + " * " + ((crt_skill.SetLv() - 8) * 10);
                        dec += "\n" + common_items_list.天赋精华 + " * " + ((crt_skill.SetLv() - 8) * 100);
                    }
                    else
                    {
                        dec = "\n" + common_items_list.天赋精华 + " * " + ((crt_skill.SetLv() - 8) * 500);
                        dec += "\n" + common_items_list.强者证明 + " * " + ((crt_skill.SetLv() - 8) * 100);
                        dec += "\n" + common_items_list.金色传说 + " * " + ((crt_skill.SetLv() - 8) * 10);
                    }
                    Alert.Show("宗师技能", "需要" + Show_Color.Red(dec), advanced_Maxskill);
                }
                else Alert_Dec.Show("技能已满级");
                break;
        }
    }

    private bool IsNeed(int max)
    {
        if (crt_skill.SetLv() >= 9 && crt_skill.SetLv() <= max)
        {
            if (crt_skill.Job == -1) return true;
            else
            {

                return crt_skill.SetLv() < crt_skill.skill_damages.Count - 1;
            }
        }
        return false;
    }
    private void advanced_Maxskill(object arg0)
    {
        Clear_Condition();
        if (crt_skill.Job == -1)//被动技能
        {
            Need_Condition(common_items_list.被动精华, (crt_skill.SetLv() - 8) * 50);
            Need_Condition(common_items_list.强者证明, (crt_skill.SetLv() - 8) * 10);
            Need_Condition(common_items_list.天赋精华, (crt_skill.SetLv() - 8) * 100);
        }
        else
        {
            Need_Condition(common_items_list.天赋精华, (crt_skill.SetLv() - 8) * 500);
            Need_Condition(common_items_list.强者证明, (crt_skill.SetLv() - 8) * 100);
            Need_Condition(common_items_list.金色传说, (crt_skill.SetLv() - 8) * 10);
        }
        if (Return_Condition())
        {
            crt_skill.monster_lv(crt_skill.SetLv() + 1);
            SumSave.crt_skill.UpLv_skill();
            Alert_Dec.Show(crt_skill.show_name + "升级成功");
            SendNotification(NotiList.Refresh_Max_Hero_Attribute);
        }
        else Alert_Dec.Show("材料不足");
    }

    /// <summary>
    /// 技能最大效果
    /// </summary>
    /// <returns></returns>
    private int MaxLv()
    {
        int lv = 1;
        List<(int, int, long)> artifacts = SumSave.crt_user_artifact.Get;
        //神器
        for (int i = 0; i < SumSave.db_artifacts.Count; i++)
        {
            for (int j = 0; j < artifacts.Count; j++)
            {
                if (SumSave.db_artifacts[i].artifact_type == artifacts[j].Item1)
                {
                    if (artifacts[j].Item2 > 0)
                    {
                        List<string> list = ArrayHelper.Get_Split<string>(SumSave.db_artifacts[i].artifact_offect, ',');
                        for (int k = 0; k < list.Count; k++)
                        {
                            List<string> list2 = ArrayHelper.Get_Split<string>(list[k], ' ');
                            if (list2.Count == 3)
                            {
                                int value = ((artifacts[j].Item2 / int.Parse(list2[1])) + 1) * (int.Parse(list2[2]));
                                switch ((artifact_offect_list)(int.Parse(list2[0])))
                                {
                                    
                                    case artifact_offect_list.战系主动技能等级上限:
                                        if (crt_skill.Job == 1) lv = value;
                                        break;
                                    case artifact_offect_list.法系主动技能等级上限:
                                        if (crt_skill.Job == 2) lv = value;
                                        break;
                                    case artifact_offect_list.道士主动技能等级上限:
                                        if (crt_skill.Job == 3) lv = value;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }

        }
        return lv;
    }
    private void advancedskill(object arg0)
    {
        Clear_Condition();
        Need_Condition("转生石", crt_skill.needLvitem[crt_skill.SetLv()]);
        Need_Condition("天赋书页", crt_skill.needLvitem[crt_skill.SetLv()]);
        if (crt_skill.need_lv >= 60)
        {
            Need_Condition("天赋精华", crt_skill.needLvitem[crt_skill.SetLv()] / 2);
        }
        if (Return_Condition())
        {
            crt_skill.monster_lv(crt_skill.SetLv() + 1);
            SumSave.crt_skill.UpLv_skill();
            Alert_Dec.Show(crt_skill.show_name + "升级成功");
            SendNotification(NotiList.Refresh_Max_Hero_Attribute);
        }
        else Alert_Dec.Show("背包中天赋书页不足");
    }

    /// <summary>
    /// 升级技能
    /// </summary>
    /// <param name="arg0"></param>
    private void upLv_skill(object arg0)
    {
        List<(string, ObscuredInt )> lists = SumSave.crt_bags.Set();
        int  number = 0;
        if (lists.Count > 0)
        {
            foreach (var item in lists)
            {
                if (item.Item1 == crt_skill.show_name)
                {
                    number = item.Item2;
                    break;
                }
            } 
        }
        if (number > 0)
        {
            Clear_Condition();
            Need_Condition(crt_skill.show_name, number);
            if (Return_Condition())
            {
                crt_skill.GetExp(number * 10);
                SumSave.crt_skill.UpLv_skill();
                SendNotification(NotiList.Refresh_Max_Hero_Attribute);
                Alert_Dec.Show(crt_skill.show_name + "升级成功");
            }
        }
        else Alert_Dec.Show("背包中" + crt_skill.show_name + "不足");
    }

    /// <summary>
    /// 激活技能
    /// </summary>
    /// <param name="arg0"></param>
    private void activate_skill(object arg0)
    {
        Clear_Condition();
        Need_Condition(crt_skill.show_name, 1);
        if (Return_Condition())
        {
            crt_skill.activate_skill();
            SumSave.crt_skill.activate_skill(crt_skill);
            Show();
            SendNotification(NotiList.Refresh_Max_Hero_Attribute);
            Alert_Dec.Show(crt_skill.show_name + "激活成功");
        }
        else Alert_Dec.Show(crt_skill.show_name + "不足");
    }

    private void ShowBtn()
    {
        ClearObject(p_btn_brom);
        //未激活
        int lv = crt_skill.SetLv();
        for (int i = 0; i < Enum.GetNames(typeof(Skill_Btn_Type)).Length; i++)
        {
            bool exist = false;
            switch ((Skill_Btn_Type)i)
            {
                case Skill_Btn_Type.上阵:
                    exist = lv >= 0 && crt_skill.Job >= 0 && crt_skill.EffectType < 4; 
                    break;
                case Skill_Btn_Type.升级:
                    exist = lv >= 0 && (crt_skill.SetLv() < crt_skill.skill_up_lv.Count - 1);
                    break;
                case Skill_Btn_Type.激活:
                    exist = lv < 0;
                    break;
                case Skill_Btn_Type.进阶:
                    exist = lv >= 0 && (crt_skill.SetLv() >= crt_skill.skill_up_lv.Count - 1) && lv < 9;
                    break;
                case Skill_Btn_Type.宗师:
                    exist = lv >= 9 && (crt_skill.EffectType <= 4);
                    break;
            }
            if (exist)
            {
                btn_item btn_item = Instantiate(btn_item_prefab, p_btn_brom);
                btn_item.Show(i, (Skill_Btn_Type)i);
                btn_item.GetComponent<Button>().onClick.AddListener(() => { Skill_Btn_Click(btn_item); });

            }

        }
    }
    /// <summary>
    /// 关闭选择技能
    /// </summary>
    private void CloseShow_select_skill()
    {
        show_select_skill.gameObject.SetActive(false);
    }
    private void go_into_battle()
    {
        show_select_skill.gameObject.SetActive(true);
        Show_Battle_Skill(p_select_skill_brom, true);
    }

    public override void Show()
    {
        base.Show();
        BaseShow();
    }

    private void Show_Battle_Skill(Transform p_brom,bool exist=false)
    {
        ClearObject(p_brom);
        List<int> list = SumSave.crt_skill.Set_Select_Skill_Type();
        Dictionary<int, db_skill_vo> dic = SumSave.crt_skill.Set_Current_skill();
        for (int i = 0; i < list.Count; i++)
        {
            skill_offect_item item = Instantiate(skill_offect_item_prefab, p_brom);
            if (dic.ContainsKey( list[i]))
            {
                item.Init(i, dic[list[i]]);
            }
            else
            {
                db_Hero_VO hero = SumSave.db_heros.Find((db_Hero_VO hero) => hero.id == SumSave.crtHero.job);
                db_skill_vo skill = SumSave.db_skills.Find((db_skill_vo skill) => skill.show_name == hero.initskill);
                item.Init(i, skill);
            }
            if (exist)
            { 
                item.GetComponent<Button>().onClick.AddListener(() => { Select_Skill(item); });
            }
        }
    }
    /// <summary>
    /// 选择上阵技能
    /// </summary>
    /// <param name="item"></param>
    private void Select_Skill(skill_offect_item item)
    {
        List<int> list = SumSave.crt_skill.Set_Select_Skill_Type();
        list[item.index] = crt_skill.id;
        SumSave.crt_skill.Get_Select_Skill_Type = list;
        Show_Battle_Skill(p_battleshow_brom);
        CloseShow_select_skill();
    }

    /// <summary>
    /// 初始化技能列表
    /// </summary>
    private void BaseShow()
    {
        ClearObject(p_skill_brom);
        for (int i = 0; i < SumSave.db_skills.Count; i++)
        {
            bool exist = false;
            switch (crt_skill_type)
            {
                case Skill_Type.职业技能:
                    if ((SumSave.db_skills[i].Job == SumSave.crtHero.job || SumSave.crtHero.job == 0) && SumSave.db_skills[i].Job != -1) exist = true;
                    if (SumSave.db_skills[i].id >= 30 && SumSave.crtHero.zs_lvs <= 1) exist = false;
                    break;
                case Skill_Type.被动技能:
                    if (SumSave.db_skills[i].Job == -1) exist = true;
                    break;
            }
            if (exist)
            { 
                skill_item skill_item = Instantiate(skill_item_prefab, p_skill_brom);
                skill_item.Init(SumSave.db_skills[i]);
                skill_item.GetComponent<Button>().onClick.AddListener(() => { SkillClick(skill_item); });
            }
        }
        offect_skill.gameObject.SetActive(false);
        Show_Battle_Skill(p_battleshow_brom);

    }
    private void SkillClick(skill_item item)
    {
        offect_skill.gameObject.SetActive(true);
        crt_skill = item.GetSkill();
        skill_name.text = crt_skill.show_name;
        ShowBtn();
        skill_des.text = crt_skill.Job == -1 ? passive_skill_str() : active_skill_str();
        scrollRect.verticalNormalizedPosition = 1;
    }
    /// <summary>
    /// 被动技能
    /// </summary>
    /// <returns></returns>
    private string passive_skill_str()
    {
        Color color_list = UnityColorPresets.HexToColor("ffffff");
        string str = "";
        int lv = crt_skill.SetLv();
        int talent_lv = 0;
        if (lv == -1) str += "未激活\n";
        else
        {
            foreach (var item1 in crt_skill.GetBuff.Keys)
            {
                switch (item1)
                {
                    case enum_talent_offect_list.弹道:
                        talent_lv += crt_skill.GetBuff[item1];
                        break;
                }
            }
            if (crt_skill.Job != -1)
            {
                
                str += "[技能等级]:" + (skill_Lv_Type)(lv) + (talent_lv == 0 ? "" : Show_Color.Red("(+" + talent_lv + ")")) + "(" + crt_skill.SetExp() + "/" + crt_skill.skill_up_lv[lv] + ")" + "\n";
            }
            else
            {
                 str += "[技能等级]:" + (skill_Lv_Type)(lv+ talent_lv) + "(" + crt_skill.SetExp() + "/" + "\n";
            }
            
        }
        str += "[被动效果] ";
        if (crt_skill.skill_offect_value_list.Count > 0)
        {
            foreach (enum_equip_entry_list entry in crt_skill.skill_offect_value_list.Keys)
            { 
                for (int i = 0; i < crt_skill.skill_offect_value_list[entry].Count; i++)
                {
                    color_list = (i <= lv + talent_lv) ? UnityColorPresets.HexToColor("ffe400") : UnityColorPresets.HexToColor("#808080");
                    str += "\n" + (skill_Lv_Type)i + " ";
                    switch (entry)
                    {
                        case enum_equip_entry_list.生命值:
                        case enum_equip_entry_list.魔法值:
                        case enum_equip_entry_list.每秒回血:
                        case enum_equip_entry_list.每秒回蓝:
                        case enum_equip_entry_list.真实伤害:
                        case enum_equip_entry_list.吸收伤害:
                        case enum_equip_entry_list.命中:
                        case enum_equip_entry_list.闪避:
                            str += entry + " " + UnityColorPresets.Colorize(crt_skill.skill_offect_value_list[entry][i], color_list) + ";";
                            break;
                        case enum_equip_entry_list.物理防御:
                        case enum_equip_entry_list.魔法防御:
                        case enum_equip_entry_list.物理攻击:
                        case enum_equip_entry_list.魔法攻击:
                        case enum_equip_entry_list.道术攻击:
                            str += entry + " " + UnityColorPresets.Colorize(crt_skill.skill_offect_value_list[entry][i] + " - " + crt_skill.skill_offect_value_list[entry][i], color_list) + ";";
                            break;

                        case enum_equip_entry_list.幸运:
                            break;
                        case enum_equip_entry_list.生命属性:
                        case enum_equip_entry_list.魔法属性:
                        case enum_equip_entry_list.防御属性:
                        case enum_equip_entry_list.魔防属性:
                        case enum_equip_entry_list.物攻属性:
                        case enum_equip_entry_list.魔攻属性:
                        case enum_equip_entry_list.道攻属性:
                        case enum_equip_entry_list.攻击速度:
                        case enum_equip_entry_list.攻击范围:
                        case enum_equip_entry_list.暴击属性:
                        case enum_equip_entry_list.暴击伤害:
                            str += entry + " " + UnityColorPresets.Colorize(crt_skill.skill_offect_value_list[entry][i] + "%", color_list) + ";";
                            break;

                        case enum_equip_entry_list.烈阳文:
                            break;
                        case enum_equip_entry_list.盾护文:
                            break;
                        case enum_equip_entry_list.守月文:
                            break;
                        case enum_equip_entry_list.幽狼文:
                            break;
                        case enum_equip_entry_list.神行文:
                            break;
                        case enum_equip_entry_list.怒目文:
                            break;
                        case enum_equip_entry_list.震火文:
                            break;
                        case enum_equip_entry_list.金刚文:
                            break;
                        case enum_equip_entry_list.大愈文:
                            break;
                        case enum_equip_entry_list.回春文:
                            break;
                        case enum_equip_entry_list.回心文:
                            break;
                        case enum_equip_entry_list.峰芒文:
                            break;
                        case enum_equip_entry_list.破枪文:
                            break;
                        case enum_equip_entry_list.深寒文:
                            break;
                        case enum_equip_entry_list.瑶光文:
                            break;
                        case enum_equip_entry_list.物理下防:
                            break;
                        case enum_equip_entry_list.魔法下防:
                            break;
                        case enum_equip_entry_list.物理下攻:
                            break;
                        case enum_equip_entry_list.魔法下攻:
                            break;
                        case enum_equip_entry_list.道术下攻:
                            break;
                        case enum_equip_entry_list.物伤减免:
                        case enum_equip_entry_list.魔伤减免:
                        case enum_equip_entry_list.怪物爆率:
                        case enum_equip_entry_list.极品爆率:
                        case enum_equip_entry_list.经验加成:
                        case enum_equip_entry_list.金币掉落:
                            str += entry + " " + UnityColorPresets.Colorize(crt_skill.skill_offect_value_list[entry][i] + "%", color_list) + ";";
                            break;
                        default:
                            if ((int)entry > 1000)
                            {
                                db_skill_vo skill = ArrayHelper.Find(SumSave.db_skills, (x) => x.id == ((int)entry - 1000));
                                if (skill != null)
                                {
                                    str += "\n" + skill.show_name + " 技能效果 + " + UnityColorPresets.Colorize(crt_skill.skill_offect_value_list[entry][i] + "%", color_list) + ";";
                                }
                            }
                            break;
                    }
                }
            }
        }
        return str;
    }
    /// <summary>
    /// 主动技能描述
    /// </summary>
    /// <returns></returns>
    private string active_skill_str()
    {
        string str = "";
        int i = crt_skill.SetLv();
        Show_Color_list color_list = Show_Color_list.orange;
        if (i == -1)
        {
            str += "未激活\n";
            color_list = Show_Color_list.grey; i = 0;
        }
        if (i > crt_skill.skill_up_lv.Count - 1)
        {
            str += "[技能等级] Lv." + i + "(Max)" + "\n";

        }
        else str += "[技能等级] Lv." + (skill_Lv_Type)i + "(" + crt_skill.SetExp() + "/" + crt_skill.skill_up_lv[i] + ")" + "\n";

        str += "[技能类型] " + (Skill_Job_Type)crt_skill.Job + "\n";

        str += "[技能消耗] " + Show_Color.Set_String(crt_skill.spells[i] + "Mp", color_list) + "\n";

        str += "[技能效果] ";
        switch ((Skill_Effect_Type)crt_skill.EffectType)
        {
            case Skill_Effect_Type.单体:
                str += "造成 " + Show_Color.Set_String((crt_skill.Power + crt_skill.DefPowers[i]) + " %" + (Skill_Effect_Type)crt_skill.EffectType + " 伤害" + ";", color_list);
                if (crt_skill.Effect == 2)
                {
                    str += "\n " + Show_Color.Set_String(5 + "%概率造成 10倍 伤害" + ";", color_list);
                    if (crt_skill.skill_damages.Count > 0) str += "\n[物攻] + " + Show_Color.Set_String(crt_skill.skill_damages[i], color_list) + ";";
                }else
                if (crt_skill.skill_damages.Count > 0) str += "\n[真实伤害] " + Show_Color.Set_String(crt_skill.skill_damages[i], color_list) + ";";
                break;
            case Skill_Effect_Type.群体:
                if (crt_skill.Effect == 1)
                {
                    str += "造成 " + Show_Color.Set_String((crt_skill.Power + crt_skill.DefPowers[i]) + " %" + (Skill_Effect_Type)crt_skill.EffectType + " 伤害" + ";", color_list);
                    if (crt_skill.skill_damages.Count > 0) str += "\n[真实伤害] " + Show_Color.Set_String(crt_skill.skill_damages[i], color_list) + ";";
                }
                else
                {
                    str += "对 " + Show_Color.Set_String(crt_skill.Effect, color_list) + " 个目标,分别造成 " + Show_Color.Set_String((crt_skill.Power + crt_skill.DefPowers[i]) + " %" + (Skill_Effect_Type)crt_skill.EffectType + " 伤害" + ";", color_list);
                    if (crt_skill.skill_damages.Count > 0) str += "\n[魔攻] +" + Show_Color.Set_String(crt_skill.skill_damages[i], color_list) + ";";
                }
                break;
            case Skill_Effect_Type.回复:
                str += "回复" + Show_Color.Set_String(crt_skill.Effect + (crt_skill.Power + crt_skill.DefPowers[i]) + "%", color_list) + "生命值" + ";";
                if (crt_skill.skill_damages.Count > 0) str += "\n[额外回复] " + Show_Color.Set_String(crt_skill.skill_damages[i], color_list) + ";";

                break;
            case Skill_Effect_Type.状态:
                str += "攻击降低目标 " + Show_Color.Set_String((crt_skill.Power + crt_skill.DefPowers[i]) + " %道术" + "的防御 ", color_list);
                if (crt_skill.skill_damages.Count > 0) str += "\n[真实减防] " + Show_Color.Set_String(crt_skill.skill_damages[i], color_list) + ";";
                break;
            case Skill_Effect_Type.召唤:
                str += "召唤 " + Show_Color.Set_String(crt_skill.show_name, color_list) + "\n继承" + Show_Color.Set_String((crt_skill.Power + crt_skill.DefPowers[i]) + " %属性" + " ", color_list);
                if (crt_skill.skill_damages.Count > 0) str += "\n[召唤兽伤害] " + Show_Color.Set_String(crt_skill.skill_damages[i], color_list) + ";";
                break;
            case Skill_Effect_Type.护盾:
                if (crt_skill.Effect == 3)
                {
                    str += "生成 " + Show_Color.Set_String((crt_skill.Power + crt_skill.DefPowers[i]) + ("% 道术加持") + " ", color_list);
                    if (crt_skill.skill_damages.Count > 0) str += "\n[道术] + " + Show_Color.Set_String(crt_skill.skill_damages[i], color_list) + ";";
                    str += "\n[特效] " + Show_Color.Set_String("反弹道术*200%无视防御伤害", color_list) + ";";

                }
                else
                {
                    str += "生成 " + Show_Color.Set_String((crt_skill.Power + crt_skill.DefPowers[i]) + (crt_skill.Effect == 1 ? " 双防御" : "% 双免伤") + " ", color_list);
                    if (crt_skill.skill_damages.Count > 0) str += "\n[免伤] " + Show_Color.Set_String(crt_skill.skill_damages[i], color_list) + ";";
                }
                break;
        }
        if (SumSave.crtHero.zs_lvs > 1)
        {
            str += "\n[转生加成] ";
            if (crt_skill.skill_offect_value_list.Count > 0)
            {
                foreach (enum_equip_entry_list entry in crt_skill.skill_offect_value_list.Keys)
                {
                    for (int j = 0; j < crt_skill.skill_offect_value_list[entry].Count; j++)
                    {
                        if (j == i)
                        {
                            Color color_lists = UnityColorPresets.HexToColor("ffe400");
                            str += "\n" + (skill_Lv_Type)j + " ";
                            switch (entry)
                            {
                                case enum_equip_entry_list.生命值:
                                case enum_equip_entry_list.魔法值:
                                case enum_equip_entry_list.每秒回血:
                                case enum_equip_entry_list.每秒回蓝:
                                case enum_equip_entry_list.真实伤害:
                                case enum_equip_entry_list.吸收伤害:
                                case enum_equip_entry_list.命中:
                                case enum_equip_entry_list.闪避:
                                    str += entry + " " + UnityColorPresets.Colorize(crt_skill.skill_offect_value_list[entry][i], color_lists) + ";";
                                    break;
                                case enum_equip_entry_list.物理防御:
                                case enum_equip_entry_list.魔法防御:
                                case enum_equip_entry_list.物理攻击:
                                case enum_equip_entry_list.魔法攻击:
                                case enum_equip_entry_list.道术攻击:
                                    str += entry + " " + UnityColorPresets.Colorize(crt_skill.skill_offect_value_list[entry][i] + " - " + crt_skill.skill_offect_value_list[entry][i], color_lists) + ";";
                                    break;

                                case enum_equip_entry_list.幸运:
                                    break;
                                case enum_equip_entry_list.生命属性:
                                case enum_equip_entry_list.魔法属性:
                                case enum_equip_entry_list.防御属性:
                                case enum_equip_entry_list.魔防属性:
                                case enum_equip_entry_list.物攻属性:
                                case enum_equip_entry_list.魔攻属性:
                                case enum_equip_entry_list.道攻属性:
                                case enum_equip_entry_list.攻击速度:
                                case enum_equip_entry_list.攻击范围:
                                case enum_equip_entry_list.暴击属性:
                                case enum_equip_entry_list.暴击伤害:
                                    str += entry + " " + UnityColorPresets.Colorize(crt_skill.skill_offect_value_list[entry][i] + "%", color_lists) + ";";
                                    break;
                                case enum_equip_entry_list.物理下防:
                                    break;
                                case enum_equip_entry_list.魔法下防:
                                    break;
                                case enum_equip_entry_list.物理下攻:
                                    break;
                                case enum_equip_entry_list.魔法下攻:
                                    break;
                                case enum_equip_entry_list.道术下攻:
                                    break;
                                case enum_equip_entry_list.物伤减免:
                                case enum_equip_entry_list.魔伤减免:
                                case enum_equip_entry_list.怪物爆率:
                                case enum_equip_entry_list.极品爆率:
                                case enum_equip_entry_list.经验加成:
                                case enum_equip_entry_list.金币掉落:
                                    str += entry + " " + UnityColorPresets.Colorize(crt_skill.skill_offect_value_list[entry][i] + "%", color_lists) + ";";
                                    break;
                                default:
                                    if ((int)entry > 1000)
                                    {
                                        db_skill_vo skill = ArrayHelper.Find(SumSave.db_skills, (x) => x.id == ((int)entry - 1000));
                                        if (skill != null)
                                        {
                                            str += "\n" + skill.show_name + " 技能效果 + " + UnityColorPresets.Colorize(crt_skill.skill_offect_value_list[entry][i] + "%", color_lists) + ";";
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }
        foreach (var item1 in crt_skill.GetBuff.Keys)
        {
            string dec="";
            switch (item1)
            {
                case enum_talent_offect_list.生命:
                    break;
                case enum_talent_offect_list.攻击:
                    break;
                case enum_talent_offect_list.魔法:
                    break;
                case enum_talent_offect_list.道术:
                    break;
                case enum_talent_offect_list.防御:
                    break;
                case enum_talent_offect_list.攻击速度:
                    break;
                case enum_talent_offect_list.物理攻击:
                    break;
                case enum_talent_offect_list.魔法攻击:
                    break;
                case enum_talent_offect_list.道术攻击:
                    break;
                case enum_talent_offect_list.防御值:
                    break;
                case enum_talent_offect_list.躲避:
                    break;
                case enum_talent_offect_list.命中:
                    break;
                case enum_talent_offect_list.技能:
                    break;
                case enum_talent_offect_list.附加攻击:
                    break;
                case enum_talent_offect_list.附加魔法:
                    break;
                case enum_talent_offect_list.附加道术:
                    break;
                case enum_talent_offect_list.附加双防:
                    break;
                case enum_talent_offect_list.附加伤害:
                    break;
                case enum_talent_offect_list.附加回血:
                    break;
                case enum_talent_offect_list.附加攻击范围:
                    break;
                case enum_talent_offect_list.无视防御:
                    break;
                case enum_talent_offect_list.召唤兽:
                    break;
                case enum_talent_offect_list.召唤兽攻击:
                    break;
                case enum_talent_offect_list.召唤兽生命:
                    break;
                case enum_talent_offect_list.召唤兽防御:
                    break;
                case enum_talent_offect_list.召唤兽速度:
                    break;
                case enum_talent_offect_list.召唤兽死亡爆炸:
                    break;
                case enum_talent_offect_list.召唤数量:
                    break;
                case enum_talent_offect_list.特殊效果:
                    break;
                case enum_talent_offect_list.临时伤害:
                    break;
                case enum_talent_offect_list.临时防御:
                    break;
                case enum_talent_offect_list.临时速度:
                    break;
                case enum_talent_offect_list.单体改群体:
                    break;
                case enum_talent_offect_list.技能攻击个数:
                    break;
                case enum_talent_offect_list.技能概率不消耗蓝:
                    break;
                case enum_talent_offect_list.技能全体伤害:
                    break;
                case enum_talent_offect_list.群体技能攻击范围:
                    break;
                case enum_talent_offect_list.每秒回复全体血量百分比:
                    break;
                case enum_talent_offect_list.攻击击退敌人概率:
                    break;
                case enum_talent_offect_list.技能伤害:
                    dec = "%";
                    break;
                case enum_talent_offect_list.技能触发概率:
                    dec = "%";
                    break;
                case enum_talent_offect_list.溅射数量:
                    break;
                case enum_talent_offect_list.爆炸伤害:
                    dec = "%";
                    break;
                case enum_talent_offect_list.弹道:
                    break;
            }
            str += "\n[天赋效果] " + Show_Color.Set_String(item1.ToString(), color_list) + " " + Show_Color.Set_String(crt_skill.GetBuff[item1] + dec, color_list);
            str += "\n";
        }
        return str;
    }
    protected override void Awake()
    {
        base.Awake();
    }
}
