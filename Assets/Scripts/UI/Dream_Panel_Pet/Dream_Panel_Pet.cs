using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Dream_Panel_Pet : Panel_Base
{
    private enum replace_state
    {
        上阵,
        改名,
        放生,
        学习,
        吞噬,
        炼妖,
        一键学习,
        继承,
    }
    private Transform m_info_brom, m_btn_brom, m_Talent_brom,m_icon_brom;

    private info_item p_info_item_prefab;

    private btn_item p_btn_item_prefab;

    private pet_talent_item p_talent_item_prefab;

    private List<enum_equip_entry_list> m_list = new List<enum_equip_entry_list>();

    private Dictionary<enum_equip_entry_list, info_item> info_Dic = new Dictionary<enum_equip_entry_list, info_item>();
    /// <summary>
    /// 当前宠物
    /// </summary>
    private db_pet_vo crt_pet;

    private TMP_Text info_name;

    private PanelMian panelMian;

    private Dream_Panel_Bag dream_Panel_Bag;
    /// <summary>
    /// 改名预制件
    /// </summary>
    private Transform m_input_brom;

    private input_offect input_offect_prefab, crt_input_offect;
    /// <summary>
    /// 当前学习技能
    /// </summary>
    private pet_study crt_study;
    /// <summary>
    /// 当前吞噬
    /// </summary>
    private pet_devour crt_devour;
    /// <summary>
    /// 当前炼妖
    /// </summary>
    private pet_Demon crt_demon;

    private pet_inheritance crt_inheritance;

    private Image offect;
    public override void Hide()
    {
        base.Hide();
    }

    public override void Initialize()
    {
        base.Initialize();
        panelMian = UI_Manager.I.GetPanel<PanelMian>();
        dream_Panel_Bag = UI_Manager.I.GetPanel<Dream_Panel_Bag>();
        m_icon_brom = Find<Transform>("bg/offect_list/icon");
        m_btn_brom = Find<Transform>("bg/battle_btn_list/Scroll View/Viewport/Content");
        m_info_brom = Find<Transform>("bg/offect_list/show_list/Viewport/Content");
        p_info_item_prefab = Tool_UI.Find_Prefabs<info_item>("info_item");
        p_btn_item_prefab = Tool_UI.Find_Prefabs<btn_item>("btn_item");
        m_Talent_brom= Find<Transform>("bg/offect_list/talent_list/Viewport/Content");
        p_talent_item_prefab= Tool_UI.Find_Prefabs<pet_talent_item>("pet_talent_item");
        info_name = Find<TMP_Text>("bg/show_title/info/info");
        InitObtain_Equip_List();
        m_input_brom = Find<Transform>("bg");
        input_offect_prefab = Tool_UI.Find_Prefabs<input_offect>("input_offect");
        offect = Find<Image>("offect");
        crt_study = Find<pet_study>("offect/pet_study");
        crt_devour= Find<pet_devour>("offect/pet_devour");
        crt_demon = Find<pet_Demon>("offect/pet_Demon");
        crt_inheritance= Find<pet_inheritance>("offect/pet_inheritance");
    }
    /// <summary>
    /// 初始化获得装备列表
    /// </summary>
    private void InitObtain_Equip_List()
    {
        m_list.Add(enum_equip_entry_list.物理防御);
        m_list.Add(enum_equip_entry_list.魔法防御);
        m_list.Add(enum_equip_entry_list.物理攻击);
        m_list.Add(enum_equip_entry_list.魔法攻击);
        m_list.Add(enum_equip_entry_list.道术攻击);
        for (int i = 0; i < m_list.Count; i++)
        {
            info_item item = Instantiate(p_info_item_prefab, m_info_brom);
            info_Dic.Add(m_list[i], item);
        }
    }

    public override void Show()
    {
        base.Show();
    }
    /// <summary>
    /// 获取宠物信息
    /// </summary>
    /// <param name="data"></param>
    /// <param name="index"></param>
    public void InitPet(dream_BagItem data, Panel_BagType index)
    {
        crt_pet = data.Pet_Data;
        info_name.text = crt_pet.crt_name; 
        Show_Icon();
        ShowInfo();
        List < replace_state > btn_list = new List<replace_state>();
        switch (index)
        {
            case Panel_BagType.装备:
                btn_list.Add(replace_state.上阵);
                btn_list.Add(replace_state.改名);
                btn_list.Add(replace_state.放生);
                btn_list.Add(replace_state.一键学习);
                break;
            case Panel_BagType.已装备:
                btn_list.Add(replace_state.学习);
                btn_list.Add(replace_state.吞噬);
                btn_list.Add(replace_state.炼妖);
                btn_list.Add(replace_state.继承);
                break;
            case Panel_BagType.展示:
                break;
        }
        Init_btn(btn_list);
    }
    protected void Refresh()
    {
        panelMian.Show();
        dream_Panel_Bag.Show();
        Hide();
    }
    private void Show_Icon()
    {
        ClearObject(m_icon_brom);
        Instantiate(Resources.Load<GameObject>("UI/frame/frame/" + 6), m_icon_brom);
        GameObject icon = Instantiate(Resources.Load<GameObject>("UI/Prefabs/panel_pet/" + crt_pet.pet_id), m_icon_brom);
        icon.transform.localScale = new Vector3(crt_pet.pet_scale, crt_pet.pet_scale, crt_pet.pet_scale);
    }

    private void Init_btn(List<replace_state> btn_list)
    {
        ClearObject(m_btn_brom);
        for (int i = 0; i < btn_list.Count; i++)
        {
            btn_item btn = Instantiate(p_btn_item_prefab, m_btn_brom);
            btn.Show((int)(btn_list[i]), btn_list[i]);
            btn.GetComponent<Button>().onClick.AddListener(() => { btn_click(btn); });
        }
    }
    /// <summary>
    /// 功能开关
    /// </summary>
    /// <param name="btn"></param>
    private void btn_click(btn_item btn)
    {
        switch ((replace_state)btn.index)
        {
            case replace_state.上阵:
                List<db_pet_vo> list = SumSave.crt_pet.GetPets;
                list.Remove(crt_pet);
                list.Add(SumSave.crt_pet.GetPet);
                SumSave.crt_pet.SetPets = list;
                SumSave.crt_pet.SetPet = crt_pet;
                SendNotification(NotiList.Refresh_Max_Hero_Attribute);
                Refresh();
                break;
            case replace_state.改名:
                if (crt_input_offect == null)
                {
                    crt_input_offect = Instantiate(input_offect_prefab, m_input_brom);
                    crt_input_offect.gameObject.SetActive(true);
                    crt_input_offect.Init(replace_state.改名, "对" + crt_pet.crt_name + "进行调整");
                    crt_input_offect.GetConfirm.onClick.AddListener(() => { confirm(); });
                }
                else
                {
                    crt_input_offect.gameObject.SetActive(true);
                    crt_input_offect.Init(replace_state.改名, "对" + crt_pet.crt_name + "进行调整");
                }

                break;
            case replace_state.放生:
                Alert.Show("放生", "是否确定放生" + crt_pet.crt_name + "?", confirm_release, null);
                break;
            case replace_state.学习:
                offect.gameObject.SetActive(true);
                crt_study.gameObject.SetActive(true);
                crt_study.Init(crt_pet);
                break;
            case replace_state.吞噬:
                offect.gameObject.SetActive(true);
                crt_devour.gameObject.SetActive(true);
                crt_devour.Init(crt_pet);
                break;
            case replace_state.炼妖:
                offect.gameObject.SetActive(true);
                crt_demon.gameObject.SetActive(true);
                crt_demon.Init(crt_pet);
                break;
           case replace_state.一键学习:
                Alert.Show("一键学习", "自动消耗 兽诀 将当前宠物天赋提升到5星?", confirm_Anto_Study, null);
                break;
            case replace_state.继承:
                offect.gameObject.SetActive(true);
                crt_inheritance.gameObject.SetActive(true);
                crt_inheritance.Init(crt_pet);
                break;
        }
    }

    private void confirm_release(object arg0)
    {
        List<db_pet_vo> lists = SumSave.crt_pet.GetPets;
        lists.Remove(crt_pet);
        SumSave.crt_pet.SetPets = lists;
        Refresh();
    }

    /// <summary>
    /// 一键学习
    /// </summary>
    /// <param name="arg0"></param>
    private void confirm_Anto_Study(object arg0)
    {
        StartCoroutine(Anto_petStudy());
    }

    private IEnumerator Anto_petStudy()
    {
        Clear_Condition();
        while (crt_pet.GetCrtTalent.Count < 5)
        {
            Need_Condition("兽诀", 1);
            if (!Return_Condition())
            { 
                Alert_Dec.Show("物品不足"); 
                yield break;
            }
            OnClick_btn();
            yield return new WaitForSeconds(1f);
        }
        Alert_Dec.Show("学习完成");
        Game_Omphalos.i.archive();
        Refresh();
    }

    private void OnClick_btn()
    { 
        int pet_talent_level = 1; 
        List<db_pet_talent_vo> list_vo = ArrayHelper.FindAll(SumSave.db_pet_talents, e => e.pet_talent_level == pet_talent_level);
        List<db_pet_talent_vo> CrtTalent = crt_pet.GetCrtTalent;
        if (list_vo.Count > 0)
        {
            db_pet_talent_vo talent = Obtain_Talent(CrtTalent, list_vo);
            if (talent == null) { Alert_Dec.Show("似乎什么都没有发生"); return; }
            if (CrtTalent.Count < 5)//数量低于5个
            {
                if (Random.Range(0, 100) >= m_Talent_brom.childCount * 20)
                {
                    //新增
                    CrtTalent.Add(talent);
                }
                else
                {
                    //替换
                    CrtTalent[Random.Range(crt_pet.pet_id >= 8 ? 2 : 1, CrtTalent.Count)] = talent;
                }
            }
            else
                CrtTalent[Random.Range(crt_pet.pet_id >= 8 ? 2 : 1, CrtTalent.Count)] = talent;
            update_pet();
            Alert_Dec.Show("获得天赋" + talent.pet_talent_name);
            SumSave.crt_pet.MysqlData();
        }
    }

    private db_pet_talent_vo Obtain_Talent(List<db_pet_talent_vo> CrtTalent, List<db_pet_talent_vo> list)
    {
        db_pet_talent_vo talent = list[Random.Range(0, list.Count)];
        int number = 0;
        while (CrtTalent.Contains(talent) && number < 1000)
        {
            talent = list[Random.Range(0, list.Count)];
            number++;
        }
        if (number >= 1000) talent = null;
        return talent;
    }

    /// <summary>
    /// 
    /// </summary>
    private void confirm()
    {
        if (crt_input_offect.GetInput != "")
        {
            crt_pet.crt_name= crt_input_offect.GetInput;
            SumSave.crt_pet.MysqlData();
            info_name.text = crt_pet.crt_name;
            crt_input_offect.gameObject.SetActive(false);
            Alert_Dec.Show("修改成功");
            Refresh();
        }
        else Alert_Dec.Show("请输入不含特殊符号的内容");
    }
    /// <summary>
    /// 更新宠物信息
    /// </summary>
    protected void update_pet()
    {
        ShowInfo();
    }
    /// <summary>
    /// 显示信息
    /// </summary>
    private void ShowInfo()
    {
        (int, int, int, int, int) CrtAttr = crt_pet.GetCrtAttr;
        (int, int, int, int, int) AddAttr = crt_pet.GetAddAttr;
        List<db_pet_talent_vo> CrtTalent = crt_pet.GetCrtTalent;
        Bag_Base_VO bag = ArrayHelper.Find(SumSave.db_stditems, e => e.Name == crt_pet.pet_name);
        foreach (enum_equip_entry_list item in info_Dic.Keys)
        {
            switch (item)
            {
                case enum_equip_entry_list.物理防御:
                    info_Dic[item].SetInfo(item, 1 + " - " +(bag.ac2+ CrtAttr.Item1) +Show_Color.Red(" +( " + AddAttr.Item1+")")); break;
                case enum_equip_entry_list.魔法防御:
                    info_Dic[item].SetInfo(item, 1 + " - " + (bag.mac2+ CrtAttr.Item2) + Show_Color.Red(" +( " + AddAttr.Item2 + ")")); break;
                case enum_equip_entry_list.物理攻击:
                    info_Dic[item].SetInfo(item, 1 + " - " + (bag.dc2+ CrtAttr.Item3) + Show_Color.Red(" +( " + AddAttr.Item3 + ")")); break;
                case enum_equip_entry_list.魔法攻击:
                    info_Dic[item].SetInfo(item, 1 + " - " + (bag.mc2+ CrtAttr.Item4) + Show_Color.Red(" +( " + AddAttr.Item4 + ")")); break;
                case enum_equip_entry_list.道术攻击:
                    info_Dic[item].SetInfo(item, 1 + " - " + (bag.sc2+ CrtAttr.Item5 )+ Show_Color.Red(" +( " + AddAttr.Item5 + ")")); break;  
            }
        }
        ClearObject(m_Talent_brom);
        for (int i = 0; i < CrtTalent.Count; i++)
        {
            pet_talent_item item = Instantiate(p_talent_item_prefab, m_Talent_brom);
            item.Init(CrtTalent[i]);
            item.GetComponent<Button>().onClick.AddListener(() => { show_Talent(item); });
        }
    }
    /// <summary>
    ///  显示天赋
    /// </summary>
    /// <param name="item"></param>
    private void show_Talent(pet_talent_item item)
    {
        string dec = Tool_Battle.show_Talent(item);
        Alert.Show(item.GetTalentValue.pet_talent_name, dec);
        //db_pet_talent_vo talent = item.GetTalentValue;
        //string dec = talent.pet_talent_name+"\n";
        ///// <summary>
        ///// 触发类型 
        ///// 1作用自身
        ///// 1.1物理伤害百分比
        ///// 1.2魔法伤害百分比
        ///// 1.3召唤兽伤害百分比
        ///// 1.4物理防御
        ///// 1.5魔法防御
        ///// 1.6回复hp
        ///// 1.7回复mp
        ///// 1.8减少物理伤害%
        ///// 1.9减少魔法伤害%
        ///// 1.11增加躲避    
        ///// 2战斗触发
        ///// 2.1连击  
        ///// 2.2忽视物理防御
        ///// 2.3忽视魔法防御
        ///// 2.4忽视召唤兽防御
        ///// 2.5反镇
        ///// 2.6防爆
        ///// 2.7招架
        ///// 2.8反击
        ///// 2.10技能释放消耗减少
        ///// 3特殊
        ///// 1 增加生命上限
        ///// 2 增加基础属性
        ///// 3 概率随机传送一个敌人
        ///// 4 击杀后追击另一个目标
        ///// 5 攻击无视防御
        ///// 6 攻击概率10倍
        ///// 7 攻击概率斩杀
        ///// 8 连击效果提升
        //switch (talent.pet_talent_type)
        //{
        //    case 3:
        //        switch ((talent.pet_talent_offect))
        //        {
        //            case 1: dec += "生命上限 + " + Show_Color.Red(talent.pet_talent_offectvalue)+" %"; break;
        //            case 2: dec += "基础属性\n" + enum_equip_entry_list.物理攻击+" +"+Show_Color.Red(talent.pet_talent_offectvalue*SumSave.crtHero.lv)
        //                    + "\n" + enum_equip_entry_list.魔法攻击 + " +" + Show_Color.Red(talent.pet_talent_offectvalue * SumSave.crtHero.lv)
        //                    + "\n" + enum_equip_entry_list.道术攻击 + " +" + Show_Color.Red(talent.pet_talent_offectvalue * SumSave.crtHero.lv)
        //                    + Show_Color.Grey("\n(每级 + " + talent.pet_talent_offectvalue + ")");
        //                ; break;
        //            case 3: dec += "攻击目标时 " + Show_Color.Red(talent.pet_talent_offecttype + "%") + " 概率 触发 " + "随机传送一个敌人"; break;
        //            case 4: dec += "击杀后追击另一个目标\n每次触发消耗最大Hp的"+Show_Color.Red("10%"); break;
        //            case 5: dec += "攻击目标时 "+ Show_Color.Red(talent.pet_talent_offecttype + "%") + " 概率 触发 " + Show_Color.Red("无视防御") + " 效果"; ; break;
        //            case 6: dec += "攻击目标时 " + Show_Color.Red(talent.pet_talent_offecttype + "%") + " 概率 触发 " + Show_Color.Red(" 伤害 * "+ talent.pet_talent_offectvalue) + " 效果"; break;
        //            case 7: dec += "攻击目标时 当目标血量低于" + Show_Color.Red(talent.pet_talent_offecttype + "%") + " 时 触发 " + Show_Color.Red("斩杀") + " 效果"; break;
        //            case 8: dec += "连击效果提升 " + Show_Color.Red(talent.pet_talent_offecttype + "%"); break;
        //        }
        //        break;
        //    case 1:
        //        switch ((talent.pet_talent_offect))
        //        {
        //            case 1:dec += "物理伤害 + " + Show_Color.Red( talent.pet_talent_offectvalue )+ " %"; break;
        //            case 2: dec += "魔法伤害 + " + Show_Color.Red(talent.pet_talent_offectvalue) + " %"; break;
        //            case 3: dec += "召唤兽伤害 + " + Show_Color.Red(talent.pet_talent_offectvalue) + " %"; break;
        //            case 4: dec += "物理防御 + " + Show_Color.Red(talent.pet_talent_offectvalue * SumSave.crtHero.lv) + Show_Color.Grey("\n(每级 + " + talent.pet_talent_offectvalue + ")"); break;
        //            case 5: dec += "魔法防御 + " + Show_Color.Red(talent.pet_talent_offectvalue * SumSave.crtHero.lv) + Show_Color.Grey("\n(每级 + " + talent.pet_talent_offectvalue + ")"); ; break;
        //            case 6: dec += "每s回复 + " + Show_Color.Red(talent.pet_talent_offectvalue * SumSave.crtHero.lv)+" Hp" + Show_Color.Grey("\n(每级 + " + talent.pet_talent_offectvalue + ")"); ; break;
        //            case 7: dec += "每s回复 + " + Show_Color.Red(talent.pet_talent_offectvalue * SumSave.crtHero.lv) + " Mp" + Show_Color.Grey("\n(每级 + " + talent.pet_talent_offectvalue + ")"); ; break;
        //            case 8: dec += "受到物理伤害减少  " + Show_Color.Red(talent.pet_talent_offectvalue) + " %"; break;
        //            case 9: dec += "受到魔法伤害减少  " + Show_Color.Red(talent.pet_talent_offectvalue) + " %"; break;
        //            case 11: dec += "躲避 + " + Show_Color.Red(talent.pet_talent_offectvalue) + " "; break;

        //            default:
        //                break;
        //        }
        //        break;
        //    case 2:
        //        switch ((talent.pet_talent_offect)) 
        //        {
        //            case 1:
        //                dec += "攻击目标时 "+ Show_Color.Red((Hero_Type)(talent.pet_talent_job)) + " 职业 "
        //                    //+ (talent.pet_talent_job == 3 ? "(召唤兽)" : "")
        //                    + Show_Color.Red(talent.pet_talent_offecttype + "%") + " 概率 触发 " + Show_Color.Red(talent.pet_talent_offectvalue + "%") + " 伤害"; break;
        //            case 2:
        //            case 3:
        //            case 4:
        //                dec += "攻击目标时 " + Show_Color.Red((Hero_Type)(talent.pet_talent_offect-1)) + " 职业 "
        //                    //+ (talent.pet_talent_offect == 4 ? "(召唤兽)" : "")
        //                    + Show_Color.Red(talent.pet_talent_offecttype + "%") + " 概率 忽视 " + Show_Color.Red(talent.pet_talent_offectvalue * SumSave.crtHero.lv) + " 防御"; break;
        //            case 5:
        //                dec+="受到伤害时 "+Show_Color.Red(talent.pet_talent_offecttype + "%") + "概率 反震 " + Show_Color.Red(talent.pet_talent_offectvalue +"%") + " 伤害"; break;
        //            case 6: dec += "受到攻击时 " + Show_Color.Red(talent.pet_talent_offecttype + "%") + "概率 降低 " + Show_Color.Red(talent.pet_talent_offectvalue + "%") + " 暴击概率"; break;
        //            case 7: dec += "受到伤害时 " + Show_Color.Red(talent.pet_talent_offecttype + "%") + "概率 降低 " + Show_Color.Red(talent.pet_talent_offectvalue + "%") + " 伤害"; break;
        //            case 8: dec += "受到攻击时 " + Show_Color.Red(talent.pet_talent_offecttype + "%") + "概率 反弹 " + Show_Color.Red(talent.pet_talent_offectvalue + "%") + " 伤害"; break;
        //            case 10: dec += "技能释放消耗减少  " + Show_Color.Red(talent.pet_talent_offectvalue) + " %"; break;
        //        }
        //        break;
        //    default:
        //        break;
        //}
        //Alert.Show(talent.pet_talent_name, dec);
    }
}
