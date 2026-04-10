using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;
/// <summary>
/// 邮件列表
/// </summary>
public class offect_emial : Base_Mono
{
    /// <summary>
    /// 邮件位置
    /// </summary>
    private Transform pos_list, pos_receive;
    /// <summary>
    /// 邮件列表
    /// </summary>
    private emial_item emial_Item_Prefab;
    /// <summary>
    /// 当前邮件
    /// </summary>
    private emial_item crtMail;

    private material_item material_Item_Prefab;
    /// <summary>
    /// 显示信息
    /// </summary>
    private Text title, content;
    /// <summary>
    /// 接收按钮
    /// </summary>
    private Button btn_receive;
    /// <summary>
    /// 领取列表
    /// </summary>
    private List<db_mail_vo> emial_items = new List<db_mail_vo>();
    private void Awake()
    {
        emial_Item_Prefab = Battle_Tool.Find_Prefabs<emial_item>("emial_item");
        pos_list = Find<Transform>("offect/Scroll View/Viewport/Content");
        content = Find<Text>("offect/show_offect/content");
        pos_receive= Find<Transform>("offect/show_offect/Scroll View/Viewport/Content");
        material_Item_Prefab= Battle_Tool.Find_Prefabs<material_item>("material_item");
        title = Find<Text>("offect/show_offect/info");
        content= Find<Text>("offect/show_offect/content");
        btn_receive= Find<Button>("offect/show_offect/receive");
        btn_receive.onClick.AddListener(Receive);
    }
    /// <summary>
    /// 领取奖励
    /// </summary>
    private void Receive()
    {
        if (crtMail == null)
        {
            Alert_Dec.Show("当前暂无可领取邮件");
            return;
        } 
        if (crtMail.crt_mail.mail_par == -1)
        {
            foreach (var item in SumSave.CrtMail.lists)
            {
                if (item == crtMail.crt_mail.mail_id)
                {
                    Alert_Dec.Show("当前邮件已领取");
                    return;
                }
            }
            SumSave.CrtMail.lists.Add(crtMail.crt_mail.mail_id);
            Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.user_emial, 
                SumSave.CrtMail.Set_Uptade_String(),SumSave.CrtMail.Get_Update_Character());
            Receive_Resources();
        }
        else
        {
            if (emial_items.Count > 0)
            {
                for (int i = 0; i < emial_items.Count; i++)
                {
                    if (crtMail.crt_mail.mail_time == emial_items[i].mail_time)
                    {
                        Alert_Dec.Show("当前邮件已领取");
                        return;
                    }
                }
            }
            if (crtMail.crt_mail.moeny > 0) SumSave.crt_accumulatedrewards.Set(1, crtMail.crt_mail.moeny);
            emial_items.Add(crtMail.crt_mail);
            Game_Omphalos.i.GetQueue(Mysql_Type.Delete, Mysql_Table_Name.server_mail,
                new string[] { Battle_Tool.GetStr(crtMail.crt_mail.uid) }, new string[] { "mail_recipient" });
            Game_Omphalos.i.GetQueue(Mysql_Type.InsertInto, Mysql_Table_Name.history_server_mail,
              crtMail.crt_mail.Set_Instace_String());
            SumSave.Db_Mails.Remove(crtMail.crt_mail);
            Receive_Resources();
        }
        
    }
    /// <summary>
    /// 领取资源
    /// </summary>
    private void Receive_Resources()
    {
        Dictionary<int, Dictionary<string, int>> str = crtMail.crt_mail.mail_dec;
        foreach (var index in str.Keys)
        {
            foreach (var material in str[index].Keys)
            {
                switch (index)
                {
                    case 1://货币
                        Battle_Tool.Obtain_Unit
                        ((currency_unit)Enum.Parse(typeof(currency_unit), material), str[index][material]);
                        break;
                    case 2://道具
                        int random = Random.Range(1, 100);
                        int number = str[index][material];
                        int maxnumber = number + Random.Range(1, 100);
                        Battle_Tool.Obtain_Resources(Obtain_Int.Add(1, material, new int[] { number + random, random }), maxnumber);
                        //Battle_Tool.Obtain_Resources(material, str[index][material],true);
                        break;
                    case 3://通行证
                        Dictionary<int, List<int>> dic = SumSave.crt_pass.Set();
                        if(!dic.ContainsKey(int.Parse(material)))
                        {
                            dic.Add(int.Parse(material), new List<int>());
                        }
                        SumSave.crt_pass.Get(dic);
                        break;
                    case 4://皮肤
                        SumSave.old_crt_hero.hero_value += (SumSave.old_crt_hero.hero_value == "" ? "" : ",") + material;
                        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_hero, new string[] { Battle_Tool.GetStr(SumSave.old_crt_hero.hero_value) },
                            new string[] { "hero_value" });
                        break;
                    case 5://月卡
                        AddBuff("月卡",1.5f, 3, str[index][material]);
                        Tool_State.activation_State(State_List.月卡);
                        SendNotification(NotiList.Refresh_Max_Hero_Attribute);
                        break;
                    case 6://至尊卡
                        AddBuff("至尊卡", 1.68f, 5, str[index][material]);
                        Tool_State.activation_State(State_List.至尊卡);
                        SendNotification(NotiList.Refresh_Max_Hero_Attribute);
                        break;
                    default:
                        break;
                }
            }
        }
        Alert_Dec.Show("领取成功");
        Base_Show();
    }
    /// <summary>
    /// 添加buff
    /// </summary>
    /// <param name="_buy_item"></param>
    /// <param name="effect"></param>
    /// <param name="icon">类型 1.经验丹2.历练丹3.月卡4至尊卡</param>
    /// <param name="num"></param>
   
    private void AddBuff(string _buy_item, float effect, int icon,int num)
    {
        if (SumSave.crt_player_buff.player_Buffs.ContainsKey(_buy_item))
        {
            SumSave.crt_player_buff.player_Buffs[_buy_item] =
                (SumSave.crt_player_buff.player_Buffs[_buy_item].Item1,
                SumSave.crt_player_buff.player_Buffs[_buy_item].Item2 + (43200* num)
                , effect, icon);//当有时，增加buff时间
        }
        else
        {
            SumSave.crt_player_buff.player_Buffs.Add(_buy_item, (SumSave.nowtime, 43200* num, effect, icon));
        }
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.user_player_buff, SumSave.crt_player_buff.Set_Uptade_String(), SumSave.crt_player_buff.Get_Update_Character());//角色丹药Buff更新数据库
    }
    /// <summary>
    /// 
    /// </summary>

    public override void Show()
    {
        base.Show();
        SendNotification(NotiList.Read_Mail);
        Base_Show();
    }

    private void Base_Show()
    {
        ClearObject(pos_list);
        for (int i = 0; i < SumSave.Db_Mails.Count; i++)
        {
            if (SumSave.Db_Mails[i].mail_par == -1 || SumSave.Db_Mails[i].uid == SumSave.crt_user.uid)
            { 
                emial_item item = Instantiate(emial_Item_Prefab, pos_list);
                bool exist = false;
                foreach (var index in SumSave.CrtMail.lists)
                {
                    if (index == SumSave.Db_Mails[i].mail_id) exist = true;
                }
                item.SetInfo(SumSave.Db_Mails[i],exist);
                item.GetComponent<Button>().onClick.AddListener(() => { ShowMail(item); });
                if(crtMail==null)ShowMail(item);
            }
        }
    }

    private void ShowMail(emial_item item)
    {
        crtMail= item;
        title.text = item.crt_mail.uid == "-1" ? "系统邮件" : "玩家邮件";
        content.text = item.crt_mail.dec;
        ClearObject(pos_receive);
        Dictionary<int, Dictionary<string, int>> str = item.crt_mail.mail_dec;
        foreach (var index in str.Keys)
        {
            foreach (var material in str[index].Keys)
            {
                Instantiate(material_Item_Prefab, pos_receive).Init((material, str[index][material]));
            }
        }
    }
}
