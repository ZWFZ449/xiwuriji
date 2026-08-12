using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Components;
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

        protected List<db_skill_vo> battle_skills;

        /// <summary>
        /// 目标
        /// </summary>
        protected BattleHealthState Terget;
        /// <summary>
        /// 目标被攻击次数
        /// </summary>
        protected int terget_attack_number;

        private Text base_name;

        private Image base_icon;
        protected virtual void Awake()
        {
            Init();
        }
        /// <summary>
        /// 查看目标
        /// </summary>
        public BattleHealthState InfoTerget { get { return Terget; } }
        /// <summary>
        /// 围绕旋转
        /// </summary>
        private bool is_centerPoint = false;
        /// <summary>
        /// 中心点
        /// </summary>
        private Transform crt_centerPoint;
        private Vector2 initialOffset;
        private float angle = 0f, speed = 30f, radius = 500f;
        public void Radius(Transform centerPoint)
        {
            is_centerPoint = true;
            crt_centerPoint= centerPoint;
            initialOffset = (Vector2)transform.position - (Vector2)crt_centerPoint.position;
            angle = 0f; // 重置角度
        }

        private float attack_speed = 0;
        private void Update()
        {
            
            if (data != null)
            {
                if (data.type == Battle_Game_Type.player)
                { 
                
                }
                if (Terget != null)//判断是否有怪物
                {
                    if (Terget.isDead ||!Terget.gameObject.activeInHierarchy) Find_Terget();
                }
                else Find_Terget();
            }
            if(is_centerPoint)//判断是否围绕旋转
            {
                // 安全检查
                if (crt_centerPoint == null) return;

                Vector2 rotatedOffset = Quaternion.Euler(0, 0, angle) * initialOffset;

                // 2. 应用新位置（中心点 + 旋转后的偏移）
                transform.position = (Vector2)crt_centerPoint.position + rotatedOffset;

                // 3. 更新角度（累加）
                angle += speed * Time.deltaTime;
            }
        }
        public void Set_Target(BattleHealthState target)
        {
            Terget = target;
        }
        private IEnumerator timer()
        {   
            while (true)
            {
                yield return new WaitForSeconds(0.05f);
                if (Terget != null)
                {
                    if (data.type == Battle_Game_Type.player)
                    {

                    }
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
            terget_attack_number = 0;
            Terget = null;
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
                }
            }
            if (monsterList.Count > 0)
            {
                Terget = ArrayHelper.GetMin(monsterList, (x) => Vector3.Distance(x.transform.position, transform.position));
                //是否移动
                OnCShase();
                if (TergetTag == "Player")
                {
                    if (monsterList.Count > 1&& data.type !=  Battle_Game_Type.monster)
                    {
                        for (int i = 0; i < monsterList.Count; i++)
                        {
                            if (monsterList[i].GetComponent<BaseBattleAttack>() != null)
                            {
                                if (monsterList[i].GetComponent<BaseBattleAttack>().Data.type == Battle_Game_Type.player)
                                { 
                                    Terget= monsterList[i];
                                    return;
                                }
                            }
                        }
                    }
                }
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
            if (Terget != null) Terget = null;//重置目标
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
            oneselfHealthState.Init(data.data.battle_maxhp, data.data.battle_maxmp, data.crt_name);
            StopAllCoroutines();
            StartCoroutine(timer());
        }

        public virtual void Refresh(crtMaxBattleVO hero)
        {

        }

        public virtual void Refresh_Skill(List<db_skill_vo> skills)
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
            is_skill_probability = false;
            int battle_Damage = 0;//真实伤害
            Dictionary<int, db_skill_vo> skill_list = SumSave.crt_skill.Set_Current_skill();
            int skilldamage = 0;
            foreach (var item in skill_list)
            {
                int skill_lv = item.Value.SetLv();
                if (skill_lv >= 0)
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
                    if (monster.data.def_buff == 0)
                    {
                        if (item.Value.EffectType == 4)//状态类
                        {
                            int duff_value = item.Value.Power + item.Value.DefPowers[skill_lv];
                            duff_value = (data.data.sc + data.data.sc2) / 2 * duff_value / 100;//计算减防
                            if (item.Value.skill_damages.Count > skill_lv) duff_value += item.Value.skill_damages[skill_lv];
                            monster.data.def_buff = duff_value;
                            monster.data.def_buff = (int)MathF.Min(duff_value, MathF.Max(monster.data.data.ac2, monster.data.data.mac2) / 2);
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
            foreach (var item1 in skill.GetBuff.Keys)
            {
                switch (item1)
                {
                    case enum_talent_offect_list.生命:
                        break;
                    case enum_talent_offect_list.攻击:
                        break;
                    case enum_talent_offect_list.魔法:
                        break;
                    case enum_talent_offect_list.道术:
                        break;
                    case enum_talent_offect_list.防御:
                        break;
                    case enum_talent_offect_list.攻击速度:
                        break;
                    case enum_talent_offect_list.物理攻击:
                        break;
                    case enum_talent_offect_list.魔法攻击:
                        break;
                    case enum_talent_offect_list.道术攻击:
                        break;
                    case enum_talent_offect_list.防御值:
                        break;
                    case enum_talent_offect_list.躲避:
                        break;
                    case enum_talent_offect_list.命中:
                        break;
                    case enum_talent_offect_list.技能:
                        break;
                    case enum_talent_offect_list.附加攻击:
                        break;
                    case enum_talent_offect_list.附加魔法:
                        break;
                    case enum_talent_offect_list.附加道术:
                        break;
                    case enum_talent_offect_list.附加双防:
                        break;
                    case enum_talent_offect_list.附加伤害:
                        battle_Damage += skill.GetBuff[item1];
                        break;
                    case enum_talent_offect_list.附加回血:
                        break;
                    case enum_talent_offect_list.附加攻击范围:
                        break;
                    case enum_talent_offect_list.无视防御:
                        break;
                    case enum_talent_offect_list.召唤兽:
                        break;
                    case enum_talent_offect_list.召唤兽攻击:
                        break;
                    case enum_talent_offect_list.召唤兽生命:
                        break;
                    case enum_talent_offect_list.召唤兽防御:
                        break;
                    case enum_talent_offect_list.召唤兽速度:
                        break;
                    case enum_talent_offect_list.召唤兽死亡爆炸:
                        break;
                    case enum_talent_offect_list.特殊效果:
                        break;
                    case enum_talent_offect_list.临时伤害:
                        break;
                    case enum_talent_offect_list.临时防御:
                        break;
                    case enum_talent_offect_list.临时速度:
                        break;
                    case enum_talent_offect_list.单体改群体:
                        break;
                    case enum_talent_offect_list.技能攻击个数:
                        break;
                    case enum_talent_offect_list.技能概率不消耗蓝:
                        break;
                    case enum_talent_offect_list.技能全体伤害:
                        break;
                    case enum_talent_offect_list.群体技能攻击范围:
                        break;
                    case enum_talent_offect_list.每秒回复全体血量百分比:
                        break;
                    case enum_talent_offect_list.攻击击退敌人概率:
                        break;
                    case enum_talent_offect_list.弹道:
                        break;
                    case enum_talent_offect_list.召唤数量:
                        break;
                    case enum_talent_offect_list.技能伤害:
                        skilldamage += skill.GetBuff[item1];
                        break;
                    case enum_talent_offect_list.技能触发概率:
                        break;
                    case enum_talent_offect_list.溅射数量:
                        break;
                    case enum_talent_offect_list.爆炸伤害:
                        break;
                }
            }
            long damage = Base_Damage(monster, skilldamage);
            if (skill_probability(skill)) damage = damage * 10;
            switch ((Skill_Effect_Type)skill.EffectType)
            {
                case Skill_Effect_Type.单体:
                    bool exist = true;
                    foreach (var item1 in skill.GetBuff)
                    {
                        switch (item1.Key)
                        {
                            case enum_talent_offect_list.生命:
                                break;
                            case enum_talent_offect_list.攻击:
                                break;
                            case enum_talent_offect_list.魔法:
                                break;
                            case enum_talent_offect_list.道术:
                                break;
                            case enum_talent_offect_list.防御:
                                break;
                            case enum_talent_offect_list.攻击速度:
                                break;
                            case enum_talent_offect_list.物理攻击:
                                break;
                            case enum_talent_offect_list.魔法攻击:
                                break;
                            case enum_talent_offect_list.道术攻击:
                                break;
                            case enum_talent_offect_list.防御值:
                                break;
                            case enum_talent_offect_list.躲避:
                                break;
                            case enum_talent_offect_list.命中:
                                break;
                            case enum_talent_offect_list.技能:
                                break;
                            case enum_talent_offect_list.附加攻击:
                                break;
                            case enum_talent_offect_list.附加魔法:
                                break;
                            case enum_talent_offect_list.附加道术:
                                break;
                            case enum_talent_offect_list.附加双防:
                                break;
                            case enum_talent_offect_list.附加伤害:
                                break;
                            case enum_talent_offect_list.附加回血:
                                break;
                            case enum_talent_offect_list.附加攻击范围:
                                break;
                            case enum_talent_offect_list.无视防御:
                                break;
                            case enum_talent_offect_list.召唤兽:
                                break;
                            case enum_talent_offect_list.召唤兽攻击:
                                break;
                            case enum_talent_offect_list.召唤兽生命:
                                break;
                            case enum_talent_offect_list.召唤兽防御:
                                break;
                            case enum_talent_offect_list.召唤兽速度:
                                break;
                            case enum_talent_offect_list.召唤兽死亡爆炸:
                                break;
                            case enum_talent_offect_list.特殊效果:
                                break;
                            case enum_talent_offect_list.临时伤害:
                                break;
                            case enum_talent_offect_list.临时防御:
                                break;
                            case enum_talent_offect_list.临时速度:
                                break;
                            case enum_talent_offect_list.单体改群体:
                            case enum_talent_offect_list.爆炸伤害:
                                exist = false;
                                Buff_rangeold(skill, monster, skilldamage * item1.Value / 100, battle_Damage * item1.Value / 100);
                                break;
                            case enum_talent_offect_list.技能攻击个数:
                                exist = false;
                                Buff_range(skill, monster, skilldamage, battle_Damage, item1.Value);
                                //Buff_range(skill, monster, skilldamage * item1.Value / 100, battle_Damage * item1.Value / 100);
                                break;
                            case enum_talent_offect_list.技能概率不消耗蓝:
                                break;
                            case enum_talent_offect_list.技能全体伤害:
                                break;
                            case enum_talent_offect_list.群体技能攻击范围:
                                break;
                            case enum_talent_offect_list.每秒回复全体血量百分比:
                                break;
                            case enum_talent_offect_list.攻击击退敌人概率:
                                break;
                            case enum_talent_offect_list.弹道:
                                break;
                            case enum_talent_offect_list.召唤数量:
                                break;
                            case enum_talent_offect_list.技能伤害:
                                break;
                            case enum_talent_offect_list.技能触发概率:
                                if (skill_probability(skill)) damage = damage * 10;
                                break;
                            case enum_talent_offect_list.溅射数量:
                                Buff_range(skill, monster, skilldamage, battle_Damage, item1.Value, 50);
                                break;
                            default:
                                break;
                        }
                    }

                    if (exist) TakeDamage(damage, monster, battle_Damage);

                    break;
                case Skill_Effect_Type.群体:
                     
                    List<BattleHealthState> monsterList = FindTheTarget(skill, monster);
                    for (int i= 0; i < monsterList.Count; i++) 
                    {
                        BaseBattleAttack base_monster = monsterList[i].gameObject.GetComponent<BaseBattleAttack>(); 
                        if (base_monster != null)
                        {
                            long base_damage = Base_Damage(base_monster, skilldamage);
                            if (base_damage < 0) base_damage = 1;
                            TakeDamage(base_damage, base_monster, battle_Damage);
                        }
                    }
                    foreach (var item1 in skill.GetBuff)
                    {
                        switch (item1.Key)
                        {
                            case enum_talent_offect_list.溅射数量:
                                Buff_range(skill, monster, skilldamage, battle_Damage, item1.Value, 50);
                                break;
                            default:
                                break;
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
        /// 群攻
        /// </summary>
        /// <param name="skill"></param>
        /// <param name="monster"></param>
        /// <param name="skilldamage"></param>
        /// <param name="battle_Damage"></param>
        private void Buff_rangeold(db_skill_vo skill,BaseBattleAttack monster, int skilldamage,int battle_Damage)
        {
            List<BattleHealthState> monsterList = FindTheTarget(skill, monster);
            for (int i = 0; i < monsterList.Count; i++)
            {
                BaseBattleAttack base_monster = monsterList[i].gameObject.GetComponent<BaseBattleAttack>();
                if (base_monster != null)
                {
                    long base_damage = Base_Damage(base_monster, skilldamage);
                    if (base_damage < 0) base_damage = 1;
                    TakeDamage(base_damage, base_monster, battle_Damage);
                }
            }
        }
        private void Buff_range(db_skill_vo skill, BaseBattleAttack monster, int skilldamage, int battle_Damage, int number)
        {
            List<BattleHealthState> monsterList = FindTheTarget(skill, monster, true);
            int max = Mathf.Min(monsterList.Count, number - 1);
            for (int i = 0; i < max; i++)
            {
                BaseBattleAttack base_monster = monsterList[i].gameObject.GetComponent<BaseBattleAttack>();
                if (base_monster != null)
                {
                    long base_damage = Base_Damage(base_monster, skilldamage);
                    if (base_damage < 0) base_damage = 1;
                    TakeDamage(base_damage, base_monster, battle_Damage);
                }
            }
        }
        private void Buff_range(db_skill_vo skill, BaseBattleAttack monster, int skilldamage, int battle_Damage, int number,int coefficient)
        {

            List<BattleHealthState> monsterList = FindTheTarget(skill, monster, true);
            int max = Mathf.Min(monsterList.Count, number);
            for (int i = 0; i < max; i++)
            {
                BaseBattleAttack base_monster = monsterList[i].gameObject.GetComponent<BaseBattleAttack>();
                if (base_monster != null)
                {
                    long base_damage = Base_Damage(base_monster, skilldamage);
                    base_damage= base_damage * coefficient / 100;
                    if (skill_probability(skill)) base_damage = base_damage * 10;
                    if (base_damage < 0) base_damage = 1;
                    TakeDamage(base_damage, base_monster, battle_Damage);
                }
            }
        }

        private bool is_skill_probability = false;
        /// <summary>
        /// 技能概率
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        private bool skill_probability(db_skill_vo skill)
        {
            if (is_skill_probability) return false;
            if (skill.probability == 0) return false;
            if (Random.Range(0, 100) < skill.probability)
            {
                is_skill_probability = true;
                return true;
            }
            return false;

        }
        /// <summary>
        /// 自动回复
        /// </summary>
        public void autoreply()
        {
            if (data.numbness_IsState)
            {
                Alert_Dec.Show(data.crt_name + "麻痹中回复无效");
                return;
            }
            if (data.data.hpRegen > 0 || data.data.mpRegen>0)
            {
                //Debug.Log("huifu"+data.data.hpRegen + " " + data.data.mpRegen);
                oneselfHealthState. Use_Medicine(data.data.hpRegen, data.data.mpRegen);
            }
        }
        private List<BattleHealthState> FindTheTarget(db_skill_vo skill, BaseBattleAttack base_monster,bool exist=false)
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
                    if (exist) monsterList.Add(monster);
                    else
                    {
                        switch (skill.MoveType)
                        {
                            case 0:
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
            long damage = Base_Damage(monster);
            TakeDamage(damage, monster);
        }
        /// <summary>
        /// 统一伤害格式
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="monster"></param>
        /// /// <param name="battle_Damage">真实伤害</param>
        private void TakeDamage(long damage, BaseBattleAttack monster, int battle_Damage=0)
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
                            if (item.Item2 == 150)
                            {
                                value = (int)item.Item3;
                            }
                            value = (int)MathF.Max(1, value);
                            oneselfHealthState.TakeDamage(value, DamageEnum.普通伤害);
                        }
                        break;
                    case enum_battle_pet_talent_list.防爆:

                        break;
                    case enum_battle_pet_talent_list.招架:
                        if (Random.Range(0, 100) < item.Item2)
                        {
                            damage = (int)(damage * (100 - item.Item3) / 100);
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
                                long value = (long)(damage * item.Item3 / 100);
                                value = (long)MathF.Max(1, value);
                                monster.oneselfHealthState.TakeDamage(value, DamageEnum.普通伤害);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private long Base_Damage(BaseBattleAttack monster,int skilldamage=100)
        {
            long damage = 0;
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
        private long defense(BaseBattleAttack monster, Hero_Type type, int skilldamage = 100)
        {
            long damage = 0;
            int def = 0;
            switch (type)
            {
                case Hero_Type.平民:
                    damage = Lucky(Data.data.dc, Data.data.dc2, data.data.lucky);
                    def = Random.Range(monster.Data.data.ac, monster.Data.data.ac2);
                    break;
                case Hero_Type.战士:
                    damage = Lucky(Data.data.dc, Data.data.dc2, data.data.lucky);
                    if (SumSave.crtHero.zs_lvs > 1 && (data.type == Battle_Game_Type.player || data.type == Battle_Game_Type.call)) damage += (int)data.data.battle_maxhp / 100;
                    def = Random.Range(monster.Data.data.ac, monster.Data.data.ac2);
                    break;
                case Hero_Type.法师:
                    damage = Lucky(Data.data.mc, Data.data.mc2, data.data.lucky);
                    if (SumSave.crtHero.zs_lvs > 1 && (data.type == Battle_Game_Type.player || data.type == Battle_Game_Type.call)) damage += (int)data.data.battle_maxmp / 100;
                    def = Random.Range(monster.Data.data.mac, monster.Data.data.mac2);
                    break;
                case Hero_Type.道士:
                    damage = Lucky(Data.data.sc, Data.data.sc2, data.data.lucky);
                    if (SumSave.crtHero.zs_lvs > 1 && (data.type == Battle_Game_Type.player || data.type == Battle_Game_Type.call))
                    {
                        damage += ((int)data.data.battle_maxhp + (int)data.data.battle_maxmp) / 100;
                    }
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
                            is_skill_probability = true;
                            damage = (int)(damage * item.Item3);
                        }
                        break;
                    case enum_battle_pet_talent_list.斩杀:
                        if (monster.GetComponent<BattleHealthState>().Proportion().Item1 <= item.Item2)
                        {
                            damage = (int)(monster.data.data.battle_maxhp * item.Item2 / 100);
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
                                def -= (int)item.Item3;
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
            if (data.hero_talentList != null)
            {
                (int, int) Proportion = oneselfHealthState.Proportion();

                foreach (var item in data.hero_talentList.Keys)
                {
                    switch (item)
                    {
                        case enum_talent_offect_list.临时伤害:
                            int value = (100 - Proportion.Item1) / 10 * data.hero_talentList[item];
                            if (value > 0 && value <= 100)
                            {
                                damage = (int)(damage * (value + 100) / 100);
                            }
                            break;
                        case enum_talent_offect_list.临时防御:
                            break;
                        default:
                            break;
                    }
                }
            }
            if (monster.data.hero_talentList != null)
            {
                (int, int) Proportion = monster.GetComponent<BattleHealthState>().Proportion();
                foreach (var item in monster.data.hero_talentList.Keys)
                {
                    switch (item)
                    {
                        case enum_talent_offect_list.临时伤害:
                            
                            break;
                        case enum_talent_offect_list.临时防御:
                            int value = (100 - Proportion.Item1) / 10 * monster.data.hero_talentList[item];
                            if (value > 0 && value <= 100)
                            {
                                def = (int)(def * (value + 100) / 100);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            damage = (damage + monster.data.def_buff) * skilldamage / 100 - def;
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
                        value = (int)item.Item3;
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
            int value = lucky * 5;
            if (lucky == 9)
            { 
                value = 100;
            }

            if (lucky == 10) return (int)(max * 1.2f);//运10是1.2倍伤害

            return Random.Range(min + ((max - min) * value / 100), max);
        }
    }
}
