using Common;
using MVC;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class tool_Categoryt : MonoBehaviour
{

    public static tool_Categoryt Tool;
    public T Find<T>(string name)
    {
        if (transform.Find(name) == null)
        {
            Debug.LogError(this + " 子对象: " + name + " 没有找到!");
            return default(T);
        }
        return transform.Find(name).GetComponent<T>();
    }
    /// <summary>
    /// 获得单位
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static string Obtain_unit(int index)
    {
        string dec = "";
        switch ((enum_skill_attribute_list)index)
        {
           
            default:
                break;
        }

        switch ((enum_skill_attribute_list)index)
        {
            case enum_skill_attribute_list.生命值:
                break;
            case enum_skill_attribute_list.法力值:
                break;
            case enum_skill_attribute_list.内力值:
                break;
            case enum_skill_attribute_list.蓄力值:
                break;
            case enum_skill_attribute_list.物理防御:
                break;
            case enum_skill_attribute_list.魔法防御:
                break;
            case enum_skill_attribute_list.物理攻击:
                break;
            case enum_skill_attribute_list.魔法攻击:
                break;
            case enum_skill_attribute_list.生命回复:
                break;
            case enum_skill_attribute_list.法力回复:
                break;
            case enum_skill_attribute_list.命中:
                break;
            case enum_skill_attribute_list.躲避:
                break;
            case enum_skill_attribute_list.穿透:
                break;
            case enum_skill_attribute_list.格挡:
                break;
            case enum_skill_attribute_list.暴击:
                break;
            case enum_skill_attribute_list.幸运:
                break;
            case enum_skill_attribute_list.真实伤害:
                break;
            case enum_skill_attribute_list.伤害减免:
                break;
            case enum_skill_attribute_list.异常抗性:
            case enum_skill_attribute_list.暴击伤害:

            case enum_skill_attribute_list.伤害加成:
                dec = "%";
                break;
            case enum_skill_attribute_list.伤害吸收:
                break;
            case enum_skill_attribute_list.攻击速度:
                break;
            case enum_skill_attribute_list.移动速度:
                break;
            case enum_skill_attribute_list.生命加成:
            case enum_skill_attribute_list.法力加成:
            case enum_skill_attribute_list.物攻加成:
            case enum_skill_attribute_list.魔攻加成:
            case enum_skill_attribute_list.物防加成:
            case enum_skill_attribute_list.魔防加成:
                dec = "%";
                break;
            case enum_skill_attribute_list.土属性强化:
                break;
            case enum_skill_attribute_list.火属性强化:
                break;
            case enum_skill_attribute_list.水属性强化:
                break;
            case enum_skill_attribute_list.木属性强化:
                break;
            case enum_skill_attribute_list.金属性强化:
                break;
            case enum_skill_attribute_list.经验加成:
            case enum_skill_attribute_list.装备掉落:
            case enum_skill_attribute_list.极品宠物掉落:
            case enum_skill_attribute_list.人物历练:
            case enum_skill_attribute_list.宠物经验:
            case enum_skill_attribute_list.内功经验:
            case enum_skill_attribute_list.灵珠收益:
            case enum_skill_attribute_list.装备爆率:
            case enum_skill_attribute_list.宠物获取:
            case enum_skill_attribute_list.云游商人折扣:
            case enum_skill_attribute_list.祈愿收益:
            case enum_skill_attribute_list.奇遇任务收益:
            case enum_skill_attribute_list.游历危险躲避率:
            case enum_skill_attribute_list.游历双倍获得率:
                dec = "%";
                break;
            case enum_skill_attribute_list.游历时长:
                break;
            case enum_skill_attribute_list.游历龙珠收益:
                dec = "%";
                break;
            case enum_skill_attribute_list.寻怪间隔:
                dec = "/10s";
                break;
            case enum_skill_attribute_list.宠物容量:
                break;
            case enum_skill_attribute_list.土:
                break;
            case enum_skill_attribute_list.火:
                break;
            case enum_skill_attribute_list.水:
                break;
            case enum_skill_attribute_list.木:
                break;
            case enum_skill_attribute_list.金:
                break;
            case enum_skill_attribute_list.五行伤害:
            case enum_skill_attribute_list.五行伤害减少:
                dec = "%";
                break;
            case enum_skill_attribute_list.灵力:
                break;
            case enum_skill_attribute_list.体魄:
                break;
            case enum_skill_attribute_list.神识:
                break;
            case enum_skill_attribute_list.宠物攻击:
                break;
            case enum_skill_attribute_list.宠物防御:
                break;
            case enum_skill_attribute_list.宠物生命:
                break;
            case enum_skill_attribute_list.宠物暴击:
                break;
            case enum_skill_attribute_list.宠物暴击伤害:
                dec = "%";
                break;
            case enum_skill_attribute_list.宠物暴击率:
                break;
            case enum_skill_attribute_list.宠物攻击速度:
                break;
            case enum_skill_attribute_list.技能伤害:
                dec = "%";
                break;
            case enum_skill_attribute_list.燃血:
                break;
            case enum_skill_attribute_list.灵身:
                break;
            case enum_skill_attribute_list.连击:
                break;
            case enum_skill_attribute_list.受到减免伤害:
                break;
            case enum_skill_attribute_list.复活次数:
                break;
            case enum_skill_attribute_list.幸运一击的概率:
                dec = "%";

                break;
            case enum_skill_attribute_list.幸运一击的伤害:
                dec = "%";

                break;
            case enum_skill_attribute_list.攻击时概率抵消伤害:
                dec = "%";

                break;
            case enum_skill_attribute_list.被攻击时反击真实伤害:
                break;
            case enum_skill_attribute_list.每次攻击增加伤害:
                dec = "%";

                break;
            case enum_skill_attribute_list.治疗术效果:
            case enum_skill_attribute_list.施毒术效果:
            case enum_skill_attribute_list.青云门技能伤害:
            case enum_skill_attribute_list.魔法盾效果:
            case enum_skill_attribute_list.血刀刀法伤害:
                dec = "%";

                break;
            case enum_skill_attribute_list.鞭尸概率:
                dec = "%";
                break;
            case enum_skill_attribute_list.无视防御:
                dec = "";
                break;
            case enum_skill_attribute_list.灵气上限:
                break;
            case enum_skill_attribute_list.空白:
            case enum_skill_attribute_list.攻击回血:
            case enum_skill_attribute_list.攻击回蓝:
            case enum_skill_attribute_list.最终伤害:
            case enum_skill_attribute_list.物品双倍掉落概率:
                dec = "%";
                break;
            case enum_skill_attribute_list.吐纳经验:
                break;
            case enum_skill_attribute_list.法宝匣数量:
                break;
            case enum_skill_attribute_list.厚土大法技能伤害:
            case enum_skill_attribute_list.蛊毒大法技能伤害:
            case enum_skill_attribute_list.血河大法技能伤害:
                dec = "%";
                break;
            case enum_skill_attribute_list.复活:
                break;
            case enum_skill_attribute_list.强化费用:
                dec = "%";
                break;
            case enum_skill_attribute_list.离线间隔:
                dec = "h";
                break;
            case enum_skill_attribute_list.签到收益:
                dec = "%";
                break;
            case enum_skill_attribute_list.月卡:
                dec = "h";
                break;

            default:
                break;
        }
        return dec;
    }
    /// <summary>
    /// 任务完成进度
    /// </summary>
    /// <param name="index"></param>
    public static void Base_Task(int index)
    {

        if (SumSave.crt_greenhand.crt_task == index)//完成当前任务开启下个任务
        {
            if(SumSave.crt_greenhand.crt_progress<SumSave.GreenhandGuide_TotalTasks[index].progress)
            {
                SumSave.crt_greenhand.crt_progress++;
            }
            
        }
    }

    /// <summary>
    /// 获取数据列表
    /// </summary>
    /// <param name="bag"></param>
    public static Bag_Base_VO Read_Bag(string user_value)
    {
        Bag_Base_VO bag_base= new Bag_Base_VO();
        string[] splits =user_value.Split(' ');
        if (splits.Length > 1)
        {
            foreach (var item in SumSave.db_stditems)
            {
                if (item.Name == splits[0])
                {
                    bag_base = new Bag_Base_VO(item.hp,item.mp,item.ac,item.ac2,item.mac,item.mac2,item.dc,item.dc2,item.sc,item.sc2,item.mc,item.mc2);
                    bag_base.Name = item.Name;
                    bag_base.StdMode = item.StdMode;
                    bag_base.Shape = item.Shape;
                    bag_base.job = item.job;
                    bag_base.need_lv = item.need_lv;
                    bag_base.equip_lv= item.equip_lv;
                    bag_base.price= item.price;
                    bag_base.suit = item.suit;
                    bag_base.user_value = user_value;
                    return bag_base;
                }
             }
        }
        return bag_base;
    }
    /// <summary>
    /// 读取技能
    /// </summary>
    /// <param name="base_item"></param>
    /// <returns></returns>
    public static base_skill_vo Read_skill(base_skill_vo base_item)
    {
        base_skill_vo skill = new base_skill_vo();
        string[] slits = base_item.user_value.Split(' ');
        if (slits.Length > 1)
        {
            foreach (var item in SumSave.old_db_skills)
            {
                if (item.skillname == slits[0])
                {
                    skill = item;
                    skill.user_value = base_item.user_value;
                    skill.user_values = slits;

                    return skill;
                }
            }
        }
        return skill;
    }

    public static db_pet_vo_old crate_Pet(string name)
    { 
        db_pet_vo_old pet = new db_pet_vo_old();

        return pet;
    }
    /// <summary>
    /// 创建装备
    /// </summary>
    /// <param name="bag"></param>
    public static Bag_Base_VO crate_equip(string bag_name,int lv=-1, user_map_vo map = null)
    {
        Bag_Base_VO bag = new Bag_Base_VO();
        foreach (var item in SumSave.db_stditems)
        {
            if (item.Name == bag_name)
            {
                bag = new Bag_Base_VO(item);
                continue;  
            }
        }
        int quality = Quality();
        if (lv != -1) quality = lv;
        bag.user_value = Obtain_Equip(bag, 0, quality);
        //string user_value = bag_name;
        ////强化等级
        //user_value += " " + 0;
        ////品质
        //user_value += " " + quality;
        //if (quality > 0)
        //{
        //    List<(int, int)> list = new List<(int, int)>();
        //    //随机属性 1物攻下 2物攻上 3魔攻下 4魔攻上 5物防  6魔防 7生命8魔法
        //    int base_quality = Random.Range(1, 9);
        //    int base_quality_value = Random.Range(bag.need_lv+5, bag.need_lv * 2 + 10);
        //    if (base_quality > 6) base_quality_value *= 10;
        //    list.Add((base_quality, base_quality_value));
        //    if (quality > 1)
        //    {
        //        base_quality = Random.Range(8, 15);
        //        base_quality_value = Random.Range(3, bag.need_lv / 2 + 6);
        //        if (base_quality <= 9) base_quality_value *= 10;
        //        list.Add((base_quality, base_quality_value));
        //        if (quality > 2)
        //        {
        //            base_quality = Random.Range(8, 15);
        //            base_quality_value = Random.Range(3, bag.need_lv / 2 + 6);
        //            if (base_quality <= 9) base_quality_value *= 10;
        //            list.Add((base_quality, base_quality_value));
        //            if( quality > 3)
        //            {
        //                base_quality = Random.Range(19, 30);
        //                base_quality_value = Random.Range(3, 6);
        //                list.Add((base_quality, base_quality_value));
        //                if (quality > 4)
        //                {
        //                    base_quality = Random.Range(19, 30);
        //                    base_quality_value = Random.Range(3, 6);
        //                    list.Add((base_quality, base_quality_value));
        //                    if (quality > 5)
        //                    {
        //                        base_quality = Random.Range(19, 30);
        //                        base_quality_value = Random.Range(3, 6);
        //                        list.Add((base_quality, base_quality_value));
        //                        if (quality > 6)
        //                        {
        //                            //五行
        //                            base_quality = Random.Range(30, 35);
        //                            base_quality_value = Random.Range(3, bag.need_lv / 2 + 6);
        //                            list.Add((base_quality, base_quality_value));
        //                        }
        //                    }
        //                }
        //            }
        //        } 
        //    }
        //    string type = "", value = "";
        //    int index = 0;
        //    foreach (var item in list)
        //    {
        //        index++;
        //        type += item.Item1 + (index == list.Count ? "" : ",");
        //        value += item.Item2 + (index == list.Count ? "" : ",");
        //    }
        //    user_value += " " + type + " " + value;
        //}
        ////是否可以锁定
        //user_value += " " + 0;
        //bag.user_value = user_value;
        return bag;
    }
    /// <summary>
    /// 洗练装备属性
    /// </summary>
    /// <param name="bag">物品基础信息</param>
    /// <param name="lv">强化等级</param>
    /// <param name="quality">装备品质</param>
    /// <param name="islock">是否锁定</param>
    /// <returns></returns>
    public static string Obtain_Equip(Bag_Base_VO bag,int lv, int quality,int islock=0)
    {
        string user_value = bag.Name;
        //强化等级
        user_value += " " + lv;
        //品质
        user_value += " " + quality;
        if (quality > 0)
        {
            List<(int, int)> list = new List<(int, int)>();
            //随机属性 1物攻下 2物攻上 3魔攻下 4魔攻上 5物防  6魔防 7生命8魔法
            int base_quality = Random.Range(1, 9);
            int base_quality_value = Random.Range(bag.need_lv + 5, bag.need_lv * 2 + 10);
            if (base_quality > 6) base_quality_value *= 10;
            list.Add((base_quality, base_quality_value));
            if (quality > 1)
            {
                base_quality = Random.Range(8, 15);
                base_quality_value = Random.Range(3, bag.need_lv / 2 + 6);
                if (base_quality <= 9) base_quality_value *= 10;
                list.Add((base_quality, base_quality_value));
                if (quality > 2)
                {
                    base_quality = Random.Range(8, 15);
                    base_quality_value = Random.Range(3, bag.need_lv / 2 + 6);
                    if (base_quality <= 9) base_quality_value *= 10;
                    list.Add((base_quality, base_quality_value));
                    if (quality > 3)
                    {
                        base_quality = Random.Range(19, 30);
                        base_quality_value = Random.Range(3, 6);
                        list.Add((base_quality, base_quality_value));
                        if (quality > 4)
                        {
                            base_quality = Random.Range(19, 30);
                            base_quality_value = Random.Range(3, 6);
                            list.Add((base_quality, base_quality_value));
                            if (quality > 5)
                            {
                                base_quality = Random.Range(19, 30);
                                base_quality_value = Random.Range(3, 6);
                                list.Add((base_quality, base_quality_value));
                                if (quality > 6)
                                {
                                    //五行
                                    base_quality = Random.Range(30, 35);
                                    base_quality_value = Random.Range(3, bag.need_lv / 2 + 6);
                                    list.Add((base_quality, base_quality_value));
                                }
                            }
                        }
                    }
                }
            }
            string type = "", value = "";
            int index = 0;
            foreach (var item in list)
            {
                index++;
                type += item.Item1 + (index == list.Count ? "" : ",");
                value += item.Item2 + (index == list.Count ? "" : ",");
            }
            user_value += " " + type + " " + value;
        }
        //是否可以锁定
        user_value += " " + islock;
        return user_value;
    }
    public static Bag_Base_VO crate_equip(string bag_name, bool boss=false, bool isSuperlative=false, user_map_vo map = null)
    {
        Bag_Base_VO bag = new Bag_Base_VO();
        foreach (var item in SumSave.db_stditems)
        {
            if (item.Name == bag_name)
            {
                bag = new Bag_Base_VO(item);
                continue;
            }
        }
        string user_value = bag_name;
        //强化等级
        user_value += " " + 0;
        //品质
        int quality = Quality(boss);

        if(map!=null)
        {
            if (map.map_type != 4)
            {
                if (isSuperlative && Combat_statistics.isSuperlative())
                {
                    //Game_Omphalos.i.Alert_Show("宝箱生效,获得 " + Show_Color.Yellow(enum_equip_quality_list.神话) + " " + bag_name);
                    quality = 6;
                    Combat_statistics.ClearSuperlative();
                }
            }
        }
        
      
        user_value += " " + quality;

        if (quality > 0)
        {
            List<(int, int)> list = new List<(int, int)>();
            //随机属性 1物攻下 2物攻上 3魔攻下 4魔攻上 5物防  6魔防 7生命8魔法
            int base_quality = Random.Range(1, 9);
            int base_quality_value = Random.Range(bag.need_lv + 5, bag.need_lv * 2 + 10);
            if (base_quality > 6) base_quality_value *= 10;
            list.Add((base_quality, base_quality_value));
            if (quality > 1)
            {
                base_quality = Random.Range(8, 15);
                base_quality_value = Random.Range(3, bag.need_lv / 2 + 6);
                if (base_quality <= 9) base_quality_value *= 10;
                list.Add((base_quality, base_quality_value));
                if (quality > 2)
                {
                    base_quality = Random.Range(8, 15);
                    base_quality_value = Random.Range(3, bag.need_lv / 2 + 6);
                    if (base_quality <= 9) base_quality_value *= 10;
                    list.Add((base_quality, base_quality_value));
                    if (quality > 3)
                    {
                        base_quality = Random.Range(19, 30);
                        base_quality_value = Random.Range(3, 6);
                        list.Add((base_quality, base_quality_value));
                        if (quality > 4)
                        {
                            base_quality = Random.Range(19, 30);
                            base_quality_value = Random.Range(3, 6);
                            list.Add((base_quality, base_quality_value));
                            if (quality > 5)
                            {
                                base_quality = Random.Range(19, 30);
                                base_quality_value = Random.Range(3, 6);
                                list.Add((base_quality, base_quality_value));
                                if (quality > 6)
                                {
                                    //五行
                                    base_quality = Random.Range(30, 35);
                                    base_quality_value = Random.Range(3, bag.need_lv / 2 + 6);
                                    list.Add((base_quality, base_quality_value));
                                }
                            }
                        }
                    }
                }
                string type = "", value = "";
                int index = 0;
                foreach (var item in list)
                {
                    index++;
                    type += item.Item1 + (index == list.Count ? "" : ",");
                    value += item.Item2 + (index == list.Count ? "" : ",");
                }
                user_value += " " + type + " " + value;
            }
        }
        else
        {
        }
        //是否可以锁定
        user_value += " " + 0;
        bag.user_value = user_value;
        return bag;
    }

    /// <summary>
    /// 创建技能
    /// </summary>
    /// <param name="skill_name"></param>
    /// <returns></returns>
    public static base_skill_vo crate_skill(string skill_name,bool task=true)
    {
        base_skill_vo skill = ArrayHelper.Find(SumSave.old_db_skills, e => e.skillname == skill_name);
        /// 1技能名称 2技能等级 3技能位置 4技能内力 5技能类型 6技能伤害类型 7技能最大等级 8技能初始化升级经验 9技能升级
        skill.user_value = skill_name;
        //等级
        skill.user_value += " " + (task ? 1 : skill.skill_max_lv);
        //位置 0不上场
        skill.user_value += " " + 0;
        //分配内力 默认为0
        skill.user_value += " " + 0;

        string[] item = skill.user_value.Split(' ');
        skill.user_values= new string[item.Length];
        for (int i = 0; i < item.Length; i++)//拆解成数组添加到user_values
        {
            skill.user_values[i] = item[i];
        }
        tool_Categoryt.Base_Task(1003);

        return skill;

    }
    /// <summary>
    /// 获取随机数
    /// </summary>
    /// <returns></returns>
    public static int Obtain_Random()
    { 
        return Random.Range(1, 10000);
    }
    /// <summary>
    /// 获取装备品质
    /// </summary>
    /// <param name="boss"></param>
    /// <returns></returns>
    public static int Quality(bool boss = false)
    {
        int random = Random.Range(10000 * SumSave.titleLucky / 100, 100001);
        int[] needs = new int[] { 0, 23000, 43000, 61000, 75000, 90000, 99600, 100001 };
        int result = 1;
        for (int i = 0; i < needs.Length; i++)
        {
            if (random > needs[i] + (boss ? 0 : 390)) result = Mathf.Max(result, i + 1);
        }
        if (boss)
        {
            result = Mathf.Max(4, result);

            if (Random.Range(0, 100) < 10)
            {
                result = 7;
            }
        }
        result = Math.Clamp(result, 1, 7);
        return result; 
    }
}
