using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// 商店购买丹药
/// </summary>
enum store_Item
{
    下品历练丹,
    中品历练丹,
    上品历练丹,
    下品经验丹,
    中品经验丹,
    上品经验丹
}



public class panel_store : Base_Mono
{
    /// <summary>
    /// 商店类型父物体
    /// </summary>
    private Transform store_type; 
    /// <summary>
    /// 商店类型名字
    /// </summary>
    private string[] type_name = { "道具", "限购", "离线","荣耀商店","试炼商店" };
    /// <summary>
    /// 商店类型按钮
    /// </summary>
    private btn_item btn_item;
    /// <summary>
    /// 商店内物品父物体
    /// </summary>
    private Transform store_item;
    /// <summary>
    /// 商店内物品预制体
    /// </summary>
    private material_item material_item;
    /// <summary>
    /// 商店预制体
    /// </summary>
    private store_item store_item_Prefabs;
    /// <summary>
    /// 商店物品购买界面
    /// </summary>
    private Transform store_item_info;
    /// <summary>
    /// 商品输入的数量
    /// </summary>
    private TMP_InputField inputField;
    /// <summary>
    /// 购买界面标题
    /// </summary>
    private Text buy_item_Title;
    /// <summary>
    /// 购买按钮
    /// </summary>
    private Button buy_btn;
    /// <summary>
    /// 一次性购买商品的最大长度
    /// </summary>
    private int maxLength=3;
    /// <summary>
    /// 购买商品的最大数量
    /// </summary>
    private int max_num = 99;
    /// <summary>
    /// 玩家需要购买的数量
    /// </summary>
    private int buy_num = 1;
    /// <summary>
    /// 当前选择的商店物品
    /// </summary>
    private db_store_vo buy_item;
    /// <summary>
    /// 关闭按钮
    /// </summary>
    private Button btn;
    /// <summary>
    /// 显示最大购买数量
    /// </summary>
    private Text buy_text;

    /// <summary>
    /// 离线积分显示
    /// </summary>
    private Text OfflinePointsText;

    /// <summary>
    /// 货币0 灵珠，1 历练，2 魔丸，3离线积分，4试炼积分
    /// </summary>
    private List<long> list = new List<long>();

    private void Awake()
    {
        store_type = Find<Transform>("store_type");
        btn_item = Battle_Tool.Find_Prefabs<btn_item>("btn_item"); //Resources.Load<btn_item>("Prefabs/base_tool/btn_item");
        store_item = Find<Transform>("store_item/Viewport/Content");
        material_item = Battle_Tool.Find_Prefabs<material_item>("material_item"); //Resources.Load<material_item>("Prefabs/panel_bag/material_item");
        store_item_Prefabs= Battle_Tool.Find_Prefabs<store_item>("store_item"); //Resources.Load<store_item>("Prefabs/panel_hall/panel_store/store_item");
        btn = Find<Button>("but");
        btn.onClick.AddListener(() =>{ CloseBuyInterface(); });
        OfflinePointsText = Find<Text>("OfflinePoints_Text/Text");

        #region 商店物品购买界面
        store_item_info = Find<Transform>("store_item_buy");
        inputField = Find<TMP_InputField>("store_item_buy/inputField");
        buy_item_Title = Find<Text>("store_item_buy/buy_item_Title/Title");
        buy_btn = Find<Button>("store_item_buy/buy_btn");
        buy_text = Find<Text>("store_item_buy/buy_text");

        inputField.onEndEdit.AddListener(OnInputChanged);//监听输入框
        buy_btn.onClick.AddListener(BuyItem);//监听购买按钮
        store_item_info.gameObject.SetActive(false);
        #endregion

        ClearObject(store_type);
        for (int i = 0; i < type_name.Length; i++)
        {
            btn_item btn = Instantiate(btn_item, store_type);
            btn.Show(i, type_name[i]);
            btn.GetComponent<Button>().onClick.AddListener(() => { ShowItem(btn.index); });
        }
        ShowItem(0);
       
    }

    private void Update()
    {
        list = SumSave.crt_user_unit.Set();
        OfflinePointsText.text= currency_unit.离线积分+":"+ Battle_Tool.FormatNumberToChineseUnit(list[3])+
            "\n"+currency_unit.试炼积分+":" + Battle_Tool.FormatNumberToChineseUnit(list[4]);
    }



