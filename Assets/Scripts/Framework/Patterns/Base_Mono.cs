
using UnityEngine;
using TarenaMVC;
using System.Collections.Generic;
using Common;
using System;
using Components;

namespace MVC
{
    /// <summary>
    ///  封装发送消息的功能
    /// </summary>
    public class Base_Mono : MonoBehaviour, INotifier
    {
        /// <summary>
        /// 首次显示消息开关
        /// </summary>
        private bool isfirst = true;
        /// <summary>
        ///  发送消息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public void SendNotification(string name, object data = null)
        {
            AppFacade.I.SendNotification(name, data);
        }

        /// <summary>
        /// 显示
        /// </summary>
        public virtual void Show()
        {
            transform.SetAsLastSibling();
            this.gameObject.SetActive(true);
            if (isfirst)
            {
                for (int i = 0; i < SumSave.db_dec.Count; i++)
                {
                    string gameObjectNameWithoutClone = gameObject.name.Replace("(Clone)", "");
                    if (gameObjectNameWithoutClone == SumSave.db_dec[i].panel_index)
                    {
                        Alert.Show(SumSave.db_dec[i].title, SumSave.db_dec[i].dec);
                        isfirst = false;
                    }
                }
            }

        }
        /// <summary>
        ///  获取组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        protected T Find<T>(string name)
        {
            if (transform.Find(name) == null)
            {
                Debug.LogError(this + " 子对象: " + name + " 没有找到!");
                return default(T);
            }
            return transform.Find(name).GetComponent<T>();
        }
        /// <summary>
        /// 将时间转换为 HH:MM:SS
        /// </summary>
        /// <param name="totalSeconds"></param>
        /// <param name="type">1时分秒2日月天</param>
        /// <returns></returns>
        public string ConvertSecondsToHHMMSS(int totalSeconds, int type = 1)
        {
            string dec = "";
            int hours = totalSeconds / 3600;
            int minutes = (totalSeconds % 3600) / 60;
            int seconds = totalSeconds % 60;
            int days = hours / 24;
            //int months = days / 30;
            
            if (type == 1)
                dec = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
            else
            {
                dec = $"{days:D2}天";
                if (days <= 0)
                {
                    dec = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
                }
            }
            return dec;
        }
        /// <summary>
        /// 清空区域内的对象
        /// </summary>
        /// <param name="pos_btn"></param>
        public virtual void ClearObject(Transform pos_btn,int count = 0)
        {
            for (int i = pos_btn.childCount - 1; i >= count; i--)//清空区域内按钮
            {
                Destroy(pos_btn.GetChild(i).gameObject);
            }
        }


        /// <summary>
        /// 物资需求列表
        /// </summary>
        protected Dictionary<string, long> dic = new Dictionary<string, long>();

