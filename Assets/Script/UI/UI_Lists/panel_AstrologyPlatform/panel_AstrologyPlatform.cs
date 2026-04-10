

using Common;
using Components;
using MVC;
using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class panel_AstrologyPlatform : Panel_Base
{
    /// <summary>
    /// 天气图标
    /// </summary>
    private Image weatherImage;
    /// <summary>
    /// 天气介绍
    /// </summary>
    private Text information;
    /// <summary>
    /// 切换天气按钮
    /// </summary>
    private Button switchButton;
    /// <summary>
    /// 切换天气消耗灵珠数量
    /// </summary>
    private int need = 50;
    /// <summary>
    /// 上一次天气时间
    /// </summary>
    private int lastTime = 0;
    
    public override void Initialize()
    {
        base.Initialize();
        weatherImage = Find<Image>("weatherImage");
        information = Find<Text>("information/Viewport/Content/Text");
        switchButton = Find<Button>("switchButton");
        switchButton.onClick.AddListener(SwitchWeather);
        
    }
    private void Update()
    {
        Init();
    }
    /// <summary>
    /// 界面初始化
    /// </summary>
    private void Init()
    {
        bool isHave = false;
        if (SumSave.crt_player_buff.player_Buffs.Count > 0)
        {
            foreach (var _item in SumSave.crt_player_buff.player_Buffs)
            {
                if (_item.Value.Item4 == 4)
                {
                    ///切换图片
                    //weatherImage.sprite = Resources.Load<Sprite>("" + _item.Key);
                    for (int i = 0; i < SumSave.db_weather_list.Count; i++)
                    {
                        if (SumSave.db_weather_list[i].weather_type == _item.Key)
                        {
                            if (_item.Value.Item2 - Battle_Tool.SettlementTransport((_item.Value.Item1).ToString()) <= 0)
                            {
                                SumSave.crt_player_buff.player_Buffs.Remove(_item.Key);
                                AddWeather();
                            }
                            isHave = true;
                            string str = ShowBonus(SumSave.db_weather_list[i]);
                            int time = (_item.Value.Item2 - Battle_Tool.SettlementTransport((_item.Value.Item1).ToString()));
                            str += "剩余时间：" + time + "Min";
                            information.text = str;
                        }
                    }
                    if (!isHave)
                    {
                        AddWeather();
                        Init();
                    }
                }
            }
        }
        else
        {
            AddWeather();
            Init();
        }
       


    }
    /// <summary>
    /// 显示属性
    /// </summary>
    /// <param name="id"></param>
    private string ShowBonus(db_weather weather)
    {
        string str = "";
        str += "天象:" + weather.weather_type + "\n";
        for (int j = 0; j < weather.life_value_list.Count; j++)
        {
            switch (weather.life_value_list[j].Item1)
            {
                case 1:
                    //int baseValue = SumSave.crt_MaxHero.life[1];
                    //int percentageValue = baseValue * (id.Item2 / 100);
                    //int price = baseValue > percentageValue ? baseValue : percentageValue;
                    str += enum_skill_attribute_list.土.ToString() + ":" + weather.life_value_list[j].Item2 + "%\n";
                    break;
                case 2:
                    str += enum_skill_attribute_list.火.ToString() + ":" + weather.life_value_list[j].Item2 + "%\n";
                    break;
                case 3:
                    str += enum_skill_attribute_list.水.ToString() + ":" + weather.life_value_list[j].Item2 + "%\n";
                    break;
                case 4:
                    str += enum_skill_attribute_list.木.ToString() + ":" + weather.life_value_list[j].Item2 + "%\n";
                    break;
                case 5:
                    str += enum_skill_attribute_list.金.ToString() + ":" + weather.life_value_list[j].Item2 + "%\n";
                    break;

            }
        }
       return str;
    }

    /// <summary>
    /// 切换天气
    /// </summary>
    private void SwitchWeather()
    {
        NeedConsumablesold(currency_unit.Boss积分, need);
        if (RefreshConsumables_old())
        {
            if (SumSave.crt_player_buff.player_Buffs.Count > 0)
            {
                bool isAdd = true;
                foreach (var _item in SumSave.crt_player_buff.player_Buffs)
                {
                    if (_item.Value.Item4 == 4)
                    {
                        isAdd = false;
                        SumSave.crt_player_buff.player_Buffs.Remove(_item.Key);
                        AddWeather();
                        break;
                    }
                }
                if (isAdd)
                {
                    AddWeather();
                }
            }else
            {
                AddWeather();
            }
            Alert_Dec.Show("切换成功");
            Init();
        }
        else
        {
            Alert_Dec.Show("灵珠不足");
        }
    }
   
    public override void Hide()
    {
        base.Hide();
    }

   

    public override void Show()
    {
        base.Show();
        Init();
    }
}
