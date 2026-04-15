using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MVC
{
    public class BaseBattleAttack : Base_Mono
    {
        /// <summary>
        /// 自身血量
        /// </summary>
        protected BattleHealthState oneselfHealthState;

        protected List<skill_offect_item> battle_skills;

        /// <summary>
        /// 目标
        /// </summary>
        protected BattleHealthState Terget;

        private Text base_name;

        private Image base_icon;
        protected virtual void Awake()
        {
            Init();
        }
        private float attack_speed = 0;
        private void Update()
        {
            if (data != null)//判断是否有怪物
            {
                if (Terget != null)
                {
                    if (Terget.isDead  || !Terget.gameObject.activeInHierarchy) Find_Terget();
                }
                else Find_Terget();
            }
        }
        private IEnumerator timer()
        {   
            while (true)
            {
                yield return new WaitForSeconds(0.05f);
                if (Terget != null)
                {
                    if (Vector3.Distance(Terget.transform.position, transform.position) < data.data.battle_range * 2) attack_speed += 5;
                    if (attack_speed >= data.data.battle_speed) { attack_speed = 0; OnAuto(); }
                }
                if (data.type == Battle_Game_Type.player)
                {
                    //实时监控血量变化
                    transform.parent.parent.parent.parent.SendMessage("Anto_Use_Medicine", oneselfHealthState);
                }
            }
        }

        public void Lose_Terget()
        {
            Find_Terget();
        }

        /// <summary>
        /// 寻找攻击对象
        /// </summary>
        protected virtual void Find_Terget()
        {
            Terget= null;
            string TergetTag = "null";
            switch (data.type)
            {
                case Battle_Game_Type.player:
                case Battle_Game_Type.call:
                    TergetTag = "Monster";
                    break;
                case Battle_Game_Type.monster:
                case Battle_Game_Type.Boss:
                case Battle_Game_Type.Activity_Monster:
                    TergetTag="Player";
                    break;
            }
            List<BattleHealthState> monsterList = new List<BattleHealthState>();
            GameObject[] monsters = GameObject.FindGameObjectsWithTag(TergetTag);
            if (monsters != null && monsters.Length > 0)
            {
                for (int i = 0; i < monsters.Length; i++)
                {
                    BattleHealthState monster = monsters[i].GetComponent<BattleHealthState>();
                    //消失或者死亡
                    if (!monster.gameObject.activeInHierarchy || monster.isDead) continue;
                    monsterList.Add(monster);
                    //int distance = (int)Vector3.Distance(monster.transform.position, transform.position);
                    ////Debug.Log("距离"+distance);
                    //if (Vector3.Distance(monster.transform.position, transform.position) < data.data.battle_range * 4) monsterList.Add(monster);
                }
            }
            if (monsterList.Count > 0)
            {
                Terget = ArrayHelper.GetMin(monsterList, (x) => Vector3.Distance(x.transform.position, transform.position));
                //是否移动
                OnCShase();
            }
        }
        /// <summary>
        /// 自动追逐
        /// </summary>
        protected virtual void OnCShase()
        {
            if (GetComponent<LandingController>() != null)
            {
                GetComponent<LandingController>().OnCShase(Terget.transform, data.data.battle_range * 2, data.data.move_speed);
            }
        }

        protected virtual void Init()
        {
            base_name = Find<Text>("icon/bg_name/base_name");
            base_icon = Find<Image>("icon/base_icon");
            oneselfHealthState=GetComponent<BattleHealthState>();
        }
        /// <summary>
        /// 最终战斗数据
        /// </summary>
        protected crtMaxBattleVO data;
        /// <summary>
        /// Data
        /// </summary>
        public crtMaxBattleVO Data 
        {
            set
            {
                data = value;
                base_Init();
            }
            get
            {
                return data;
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void base_Init()
        {
            base_name.text = data.crt_name;
            switch (data.type)
            {
                case Battle_Game_Type.player:
                    base_icon.sprite = UI.UI_Manager.I.GetEquipSprite("UI/player/", data.hero_type+"头像");
                    break;
                case Battle_Game_Type.call:
                    base_icon.sprite = UI.UI_Manager.I.GetEquipSprite("UI/player/Call", data.crt_name);
                    break;
                case Battle_Game_Type.monster:
                case Battle_Game_Type.Boss:
                case Battle_Game_Type.Activity_Monster:
                    base_icon.sprite = UI.UI_Manager.I.GetEquipSprite("monster/", data.crt_name);
                    break;
            }
            //base_icon.sprite = UI.UI_Manager.I.GetEquipSprite("monster/", Random.Range(1,5));
            oneselfHealthState.Init(data.data.battle_maxhp, data.data.battle_maxmp, data.crt_name);
            StopAllCoroutines();
            StartCoroutine(timer());
        }

        public virtual void Refresh(crtMaxBattleVO hero)
        {

        }

        public virtual void Refresh_Skill(List<skill_offect_item> skills)
        {
            battle_skills = skills;
        }
        /// <summary>
        /// 对目标造成伤害
        /// </summary>
        /// <param name="skill"></param>
        public void skill_damage(db_skill_vo skill)
        {
            if (Terget == null) return;
            BaseBattleAttack monster = Terget.GetComponent<BaseBattleAttack>();
            if (monster.oneselfHealthState.isDead) return;//结战斗
            float damage = Base_Damage(monster);
            Dictionary<int, db_skill_vo> skill_list = SumSave.crt_skill.Set_Current_skill();
            int skilldamage = 0;
            foreach (var item in skill_list)
            {
                int skill_lv = item.Value.SetLv();
                if (skill_lv > 0)
                {
                    if (item.Value.skill_offect_value_list.Count > 0)
                    {
                        foreach (enum_equip_entry_list skill_effect_type in item.Value.skill_offect_value_list.Keys)
                        {
                            int value = item.Value.skill_offect_value_list[skill_effect_type][skill_lv];
                            switch (skill_effect_type)
                            {
                                default:
                                    if ((int)skill_effect_type > 1000)
                                    {
                                        db_skill_vo base_skill = ArrayHelper.Find(SumSave.db_skills, (x) => x.id == ((int)skill_effect_type - 1000));
                                        if (base_skill != null)
                                        {
                                            if (skill == base_skill)
                                            {
                                                skilldamage+= value;
                                            }
                                        }
                                    }
                                    break;
                            }

                        }
                    }
                }
            }
            //真实伤害
            if ((Skill_Effect_Type)skill.EffectType == Skill_Effect_Type.单体 || (Skill_Effect_Type)skill.EffectType == Skill_Effect_Type.群体)
            {
                skilldamage += skill.Power + skill.DefPowers[skill.SetLv()];
                if (skill.skill_damages.Count> skill.SetLv()) damage += skill.skill_damages[skill.SetLv()];
                damage = damage * (skilldamage) / 100;
            }
            switch ((Skill_Effect_Type)skill.EffectType)
            {
                case Skill_Effect_Type.单体:
                    TakeDamage((int)damage, monster);
                    break;
                case Skill_Effect_Type.群体:
                    List<BattleHealthState> monsterList = FindTheTarget(skill, monster);
                    for (int i= 0; i < monsterList.Count; i++) 
                    {
                        BaseBattleAttack base_monster = monsterList[i].gameObject.GetComponent<BaseBattleAttack>();
                        if (base_monster != null)
                        {
                            int base_damage = Base_Damage(base_monster) * (skilldamage) / 100;
                            if (skill.skill_damages.Count > skill.SetLv()) base_damage += skill.skill_damages[skill.SetLv()];
                            if (base_damage < 0) base_damage = 1;
                            TakeDamage(base_damage, base_monster);
                        }
                    }
                    break;
                case Skill_Effect_Type.回复:
                    List<BattleHealthState> player = FindTheTarget();
                    int value = Lucky(Data.data.sc, Data.data.sc2, data.data.lucky);
                    value = value * (skilldamage) / 100;
                    if (skill.skill_damages.Count > skill.SetLv()) value += skill.skill_damages[skill.SetLv()];
                    for (int i = 0; i < player.Count; i++)
                    {
                        player[i].Use_Medicine(value, 0);
                    }
                    break;
                case Skill_Effect_Type.状态:
                    TakeDamage((int)damage, monster);
                    break;
                case Skill_Effect_Type.护盾:
                    break;
                case Skill_Effect_Type.召唤:
                    break;
                default:
                    break;
            }
        }
        private List<BattleHealthState> FindTheTarget(db_skill_vo skill, BaseBattleAttack base_monster)
        {
            string TergetTag = "Monster";
             
            List<BattleHealthState> monsterList = new List<BattleHealthState>();
            GameObject[] monsters = GameObject.FindGameObjectsWithTag(TergetTag);
            if (monsters != null && monsters.Length > 0)
            {
                for (int i = 0; i < monsters.Length; i++)
                {
                    BattleHealthState monster = monsters[i].GetComponent<BattleHealthState>();
                    //消失或者死亡
                    if (!monster.gameObject.activeInHierarchy || monster.isDead) continue;
                    switch (skill.MoveType)
                    {
                        case 3:
                            if (Vector3.Distance(monster.transform.position, base_monster.transform.position) < skill.scope) monsterList.Add(monster);
                            break;
                        case 4:
                            if (Vector3.Distance(monster.transform.position, transform.position) < skill.scope) monsterList.Add(monster);
                            break;
                        default:
                            break;
                    }
                }
            }
            return monsterList;
        }
        /// <summary>
        /// 寻找自身
        /// </summary>
        /// <returns></returns>
        private List<BattleHealthState> FindTheTarget()
        {
            string TergetTag = "Player";
            List<BattleHealthState> monsterList = new List<BattleHealthState>();
            GameObject[] monsters = GameObject.FindGameObjectsWithTag(TergetTag);
            if (monsters != null && monsters.Length > 0)
            {
                for (int i = 0; i < monsters.Length; i++)
                {
                    BattleHealthState monster = monsters[i].GetComponent<BattleHealthState>();
                    //消失或者死亡
                    if (!monster.gameObject.activeInHierarchy || monster.isDead) continue;
                    monsterList.Add(monster);
                }
            }
            return monsterList;
        }

        public virtual void OnAuto()
        {
            base_icon.color = Show_Color.Set_Color(Color_list.红色);
            StartCoroutine(HideFrame());
        }

        /// <summary>
        /// 关闭光环
        /// </summary>
        /// <returns></returns>
        private IEnumerator HideFrame()
        {
            yield return new WaitForSeconds(0.2f);
            base_icon.color=Show_Color.Set_Color(Color_list.白色);
        }

        protected virtual void BaseAttack()//判断伤害
        {
            if (Terget == null) return;
            AudioManager.Instance.playAudio(ClipEnum.攻击敌人);
            BaseBattleAttack monster = Terget.GetComponent<BaseBattleAttack>();
            if (monster.oneselfHealthState.isDead) return;//结战斗
            int damage = Base_Damage(monster);
            TakeDamage(damage, monster);
        }
        /// <summary>
        /// 统一伤害格式
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="monster"></param>
        private void TakeDamage(int damage, BaseBattleAttack monster)
        {
            if (iSnHit(monster))
            {
                //传递消息，未命中;
                monster.oneselfHealthState.TakeDamage(1, DamageEnum.技能未命中);
                return;
            }
            bool isCrit = isCrate(monster);
            if (isCrit)
            {
                damage = damage * data.data.critDmg / 100;
            }
            foreach (var item in monster.data.data.buffList)
            {
                switch (item.Item1)
                {
                    case enum_battle_pet_talent_list.任意门:
                        break;
                    case enum_battle_pet_talent_list.嗜血追击:
                        break;
                    case enum_battle_pet_talent_list.连击:
                        break;
                    case enum_battle_pet_talent_list.法连:
                        break;
                    case enum_battle_pet_talent_list.道连:
                        break;
                    case enum_battle_pet_talent_list.反震:
                    case enum_battle_pet_talent_list.反弹:
                        if (Random.Range(0, 100) < item.Item2)
                        {
                            int value = (int)(damage * item.Item3 / 100);
                            value = (int)MathF.Max(1, value);
                            oneselfHealthState.TakeDamage(value, DamageEnum.普通伤害);
                        }
                        break;
                    case enum_battle_pet_talent_list.防爆:

                        break;
                    case enum_battle_pet_talent_list.招架:
                        if (Random.Range(0, 100) < item.Item2)
                        {
                            damage = damage * (100 - item.Item3) / 100;
                        }
                        break;
                    case enum_battle_pet_talent_list.慧根:
                        break;
                    default:
                        break;
                }
            }
            damage = (int)MathF.Max(1, damage);

            monster.oneselfHealthState.TakeDamage((int)damage, isCrit ? DamageEnum.暴击技能伤害 : DamageEnum.技能伤害);
            if (data.data.battle_Damage > 0)
            {
                monster.oneselfHealthState.TakeDamage(data.data.battle_Damage, DamageEnum.真实伤害);
            }
            foreach (var item in monster.data.data.buffList)
            {
                switch (item.Item1)
                {
                    case enum_battle_pet_talent_list.任意门:
                        break;
                    case enum_battle_pet_talent_list.嗜血追击:
                        if (monster.oneselfHealthState.isDead)
                        {
                            if (oneselfHealthState.Proportion().Item1 >= 40)
                            {
                                Find_Terget();
                                if (Terget != null)
                                {
                                    Terget.GetComponent<BaseBattleAttack>().oneselfHealthState.TakeDamage((int)damage, DamageEnum.普通伤害);
                                    oneselfHealthState.TakeDamage((int)(data.data.battle_maxhp / 10), DamageEnum.普通伤害);
                                }
                            }
                        }
                        break;
                    case enum_battle_pet_talent_list.连击:
                    case enum_battle_pet_talent_list.法连:
                    case enum_battle_pet_talent_list.道连:
                        if (SumSave.crtHero.job + 6 == (int)item.Item1)
                        {
                            if (Random.Range(0, 100) < item.Item2)
                            {
                                int value = (int)(damage * item.Item3 / 100);
                                value = (int)MathF.Max(1, value);
                                monster.oneselfHealthState.TakeDamage((int)value, DamageEnum.普通伤害);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private int Base_Damage(BaseBattleAttack monster)
        {
            int damage = 0;
            damage = defense(monster, data.hero_type);

            return damage;
        }

        /// <summary>
        /// 计算伤害
        /// </summary>
        /// <param name="monster">对象</param>
        /// <param name="type">类型</param>
        /// <param name="isBackstab">是否背刺</param>
        /// <returns></returns>
        private int defense(BaseBattleAttack monster, Hero_Type type, int isBackstab=1)
        {
            int damage = 0;
            int def = 0;
            switch (type)
            {
                case Hero_Type.平民:
                    damage = Lucky(Data.data.dc, Data.data.dc2, data.data.lucky);
                    def = Random.Range(monster.Data.data.ac, monster.Data.data.ac2);
                    break;
                case Hero_Type.战士:
                    damage = Lucky(Data.data.dc, Data.data.dc2, data.data.lucky);
                    def = Random.Range(monster.Data.data.ac, monster.Data.data.ac2);
                    break;
                case Hero_Type.法师:
                    damage = Lucky(Data.data.mc, Data.data.mc2, data.data.lucky);
                    def = Random.Range(monster.Data.data.mac, monster.Data.data.mac2);
                    break;
                case Hero_Type.道士:
                    damage = Lucky(Data.data.sc, Data.data.sc2, data.data.lucky);
                    def = Random.Range(monster.Data.data.mac, monster.Data.data.mac2);
                    break;
            }
            foreach (var item in data.data.buffList)
            {
                switch (item.Item1)
                {
                    case enum_battle_pet_talent_list.任意门:
                        break;
                    case enum_battle_pet_talent_list.嗜血追击:
                        break;
                    case enum_battle_pet_talent_list.破壁一击:
                        if (Random.Range(0, 100) < item.Item2)
                        {
                            def = 0;
                        }
                        break;
                    case enum_battle_pet_talent_list.华山斩:
                        if (Random.Range(0, 100) < item.Item2)
                        {
                            damage = damage * item.Item3;
                        }
                        break;
                    case enum_battle_pet_talent_list.斩杀:
                        if (monster.GetComponent<BattleHealthState>().Proportion().Item1 <= item.Item2)
                        {
                            damage = (int)(monster.data.data.battle_maxhp / 5);
                        }
                        break;
                    case enum_battle_pet_talent_list.连击:
                        break;
                    case enum_battle_pet_talent_list.法连:
                        break;
                    case enum_battle_pet_talent_list.道连:
                        break;
                    case enum_battle_pet_talent_list.战旗:
                    case enum_battle_pet_talent_list.法旗:
                    case enum_battle_pet_talent_list.道旗:
                        if (SumSave.crtHero.job + 9 == (int)(item.Item1))
                        {
                            if (Random.Range(0, 100) < item.Item2)
                                def -= item.Item3;
                        }
                        break;
                    case enum_battle_pet_talent_list.反震:
                        break;
                    case enum_battle_pet_talent_list.防爆:
                        break;
                    case enum_battle_pet_talent_list.招架:
                        break;
                    case enum_battle_pet_talent_list.反弹:
                        break;
                    case enum_battle_pet_talent_list.慧根:
                        break;
                    default:
                        break;
                }
            }
            damage = damage - def;
            if (damage <= 0) damage = 1;
            return damage;
        }
        private bool iSnHit(BaseBattleAttack monster)
        {
            bool exist = false;
            if (Data.data.hit < monster.Data.data.dodge && Random.Range(0, 100) > 10)//命中不达标也有10%的概率
            {
                //传递消息，未命中;
                exist = true;
            }
            return exist;
        }

        /// <summary>
        /// 判断是否暴击
        /// </summary>
        /// <param name="monster"></param>
        /// <returns></returns>
        private bool isCrate(BaseBattleAttack monster)
        {
            bool isCrit = false;
            int value = 0;
            foreach (var item in monster.data.data.buffList)
            {
                switch (item.Item1)
                {
                    case enum_battle_pet_talent_list.任意门:
                        break;
                    case enum_battle_pet_talent_list.嗜血追击:
                        break;
                    case enum_battle_pet_talent_list.连击:
                        break;
                    case enum_battle_pet_talent_list.法连:
                        break;
                    case enum_battle_pet_talent_list.道连:
                        break;
                    
                    case enum_battle_pet_talent_list.防爆:
                        value = item.Item3;
                        break;
                   
                    case enum_battle_pet_talent_list.慧根:
                        break;
                    default:
                        break;
                }
            }

            if (Random.Range(0, 100) < data.data.crit - value)
            {
                isCrit = true;
            }
            return isCrit;
        }

        /// <summary>
        /// 幸运加成
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="lucky"></param>
        private int Lucky(int min, int max, int lucky)
        {
            int value = 0;
            value = Random.Range(min + (max - min) * lucky / 10, max);
            if (lucky > 10)
            {
                value = value * (100 + (lucky * 10)) / 100;
            }
            return value;
        }
    }
    /// <summary>
    /// 战斗模式
    /// </summary>
    public class BaseBattleAttacks : Base_Mono
    {
        /// <summary>
        /// 获取血值
        /// </summary>
        [HideInInspector]
        public BattleHealth target;
        /// <summary>
        /// 目标
        /// </summary>
        protected BattleHealth Terget;
        /// <summary>
        /// 目标列表
        /// </summary>
        protected List<BattleHealth> Tergets;
        /// <summary>
        /// 获取目标
        /// </summary>
        /// <param name="tergets"></param>
        public void FindTergets(List<BattleHealth> tergets, int isBackstab = 0)
        {
            Tergets = tergets;
            if (isBackstab == 1)
            {
                data.isBackstab = 1;
                StateMachine.Backstab(isBackstab);
                Debug.Log("背刺");
            }
        }
        protected Image frame, icon;

        protected Image targetIcon;
        /// <summary>
        /// 技能列表
        /// </summary>
        protected List<oldskill_offect_item> battle_skills;
        /// <summary>
        /// 名称 称号
        /// </summary>
        protected TMP_Text Name, sliderInfo, damageInfo;
        /// <summary>
        /// 计数器
        /// </summary>
        protected Slider show_hp;
        /// <summary>
        /// 生命值显示文本
        /// </summary>
        protected TMP_Text hp_text;

        /// <summary>
        /// 角色状态机
        /// </summary>
        public AttackStateMachine AttackStateMachine;
        public RolesManage StateMachine;
        private TMP_Text name_text;

        /// <summary>
        /// 天命台父物体大小,当前天命大小
        /// </summary>
        private Vector2 pos_tianming_size, tianming_size;
        /// <summary>
        /// 刷新属性
        /// </summary>
        /// <param name="hero"></param>
        public virtual void Refresh(crtMaxHeroVO hero)
        {

        }


        public virtual void Refresh_Skill(List<oldskill_offect_item> skills)
        {

        }
        public virtual void Awake()
        {
            target = GetComponent<BattleHealth>();
            AttackStateMachine = GetComponent<AttackStateMachine>();
            StateMachine = GetComponent<RolesManage>();
            frame = Find<Image>("frame");
            show_hp = Find<Slider>("Slider");
            name_text = Find<TMP_Text>("base_info/info");
            hp_text = Find<TMP_Text>("Slider/Hp_text");
        }

        /// <summary>
        /// 天命台位置
        /// </summary>
        private Transform show_tianming_Platform;

        protected crtMaxHeroVO data;
        /// <summary>
        /// Data
        /// </summary>
        public crtMaxHeroVO Data
        {
            set
            {
                Tergets = new List<BattleHealth>();
                data = value;
                if (data == null) return;
                frame.gameObject.SetActive(false);
                target.maxHP = data.MaxHP;
                target.HP = data.MaxHP;
                target.EnergymaxMp = data.EnergyMp;
                target.internalforcemaxMP = data.internalforceMP;
                target.internalforceMP = data.internalforceMP;
                show_hp.maxValue = target.maxHP;
                show_hp.value = target.HP;
                hp_text.text = Battle_Tool.FormatNumberToChineseUnit(target.HP) + "/" + Battle_Tool.FormatNumberToChineseUnit(target.maxHP);
                target.maxMP = data.MaxMp;
                target.MP = data.MaxMp;
                AttackStateMachine.isAttacking = false;
                //Terget = null;
                string dec = "";

                if (data.Monster_Lv >= 1)
                {
                    icon.sprite = Resources.Load<Sprite>("Prefabs/monsters/" + data.show_name);//Assets/Resources/mon_龙.png
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
                name_text.text = dec;

                if (GetComponent<Monster>() != null)
                {

                    show_tianming_Platform = transform.Find("Appearance/tianming_Platform");

                    int[] life = data.life;
                    for (int j = show_tianming_Platform.childCount - 1; j >= 0; j--)//清空区域内按钮
                    {
                        Destroy(show_tianming_Platform.GetChild(j).gameObject);
                    }
                    for (int i = 0; i < life.Length; i++)
                    {
                        if (life[i] > 0)///怪物天命环
                        {
                            GameObject game = Resources.Load<GameObject>("Prefabs/halo/halo_" + (i + 1));
                            GameObject tianming = Instantiate(game, show_tianming_Platform);
                            pos_tianming_size = show_tianming_Platform.GetComponent<RectTransform>().rect.size;
                            tianming_size = new Vector2(pos_tianming_size.x, pos_tianming_size.y);
                            tianming.GetComponent<RectTransform>().sizeDelta = tianming_size;
                        }
                    }
                }

            }
            get
            {
                return data;
            }
        }

        /// <summary>
        /// 指定目标
        /// </summary>
        /// <param name="health"></param>
        public void SpecifyTarget(BattleHealth health)
        {
            Terget = health;
        }

        public void injured()
        {
            //播放音效
        }

        protected int SkillInfo = 0;
        /// <summary>
        /// 基础速度
        /// </summary>
        protected float baseSpeed = 2f;
        /// <summary>
        /// 自动战斗
        /// </summary>
        public virtual void OnAuto()
        {
            
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public virtual void Instace()
        {

        }

        public void Lose_Terget()
        {
            Find_Terget();
        }

        /// <summary>
        /// 寻找攻击对象
        /// </summary>
        protected virtual void Find_Terget()
        {
            //if (Tergets == null) return;
            bool exist = false;
            if (Tergets.Count > 0)//怪物找玩家
            {
                for (int i = 0; i < Tergets.Count; i++)
                {
                    if (Tergets[i] == null) continue;
                    if (Tergets[i].HP <= 0 || Tergets[i].gameObject.activeSelf == false)
                    {
                        //Debug.Log("报空");
                        Tergets.RemoveAt(i);
                        i--;
                    }
                }
                if (Tergets.Count > 0)
                {
                    //寻找距离自身最近的目标    
                    exist = true;
                }
            }
            if (exist)
            {
                Terget = ArrayHelper.GetMin(Tergets, e => Vector2.Distance(transform.position, e.transform.position));
            }
            else
            {
                if (GetComponent<Player>() != null) Game_Next_Map();
                else game_over();
            }
        }
        /// <summary>
        /// 进行下一回合
        /// </summary>
        private void Game_Next_Map()
        {
            DailyCopies(Terget);
            transform.parent.parent.parent.SendMessage("Game_Next_Map");

        }
        /// <summary>
        /// 游戏结束
        /// </summary>
        private void game_over()
        {
            DailyCopies(target);
            transform.parent.parent.parent.SendMessage("Game_Over");
        }
        /// <summary>
        /// 获取副本奖励
        /// </summary>
        private void DailyCopies(BattleHealth health)
        {

        }

        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="dec"></param>
        protected virtual void Show_Info(string dec)
        {
            if (transform.parent.parent.parent.parent) transform.parent.parent.parent.parent.SendMessage("show_info", dec);
        }

        public virtual void Update()
        {
            if (data != null)//判断是否有怪物
            {
                if (Terget != null)
                {
                    if (Terget.HP <= 0 || !Terget.gameObject.activeInHierarchy) Find_Terget();
                }
                else Find_Terget();
                show_hp.value = target.HP;
                hp_text.text = Battle_Tool.FormatNumberToChineseUnit(target.HP) + "/" + Battle_Tool.FormatNumberToChineseUnit(target.maxHP);
            }

        }

        /// <summary>
        /// 关闭光环
        /// </summary>
        /// <returns></returns>
        IEnumerator HideFrame()
        {
            yield return new WaitForSeconds(1f);
            frame.gameObject.SetActive(false);
        }

        /// <summary>
        /// 对目标造成伤害
        /// </summary>
        /// <param name="skill"></param>
        public void skill_damage(base_skill_vo skill)
        {
            BattleAttack monster = Terget.GetComponent<BattleAttack>();
            int lv = int.Parse(skill.user_values[1]);
            if (skill.skill_damage_type == 7)
            {
                target.MP += (skill.skill_damage + (skill.skill_power * lv)) * target.maxMP / 100;
                if (target.MP > target.maxMP) target.MP = target.maxMP;
                return;
            }
            if (skill.skill_damage_type == 6)
            {
                int hp = (int)(skill.skill_damage + (skill.skill_power * lv)) * (data.MagicdamageMax + data.MagicdamageMin) / 200;
                transform.parent.parent.parent.SendMessage("add_hp", hp);
                return;
            }
            if (skill.skill_damage_type == 4)
            {
                int value = (data.DefMin + data.DefMax) / 2 * ((skill.skill_damage + (skill.skill_power * lv))) / 100;
                Open_Skill_State(data, 1);
                data.skill_state[1] = (1, value, DateTime.Now, skill.skill_cd);
                return;
            }
            if (skill.skill_damage_type == 5)
            {
                int value = (data.MagicDefMin + data.MagicDefMax) / 2 * ((skill.skill_damage + (skill.skill_power * lv))) / 100;
                Open_Skill_State(data, 2);
                data.skill_state[2] = (2, value, DateTime.Now, skill.skill_cd);
                return;
            }
            if (skill.skill_damage_type == 8)
            {
                int value = (data.MagicDefMin + data.MagicDefMax) / 2 * ((skill.skill_damage + (skill.skill_power * lv))) / 100;
                Open_Skill_State(data, 3);
                data.skill_state[3] = (3, value, DateTime.Now, skill.skill_cd);
                return;
            }
            if (monster.target.HP <= 0) return;//结战斗
            float damage = Base_Damage(monster, skill);
            damage += skill.skill_spell * target.maxMP / 100;
            damage = damage * (skill.skill_damage + (skill.skill_power * lv) + Tool_State.Value_playerprobabilit(enum_skill_attribute_list.技能伤害)) / 100;
            //内力伤害
            if (skill.user_values[3] != "")
            {
                int internalforce = int.Parse(skill.user_values[3]);
                if (target.internalforceMP >= internalforce)
                {
                    target.internalforceMP -= internalforce;
                    damage = damage * (100 + internalforce) / 100;
                }
            }
            //爆发伤害
            if (target.EnergyMp > 0 && target.EnergyMp >= target.EnergymaxMp)
            {
                target.EnergyMp = 0;
                damage = damage * (100 + target.EnergymaxMp) / 100;
                tool_Categoryt.Base_Task(1007);
            }
            //判断五行伤害
            int life = restrain_value(skill.skill_life - 1, monster.Data.life);
            if (life < 0) damage = (damage / 2) * (100 + (life)) / 100f;
            else
            {
                life += Tool_State.Value_playerprobabilit(enum_skill_attribute_list.五行伤害);
                damage = damage * (100 + (life)) / 100f;
            }
            if (iSnHit(monster))
            {
                //传递消息，未命中;
                monster.target.TakeDamage(1, DamageEnum.技能未命中);
                return;
            }
            bool isCrit = isCrate(monster);
            if (isCrit)
            {
                damage = damage * data.crit_damage / 100;
            }
            damage = MathF.Max(1, damage);
            damage = damage * (100 + Tool_State.Value_playerprobabilit(enum_skill_attribute_list.最终伤害)) / 100;

            monster.target.TakeDamage((int)damage, isCrit ? DamageEnum.暴击技能伤害 : DamageEnum.技能伤害);
            if (data.Real_harm > 0)
            {
                monster.target.TakeDamage(data.Real_harm, DamageEnum.真实伤害);
            }
        }

        protected virtual void BaseAttack()//判断伤害
        {
            if (Terget == null) return;
            AudioManager.Instance.playAudio(ClipEnum.攻击敌人);
            BattleAttack monster = Terget.GetComponent<BattleAttack>();
            if (monster.target.HP <= 0) return;//结战斗
            long damage = Base_Damage(monster);
            if (iSnHit(monster))
            {
                monster.target.TakeDamage(1, DamageEnum.未命中);
                return;
            }
            bool isCrit = iSnHit(monster);
            if (isCrit)
            {
                damage = damage * data.crit_damage / 100;
            }
            damage = damage * (100 + Tool_State.Value_playerprobabilit(enum_skill_attribute_list.最终伤害)) / 100;
            monster.target.TakeDamage(damage, isCrit ? DamageEnum.暴击伤害 : DamageEnum.普通伤害);
            if (data.Real_harm > 0)
            {
                monster.target.TakeDamage(data.Real_harm, DamageEnum.真实伤害);
            }
        }

        private long Base_Damage(BattleAttack monster, base_skill_vo skill = null)
        {
            long damage = 0;
            if (skill == null)
            {
                damage = defense(monster, data.Type, data.isBackstab);
            }
            else
                damage = defense(monster, skill.skill_damage_type, data.isBackstab);
            damage = (long)Mathf.Max(1, damage);
            damage = damage * (100 + penetrate(monster)) / 100;
            damage = (long)Mathf.Max(1, damage);
            //damage = damage * (100 + data.double_damage - monster.data.Damage_Reduction) / 100;
            damage = (long)Mathf.Max(1, damage);
            if (skill == null)
            {
                //int life = restrain_value(monster.Data.life, monster.Data.life_types);
                //if (life < 0) damage = (damage / 2) * (100 + (life)) / 100;
                //else damage = damage * (100 + (life)) / 100;
            }
            damage = (long)Mathf.Max(1, damage);

            if (Tool_State.Value_playerprobabilit(data.bufflist, enum_skill_attribute_list.攻击回血) > 0)
            {
                target.HealConsumables(Tool_State.Value_playerprobabilit(data.bufflist, enum_skill_attribute_list.攻击回血),
                    Tool_State.Value_playerprobabilit(data.bufflist, enum_skill_attribute_list.攻击回蓝));
            }
            if (Tool_State.Value_playerprobabilit(data.bufflist, enum_skill_attribute_list.攻击吸血) > 0)
            {
                target.HealConsumables((int)damage / 100, 0);
            }
            return damage;
        }

        /// <summary>
        /// 计算伤害
        /// </summary>
        /// <param name="monster">对象</param>
        /// <param name="type">类型</param>
        /// <param name="isBackstab">是否背刺</param>
        /// <returns></returns>
        private long defense(BattleAttack monster, int type, int isBackstab)
        {
            long damage = 0;
            long def = 0;
            if (type == 1)
            {
                damage = Lucky(Data.damageMin, Data.damageMax, data.Lucky);
                def = (Random.Range(monster.Data.DefMin, monster.Data.DefMax) * (100 + monster.Data.bonus_Def) / 100);
            }
            if (type == 2)
            {
                damage = Lucky(Data.MagicdamageMin, Data.MagicdamageMax, data.Lucky);
                def = (Random.Range(monster.Data.MagicDefMin, monster.Data.MagicDefMax) * (100 + monster.Data.bonus_MagicDef) / 100);
            }
            if (data.equip_suit_lists.Count > 0)
            {
                foreach (var item in data.equip_suit_lists.Keys)
                {
                    switch (item)
                    {
                        case enum_equip_show_list.降低对方防御:
                            def = (def * (100 - data.equip_suit_lists[item])) / 100;
                            break;
                        case enum_equip_show_list.暴击伤害:
                            break;
                        case enum_equip_show_list.双倍打击概率:
                            break;
                        case enum_equip_show_list.中毒概率:
                            break;
                        case enum_equip_show_list.麻痹概率:
                            break;
                        case enum_equip_show_list.释放火球分身概率:
                            break;
                        default:
                            break;
                    }
                }


            }
            def = (long)MathF.Max(0, def);
            if (isBackstab == 0)
            {
                damage = damage - def;
                //damage -= skillstate(monster.data, type);
            }
            if (type == 1)
            {
                damage = damage * (100 + data.bonus_Damage) / 100;
            }
            if (type == 2)
            {
                damage = damage * (100 + data.bonus_MagicDamage) / 100;
            }
            return damage;
        }

        /// <summary>
        /// 判断技能效果
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private int skillstate(crtMaxHeroVO user, int type)
        {
            int value = 0;
            Open_Skill_State(user, type);
            if (user.skill_state[type].Item2 != 0)
            {
                int time = Battle_Tool.SettlementTransport(user.skill_state[type].Item3.ToString(), 2);
                if (time >= user.skill_state[type].Item4)//超过有效时间
                {
                    user.skill_state[type] = (type, 0, DateTime.Now, 0);
                }
                else value = user.skill_state[type].Item2;
            }

            return value;
        }
        /// <summary>
        /// 判断状态
        /// </summary>
        /// <param name="user"></param>
        /// <param name="type"></param>
        private void Open_Skill_State(crtMaxHeroVO user, int type)
        {
            while (user.skill_state.Count < type + 1)
            {
                user.skill_state.Add((type, 0, DateTime.Now, 0));
            }
        }
        /// <summary>
        /// 判断五行伤害克制关系
        /// </summary>
        /// <param name="type">自身技能五行</param>
        /// <param name="monsterlife">怪物五行属性</param>
        /// <param name="monsterlifevalue">怪物五行抗性</param>
        private int restrain_value(int type, int[] monsterlife)
        {
            int value = 0;
            int monsterlifevaluelue = 0;
            int index = 0;
            for (int i = 0; i < monsterlife.Length; i++)
            {
                if (monsterlife[i] != 0)
                {
                    index = i;
                    monsterlifevaluelue = monsterlife[i];
                }
            }
            float coefficient = 1.25f;
            foreach (var item in data.life_types.Keys)
            {
                if (item == type)
                {
                    coefficient += (Battle_Tool.battle_life_bonus(data.life_types[item]) / 100f);
                }
            }
            //        /*
            //         *    土属性强化,// 201  
            //火属性强化,//202
            //水属性强化,// 203
            //木属性强化,//204
            //金属性强化,//205
            //         //*// 0土 1火 2水 3木 4金 
            //对手五行
            value = battle_restrain_value(index, type, coefficient, monsterlifevaluelue);
            return value;
        }
        /// <summary>
        /// 普通战斗五行属性
        /// </summary>
        /// <param name="type"></param>
        /// <param name="monsterlife"></param>
        /// <param name="dec"></param>
        /// <returns></returns>
        private int restrain_value(int[] monsterlife, Dictionary<int, int> dec)
        {
            int value = 0, type = 0;
            float coefficient = 1.25f;

            foreach (var item in data.life_types.Keys)
            {
                if (item == type)
                {
                    coefficient += (Battle_Tool.battle_life_bonus(data.life_types[item]) / 100f);
                }
            }
            foreach (var item in dec.Keys)
            {
                value = (int)MathF.Max(value, battle_restrain_value(item, type, coefficient, monsterlife[item]));
            }
            return value;
        }
        /// <summary>
        /// 获取计算加成
        /// </summary>
        /// <param name="index">目标五行</param>
        /// <param name="type">自身五行</param>
        /// <param name="coefficient">系数</param>
        /// <param name="monsterlifevaluelue">五行值</param>
        /// <returns></returns>
        private int battle_restrain_value(int index, int type, float coefficient, int monsterlifevaluelue)
        {
            int value = 0;
            switch (index)//金克木 木克土 土克水 水克火 火克金   0土 1火 2水 3木 4金 
            {
                //金属性 同属计算抗性
                case 0:
                    if (type == index) value = 0;
                    //克制计算乘法（木克）
                    else if (type == 3) value = (int)(data.life[type] * coefficient - monsterlifevaluelue);
                    else if (type == 2) value = (int)(data.life[type] * 0.1f - monsterlifevaluelue);
                    //被克制计算乘法（水克）
                    if (type != 2) value = Mathf.Max(0, value);
                    break;
                case 1:
                    if (type == index) value = 0;
                    else if (type == 2) value = (int)(data.life[type] * coefficient - monsterlifevaluelue);
                    else if (type == 4) value = (int)(data.life[type] * 0.1f - monsterlifevaluelue);
                    if (type != 4) value = Mathf.Max(0, value);

                    break;
                case 2:
                    if (type == index) value = 0;
                    else if (type == 0) value = (int)(data.life[type] * coefficient - monsterlifevaluelue);
                    else if (type == 1) value = (int)(data.life[type] * 0.1f - monsterlifevaluelue);
                    if (type != 1) value = Mathf.Max(0, value);

                    break;
                case 3:
                    if (type == index) value = 0;
                    else if (type == 4) value = (int)(data.life[type] * coefficient - monsterlifevaluelue);
                    else if (type == 0) value = (int)(data.life[type] * 0.1f - monsterlifevaluelue);
                    if (type != 0) value = Mathf.Max(0, value);

                    break;
                case 4:
                    if (type == index) value = 0; //value = data.life[type] - monsterlifevaluelue;
                    else if (type == 1) value = (int)(data.life[type] * coefficient - monsterlifevaluelue);
                    else if (type == 3) value = (int)(data.life[type] * 0.1f - monsterlifevaluelue);
                    if (type != 3) value = Mathf.Max(0, value);
                    break;
                default:
                    break;
            }

            return value;
        }

        /// <summary>
        /// 判断命中
        /// </summary>
        /// <param name="monster"></param>
        /// <returns></returns>
        private bool iSnHit(BattleAttack monster)
        {
            bool exist = false;
            if (Data.hit < monster.Data.dodge && Random.Range(0, 100) > 10)//命中不达标也有10%的概率
            {
                //传递消息，未命中;
                exist = true;
            }
            return exist;
        }

        /// <summary>
        /// 判断是否暴击
        /// </summary>
        /// <param name="monster"></param>
        /// <returns></returns>
        private bool isCrate(BattleAttack monster)
        {
            bool isCrit = false;
            if (Random.Range(0, 100) < (data.crit_rate - monster.Data.crit_rate) * 100 / (data.crit_rate + 30))
            {
                isCrit = true;
            }
            return isCrit;
        }
        /// <summary>
        /// 幸运加成
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="lucky"></param>
        private int Lucky(int min, int max, int lucky)
        {
            int value = 0;
            value = Random.Range(min + (max - min) * lucky / 10, max);
            if (lucky > 10)
            {
                value = value * (100 + (lucky * 10)) / 100;
            }
            return value;
        }
        /// <summary>
        /// 计算穿透效果 
        /// </summary>
        /// <param name="monster"></param>
        /// <returns></returns>
        private int penetrate(BattleAttack monster)
        {
            int value = (data.penetrate - monster.Data.block) * 100 / (data.penetrate + 500);
            if (value < 0) value = 0;
            return value;
        }
    }
}
