using Common;
using MVC;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class panel_role_health : Base_Mono
{
    ///<summary>
    ///右上角健康栏Hp,Mp显示
    /// <summary>
    private Slider role_Hp, role_Mp, role_internalforceMP, role_EnergyMp, role_exp;

    private Text show_name, show_moeny, show_point;

    private Button btn_back;

    private BattleHealth health;
    /// <summary>
    /// 角色皮肤
    /// </summary>
    private string skin_state;
    /// <summary>
    /// 角色皮肤预制体
    /// </summary>
    private GameObject skin_prefabs;
    /// <summary>
    /// 角色头像
    /// </summary>
    private Transform pos_health;
    private void Awake()
    {
        role_Hp = Find<Slider>("panel_Hp");
        role_Mp = Find<Slider>("panel_Mp");
        role_exp = Find<Slider>("panel_exp");
        role_internalforceMP = Find<Slider>("panel_internalforceMP");
        role_EnergyMp = Find<Slider>("panel_EnergyMp");
        show_name = Find<Text>("role_name/info");
        btn_back= Find<Button>("role_name");
        btn_back.onClick.AddListener(() => {
            UI_Manager.I.TogglePanel(Panel_List.panel_Buff, true);
        });
        show_moeny = Find<Text>("show_unit/moeny/info");
        show_point = Find<Text>("show_unit/Point/info");
        pos_health = Find<Transform>("profile_picture");
        show_tianming_Platform= Find<Transform>("profile_picture/tianming_Platform");
        Instance_Skin();
    }
    public void SetHealth(BattleHealth _health)
    {
        health = _health;
        role_Hp.maxValue = health.maxHP;
        role_Mp.maxValue = health.maxMP;
        role_internalforceMP.maxValue = health.internalforcemaxMP;
        role_EnergyMp.maxValue = health.EnergymaxMp;
        show_name.text = SumSave.crt_MaxHero_okd.show_name;
        role_exp.maxValue=SumSave.db_lvs_old.hero_lv_list[SumSave.crt_MaxHero_okd.Lv];
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
    private void Update()
    {
        if (health != null)
        {
            if (SumSave.old_crt_hero.hero_pos != skin_state)
            {
                Instance_Skin();
            }

            if (tianming_Platform == null || !tianming_Platform.SequenceEqual(SumSave.old_crt_hero.tianming_Platform))
            {
                Show_Info_life();
            }


            show_name.text = SumSave.old_crt_hero.hero_name + " Lv." + SumSave.old_crt_hero.hero_Lv +
               "(" + SumSave.old_crt_hero.hero_Exp * 100 / SumSave.db_lvs_old.hero_lv_list[SumSave.old_crt_hero.hero_Lv] + "%)";
            role_exp.maxValue = SumSave.db_lvs_old.hero_lv_list[SumSave.old_crt_hero.hero_Lv];
            role_exp.value = SumSave.old_crt_hero.hero_Exp;
            if (SumSave.old_crt_hero.hero_Exp >= SumSave.db_lvs_old.hero_lv_list[SumSave.old_crt_hero.hero_Lv])
            {
                SumSave.crt_MaxHero_okd.Lv++;
                SumSave.crt_MaxHero_okd.Exp -= SumSave.db_lvs_old.hero_lv_list[SumSave.old_crt_hero.hero_Lv];
                SumSave.old_crt_hero.hero_Exp -= SumSave.db_lvs_old.hero_lv_list[SumSave.old_crt_hero.hero_Lv];
                SumSave.old_crt_hero.hero_Lv++;
                role_exp.maxValue = SumSave.db_lvs_old.hero_lv_list[SumSave.crt_MaxHero_okd.Lv];
                Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_hero,
                    SumSave.old_crt_hero.Set_Uptade_String(), SumSave.old_crt_hero.Get_Update_Character());
                //刷新数据
                SendNotification(NotiList.Refresh_Max_Hero_Attribute);
            }
            LevelTask();
            role_Hp.value = health.HP;
            role_Mp.value = health.MP;
            role_internalforceMP.value = health.internalforceMP;
            role_EnergyMp.value = health.EnergyMp;
            List<long> list = SumSave.crt_user_unit.Set();
            show_moeny.text = Battle_Tool.FormatNumberToChineseUnit(list[0]) + " " + currency_unit.金币;
            show_point.text = Battle_Tool.FormatNumberToChineseUnit(list[1]) + " " + currency_unit.元宝;
        }
        ObtainEquipmentTasks();
    }

    /// <summary>
    /// 获得装备任务
    /// </summary>
    private static void ObtainEquipmentTasks()
    {

    }


    /// <summary>
    /// 等级任务
    /// </summary>
    private static void LevelTask()
    {
        if (SumSave.old_crt_hero.hero_Lv >= 10)
        {
            tool_Categoryt.Base_Task(1024);
        }
        if (SumSave.old_crt_hero.hero_Lv >= 15)
        {
            tool_Categoryt.Base_Task(1036);
        }
        if (SumSave.old_crt_hero.hero_Lv >= 20)
        {
            tool_Categoryt.Base_Task(1040);
        }
        if (SumSave.old_crt_hero.hero_Lv >= 30)
        {
            tool_Categoryt.Base_Task(1062);
        }
        if (SumSave.old_crt_hero.hero_Lv >= 40)
        {
            tool_Categoryt.Base_Task(1084);
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
    /// 显示天命光环
    /// </summary>
    private void Show_Info_life()
    {

        tianming_Platform = (int[])SumSave.old_crt_hero.tianming_Platform.Clone();

        for (int i = show_tianming_Platform.childCount - 1; i >= 0; i--)//清空区域内按钮
        {
            Destroy(show_tianming_Platform.GetChild(i).gameObject);
        }
        pos_tianming_size = show_tianming_Platform.GetComponent<RectTransform>().rect.size;

        tianming_num =new Dictionary<int, int>();
        tianming_num = Battle_Tool.Get_Life_Type();
        //for (int i = 0; i < SumSave.crt_hero.tianming_Platform.Length; i++)
        //{
        //    if (tianming_num.ContainsKey(SumSave.crt_hero.tianming_Platform[i]))
        //    {
        //        tianming_num[SumSave.crt_hero.tianming_Platform[i]]++;
        //    }
        //    else
        //    {
        //        tianming_num.Add(SumSave.crt_hero.tianming_Platform[i], 1);
        //    }
        //}
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

}
