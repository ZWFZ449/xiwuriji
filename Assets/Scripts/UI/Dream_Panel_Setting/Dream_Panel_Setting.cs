using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public enum setting_type
{
    none=0,
    基础设置,
    捡取设置,
    Boss设置,
    系统设置,
}
public class Dream_Panel_Setting : Panel_Base
{
    /// <summary>
    /// 按钮父物体
    /// </summary>
    private Transform m_btn_type_borm,m_setting_btn_borm;
    /// <summary>
    /// 按钮预制体
    /// </summary>
    private btn_item btn_item_prefab;
    /// <summary>
    /// 当前设置类型
    /// </summary>
    private setting_type current_setting_type = setting_type.none;
    /// <summary>
    /// 设置项预制体
    /// </summary>
    private dream_setting_item dream_setting_item_prefab;
    /// <summary>
    /// 显示信息
    /// </summary>
    private TMP_Text base_info;
    /// <summary>
    /// 确认按钮
    /// </summary>
    private Button confirm;
    /// <summary>
    /// 设置列表
    /// </summary>
    private List<(int,int,string,int)> setting_list = new List<(int,int, string, int)>();

    public override void Initialize()
    {
        base.Initialize();
        m_btn_type_borm = Find<Transform>("bg/battle_btn_list/Scroll View/Viewport/Content");
        btn_item_prefab = Tool_UI.Find_Prefabs<btn_item>("btn_item");
        base_info= Find<TMP_Text>("bg/offect_list/title/info/info");
        m_setting_btn_borm = Find<Transform>("bg/offect_list/Scroll View/Viewport/Content");
        dream_setting_item_prefab = Tool_UI.Find_Prefabs<dream_setting_item>("dream_setting_item");
        confirm= Find<Button>("bg/offect_list/confirm");
        for (int i = 1; i < Enum.GetNames(typeof(setting_type)).Length; i++)
        {
            btn_item btn_item = Instantiate(btn_item_prefab, m_btn_type_borm);
            btn_item.Show(i, (setting_type)i);
            btn_item.GetComponent<Button>().onClick.AddListener(() => { OnClickBtn(btn_item); });
            if(current_setting_type==setting_type.none) OnClickBtn(btn_item);
        }
        confirm.onClick.AddListener(()=> { Confirm(); });
    }
    /// <summary>
    /// 确认
    /// </summary>
    private void Confirm()
    {
        SumSave.crt_setting.SetData(setting_list);
        setting_list.Clear();
        Game_Omphalos.Refresh(Mysql_Table_Name.dream_user_bag);
        Alert_Dec.Show("设置成功");
    }

    private void OnClickBtn(btn_item btn_item)
    {
        ClearObject(m_setting_btn_borm);
        current_setting_type = (setting_type)btn_item.index;
        base_info.text = current_setting_type + "";
        switch ((setting_type)btn_item.index)
        {
            case setting_type.基础设置:
                for (int i = 0; i < Enum.GetNames(typeof(medicineType)).Length; i++)
                {
                    dream_setting_item dream_setting_item = Instantiate(dream_setting_item_prefab, m_setting_btn_borm);
                    for (int j = 0; j < SumSave.crt_setting.medicine_list.Count; j++)
                    {
                        if (SumSave.crt_setting.medicine_list[j].Item1 == (i + 1))
                        {
                            dream_setting_item.Init((medicineType)(i + 1), SumSave.crt_setting.medicine_list[j].Item2, SumSave.crt_setting.medicine_list[j].Item3);
                        }
                    }
                }
                break;
            case setting_type.捡取设置:
                for (int i = 0; i < Enum.GetNames(typeof(enum_equip_quality_list)).Length; i++)
                {
                    dream_setting_item dream_setting_item = Instantiate(dream_setting_item_prefab, m_setting_btn_borm);
                    bool exist = true;
                    for (int j = 0; j < SumSave.crt_setting.battle_base_list.Count; j++)
                    {
                        if (SumSave.crt_setting.battle_base_list[j].Item1 == (i + 1))
                        {
                            exist=false;
                            dream_setting_item.Init((enum_equip_quality_list)(i + 1), SumSave.crt_setting.battle_base_list[j].Item2);

                        }
                    }
                    if (exist)
                    { 
                        dream_setting_item.Init((enum_equip_quality_list)(i + 1), 0);
                    }
                }
                break;
            case setting_type.Boss设置:

                foreach (var item in SumSave.crt_setting.Boss_list)
                {
                    for (int j = 0; j < SumSave.db_maps.Count; j++)
                    {
                        if (SumSave.db_maps[j].map_type == 0)
                        {
                            for (int i = 0; i < SumSave.db_maps[j].map_boss.Count; i++)
                            {
                                if (SumSave.db_maps[j].map_boss[i] == item.Key)
                                { 
                                    dream_setting_item dream_setting_item = Instantiate(dream_setting_item_prefab, m_setting_btn_borm);
                                    dream_setting_item.Init(item.Key, item.Value.Item1, item.Value.Item2);
                                    break;
                                }
                            }
                        }
                    }
                }
                //for (int i = 0; i < SumSave.crt_setting.battle_Boss_list.Count; i++)
                //{
                //    List<string> analysis = ArrayHelper.Get_Split<string>(SumSave.crt_setting.battle_Boss_list[i].Item1, '+');

                //    for (int j = 0; j < SumSave.db_maps.Count; j++)
                //    {
                //        if (SumSave.db_maps[j].map_type == 0)
                //        {
                //            if (SumSave.db_maps[j].map_boss.Contains(analysis[0]))
                //            {
                //                dream_setting_item dream_setting_item = Instantiate(dream_setting_item_prefab, m_setting_btn_borm);
                //                dream_setting_item.Init(analysis[0], SumSave.crt_setting.battle_Boss_list[i].Item2);
                //                break;
                //            }
                //        }
                //    }

                //}
                break;
            case setting_type.系统设置:
                List<int> settings = SumSave.crt_setting.user_data_settings;
                for (int i = 0; i < SumSave.db_sttings.Count; i++)
                {
                    dream_setting_item dream_setting_item = Instantiate(dream_setting_item_prefab, m_setting_btn_borm);
                    dream_setting_item.Init(i, SumSave.db_sttings[i], settings.Count > i ? settings[i] : SumSave.db_sttings[i].setting_type);
                }
                break;
        }
    }
    /// <summary>
    /// 接受设置
    /// </summary>
    /// <param name="info"></param>
    protected void DreamSetting((int,int,string, int) info)
    {
        for (int i = 0; i < setting_list.Count; i++)
        {
            (int, int, string, int) temp = setting_list[i];
            if (temp.Item1 != 3)
            {
                if (temp.Item1 == info.Item1 && temp.Item2 == info.Item2)
                {
                    setting_list[i] = info;
                    return;
                }
            }
            else
            {
                if (temp.Item3 == info.Item3)
                {
                    setting_list[i] = info;
                    return;
                }
            }
        }
        setting_list.Add(info);
    }
}
