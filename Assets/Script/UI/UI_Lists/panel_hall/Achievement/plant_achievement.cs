using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class plant_achievement : Base_Mono
{
    /// <summary>
    /// 显示具体值
    /// </summary>
    private Image show_offect;
    /// <summary>
    /// 显示成就名称
    /// </summary>
    private Text show_name, show_info;
    /// <summary>
    /// 领取奖励按钮
    /// </summary>
    private Button btn_receive;
   

    /// <summary>
    /// 成就位置
    /// </summary>
    public Transform crt;
    /// <summary>
    /// 具体成就
    /// </summary>
    public ach_item Achieve_Item_Prefab;
    /// <summary>
    /// 当前成就
    /// </summary>
    private ach_item crt_achieve_Item;
    /// <summary>
    /// 成就类型预制体
    /// </summary>
    public btn_item Achieve_Type_Prefab;
    /// <summary>
    /// 当前选择成就类型
    /// </summary>
    private btn_item crt_type;
    /// <summary>
    /// 存储字典
    /// </summary>
    private new Dictionary<btn_item, List<ach_item>> dic = new Dictionary<btn_item, List<ach_item>>();
    /// <summary>
    /// 自身经验值
    /// </summary>
    private Dictionary<string, long> dic_exp = new Dictionary<string, long>();
    /// <summary>
    /// 自身等级
    /// </summary>
    private Dictionary<string, long> dic_lv = new Dictionary<string, long>();
    
    /// <summary>
    /// 关闭按钮
    /// </summary>
    private Button close;



    private void Awake()
    {
        Achieve_Item_Prefab = Battle_Tool.Find_Prefabs<ach_item>("ach_item"); //Resources.Load<ach_item>("Prefabs/base_tool/ach_item"); 
        Achieve_Type_Prefab = Battle_Tool.Find_Prefabs<btn_item>("btn_item"); //Resources.Load<btn_item>("Prefabs/base_tool/btn_item");
        crt= Find<Transform>("achievementList/Viewport/Content");
        show_offect= Find<Image>("show_offect");
        show_name= Find<Text>("show_offect/show_name/info");
        show_info=Find<Text>("show_offect/base_info/Viewport/Content/info");
        btn_receive = Find<Button>("show_offect/btn_list/btn_item");
        btn_receive.onClick.AddListener(() => { Receive(); });
        close= Find<Button>("close");
        close.onClick.AddListener(() => { Close(); });
        Insace_Base_Info();
    }


    private void Update()
    {
        CompleteAchievementTasks();
    }

    /// <summary>
    /// 成就信息栏点击其他区域关闭
    /// </summary>
    private void Close()
    {
        show_offect.gameObject.SetActive(false);
        close.gameObject.SetActive(false);
    }

    /// <summary>
    /// 点击按钮领取奖励
    /// </summary>
    private void Receive()
    {

        dic_exp = SumSave.crt_achievement.Set_Exp();
        dic_lv = SumSave.crt_achievement.Set_Lv();
        if(!dic_lv.ContainsKey(crt_achieve_Item.Data.achievement_value))
        {
            //Alert_Dec.Show("未达到领取条件");
            //return;
            dic_lv.Add(crt_achieve_Item.Data.achievement_value, 0);
        }
        int lv = (int)dic_lv[crt_achieve_Item.Data.achievement_value];
        if (true)
        {
            if (lv >= crt_achieve_Item.Data.achievement_needs.Count)
            {
                Alert_Dec.Show("当前阶段已满");
                return;
            }

            
            if (crt_achieve_Item.Data.achievement_needs[lv] > dic_exp[crt_achieve_Item.Data.achievement_value])
            {
                Alert_Dec.Show("未达到领取条件");
                return;
            }
        }

        dic_lv[crt_achieve_Item.Data.achievement_value]++;//成就等级++
        SumSave.crt_achievement.Get_lv(dic_lv);
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_achieve,
                   SumSave.crt_achievement.Set_Uptade_String(), SumSave.crt_achievement.Get_Update_Character());


        string[] temp = crt_achieve_Item.Data.achievement_rewards[lv].Split(' ');
        if(int.Parse(temp[0])>1)//temp[0]为属性加成类型
        {
            string reward = "";

            switch (int.Parse(temp[0]))//成就完成奖励
            { 
                case 1://获得属性
                    break;
                case 2://获得材料
                    reward = temp[1];
                    break;
                case 3://获得灵珠
                    reward = currency_unit.金币.ToString();
                    break;
                case 4://获得魔丸
                    reward = currency_unit.Boss积分.ToString();
                    break;
            
            }
            int random = Random.Range(1, 100);
            int number = int.Parse(temp[2]);
            int maxnumber = number + Random.Range(1, 100);
            Battle_Tool.Obtain_Resources(Obtain_Int.Add(1, reward, new int[] { number + random, random }), maxnumber);
            //Battle_Tool.Obtain_Resources(reward, int.Parse(temp[2]));//获得奖励
            Alert_Dec.Show("领取 " + reward+"X"+ temp[2]+" 成功");
          
        }
        else if (int.Parse(temp[0]) == 1)
        {
            Alert_Dec.Show("获得 "+(enum_skill_attribute_list)(int.Parse(temp[1]))+"+"+temp[2]); 
        }
        CompleteTheAchievementCollectionTask();
        SendNotification(NotiList.Refresh_Max_Hero_Attribute);
        show_offect.gameObject.SetActive(false);
        crt_achieve_Item.Init(); 
    }
    /// <summary>
    /// 完成领取成就任务
    /// </summary>
    private void CompleteTheAchievementCollectionTask()
    {
        tool_Categoryt.Base_Task(1025);
    }




    /// <summary>
    /// 初始化成就列表
    /// </summary>
    private void Insace_Base_Info()
    {
        for (int i = 0; i < Enum.GetNames(typeof(Achieve_Type)).Length; i++)//更具枚举创建成就类型
        {
            btn_item temp = Instantiate(Achieve_Type_Prefab, crt);//实例化
            temp.Show(i,(Achieve_Type)(i + 1));//显示标题信息
            for (int j = 0; j < SumSave.db_Achievement_dic.Count; j++)//成就数据库
            {
                if (SumSave.db_Achievement_dic[j].achievement_type == i + 1)//判断是否在当前类型
                {
                    if(SumSave.db_Achievement_dic[j].achievement_value==( Achieve_collect.等级升级).ToString())
                    {
                        SumSave.crt_achievement.up_date_Exp((Achieve_collect.等级升级).ToString(), SumSave.old_crt_hero.hero_Lv);//更新等级
                    }
                    else if(SumSave.db_Achievement_dic[j].achievement_value == (Achieve_collect.技能数量).ToString())
                    {

                        SumSave.crt_achievement.up_date_Exp((Achieve_collect.技能数量).ToString(), SumSave.oldcrt_skills.Count);//更新技能数量
                    }

                    ach_item item = Instantiate(Achieve_Item_Prefab, crt);//实例化具体成就
                    item.Data = SumSave.db_Achievement_dic[j];//获取成就信息
                    //读取数据
                    if (!dic.ContainsKey(temp)) dic.Add(temp, new List<ach_item>());//判断字典里是否已经存在
                    dic[temp].Add(item);
                    item.GetComponent<Button>().onClick.AddListener(() => { Select_Achieve(item); });//具体成就点击事件
                }
            }
            temp.GetComponent<Button>().onClick.AddListener(() => { Select_Type(temp); });//成就类型点击事件
        }
        foreach (btn_item item in dic.Keys)
        {
            foreach (ach_item crt_item in dic[item])//关闭所以具体成就
            {
                crt_item.gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// 完成成就任务
    /// </summary>
    private void CompleteAchievementTasks()
    {
        Dictionary<string, long> user_achievements = SumSave.crt_achievement.Set_Exp();
        if (!user_achievements.ContainsKey(Achieve_collect.击杀怪物.ToString()))
        {
            user_achievements.Add(Achieve_collect.击杀怪物.ToString(), 0);
        }
        if (user_achievements[(Achieve_collect.击杀怪物).ToString()] >= 1000)
        {
            tool_Categoryt.Base_Task(1060);
        }
    }



    /// <summary>
    /// 读取成就
    /// </summary>
    /// <param name="item"></param>
    private void Select_Achieve(ach_item item)
    {
        show_offect.gameObject.SetActive(true);
        close.gameObject.SetActive(true);
        crt_achieve_Item = item;
        show_name.text = item.Data.achievement_value;
        Show_Dec();
    }


    
    /// <summary>
    /// 显示成就描述
    /// </summary>
    private void Show_Dec()
    {
        string dec = (Achieve_Type)crt_achieve_Item.Data.achievement_type + " " + crt_achieve_Item.Data.achievement_value;
        dic_exp = SumSave.crt_achievement.Set_Exp();
        dic_lv = SumSave.crt_achievement.Set_Lv();
        if (dic_lv.ContainsKey(crt_achieve_Item.Data.achievement_value))
        {
            int max = (int)MathF.Min(dic_lv[crt_achieve_Item.Data.achievement_value], crt_achieve_Item.Data.achievement_needs.Count);
            
            for (int i = 0; i < max; i++)
            {
                string[] temp = crt_achieve_Item.Data.achievement_rewards[i].Split(' ');
                dec += "\n" + Show_Color.Green("(已领取)" + "获得 " + (enum_skill_attribute_list)(int.Parse(temp[1])) + "+" + temp[2]);
            }
            if (dic_lv[crt_achieve_Item.Data.achievement_value] < crt_achieve_Item.Data.achievement_needs.Count)
            {
                int number = (int)dic_lv[crt_achieve_Item.Data.achievement_value];
                dec += "\nLv" + (number + 1) + ".成就阶段 " + dic_exp[crt_achieve_Item.Data.achievement_value] + "/" + crt_achieve_Item.Data.achievement_needs[number];
                //dec += "\n奖励 " + Show_Color.Yellow(InSetInfo(number));
            }
        }
        else
        {
            
            dec += "\nLv1.成就阶段 0/" + crt_achieve_Item.Data.achievement_needs[0];
        }
        show_info.text = dec;
    }
    /// <summary>
    /// 选择成就类型
    /// </summary>
    /// <param name="temp"></param>
    private void Select_Type(btn_item temp)
    {
        if (crt_type != null)
        {
            if (crt_type == temp)//判断是否为当前选择类型
            {
                if (!temp.Active())//判断是否激活
                {
                    crt_type.Selected = true;
                    foreach (ach_item item in dic[crt_type])
                    {
                        item.gameObject.SetActive(crt_type.Active());//判断是否需要激活
                        item.Init();//初始化
                    }

                }
                else
                {
                    crt_type.Selected = false;
                    foreach (ach_item item in dic[crt_type])
                    {
                        item.gameObject.SetActive(crt_type.Active());
                    }
                }
                return;
            }

            crt_type.Selected = false;
            foreach (ach_item item in dic[crt_type])//不是当前选择类型关闭
            {
                item.gameObject.SetActive(crt_type.Active());
            }
        }
        crt_type = temp;//更新当前选择类型
        crt_type.Selected = true;//选中状态
        foreach (ach_item item in dic[crt_type])//打开并且初始化item
        {
            item.gameObject.SetActive(crt_type.Active());
            item.Init();
        }
    }

}
