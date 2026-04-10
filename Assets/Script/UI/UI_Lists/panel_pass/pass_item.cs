using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pass_item : Base_Mono
{
    private material_item material_item_parfabs;

    private Image Image_parfabs;
    /// <summary>
    /// 物品信息
    /// </summary>
    private Transform info_lv, info_value, upinfo_value;
    private void Awake()
    {
        info_lv =  Find<Transform>("pos_index"); 
        info_value = Find<Transform>("info_value");
        upinfo_value = Find<Transform>("upinfo_value"); 
        material_item_parfabs = Battle_Tool.Find_Prefabs<material_item>("material_item"); //Resources.Load<material_item>("Prefabs/panel_bag/material_item");
        Image_parfabs = Resources.Load<Image>("Prefabs/panel_text/Image_text");
    }
    /// <summary>
    /// 领取进阶奖励
    /// </summary>
    private void OnupLvReceive()
    {
        transform.parent.parent.parent.parent.parent.SendMessage("GetupLvReward", this);

    }
    /// <summary>
    /// 点击领取
    /// </summary>
    private void OnReceive()
    {
        transform.parent.parent.parent.parent.parent.SendMessage("GetReward", this);
    }

    private user_pass_vo data;
    /// <summary>
    /// Data
    /// </summary>
    public user_pass_vo Data
    {
        set
        {
            data = value;
            if (data == null) return;
            //info.text = data.arrifact_name + "(未激活)";
            getList();
        }
        get
        {
            return data;
        }
    }

    private void getList()
    {
        string dec = data.lv.ToString();
        for (int i = 0; i < dec.Length; i++)
        {
            Image img = Instantiate(Image_parfabs, info_lv);
            img.sprite= UI.UI_Manager.I.GetEquipSprite("base_bg/文字/", dec[i].ToString());
            img.color = Color.yellow;  
        }
        Show_Info(data.reward, info_value);
        Show_Info(data.uplv_reward, upinfo_value);
    }
    /// <summary>
    /// 显示信息
    /// </summary>
    /// <param name="reward"></param>
    /// <returns></returns>
    private void Show_Info(string reward,Transform crt)
    {
        string[] arr = reward.Split('*');
        if (arr.Length >= 2)
        {
            // Debug.Log(arr[0] + " " + arr[1]);
            Instantiate(material_item_parfabs, crt).Init((arr[0], int.Parse(arr[1])));
        }
    }

    /// <summary>
    /// 读取等级
    /// </summary>
    public void Set(int lv,int upLv)
    {
        for (int i = info_value.childCount - 1; i >= 0; i--)
        {
            info_value.GetChild(i).GetComponent<material_item>().SetFrame(lv == 0);
            if (lv == 0)
            {
                info_value.GetChild(i).GetComponent<material_item>().GetComponent<Button>()
                    .onClick.AddListener(OnReceive);
            }
        }
        for (int i = upinfo_value.childCount - 1; i >= 0; i--)
        {
            upinfo_value.GetChild(i).GetComponent<material_item>().SetFrame(upLv == 0);
            if (upLv == 0)
            {
                upinfo_value.GetChild(i).GetComponent<material_item>().GetComponent<Button>()
                    .onClick.AddListener(OnupLvReceive);
            }
        }
    }
}
