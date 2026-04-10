using Common;
using Components;
using MVC;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class artifact_offect : Base_Mono
{
    private Button close;
    /// <summary>
    /// 确认升级
    /// </summary>
    private Button confirm;
    /// <summary>
    /// 显示信息
    /// </summary>
    private Text artifact_name, artifact_info,info_state;
    /// <summary>
    /// 确认升级
    /// </summary>
    private Text confirm_info;
    /// <summary>
    /// 当前显示的物品
    /// </summary>
    private artifact_item crt_artifact;
    private void Awake()
    {
        close=Find<Button>("close_button");
        close.onClick.AddListener(()=> { gameObject.SetActive(false); });
        confirm = Find<Button>("confirm");
        confirm.onClick.AddListener(()=> { confirm_artifact(); });
        info_state = Find<Text>("confirm/info");
        artifact_name = Find<Text>("show_name/info");
        artifact_info = Find<Text>("Scroll View/Viewport/info");
        confirm_info = Find<Text>("confirm/info");
    }

    /// <summary>
    /// 选中功能
    /// </summary>
    private void confirm_artifact()
    {
        
        AudioManager.Instance.playAudio(ClipEnum.使用物品);
        if (crt_artifact != null)
        {
            (string, int) result = ArrayHelper.Find(SumSave.crt_artifact.Set(), e => e.Item1 == crt_artifact.Data.arrifact_name);
            if (result.Item2 == 0)//未激活
            {
                for (int i = 0; i < crt_artifact.Data.Artifact_open_needs.Length; i++)
                {
                    string[] temp = crt_artifact.Data.Artifact_open_needs[i].Split(' ');
                    NeedConsumablesold(temp[0], int.Parse(temp[1]));
                }
                if (RefreshConsumables_old())
                {
                    result.Item1 = crt_artifact.Data.arrifact_name;
                    result.Item2 = 1;
                    SumSave.crt_artifact.Get(result);
                    set_artifact(crt_artifact);
                    if (result.Item1 == "天机扇")
                    {
                        //开启小世界
                        Open_smallWorld();
                    }
                    Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_artifact, SumSave.crt_artifact.Set_Uptade_String(), SumSave.crt_artifact.Get_Update_Character());
                    SendNotification(NotiList.Refresh_Max_Hero_Attribute);
                    crt_artifact.Set(1);
                    set_artifact(crt_artifact);
                    Alert_Dec.Show("激活成功");
                }
                else Alert_Dec.Show("激活失败");
                
            }
            else
            {
                if (result.Item2 < crt_artifact.Data.Artifact_MaxLv)
                {

                    for (int i = 0; i < crt_artifact.Data.arrifact_needs.Length; i++)
                    {
                        string[] temp = crt_artifact.Data.arrifact_needs[i].Split(' ');
                        NeedConsumablesold(temp[0], int.Parse(temp[1]));
                    }
                    if (RefreshConsumables_old())
                    {
                        result.Item2 += 1;
                        SumSave.crt_artifact.Get(result);
                        crt_artifact.Set(result.Item2);
                        set_artifact(crt_artifact);
                        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_artifact, SumSave.crt_artifact.Set_Uptade_String(), SumSave.crt_artifact.Get_Update_Character());
                        SendNotification(NotiList.Refresh_Max_Hero_Attribute);
                        Alert_Dec.Show("升级成功");
                    }else Alert_Dec.Show("升级失败");
                }
                else Alert_Dec.Show("已满级");
            }
        }
    }
   

    /// <summary>
    /// 开启小世界
    /// </summary>
    private void Open_smallWorld()
    {
        SumSave.crt_world = new user_world_vo();
        SumSave.crt_world.World_Lv = 1;
        Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.mo_user_world, SumSave.crt_world.Set_Instace_String());
        AddLand();
        Alert_Dec.Show("小世界开启成功");
    }
    /// <summary>
    /// 添加一块土地
    /// </summary>
    private static void AddLand()
    {
        if(SumSave.crt_plant.Set().Count>0)
        {
            return;
        }
        List<(string, DateTime)> Set = SumSave.crt_plant.Set();
        Set.Add(("0", SumSave.nowtime));
        SumSave.crt_plant.Set_data(Set);
    }

    /// <summary>
    /// 设置当前显示的物品
    /// </summary>
    /// <param name="item"></param>
    public void set_artifact(artifact_item item)
    {
        crt_artifact=item;
        info_state.text = item.base_lv == 0? "激活":"升级";
        artifact_name.text = crt_artifact.Data.arrifact_name + (item.base_lv == 0
            ? "(未激活)" : Show_Color.Red("(Lv." + item.base_lv + ")"));
        string dec = Show_Color.Green("等级: ")+Show_Color.Red(item.base_lv)+"/(Max"+item.Data.Artifact_MaxLv+")";
        string[] splits = crt_artifact.Data.arrifact_effects;

        dec += Show_Color.Green("\n属性: \n");
        if (splits.Length >= 1)
        {
            foreach (var base_info in splits)
            {
                string[] infos= base_info.Split(' ');
                //1类型 2每一级加成 3开启等级
                if (infos.Length >= 3)
                {
                    dec += (infos[1] == "0" ? Show_Color.Green("开启加成:") : infos[2] + "级 激活: ") +
                        (item.base_lv >= int.Parse(infos[2]) ? Show_Color.Red((enum_skill_attribute_list)int.Parse(infos[0]) +
                        " + " + (float.Parse(infos[1]) * item.base_lv) + tool_Categoryt.Obtain_unit(int.Parse(infos[0]))):
                        Show_Color.Grey((enum_skill_attribute_list)int.Parse(infos[0]) + " + Max: " + (float.Parse(infos[1]) * item.Data.Artifact_MaxLv) + tool_Categoryt.Obtain_unit(int.Parse(infos[0]))
                        + "(未激活)")) + "\n";
                }
            }
        }
        if (item.base_lv == 0)
        {
            dec += Show_Color.Green("\n激活条件: \n");
            dec += Show_Needs(crt_artifact.Data.Artifact_open_needs);
        }
        else
        { 
            dec += Show_Color.Green("\n升级条件: \n");
            dec += Show_Needs(crt_artifact.Data.arrifact_needs);
        }
        dec +="\n "+ crt_artifact.Data.Artifact_dec;
        artifact_info.text = dec;
    }

    private string Show_Needs(string[] splits)
    {
        string dec = "";
        for (int i = 0; i < splits.Length; i++)
        {
            dec += Show_Color.Red(splits[i]) + "\n";
        }
        return dec;
    }
}
