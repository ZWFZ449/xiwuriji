using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class panel_fight : Panel_Base
{
    /// <summary>
    /// 玩家预制体
    /// </summary>
    private GameObject player_battle_attack_prefabs;

    private GameObject monster_battle_attack_prefabs;

    /// <summary>
    /// 怪物生成位置
    /// </summary>
    private Transform pos_player, pos_monster;
    /// <summary>
    /// 当前怪物列表
    /// </summary>
    private List<crtMaxHeroVO> crt_map_monsters = new List<crtMaxHeroVO>();
    /// <summary>
    /// 当前地图
    /// </summary>
    private user_map_vo select_map;
    /// <summary>
    /// 刷新状态
    /// </summary>
    private bool Open_Monster_State= true;
    /// <summary>
    /// 刷新技能
    /// </summary>
    private pight_show_skill pight_show_skill;
    /// <summary>
    /// 战斗技能
    /// </summary>
    private List<oldskill_offect_item> battle_skills;
    /// <summary>
    /// 显示基础信息
    /// </summary>
    private panel_role_health role_health;
    /// <summary>
    /// 显示地图名称和回合时间
    /// </summary>
    private Text map_name, map_time;
    /// <summary>
    /// 关闭列表按钮
    /// </summary>
    private Button close_btn;
    /// <summary>
    /// 是否关闭列表
    /// </summary>
    private bool close_panel_state = true;
    /// <summary>
    /// 列表位置
    /// </summary>
    private Transform pos_btn,pos_show_info;
    /// <summary>
    /// 信息框
    /// </summary>
    private show_info_item show_info_prefabs;
    /// <summary>
    /// 存储信息
    /// </summary>
    private List<show_info_item> show_info_list = new List<show_info_item>();
    /// <summary>
    /// 战斗信息汇总
    /// </summary>
    private Text battle_info_list;
    /// <summary>
    /// 战斗统计清空
    /// </summary>
    private Button btn_Combat_statistics;

    public Button close_battle;
    /// <summary>
    /// 当前出现怪物数量 总怪物数量 试练塔层数
    /// </summary>
    private int crt_monster_number = 0, maxnumber = 0, trial_storey = -1;
    /// <summary>
    /// 试练塔
    /// </summary>
    private Trial_Tower trial_tower;
    /// <summary>
    /// 五行种子
    /// </summary>
    private string[] FiveElementSeeds= { "牡丹皮", "青蒿", "苦参", "葛根", "金银花" , "黄连" , "薄荷" , "决明子", "芍药" , "菊花", "桂枝" , "穿心莲", "银翘", "栀子", "桑叶" };
    /// <summary>
    /// 数据库
    /// </summary>
    private List<BattleHealth> players=new List<BattleHealth>(), monsters=new List<BattleHealth>();
    /// <summary>
    /// 全部对象
    /// </summary>
    private List<BattleHealth> sumhealths = new List<BattleHealth>();
    /// <summary>
    /// 状态刷新开关
    /// </summary>
    private bool openRefreshStatus = true;
    /// <summary>
    /// 怪物属性展示
    /// </summary>
    private show_monster_info show_monster_info;
    protected override void Awake()
    {
        base.Awake();
        Combat_statistics.Init();
    }

    public override void Initialize()
    {
        base.Initialize();
        pos_monster = Find<Transform>("battle_pos/monster_pos");
        pos_player = Find<Transform>("battle_pos/player_pos");
        player_battle_attack_prefabs= Resources.Load<GameObject>("Prefabs/prefab/player_battle_attck_item"); ; //Battle_Tool.Find_Prefabs<GameObject>("player_battle_attck_item")
        monster_battle_attack_prefabs = Resources.Load<GameObject>("Prefabs/prefab/monster_battle_attck_item");// Battle_Tool.Find_Prefabs<GameObject>("monster_battle_attck_item"); 
        pight_show_skill = Find<pight_show_skill>("skill_list");
        role_health = Find<panel_role_health>("panel_role_health");
        map_name = Find<Text>("battle_pos/map_name/info");
        map_time = Find<Text>("battle_pos/map_name/show_time");
        close_btn = Find<Button>("btn_list/close_btn");
        close_btn.onClick.AddListener(() => { Close(); });
        pos_btn = Find<Transform>("btn_list");
        pos_show_info = Find<Transform>("info_list/Viewport/Content");
        show_info_prefabs = Battle_Tool.Find_Prefabs<show_info_item>("show_info_item");
        btn_Combat_statistics = Find<Button>("Combat_statistics");
        btn_Combat_statistics.onClick.AddListener(() => { clear_Combat_statistics(); });
        battle_info_list= Find<Text>("Combat_statistics/info");
        show_monster_info = Find<show_monster_info>("show_monster_info");
    }
    /// <summary>
    /// 初始状态
    /// </summary>
    private void clear_Combat_statistics()
    {
        Alert.Show("清空战斗信息", "是否清空战斗信息", Clear_Combat_statistics);
       
    }

    private void Update()
    {
        if (openRefreshStatus)RefreshStatus();
    }



    private void RefreshStatus()
    {
        //Debug.Log("刷新状态");
        if (players != null)
        {
            if (players.Count > 0)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    players[i].GetComponent<BattleAttack>().FindTergets(monsters);
                }
            }
        }
        else
        {
            Debug.Log("玩家为空");
        }
        if (monsters != null)
        {
            if (monsters.Count > 0)
            {
                for (int i = 0; i < monsters.Count; i++)
                {
                    monsters[i].GetComponent<BattleAttack>().FindTergets(players);
                }
            }
        }else
        {
            Debug.Log("怪物为空");
        }
        openRefreshStatus = false;
    }
    /// <summary>
    /// 清空战斗信息
    /// </summary>
    /// <param name="arg0"></param>
    private void Clear_Combat_statistics(object arg0)
    {
        Combat_statistics.Init();
    }

    /// <summary>
    /// 显示战斗信息
    /// </summary>
    public void Show_Combat_statistics()
    {
        battle_info_list.text = Combat_statistics.Show_Info();
        IsReply(players);
        IsReply(monsters);
    }

    /// <summary>
    /// 群聊回血
    /// </summary>
    /// <param name="hp"></param>
    protected void add_hp(int hp)
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].HealConsumables(hp, 0);
        }
    }
    /// <summary>
    /// 回复生命魔法
    /// </summary>
    /// <param name="list"></param>
    private void IsReply(List<BattleHealth> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            BattleAttack attack = list[i].GetComponent<BattleAttack>();
            if (attack.Data.Heal_Hp > 0|| attack.Data.Heal_Mp > 0)
            {
                //Debug.Log(attack.Data.show_name + " " + attack.Data.Heal_Hp);
                list[i].HealConsumables(attack.Data.Heal_Hp, attack.Data.Heal_Mp);
            }
        }
    }

    /// <summary>
    /// 关闭列表
    /// </summary>
    private void Close()
    {
        if (trial_storey >= 0)
        {
            trial_tower.Show();
        } 
        close_battle.gameObject.SetActive(true);
        transform.SetAsFirstSibling();
        return;
        
    }
    public override void Show()
    {
        base.Show();
        AudioManager.Instance.ChangeBGM(BGMenum.开启);
        openRefreshStatus = true;
    }
    public override void Hide()
    { 
        base.Hide();
    }

    /// <summary>
    /// 获取秘境奖励
    /// </summary>
    /// <param name="target"></param>
    protected void settlementSecretRealm(BattleAttack target)
    {
        user_map_vo map = ArrayHelper.Find(SumSave.read_lose_map, e => e.map_index == target.Data.map_index);
        Dictionary<string, int> dic = new Dictionary<string, int>();
        string dec = "";
        if (map.Independent_Drop != "")//保底收益
        {
            string[] values = map.Independent_Drop.Split('&');
            int max = SecretRealm_lv + 1;
            for (int j = 0; j < max; j++)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i] != "")
                    {
                        if (!dic.ContainsKey(values[i])) dic.Add(values[i], 0); 
                        dic[values[i]] += 1;
                    }
                    
                }
            }
            foreach (var item in dic.Keys)
            {

                dec += "获得" + item + " * " + dic[item] + "\n";
                int random = Random.Range(1, 100);
                int number = dic[item];
                int maxnumber = number + Random.Range(1, 100);
                Battle_Tool.Obtain_Resources(Obtain_Int.Add(1, item, new int[] { number + random, random }), maxnumber);

            }
        }
        //10%的概率掉落装备
        if (Random.Range(0, 100) < 10)
        {
            if (map.ProfitList != "")
            { 
                string[] values = map.ProfitList.Split('&');
                string ProfitList = values[Random.Range(0, values.Length)];
                string[] ProfitList_values = ProfitList.Split(' ');
                if (ProfitList_values.Length == 3)
                {
                    int random = Obtain_Number(SecretRealm_lv);
                    Bag_Base_VO bag = tool_Categoryt.crate_equip(ProfitList_values[0], random);
                    //dec += "获得" + (enum_equip_quality_list)random + " " + bag.Name + " * 1\n";
                    //SumSave.crt_bag.Add(bag);
                }
            }
        }
        Alert.Show(map.map_name, dec);
    }
    /// <summary>
    /// 获得品质
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    private int Obtain_Number(int number)
    {
        int value = 1;
        var enumNames = Enum.GetNames(typeof(enum_equip_quality_list));
        if (number == enumNames.Length)//最大难度
        {
            if (Random.Range(0, 100) < 10)
            {
                value = number;
            }
            else value = number - 1;
        }
        else
        {
            if (number == 0) value = 1;
            else
           {
                if (Random.Range(0, 100) < 5)
                {
                    value = number + 1;
                }
                else
                {

                    if (Random.Range(0, 100) < 45)
                    { 
                       value = number;
                    }else value = number - 1;
                }
            
           }
        }
        value = Mathf.Max(1, value);
        return value;
    }

    /// <summary>
    /// 副本初始化
    /// </summary>
    /// <param name="damage"></param>
    protected void DailyCopies(BattleHealth target)
    {
        int damge = (int)(target.maxHP - target.HP);
        damge = (int)MathF.Min(damge, target.maxHP);
        long max = 0;
        long value = 0;
        switch (select_map.map_index)
        {
            case 37: //历练
                max = SumSave.crt_MaxHero_okd.Lv * 1000;
                value = (long)(damge * max / target.maxHP);
                Alert.Show(select_map.map_name, "副本战斗结束,造成伤害 " + damge + "\n获得历练 " + value);
                Battle_Tool.Obtain_Unit(currency_unit.元宝, value) ;
                break;
            case 36: //魔丸
                max = SumSave.crt_MaxHero_okd.Lv / 5 + 20;
                value = (long)(damge * max / target.maxHP);
                Alert.Show(select_map.map_name, "副本战斗结束,造成伤害 " + damge + "\n获得魔丸 " + value );
                Battle_Tool.Obtain_Unit(currency_unit.Boss积分, value);
                break;
            case 35: //灵气
                if (SumSave.crt_world != null)
                {
                    max = (SumSave.crt_world.World_Lv + 1) * 100;
                    value = (long)(damge * max / target.maxHP);
                    Alert.Show(select_map.map_name, "副本战斗结束,造成伤害 " + damge + "\n获得灵气 " + value );
                    //SumSave.crt_world.Set((int)value);
                    Battle_Tool.Obtain_Unit(currency_unit.灵气, (int)value);

                    //SumSave.crt_world.AddValue_lists((int)value);
                    //Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_world, SumSave.crt_world.Set_Uptade_String(), SumSave.crt_world.Get_Update_Character());
                }
                break;
            case 34: //经验
                max = SumSave.db_lvs_old.hero_lv_list[SumSave.crt_MaxHero_okd.Lv] / 5;
                value = (long)(damge * max / target.maxHP );
                Alert.Show(select_map.map_name, "副本战斗结束,造成伤害 " + damge + "\n获得经验 " + value );
                Battle_Tool.Obtain_Exp(value,2);
                break;
            case 33://灵珠
                max = SumSave.crt_MaxHero_okd.Lv * 1000000;
                value = (long)(damge * max / target.maxHP);
                Alert.Show(select_map.map_name, "副本战斗结束,造成伤害 " + damge + "\n获得灵珠 " + value );
                Battle_Tool.Obtain_Unit(currency_unit.金币, value);
                break;
            case 32://下品噬心魔丸
                max = SumSave.crt_MaxHero_okd.Lv + 50;
                value = (int)(damge * max / target.maxHP);
                List<(string,int)> str = SumSave.db_lvs_old.world_lv_list_dic[0];
                Alert.Show(select_map.map_name, "副本战斗结束,造成伤害 " + damge + "\n获得 " + str[0].Item1+" * "+value );
                int random = Random.Range(1, 100);
                int number = (int)value;
                int maxnumber = number + Random.Range(1, 100);
                Battle_Tool.Obtain_Resources(Obtain_Int.Add(1, str[0].Item1, new int[] { number + random, random }), maxnumber);
                //Battle_Tool.Obtain_Resources(str[0].Item1,(int)value);
                break;
            default:
                break;
        }
     
        Open_Map(ArrayHelper.Find(SumSave.read_lose_map, e => e.map_name == SumSave.crt_resources.user_map_index));
    }
    /// <summary>
    /// 游戏结束
    /// </summary>
    protected void Game_Over()
    {
        if (trial_storey >= 0)
        {
            select_map = ArrayHelper.Find(SumSave.read_lose_map, e => e.map_name == SumSave.crt_resources.user_map_index);
        } 
        trial_storey = -1;
        if (Open_Monster_State)
        {
            Open_Monster_State = false;
            //进入复活周期 5s
            StopAllCoroutines();
            StartCoroutine(Game_WaitTime(5));
        }
    }
    /// <summary>
    /// 写入战斗信息
    /// </summary>
    private void write_Trial()
    {
        SendNotification(NotiList.Refresh_Trial_Tower,trial_storey);
    }
    /// <summary>
    /// 进入地图
    /// </summary>
    /// <param name="map"></param>
    /// <param name="isCopies">是否副本</param>
    public void Open_Map(user_map_vo map,bool isCopies = false)
    {
        trial_storey = -1;
        openRefreshStatus = true;
        if (!isCopies)
        {
            if(map.map_type==1)//普通地图记录切换
            {
                SumSave.crt_resources.user_map_index = map.map_name;
                
            }
            
        }else
        {


            //获得五行随机五行种子


#if UNITY_EDITOR
            #region 测试副本获得植物材料
            //string str="";
            //for(int i = 0; i <1000; i++)
            //{
            //    string material = FiveElementSeeds[Random.Range(0, FiveElementSeeds.Length)];
            //    int num = Random.Range(1, 3);
            //    Battle_Tool.Obtain_Resources(material, num);
            //  str+= material + " * " + num + "\n";
            //
            //Alert.Show("副本战斗结束", str);
            #endregion
#elif UNITY_ANDROID

#elif UNITY_IPHONE

#endif
            string material = FiveElementSeeds[Random.Range(0, FiveElementSeeds.Length)];
            int num = Random.Range(1, 3);
            int random = Random.Range(1, 100);
            int number = num;
            int maxnumber = number + Random.Range(1, 100);
            Battle_Tool.Obtain_Resources(Obtain_Int.Add(1, material, new int[] { number + random, random }), maxnumber);
            //Battle_Tool.Obtain_Resources(material, num);
            Alert.Show(map.map_name, "获得 " + material + " * " + num);

        }
        Combat_statistics.isTime = true;
        select_map = map;
        map_name.text = map.map_name;
        init();
        Crate_Init();
        StopAllCoroutines();
        
    }
    /// <summary>
    /// 秘境副本等级
    /// </summary>
    private int SecretRealm_lv = -1;
    /// <summary>
    /// 打开秘境副本
    /// </summary>
    /// <param name="map"></param>
    /// <param name="map_lv">地图强度等级</param>
    public void Open_SecretRealm(user_map_vo map, int map_lv)
    {
        select_map = map;
        map_name.text = map.map_name;
        SecretRealm_lv = map_lv;
        init();
        Crate_Init();
        StopAllCoroutines();
    }

    /// <summary>
    /// 进入地图
    /// </summary>
    /// </summary>
    /// <param name="map"></param>
    /// <param name="storey">试炼塔开启层数</param>
    public void Open_Map(user_map_vo map, int storey,Trial_Tower rank)
    {
        if (trial_tower == null) trial_tower = rank;
        Combat_statistics.isTime = true;
        if (trial_storey <= storey)//拒绝断网重复刷新
            trial_storey = storey + 1;
        select_map = map;
        map_name.text = map.map_name;
        init();
        Crate_Init();
        StopAllCoroutines();

    }
    /// <summary>
    /// 显示战斗状态
    /// </summary>
    /// <param name="dec"></param>
    private void Show_Battle_State(string dec)
    {
        map_time.text= dec;
    }
    /// <summary>
    /// 生成角色初始化
    /// </summary>
    private void Crate_Init()
    {
        crate_Skill();
        crate_hero();
        crate_monster();
    }

    public void Refresh_Max_Hero_Attribute()
    {
        if (gameObject.activeInHierarchy)
        {
            crate_Skill();
            foreach (var item in players)
            {
                item.GetComponent<BattleAttack>().Refresh(SumSave.crt_MaxHero_okd);
                item.GetComponent<BattleAttack>().Refresh_Skill(battle_skills);
            }
        }
    }
    /// <summary>
    /// 清除死亡物品
    /// </summary>
    /// <param name="health"></param>
    protected void clearhealth(BattleHealth health)
    {
        if (health.GetComponent<player_battle_attck>() != null)
        { 
            players.Remove(health);
        }else if (health.GetComponent<monster_battle_attck>() != null)
            monsters.Remove(health);
        openRefreshStatus = true;
    }
    /// <summary>
    /// 播放完死亡动画后完全回收
    /// </summary>
    /// <param name="health"></param>
    protected void clearSumhealth(BattleHealth health)
    { 
        sumhealths.Remove(health);
    }


    private void crate_Skill()
    {
        battle_skills = pight_show_skill.Init();
    }

    private void crate_hero()
    {
       
        crtMaxHeroVO crt = SumSave.crt_MaxHero_okd;
        GameObject item = ObjectPoolManager.instance.GetObjectFormPool(crt.show_name, player_battle_attack_prefabs,
            new Vector3(pos_player.position.x, pos_player.position.y, pos_player.position.z), Quaternion.identity, pos_player);
        // 设置Data
        crt.Push_name = crt.show_name;
        item.GetComponent<player_battle_attck>().Data = crt;
        item.GetComponent<player_battle_attck>().Refresh_Skill(battle_skills);
        //if (item.GetComponent<Button>().enabled)
        //    item.GetComponent<Button>().onClick.AddListener(delegate { AudioManager.Instance.playAudio(ClipEnum.购买物品); SelectMonster(item.GetComponent<MonsterBattleAttack>()); });
        //item.GetComponent<Button>().enabled = true;
        //SumSave.battleHeroHealths.Add(item.GetComponent<BattleHealth>());
        players.Add(item.GetComponent<BattleHealth>());
        sumhealths.Add(item.GetComponent<BattleHealth>());
        openRefreshStatus = true;
        role_health.SetHealth(item.GetComponent<BattleHealth>());
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void init()
    {

        crt_monster_number = 0;
        maxnumber = 100;
        switch (select_map.map_type)
        {
            case 2: maxnumber = 10; break;
            case 3: maxnumber = 1; break;
            case 4: maxnumber = 1; break;
            case 6: maxnumber = 1; break;
            case 7: maxnumber = 1; break;
            case 8: maxnumber = 1; break;

            default:
                break;
        }
        for (int i = 0; i < sumhealths.Count; i++)
        {

            sumhealths[i].Clear();
        }
        monsters.Clear();
        players.Clear();
        crt_map_monsters.Clear();
        if (select_map.map_type == 6)
        {
            for (int i = 0; i < SumSave.olddb_monsters.Count; i++)
                crt_map_monsters.Add(SumSave.olddb_monsters[i]);
        }
        else
        {
            for (int i = 0; i < SumSave.olddb_monsters.Count; i++)
            {
                if (SumSave.olddb_monsters[i].index == select_map.map_index)
                {
                    crt_map_monsters.Add(SumSave.olddb_monsters[i]);
                }
            }
        }
        
    }
    /// <summary>
    /// 死亡等待
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator Game_WaitTime(float time)
    {
        Combat_statistics.AddDead();
        while (time > 0)
        {
            Show_Battle_State("复活剩余时间 " + time.ToString("F1") + "s");
            time -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        //判断是否切换地图
        if (iSOpenMap(true))
            Open_Map(select_map);
        else Open_Map(ArrayHelper.Find(SumSave.read_lose_map, e => e.map_name == SumSave.crt_resources.user_map_index));
    }
    /// <summary>
    /// 开始游戏
    /// </summary>
    public void Game_Start()
    {
        StartCoroutine(ProduceMonster(WaitTime()));
    }

    private float WaitTime()
    {
        float Waittime = 5f;
        Waittime = (select_map.map_index-1) * 0.5f;
        Waittime = Mathf.Min(Waittime, 5f - Tool_State.Value_playerprobabilit(enum_skill_attribute_list.寻怪间隔) / 10f);
        Waittime = Mathf.Clamp(Waittime, 0.5f, 5f);
        return Waittime;
    }
    // 下一波怪
    protected void Game_Next_Map()
    {
        if (Open_Monster_State)
        {
            Open_Monster_State = false;
            if (trial_storey >= 0)
            {
                Trial_Tower_reward();
            }
            if(iSOpenMap())
                StartCoroutine(ProduceMonster(WaitTime()));
            else Open_Map(ArrayHelper.Find(SumSave.read_lose_map, e => e.map_name == SumSave.crt_resources.user_map_index));
        }
    }
    /// <summary>
    /// 试练塔奖励
    /// </summary>
    private void Trial_Tower_reward()
    {
        write_Trial();
        if (SumSave.openMysql)
        {
            Alert_Dec.Show("网络连接失败");
            return;
        }
        string dec = "";
        dec = "通关试练塔" + Show_Color.Red(trial_storey) + "层";
        dec += "\n奖励" + Show_Color.Red("下品噬心魔种" + " * 5");
        dec += "\n奖励" + Show_Color.Red("历练值" + " * " + ((trial_storey / 10 + 1) * 10000));
        Battle_Tool.Obtain_Unit(currency_unit.元宝, ((trial_storey / 10 + 1) * 10000));
        int random = Random.Range(1, 100);
        int number = 5;
        int maxnumber = number + Random.Range(1, 100);
        Battle_Tool.Obtain_Resources(Obtain_Int.Add(1, "下品噬心魔种", new int[] { number + random, random }), maxnumber);
        //Battle_Tool.Obtain_Resources("下品噬心魔种", 5);
        if (trial_storey > 0)
        {
            if ((trial_storey) % 10 == 0)
            {
                dec += "\n进阶奖励" + Show_Color.Red("万鸦壶" + " * 1");
                int randoms = Random.Range(1, 100);
                int numbers = 1;
                int maxnumbers = number + Random.Range(1, 100);
                Battle_Tool.Obtain_Resources(Obtain_Int.Add(1, "万鸦壶", new int[] { numbers + randoms, randoms }), maxnumbers);
                //Battle_Tool.Obtain_Resources("万鸦壶", 1);
            }
        }
        Alert.Show("试炼塔奖励", dec);
    }
    /// <summary>
    /// 判断地图是否可以打开
    /// </summary>
    /// <returns></returns>
    private bool iSOpenMap(bool state=false)
    {
        bool exist = true;
        if (trial_storey >= 0)
        {
            trial_storey++;
            exist = true;//试炼模式
        }
        else
        if (select_map.need_Required != "")
        {
            if (state || crt_monster_number >= maxnumber)
            {
                NeedConsumablesold(select_map.need_Required, 1);
                if (!RefreshConsumables_old())
                {
                    exist = false;
                }
            }
        }
        if (select_map.map_type == 4)
        {
            exist = false;
        }
       
        return exist;
      
    }
    private IEnumerator ProduceMonster(float time)
    {
        float basetime = 0.1f;
        Combat_statistics.isTime = false;
        while (time > 0)
        {
            Show_Battle_State("刷新时间 " + time.ToString("F1") +"s") ;
            time -= basetime;
            yield return new WaitForSeconds(basetime);
        }
        Show_Battle_State("战斗中...");
        crate_monster();
    }

    private void crate_monster()
    {
        ///调用天气buff
        //bool isAdd = true;
        //if (SumSave.crt_player_buff.player_Buffs.Count > 0)
        //{
        //    foreach (var _item in SumSave.crt_player_buff.player_Buffs)
        //    {
        //        if (_item.Value.Item4 == 4)
        //        {
        //            isAdd = false;
        //            int remainingTime = Battle_Tool.SettlementTransport((_item.Value.Item1).ToString("yyyy-MM-dd HH:mm:ss"));
        //            if (remainingTime > _item.Value.Item2)
        //            {
        //                SumSave.crt_player_buff.player_Buffs.Remove(_item.Key);
        //                AddWeather();
        //                break;
        //            }
        //        }
        //    }
        //    if (isAdd)
        //    {
        //        AddWeather();
        //    }
        //}else
        //{
        //    AddWeather();
        //}
        if (crt_monster_number >= maxnumber) crt_monster_number = 0;
        crt_monster_number++;
        crtMaxHeroVO crt = crt_map_monsters[Random.Range(0, crt_map_monsters.Count)];
        if (crt_monster_number == maxnumber)
        {
            crt = crt_map_monsters[crt_map_monsters.Count - 1];
        }
        else
        {
            if (maxnumber == 10)
            {
                crt = crt_map_monsters[0];
            }
        }
        if (trial_storey >= 0)
        {
            crt = crt_map_monsters[trial_storey % crt_map_monsters.Count];
        }
        if (select_map.map_type == 8) crt = Battle_Tool.crate_SecretRealm_monster(crt, select_map, SecretRealm_lv);
        else crt = Battle_Tool.crate_monster(crt, select_map, crt_monster_number == maxnumber, trial_storey);
        GameObject item = ObjectPoolManager.instance.GetObjectFormPool(crt.show_name, monster_battle_attack_prefabs,
            new Vector3(pos_monster.position.x, pos_monster.position.y,pos_monster.position.z), Quaternion.identity, pos_monster);
        crt.Push_name = crt.show_name;
        // 设置Data
        item.GetComponent<monster_battle_attck>().Data = crt;
        if (item.GetComponent<Button>().enabled)
            item.GetComponent<Button>().onClick.AddListener(delegate { AudioManager.Instance.playAudio(ClipEnum.购买物品); SelectMonster(crt); });
        item.GetComponent<Button>().enabled = true;
        //SumSave.battleMonsterHealths.Add(item.GetComponent<BattleHealth>());
        monsters.Add(item.GetComponent<BattleHealth>());
        sumhealths.Add(item.GetComponent<BattleHealth>());
        openRefreshStatus = true;
        Open_Monster_State=true;
        ShowInfoMap();
    }
    /// <summary>
    /// 显示怪物属性
    /// </summary>
    /// <param name="crt"></param>
    private void SelectMonster(crtMaxHeroVO crt)
    {
        show_monster_info.gameObject.SetActive(true);
        show_monster_info.show_info(crt);
    }

    private void ShowInfoMap()
    {
        map_name.text = select_map.map_name + "(" + crt_monster_number + "/" + maxnumber + ")";
        if (trial_storey >= 0)
        {
            map_name.text = select_map.map_name + "(" + (trial_storey) + "层)";
        }
    }
    /// <summary>
    /// 判断当前地图是否为对应地图  4副本地图
    /// </summary>
    public  bool isMapType(int type)
    {
        if(select_map.map_type==type)
        {
            return true;
        }
        return false;
    }

    


    /// <summary>
    /// 显示战斗信息
    /// </summary>
    /// <param name="str"></param>
    protected void show_battle_info(string str)
    {
        if (show_info_list.Count <= 80)
        {
            show_info_item item = Instantiate(show_info_prefabs, pos_show_info);
            item.GetComponent<Text>().text = " " + str;
            show_info_list.Add(item);
        }
        else
        {
            show_info_item item = show_info_list[0];
            item.GetComponent<Text>().text = " " + str;
            item.transform.SetAsLastSibling();
            show_info_list.Remove(item);
            show_info_list.Add(item);
        }    
    }
}
