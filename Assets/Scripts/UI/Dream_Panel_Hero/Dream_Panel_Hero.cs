using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using MVC;
using System;
using Common;
using UnityEngine.UI;
using Components;
using TMPro;
using static UnityColorPresets;

public class Dream_Panel_Hero : Panel_Base
{

    private Transform m_info_brom,m_player_talent_brom;

    private info_item p_info_item_prefab;

    private btn_item p_btn_item_prefab;

    private player_talent_item p_player_talent_item_prefab;

    private List<enum_equip_entry_list> m_list = new List<enum_equip_entry_list>();

    private Dictionary<enum_equip_entry_list,info_item> info_Dic= new Dictionary<enum_equip_entry_list, info_item>();
    /// <summary>
    /// 玩家天赋和职业
    /// </summary>
    private TMP_Text player_talent_name;

    private Image offect_talent;
    /// <summary>
    /// 控制面板
    /// </summary>
    private GridLayoutGroup grid_layout_group;
    public override void Hide()
    {
        if(offect_talent.gameObject.activeInHierarchy)offect_talent.gameObject.SetActive(false);
        else 
        base.Hide();
    }

    public override void Initialize()
    {
        base.Initialize();
        m_info_brom=Find<Transform>("bg/show_list/Viewport/Content");

        p_info_item_prefab=Tool_UI.Find_Prefabs<info_item>("info_item");
        p_btn_item_prefab=Tool_UI.Find_Prefabs<btn_item>("btn_item");
        p_player_talent_item_prefab = Tool_UI.Find_Prefabs<player_talent_item>("player_talent_item");
        m_player_talent_brom = Find<Transform>("bg/job_list");
        grid_layout_group = Find<GridLayoutGroup>("bg/job_list");
        player_talent_name =Find<TMP_Text>("bg/show_title/info/info");
        offect_talent = Find<Image>("bg/offect_talent");
        InitObtain_Equip_List();
    }
    /// <summary>
    /// 初始化获得装备列表
    /// </summary>
    private void InitObtain_Equip_List()
    {
        m_list.Add(enum_equip_entry_list.生命值);
        m_list.Add(enum_equip_entry_list.魔法值);
        m_list.Add(enum_equip_entry_list.物理防御);
        m_list.Add(enum_equip_entry_list.魔法防御);
        m_list.Add(enum_equip_entry_list.物理攻击);
        m_list.Add(enum_equip_entry_list.魔法攻击);
        m_list.Add(enum_equip_entry_list.道术攻击);
        m_list.Add(enum_equip_entry_list.幸运);
        m_list.Add(enum_equip_entry_list.每秒回血);
        m_list.Add(enum_equip_entry_list.每秒回蓝);
        m_list.Add(enum_equip_entry_list.真实伤害);
        m_list.Add(enum_equip_entry_list.吸收伤害);
        m_list.Add(enum_equip_entry_list.命中);
        m_list.Add(enum_equip_entry_list.闪避);
        m_list.Add(enum_equip_entry_list.暴击属性);
        m_list.Add(enum_equip_entry_list.暴击伤害);
        m_list.Add(enum_equip_entry_list.生命属性);
        m_list.Add(enum_equip_entry_list.魔法属性);
        m_list.Add(enum_equip_entry_list.防御属性);
        m_list.Add(enum_equip_entry_list.魔防属性);
        m_list.Add(enum_equip_entry_list.物攻属性);
        m_list.Add(enum_equip_entry_list.魔攻属性);
        m_list.Add(enum_equip_entry_list.道攻属性);
        m_list.Add(enum_equip_entry_list.攻击速度);
        m_list.Add(enum_equip_entry_list.攻击范围);
        m_list.Add(enum_equip_entry_list.物伤减免);
        m_list.Add(enum_equip_entry_list.魔伤减免);
        m_list.Add(enum_equip_entry_list.金币掉落);
        m_list.Add(enum_equip_entry_list.经验加成);
        m_list.Add(enum_equip_entry_list.怪物爆率);
        m_list.Add(enum_equip_entry_list.极品爆率);
        for (int i= 0; i < m_list.Count; i++)
        {
            info_item item = Instantiate(p_info_item_prefab, m_info_brom);
            info_Dic.Add(m_list[i],item);
        }
    }