        /// <summary>
        /// 判断是否满足条件
        /// </summary>
        /// <returns></returns>
        protected bool Return_Condition()
        {
#if UNITY_EDITOR
            //return true;
#elif UNITY_ANDROID
#elif UNITY_IPHONE
#endif
            bool exist = false;
            Dictionary<string, long> keys = dic;
            Dictionary<string, int> bagdic = new Dictionary<string, int>();
            long count = 0;
            List<long> currency_unit_list = new List<long>(new long[Enum.GetNames(typeof(currency_unit)).Length]);
            List<long> listunit = SumSave.crt_user_unit.Set();
            foreach (string item in keys.Keys)
            {
                bool isneed = true;//下一个
                if (isneed)
                {
                    for (int i = 0; i < Enum.GetNames(typeof(currency_unit)).Length; i++)
                    {
                        if (item == ((currency_unit)i).ToString())
                        {
                            if (listunit[i] >= Mathf.Abs(keys[item]))
                            {
                                currency_unit_list[i] += (long)Mathf.Abs(keys[item]);
                                count++;
                                isneed = false;
                                // continue;
                                break;
                            }
                        }
                    }
                    if (isneed)
                    {
                        List<(string, int)> list = SumSave.crt_bags.Set();
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (item == list[i].Item1)
                            {
                                if (list[i].Item2 >= Mathf.Abs(keys[item]))
                                {
                                    if (!bagdic.ContainsKey(item))
                                    {
                                        bagdic.Add(item, (int)-Mathf.Abs(keys[item]));
                                        count++;
                                    }
                                    isneed = false;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (count == keys.Count)
            {
                for (int i = 0; i < currency_unit_list.Count; i++)
                {
                    if (currency_unit_list[i] > 0)
                    {
                        SumSave.crt_user_unit.verify_data((currency_unit)i, -currency_unit_list[i]);
                    }
                }
                if (bagdic.Count > 0)
                {
                    SumSave.crt_bags.Get(bagdic, 1);
                }
            }
            exist = count == keys.Count;
            dic.Clear();
            return exist;
        }
        /// <summary>
        /// 添加条件
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="value"></param>
        protected void Need_Condition(object keys, long value)
        {
            //Debug.Log(keys + "" + value);
            if (!dic.ContainsKey(keys.ToString())) dic.Add(keys.ToString(), value);
            else dic[keys.ToString()] += value;
        }


        #region old


        /// <summary>
        /// 消耗物品
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        protected bool RefreshConsumables_old()
        {
#if UNITY_EDITOR
            //return true;
#elif UNITY_ANDROID
#elif UNITY_IPHONE
#endif
            bool exist = false;
            Dictionary<string, long> keys = dic;
            Dictionary<string, int> bagdic = new Dictionary<string, int>();
            long count = 0;
            //0灵珠 1历练 2元宝 3灵气4离线积分5试炼积分
            List<long> currency_unit_list = new List<long> { 0, 0, 0, 0, 0, 0 };
            List<long> listunit = SumSave.crt_user_unit.Set();

            foreach (string item in keys.Keys)
            {
                bool isneed = true;//下一个
                if (isneed)
                {
                    for (int i = 0; i < Enum.GetNames(typeof(currency_unit)).Length; i++)
                    {
                        if (item == ((currency_unit)i).ToString())
                        {
                            if (listunit[i] >= Mathf.Abs(keys[item]))
                            {
                                currency_unit_list[i] += (long)Mathf.Abs(keys[item]);
                                count++;
                                isneed = false;
                                // continue;
                                break;
                            }
                        }
                    }
                    if (isneed)
                    {
                        List<(string, int)> list = SumSave.crt_bag_resources.Set();
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (item == list[i].Item1)
                            {
                                if (list[i].Item2 >= Mathf.Abs(keys[item]))
                                {
                                    if (!bagdic.ContainsKey(item))
                                    {
                                        bagdic.Add(item, (int)-Mathf.Abs(keys[item]));
                                        count++;
                                    }
                                    isneed = false;
                                    break;
                                    //continue;
                                }
                            }
                        }
                    }
                }
            }
            if (count == keys.Count)
            {
                for (int i = 0; i < currency_unit_list.Count; i++)
                {
                    if (currency_unit_list[i] > 0)
                    {
                        switch ((currency_unit)i)
                        {
                            case currency_unit.金币:
                            case currency_unit.元宝:
                            case currency_unit.Boss积分:
                            case currency_unit.离线积分:
                            case currency_unit.试炼积分:
                            case currency_unit.灵气:
                                SumSave.crt_user_unit.verify_data((currency_unit)i, -currency_unit_list[i]);
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (bagdic.Count > 0)
                {
                    SumSave.crt_bag_resources.Get(bagdic, 1);
                    Game_Omphalos.i.Wirte_ResourcesList(Emun_Resources_List.material_value, SumSave.crt_bag_resources.GetData());
                }
            }
            exist = count == keys.Count;
            dic.Clear();
            return exist;

        }
        /// <summary>
        /// 随机获得天气并写入
        /// </summary>
        protected void AddWeather()
        {
            string name = "";
            int weight = 0;
            bool isAdd = true;
            for (int i = 0; i < SumSave.db_weather_list.Count; i++)
            {
                weight += SumSave.db_weather_list[i].probability;
            }

            while (isAdd)
            {
                int rand = UnityEngine.Random.Range(0, SumSave.db_weather_list.Count - 1);

                if (SumSave.db_weather_list[rand].probability >= UnityEngine.Random.Range(0, weight))
                {
                    name = SumSave.db_weather_list[rand].weather_type;
                    isAdd = false;
                    break;
                }

            }

            SumSave.crt_player_buff.player_Buffs.Add(name, (SumSave.nowtime, 60 * 6, 1, 4));
            Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.user_player_buff, SumSave.crt_player_buff.Set_Uptade_String(), SumSave.crt_player_buff.Get_Update_Character());
        }


        /// <summary>
        /// 获取需求
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="value"></param>
        //[System.Obsolete("消耗材料使用->称号中心.Instance.RedConsumables()")]
        protected void NeedConsumablesold(object keys, long value)
        {
            //Debug.Log(keys.ToString() + "  " + value);
            if (!dic.ContainsKey(keys.ToString())) dic.Add(keys.ToString(), value);
            else dic[keys.ToString()] += value;
        }


        #endregion
    }
}
