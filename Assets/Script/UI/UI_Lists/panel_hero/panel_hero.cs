using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVC;
using UI;
using System;
using Common;
using UnityEngine.UI;
using Components;
using System.Linq;

public class panel_hero : Panel_Base
{
    /// <summary>
    /// 位置
    /// </summary>
    private Transform crt,crt_pos_hero; 
    /// <summary>
    /// 信息预制体
    /// </summary>
    public info_item info_item_prefabs;
    /// <summary>
    /// 职业预制件
    /// </summary>
    private hero_item hero_item_prefabs;
    /// <summary>
    /// 开启切换角色
    /// </summary>
    private Button open_activation;
    /// <summary>
    /// 选择职业
    /// </summary>
    private hero_item crt_hero;
    /// <summary>
    /// 选择角色面板
    /// </summary>
    private Image select_hero;
    /// <summary>
    /// 选择角色面板关闭按钮
    /// </summary>
    private Button select_hero_close_btn;

    private Dictionary<enum_attribute_list, info_item> info_item_dic = new Dictionary<enum_attribute_list, info_item>();

    private Dictionary<string,hero_item> hero_item_dic = new Dictionary<string, hero_item>();
    /// <summary>
    /// 显示英雄面板
    /// </summary>
    private Image show_hero_info;
    /// <summary>
    /// 显示英雄信息
    /// </summary>
    private Text hero_info, hero_name;
    /// <summary>
    /// 英雄列表
    /// </summary>
    private Button crate_btn,show_hero_info_close_btn;
   
    /// <summary>
    /// 角色皮肤预制体
    /// </summary>
    private GameObject skin_prefabs;
    /// <summary>
    /// 角色within位置
    /// </summary>
    private Transform panel_role_health;
    /// <summary>
    /// 装备类型
    /// </summary>
    private Dictionary<EquipTypeList, show_equip_item> dic_equips = new Dictionary<EquipTypeList, show_equip_item>();
    /// <summary>
    /// 显示装备
    /// </summary>
    private panel_equip panel_equip;

    protected override void Awake()
    {
        base.Awake();
    }
    public override void Initialize()
    {
        base.Initialize();
        crt = Find<Transform>("bg_main/base_info/Scroll View/Viewport/Content");
        crt_pos_hero = Find<Transform>("bg_main/select_hero/Scroll View/Viewport/Content");
        open_activation = Find<Button>("bg_main/bag_equips/hero_icon/open_activation");
        open_activation.onClick.AddListener(() => { select_hero.gameObject.SetActive(true); });
        hero_item_prefabs = Battle_Tool.Find_Prefabs<hero_item>("hero_item");// Resources.Load<hero_item>("Prefabs/panel_hero/hero_item");
        select_hero=Find<Image>("bg_main/select_hero");
        select_hero_close_btn = Find<Button>("bg_main/select_hero/close");
        select_hero_close_btn.onClick.AddListener(() => { select_hero.gameObject.SetActive(false); });
        show_hero_info=Find<Image>("bg_main/select_hero/show_hero_info");
        hero_info =Find<Text>("bg_main/select_hero/show_hero_info/Scroll View/Viewport/info");
        hero_name=Find<Text>("bg_main/select_hero/show_hero_info/hero_name/info");
        show_hero_info_close_btn = Find<Button>("bg_main/select_hero/show_hero_info/close");
        show_hero_info_close_btn.onClick.AddListener(() => { show_hero_info.gameObject.SetActive(false); });
        crate_btn = Find<Button>("bg_main/select_hero/show_hero_info/crate_btn");
        crate_btn.onClick.AddListener(() => { SwitchRoles(); });
        panel_role_health = Find<Transform>("bg_main/bag_equips/hero_icon/panel_role_health");
        panel_equip = UI_Manager.I.GetPanel<panel_equip>();
        show_tianming_Platform= Find<Transform>("bg_main/bag_equips/hero_icon/tianming_Platform");

        CharacterRefresh();



        for (int i = 0; i < Enum.GetNames(typeof(enum_attribute_list)).Length; i++)
        { 
            info_item item = Instantiate(info_item_prefabs, crt);
            item.Show((enum_attribute_list)i, UnityEngine.Random.Range(1, 1000));
            info_item_dic.Add((enum_attribute_list)i, item);
        }
        for (int i = 0; i < SumSave.old_db_heros.Count; i++)
        {
            hero_item item = Instantiate(hero_item_prefabs, crt_pos_hero);
            item.Data = SumSave.old_db_heros[i];
            item.GetComponent<Button>().onClick.AddListener(delegate { Select_Hero(item); });
            if(!hero_item_dic.ContainsKey(SumSave.old_db_heros[i].hero_name))
                hero_item_dic.Add(SumSave.old_db_heros[i].hero_name, item);
        }
        select_hero.gameObject.SetActive(false);
    }