    /// <summary>
    /// 点击关闭购买界面
    /// </summary>
    private void CloseBuyInterface()
    {
        store_item_info.gameObject.SetActive(false);
        btn.gameObject.SetActive(false);
    }

  

    /// <summary>
    /// 点击购买
    /// </summary>
    private void BuyItem()
    {
        if (buy_item.ItemMaxQuantity > 0)//限购物品
        {
            //减少限购物品可购买的数量
            if (SumSave.crt_needlist.store_value_dic.ContainsKey(buy_item.ItemName))//判断字典中是否含有该物品
            {
                int nums = exceedNum(SumSave.crt_needlist.store_value_dic[buy_item.ItemName]);
                if (nums > 0)//查找限购物品
                {
                    long value = (((long)buy_num) * (long)(buy_item.ItemPrice));
                    NeedConsumablesold(buy_item.unit, value);
                    if (RefreshConsumables_old())
                    {
                        //Debug.Log("数量："+ buy_num + "消耗金额：" + buy_num * buy_item.ItemPrice);
                        int num = SumSave.crt_needlist.store_value_dic[buy_item.ItemName] + buy_num;
                        SumSave.crt_needlist.store_value_dic[buy_item.ItemName] = num;
                        QuotaComplete();
                        return;
                    }

                }
            }
            else
            {
                exceedNum(0);
                long value = (((long)buy_num) * (long)(buy_item.ItemPrice));
                NeedConsumablesold(buy_item.unit, value);
                if (RefreshConsumables_old())
                {
                    SumSave.crt_needlist.store_value_dic.Add(buy_item.ItemName, buy_num);
                    QuotaComplete();
                    return;
                }

            }
            Alert_Dec.Show("限购商品 " + buy_item.ItemName + " 无购买次数 ");
            return;
        }
        else
        {
            long value = (((long)buy_num) * (long)(buy_item.ItemPrice));
            NeedConsumablesold(buy_item.unit, value);
            if (RefreshConsumables_old())
            {
                SpecialItems();
            }else Alert_Dec.Show("购买失败");
        }
    }
   /// <summary>
   /// 限购商品购买成功
   /// </summary>
    private void QuotaComplete()
    {
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_needlist, SumSave.crt_needlist.Set_Uptade_String(), SumSave.crt_needlist.Get_Update_Character());
        SpecialItems();
        Alert_Dec.Show(buy_item.ItemName + "X" + buy_num + " 购买成功(限购物品) ");
        ShowItemInfo(buy_item);
    }

    /// <summary>
    /// 判断剩余数量是否足够 并更新显示 (可获取剩余数量)
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private int exceedNum(int num)
    {
        int nums = buy_item.ItemMaxQuantity -num;//判断限购商品是否购买完
        if (buy_num > nums)
        {
            buy_num = nums;
            inputField.text = buy_num.ToString();
        }
        return nums;
    }

    /// <summary>
    /// 商城活动物品
    /// </summary>
    private void SpecialItems()
    {
        switch (buy_item.ItemName)
        {
            case "1亿灵珠":
                long value = (long)100000000 * buy_num;
                SumSave.crt_user_unit.verify_data(currency_unit.金币, value);//获得灵珠
                Alert_Dec.Show(buy_item.ItemName + "X" + buy_num + " 购买成功 ");
                break;
            case "2000历练值":
                SumSave.crt_user_unit.verify_data(currency_unit.元宝, 2000 * buy_num);
                break;
            case "下品历练丹":
                //添加1.5倍的历练值
                if (SumSave.crt_player_buff.player_Buffs.ContainsKey("中品历练丹") || SumSave.crt_player_buff.player_Buffs.ContainsKey("上品历练丹"))
                {
                    Alert_Dec.Show("更高级丹药生效中");
                    return;
                }
                AddBuff(buy_item,1.5f,2);
                break;
            case "中品历练丹":
                //添加2倍的历练值
                if (SumSave.crt_player_buff.player_Buffs.ContainsKey("上品历练丹"))
                {
                    Alert_Dec.Show("更高级丹药生效中");
                    return;
                }
                if (SumSave.crt_player_buff.player_Buffs.ContainsKey("下品历练丹"))
                {
                    Alert_Dec.Show("下品历练丹失效");
                    SumSave.crt_player_buff.player_Buffs.Remove("下品历练丹");
                }

                AddBuff(buy_item,2f,2);
                break;
            case "上品历练丹":
                if (SumSave.crt_player_buff.player_Buffs.ContainsKey("下品历练丹"))
                {
                    Alert_Dec.Show("下品历练丹失效");
                    SumSave.crt_player_buff.player_Buffs.Remove("下品历练丹");
                }
                if (SumSave.crt_player_buff.player_Buffs.ContainsKey("中品历练丹"))
                {
                    Alert_Dec.Show("中品历练丹失效");
                    SumSave.crt_player_buff.player_Buffs.Remove("中品历练丹");
                }
                
                AddBuff(buy_item,3f,2);
                break;
            case "下品经验丹":
                //添加1.5倍的经验值
                if (SumSave.crt_player_buff.player_Buffs.ContainsKey("中品经验丹") || SumSave.crt_player_buff.player_Buffs.ContainsKey("上品经验丹"))
                {
                    Alert_Dec.Show("更高级丹药生效中");
                    return;
                }
                AddBuff(buy_item,1.5f,1);
                break;
            case "中品经验丹":
                //添加2倍的经验值
                if (SumSave.crt_player_buff.player_Buffs.ContainsKey("上品经验丹"))
                {
                    Alert_Dec.Show("更高级丹药生效中");
                    return;
                }
                if (SumSave.crt_player_buff.player_Buffs.ContainsKey("下品经验丹"))
                {
                    Alert_Dec.Show("下品经验丹失效");
                    SumSave.crt_player_buff.player_Buffs.Remove("下品经验丹");
                }
                AddBuff(buy_item,2f,1);
                break;
            case "上品经验丹":

                if (SumSave.crt_player_buff.player_Buffs.ContainsKey("下品经验丹"))
                {
                    Alert_Dec.Show("下品经验丹失效");
                    SumSave.crt_player_buff.player_Buffs.Remove("下品经验丹");
                }
                if (SumSave.crt_player_buff.player_Buffs.ContainsKey("中品经验丹"))
                {
                    Alert_Dec.Show("中品经验丹失效");
                    SumSave.crt_player_buff.player_Buffs.Remove("中品经验丹");
                }

                AddBuff(buy_item,3f,1);
                break;
                default:
                int random = Random.Range(1, 100);
                int number = buy_num;
                int maxnumber = number + Random.Range(1, 100);
                Battle_Tool.Obtain_Resources(Obtain_Int.Add(1, buy_item.ItemName, new int[] { number + random, random }), maxnumber);
                //Battle_Tool.Obtain_Resources(buy_item.ItemName, buy_num);//获取奖励
               
                break;
        }
        Alert_Dec.Show("购买" + buy_item.ItemName + " * " + buy_num + " 成功");
        ShopPurchaseTask(buy_item);
    }
    /// <summary>
    /// 商店购买物品任务
    /// </summary>
    private void ShopPurchaseTask(db_store_vo buy_item)
    {
       if( buy_item.ItemName == "血牙狼窟通行证")
        {
            tool_Categoryt.Base_Task(1050);
        }
       //if(buy_item.ItemName== "下品经验丹")
       if(buy_item.ItemName.Contains("经验丹"))
        {
            tool_Categoryt.Base_Task(1051);
        }

    }


    /// <summary>
    /// 添加BUff
    /// </summary>
    private void AddBuff(db_store_vo _buy_item,float effect,int icon)
    {
        if (SumSave.crt_player_buff.player_Buffs.ContainsKey(_buy_item.ItemName))
        {
            SumSave.crt_player_buff.player_Buffs[_buy_item.ItemName] =
                (SumSave.crt_player_buff.player_Buffs[_buy_item.ItemName].Item1,
                SumSave.crt_player_buff.player_Buffs[_buy_item.ItemName].Item2+(60 * buy_num)
                , effect, icon);//当有时，增加buff时间
        }
        else
        {
            SumSave.crt_player_buff.player_Buffs.Add(_buy_item.ItemName, (SumSave.nowtime, 60 * buy_num, effect, icon));
        }

        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.user_player_buff, SumSave.crt_player_buff.Set_Uptade_String(), SumSave.crt_player_buff.Get_Update_Character());//角色丹药Buff更新数据库
        Tool_State.activation_State(State_List.经验丹);
        Tool_State.activation_State(State_List.历练丹);
        SendNotification(NotiList.Refresh_Max_Hero_Attribute);
    }



    /// <summary>
    /// 显示商店内容
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private void ShowItem(int index)
    {
        ClearObject(store_item);
        (int, int, string) vip = SumSave.crt_accumulatedrewards.SetRecharge();
        if (index == 3)
        {
            if (vip.Item1 <= 3)
            {
                Alert_Dec.Show("当前荣耀等级暂无商品");
                return;

            }
        }
        for (int i = 0; i < SumSave.db_stores_list.Count; i++)//判断是否为当前商店类型
        {
            if (SumSave.db_stores_list[i].store_Type == (index + 1))
            {
                store_item item = Instantiate(store_item_Prefabs, store_item);
                db_store_vo items = SumSave.db_stores_list[i];
                item.Init(items);
                item.GetComponent<Button>().onClick.AddListener(() => { ShowItemInfo(items); });
                
            }
        }
    }
    /// <summary>
    /// 显示物品信息
    /// </summary>
    /// <param name="db_store_vo"></param>
    private void ShowItemInfo(db_store_vo item)
    {
        btn.gameObject.SetActive(true);
        store_item_info.gameObject.SetActive(true);
        buy_item_Title.text = item.ItemName;
        buy_item= item;
        buy_text.text= "";

        switch (item.ItemName)
        {
            case "下品历练丹":
                buy_text.text += Show_Color.Red("添加1.5倍历练Buff (1小时)\n");
                break;
            case "中品历练丹":
                buy_text.text += Show_Color.Red("添加2倍历练Buff (1小时)\n");
                break;
            case "上品历练丹":
                buy_text.text += Show_Color.Red("添加3倍历练Buff (1小时)\n");
                break;
            case "下品经验丹":
                buy_text.text += Show_Color.Red("添加1.5倍经验Buff (1小时)\n");
                break;
            case "中品经验丹":
                buy_text.text += Show_Color.Red("添加2倍经验Buff (1小时)\n");
                break;
            case "上品经验丹":
                buy_text.text += Show_Color.Red("添加3倍经验Buff (1小时)\n");
                break;
        }









        if (buy_item.ItemMaxQuantity > 0)
        {
            int nums = 0;
            if (SumSave.crt_needlist.store_value_dic.ContainsKey(buy_item.ItemName))//判断字典中是否含有该物品
            {
                nums = buy_item.ItemMaxQuantity - SumSave.crt_needlist.store_value_dic[buy_item.ItemName];
            }
            else
            {
                nums= buy_item.ItemMaxQuantity;
            }
            
            buy_text.text += "购买数量:\n" + nums + "/"+ buy_item.ItemMaxQuantity;
        }else
        {
            buy_text.text += " ";
        }

    }

    /// <summary>
    /// 购买物品输入数量
    /// </summary>
    /// <param name="arg0"></param>
    private void OnInputChanged(string newText)
    {

        char[] chars = newText.ToCharArray();
        string filteredText= "";
        // 仅保留数字
        foreach (char c in chars)
        {
            if (char.IsDigit(c)) 
            {
                filteredText+= c;
            }
        }
        // 截断超长部分
        if (filteredText.Length > maxLength)
        {
            filteredText = filteredText.Substring(0, maxLength);
        }


        if (buy_item.ItemMaxQuantity > 0)//限购物品
        {
            
            if(SumSave.crt_needlist.store_value_dic.ContainsKey(buy_item.ItemName))//判断字典中是否含有该物品
            {
                int num = SumSave.crt_needlist.store_value_dic[buy_item.ItemName];//玩家以购买的数量
                if (int.Parse(filteredText) > (buy_item.ItemMaxQuantity - num))//判断输入的值是否大于最大购买数量
                {
                    filteredText = (buy_item.ItemMaxQuantity - num).ToString();
                    Alert_Dec.Show("超过最大购买数量");
                }
            }else
            {
                if (int.Parse(filteredText) > buy_item.ItemMaxQuantity)//判断输入的值是否大于最大购买数量
                {
                    filteredText = buy_item.ItemMaxQuantity.ToString();
                    Alert_Dec.Show("超过最大购买数量");
                }
            }
            
        }
        

        // 同步输入框内容
        if (filteredText != inputField.text)
        {
            inputField.text = filteredText;
            SetCursorToEnd();
        }
        //获取购买数量
        if (int.TryParse(inputField.text, out int value))
        {
            buy_num = value;   
        }else
        {
            buy_num = 1;
        }
    }
    // 保持光标在末尾（避免跳动）
    private void SetCursorToEnd()
    {
        inputField.selectionAnchorPosition = inputField.text.Length;
        inputField.selectionFocusPosition = inputField.text.Length;
    }
}
