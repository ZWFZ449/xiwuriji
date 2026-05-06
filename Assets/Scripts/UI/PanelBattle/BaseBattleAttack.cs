using System;
using System.Collections;
using System.Collections.Generic;
using Common;
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
                    base_name.text += Show_Color.Red(Battle_Tool.Obtain_Talent_Name());
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
            int battle_Damage = 0;//真实伤害
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
                                                skilldamage += value;
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
                if (skill.skill_damages.Count> skill.SetLv()) battle_Damage = skill.skill_damages[skill.SetLv()];
            }
            float damage = Base_Damage(monster, skilldamage);
            switch ((Skill_Effect_Type)skill.EffectType)
            {
                case Skill_Effect_Type.单体:
                    TakeDamage((int)damage, monster,battle_Damage);
                    break;
                case Skill_Effect_Type.群体:
                    List<BattleHealthState> monsterList = FindTheTarget(skill, monster);
                    for (int i= 0; i < monsterList.Count; i++) 
                    {
                        BaseBattleAttack base_monster = monsterList[i].gameObject.GetComponent<BaseBattleAttack>(); 
                        if (base_monster != null)
                        {
                            int base_damage = Base_Damage(base_monster, skilldamage);
                            if (base_damage < 0) base_damage = 1;
                            TakeDamage(base_damage, base_monster, battle_Damage);
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
        /// <summary>
        /// 自动回复
        /// </summary>
        public void autoreply()
        {
            if (data.data.hpRegen > 0 || data.data.mpRegen>0)
            {
                //Debug.Log("huifu"+data.data.hpRegen + " " + data.data.mpRegen);
                oneselfHealthState. Use_Medicine(data.data.hpRegen, data.data.mpRegen);
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
        /// /// <param name="battle_Damage">真实伤害</param>
        private void TakeDamage(int damage, BaseBattleAttack monster, int battle_Damage=0)
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
            if (data.data.battle_Damage > 0 || battle_Damage > 0)
            {
                int data_battle_Damage = data.data.battle_Damage + battle_Damage - monster.data.data.damage_reduction;
                if (data_battle_Damage > 0)
                monster.oneselfHealthState.TakeDamage(data_battle_Damage, DamageEnum.真实伤害); 
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

        private int Base_Damage(BaseBattleAttack monster,int skilldamage=100)
        {
            int damage = 0;
            switch (data.type)
            {
                case Battle_Game_Type.player:
                case Battle_Game_Type.call:
                    damage = defense(monster, data.hero_type, skilldamage);
                    break;
                case Battle_Game_Type.monster:
                case Battle_Game_Type.Boss:
                case Battle_Game_Type.Activity_Monster:
                    damage = defense(monster, data.data.dc2 > data.data.mc2 ? Hero_Type.战士 : Hero_Type.法师, skilldamage);
                    break;
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
        private int defense(BaseBattleAttack monster, Hero_Type type, int skilldamage = 100)
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
            damage = damage * skilldamage / 100 - def;
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
            else
            {
                //传递消息，命中;
                if (Random.Range(0, 100) > Data.data.hit - monster.Data.data.dodge)
                { 
                    exist = true;
                }
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
}
