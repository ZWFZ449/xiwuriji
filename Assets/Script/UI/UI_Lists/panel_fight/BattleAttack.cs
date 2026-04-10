using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using StateMachine;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MVC
{
    /// <summary>
    /// 战斗模式
    /// </summary>
    /// 
    public class BattleAttack : Base_Mono
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
        public void FindTergets(List<BattleHealth> tergets,int isBackstab=0)
        { 
            Tergets = tergets;
            if(isBackstab==1)
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
        protected Text Name, sliderInfo, damageInfo;
        /// <summary>
        /// 计数器
        /// </summary>
        protected Slider show_hp;
        /// <summary>
        /// 生命值显示文本
        /// </summary>
        protected Text hp_text;

        /// <summary>
        /// 角色状态机
        /// </summary>
        public AttackStateMachine AttackStateMachine;
        public RolesManage StateMachine;
        private Text name_text;

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
            StateMachine= GetComponent<RolesManage>();
            frame = Find<Image>("frame");
            show_hp = Find<Slider>("Slider");
            name_text = Find<Text>("base_info/info");
            hp_text = Find<Text>("Slider/Hp_text");
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
                target.EnergymaxMp=data.EnergyMp;
                target.internalforcemaxMP = data.internalforceMP;
                target.internalforceMP= data.internalforceMP;
                show_hp.maxValue = target.maxHP;
                show_hp.value = target.HP;
                hp_text.text = Battle_Tool.FormatNumberToChineseUnit(target.HP) + "/" + Battle_Tool.FormatNumberToChineseUnit(target.maxHP);
                target.maxMP= data.MaxMp;
                target.MP= data.MaxMp;
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
                            dec += " " + Show_Color.Yellow((enum_skill_attribute_list)(201 + i)+"(" + data.life[i] + ")");
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
                            GameObject game = Resources.Load<GameObject>("Prefabs/halo/halo_" + (i+1));
                            GameObject tianming = Instantiate(game, show_tianming_Platform);
                            pos_tianming_size = show_tianming_Platform.GetComponent<RectTransform>().rect.size;
                            tianming_size = new Vector2(pos_tianming_size.x , pos_tianming_size.y );
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
            //副本怪物属性自动增长
            if (data.Monster_Lv == 4)
            {
                data.damageMax = (int)(data.damageMax * 1.2f);
                data.hit= (int)(data.hit * 1.2f);
                data.MagicdamageMax= (int)(data.MagicdamageMax * 1.2f);
            }
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
                    if(Tergets[i]==null) continue;
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
                AttackStateMachine.Init(this, Terget);
                StateMachine.Init(this, Terget);
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
            if (health.GetComponent<BattleAttack>().data.Monster_Lv == 4)
            {
                transform.parent.parent.parent.SendMessage("DailyCopies", health);
            }
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
                //target.HP += (skill.skill_damage + (skill.skill_power * lv)) * Data.MagicdamageMax/ 100;
                //if (target.HP > target.maxHP) target.HP = target.maxHP;
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
            damage = damage * (skill.skill_damage + (skill.skill_power * lv)+ Tool_State.Value_playerprobabilit(enum_skill_attribute_list.技能伤害)) / 100;
            //内力伤害
            if (skill.user_values[3] != "")
            {
                int internalforce= int.Parse(skill.user_values[3]);
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
            int life = restrain_value(skill.skill_life-1, monster.Data.life);
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

        private long Base_Damage(BattleAttack monster,base_skill_vo skill=null)
        {
            long damage = 0;
            if (skill == null)
            {
                damage = defense(monster, data.Type, data.isBackstab);
            }else
                damage = defense(monster, skill.skill_damage_type, data.isBackstab);
            damage = (long)Mathf.Max(1, damage);
            damage = damage * (100 + penetrate(monster)) / 100;
            damage = (long)Mathf.Max(1, damage);
            damage = damage * (100 + data.double_damage - monster.data.Damage_Reduction) / 100;
            damage = (long)Mathf.Max(1, damage);
            if (skill == null)
            {
                //int life = restrain_value(monster.Data.life, monster.Data.life_types);
                //if (life < 0) damage = (damage / 2) * (100 + (life)) / 100;
                //else damage = damage * (100 + (life)) / 100;
            }
            damage =(long) Mathf.Max(1, damage);

            if (Tool_State.Value_playerprobabilit(data.bufflist, enum_skill_attribute_list.攻击回血) > 0)
            { 
                target.HealConsumables ( Tool_State.Value_playerprobabilit(data.bufflist, enum_skill_attribute_list.攻击回血),
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
        private long defense(BattleAttack monster, int type,int isBackstab)
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
                            def= (def * (100 - data.equip_suit_lists[item])) / 100;
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
                damage -= skillstate(monster.data, type);
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
                }else value= user.skill_state[type].Item2;
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
            while (user.skill_state.Count < type+1)
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
        private int restrain_value(int[] monsterlife,Dictionary<int,int> dec)
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
        private int battle_restrain_value(int index,int type,float coefficient,int monsterlifevaluelue)
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
            bool exist= false;
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
            bool isCrit= false;
            if (Random.Range(0, 100) < (data.crit_rate - monster.Data.crit_rate)*100 / (data.crit_rate+30))
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
