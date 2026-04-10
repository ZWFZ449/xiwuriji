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

public class panel_EndlessBattle : Panel_Base
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
    /// 列表位置
    /// </summary>
    private Transform pos_btn, pos_show_info;
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
    [HideInInspector]
    public Button close_battle;
    /// <summary>
    /// 当前出现怪物数量 总怪物数量 试练塔层数
    /// </summary>
    private int crt_monster_number = 0, maxnumber = 0, trial_storey = -1, kill_monster_number=0;
    /// <summary>
     //对象池
    /// </summary>
    private List<BattleHealth> players=new List<BattleHealth>(), monsters=new List<BattleHealth>();
    /// <summary>
    /// 全部对象
    /// </summary>
    private List<BattleHealth> sumhealths = new List<BattleHealth>();
    private bool openRefreshStatus = true;
    /// <summary>
    /// 刷怪间隔
    /// </summary>
    private float wait_time = 3f;
    /// <summary>
    /// 获取面板的宽度
    /// </summary>
    private float width;
    protected override void Awake()
    {
        base.Awake();
        Combat_statistics.Init();
        width = GetComponent<RectTransform>().rect.width;
    }

    public override void Initialize()
    {
        base.Initialize();
        pos_monster = Find<Transform>("battle_pos/monster_pos");
        pos_player = Find<Transform>("battle_pos/player_pos");
        //player_battle_attack_prefabs = Resources.Load<GameObject>("Prefabs/prefab/player_battle_attck_item"); ; //Battle_Tool.Find_Prefabs<GameObject>("player_battle_attck_item")
        //monster_battle_attack_prefabs = Resources.Load<GameObject>("Prefabs/prefab/monster_battle_attck_item");// Battle_Tool.Find_Prefabs<GameObject>("monster_battle_attck_item"); 
        player_battle_attack_prefabs = Resources.Load<GameObject>("Prefabs/prefab/endlessplayer_battle_attck_item");
        monster_battle_attack_prefabs = Resources.Load<GameObject>("Prefabs/prefab/endiessmonster_battle_attck_item");
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
        battle_info_list = Find<Text>("Combat_statistics/info");
    }
    /// <summary>
    /// 初始状态
    /// </summary>
    private void clear_Combat_statistics()
    {
        Alert.Show("清空战斗信息", "是否清空战斗信息", Clear_Combat_statistics);

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
    /// 播放完死亡动画后完全回收
    /// </summary>
    /// <param name="health"></param>
    protected void clearSumhealth(BattleHealth health)
    {
        sumhealths.Remove(health);
    }


    /// <summary>
    /// 显示战斗信息
    /// </summary>
    public void Show_Combat_statistics()
    {
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
            //Debug.Log(players[i].GetComponent<BattleAttack>().Data.show_name + " " + hp);
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
            if (attack.Data.Heal_Hp > 0 || attack.Data.Heal_Mp > 0)
            {
                list[i].HealConsumables(attack.Data.Heal_Hp, attack.Data.Heal_Mp);
            }
        }
    }

    /// <summary>
    /// 关闭列表
    /// </summary>
    private void Close()
    {
        Alert.Show("退出无尽深渊", "请确认是否退出无尽深渊",Hide);
      
    }

    private void Hide(object arg0)
    {
        Hide();

    }

    public override void Show()
    {
        base.Show();
        AudioManager.Instance.ChangeBGM(BGMenum.开启);
    }
  

    public override void Hide()
    {
        base.Hide();
        settlement();
    }
    /// <summary>
    /// 结算收益
    /// </summary>
    private void settlement()
    {
        List<(string, int)> list = SumSave.crt_needlist.SetMap();
        bool exist = true;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Item1 == select_map.map_name)
            {
                exist = false;
                Alert.Show("挑战结束","今日无尽试炼已获得过奖励"+ "\n本次击杀 "+kill_monster_number+"只怪物");

                break;
            }
        }
        if (exist)
        {
            SumSave.crt_needlist.SetMap((select_map.map_name, 1));
            if (kill_monster_number > 0)
            {
                long exp= (long)(kill_monster_number * 10000);//经验
                int plint= (kill_monster_number * 500);//历练值
                string dec = select_map.map_name+"\n";
                dec+="本次击杀 "+kill_monster_number+" 只怪物\n";
                dec += "获得经验 " + exp + "\n";
                dec += "获得历练值 " + plint + "\n";
                Battle_Tool.Obtain_Exp(exp, 2);
                Battle_Tool.Obtain_Unit(currency_unit.元宝, plint);

                Dictionary<string, int> dic = new Dictionary<string, int>();
                for (int i = 0; i < SumSave.db_EndlessBattle_list.Count; i++)
                {
                    int max= kill_monster_number/ SumSave.db_EndlessBattle_list[i].need_number;
                    if (max > 0)
                    {
                        max = Mathf.Min(max, SumSave.db_EndlessBattle_list[i].max_number);
                        for (int j = 0; j < max; j++)
                        {
                            string value = SumSave.db_EndlessBattle_list[i].goods[Random.Range(0, SumSave.db_EndlessBattle_list[i].goods.Length)];
                            if(!dic.ContainsKey(value))
                                dic.Add(value, 0);
                            dic[value]++;
                        }
                    }
                }
                foreach (var item in dic.Keys)
                {
                    dec+="获得"+item+"个"+dic[item]+"\n";
                    int random = Random.Range(1, 100);
                    int number = dic[item];
                    int maxnumber = number + Random.Range(1, 100);
                    Battle_Tool.Obtain_Resources(Obtain_Int.Add(1, item, new int[] { number + random, random }), maxnumber);

                }
                Alert.Show(select_map.map_name, dec);
            }
        }
        if (kill_monster_number > 0)
        {
            Write_into_the_leaderboard(kill_monster_number);
        }
        //ObjectPoolManager.instance.ClearAll();
        //DamageTextManager.Instance.ClearAll();

    }

    /// <summary>
    /// 写入排行榜
    /// </summary>
    private void  Write_into_the_leaderboard(int _num)
    {
        if (SumSave.crt_endless_battle.endless_dic.ContainsKey(SumSave.uid))
        {
            if (_num > SumSave.crt_endless_battle.endless_dic[SumSave.uid].num)
            {
                SendNotification(NotiList.Read_EndlessBattle);
                user_endless_battle.endlsess_battle data = new user_endless_battle.endlsess_battle();
                data.endless_uid = SumSave.uid;
                data.name = SumSave.old_crt_hero.hero_name;
                data.type = SumSave.old_crt_hero.hero_pos;
                data.num = _num;
                SumSave.crt_endless_battle.AddEndless(data, true);
            }
            else
            {
                return;
            }
        }else
        {
            SendNotification(NotiList.Read_EndlessBattle);
            user_endless_battle.endlsess_battle data = new user_endless_battle.endlsess_battle();
            data.endless_uid = SumSave.uid;
            data.name = SumSave.old_crt_hero.hero_name;
            data.type = SumSave.old_crt_hero.hero_pos;
            data.num = _num;
            SumSave.crt_endless_battle.AddEndless(data, true);
        }
    }
    /// <summary>
    /// 游戏结束
    /// </summary>
    protected void Game_Over()
    {
        Hide();
    }
    
   
    /// <summary>
    /// 进入地图
    /// </summary>
    /// </summary>
    /// <param name="map"></param>
    /// <param name="storey">试炼塔开启层数</param>
    public void Open_Map(user_map_vo map)
    {
        select_map = map;
        map_name.text = map.map_name;
        init();
        Crate_Init();
        StopAllCoroutines();
        StartCoroutine(ProduceMonster(wait_time));

    }
    /// <summary>
    /// 显示战斗状态
    /// </summary>
    /// <param name="dec"></param>
    private void Show_Battle_State(string dec)
    {
        map_time.text = dec;
    }
    /// <summary>
    /// 生成角色初始化
    /// </summary>
    private void Crate_Init()
    {
        crate_Skill();
        crate_hero();
        crate_pet();
        crate_monster();
    }
    /// <summary>
    /// 创造宠物
    /// </summary>
    private void crate_pet()
    {
  
        if (SumSave.crt_world != null)
        {
            if (SumSave.crt_pet_Old!=null)
            {
                List<db_pet_vo_old> crt_pet_list = SumSave.crt_pet_Old.Set();
                foreach (db_pet_vo_old pet in crt_pet_list)
                {
                    if (pet.pet_state == "1")
                    {
                        crtMaxHeroVO crt = new crtMaxHeroVO();
                        crt.show_name = pet.petName;
                        for (int i = 0; i < pet.crate_values.Count; i++)
                        {
                            if (pet.crate_values[i] != "" && pet.up_values[i] != "")
                            {
                                int info = int.Parse(pet.crate_values[i]) + (int.Parse(pet.up_values[i]) * pet.level);
                                Battle_Tool.Enum_Value(crt, i, info);
                            }
                        }
                        GameObject item = ObjectPoolManager.instance.GetObjectFormPool("无尽"+crt.show_name, player_battle_attack_prefabs,
                        new Vector3(pos_player.position.x, pos_player.position.y+200, pos_player.position.z), Quaternion.identity, pos_player);
                        // 设置Data
                        crt.Push_name = "无尽" + crt.show_name;
                        item.GetComponent<endlessplayer_battle_attck>().Data = crt;
                        item.GetComponent<endlessplayer_battle_attck>().Refresh_Skill(new List<oldskill_offect_item>());
                        item.GetComponent<BattleAttack>().FindTergets(monsters);
                        item.GetComponent<BattleAttack>().StateMachine.skil_pet(crt.show_name);
                        //if (item.GetComponent<Button>().enabled)
                        //    item.GetComponent<Button>().onClick.AddListener(delegate { AudioManager.Instance.playAudio(ClipEnum.购买物品); SelectMonster(item.GetComponent<MonsterBattleAttack>()); });
                        //item.GetComponent<Button>().enabled = true;
                        players.Add(item.GetComponent<BattleHealth>());
                        sumhealths.Add(item.GetComponent<BattleHealth>());
                        return;
                    }
                    
                }
            }
        }
       
    }

    /// <summary>
    /// 生成技能
    /// </summary>
    private void crate_Skill()
    {
        battle_skills = pight_show_skill.Init();
    }

    private void crate_hero()
    {
        int num = 0;///玩家初始化偏移量
        List<db_pet_vo_old> crt_pet_list = SumSave.crt_pet_Old.Set();
        foreach (db_pet_vo_old pet in crt_pet_list)
        {
            if (pet.pet_state == "1")
            {
                num= 220;
            }
        }
        crtMaxHeroVO crt = SumSave.crt_MaxHero_okd;
        GameObject item = ObjectPoolManager.instance.GetObjectFormPool("无尽" + crt.show_name, player_battle_attack_prefabs,
            new Vector3(pos_player.position.x, pos_player.position.y-num, pos_player.position.z), Quaternion.identity, pos_player);
        // 设置Data
        crt.Push_name = "无尽" + crt.show_name;
        item.GetComponent<BattleAttack>().Data = crt;
        item.GetComponent<BattleAttack>().Refresh_Skill(battle_skills);
        item.GetComponent<BattleAttack>().FindTergets(monsters);
        //if (item.GetComponent<Button>().enabled)
        //    item.GetComponent<Button>().onClick.AddListener(delegate { AudioManager.Instance.playAudio(ClipEnum.购买物品); SelectMonster(item.GetComponent<MonsterBattleAttack>()); });
        //item.GetComponent<Button>().enabled = true;
        players.Add(item.GetComponent<BattleHealth>());
        sumhealths.Add(item.GetComponent<BattleHealth>());
        role_health.SetHealth(item.GetComponent<BattleHealth>());
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void init()
    {
        crt_monster_number = 0;
        kill_monster_number = 0;
        maxnumber = 100;
        wait_time = 3f;
        switch (select_map.map_type)
        {
            case 7: maxnumber = 9999; break;
            default:
                break;
        }
        for (int i = 0; i < sumhealths.Count; i++)
        {

            sumhealths[i].Clear();
        }
        monsters.Clear();
        players.Clear();


        //if (monsters != null)
        //{
        //    for (int i = pos_monster.childCount - 1; i >= 0; i--)
        //    {
        //        pos_monster.GetChild(i).gameObject.GetComponent<BattleHealth>().Clear();
        //    }
        //    monsters.Clear();
        //}else monsters=new List<BattleHealth>();
        //if (players != null)
        //{
        //    for (int i = pos_player.childCount - 1; i >= 0; i--)
        //    {
        //        pos_player.GetChild(i).gameObject.GetComponent<BattleHealth>().Clear();
        //    }
        //    players.Clear();
        //}else players=new List<BattleHealth>();

        crt_map_monsters.Clear();
        if (select_map.map_type == 7)
        {
            for (int i = 0; i < SumSave.olddb_monsters.Count; i++)
                crt_map_monsters.Add(SumSave.olddb_monsters[i]);
        }
    }
    // 下一波怪
    protected void Game_Next_Map()
    {
        
    }

    private void Update()
    {
        if (openRefreshStatus) RefreshStatus();
    }

    private void RefreshStatus()
    {
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
        if (monsters != null)
        {
            if (monsters.Count > 0)
            {
                for (int i = 0; i < monsters.Count; i++)
                {
                    monsters[i].GetComponent<BattleAttack>().FindTergets(players);
                }
            }
        }
        openRefreshStatus = false;
    }
    /// <summary>
    /// 清除死亡物品
    /// </summary>
    /// <param name="health"></param>
    protected void clearhealth(BattleHealth health)
    {
        if (health.GetComponent<BattleAttack>().Data.Monster_Lv == 0)
        {
            players.Remove(health);
        }
        else
        {
            //击杀一个怪物
            kill_monster_number++;
            monsters.Remove(health);
        } 
        openRefreshStatus = true;
    }
    private IEnumerator ProduceMonster(float time)
    {
        float basetime = 0.1f;
       
        while (time>0)
        {
            time-=basetime;
            Show_Battle_State("刷新时间 " + time.ToString("F1") + "s");
            yield return new WaitForSeconds(basetime);
        }
        crate_monster();
        StartCoroutine(ProduceMonster(wait_time));
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
        //}
        //else
        //{
        //    AddWeather();
        //}
        if (crt_monster_number >= maxnumber) crt_monster_number = 0;
        crt_monster_number++;
        //每100个怪减少刷新cd
        if (crt_monster_number % 200 == 0) wait_time -= 0.1f;
        wait_time = Mathf.Max(0.1f, wait_time);
        int dic=Random.Range(0,100)>50?1:-1;

        float y = pos_monster.position.y + (width / 20 * Random.Range(0, 10)) * dic;
        if (y >  (width-300 )||y<100) y = pos_monster.position.y;
        crtMaxHeroVO crt = crt_map_monsters[Random.Range(0, crt_map_monsters.Count)];
        crt = Battle_Tool.crate_monster(crt, select_map, crt_monster_number / 10 + 1);

        GameObject item = ObjectPoolManager.instance.GetObjectFormPool("无尽" + crt.show_name, monster_battle_attack_prefabs,
            new Vector3(pos_monster.position.x, y, pos_monster.position.z), Quaternion.identity, pos_monster);
        // 设置Data   
        crt.Push_name = "无尽"+ crt.show_name;
        item.GetComponent<BattleAttack>().Data = crt;
        item.GetComponent<BattleAttack>().FindTergets(players, Random.Range(0, 100) < 3 ? 1 : 0);//百分之3的概率为背刺怪

#if UNITY_EDITOR
        //item.GetComponent<BattleAttack>().FindTergets(players, Random.Range(0, 100) < 300 ? 1 : 0);//百分之3的概率为背刺怪
#endif



        //点击怪物
        //if (item.GetComponent<Button>().enabled)
        //    item.GetComponent<Button>().onClick.AddListener(delegate { AudioManager.Instance.playAudio(ClipEnum.购买物品); SelectMonster(item.GetComponent<BattleAttack>()); });
        //item.GetComponent<Button>().enabled = true;
        sumhealths.Add(item.GetComponent<BattleHealth>());
        monsters.Add(item.GetComponent<BattleHealth>());
        //Open_Monster_State = true;
        openRefreshStatus = true;
        ShowInfoMap();   
    }
    /// <summary>
    /// 锁定怪物 增加目标
    /// </summary>
    /// <param name="battleAttack"></param>
    private void SelectMonster(BattleAttack battleAttack)
    {
        
    }

    private void ShowInfoMap()
    {
        map_name.text = select_map.map_name + "(" + crt_monster_number + "/" + maxnumber + ")";
    }
    /// <summary>
    /// 判断当前地图是否为对应地图  4副本地图
    /// </summary>
    public bool isMapType(int type)
    {
        if (select_map.map_type == type)
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