    /// <summary>
    /// 点击切换角色
    /// </summary>
    private void SwitchRoles()
    {
        string[] herolists = SumSave.old_crt_hero.hero_value.Split(',');
        foreach (var item in herolists)
        {
            //if (crt_hero.SetData().hero_name == item)
            //{
            //    switch_Hero(item);
            //    return;
            //}

            string[] hero = item.Split('|');

            if (crt_hero.SetData().hero_name == hero[0])
            {
                SumSave.old_crt_hero.Merge_hero_value();

                if (hero.Length<2)
                {
                    SumSave.old_crt_hero.Uptianming_Platform();
                    switch_Hero(hero[0]);
                }
                else
                {
                    string[] tianming = hero[1].Split(' ');
                    SumSave.old_crt_hero.tianming_Platform= new int[5];
                    for (int i = 0; i < tianming.Length; i++)
                    {
                        SumSave.old_crt_hero.tianming_Platform[i] = int.Parse(tianming[i]);
                    }
                    switch_Hero(hero[0]);
                }
                return;
            }
        }

        if (Tool_State.IsState(State_List.至尊卡))
        {
            if (crt_hero.SetData().hero_name == "昭月"|| crt_hero.SetData().hero_name == "琉璃")
            {
                SumSave.old_crt_hero.Merge_hero_value();
                switch_Hero(crt_hero.SetData().hero_name);
                SumSave.old_crt_hero.AddSkin(crt_hero.SetData().hero_name);
                return;
           }
        }
        Alert_Dec.Show("该角色尚未获取");
    }

    private void switch_Hero(string item)
    {
       
        SumSave.old_crt_hero.hero_pos = item;
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_hero,
            SumSave.old_crt_hero.Set_Uptade_String(), SumSave.old_crt_hero.Get_Update_Character());
        Alert_Dec.Show("切换角色 " + item + " 成功");

