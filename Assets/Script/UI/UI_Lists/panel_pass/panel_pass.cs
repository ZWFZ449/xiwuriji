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

public class panel_pass : Base_Mono
{
    /// <summary>
    /// 通行证列表
    /// </summary>
    private List<string> btn_list = new List<string>() { "S1" }; //, "S2"


    private Transform pos_btn, pos_item,pos_task;

    private btn_item btn_itm_prefabs;

    private pass_item pass_item_prefabs;

    private task_item task_item_prefabs;
    /// <summary>
    /// 显示任务列表
    /// </summary>
    private Dictionary<int, task_item> dic_task = new Dictionary<int, task_item>();
    /// <summary>
    /// 显示累积任务信息
    /// </summary>
    private Text task_info;
    /// <summary>
    /// 第几个通行证
    /// </summary>
    private int index=0;
    /// <summary>
    /// 累积奖励
    /// </summary>
    private Panel_Accumulatedrewards panel_accumulatedrewards;

    /// <summary>
    /// 显示通行证等级
    /// </summary>
    private Text pass_lvExp_Text;
    protected void Awake()
    {
        Initialize();
    }
    public void Initialize()
    {
        pos_btn = Find<Transform>("bg_main/btn_list");
        pos_item = Find<Transform>("bg_main/Scroll View/Viewport/Content");
        pos_task = Find<Transform>("bg_main/task/task_list/Viewport/Content");
        task_info = Find<Text>("bg_main/task/info/info");
        task_info.GetComponent<Button>().onClick.AddListener(() => { OnTaskInfoClick(); });
        btn_itm_prefabs = Battle_Tool.Find_Prefabs<btn_item>("btn_item"); //Resources.Load<btn_item>("Prefabs/base_tool/btn_item"); 
        pass_item_prefabs = Battle_Tool.Find_Prefabs<pass_item>("pass_item"); //Resources.Load<pass_item>("Prefabs/panel_hall/panel_pass/pass_item"); 
        task_item_prefabs = Battle_Tool.Find_Prefabs<task_item>("task_item"); //Resources.Load<task_item>("Prefabs/panel_hall/panel_pass/task_item");
        panel_accumulatedrewards= UI_Manager.I.GetPanel<Panel_Accumulatedrewards>();
        pass_lvExp_Text= Find<Text>("bg_main/pass_lvExp/Text");
        for (int i = 0; i < btn_list.Count; i++)
        {
            btn_item btn = Instantiate(btn_itm_prefabs, pos_btn);
            btn.Show(i + 1, btn_list[i]);
            btn.GetComponent<Button>().onClick.AddListener(() => { OnBtnClick(btn); });
        }
        List<int> list = SumSave.crt_pass.Get_day_state();
        ClearObject(pos_task);
        //固定6个任务
        for (int i = 0; i < 6; i++)
        {
            task_item item = Instantiate(task_item_prefabs, pos_task);
            item.Show(i, list[i], list[i] == -1);
            item.GetComponent<Button>().onClick.AddListener(() => { OnTaskClick(item); });
            dic_task.Add(i, item);
        }
    }
    /// <summary>
    /// 显示累积奖励
    /// </summary>
    private void OnTaskInfoClick()
    {
        panel_accumulatedrewards.Show();
        panel_accumulatedrewards.Init(1);
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="item"></param>
    private void OnTaskClick(task_item item)
    {
        List<int> list = SumSave.crt_pass.Get_day_state();
        if (list[item.index] == -1)
        {
            Alert_Dec.Show("任务已完成");
            return;
        }
        if (item.State(list[item.index]))
        {
            int num = 1;
            //领取奖励
            int random = Random.Range(1, 100);
            int number = 1;
            int maxnumber = number + Random.Range(1, 100);
            Battle_Tool.Obtain_Resources(Obtain_Int.Add(1, "命运金币", new int[] { number + random, random }), maxnumber);
            //Battle_Tool.Obtain_Resources("命运金币", num);
            Dictionary<int, List<int>> Dir= SumSave.crt_pass.Set();
            SumSave.crt_pass.data_exp++;

            if (Dir.ContainsKey(1))///升级通行证累计次数加2
            {
                SumSave.crt_pass.Max_task_number += 2;

            }else
            {
                SumSave.crt_pass.Max_task_number++;
            }
           

            
            if (SumSave.crt_pass.data_exp >= 10)
            {
                SumSave.crt_pass.data_lv++;
                SumSave.crt_pass.data_exp -= 10;
            }
            SumSave.crt_pass.Get(item.index);

            Alert.Show("领取成功", "命运金币*"+ num);

            Show_Pass_Progress();
            ShowLv();
        }
        else
        {
            Alert_Dec.Show("未满足完成条件");
        }

    }

    /// <summary>
    /// 领取通行证任务完成
    /// </summary>
    private void CollectingPassTasks()
    {
        tool_Categoryt.Base_Task(1057);
        tool_Categoryt.Base_Task(1077);
    }

    private void OnBtnClick(btn_item btn)
    {

    }

    public override void Show()
    {
        base.Show();
        Base_Show();
        Show_Pass_Progress();
        viewPass();
        ShowLv();
    }
    /// <summary>
    /// 显示通行证等级
    /// </summary>
    private void ShowLv()
    {
        pass_lvExp_Text.text="通行证等级："+Show_Color.Yellow( SumSave.crt_pass.data_lv)+"\n 经验："+ Show_Color.Yellow(SumSave.crt_pass.data_exp)+"";
    }

    /// <summary>
    /// 查看通行证任务
    /// </summary>
    private void viewPass()
    {
        tool_Categoryt.Base_Task(1076);
    }

    /// <summary>
    /// 显示任务进度
    /// </summary>
    private void Show_Pass_Progress()
    {
        task_info.text= "累计完成任务：" + SumSave.crt_pass.Max_task_number;
        List<int> list = SumSave.crt_pass.Get_day_state();
        foreach (int item in dic_task.Keys)
        {
            dic_task[item].progress(list[item], list[item]==-1);
        }
    }
    /// <summary>
    /// 显示通行证
    /// </summary>
    private void Base_Show()
    {
        for (int i = pos_item.childCount - 1; i >= 0; i--)
        {
            Destroy(pos_item.GetChild(i).gameObject);
        }
        //自身领取状态
        Dictionary<int, List<int>> dic = SumSave.crt_pass.Set();

        for (int i = 0; i < SumSave.db_pass.Count; i++)
        {
            if (SumSave.db_pass[i].pass_index == index)
            { 
                pass_item item=Instantiate(pass_item_prefabs, pos_item);
                item.Data= SumSave.db_pass[i];
                if(!dic.ContainsKey(index))dic.Add(index,new List<int>(index));
                if (!dic[index].Contains(i))
                {
                    dic[index].Add(0);
                }
                if (dic.ContainsKey(index+1))
                {
                    if (!dic[index+1].Contains(i))
                    {
                        dic[index+1].Add(0);
                    }
                    item.Set(dic[index][i], dic[index + 1][i]);
                }
                else 
                 item.Set(dic[index][i], 1);

            }
        }

    }

    /// <summary>
    /// 升级
    /// </summary>
    /// <param name="item"></param>
    protected void GetReward(pass_item item)
    {
        if (SumSave.crt_pass.data_lv >= item.Data.lv)
        {
            Dictionary<int, List<int>> dic = SumSave.crt_pass.Set();
            if (dic.ContainsKey(index))
            {
                if (dic[index][item.Data.lv] == 0)
                {
                    //领取奖励
                    Obtain_result(item.Data.reward);
                    dic[index][item.Data.lv] = 1;
                    if (dic.ContainsKey(index + 1))
                        item.Set(dic[index][item.Data.lv], dic[index + 1][item.Data.lv]);
                    else item.Set(dic[index][item.Data.lv], 1);
                    SumSave.crt_pass.Get(dic);
                    Alert_Dec.Show("领取成功");
                }
                else Alert_Dec.Show("已经领取");
            }
        }else Alert_Dec.Show("等级不足");
    }

    /// <summary>
    /// 进阶领取
    /// </summary>
    /// <param name="item"></param>
    protected void GetupLvReward(pass_item item)
    {
        if (SumSave.crt_pass.data_lv >= item.Data.lv)
        {
            Dictionary<int, List<int>> dic = SumSave.crt_pass.Set();
            if (dic.ContainsKey(index+1))
            {
                if (dic[index+1][item.Data.lv] == 0)
                {
                    //领取奖励
                    Obtain_result(item.Data.uplv_reward);
                    dic[index+1][item.Data.lv] = 1;
                    item.Set(dic[index][item.Data.lv], dic[index + 1][item.Data.lv]);
                    SumSave.crt_pass.Get(dic);
                    //Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_pass, SumSave.crt_pass.Set_Uptade_String(), SumSave.crt_pass.Get_Update_Character());
                    Alert_Dec.Show("领取成功");
                }
                else Alert_Dec.Show("已经领取");
            }
        }
        else Alert_Dec.Show("等级不足");
    }
    /// <summary>
    /// 获取奖励
    /// </summary>
    /// <param name="result"></param>
    private void Obtain_result(string result)//进阶奖励1、材料2、灵物3、灵珠4、魔丸5、皮肤6、灵气
    {
        Battle_Tool.Obtain_result(result);
        CollectingPassTasks();
    }


}
