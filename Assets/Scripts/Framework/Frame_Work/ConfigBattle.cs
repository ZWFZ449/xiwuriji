using System;
using System.Collections.Generic;
using System.IO;
using Common;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MVC
{
    /// <summary>
    /// 读取文件
    /// </summary>
    public static class ConfigBattle
    {
        /*
        /// <summary>
        /// 掉落金币
        /// </summary>
        public static int Money = 0;
        /// <summary>
        /// 掉落物品
        /// </summary>  
        public static Dictionary<string, string[]> CalculationBattle = new Dictionary<string, string[]>();

        /// <summary>
        /// 保底值
        /// </summary>
        private static Dictionary<string, (int, string[])> keyValuePairs = new Dictionary<string, (int, string[])>();
        /// <summary>
        /// 掉落集合
        /// </summary>
        public static List<string> Calculations;
        public static void Instance()
        {
            CalculationBattle.Clear();
        }
        /// <summary>
        ///  读取设置
        /// </summary>
        /*public static List<string> LoadSetting(BattleAttack monster, int number)
        {
            user_map_vo map = ArrayHelper.Find(SumSave.read_lose_map, e => e.map_index == monster.Data.map_index);
            if (map == null) return null;
            string base_name = map.map_name;
            if (!CalculationBattle.ContainsKey(base_name))
            {
                CalculationBattle.Add(base_name, map.ProfitList.Split('&'));
            }
            Calculations = new List<string>();
            //计算概率
            int count = 1;
            if (Tool_State.Is_playerprobabilit(enum_skill_attribute_list.鞭尸概率))
            {
                count = 2;
                Game_Omphalos.i.Alert_Show("鞭尸成功");
            }
            for (int Z = 0; Z < count; Z++)
            {
                for (int i = 0; i < (number); i++)
                {
                    string countEquip = CalculationBattle[base_name][Random.Range(0, CalculationBattle[base_name].Length)];
                    CalculationBag(countEquip, monster.Data.Monster_Lv == 3, 1, map.map_type == 1,map);
                }
                if (map.Independent_Drop != "")//计算独立掉落
                {
                    Independent_Drop(map.Independent_Drop, number, map.map_type == 1, map);
                }
                Show_Info();
            }
            return Calculations;
        }
        /// <summary>
        /// 独立掉落
        /// </summary>
        /// <param name="independent_Drop"></param>
        private static void Independent_Drop(string independent_Drop,int number,bool isSuperlative,user_map_vo map)
        {
            string[] values = independent_Drop.Split('&');
            for (int i = 0; i < number; i++)
            {
                string countEquip = values[Random.Range(0, values.Length)];
                //Debug.Log(countEquip);
                CalculationBag(countEquip, false,2, isSuperlative,map);
            }
        }

        /// <summary>
        /// 离线收益
        /// </summary>
        public static List<string> Offline(int number)
        {
            string base_name = SumSave.crt_resources.user_map_index;
            if (!CalculationBattle.ContainsKey(SumSave.crt_resources.user_map_index))
            {
                CalculationBattle.Add(base_name, ArrayHelper.Find(SumSave.read_lose_map, e => e.map_name == base_name).ProfitList.Split('&'));
            }
            Calculations = new List<string>();
            for (int i = 0; i < number; i++)
            {
                string countEquip = CalculationBattle[base_name][Random.Range(0, CalculationBattle[base_name].Length)];
                CalculationBag(countEquip, false);
            }
            return Calculations;
        }
        /// <summary>
        /// 获取物品掉落
        /// </summary>
        /// <param name="line"></param>
        private static (bool,string) Obtain_ProfitList(string line,int state)
        {
            (bool,string) result = (false, null);
            string[] values1 = line.Split(' ');
            if (values1.Length == 3)
            {
                string[] values2 = values1[1].Split('/');
                if (values2.Length > 1)
                {
                    int probability = int.Parse(values2[0]);
                    if (state == 1)
                    {

                        probability += Tool_State.Value_playerprobabilit(enum_skill_attribute_list.装备爆率);
                        probability = (int)MathF.Min(int.Parse(values2[1]) / 10, probability);
                    }
                    if (Random.Range(0, int.Parse(values2[1])) < probability)
                    {
                        result = (true, values1[0]);
                        return result;
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 掉落配置
        /// </summary>
        /// <param name="line">掉落列表</param>
        /// <param name="boss">是否boss</param>
        /// <param name="state">是否需要加成爆率默认增加2不增加</param>
        /// /// <param name="isSuperlative">是否是超级boss</param>
        private static void CalculationBag(string line,bool boss,int state = 1,bool isSuperlative=true, user_map_vo map=null)
        {

            (bool, string) result = Obtain_ProfitList(line, state);
            if (isSuperlative && Combat_statistics.isSuperlative())
            {
                result.Item1 = true;
            } 
            if (!result.Item1||result.Item2 == null) return;
            Bag_Base_VO bag = new Bag_Base_VO();
            bag = ArrayHelper.Find(SumSave.db_stditems, e => e.Name == result.Item2);
            if (bag == null) return;
            bool exist = true;
            switch ((EquipConfigTypeList)Enum.Parse(typeof(EquipConfigTypeList), bag.StdMode))
            {
                case EquipConfigTypeList.武器:
                case EquipConfigTypeList.衣服:
                case EquipConfigTypeList.头盔:
                case EquipConfigTypeList.项链:
                case EquipConfigTypeList.护臂:
                case EquipConfigTypeList.戒指:
                case EquipConfigTypeList.手镯:
                case EquipConfigTypeList.扳指:
                case EquipConfigTypeList.腰带:
                case EquipConfigTypeList.靴子:
                case EquipConfigTypeList.护符:
                case EquipConfigTypeList.灵宝:
                case EquipConfigTypeList.勋章:
                case EquipConfigTypeList.饰品:
                case EquipConfigTypeList.玉佩:
                case EquipConfigTypeList.披风:
                    bag = tool_Categoryt.crate_equip(bag.Name, boss, isSuperlative, map);
                    break;
                default:
                    exist = false;
                    break;
            }
            if (exist)
            {
                int quality = int.Parse(bag.user_value.Split(' ')[2]);
                Combat_statistics.AddBag(1);
                //if (SumSave.crt_resources.pages[0] > SumSave.crt_bag.Count)
                //{
                //    //判断回收
                //    if (SumSave.crt_setting.user_setting[0] < quality)
                //    {
                //        SumSave.crt_bag.Add(bag);
                //        Game_Omphalos.i.Wirte_ResourcesList(Emun_Resources_List.bag_value, SumSave.crt_bag);
                //        Calculations.Add("获得 " + (enum_equip_quality_list)quality + " " + bag.Name);
                //    }
                //    else
                //    {
                //        Calculations.Add("过滤 " + (enum_equip_quality_list)quality + " " + bag.Name);
                //        Calculations.Add("过滤 收益 灵珠 + " + bag.price);
                //        Battle_Tool.Obtain_Unit(currency_unit.灵珠, bag.price,2);
                //    }
                //}
                //else Calculations.Add("丢弃 " + (enum_equip_quality_list)quality + " " + bag.Name);
            }
            else
            {
                //回收宠物蛋
                if (SumSave.crt_settingold.user_setting[9] == 1)
                {
                    if (bag.StdMode == EquipConfigTypeList.宠物蛋.ToString())
                    {
                        
                        Calculations.Add("丢失 " + bag.Name + " * " + 1);
                        return;
                    }
                }
                if ( map!=null&&map.map_type != 4)
                {
                    if (Combat_statistics.isSuperlative())
                    {
                        Game_Omphalos.i.Alert_Show("宝箱生效,获得 " + bag.Name);

                        Combat_statistics.ClearSuperlative();
                    }
                }
                //获取材料
                int random = Random.Range(1, 100);
                int number = 1;
                int maxnumber = number + Random.Range(1, 100);
                Battle_Tool.Obtain_Resources(Obtain_Int.Add(1, bag.Name, new int[] { number + random, random }), maxnumber);
                //Battle_Tool.Obtain_Resources(bag.Name, 1);
                ObtainEquipmentTasks(bag);
                Calculations.Add("获得 " + bag.Name + " * " + 1);
                
            }

        }
        /// <summary>
        /// 获得装备任务
        /// </summary>
        private static void ObtainEquipmentTasks(Bag_Base_VO bag)
        {
           
            if(bag.Name == "无影蝉蜕")
            {
                tool_Categoryt.Base_Task(1033);
            }
            if (bag.Name == "破军七劫")
            {
                tool_Categoryt.Base_Task(1048);
            }
            if (bag.Name == "青冥断刃碎片")
            {
                tool_Categoryt.Base_Task(1055);

            }
            if (bag.Name == "冥君诏令通行证")
            {
                tool_Categoryt.Base_Task(1056);
            }
            if (bag.Name == "龙骸密匙通行证")
            {
                tool_Categoryt.Base_Task(1058);
            }
            if (bag.Name == "缚魂玉")
            {
                tool_Categoryt.Base_Task(1065);
            }
        }


        /// <summary>
        /// 显示获取信息
        /// </summary>
        /// <param name="bag"></param>
        private static void Obtian_Bag(Bag_Base_VO bag)
        { 
            string[] keyValue = bag.user_value.Split(' ');
            //Show_Info("获得 " + (enum_equip_quality_list)int.Parse(keyValue[2]) + " " + bag.Name);
            //Show_Info(bag.Name);
        }

        private static void Show_Info()
        { 
            //Alert_Dec.Show(info);
            //Alert_Icon.Show(Calculations);
         
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="line"></param>
        private static void Read(string line)
        {
            if (line.Length <= 0) return;

            //vs.Add(line);

            string[] keyValue = line.Split(' ');

            if (keyValue.Length <= 1) return;
        }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="content">配置文件内容</param>
        /// <param name="handle">每行处理逻辑</param>
        public static void ReadConfig(string content, Action<string> lineHandle)
        {
            //字符串读取器
            using (StringReader sr = new StringReader(content))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    lineHandle(line);
                }
            }
        }

    }
}
        */
    }
}