        SendNotification(NotiList.Refresh_Max_Hero_Attribute);
        base_show();
        CharacterRefresh();
    }
    /// <summary>
    /// 角色刷新
    /// </summary>
    private void CharacterRefresh()
    {
        skin_prefabs = Resources.Load<GameObject>("Prefabs/Skins/within_" + SumSave.old_crt_hero.hero_pos);
        ClearObject(panel_role_health);
        Instantiate(skin_prefabs, panel_role_health);
    }


    /// <summary>
    /// 选择角色
    /// </summary>
    /// <param name="item"></param>
    private void Select_Hero(hero_item item)
    {
        crt_hero = item;
        show_hero_info.gameObject.SetActive(true);
        Show_Hero_Type_Attribute(crt_hero);

    }
    private void Show_Hero_Type_Attribute(hero_item item)
    {
        hero_name.text= item.Data.hero_name;
        string dec = "伤害类型:" + (item.Data.hero_type == 1 ? "物理伤害" : "魔法伤害") + "\n";
        for (int i = 0; i < item.Data.crate_value.Length; i++)
        {
            dec += (enum_attribute_list)i + ":" + item.Data.crate_value[i] + "(每" + item.Data.up_base_value[i] + "级 + " + item.Data.up_value[i] + ")\n";
        }
        hero_info.text = dec;
    }


 
    /// <summary>
    /// 初始化位置
    /// </summary>
    /// <param name="item"></param>
    protected void Instance_Pos(show_equip_item item)
    {
        if (!dic_equips.ContainsKey(item.type)) dic_equips.Add(item.type, item);
    }

    /// <summary>
    /// 显示穿戴装备列表
    /// </summary>
    private void Base_Show()
    {
        foreach (var item in dic_equips.Keys)
        {
            dic_equips[item].Init();

            //foreach (Bag_Base_VO equip in SumSave.crt_euqip)
            //{
            //    if (equip.StdMode == item.ToString())
            //    {
            //        dic_equips[item].Data = equip;
            //    }
            //}
        }
    }
    #region 显示天命光环
    /// <summary>
    /// 天命台
    /// </summary>
    private int[] tianming_Platform;
    /// <summary>
    /// 天命台位置
    /// </summary>
    private Transform show_tianming_Platform;
    /// <summary>
    /// 天命台父物体大小,当前天命大小
    /// </summary>
    private Vector2 pos_tianming_size, tianming_size;
    /// <summary>
    /// 缩放比例
    /// </summary>
    private float scaling = 1;
    /// <summary>
    /// 每个天命的数量
    /// </summary>
    private Dictionary<int, int> tianming_num;

    /// <summary>
    /// 显示五行光环
    /// </summary>
    private void Show_Info_life()
    {

        tianming_Platform = (int[])SumSave.old_crt_hero.tianming_Platform.Clone();

        for (int i = show_tianming_Platform.childCount - 1; i >= 0; i--)//清空区域内按钮
        {
            Destroy(show_tianming_Platform.GetChild(i).gameObject);
        }
        pos_tianming_size = show_tianming_Platform.GetComponent<RectTransform>().rect.size;

        tianming_num = new Dictionary<int, int>();



        for (int i = 0; i < SumSave.old_crt_hero.tianming_Platform.Length; i++)
        {
            if (tianming_num.ContainsKey(SumSave.old_crt_hero.tianming_Platform[i]))
            {
                tianming_num[SumSave.old_crt_hero.tianming_Platform[i]]++;
            }
            else
            {
                tianming_num.Add(SumSave.old_crt_hero.tianming_Platform[i], 1);
            }
        }


        for (int i = 0; i < SumSave.old_crt_hero.tianming_Platform.Length; i++)
        {
            GameObject game = Resources.Load<GameObject>("Prefabs/halo/halo_" + (SumSave.old_crt_hero.tianming_Platform[i] + 1));
            GameObject tianming = Instantiate(game, show_tianming_Platform);

            tianming.transform.Rotate(new Vector3(0, 0, 15 * i));


            tianming_size = new Vector2(pos_tianming_size.x * scaling, pos_tianming_size.y * scaling);
            tianming.GetComponent<RectTransform>().sizeDelta = tianming_size;

            Color currentColor = tianming.GetComponentInChildren<Image>().color;
            currentColor.a = tianming_num[SumSave.old_crt_hero.tianming_Platform[i]] * 0.2f;
            tianming.GetComponentInChildren<Image>().color = currentColor;
        }
    }

    #endregion



    public override void Show()
    {
        base.Show();
        base_show();
        Base_Show();

        if (tianming_Platform == null || !tianming_Platform.SequenceEqual(SumSave.old_crt_hero.tianming_Platform))
        {
            Show_Info_life();
        }
    }
    /// <summary>
    /// 显示属性表
    /// </summary>
    private void base_show()
    {
        foreach (var item in info_item_dic.Keys)
        {
            switch (item)
            {
                case enum_attribute_list.生命值:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.MaxHP+tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.法力值:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.MaxMp+tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.内力值:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.internalforceMP+tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.蓄力值:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.EnergyMp+tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.物理防御:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.DefMin+" - "+SumSave.crt_MaxHero_okd.DefMax+tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.魔法防御:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.MagicDefMin + " - " + SumSave.crt_MaxHero_okd.MagicDefMax+tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.物理攻击:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.damageMin + " - " + SumSave.crt_MaxHero_okd.damageMax+tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.魔法攻击:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.MagicdamageMin + " - " + SumSave.crt_MaxHero_okd.MagicdamageMax+tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.命中:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.hit+tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.躲避:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.dodge+tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.穿透:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.penetrate+tool_Categoryt.Obtain_unit((int)item)); 
                    break;
                case enum_attribute_list.格挡:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.block+tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.暴击:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.crit_rate+tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.幸运:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.Lucky+tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.暴击伤害:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.crit_damage+tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.伤害加成:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.double_damage+tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.真实伤害:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.Real_harm+tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.伤害减免:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.Damage_Reduction+tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.伤害吸收:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.Damage_absorption+tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.异常抗性:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.resistance+tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.攻击速度:
                    info_item_dic[item].Show(item, (SumSave.crt_MaxHero_okd.attack_speed / 60F).ToString("F2") + "s");
                    break;
                case enum_attribute_list.移动速度:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.move_speed+tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.生命加成:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.bonus_Hp+tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.法力加成:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.bonus_Mp+tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.生命回复:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.Heal_Hp+tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.法力回复:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.Heal_Mp+tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.物攻加成: 
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.bonus_Damage+tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.魔攻加成:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.bonus_MagicDamage+tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.物防加成:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.bonus_Def+tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.魔防加成:
                    info_item_dic[item].Show(item, SumSave.crt_MaxHero_okd.bonus_MagicDef+tool_Categoryt.Obtain_unit((int)item));
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// 显示自身装备
    /// </summary>
    /// <param name="item"></param>
    protected void Select_Equip(bag_item item)
    {
        panel_equip.Show();
        panel_equip.Select_Equip(item);

    }
}
