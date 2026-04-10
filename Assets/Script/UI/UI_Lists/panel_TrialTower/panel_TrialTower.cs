using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class panel_TrialTower : Panel_Base
{
    /// <summary>
    /// 世界boss图标
    /// </summary>
    private Image icon;
    /// <summary>
    /// 进入调整按钮
    /// </summary>
    private Button up_map;
    /// <summary>
    /// 战斗地图
    /// </summary>
    private panel_fight fight_panel;
    /// <summary>
    /// 当前选择地图
    /// </summary>
    private user_map_vo crt_map;
    /// <summary>
    /// 信息预制体
    /// </summary>
    public info_item info_item_prefabs;
    /// <summary>
    /// 属性预制体字典
    /// </summary>
    private Dictionary<enum_attribute_list, info_item> info_item_dic = new Dictionary<enum_attribute_list, info_item>();
    /// <summary>
    /// 属性显示位置
    /// </summary>
    private Transform crt;
    /// <summary>
    /// boss挑战进度条
    /// </summary>
    private Slider progress;
    /// <summary>
    /// 当前世界Boss
    /// </summary>
    private crtMaxHeroVO monster = new crtMaxHeroVO();
    /// <summary>
    /// 血量显示，次数显示
    /// </summary>
    private Text Hp_Text, numberText;
    /// <summary>
    /// 当前世界Boss挑战次数，最大挑战次数
    /// </summary>
    private  int boss_number=0,boss_number_max = 3;


    public override void Initialize()
    {
        base.Initialize();
        fight_panel = UI_Manager.I.GetPanel<panel_fight>();
        icon = Find<Image>("boss_icon");
        up_map = Find<Button>("up_map");
        up_map.onClick.AddListener(Set_Map);
        crt = Find<Transform>("information/Viewport/Content");
        info_item_prefabs = Battle_Tool.Find_Prefabs<info_item>("info_item");
        progress= Find<Slider>("progress");
        Hp_Text= Find<Text>("progress/Hp_Text");
        numberText = Find<Text>("up_map/number");
        for (int i = 0; i < SumSave.read_lose_map.Count; i++)
        {
            if (SumSave.read_lose_map[i].map_type == 5 && SumSave.read_lose_map[i].need_lv == 1)
            {
                crt_map = SumSave.read_lose_map[i];
            }
        }

        if(crt_map==null)
        {
            Debug.Log("没有世界boss");
            return;
        }

        List<(string, int)> list = SumSave.crt_needlist.SetMap();
        boss_number = boss_number_max;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Item1 == crt_map.map_name)
            {
                boss_number = list[i].Item2;
            }
        }

        for (int i = 0; i < SumSave.olddb_monsters.Count; i++)
        {
            if (crt_map.monster_list == SumSave.olddb_monsters[i].show_name)
            {
                monster = SumSave.olddb_monsters[i];
            }
        }

        ClearObject(crt);
        for (int i = 0; i < Enum.GetNames(typeof(enum_attribute_list)).Length; i++)
        {
            info_item item = Instantiate(info_item_prefabs, crt);
            item.Show((enum_attribute_list)i, UnityEngine.Random.Range(1, 1000));
            info_item_dic.Add((enum_attribute_list)i, item);
        }

        Init();
    }
    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        if (crt_map == null)
        {
            ClearObject(crt);
            Hp_Text.text = "世界Boss暂时未开启";
            return;
        }
        //需要获取实时怪物血量 SendNotification(NotiList.Read_Crate_world_boss_Login);


        SendNotification(NotiList.Read_Crate_world_boss_Login, crt_map.monster_list);
        icon.sprite = Resources.Load<Sprite>("Prefabs/monsters/" + crt_map.monster_list);
        progress.maxValue = monster.MaxHP;
        numberText.text ="挑战次数:"+ boss_number.ToString()+"/3";
        progress.value = SumSave.db_world_boos.Get();
        if (monster.MaxHP - SumSave.db_world_boos.Get() > 0)
        {
            Hp_Text.text = (SumSave.db_world_boos.Get()).ToString() + "/" + monster.MaxHP.ToString();
        }else
        {
            Hp_Text.text= "世界Boss已挑战完成";
        }

    }

    public override void Hide()
    {
        base.Hide();
    }

  
    public override void Show()
    {
        base.Show();
        Init();
        base_show();
    }
    /// <summary>
    /// 开始挑战世界boss
    /// </summary>
    private void Set_Map()
    {
        if (crt_map == null)
        {
            Alert_Dec.Show("世界boss暂为开启，请等待挑战");
            return;
        }
        if(boss_number>=boss_number_max)
        {
            Alert_Dec.Show("今日挑战次数已用完");
            return;
        }

        IncreaseFrequency();

        Init();
        if (monster.MaxHP - SumSave.db_world_boos.Get() > 0)
        {
            SendNotification(NotiList.Read_Crate_world_boss_Login, crt_map.monster_list);
            fight_panel.Show();
            fight_panel.Open_Map(crt_map);
            Hide();
        }
        else
        {

            Alert_Dec.Show("世界boss已死，请等待下一轮挑战");
        }

    }
    /// <summary>
    /// 挑战次数++
    /// </summary>
    private void IncreaseFrequency()
    {
        bool exist = true;
        List<(string, int)> list = SumSave.crt_needlist.SetMap();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Item1 == crt_map.map_name)
            {
                exist = false;
                list[i] = (list[i].Item1, list[i].Item2 + 1);
                SumSave.crt_needlist.SetMap(list[i]);
                break;
            }
        }
        if (exist)
        {
            SumSave.crt_needlist.SetMap((crt_map.map_name, 1));

        }
        boss_number++;
        
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
                    info_item_dic[item].Show(item, monster.MaxHP + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.法力值:
                    info_item_dic[item].Show(item, monster.MaxMp + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.内力值:
                    info_item_dic[item].Show(item, monster.internalforceMP + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.蓄力值:
                    info_item_dic[item].Show(item, monster.EnergyMp + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.物理防御:
                    info_item_dic[item].Show(item, monster.DefMin + " \n- " + monster.DefMax + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.魔法防御:
                    info_item_dic[item].Show(item, monster.MagicDefMin + " \n- " + monster.MagicDefMax + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.物理攻击:
                    info_item_dic[item].Show(item, monster.damageMin + " \n- " + monster.damageMax + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.魔法攻击:
                    info_item_dic[item].Show(item, monster.MagicdamageMin + " \n- " + monster.MagicdamageMax + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.命中:
                    info_item_dic[item].Show(item, monster.hit + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.躲避:
                    info_item_dic[item].Show(item, monster.dodge + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.穿透:
                    info_item_dic[item].Show(item, monster.penetrate + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.格挡:
                    info_item_dic[item].Show(item, monster.block + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.暴击:
                    info_item_dic[item].Show(item, monster.crit_rate + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.幸运:
                    info_item_dic[item].Show(item, monster.Lucky + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.暴击伤害:
                    info_item_dic[item].Show(item, monster.crit_damage + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.伤害加成:
                    info_item_dic[item].Show(item, monster.double_damage + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.真实伤害:
                    info_item_dic[item].Show(item, monster.Real_harm + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.伤害减免:
                    info_item_dic[item].Show(item, monster.Damage_Reduction + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.伤害吸收:
                    info_item_dic[item].Show(item, monster.Damage_absorption + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.异常抗性:
                    info_item_dic[item].Show(item, monster.resistance + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.攻击速度:
                    info_item_dic[item].Show(item, (monster.attack_speed / 60F).ToString("F2") + "s");
                    break;
                case enum_attribute_list.移动速度:
                    info_item_dic[item].Show(item, monster.move_speed + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.生命加成:
                    info_item_dic[item].Show(item, monster.bonus_Hp + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.法力加成:
                    info_item_dic[item].Show(item, monster.bonus_Mp + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.生命回复:
                    info_item_dic[item].Show(item, monster.Heal_Hp + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.法力回复:
                    info_item_dic[item].Show(item, monster.Heal_Mp + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.物攻加成:
                    info_item_dic[item].Show(item, monster.bonus_Damage + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.魔攻加成:
                    info_item_dic[item].Show(item, monster.bonus_MagicDamage + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.物防加成:
                    info_item_dic[item].Show(item, monster.bonus_Def + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.魔防加成:
                    info_item_dic[item].Show(item, monster.bonus_MagicDef + tool_Categoryt.Obtain_unit((int)item));
                    break;
                default:
                    break;
            }
        }
    }
}
