using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PanelBattleOld : PanelBase
{
    /// <summary>
    /// 怪物生成点
    /// </summary>
    private Transform m_monster_borm;
    /// <summary>
    /// 玩家生成点
    /// </summary>
    private Transform m_player_borm;
    /// <summary>
    /// 药品生成点
    /// </summary>
    private Transform m_medicine_borm;
    /// <summary>
    /// 位置预制体
    /// </summary>
    private Transform borm_prefab, m_skill_borm;
    /// <summary>
    /// 怪物位置数组
    /// </summary>
    private List<Transform> monster_borms;
    /// <summary>
    /// 玩家位置数组
    /// </summary>
    private List<Transform> player_borms;
    /// <summary>
    /// 怪物预制体
    /// </summary>
    private GameObject battle_monster_prefab;
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
    private Dictionary<medicineType, medicineitem> medicine_list;
    /// <summary>
    /// 当前地图
    /// </summary>
    private db_map_vo crt_map;

    private skill_offect_item skill_offect_item_prefab;

    private show_drop_list show_drop_list;
    public override void Hide()
    {
        base.Hide();
    }

    public override void Initialize()
    {
        base.Initialize();
        monster_list = new List<GameObject>();
        player_list = new List<GameObject>();
        monster_battle_list = new List<crtMaxBattleVO>();
        monster_Bossbattle_list = new List<crtMaxBattleVO>();
        //获取初始化部位
        monster_borms = new List<Transform>();
        m_monster_borm = Find<Transform>("monsterpos");
        borm_prefab = Tool_UI.Find_Prefabs<Transform>("Transform_item");
        player_borms = new List<Transform>();
        m_player_borm = Find<Transform>("playerpos");
        for (int i = 0; i < 3; i++)
        {
            Transform monster_borm = Instantiate(borm_prefab, m_monster_borm);
            monster_borms.Add(monster_borm);
            Transform player_borm = Instantiate(borm_prefab, m_player_borm);
            player_borms.Add(player_borm);
        }
        battle_monster_prefab = Resources.Load<GameObject>("UI/Prefabs/battle_monsters");
        battle_player_prefab = Resources.Load<GameObject>("UI/Prefabs/battle_players");
        m_medicine_borm = Find<Transform>("finishing lines");
        Medicineitem_prefab = Tool_UI.Find_Prefabs<medicineitem>("medicineitem");
        skill_offect_item_prefab = Tool_UI.Find_Prefabs<skill_offect_item>("skill_offect_item");
        show_drop_list = Find<show_drop_list>("show_drop_list");
        InitMedicine();
    }
    /// <summary>
    /// 初始化药品
    /// </summary>
    private void InitMedicine()
    {
        medicine_list = new Dictionary<medicineType, medicineitem>();
        for (int i = 0; i < Enum.GetNames(typeof(medicineType)).Length; i++)
        {
            medicineitem medicineitem = Instantiate(Medicineitem_prefab, m_medicine_borm);
            //medicineitem.Init((medicineType)i);
            medicineitem.GetComponent<Button>().onClick.AddListener(() => { medicineitem.OnClick(); });
            medicine_list.Add((medicineType)i, medicineitem);
        }
        refresh_Medicine();
    }
    /// <summary>
    /// 刷新药品
    /// </summary>
    private void refresh_Medicine()
    {
        if (SumSave.data_settings == null) SumSave.data_settings = new data_settings_vo();
        foreach (medicineType item in medicine_list.Keys)
        {
            for (int i = 0; i < SumSave.data_settings.medicine_settings_list.Count; i++)
            {
                (medicineType, string, int) medicine_settings = SumSave.data_settings.medicine_settings_list[i];

                if (medicine_settings.Item1 == item)
                {
                    //查看是否存在药品
                    //刷新显示效果
                    //medicine_list[item].DicInit(medicine_settings.Item2, medicine_settings.Item3);
                }
            }
        }
    }

    public override void Show()
    {
        base.Show();
    }
    /// <summary>
    /// 开启地图
    /// </summary>
    /// <param name="map"></param>
    public void GoMap(db_map_vo map)
    {
        crt_map = map;
        InitMap();
        crate_player();
        StartCoroutine(Game_WaitTime(crt_map.map_cd[crt_map.GetMapIntensityDrop - 1]));
    }
    /// <summary>
    /// 清空场景
    /// </summary>
    private void InitMap()
    {
        for (int i = 0; i < monster_list.Count; i++)
        {
            monster_list[i].GetComponent<BattleHealthState>().Clear();
        }
        for (int i = 0; i < player_list.Count; i++)
        {
            player_list[i].GetComponent<BattleHealthState>().Clear();
        }
        monster_list.Clear();
        player_list.Clear();
        monster_battle_list = Obtain_Monster_MaxBattle(crt_map.map_monster[crt_map.GetMapIntensityDrop - 1]);
        monster_Bossbattle_list = Obtain_Monster_MaxBattle(crt_map.map_boss[crt_map.GetMapIntensityDrop - 1]);
    }
    /// <summary>
    /// 生成怪物属性
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private List<crtMaxBattleVO> Obtain_Monster_MaxBattle(string value)
    {
        List<crtMaxBattleVO> list = new List<crtMaxBattleVO>();
        string[] monsters = value.Split(',');
        for (int i = 0; i < monsters.Length; i++)
        {
            if (!string.IsNullOrEmpty(monsters[i]))
            {
                crtMaxBattleVO monster = ArrayHelper.Find(SumSave.db_monsters, e => e.crt_name == (monsters[i]));
                if (monster != null)
                {
                    list.Add(monster);
                }
            }
        }
        return list;
    }
    /// <summary>
    /// 清除怪物
    /// </summary>
    /// <param name="health"></param>
    protected void clearSumhealth(BattleHealthState health)
    {
        if (health.GetComponent<BaseBattleAttack>() != null)
        {
            switch (health.GetComponent<BaseBattleAttack>().Data.type)
            {
                case Battle_Game_Type.player:
                case Battle_Game_Type.call:
                    player_list.Remove(health.gameObject);
                    health.Clear();
                    break;
                case Battle_Game_Type.monster:
                case Battle_Game_Type.Boss:
                case Battle_Game_Type.Activity_Monster:
                    monster_list.Remove(health.gameObject);
                    health.Clear();
                    break;
            }
        }
    }

    /// <summary>
    /// 生成技能
    /// </summary>
    /// <param name="p_brom"></param>
    /// <param name="exist"></param>
    /// <returns></returns>
    private List<skill_offect_item> Show_Battle_Skill(Transform p_brom, bool exist = false)
    {
        ClearObject(p_brom, medicine_list.Count);
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
    /// 生成玩家
    /// </summary>
    private void crate_player()
    {
        Transform pos = player_borms[1];
        GameObject item = ObjectPoolManager.instance.GetObjectFormPool(SumSave.crtMaxBattle.crt_name, battle_player_prefab,
            new Vector3(pos.position.x, pos.position.y, pos.position.z), Quaternion.identity, pos);
        item.GetComponent<BaseBattleAttack>().Data = SumSave.crtMaxBattle;
        item.GetComponent<BaseBattleAttack>().Refresh_Skill(Show_Battle_Skill(m_medicine_borm));
        player_list.Add(item);
    }

    private IEnumerator Game_WaitTime(float time)
    {
        while (time > 0)
        {
            time -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        Crate_Monster();
        StartCoroutine(Game_WaitTime(crt_map.map_cd[crt_map.GetMapIntensityDrop - 1]));


    }
    /// <summary>
    /// 生成怪物
    /// </summary>
    private void Crate_Monster()
    {

        Transform pos = monster_borms[Random.Range(0, monster_borms.Count)];//monster_borms[0]; 
        crtMaxBattleVO monster = monster_battle_list[Random.Range(0, monster_battle_list.Count)];
        GameObject item = ObjectPoolManager.instance.GetObjectFormPool(monster.crt_name, battle_monster_prefab,
            new Vector3(pos.position.x, pos.position.y + Random.Range(0, 100), pos.position.z), Quaternion.identity, pos);
        item.GetComponent<BaseBattleAttack>().Data = monster;
        item.GetComponent<LandingController>().Init(monster.data.move_speed);
        monster_list.Add(item);
        //show_drop_list.Init(crt_map, monster.type);
        //测试掉落
        //show_drop_list.Init(crt_map, Battle_Game_Type.Boss);

    }

}
