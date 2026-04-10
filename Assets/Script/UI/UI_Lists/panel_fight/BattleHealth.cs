using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Components;
using StateMachine;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MVC
{
    /// <summary>
    ///  战斗显示血量
    /// </summary>
    public class BattleHealth : Base_Mono
    {
        /// <summary>
        /// 基础数值
        /// </summary>
        [HideInInspector]
        public long maxHP, HP, maxMP, MP, add_hp = 0;

        public int internalforceMP, EnergyMp, internalforcemaxMP, EnergymaxMp;
        /// <summary>
        /// 战斗位置
        /// </summary>
        [HideInInspector]
        public int Pos = 0;

     
        private void Awake()
        {
            HP = maxHP;
            MP = maxMP;
            internalforceMP = internalforcemaxMP;
            EnergyMp = EnergymaxMp;
        }
        public void Clear()
        { 

            StopAllCoroutines();
            PushObjectToPool(GetComponent<BattleAttack>().Data.Push_name);
        }
        /// <summary>
        /// 回收
        /// </summary>
        /// <param name="healthname"></param>
        private void PushObjectToPool(string healthname)
        {
            //Debug.Log("回收  " + healthname);
            ObjectPoolManager.instance.PushObjectToPool(healthname, this.gameObject);

        }
        /// <summary>
        /// 回复生命魔法
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="dec"></param>
        public void HealConsumables(int hp, int mp)
        {
            HP = (long)Mathf.Clamp(HP + hp, 0, maxHP);
            MP = (long)Mathf.Clamp(MP + mp, 0, maxMP);
            if (internalforcemaxMP > 0) internalforceMP = Mathf.Clamp(internalforceMP + 2, 0, internalforcemaxMP);
            if (EnergymaxMp > 0) EnergyMp = Mathf.Clamp(EnergyMp + 1, 0, EnergymaxMp);
        }
        /// <summary>
        /// 消耗蓝
        /// </summary>
        /// <param name="value"></param>
        public void UseMp(float value)
        {
            if (value < 0) value = -value;

            MP -= (long)value;

            MP = (long)Mathf.Max(0, MP);

        }
        public void TakeDamage(long damage, DamageEnum damageEnum )
        {
            if (HP < 0) return;

#if UNITY_EDITOR
            //damage = 100;
#elif UNITY_ANDROID
#elif UNITY_IPHONE
#endif


            HP -= damage;
            AudioManager.Instance.playAudio(ClipEnum.被敌人攻击);

            Hurt(damage, damageEnum);
            //测试掉落
            //ConfigBattle.LoadSetting(monster, 2);
            //if (monster.GetComponent<monster_battle_attck>() != null) WaitAndDestory(monster);
            if (HP <= 0)
            {
                //死亡 掉落
                if (GetComponent<monster_battle_attck>() != null)
                {
                    WaitAndDestory();
                    AudioManager.Instance.playAudio(ClipEnum.男角色死亡);
                }
                else if (GetComponent<player_battle_attck>() != null)
                {

                    SumSave.crt_achievement.increase_date_Exp((Achieve_collect.死亡).ToString(), 1);
                }
                transform.parent.parent.parent.SendMessage("clearhealth", this);
                StartCoroutine(WaitAndDestory(GetComponent<BattleAttack>().Data.Push_name));
            }
        }

      
        /// <summary>
        /// 文字偏移量
        /// </summary>
        int offset = 1;

        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="dec"></param>
        /// <param name="type">1伤害2治疗</param>
        private void Hurt(float dec, DamageEnum type)
        {
            offset *= -1;
            string _dec=dec.ToString("F0");
            DamageTextManager.Instance.ShowDamageText(type, _dec, this.transform, offset);
            if(SumSave.crt_settingold.user_setting[2]==0)
            transform.parent.parent.parent.SendMessage("show_battle_info",
                    GetComponent<BattleAttack>().Data.show_name+" 受到 "+type+" 效果"+"造成"+dec+"伤害");
        }
        /// <summary>
        /// 延时销毁，掉落物品
        /// </summary>
        /// <param name="monster"></param>
        /// <returns></returns>
        private void WaitAndDestory()
        {
            BattleAttack monster = GetComponent<BattleAttack>();
            //结算秘境收益
            if (monster.Data.Monster_Lv == 8)
            {
                transform.parent.parent.parent.SendMessage("settlementSecretRealm", monster);
                return;
            }
            SumSave.crt_achievement.increase_date_Exp((Achieve_collect.击杀怪物).ToString(), 1);
            Battle_Tool.Obtain_Exp(monster.Data.Exp);
            user_map_vo map = ArrayHelper.Find(SumSave.read_lose_map, e => e.map_index == monster.Data.map_index);
            if (map.map_type == 4)
            {
                Battle_Tool.Obtain_Unit(currency_unit.金币, monster.Data.unit);

            }
            else Battle_Tool.Obtain_Unit(currency_unit.金币, monster.Data.unit, 2);
            int number = 1;
            Combat_statistics.AddMaxNumber();

            if (monster.Data.Monster_Lv == 1)
            {
                if (Tool_State.IsState(State_List.至尊卡))
                {
                    int value = Random.Range(1, 3);
                    SumSave.crt_user_unit.verify_data(currency_unit.元宝, value);//monster.Data.Point
                    transform.parent.parent.parent.SendMessage("show_battle_info",
                    "至尊卡击杀 " + monster.Data.show_name + " 获得 " + value + "历练");//monster.Data.Point 
                }
            }
            //判断是否增加历练值
            if (monster.Data.Monster_Lv != 1) 
            {
                number = Random.Range(2, 5);

                if (monster.Data.Monster_Lv == 3)
                {
                    SumSave.crt_achievement.increase_date_Exp((Achieve_collect.击杀Boss).ToString(), 1);
                    number = Random.Range(5, 11);
                    Combat_statistics.AddBossNumber();
                    SumSave.crt_pass.progress(3);//通行证任务
                }
                else if (monster.Data.Monster_Lv == 2)
                {
                    Combat_statistics.AddEliteNumber();
                    SumSave.crt_pass.progress(2);
                }

               if (map.map_type == 3)
                {
                    SumSave.crt_pass.progress(4);
                }
                if (map.map_type == 4)//判断是否是副本
                {
                    Battle_Tool.Obtain_Unit(currency_unit.元宝, monster.Data.Point);
                }
                else
                {
                    Battle_Tool.Obtain_Unit(currency_unit.元宝, monster.Data.Point, 2);
                }
                transform.parent.parent.parent.SendMessage("show_battle_info",
                "击杀 " + monster.Data.show_name + " 获得 " + monster.Data.Point + "历练" + Show_Color.Red(" (+" + (long)(monster.Data.Point * Tool_State.Value_playerprobabilit(enum_skill_attribute_list.人物历练) / 100) + ")"));//monster.Data.Point 
            }
            if (Tool_State.Is_playerprobabilit(enum_skill_attribute_list.物品双倍掉落概率))
            {
                number *= 2;
                Game_Omphalos.i.Alert_Show("获得双倍掉落");
            }

//#if UNITY_EDITOR
           //number = 1000000;
//#endif


            ClearanceMapTask(monster);
            Game_Omphalos.i.GetQueue(
                        Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user, SumSave.crt_user_unit.Set_Uptade_String(), SumSave.crt_user_unit.Get_Update_Character());
            //获取物品掉落
            List<string> lists = ConfigBattle.LoadSetting(monster, number);
           
            //增加经验
            if (SumSave.crt_settingold.user_setting[2] == 0)
            {
                transform.parent.parent.parent.SendMessage("show_battle_info",
            "击杀 " + monster.Data.show_name + " 获得 " + monster.Data.Exp+ Show_Color.Red(" (+" + (long)(monster.Data.Exp * Tool_State.Value_playerprobabilit(enum_skill_attribute_list.经验加成) / 100) + ")")+ "经验");
                transform.parent.parent.parent.SendMessage("show_battle_info",
            "击杀 " + monster.Data.show_name + " 获得 " + monster.Data.unit + Show_Color.Red(" (+" + (long)(monster.Data.unit * Tool_State.Value_playerprobabilit(enum_skill_attribute_list.灵珠收益) / 100) + ")")+ "灵珠");
            }
            if (lists.Count > 0)
            {
                for (int i = 0; i < lists.Count; i++)
                {
                    transform.parent.parent.parent.SendMessage("show_battle_info", lists[i]);
                }
            }
        }
        /// <summary>
        /// 击杀怪物任务
        /// </summary>
        private static void KillMonsterMission(BattleAttack monster)
        {
            tool_Categoryt.Base_Task(1006);
            tool_Categoryt.Base_Task(1008);
            tool_Categoryt.Base_Task(1010);
            tool_Categoryt.Base_Task(1011);
            tool_Categoryt.Base_Task(1015);


            if (monster.Data.show_name == "昏眼牛")
            {
                tool_Categoryt.Base_Task(1017);
            }
            if (monster.Data.show_name == "扎纸鬼")
            {
                tool_Categoryt.Base_Task(1018);
            }
            if (monster.Data.show_name == "黑爪猫")
            {
                tool_Categoryt.Base_Task(1019);
            }
            if (monster.Data.show_name == "黑鳞君")
            {
                tool_Categoryt.Base_Task(1031);
            }
            if (monster.Data.show_name == "刺皮将")
            {
                tool_Categoryt.Base_Task(1032);
            }
            if (monster.Data.show_name == "血刀鬼")
            {
                tool_Categoryt.Base_Task(1038);
            }
            if (monster.Data.show_name == "独角魔")
            {
                tool_Categoryt.Base_Task(1045);
            }
            if (monster.Data.show_name == "肉瘤怪")
            {
                tool_Categoryt.Base_Task(1046);
            }
            if (monster.Data.show_name == "断角尊")
            {
                tool_Categoryt.Base_Task(1047);
            }
            if (monster.Data.show_name == "七爪狼")
            {
                tool_Categoryt.Base_Task(1053);
            }
            if (monster.Data.show_name == "玄铁帅")
            {
                tool_Categoryt.Base_Task(1061);
            }
            if (monster.Data.show_name == "血喉尸")
            {
                tool_Categoryt.Base_Task(1079);
            }
            if (monster.Data.show_name == "地龙虫")
            {
                tool_Categoryt.Base_Task(1081);
            }
        }

        /// <summary>
        /// 通关地图任务
        /// </summary>
        private static void ClearanceMapTask(BattleAttack monster)
        {
            foreach (var item in SumSave.GreenhandGuide_TotalTasks.Keys)
            {
                if (SumSave.GreenhandGuide_TotalTasks[item].tasktype == GreenhandGuideTaskType.击杀怪物)
                { 
                    GreenhandGuide_TotalTaskVO task = SumSave.GreenhandGuide_TotalTasks[item];//读取任务
                    if (task.TaskDesc.Contains(monster.Data.show_name))//判断是否是当前地图
                    {
                        tool_Categoryt.Base_Task(task.taskid);
                    }
                }else 
                if (SumSave.GreenhandGuide_TotalTasks[item].tasktype == GreenhandGuideTaskType.通关地图)
                {
                    if (monster.Data.Monster_Lv == 3)//判断是否是当前地图
                    {
                        GreenhandGuide_TotalTaskVO task = SumSave.GreenhandGuide_TotalTasks[item];//读取任务
                        user_map_vo map = SumSave.read_lose_map.Find(x => x.map_index == monster.Data.map_index);//读取地图
                        if (task.TaskDesc.Contains(map.map_name))//判断是否是当前地图
                        {
                            tool_Categoryt.Base_Task(task.taskid);
                        }
                    }
                        
                }
            }

           
            if (monster.Data.Monster_Lv == 3)//判断是否是当前地图
            {
                ///通关地图成就
                Array enumValues = Enum.GetValues(typeof(Achieve_collect));
                for (int i = 0; i < enumValues.Length; i++)
                {
                    user_map_vo map = SumSave.read_lose_map.Find(x => x.map_index == monster.Data.map_index);//读取地图
                    string enumName = enumValues.GetValue(i).ToString(); // 获取枚举的字符串值
                    if (enumName.Contains(map.map_name))
                    {
                        SumSave.crt_achievement.increase_date_Exp(enumName, 1);
                    }
                }
            }
        }



        /// <summary>
        /// 删除销毁
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator WaitAndDestory(string healthname)
        {
            if (gameObject.activeInHierarchy)
            {
                yield return new WaitForSeconds(0.48f);
                PushObjectToPool(healthname);
                transform.parent.parent.parent.SendMessage("clearSumhealth", this);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        public void Destroy()
        {

        }

        private void OnDestroy()
        {
          
        }
        /// <summary>
        ///  是否已经挂掉
        /// </summary>
        public bool Dead
        {
            get { return HP <= 0; }
        }

        public void OnDisable()
        {
            StopAllCoroutines();
        }

    }
}
