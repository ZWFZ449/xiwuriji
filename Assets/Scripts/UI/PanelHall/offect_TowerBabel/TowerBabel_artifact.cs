using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityColorPresets;
using Random = UnityEngine.Random;

public class TowerBabel_artifact : Base_Mono
{
    /// <summary>
    /// 状态
    /// </summary>
    private enum TowerBabel_skill_state
    { 
    激活,
    升级
    }
    private Transform m_btn_brom;

    private TowerBabel_skill_item TowerBabel_skill_item_prefab;

    private Dictionary<string, TowerBabel_skill_item> TowerBabel_skill_item_dic = new Dictionary<string, TowerBabel_skill_item>();

    private Button close_bg;

    private Image offect_skill;

    private TowerBabel_skill_item crt_skill;

    private TMP_Text skill_name;

    private Transform m_info_brom,m_skill_btn_brom; 

    private equip_type_info_item equip_type_info_item_prefab;

    private equip_show_info_item equip_show_info_item_prefab;

    private btn_item btn_item_prefab;

    private List<TowerBabel_skill_state> skill_state_list = new List<TowerBabel_skill_state>();
    private void Awake()
    {
        m_btn_brom=Find<Transform>("Scroll View/Viewport/Content");
        TowerBabel_skill_item_prefab = Tool_UI.Find_Prefabs<TowerBabel_skill_item>("TowerBabel_skill_item");
        skill_name = Find<TMP_Text>("offect_skill/skill_name/info");
        offect_skill = Find<Image>("offect_skill");
        equip_type_info_item_prefab = Tool_UI.Find_Prefabs<equip_type_info_item>("equip_type_info_items");
        equip_show_info_item_prefab = Tool_UI.Find_Prefabs<equip_show_info_item>("equip_show_info_items");
        m_info_brom = Find<Transform>("offect_skill/skill_offect_brom/Viewport/Content");
        close_bg = Find<Button>("offect_skill/close_button");
        close_bg.onClick.AddListener(() => { offect_skill.gameObject.SetActive(false); });
        btn_item_prefab = Tool_UI.Find_Prefabs<btn_item>("btn_item");
        m_skill_btn_brom = Find<Transform>("offect_skill/skill_btn_brom");
        Init();
    }

    private void Init()
    {
        ClearObject(m_btn_brom);

        for (int i = 0; i < SumSave.db_towerbabel_artifacts.Count; i++)
        {
            TowerBabel_skill_item item = Instantiate(TowerBabel_skill_item_prefab, m_btn_brom);
            item.Data = SumSave.db_towerbabel_artifacts[i];
            TowerBabel_skill_item_dic.Add(SumSave.db_towerbabel_artifacts[i].TowerBabel_name, item);
            item.GetComponent<Button>().onClick.AddListener(() => { SkillClick(item); });
        }
    }


    private void SkillClick(TowerBabel_skill_item item)
    {
        offect_skill.gameObject.SetActive(true);
        crt_skill = item;
        skill_name.text = crt_skill.Data.TowerBabel_name+" Lv."+ crt_skill.Data.user_lv;
        ShowBtn();
    }

