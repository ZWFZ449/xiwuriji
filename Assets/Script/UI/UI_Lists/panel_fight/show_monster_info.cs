using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class show_monster_info :Base_Mono
{

    /// <summary>
    /// 位置
    /// </summary>
    private Transform crt;
    /// <summary>
    /// 信息预制体
    /// </summary>
    public info_item info_item_prefabs;

    private Text base_name;

    private Dictionary<enum_attribute_list, info_item> info_item_dic = new Dictionary<enum_attribute_list, info_item>();

    private Button close;

    private void Awake()
    {
        close = Find<Button>("close");
        close.onClick.AddListener(() => { gameObject.SetActive(false); });
        crt =Find<Transform>("base_info/Scroll View/Viewport/Content");
        base_name=Find<Text>("info_name/name");
        for (int i = 0; i < Enum.GetNames(typeof(enum_attribute_list)).Length; i++)
        {
            info_item item = Instantiate(info_item_prefabs, crt);
            item.Show((enum_attribute_list)i, UnityEngine.Random.Range(1, 1000));
            info_item_dic.Add((enum_attribute_list)i, item);
        }
    }


    public void show_info(crtMaxHeroVO data)
    {
        string dec = "";

        if (data.Monster_Lv >= 1)
        {
            for (int i = 0; i < data.life.Length; i++)
            {
                if (data.life[i] != 0)
                {
                    dec += " " + Show_Color.Yellow((enum_skill_attribute_list)(201 + i) + "(" + data.life[i] + ")");
                }
            }
            if (data.monster_attrList.Count > 0)
            {
                dec += " " + (enum_monster_state)data.monster_attrList[0];
            }
            switch (data.Monster_Lv)
            {
                case 2:
                    dec += " " + Show_Color.Red("[精英级]");
                    break;
                case 3:
                    dec += " " + Show_Color.Red("【Boss级】");
                    break;
                default:
                    break;
            }
        }
        dec += " " + data.show_name;
        base_name.text= dec;
        foreach (var item in info_item_dic.Keys)
        {
            switch (item)
            {
                case enum_attribute_list.生命值:
                    info_item_dic[item].Show(item, data.MaxHP + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.法力值:
                    info_item_dic[item].Show(item, data.MaxMp + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.内力值:
                    info_item_dic[item].Show(item, data.internalforceMP + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.蓄力值:
                    info_item_dic[item].Show(item, data.EnergyMp + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.物理防御:
                    info_item_dic[item].Show(item, data.DefMin + " - " + data.DefMax + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.魔法防御:
                    info_item_dic[item].Show(item, data.MagicDefMin + " - " + data.MagicDefMax + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.物理攻击:
                    info_item_dic[item].Show(item, data.damageMin + " - " + data.damageMax + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.魔法攻击:
                    info_item_dic[item].Show(item, data.MagicdamageMin + " - " + data.MagicdamageMax + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.命中:
                    info_item_dic[item].Show(item, data.hit + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.躲避:
                    info_item_dic[item].Show(item, data.dodge + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.穿透:
                    info_item_dic[item].Show(item, data.penetrate + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.格挡:
                    info_item_dic[item].Show(item, data.block + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.暴击:
                    info_item_dic[item].Show(item, data.crit_rate + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.幸运:
                    info_item_dic[item].Show(item, data.Lucky + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.暴击伤害:
                    info_item_dic[item].Show(item, data.crit_damage + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.伤害加成:
                    info_item_dic[item].Show(item, data.double_damage + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.真实伤害:
                    info_item_dic[item].Show(item, data.Real_harm + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.伤害减免:
                    info_item_dic[item].Show(item, data.Damage_Reduction + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.伤害吸收:
                    info_item_dic[item].Show(item, data.Damage_absorption + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.异常抗性:
                    info_item_dic[item].Show(item, data.resistance + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.攻击速度:
                    info_item_dic[item].Show(item, (data.attack_speed / 60F).ToString("F2") + "s");
                    break;
                case enum_attribute_list.移动速度:
                    info_item_dic[item].Show(item, data.move_speed + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.生命加成:
                    info_item_dic[item].Show(item, data.bonus_Hp + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.法力加成:
                    info_item_dic[item].Show(item, data.bonus_Mp + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.生命回复:
                    info_item_dic[item].Show(item, data.Heal_Hp + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.法力回复:
                    info_item_dic[item].Show(item, data.Heal_Mp + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.物攻加成:
                    info_item_dic[item].Show(item, data.bonus_Damage + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.魔攻加成:
                    info_item_dic[item].Show(item, data.bonus_MagicDamage + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.物防加成:
                    info_item_dic[item].Show(item, data.bonus_Def + tool_Categoryt.Obtain_unit((int)item));
                    break;
                case enum_attribute_list.魔防加成:
                    info_item_dic[item].Show(item, data.bonus_MagicDef + tool_Categoryt.Obtain_unit((int)item));
                    break;
                default:
                    break;
            }
        }

    }

     
}
