using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PanelBattle : PanelBase
{
    /// <summary>
    /// 出生点
    /// </summary>
    private Transform battle_borm;
    /// <summary>
    /// 药品生成点
    /// </summary>
    private Transform m_medicine_borm;
    /// <summary>
    /// 位置预制体
    /// </summary>
    private Transform borm_prefab,m_skill_borm;
    /// <summary>
    /// 怪物预制体
    /// </summary>
    private GameObject battle_monster_prefab,battle_Boss_monster_prefab;
    /// <summary>
    /// 玩家预制体
    /// </summary>
    private GameObject battle_player_prefab;
    /// <summary>
    /// 药品预制体
    /// </summary>
    private medicineitem Medicineitem_prefab;
    /// <summary>
    /// 怪物列表
    /// </summary>
    private List<GameObject> monster_list;
    /// <summary>
    /// 玩家列表
    /// </summary>
    private List<GameObject> player_list;

    /// <summary>
    /// 怪物战斗列表
    /// </summary>
    private List<crtMaxBattleVO> monster_battle_list;
    /// <summary>
    /// 怪物boss战斗列表
    /// </summary>
    private List<crtMaxBattleVO> monster_Bossbattle_list;

    /// <summary>
    /// 初始化药品
    /// </summary>
    private Dictionary<medicineType,medicineitem> medicine_list;
    /// <summary>
    /// 当前地图
    /// </summary>
    private db_map_vo crt_map;

    private skill_offect_item skill_offect_item_prefab;

    private show_drop_list show_drop_list;
    /// <summary>
    /// 刷新boss前置条件
    /// </summary>
    private int map_crate_boss_condition = 0;
    /// <summary>
    /// 击杀总数量
    /// </summary>
    private int kill_monster = 0;
    /// <summary>
    /// 限时地图f
    /// </summary>
    private float limited_time = -1f;
    /// <summary>
    /// 刷新boss监控
    /// </summary>
    private Boss_Slider boss_slider;
    /// <summary>
    /// 刷新间隔
    /// </summary>
    private TMP_Text time_info,map_name;
    /// <summary>
    /// 信息预制体
    /// </summary>
    private show_info_item show_info_item_prefab;
    /// <summary>
    /// 信息框
    /// </summary>
    private Transform show_info_borm;
    /// <summary>
    /// 初始化怪物刷新
    /// </summary>
    public override void Hide()
    {
        this.transform.SetAsFirstSibling();
    }
    public override void Initialize()
    {
        base.Initialize();
        time_info = Find<TMP_Text>("Title/time_info/info");
        map_name= Find<TMP_Text>("Title/map_name/info");
        monster_list = new List<GameObject>();
        player_list = new List<GameObject>();
        monster_battle_list = new List<crtMaxBattleVO>();
        monster_Bossbattle_list = new List<crtMaxBattleVO>();
        //获取初始化部位
        battle_borm = Find<Transform>("BaseMap/Viewport/Content");
        borm_prefab =  Tool_UI.Find_Prefabs<Transform>("Transform_item");
        battle_monster_prefab = Resources.Load<GameObject>("UI/Prefabs/battle_monsters");
        battle_Boss_monster_prefab = Resources.Load<GameObject>("UI/Prefabs/battle_Boss_monsters");
        battle_player_prefab = Resources.Load<GameObject>("UI/Prefabs/battle_players"); 
        m_medicine_borm = Find<Transform>("medicine_list");
        m_skill_borm = Find<Transform>("skill_list");
        Medicineitem_prefab = Tool_UI.Find_Prefabs<medicineitem>("medicineitem");
        skill_offect_item_prefab = Tool_UI.Find_Prefabs<skill_offect_item>("skill_offect_item");
        show_drop_list = Find<show_drop_list>("show_drop_list");
        boss_slider = Find<Boss_Slider>("Boss_Slider");
        show_info_borm= Find<Transform>("show_info");
        show_info_item_prefab = Tool_UI.Find_Prefabs<show_info_item>("show_info_item");
        InitMedicine();
        InitBoss();
    }

    /// <summary>
    /// 刷新状态
    /// </summary>
    public void Refresh()
    {
        refresh_Medicine();
    }
    /// <summary>
    /// 初始化药品
    /// </summary>
    private void InitMedicine()
    {
        medicine_list = new Dictionary<medicineType, medicineitem>();
        List<(string, int)> crt_bag_resources = SumSave.crt_bags.Set();
        
        for (int i = 0; i < Enum.GetNames(typeof(medicineType)).Length; i++)
        {
            medicineitem medicineitem = Instantiate(Medicineitem_prefab, m_medicine_borm);
            int number = -1;
            for (int j = 0; j < SumSave.crt_setting.medicine_list.Count; j++)
            {
                if (SumSave.crt_setting.medicine_list[j].Item1 == (i + 1))
                {
                    for (int k = 0; k < crt_bag_resources.Count; k++)
                    {
                        if (crt_bag_resources[k].Item1 == SumSave.crt_setting.medicine_list[j].Item2)
                        {
                            number = crt_bag_resources[k].Item2;
                            break;
                        }
                    }
                    medicineitem.Init((medicineType)(i+1), SumSave.crt_setting.medicine_list[j].Item2, SumSave.crt_setting.medicine_list[j].Item3, number);
                    break;
                }
            }
            medicineitem.GetComponent<Button>().onClick.AddListener(() => { medicineitem.OnClick(); });
            medicine_list.Add((medicineType)(i + 1), medicineitem);
        }
    }
    /// <summary>
    /// 刷新药品
    /// </summary>
    private void refresh_Medicine()
    {
        List<(string, int)> crt_bag_resources = SumSave.crt_bags.Set();
        int number = -1;
        foreach (medicineType item in medicine_list.Keys)
        {
            number = -1;
            for (int i = 0; i < SumSave.crt_setting.medicine_list.Count; i++)
            {
                if (SumSave.crt_setting.medicine_list[i].Item1 == (int)item)
                {
                    for (int k = 0; k < crt_bag_resources.Count; k++)
                    {
                        if (crt_bag_resources[k].Item1 == SumSave.crt_setting.medicine_list[i].Item2)
                        {
                            number = crt_bag_resources[k].Item2;
                            break;
                        }
                    }
                    medicine_list[item].DicInit(SumSave.crt_setting.medicine_list[i].Item2, SumSave.crt_setting.medicine_list[i].Item3, number);
                    break;
                }
            }
        }
    }
    /// <summary>
    /// 使用药品
    /// </summary>
    /// <param name="type"></param>
    protected void Use_Medicine(medicineType type)
    {
        Need_Condition(medicine_list[type].GetBag.Name, 1);
        if (Return_Condition())
        {
            foreach (var item in medicine_list.Keys)
            {
                if (item == type)
                {
                    switch (item)
                    {
                        case medicineType.回血:
                        case medicineType.回蓝:
                        case medicineType.强效:
                            Bag_Base_VO bag = medicine_list[item].GetBag;
                            if (bag == null) return;
                            for (int i = 0; i < player_list.Count; i++)
                            {
                                if (player_list[i].GetComponent<BaseBattleAttack>() != null)
                                {
                                    if (player_list[i].GetComponent<BaseBattleAttack>().Data.type == Battle_Game_Type.player)
                                    {
                                        BattleHealthState health = player_list[i].GetComponent<BattleHealthState>();
                                        health.Use_Medicine(bag.hp, bag.mp);
                                    }
                                }
                            }
                            break;
                        case medicineType.随机:
                            for (int i = 0; i < monster_list.Count; i++)
                            {
                                monster_list[i].transform.position = new Vector2(Screen.width / 2 + Random.Range(-4000, 4000), Screen.height / 2 + Random.Range(-4000, 4000));
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

        }
        else Debug.Log(medicine_list[type].GetBag.Name+"  不足");
    }
    /// <summary>
    /// 自动使用药品
    /// </summary>
    /// <param name="proportion"></param>
    protected void Anto_Use_Medicine(BattleHealthState user)
    {
        (int, int) bag = user.Proportion();

        foreach (var item in medicine_list.Keys)
        {
            switch (item)
            {
                case medicineType.回血:
                case medicineType.强效:
                case medicineType.随机:
                    if (bag.Item1 <= medicine_list[item].proportion)
                    {
                        medicine_list[item].OnClick();
                    }
                    break;
                case medicineType.回蓝:
                    if (bag.Item2 <= medicine_list[item].proportion)
                    { 
                        medicine_list[item].OnClick();
                    }
                    break;
            }
        }
    }

    public override void Show()
    {
        transform.SetAsLastSibling();
        base.Show();
    }
    /// <summary>
    /// 开启地图
    /// </summary>
    /// <param name="map"></param>
    public void GoMap(db_map_vo map,int lv)
    {
        crt_map = map;
        crt_map.SetMapIntensity(lv);
        map_name.text= map.map_name;
        time_info.text = "";
        Init();
    }

    private void Init()
    {
        open_crate_monster = true;
        if (crt_map.map_type != 0)
        {
            limited_time = 60f;//限时地图
        }
        map_crate_boss_condition = 0;
        boss_slider.gameObject.SetActive(false);
        InitMap();
        crate_player();
        crate_monster();
        StartCoroutine(Game_WaitTime(crt_map.map_cd[crt_map.GetMapIntensityDrop - 1]));
    }

    /// <summary>
    /// 初始化怪物
    /// </summary>
    private void crate_monster()
    {
        //判断是否生成boss
        if (map_crate_boss_condition >= crt_map.map_crate_boss_condition[crt_map.GetMapIntensityDrop - 1])
        {
            if (Meet_maposs_criteria(crt_map.map_boss[crt_map.GetMapIntensityDrop - 1]))
            {
                Generate_Boss_Monster(crt_map.map_boss[crt_map.GetMapIntensityDrop - 1]);
            }
        }
        int max = (int)MathF.Min(crt_map.map_crate_number_monster[crt_map.GetMapIntensityDrop - 1],
            crt_map.map_max_number_monster[crt_map.GetMapIntensityDrop - 1] - monster_list.Count);

        if (max > 0)
        { 
            for (int i = 0; i < max; i++) Generate_Monster();
        }
        
        //判断自动boss
        Auto_Generate_Boss();

    }

    /// <summary>
    /// 判断boss是否到了刷新时间
    /// </summary>
    private Dictionary<string,int> Generate_Boss_Time = new Dictionary<string, int>();

    /// <summary>
    /// 判断是否自动boss
    /// </summary>
    private void Auto_Generate_Bossold()
    {
        if(!IsBoss)return;
        for (int i = 0; i < SumSave.crt_setting.battle_Boss_list.Count; i++)
        {
            (string, int) boss = SumSave.crt_setting.battle_Boss_list[i];
            if (boss.Item2 > 0)
            {
                List<string> list = ArrayHelper.Get_Split<string>(boss.Item1, '+');
                if (list.Count == 2)
                {
                    if (!Search_Generate_Boss_Time(list[0]))
                    {
                        //不存在boss跳过
                        break;
                    }
                    //计算过去了多久
                    int spanSeconds = Battle_Tool.SettlementTransport(list[1], 2);
                    //判断是否到了刷新时间
                    if (Generate_Boss_Time[list[0]] <= spanSeconds)
                    {
                        //判断是否满足召唤条件
                        Need_Condition(common_items_list.Boss召唤卷轴, 1);
                        if (Return_Condition())
                        {
                            Alert_Dec.Show("召唤 " + list[0] + " 成功");
                            Generate_Boss_Monster(list[0]);
                            SumSave.crt_setting.battle_Boss_list[i] = (
                                list[0] + "+" +
                                UnifiedDateTime.ToString(UnifiedDateTime.FromString(list[1]).AddSeconds(Generate_Boss_Time[list[0]])),
                                boss.Item2 - 1);
                            SumSave.crt_setting.MysqlData();
                        }
                        return;
                    }
                }

            }
        }

    }
    private void Auto_Generate_Boss()
    {
        if (!IsBoss) return;
        for (int i = 0; i < SumSave.crt_setting.battle_Boss_list.Count; i++)
        {
            (string, int) boss = SumSave.crt_setting.battle_Boss_list[i];
            if (boss.Item2 > 0)
            {
                List<string> list = ArrayHelper.Get_Split<string>(boss.Item1, '+');
                if (list.Count == 2)
                {
                    //时间召唤
                    if (Meet_maposs_criteria(list[0]))
                    {
                        //判断是否满足召唤条件 开启召唤
                        Generate_Boss_Monster(list[0]);
                        return;
                    }
                }

            }

        }

        for (int i = 0; i < SumSave.crt_setting.battle_Boss_list.Count; i++)
        {
            (string, int) boss = SumSave.crt_setting.battle_Boss_list[i];
            if (boss.Item2 > 0)
            {
                List<string> list = ArrayHelper.Get_Split<string>(boss.Item1, '+');
                if (list.Count == 2)
                {
                    //物品召唤
                    if (int.Parse(list[1]) > 0)
                    {
                        Need_Condition(common_items_list.Boss召唤卷轴, 1);
                        if (Return_Condition())
                        {
                            Alert_Dec.Show("召唤 " + list[0] + " 成功");
                            Generate_Boss_Monster(list[0]);
                            SumSave.crt_setting.battle_Boss_list[i] = (
                                list[0] + "+" + (int.Parse(list[1]) - 1),
                                boss.Item2 - 1);
                            SumSave.crt_setting.MysqlData();
                        }
                        return;
                    }
                }

            }
        }


    }
    /// <summary>
    /// 符合刷新条件
    /// </summary>
    /// <param name="value">boss</param>
    private bool Meet_maposs_criteria(string value)
    {
        if (!IsBoss) return false;
        bool is_true = false;
        (int, string) Boss_Time = Tool_Battle.GetBossTime(value);
        if (Boss_Time.Item2 == "no") return false;
        int spanSeconds = Battle_Tool.SettlementTransport(Boss_Time.Item2 , 2);
        db_vip crt_vip = Tool_Battle.Obtain_Vip();
        if (crt_vip != null)
        {
            if (spanSeconds >= Boss_Time.Item1 * (100 - crt_vip.monsterHuntingInterval) / 100)
            {
                is_true = true;
            }
        }
        else
        {
            if (spanSeconds >= Boss_Time.Item1)
            { 
                is_true = true;
            }
        }
        if (is_true)
        {
            Tool_Battle.SetBossTime(value, Boss_Time.Item1, Tool_UI.ToStandardFormat(SumSave.nowtime >= DateTime.Now ? SumSave.nowtime : DateTime.Now));
            //添加boss列表
            //SumSave.crt_setting.battle_Boss_list.Add(((value + "+" + SumSave.nowtime), 0));
            //SumSave.crt_setting.MysqlData();
        }
      
        return is_true;
    }
    /// <summary>
    /// 查找boss刷新时间
    /// </summary>
    /// <param name="boss_name"></param>
    /// <returns></returns>
    private bool Search_Generate_Boss_Time(string boss_name)
    {
        if (!Generate_Boss_Time.ContainsKey(boss_name))
        {
            foreach (var item in SumSave.db_maps)
            {
                for (int i = 0; i < item.map_boss.Count; i++)
                {
                    if (item.map_boss[i] == boss_name)
                    {
                        Generate_Boss_Time.Add(boss_name, item.map_boss_cdtime[i]);
                        return true;
                    }
                }
            }
            return false;
        }
        else return true;
        
    }

    /// <summary>
    /// 清空场景
    /// </summary>
    private void InitMap()
    {
        StopAllCoroutines();
        for (int i = 0; i < monster_list.Count; i++)
        { 
            monster_list[i].GetComponent<BattleHealthState>().Clear();
        }
        for (int i = 0; i < player_list.Count; i++)
        { 
            player_list[i].GetComponent<BattleHealthState>().Clear();
        }
        monster_battle_list.Clear();
        monster_list.Clear();
        player_list.Clear();
        Close_BossSlider();
        monster_battle_list.Add(Obtain_Monster_MaxBattle(crt_map.map_monster[crt_map.GetMapIntensityDrop - 1]));
    }
    /// <summary>
    /// 初始化boss列表
    /// </summary>
    private void InitBoss()
    {
        foreach (var item in SumSave.db_maps)
        {
            for (int i = 0; i < item.map_boss.Count; i++)
            {
                monster_Bossbattle_list.Add(Obtain_Monster_MaxBattle(item.map_boss[i]));
            }
        }
    }
    /// <summary>
    /// 生成怪物属性
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private crtMaxBattleVO Obtain_Monster_MaxBattle(string value)
    {
        crtMaxBattleVO monster = ArrayHelper.Find(SumSave.db_monsters, e => e.crt_name == value);
        return monster;
    }
    /// <summary>
    /// 清除怪物
    /// </summary>
    /// <param name="health"></param>
    protected void clearSumhealth(BattleHealthState health)
    {
        if (health.GetComponent<BaseBattleAttack>() != null)
        {
            BaseBattleAttack baseBattleAttack = health.GetComponent<BaseBattleAttack>();
            SumSave.crt_illustrated.Add_illustrated_list(baseBattleAttack.Data.crt_name);
            switch (baseBattleAttack.Data.type)
            {
                case Battle_Game_Type.player:
                case Battle_Game_Type.call:
                    
                    player_list.Remove(health.gameObject);
                    health.Clear();
                    if (player_list.Count == 0)
                    {
                        //战斗失败
                        gameover();
                    }else
                    if (baseBattleAttack.Data.type == Battle_Game_Type.call)
                    {
                        //复活召唤兽
                        Resurrection_Call(baseBattleAttack.Data.crt_name);
                    }
                    break;
                case Battle_Game_Type.monster:
                case Battle_Game_Type.Boss:
                case Battle_Game_Type.Activity_Monster:
                    map_crate_boss_condition++;
                    kill_monster++;
                    Drop(baseBattleAttack);
                    monster_list.Remove(health.gameObject);
                    if (baseBattleAttack.Data.type == Battle_Game_Type.Boss)
                    {
                        AddBossStringData(baseBattleAttack.Data.crt_name);
                    }
                    health.Clear();
                    break;
            }
        }
    }
    /// <summary>
    /// 首次击杀后写入可以召唤列表
    /// </summary>
    /// <param name="value"></param>
    private void AddBossStringData(string value)
    {

        for (int i = 0; i < SumSave.crt_setting.battle_Boss_list.Count; i++)
        {
            (string, int) boss = SumSave.crt_setting.battle_Boss_list[i];
            if (boss.Item1.Contains(value))
            {
                return;
            }
        }
        SumSave.crt_setting.battle_Boss_list.Add((value + "+" + 0, 0));
        SumSave.crt_setting.MysqlData();
    }
    /// <summary>
    /// 获取额外收益
    /// </summary>
    /// <param name="monster"></param>
    private void AdditionalIncome(BaseBattleAttack monster)
    { 
        //图鉴收益

    }
    /// <summary>
    /// 复活cd中
    /// </summary>
    private void gameover()
    {
        StartCoroutine(Game_InitMap(5));
    }
    /// <summary>
    /// 战斗失败
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator Game_InitMap(float time)
    {
        while (time > 0)
        {
            Alert_Dec.Show("战斗失败,重置中" + time.ToString("0.0") + "s");
            time -= 0.5f;
            yield return new WaitForSeconds(0.5f);
        }
        Init();

    }


    /// <summary>
    /// 掉落收益
    /// </summary>
    /// <param name="monster"></param>
    private void Drop(BaseBattleAttack monster)
    {
        int exp = (int)monster.Data.exp * (100 + SumSave.crtMaxBattle.exp_bonus) / 100;
        Show_Info("击杀 " + monster.Data.crt_name + " 获得经验 " + exp);
        //掉落收益

        Add_Exp(exp); 
        AdditionalIncome(monster);
        Show_Info( show_drop_list.Init(monster));
        switch (monster.Data.type)
        {
            //case Battle_Game_Type.monster://基础掉落
            case Battle_Game_Type.Boss://boss掉落
                Game_Omphalos.global_battle_info("击杀 " + monster.Data.crt_name);
                AddSkill();
                Close_BossSlider();
                break;
        }
    }
    /// <summary>
    /// 增加技能经验
    /// </summary>
    private void AddSkill()
    {
        Dictionary<int, db_skill_vo> keyValues = SumSave.crt_skill.Set_Current_skill();
        foreach (var item in keyValues)
        {
            if (item.Value.Job != -1)
            {
                item.Value.GetExp(1); 
            }
        }
        SumSave.crt_skill.UpLv_skill();
    }

    List<show_info_item> list_show_info = new List<show_info_item>();
    /// <summary>
    /// 显示信息
    /// </summary>
    /// <param name="valueS"></param>
    private void Show_Info(List<string> valueS)
    {
        foreach (var item in valueS)
        { 
            Show_Info(item);
        }
    }
    /// <summary>
    /// 获取信息显示
    /// </summary>
    /// <param name="value"></param>
    private void Show_Info(string value)
    {
        if (list_show_info.Count > 5)
        {
            show_info_item item = list_show_info[0];
            list_show_info.RemoveAt(0);
            item.gameObject.SetActive(true);
            item.Show(value);
            item.transform.SetAsLastSibling();
            list_show_info.Add(item);
        }
        else
        { 
            show_info_item item = Instantiate(show_info_item_prefab, show_info_borm);
            item.Show(value);
            list_show_info.Add(item);
        }
    }
    /// <summary>
    /// 增加经验
    /// </summary>
    /// <param name="exp"></param>
    private void Add_Exp(int exp)
    {
        Battle_Tool.Obtain_Exp(exp);
    }

    /// <summary>
    /// 生成技能
    /// </summary>
    /// <param name="p_brom"></param>
    /// <param name="exist"></param>
    /// <returns></returns>
    private List<skill_offect_item> Show_Battle_Skill(Transform p_brom, bool exist = false)
    {
        ClearObject(p_brom);
        List<skill_offect_item> list_skill = new List<skill_offect_item>();
        List<int> list = SumSave.crt_skill.Set_Select_Skill_Type();
        Dictionary<int, db_skill_vo> dic = SumSave.crt_skill.Set_Current_skill();
        for (int i = 0; i < list.Count; i++)
        {
            skill_offect_item item = Instantiate(skill_offect_item_prefab, p_brom);
            if (dic.ContainsKey(list[i]))
            {
                item.Init(i, dic[list[i]]);
            }
            else
            {
                db_Hero_VO hero = SumSave.db_heros.Find((db_Hero_VO hero) => hero.id == SumSave.crtHero.job);
                db_skill_vo skill = SumSave.db_skills.Find((db_skill_vo skill) => skill.show_name == hero.initskill);
                item.Init(i, skill);
            }
            list_skill.Add(item);
        }
        return list_skill;
    }
    /// <summary>
    /// 是否生成护盾
    /// </summary>
    private bool Open_Crate_Effect_5 = true;
    /// <summary>
    /// 生成玩家
    /// </summary>
    private void crate_player()
    {
        GameObject item = ObjectPoolManager.instance.GetObjectFormPool(SumSave.crtMaxBattle.crt_name, battle_player_prefab,
            GetRandomUVPosition(10), Quaternion.identity, battle_borm.transform);
        item.GetComponent<BaseBattleAttack>().Data = SumSave.crtMaxBattle;
        item.GetComponent<BaseBattleAttack>().Refresh_Skill(Show_Battle_Skill(m_skill_borm));
        Dictionary<int, db_skill_vo> dic = SumSave.crt_skill.Set_Current_skill();
        foreach (var id in dic) 
        {
            if (id.Value.EffectType == 5)//护盾
            {
                if (Open_Crate_Effect_5)
                {
                    //护盾只生成一次
                    GameObject skill_prefabs = Resources.Load<GameObject>("UI/skill_prefabs/skill_" + id.Value.id);// skill.id); 
                    ObjectPoolManager.instance.GetObjectFormPool(id.Value.show_name, skill_prefabs, new Vector3(item.transform.position.x, item.transform.position.y), Quaternion.identity, item.transform);
                    Open_Crate_Effect_5 = false;
                }
            }
            else
            if (id.Value.EffectType == 6)//召唤
            {
                GameObject call = ObjectPoolManager.instance.GetObjectFormPool(id.Value.show_name, battle_player_prefab,
           GetRandomUVPosition(800), Quaternion.identity, battle_borm.transform);
                call.GetComponent<BaseBattleAttack>().Data = Tool_Battle.Crate_Call(id.Value);
                player_list.Add(call);
            }
        }
        player_list.Add(item);
    }

    private void Resurrection_Call(string name)
    {
        Dictionary<int, db_skill_vo> dic = SumSave.crt_skill.Set_Current_skill();
        foreach (var id in dic)
        {
            if (id.Value.show_name == name)
            {
                GameObject call = ObjectPoolManager.instance.GetObjectFormPool(id.Value.show_name, battle_player_prefab,
               GetRandomUVPosition(800), Quaternion.identity, battle_borm.transform);
                call.GetComponent<BaseBattleAttack>().Data = Tool_Battle.Crate_Call(id.Value);
                player_list.Add(call);
            }
        }
    }

    bool open_crate_monster = false;
    private IEnumerator Game_WaitTime(float time)
    {
        while (time > 0)
        {
            time -= 0.1f;
            time_info.text = time.ToString("0.0") + "s";
            if (crt_map.map_type != 0)
            { 
                limited_time-=0.1f;
                time_info.text += "\n地图关闭" + limited_time.ToString("0.0") + "s";
            }
            yield return new WaitForSeconds(0.1f);
        }
        if (crt_map.map_type != 0)
        {
            if (limited_time <= 0)
            {
                open_crate_monster = false;
                 Alert.Show(crt_map.map_name,crt_map.map_name+"地图已关闭,请切换地图");
            }
        }
        if (open_crate_monster)
        {
            time_info.text = "";
            crate_monster();
            StartCoroutine(Game_WaitTime(crt_map.map_cd[crt_map.GetMapIntensityDrop - 1]));
        }
        


    }

    private bool IsBoss = true;


    private void Generate_Boss_Monster(string specify)
    {
        //每次只刷新一个boss
        if (!IsBoss) return;
        crtMaxBattleVO monster = ArrayHelper.Find(monster_Bossbattle_list, e => e.crt_name == specify);
        GameObject item = ObjectPoolManager.instance.GetObjectFormPool(monster.crt_name, battle_Boss_monster_prefab,
            GetRandomUVPosition(2000), Quaternion.identity, battle_borm.transform);
        item.GetComponent<BaseBattleAttack>().Data = monster;
        item.transform.SetAsFirstSibling();
        monster_list.Add(item);
        InitBossSlider(monster);

    }
    /// <summary>
    /// 初始化boss血条
    /// </summary>
    /// <param name="monster"></param>
    private void InitBossSlider(crtMaxBattleVO monster)
    {
        IsBoss = false;
        boss_slider.gameObject.SetActive(true);
        boss_slider.Init(monster.crt_name, monster.data.battle_maxhp);
    }
    /// <summary>
    /// 刷新boss血条
    /// </summary>
    /// <param name="hp"></param>
    protected void Real_Time_BossSlider(long hp)
    { 
        boss_slider.Set((int)hp);
    }
    /// <summary>
    /// 关闭boss血条
    /// </summary>
    private void Close_BossSlider()
    { 
        boss_slider.gameObject.SetActive(false);
        IsBoss = true;
    }

    /// <summary>
    /// 生成怪物
    /// </summary>
    private void Generate_Monster()
    {
        crtMaxBattleVO monster = monster_battle_list[Random.Range(0, monster_battle_list.Count)];
        GameObject item = ObjectPoolManager.instance.GetObjectFormPool(monster.crt_name, battle_monster_prefab,
            GetRandomUVPosition(2000), Quaternion.identity, battle_borm.transform);
        item.GetComponent<BaseBattleAttack>().Data = monster;
        item.transform.SetAsFirstSibling();
        monster_list.Add(item);
        //测试掉落
        //show_drop_list.Init(crt_map, monster.type);

    }
    /// <summary>
    /// 生成半径内随机坐标
    /// </summary>
    /// <returns></returns>
    private Vector2 GetRandomUVPosition(int radius)
    {
        return GetRandomPoint(new Vector2(Screen.width / 2, Screen.height / 2), radius);
    }
    /// <summary>
    /// 生成半径内随机坐标
    /// </summary>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    private Vector2 GetRandomPoint(Vector2 center, float radius)
    {
        // 生成随机角度（0-360度）
        float angle = Random.Range(0f, 2f * Mathf.PI);

        // 生成均匀分布的半径（避免点聚集在圆心）
        float adjustedRadius = Mathf.Sqrt(Random.value) * radius;

        // 转换为笛卡尔坐标
        float x = center.x + adjustedRadius * Mathf.Cos(angle);
        float y = center.y + adjustedRadius * Mathf.Sin(angle);

        return new Vector2(x, y);
    }
}
