using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class user_pet_vo : Base_VO
{
    /// <summary>
    /// 宠物信息 0宠物名字 1孵化时间  2宠物品质 3宠物等级 4宠物经验 5宠物属性 6 pos 他在干什么0闲置1守护庄园2是探索
    /// </summary>
    public string pet_value;
    /// <summary>
    /// 获取属性基准值
    /// </summary>
    private List<db_pet_vo_old> pet_list = new List<db_pet_vo_old>();
    /// <summary>
    /// 宠物蛋列表
    /// </summary>
    private List<string> crt_pet_eggs = new List<string>();
 
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="value"></param>
    public void Init(string value)
    {
        if (value == "") return;
        
        string[] pets = value.Split('&');
        for (int i = 0; i < pets.Length; i++)
        {
            string[] splits = pets[i].Split(',');
            db_pet_vo_old pet = new db_pet_vo_old();
            db_pet_vo_old base_pet = ArrayHelper.Find(SumSave.db_pet_old, e => e.petName == splits[0]);
            pet.pet_explore = base_pet.pet_explore;
            if (splits.Length == 7)
            {
                pet.petName = splits[0];
                pet.startHatchingTime = DateTime.Parse(splits[1]);
                pet.quality = splits[2];
                pet.level = int.Parse(splits[3]);
                pet.exp = int.Parse(splits[4]);
                string[] attributes = splits[5].Split('|');
                pet.crate_value = "";
                pet.up_value = "";
                pet.up_base_value = "";
                pet.crate_value = attributes[0];
                pet.up_value = attributes[1];
                pet.up_base_value = attributes[2];
                pet.GetNumerical();
                pet.pet_state = splits[6];
                pet_list.Add(pet);
            }
            else crt_pet_eggs.Add(pets[i]);
        }

    }
    /// <summary>
    /// 获取宠物
    /// </summary>
    /// <returns></returns>
    public List<db_pet_vo_old> Set()
    {
        return pet_list;
    }

    public void Get_pet_list(db_pet_vo_old pet)
    {
        pet_list.Add(pet);
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_pet,
       SumSave.crt_pet_Old.Set_Uptade_String(), SumSave.crt_pet_Old.Get_Update_Character());
    }


    /// <summary>
    /// 获取宠物蛋列表
    /// </summary>
    /// <returns></returns>
    public List<string> GetEggs()
    { 
        return crt_pet_eggs;
    }
    /// <summary>
    /// 写入宠物
    /// </summary>
    public void Get()
    {
        MysqlData();
    }

    public override void MysqlData()
    {
        base.MysqlData();
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_pet,
        SumSave.crt_pet_Old.Set_Uptade_String(), SumSave.crt_pet_Old.Get_Update_Character());
    }

    public string Set_pet_value()
    {
        string value = "";
        foreach (var pet in pet_list)
        {
            value += value == "" ? "" : "&";
            value += pet.petName + ",";
            value += pet.startHatchingTime + ",";
            value += pet.quality + ",";
            value += pet.level + ",";
            value += pet.exp + ",";
            value += pet.crate_value + "|" + pet.up_value + "|" + pet.up_base_value + ",";
            value += pet.pet_state;
        }
        for (int i = 0; i < crt_pet_eggs.Count; i++)
        {
            if (value != "")
            {
                value += "&";
            }
            value += crt_pet_eggs[i];
        }

        return value;

    }


    public override string[] Set_Instace_String()
    {
        return new string[]
        {
           GetStr(0),
           GetStr(SumSave.crt_user.uid),
           GetStr(pet_value),
        };
    }

    public override string[] Get_Update_Character()
    {
        return new string[]
        {
            "user_value",
        };
    }


    public override string[] Set_Uptade_String()
    {
        return new string[]
        {
            GetStr(Set_pet_value()),
        };
    }

}
