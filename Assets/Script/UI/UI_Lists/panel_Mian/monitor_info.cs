using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class monitor_info : Base_Mono
{
    ///<summary>
    ///右上角健康栏Hp,Mp显示
    /// <summary>
    private Slider role_exp;

    private Text show_name, show_moeny, show_point, show_diamond, show_exp;

    private BattleHealth health;
    /// <summary>
    /// 角色皮肤
    /// </summary>
    private string skin_state;
    /// <summary>
    /// 天命台
    /// </summary>
    private int[] tianming_Platform;
    /// <summary>
    /// 角色皮肤预制体
    /// </summary>
    private GameObject skin_prefabs;
    /// <summary>
    /// 角色头像
    /// </summary>
    private Transform pos_health, show_tianming_Platform;
    /// <summary>
    /// 天命台父物体大小，当前天命台大小
    /// </summary>
    private Vector2 pos_tianming_size, tianming_size;
    /// <summary>
    /// 天命属性缩放比例
    /// </summary>
    private float scaling=1f;
    /// <summary>
    /// 每个天命光环的数量
    /// </summary>
    private Dictionary<int, int> tianming_num;

    /// <summary>
    /// 任务按钮
    /// </summary>
    private Button btn_base_task;
    /// <summary>
    /// 名称按钮
    /// </summary>
    private Button btn_back;
    /// <summary>
    /// 任务信息
    /// </summary>
    private Text show_task;



    private void Awake()
    {
        role_exp = Find<Slider>("base_info/Slider");
        show_exp = Find<Text>("base_info/Slider/exp");
        show_name = Find<Text>("base_info/role_name/info");
        show_moeny = Find<Text>("show_unit/moeny/info");
        show_point = Find<Text>("show_unit/Point/info");
        show_diamond = Find<Text>("show_unit/diamond/info");
        pos_health = Find<Transform>("base_info/profile_picture");
        show_tianming_Platform = Find<Transform>("base_info/profile_picture/tianming_Platform");
        btn_base_task = Find<Button>("base_task");
        show_task = Find<Text>("base_task/task_info");
        btn_base_task.onClick.AddListener(Show_GreenhandGuide);
        btn_back = Find<Button>("base_info/role_name");
        btn_back.onClick.AddListener(() => {
            UI_Manager.I.TogglePanel(Panel_List.panel_Buff, true);
        });
        Instance_Skin();
    }
    /// <summary>
    /// 领取任务奖励
    /// </summary>
    private void Show_GreenhandGuide()
    {
        if (SumSave.GreenhandGuide_TotalTasks.ContainsKey(SumSave.crt_greenhand.crt_task))
        {
            GreenhandGuide_TotalTaskVO task = SumSave.GreenhandGuide_TotalTasks[SumSave.crt_greenhand.crt_task];
            string dec = "";
            dec += task.task_dec_value +
              Show_Color.Green("\n进度(" + SumSave.crt_greenhand.crt_progress + "/" + task.progress + ")");
            Alert.Show(task.TaskDesc, dec, Claim_Rewards);
        }
        else Alert_Dec.Show("任务已完成");
    }
    /// <summary>
    /// 领取奖励
    /// </summary>
    /// <param name="arg0"></param>
    private void Claim_Rewards(object arg0)
    {
        if (SumSave.GreenhandGuide_TotalTasks.ContainsKey(SumSave.crt_greenhand.crt_task))
        {
            string dec = "";
            GreenhandGuide_TotalTaskVO task = SumSave.GreenhandGuide_TotalTasks[SumSave.crt_greenhand.crt_task];
            if (SumSave.crt_greenhand.crt_progress >= task.progress)
            {
                foreach (var item in SumSave.crt_greenhand.task_list)
                {
                    if (item == task.taskid)
                    {
                        Alert_Dec.Show("任务已完成");
                        return;
                    }
                }
                SumSave.crt_greenhand.task_list.Add(task.taskid);
                bool exist = false;
                foreach (var item in SumSave.GreenhandGuide_TotalTasks.Keys)
                {
                    //获取下一个进度条列表
                    if (exist)
                    {
                        SumSave.crt_greenhand.crt_task = item;
                        SumSave.crt_greenhand.crt_progress = 0;
                        exist = false;
                        break;
                    } else
                    if (SumSave.crt_greenhand.crt_task == item)
                    {
                        exist = true;
                    }
                }
                Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_greenhandguide,
                    SumSave.crt_greenhand.Set_Uptade_String(), SumSave.crt_greenhand.Get_Update_Character());
                for (int i = 0; i < task.Award.Length; i++)
                {
                    dec += "获得 " + Show_Color.Green(task.Award[i] + ":" + task.AwardNumber[i] + "\n");
                    switch ((GreenhandGuide_Enum_List)Enum.Parse(typeof(GreenhandGuide_Enum_List), task.AwardType[i]))
                    {
                        case GreenhandGuide_Enum_List.武器:
                        case GreenhandGuide_Enum_List.装备:
                            //SumSave.crt_bag.Add(tool_Categoryt.crate_equip(task.Award[i], task.AwardNumber[i]));
                            //Game_Omphalos.i.Wirte_ResourcesList(Emun_Resources_List.bag_value, SumSave.crt_bag);
                            break;
                        case GreenhandGuide_Enum_List.物品:
                        case GreenhandGuide_Enum_List.材料:
                        case GreenhandGuide_Enum_List.技能:
                        case GreenhandGuide_Enum_List.战斗技能:
                        case GreenhandGuide_Enum_List.奇物:
                            int random = Random.Range(1, 100);
                            int number = task.AwardNumber[i];
                            int maxnumber = number + Random.Range(1, 100);
                            Battle_Tool.Obtain_Resources(Obtain_Int.Add(1, task.Award[i], new int[] { number + random, random }), maxnumber);
                            //Battle_Tool.Obtain_Resources(task.Award[i], task.AwardNumber[i]);
                            break;
                        case GreenhandGuide_Enum_List.经验:
                            Battle_Tool.Obtain_Exp(task.AwardNumber[i]);
                            break;
                        case GreenhandGuide_Enum_List.灵珠:
                            Battle_Tool.Obtain_Unit(currency_unit.金币, task.AwardNumber[i]);
                            break;
                        case GreenhandGuide_Enum_List.宠物:
                            Battle_Tool.Obtain_Pet(task.Award[i], task.AwardNumber[i]);
                            break;
                        case GreenhandGuide_Enum_List.荣耀点:
                            SumSave.crt_accumulatedrewards.Set(2, task.AwardNumber[i]);
                            break;
                        default:
                            break;
                    }
                }
                base_info_Task();
                Alert.Show(task.TaskDesc, dec);
            }
            else Alert_Dec.Show("任务未完成");
        }
    }
    /// <summary>
    /// 初始化角色皮肤
    /// </summary>
    private void Instance_Skin()
    {
        for (int i = pos_health.childCount - 1; i >= 1; i--)//清空区域内按钮
        {
            Destroy(pos_health.GetChild(i).gameObject);
        }
        skin_prefabs = Resources.Load<GameObject>("Prefabs/Skins/within_" + SumSave.old_crt_hero.hero_pos);
        Instantiate(skin_prefabs, pos_health);
        skin_state = SumSave.old_crt_hero.hero_pos;
    }
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
        private void Update()
        {
            if (SumSave.crt_MaxHero_okd != null)
            {
                if (SumSave.old_crt_hero.hero_pos != skin_state)
                {
                    Instance_Skin();
                }
                if (tianming_Platform == null ||!tianming_Platform.SequenceEqual(SumSave.old_crt_hero.tianming_Platform))
                {
                    Show_Info_life();
                }
                show_name.text = SumSave.old_crt_hero.hero_name;
                show_exp.text = " Lv." + SumSave.old_crt_hero.hero_Lv +
                   "(" + SumSave.old_crt_hero.hero_Exp * 100 / SumSave.db_lvs_old.hero_lv_list[SumSave.old_crt_hero.hero_Lv] + "%)";
                role_exp.maxValue = SumSave.db_lvs_old.hero_lv_list[SumSave.old_crt_hero.hero_Lv];
                role_exp.value = SumSave.old_crt_hero.hero_Exp;
                List<long> list = SumSave.crt_user_unit.Set();
                show_moeny.text = Battle_Tool.FormatNumberToChineseUnit(list[0]) + " " + currency_unit.金币;
                show_point.text = Battle_Tool.FormatNumberToChineseUnit(list[1]) + " " + currency_unit.元宝;
                show_diamond.text = Battle_Tool.FormatNumberToChineseUnit(list[2]) + " " + currency_unit.Boss积分;
                base_info_Task();
            }
        }
        /// <summary>
        /// 显示任务信息
        /// </summary>
        private void base_info_Task()
        {
            string dec = "";
            if (SumSave.GreenhandGuide_TotalTasks.ContainsKey(SumSave.crt_greenhand.crt_task))
            {
                GreenhandGuide_TotalTaskVO task = SumSave.GreenhandGuide_TotalTasks[SumSave.crt_greenhand.crt_task];
                dec = task.TaskDesc + "(" + SumSave.crt_greenhand.crt_progress + "/" + task.progress + ")";
            }
            show_task.text = dec;
        }



}