    private void ShowBtn()
    {
        ClearObject(m_info_brom);
        int lv = crt_skill.Data.user_lv;
        Color c = HexToColor("#ffffff");
        c = lv > 0 ? HexToColor("#70ff69") : HexToColor("#808080");
        Get().Init(("[" + "激活加成" + "]"), c);
        Show_PassiveSkill(crt_skill.Data.activate_offect, lv, true, c);
        if (lv > 0)
        {
            c = HexToColor("#00ffff");
            Get().Init(("[" + "等级加成" + "]"), c);
            Show_PassiveSkill(crt_skill.Data.up_offect, lv, false, c);
        }
        UpShow(lv, c);
        if (lv == 0)
        {
            c = HexToColor("#808080");
            Get().Init(("[" + "未生效" + "]"), c);
            Get().Init(("[" + "共鸣特效" + "]"), c);
            Get().Init(("[" + "未激活" + "]"), c);

        }
        else
        {
            c = HexToColor("#00FF00");
            Get().Init(("[" + "生效中" + "]"), c);
            Dictionary<string, db_towerbabel_vo> TowerBabel_skill_dic = SumSave.crt_user_towerbabel.GetArtifact;
            List<int> list = new List<int>();
            foreach (var item in TowerBabel_skill_dic)
            {
                if (item.Value.user_lv > 0 && item.Value.user_lv <= item.Value.max_lv) list.Add(item.Value.user_lv);
            }
            int min = ArrayHelper.GetMin(list, i => i);

            if (list.Count==SumSave.db_towerbabel_artifacts.Count && min >= 5)
            {
                c = HexToColor("#FF4500");
                Get().Init(("[" + "共鸣特效" + "]"), c);
                Get().Init(("[" + "全体技能效果+" + ((min / 5) * 5) + "%]"), c);
            }
            else
            {
                Get().Init(("[" + "共鸣特效" + "]"), c);
                Get().Init(("[" + "未激活" + "]"), c);
            }
        }
    }
    private void UpShow(int lv, Color c)
    {
        skill_state_list.Clear();
        Clear_Condition();
        if (lv >= crt_skill.Data.max_lv)
        {
            c = HexToColor("#ffff00");
            Get().Init(("[" + "满级" + "]"), c);
        }
        else
        {
            List<string> list = new List<string>();
            string needlist;
            c = HexToColor("#ff0000");
            if (lv == 0)
            {
                c = HexToColor("#ff0000");
                Get().Init(("[" + "激活需求" + "]"), c);
                needlist = crt_skill.Data.need_activate;
                skill_state_list.Add(TowerBabel_skill_state.激活);
            }
            else
            {
                c = HexToColor("#ff0000");
                Get().Init(("[" + "升级需求" + "]"), c);
                list = ArrayHelper.Get_Split<string>(crt_skill.Data.need_offect, '|');
                needlist = list[lv - 1];
                skill_state_list.Add(TowerBabel_skill_state.升级);
            }
            List<string> need_arr = ArrayHelper.Get_Split<string>(needlist, ',');
            foreach (var baseneed in need_arr)
            {
                List<string> need = ArrayHelper.Get_Split<string>(baseneed, ' ');
                if (need.Count == 3)
                {
                    equip_show_info_item itemValue = Instantiate(equip_show_info_item_prefab, m_info_brom);
                    switch (need[0])
                    {
                        case "0":
                            itemValue.towerbabel_Init((currency_unit)(int.Parse(need[1])), Battle_Tool.FormatNumberToChineseUnit(long.Parse(need[2])));
                            Need_Condition(need[1], long.Parse(need[2]));
                            break;
                        case "1":
                        case "3":
                            itemValue.towerbabel_Init(need[1], Battle_Tool.FormatNumberToChineseUnit(int.Parse(need[2])));
                            Need_Condition(need[1], int.Parse(need[2]));
                            break;
                        default:
                            break;
                    }
                }
            }
            Show_btn_state();
        }
    }
    /// <summary>
    /// 技能升级
    /// </summary>
    private void Show_btn_state()
    {
        ClearObject(m_skill_btn_brom);
        for (int i = 0; i < skill_state_list.Count; i++)
        { 
            btn_item item= Instantiate(btn_item_prefab, m_skill_btn_brom);
            item.Show(i,skill_state_list[i]);
            item.GetComponent<Button>().onClick.AddListener(() => { OnClick_btn(item); });
        }    
    }
    /// <summary>
    /// 升级
    /// </summary>
    /// <param name="item"></param>
    private void OnClick_btn(btn_item item)
    {
        Clear_Condition();
        int lv = crt_skill.Data.user_lv;
        if (lv >= crt_skill.Data.max_lv)
        {
            Alert_Dec.Show("已满级");
            return;
        }
        else
        {
            List<string> list = new List<string>();
            string needlist;
            if (lv == 0)
            {
                needlist = crt_skill.Data.need_activate;
            }
            else
            {
                list = ArrayHelper.Get_Split<string>(crt_skill.Data.need_offect, '|');
                needlist = list[lv - 1];
            }
            List<string> need_arr = ArrayHelper.Get_Split<string>(needlist, ',');
            foreach (var baseneed in need_arr)
            {
                List<string> need = ArrayHelper.Get_Split<string>(baseneed, ' ');
                if (need.Count == 3)
                {
                    switch (need[0])
                    {
                        case "0":
                            Need_Condition(need[1], long.Parse(need[2]));
                            break;
                        case "1":
                        case "3":
                            Need_Condition(need[1], int.Parse(need[2]));
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        if (Return_Condition())
        {
            crt_skill.Data.user_lv++;
            SumSave.crt_user_towerbabel.SetArtifact(crt_skill.Data);
            SkillClick(crt_skill);
            InitShow();
            SendNotification(NotiList.Refresh_Max_Hero_Attribute);
            Alert_Dec.Show("升级成功");
        }
        else 
        {
            Alert_Dec.Show("升级失败");
        }
    }

    /// <summary>
    /// 显示主动技能
    /// </summary>
    /// <param name="activate_offect"></param>
    /// <param name="lv"></param>
    /// <param name="v"></param>
    /// <param name="c"></param>
    private void Show_active_skill(string activate_offect, int lv, bool isUp, Color c)
    {
        List<string> list = ArrayHelper.Get_Split<string>(activate_offect, '|');
        int i = isUp ? (int)enum_equip_basetype_list.元素属性 : (int)enum_equip_basetype_list.附加属性;
        if (list.Count > 0)
        {
            foreach (var item in list)
            {
                List<string> entry_arr = ArrayHelper.Get_Split<string>(item, ' ');
                if (entry_arr.Count == 2)
                {
                    equip_show_info_item itemValue = Instantiate(equip_show_info_item_prefab, m_info_brom);
                    int value = int.Parse(entry_arr[1]);
                    string skill_name = "";
                    skill_name = ArrayHelper.Find(SumSave.db_skills, x => x.id == (int.Parse(entry_arr[0]))).show_name;
                    itemValue.towerbabel_Init(skill_name, value);
                }else 
                if (entry_arr.Count == 3)
                {
                    equip_show_info_item itemValue = Instantiate(equip_show_info_item_prefab, m_info_brom);
                    int value = (lv - 1) / int.Parse(entry_arr[1]) * int.Parse(entry_arr[2]);
                    string skill_name = "";
                    skill_name = ArrayHelper.Find(SumSave.db_skills, x => x.id == (int.Parse(entry_arr[0]))).show_name;
                    itemValue.towerbabel_Init(skill_name, value);
                }
            }
        }
    }

    /// <summary>
    /// 显示信息
    /// </summary>
    /// <param name="skill_value">基础信息</param>
    /// <param name="lv">技能等级</param>
    /// <param name="isUp">是否为激活属性</param>
    /// <param name="c">颜色</param>
    private void Show_PassiveSkill(string skill_value,int lv,bool isUp, Color c)
    {
        List<string> list = ArrayHelper.Get_Split<string>(skill_value, '|');
        int i = isUp ? (int)enum_equip_basetype_list.元素属性 : (int)enum_equip_basetype_list.附加属性;
        if (list.Count > 0)
        {
            foreach (var item in list)
            {
                List<string> entry_arr = ArrayHelper.Get_Split<string>(item, ' ');
                if (entry_arr.Count == 2 || entry_arr.Count == 3)
                {
                    equip_show_info_item itemValue = Instantiate(equip_show_info_item_prefab, m_info_brom);
                    enum_equip_entry_list e = (enum_equip_entry_list)int.Parse(entry_arr[0]);
                    int value = int.Parse(entry_arr[1]);
                    if (entry_arr.Count == 3) value = (lv - 1) / int.Parse(entry_arr[1]) * int.Parse(entry_arr[2]);
                    switch (e)
                    {
                        case enum_equip_entry_list.生命值:
                        case enum_equip_entry_list.魔法值:
                            itemValue.Init((enum_equip_basetype_list)i, e, " " + value, c);
                            break;
                        case enum_equip_entry_list.物理防御:
                        case enum_equip_entry_list.魔法防御:
                        case enum_equip_entry_list.物理攻击:
                        case enum_equip_entry_list.魔法攻击:
                        case enum_equip_entry_list.道术攻击:
                            itemValue.Init((enum_equip_basetype_list)i, e, value + " - " + value, c);
                            break;
                        case enum_equip_entry_list.每秒回血:
                        case enum_equip_entry_list.每秒回蓝:
                        case enum_equip_entry_list.真实伤害:
                        case enum_equip_entry_list.吸收伤害:
                        case enum_equip_entry_list.幸运:
                        case enum_equip_entry_list.命中:
                        case enum_equip_entry_list.闪避:
                            itemValue.Init((enum_equip_basetype_list)i, e, " " + value, c);
                            break;
                        case enum_equip_entry_list.物理下防:
                            e = enum_equip_entry_list.物理防御;
                            itemValue.Init((enum_equip_basetype_list)i, e, value + " - " + value, c);
                            break;
                        case enum_equip_entry_list.魔法下防:
                            e = enum_equip_entry_list.魔法防御;
                            itemValue.Init((enum_equip_basetype_list)i, e, value + " - " + value, c);
                            break;
                        case enum_equip_entry_list.物理下攻:
                            e = enum_equip_entry_list.物理攻击;
                            itemValue.Init((enum_equip_basetype_list)i, e, value + " - " + value, c);
                            break;
                        case enum_equip_entry_list.魔法下攻:
                            e = enum_equip_entry_list.魔法攻击;
                            itemValue.Init((enum_equip_basetype_list)i, e, value + " - " + value, c);
                            break;
                        case enum_equip_entry_list.道术下攻:
                            e = enum_equip_entry_list.道术攻击;
                            itemValue.Init((enum_equip_basetype_list)i, e, value + " - " + value, c);
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
                        case enum_equip_entry_list.物伤减免:
                        case enum_equip_entry_list.魔伤减免:
                        case enum_equip_entry_list.怪物爆率:
                        case enum_equip_entry_list.极品爆率:
                        case enum_equip_entry_list.经验加成:
                        case enum_equip_entry_list.金币掉落:
                            itemValue.Init((enum_equip_basetype_list)i, e, value + " %", c);
                            break;
                        case enum_equip_entry_list.麻痹概率:
                            itemValue.Init((enum_equip_basetype_list)i, e, value + " %", c);
                            break;
                        case enum_equip_entry_list.神佑护体:
                            itemValue.Init((enum_equip_basetype_list)i, e, value + " %", c);
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
                        case enum_equip_entry_list.洗炼次数:
                            break;
                        default:
                            if ((int)e >= 1000)//附加技能
                            {
                                if ((int)e >= 2000)//天赋
                                {
                                    string talent = "";
                                    talent = ArrayHelper.Find(SumSave.db_pet_talents, x => x.pet_talent_id == (((int)e) - 2000)).pet_talent_name;
                                    itemValue.Init_7(talent, value);
                                }
                                else
                                {
                                    string skill_name = "";
                                    skill_name = ArrayHelper.Find(SumSave.db_skills, x => x.id == (((int)e) - 1000)).show_name;
                                    itemValue.Init(skill_name, value);

                                }
                            }
                            break;
                    }
                }
            }
        }
    }

    private equip_type_info_item Get()
    {
        return Instantiate(equip_type_info_item_prefab, m_info_brom);
    }

    /// <summary>
    /// 被动技能
    /// </summary>
    /// <returns></returns>
    private string passive_skill_str()
    {
        Color color_list = UnityColorPresets.HexToColor("ffffff");
        string str = "";
         
        return str;
    }
    /// <summary>
    /// 主动技能描述
    /// </summary>
    /// <returns></returns>
    private string active_skill_str()
    {
        string str = "";
         
        return str;
    }

    public void InitShow()
    {
        Dictionary<string, db_towerbabel_vo> TowerBabel_skill_dic = SumSave.crt_user_towerbabel.GetArtifact;

        foreach (var item in TowerBabel_skill_item_dic)
        {
            if (TowerBabel_skill_dic.ContainsKey(item.Key))
            {
                item.Value.Data = TowerBabel_skill_dic[item.Key];
            }
        }
    }
}