    public override void Show()
    {
        base.Show();
        Show_Talent();
        ShowInfo();
    }
    /// <summary>
    /// 显示天赋
    /// </summary>
    private void Show_Talent()
    {
        grid_layout_group.cellSize= new Vector2(240, 240);
        player_talent_name.text = (Hero_Type)SumSave.crtHero.job + "";
        ClearObject(m_player_talent_brom);
        if (SumSave.crtHero.job == 0)
        {
            //没有职业
            for (int i = 0; i < SumSave.db_player_talent_types.Count; i++)
            {
                player_talent_item item = Instantiate(p_player_talent_item_prefab, m_player_talent_brom);
                item.Init(SumSave.db_player_talent_types[i].talent_type_job, SumSave.db_player_talent_types[i].talent_type_job, -1);
                item.GetComponent<Button>().onClick.AddListener(() => { OnClick_Talent(item); });
            }

        }
        else
        { //有职业 
            if (SumSave.crtHero.SelectPos == -1)
            {
                //没选天赋
                for (int i = 0; i < SumSave.db_player_talent_types.Count; i++)
                {
                    if (SumSave.db_player_talent_types[i].talent_type_job == SumSave.crtHero.job)
                    {
                        for (int j = 0; j < SumSave.db_player_talent_types[i].talent_type_name.Count; j++)
                        {
                            player_talent_item item = Instantiate(p_player_talent_item_prefab, m_player_talent_brom);
                            item.Init(j + 1, SumSave.db_player_talent_types[i].talent_type_name[j], -1);
                            item.GetComponent<Button>().onClick.AddListener(() => { Open_Talent(); });
                        }
                    }
                }
            }
            else
            {
                grid_layout_group.cellSize = new Vector2(120, 120);
                //选了天赋
                for (int i = 0; i < SumSave.db_player_talents.Count; i++)
                {
                    if (SumSave.db_player_talents[i].job == SumSave.crtHero.job &&//职业相同
                                SumSave.db_player_talents[i].talent_type == SumSave.crtHero.SelectPos)//天赋类型相同
                    { 
                        player_talent_item item = Instantiate(p_player_talent_item_prefab, m_player_talent_brom);
                        int lv = Obtain_Talent_Lv(SumSave.db_player_talents[i].talent_name);
                        item.Init(SumSave.db_player_talents[i].talent_id, SumSave.db_player_talents[i].talent_name, lv);
                        item.GetComponent<Button>().onClick.AddListener(() => { Open_Talent(); }); 
                    }
                }
            }
        }
    }
    /// <summary>
    /// 打开天赋
    /// </summary>
    /// <param name="item"></param>
    private void Open_Talent()
    {
        offect_talent.gameObject.SetActive(true);
        ClearObject(offect_talent.transform);
        for (int i = 0; i < SumSave.db_player_talent_types.Count; i++)
        {
            if (SumSave.db_player_talent_types[i].talent_type_job == SumSave.crtHero.job)
            {
                for (int j = 0; j < SumSave.db_player_talent_types[i].talent_type_name.Count; j++)
                {
                    player_talent_item item = Instantiate(p_player_talent_item_prefab, offect_talent.transform);
                    item.Init(j + 1, SumSave.db_player_talent_types[i].talent_type_name[j], -1);
                    for (int k = 0; k < SumSave.db_player_talents.Count; k++)
                    {
                        if (SumSave.db_player_talents[k].job == SumSave.crtHero.job &&
                            SumSave.db_player_talents[k].talent_type == j + 1)//天赋类型相同
                        {
                            player_talent_item talent_Item = Instantiate(p_player_talent_item_prefab, offect_talent.transform);
                            int lv = Obtain_Talent_Lv(SumSave.db_player_talents[k].talent_name);
                            talent_Item.Init(SumSave.db_player_talents[k].talent_id, SumSave.db_player_talents[k].talent_name, lv);
                            talent_Item.GetComponent<Button>().onClick.AddListener(() => { Show_Select_Talent(talent_Item); });
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// 展示天赋和加点天赋
    /// </summary>
    /// <param name="item"></param>
    private void Show_Select_Talent(player_talent_item item)
    {
        db_player_talent_vo data = SumSave.db_player_talents.Find(x => x.talent_id == item.index);
        if (data == null)
        {
            Alert_Dec.Show("提示该天赋不存在");
            return;
        }
        string dec = "";
        /*
     * /// 100属性 生命百分比
    /// 101攻击百分比
    /// 102魔法百分比
    /// 103道术百分比
    /// 104防御百分比
    /// 106攻击速度百分比
    /// 
    /// 107 攻击
    /// 108 魔法
    /// 109 道术
    /// 110 防御
    /// 111 躲避
    /// 112 速度
    /// 113 命中
    /// 200技能
    /// 201 技能附加攻击 跟等级走
    /// 202 技能附加魔法 跟等级走
    /// 203 技能附加道术 跟等级走
    /// 204 技能附加双防御 
    /// 205 技能附加伤害
    /// 206 技能附加回血
    /// 207 技能附加攻击范围
    /// 208 无视防御
    /// 300召唤兽
    /// 301召唤兽攻击 跟着道术走
    /// 302召唤兽生命 跟着道术走
    /// 303召唤兽防御 跟着道术走
    /// 304召唤兽速度
    /// 305召唤兽死亡爆炸
    /// 400特殊效果 buff本源
    /// 401伤害 
    /// 402防御
    /// 403 速度
    /// 500 单体改群体
    /// 特殊效果
    /// 600 技能攻击个数
    /// 601 技能概率不消耗蓝
    /// 602 技能全体伤害
    /// 603 群体技能攻击范围
    /// 604 每s回复全体血量百分比
    /// 605 攻击击退敌人概率
    /// 
     */
        int lv = Obtain_Talent_Lv(data.talent_name);
        if (lv == data.ralent_need_uplv_value.Count)
        {
            dec += "[当前等级]:Lv." + lv + "已满级\n";
        }
        else
        {
            if (lv == 0)
            {
                dec += "[开启等级]:Lv." + data.talent_need_lv + "\n" + "[开启需求]" + data.ralent_need_uplv_value[0] + "*" + data.ralent_need_uplv[0] + "\n";
            }
            else
            {
                dec += "[当前等级]:Lv." + lv + "\n"+"[升级需求]" + data.ralent_need_uplv_value[lv] + "*" + data.ralent_need_uplv[lv] + "\n";
            }
        }
        int index = lv - 1;
        if (index < 0) index = 0;
        string skill_name = "";
        switch ((enum_talent_offect_list)data.talent_offect)
        {
            case enum_talent_offect_list.生命:
            case enum_talent_offect_list.攻击:
            case enum_talent_offect_list.魔法:
            case enum_talent_offect_list.道术:
            case enum_talent_offect_list.防御:
                if (data.correlation_skill == -1)
                {
                    dec += "[被动效果]\n" + (enum_talent_offect_list)data.talent_offect + "+" + data.talent_offect_value[index] + "%";
                }
                else
                {
                    skill_name = SumSave.db_skills.Find(x => x.id == data.correlation_skill).show_name;
                    dec += "[被动效果]\n" + (enum_talent_offect_list)data.talent_offect + "+" + Show_Color.Yellow(skill_name) + "等级 * " + data.talent_offect_value[index] + "%";

                }
                break;
            case enum_talent_offect_list.攻击速度:
            case enum_talent_offect_list.物理攻击:
            case enum_talent_offect_list.魔法攻击:
            case enum_talent_offect_list.道术攻击:
            case enum_talent_offect_list.防御值:
            case enum_talent_offect_list.躲避:
            case enum_talent_offect_list.命中:
                if (data.correlation_skill == -1)
                {
                    dec += "[被动效果]\n" + (enum_talent_offect_list)data.talent_offect + "+" + data.talent_offect_value[index] + "";
                }
                else
                {
                    skill_name = SumSave.db_skills.Find(x => x.id == data.correlation_skill).show_name;
                    dec += "[被动效果]\n" + (enum_talent_offect_list)data.talent_offect + "+" + Show_Color.Yellow(skill_name) + "等级 * " + data.talent_offect_value[index] + "";

                }
                break;
            case enum_talent_offect_list.技能:
                break;
            case enum_talent_offect_list.附加攻击:
            case enum_talent_offect_list.附加魔法:
            case enum_talent_offect_list.附加道术:
            case enum_talent_offect_list.附加双防:
                if (data.correlation_skill == -1)
                {
                    dec += "[战斗效果]\n" + (enum_talent_offect_list)data.talent_offect + "+" + data.talent_offect_value[index] + "";
                }
                else
                {
                    skill_name = SumSave.db_skills.Find(x => x.id == data.correlation_skill).show_name;
                    dec += "[战斗效果]\n" + (enum_talent_offect_list)data.talent_offect + "+" + Show_Color.Yellow(skill_name) + "等级 * " + data.talent_offect_value[index] + "";

                }
                break;
            case enum_talent_offect_list.附加回血:
                if (data.correlation_skill == -1)
                {
                    dec += "[战斗效果]\n" + (enum_talent_offect_list)data.talent_offect + "+" + data.talent_offect_value[index] + "";
                }
                else
                {
                    skill_name = SumSave.db_skills.Find(x => x.id == data.correlation_skill).show_name;
                    dec += "[专属技能效果]\n" + Show_Color.Yellow(skill_name) + (enum_talent_offect_list)data.talent_offect + "+" + data.talent_offect_value[index] + "";

                }
                break;
            case enum_talent_offect_list.附加伤害:
            case enum_talent_offect_list.附加攻击范围:
            case enum_talent_offect_list.无视防御:
                if (data.correlation_skill == -1)
                {
                    dec += "[战斗效果]\n" + (enum_talent_offect_list)data.talent_offect + "+" + data.talent_offect_value[index] + "%";
                }
                else
                {
                    skill_name = SumSave.db_skills.Find(x => x.id == data.correlation_skill).show_name;
                    dec += "[专属技能效果]\n" + Show_Color.Yellow(skill_name) + (enum_talent_offect_list)data.talent_offect + "+" + data.talent_offect_value[index] + "%";

                }
                break;
            case enum_talent_offect_list.召唤兽:
                break;
            case enum_talent_offect_list.召唤兽攻击:
            case enum_talent_offect_list.召唤兽生命:
            case enum_talent_offect_list.召唤兽防御:
            case enum_talent_offect_list.召唤兽速度:
                dec += "[战斗效果]\n" + (enum_talent_offect_list)data.talent_offect + "+" + data.talent_offect_value[index] + "";
                break;
            case enum_talent_offect_list.召唤兽死亡爆炸:
                dec += "[战斗效果]\n召唤兽死亡时对周围目标造成最大生命值" + "+" + data.talent_offect_value[index] + "%的伤害";
                break;
            case enum_talent_offect_list.特殊效果:
                break;
            case enum_talent_offect_list.临时伤害:
            case enum_talent_offect_list.临时防御:
            case enum_talent_offect_list.临时速度:
                dec += "[战斗效果]\n自身生命值每降低10%" + " \n获得 " + (enum_talent_offect_list)data.talent_offect + "+" + data.talent_offect_value[index] + "%";
                break;
            case enum_talent_offect_list.单体改群体:
                skill_name = SumSave.db_skills.Find(x => x.id == data.correlation_skill).show_name;
                dec += "[战斗效果]\n" + Show_Color.Yellow(skill_name) + "变为群体技能" +
                    "\n群体技能攻击伤害 = " + skill_name + " 的 " + data.talent_offect_value[index] + "%";
                break;
            case enum_talent_offect_list.技能攻击个数:
                skill_name = SumSave.db_skills.Find(x => x.id == data.correlation_skill).show_name;
                dec += "[战斗效果]\n" + Show_Color.Yellow(skill_name) + " 的攻击次数变为 " + data.talent_offect_value[index] + "";
                break;
            case enum_talent_offect_list.技能概率不消耗蓝:
                if (data.correlation_skill == -1)
                {
                    dec += "[战斗效果]\n" + "释放技能" + " " + data.talent_offect_value[index] + "% 概率不消耗魔法";
                }
                else
                {
                    skill_name = SumSave.db_skills.Find(x => x.id == data.correlation_skill).show_name;
                    dec += "[战斗效果]\n" + "释放技能 " + Show_Color.Yellow(skill_name) + " " + data.talent_offect_value[index] + "%概率不消耗魔法";

                }
                break;
            case enum_talent_offect_list.技能全体伤害:
                break;
            case enum_talent_offect_list.群体技能攻击范围:
                if (data.correlation_skill == -1)
                {
                    dec += "[战斗效果]\n" + "技能攻击范围" + "+" + data.talent_offect_value[index] + "% ";
                }
                else
                {
                    skill_name = SumSave.db_skills.Find(x => x.id == data.correlation_skill).show_name;
                    dec += "[战斗效果]\n" + "技能 " + Show_Color.Yellow(skill_name) + " 攻击范围 + " + data.talent_offect_value[index] + "%";

                }
                break;
            case enum_talent_offect_list.每秒回复全体血量百分比:
                dec += "[被动效果]\n" + "每秒回复全体血量" + "+" + data.talent_offect_value[index] + "% ";
                break;
            case enum_talent_offect_list.攻击击退敌人概率:
                if (data.correlation_skill == -1)
                {
                    dec += "[战斗效果]\n" + "攻击时" + data.talent_offect_value[index] + "%" + "击退敌人"; 
                }
                else
                {
                    skill_name = SumSave.db_skills.Find(x => x.id == data.correlation_skill).show_name;
                    dec += "[战斗效果]\n" + "技能 " + Show_Color.Yellow(skill_name) + " 攻击时" + data.talent_offect_value[index] + "%" + "击退敌人";

                }
                break;
        }
        if ((SumSave.crtHero.SelectPos==-1||data.talent_type == SumSave.crtHero.SelectPos) && data.job==SumSave.crtHero.job)
        {
            if (lv == data.ralent_need_uplv_value.Count)
            {
                Alert.Show(data.talent_name, dec);
            }
            else
            {
                if (lv == 0)
                {
                    dec += "\n点击确定开启 "+data.talent_name + "\n";
                    if (SumSave.crtHero.SelectPos == -1)
                    {
                        dec += Show_Color.Red("重要提醒:仅可选择一个分支提升");
                    }
                }
                else
                {
                    dec += "\n点击确定提升 " + data.talent_name + "\n";
                }
                Alert.Show(data.talent_name, dec, upLvTalent, data);

            }
        }
        else Alert.Show(data.talent_name, dec);

    }
    /// <summary>
    /// 开启/升级天赋
    /// </summary>
    /// <param name="arg0"></param>
    private void upLvTalent(object arg0)
    {
        db_player_talent_vo data = (db_player_talent_vo)arg0;
        if (SumSave.crtHero.lv < data.talent_need_lv) { Alert.Show("升级失败", "等级不足"); return; }
        int lv = Obtain_Talent_Lv(data.talent_name);
        int index = lv;
        if (index <= 0) index = 0;
        if (SumSave.crtHero.SelectPos == -1)
        {
            if (data.talent_need_lv == 0)
            {
                Need_Condition(data.ralent_need_uplv_value[index], data.ralent_need_uplv[index]);
                if (Return_Condition())
                {
                    SumSave.crtHero.SelectPos = data.talent_type;
                    SumSave.crtHero.talent.Add((data, 1));
                    SumSave.crtHero.MysqlData();
                    SendNotification(NotiList.Refresh_Max_Hero_Attribute);
                    Alert_Dec.Show("开启成功,天赋" + data.talent_name + "已开启");
                    Show();
                    Open_Talent();
                }
                else Alert_Dec.Show("开启失败,材料不足天赋" + data.talent_name + "未开启");
            }
            else
            {
                Alert.Show("开启失败", "请按照顺序开启天赋");
                return;
            }
        }
        else
        {
            if (lv >= data.ralent_need_uplv_value.Count)
            {
                Alert.Show("升级失败", "天赋" + data.talent_name + "已满级");
                return;
            }
            if (lv == 0)
            {
                List<db_player_talent_vo> data_list = SumSave.db_player_talents.FindAll(x => x.job == data.job && x.talent_type == SumSave.crtHero.SelectPos);
                data_list = ArrayHelper.Ascending(data_list, x => x.talent_need_lv);
                index = -1;
                for (int i = 0; i < data_list.Count; i++)
                { 
                    if(data_list[i].talent_name==data.talent_name)
                    {
                        index = i;
                        break;
                    }
                }
                if (index > 0)
                {
                    if (Obtain_Talent_Lv(data_list[index - 1].talent_name) > 0)
                    {
                        UpLv(data, lv);
                    }
                    else Alert.Show("升级失败", "请先开启 " + data_list[index - 1].talent_name + " 天赋");
                }

            }
            else
            {
                UpLv(data, lv);
            }
        }
    }
    /// <summary>
    /// 升级天赋
    /// </summary>
    /// <param name="data"></param>
    /// <param name="lv"></param>
    private void UpLv(db_player_talent_vo data,int lv)
    {
        int index = lv;
        if(index<=0) index = 0;
        Need_Condition(data.ralent_need_uplv_value[index], data.ralent_need_uplv[index]);
        if (Return_Condition())
        {
            bool exist = true;
            for (int i = 0; i < SumSave.crtHero.talent.Count; i++)
            {
                if (SumSave.crtHero.talent[i].Item1.talent_name == data.talent_name)
                {
                    SumSave.crtHero.talent[i] = (data, SumSave.crtHero.talent[i].Item2 + 1);
                    exist = false;
                    break;
                }
            }

            if (exist)
            { 
                SumSave.crtHero.talent.Add((data, 1));
            }
            SumSave.crtHero.MysqlData();
            SendNotification(NotiList.Refresh_Max_Hero_Attribute);
            Alert_Dec.Show("升级成功,天赋" + data.talent_name + "已升级");
            Open_Talent();
            ShowInfo();
        }
    }

    /// <summary>
    /// 获取天赋等级
    /// </summary>
    /// <param name="talent_name"></param>
    /// <returns></returns>
    private int Obtain_Talent_Lv(string talent_name)
    {
        foreach (var talent in SumSave.crtHero.talent)
        {
            if (talent.Item1.talent_name == talent_name)
            {
                return talent.Item2;
            }
        }
        return 0;
    }
     
    /// <summary>
    /// 选择职业
    /// </summary>
    /// <param name="item"></param>
    private void OnClick_Talent(player_talent_item item)
    {
        Alert.Show("激活职业", "当前选择职业 " + (Hero_Type)item.index + "\n请确认是否激活?", confirm_job, item.index);
    }
    /// <summary>
    /// 确认职业
    /// </summary>
    /// <param name="arg0"></param>
    private void confirm_job(object arg0)
    {
        SumSave.crtHero.job = int.Parse(arg0.ToString());
        SumSave.crtHero.SelectPos = -1;
        SumSave.crtHero.MysqlData();
        SendNotification(NotiList.Refresh_Max_Hero_Attribute);
        UI_Manager.I.GetPanel<PanelMian>().Show();
        Game_Omphalos.i.archive();
        Show();
     }

    /// <summary>
    /// 
    /// 
    /// 显示信息
    /// </summary>
    private void ShowInfo()
    {
        FinalBattleValueVO data = SumSave.crtMaxBattle.data;
        foreach (enum_equip_entry_list item in info_Dic.Keys)
        {
            switch (item)
            { 
               case enum_equip_entry_list.生命值:
                   info_Dic[item].SetInfo(item, data.battle_maxhp);break;
               case enum_equip_entry_list.魔法值:
                   info_Dic[item].SetInfo(item, data.battle_maxmp);break;
               case enum_equip_entry_list.物理防御:
                   info_Dic[item].SetInfo(item, data.ac+" - "+data.ac2);break;
               case enum_equip_entry_list.魔法防御:
                   info_Dic[item].SetInfo(item, data.mac+" - "+data.mac2);break;
               case enum_equip_entry_list.物理攻击:
                   info_Dic[item].SetInfo(item, data.dc+" - "+data.dc2);break;
               case enum_equip_entry_list.魔法攻击:
                   info_Dic[item].SetInfo(item, data.mc+" - "+data.mc2);break;
               case enum_equip_entry_list.道术攻击:
                   info_Dic[item].SetInfo(item, data.sc+" - "+data.sc2);break;
               case enum_equip_entry_list.每秒回血:
                   info_Dic[item].SetInfo(item, data.hpRegen);break;
               case enum_equip_entry_list.每秒回蓝:
                   info_Dic[item].SetInfo(item, data.mpRegen);break;
               case enum_equip_entry_list.真实伤害:
                   info_Dic[item].SetInfo(item, data.battle_Damage);break;
               case enum_equip_entry_list.吸收伤害:
                   info_Dic[item].SetInfo(item, data.battle_def);break;
               case enum_equip_entry_list.幸运:
                   info_Dic[item].SetInfo(item, data.lucky);break;
               case enum_equip_entry_list.命中:
                    
                   info_Dic[item].SetInfo(item, data.hit+" %",HexToColor("bbfdff"));break;
               case enum_equip_entry_list.闪避:
                   info_Dic[item].SetInfo(item, data.dodge+" %",HexToColor("bbfdff"));break;
               case enum_equip_entry_list.生命属性:
                   info_Dic[item].SetInfo(item, data.battle_hp+" %",HexToColor("bbfdff"));break;
               case enum_equip_entry_list.魔法属性:
                   info_Dic[item].SetInfo(item, data.battle_mp+" %",HexToColor("bbfdff"));break;
               case enum_equip_entry_list.防御属性:
                   info_Dic[item].SetInfo(item, data.battle_ac+" %",HexToColor("bbfdff"));break;
               case enum_equip_entry_list.魔防属性:
                   info_Dic[item].SetInfo(item, data.battle_mac+" %",HexToColor("bbfdff"));break;
               case enum_equip_entry_list.物攻属性:
                   info_Dic[item].SetInfo(item, data.battle_dc+" %",HexToColor("bbfdff"));break;
               case enum_equip_entry_list.魔攻属性:
                   info_Dic[item].SetInfo(item, data.battle_mc+" %",HexToColor("bbfdff"));break;
               case enum_equip_entry_list.道攻属性:
                   info_Dic[item].SetInfo(item, data.battle_sc+" %",HexToColor("bbfdff"));break;
               case enum_equip_entry_list.攻击速度:
                   info_Dic[item].SetInfo(item, data.battle_speed+"",HexToColor("bbfdff"));break;
               case enum_equip_entry_list.攻击范围:
                   info_Dic[item].SetInfo(item, data.battle_range+"",HexToColor("bbfdff"));break;
               case enum_equip_entry_list.暴击属性:
                   info_Dic[item].SetInfo(item, data.crit+" %",HexToColor("bbfdff"));break;
               case enum_equip_entry_list.暴击伤害:
                   info_Dic[item].SetInfo(item, data.critDmg+" %",HexToColor("bbfdff"));break;
               case enum_equip_entry_list.物伤减免:
                   info_Dic[item].SetInfo(item, data.damage_reduction+" %",HexToColor("ffff00"));break;
               case enum_equip_entry_list.魔伤减免:
                   info_Dic[item].SetInfo(item, data.magic_damage_reduction + " %",HexToColor("ffff00"));break;
               case enum_equip_entry_list.怪物爆率:
                   info_Dic[item].SetInfo(item, SumSave.crtMaxBattle.drop_bonus + " %",HexToColor("ffff00"));break;
               case enum_equip_entry_list.极品爆率:
                   info_Dic[item].SetInfo(item, SumSave.crtMaxBattle.quality_bonus + " %",HexToColor("ffff00"));break;
               case enum_equip_entry_list.金币掉落:
                   info_Dic[item].SetInfo(item, SumSave.crtMaxBattle.gold_bonus + " %",HexToColor("ffff00"));break;
               case enum_equip_entry_list.经验加成:
                   info_Dic[item].SetInfo(item, SumSave.crtMaxBattle.exp_bonus + " %",HexToColor("ffff00"));break; 
            }
        }

    }

    protected override void Awake()
    {
        base.Awake();
    }
}
