using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pet_item : Base_Mono
{
    /// <summary>
    /// 宠物数据
    /// </summary>
    private string data;
    /// <summary>
    /// 宠物基准数据
    /// </summary>
    private db_pet_vo_old crt_pet;

    private Text text;
    /// <summary>
    /// 宠物奖励列表
    /// </summary>
    private List<(string, int)> pet_item_list=new List<(string, int)>();
    /// <summary>
    /// 可能获得宠物奖励
    /// </summary>
    private string[] va= { "下品历练丹" , "下品经验丹", "下品灵石" };//Assets/Resources/Prefabs/panel_smallWorld/pets/pet_item.prefab

    private Image iocn, frame, state;

    private void Awake()
    {
        text = Find<Text>("info");
        iocn = Find<Image>("iocn");
        frame = Find<Image>("frame");
        state = Find<Image>("state");
    }
    /// <summary>
    /// 是否被选中
    /// </summary>
    public bool Selected
    {
        set { GetComponent<Image>().color = value ? Color.yellow : Color.white; }
    }

    public void Init(string db_pet_vo)
    {
        data = (db_pet_vo);
        text.text ="";
        string[] va = data.Split(',');
        text.text = va[0]+"lv:"+ va[3];
    }
    public void Init(db_pet_vo_old data)
    {
        if(data==null)
        {
            return;
        }
        crt_pet = data;
        text.text = crt_pet.petName + "lv:" + crt_pet.level;
        iocn.sprite = UI.UI_Manager.I.GetEquipSprite("UI/pet/", data.petName);
        frame.sprite = UI.UI_Manager.I.GetEquipSprite("frame/", data.quality);
        if (data.pet_state != "0")
            state.sprite = UI.UI_Manager.I.GetEquipSprite("UI/pet/", data.pet_state);
        else state.gameObject.SetActive(false);

    }
    /// <summary>
    /// 开始获得宠物奖励
    /// </summary>
    public void StartPetItem()
    {
        StartCoroutine(GetItem());
    }
    /// <summary>
    /// 一定时间获得宠物奖励
    /// </summary>
    /// <returns></returns>
    private IEnumerator GetItem()
    {
        yield return new WaitForSeconds(5);
        
        int index = UnityEngine.Random.Range(0, va.Length);//随机获得一个奖励
        //transform.parent.parent.parent.parent.parent.parentSendMessage("Get_pet_guard", crt_pet_Old.SetPet());
        GainRewards(va[index], 5);
        StartCoroutine(GetItem());
    }


    /// <summary>
    /// 随机获得多少个奖励
    /// </summary>
    /// <param name="data"></param>
    /// <param name="num"></param>
    private void GainRewards(string _data,int num)
    {
        int index = UnityEngine.Random.Range(1, num);
        for(int i = 0; i < pet_item_list.Count; i++)
        {
            if(pet_item_list[i].Item1 == _data)
            {
                int temp = pet_item_list[i].Item2+index;
                pet_item_list[i]=(pet_item_list[i].Item1, temp);
                Debug.Log("奖励"+ pet_item_list[i].Item1+ "已获得个数" + temp);
                return;
            }
        }
        pet_item_list.Add((_data, index));
        Debug.Log("奖励" + _data + "已获得个数" + index);
    }
    /// <summary>
    /// 获取宠物数据
    /// </summary>
    /// <returns></returns>
    public string Set()
    {
        return data;
    }
    public db_pet_vo_old SetPet()
    {
        return crt_pet;
    }
    public List<(string, int)> SetItemList()
    {
        return pet_item_list;
    }
}
