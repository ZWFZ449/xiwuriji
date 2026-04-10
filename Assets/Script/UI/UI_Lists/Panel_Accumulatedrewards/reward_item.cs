using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class reward_item : Base_Mono
{
    private Text need_info;
    private Button reward;

    private Transform crt_list;

    private material_item material_Prefabs;
    private int index;
    private void Awake()
    {
        need_info = Find<Text>("need_info");
        reward = Find<Button>("bg/reward");
        reward.onClick.AddListener(Reward);
        crt_list = Find<Transform>("list");
        material_Prefabs = Battle_Tool.Find_Prefabs<material_item>("material_item");
    }

    private void Reward()
    {
        transform.parent.parent.parent.parent.parent.SendMessage("Reward", index);
    }

    public void Init(int _index,(int,string) value,bool exist,int type)
    {
        reward.gameObject.SetActive(exist);
        index = _index;
        string[] info = value.Item2.Split(',');
        need_info.text = "累积需求 " + value.Item1 + (type == 1 ? "次" : "天");
        for (int i = 0; i < info.Length; i++)
        {
            if (info[0] != "")
            { 
                string[] info2 = info[i].Split(' ');
                if (info2.Length == 3)
                {
                   Instantiate(material_Prefabs, crt_list).Init(((info2[1]), int.Parse(info2[2])));
                }
            }
        }
    }
}
