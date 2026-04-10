using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class panel_wodleBoss : Panel_Base
{
    /// <summary>
    /// 世界boss图标
    /// </summary>
    private Image icon;
    /// <summary>
    /// 进入调整按钮
    /// </summary>
    private Button up_map;
    /// <summary>
    /// 战斗地图
    /// </summary>
    private panel_fight fight_panel;
    /// <summary>
    /// 属性显示位置
    /// </summary>
    private Transform crt;
    /// <summary>
    /// boss挑战进度条
    /// </summary>
    private Slider progress;
    /// <summary>
    /// 血量显示，次数显示
    /// </summary>
    private Text Hp_Text, numberText, nameText;
    /// <summary>
    /// 最大生命值
    /// </summary>
    private long maxHp;
    /// <summary>
    /// 当前世界Boss挑战次数，最大挑战次数
    /// </summary>
    private int boss_number = 0, boss_number_max = 1;
    /// <summary>
    /// 排行榜预制体
    /// </summary>
    private rank_item rank_itemPrefab;
    /// <summary>
    /// 排行榜位置
    /// </summary>
    private Transform information;

    public override void Initialize()
    {
        base.Initialize();
        fight_panel = UI_Manager.I.GetPanel<panel_fight>();
        icon = Find<Image>("boss_icon");
        up_map = Find<Button>("up_map");
        up_map.onClick.AddListener(Challenge);
        crt = Find<Transform>("information/Viewport/Content");
        progress = Find<Slider>("progress");
        Hp_Text = Find<Text>("progress/Hp_Text");
        numberText = Find<Text>("up_map/number");
        nameText =  Find<Text>("boss_icon/nameText");
        rank_itemPrefab = Battle_Tool.Find_Prefabs<rank_item>("rank_item");
        information= Find<Transform>("information/Viewport/Content");

        List<(string, int)> list = SumSave.crt_needlist.SetMap();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Item1 == SumSave.db_world_boos.name)
            {
                boss_number = list[i].Item2;
            }
        }
        Init();
    }

   /// <summary>
   /// 初始化世界boss排行榜
   /// </summary>
    private void GetList()
    {
        ClearObject(crt);
        for (int i = 0; i < SumSave.crt_world_boss_rank.lists.Count; i++)
        {
            rank_item item = Instantiate(rank_itemPrefab, crt);
            item.Data2 = SumSave.crt_world_boss_rank.lists[i];
            item.Show_index2(i + 1);
        }
    }


    /// <summary>
    /// 点击挑战
    /// </summary>
    private void Challenge()
    {
        if(SumSave.db_world_boos.Get()<=0)
        {
            Alert_Dec.Show("世界Boss已被击败");
            return;
        }

#if UNITY_EDITOR
        if (boss_number >= boss_number_max)
        {
            Alert_Dec.Show("挑战次数不足");
            return;
        }
#elif UNITY_ANDROID
        if (boss_number >= boss_number_max)
        {
            Alert_Dec.Show("挑战次数不足");
            return;
        }
#elif UNITY_IPHONE
        if (boss_number >= boss_number_max)
        {
            Alert_Dec.Show("挑战次数不足");
            return;
        }
#endif


        IncreaseFrequency();
        long hurt=Random.Range(SumSave.crt_MaxHero_okd.totalPower*50/100, SumSave.crt_MaxHero_okd.totalPower * 60 / 100);//每次挑战根据战力50%-60%随机伤害


#if UNITY_EDITOR
       // hurt = 1;
#elif UNITY_ANDROID

#elif UNITY_IPHONE

#endif



        world_Boss_set(hurt);
        crate_rank(hurt);
        GainRewards(hurt);
        Init();
    }
    /// <summary>
    /// 获得奖励
    /// </summary>
    private void GainRewards(long hurt)
    {
        List < (int, long) > list = SumSave.db_world_boos.DamageLevel_List;
        for (int i = list.Count-1; i >=0; i--)
        {
            if(list[i].Item2<= hurt)
            {
                long experience = 400 * list[i].Item1;
                long MagicPill = 20 * list[i].Item1;
                int honor = 2;
                string dec = "造成伤害：" + hurt;
                dec += "\n获得奖励：\n历练 + " + experience + "\n魔丸 + " + MagicPill;
                SumSave.crt_user_unit.verify_data(currency_unit.元宝, experience);
                SumSave.crt_user_unit.verify_data(currency_unit.Boss积分, MagicPill);
                if (boss_number == 1)
                {
                    SumSave.crt_accumulatedrewards.Set(2, honor);
                    dec += "\n荣誉 + " + honor;
                } 
                Alert.Show("世界Boss挑战奖励", dec);
                return;
            }
        }
    }



    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        if (SumSave.db_world_boos.Get() <= 0)///如果boss死亡，重置挑战
        {
            SumSave.db_world_boos.number++;
            SumSave.db_world_boos.InitFinalDamage(SumSave.db_world_boos.BossHpBasic * SumSave.db_world_boos.number);
            SumSave.db_world_boos.UpTime = SumSave.nowtime.ToString();
            SendNotification(NotiList.Read_Crate_world_boss_update);

            SumSave.crt_world_boss_rank.lists = new List<(string, string, long)>();
            SumSave.crt_world_boss_rank.SetData();
            Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.user_world_boss_rank,
            SumSave.crt_world_boss_rank.Set_Uptade_String(), SumSave.crt_world_boss_rank.Get_Update_Character());
            SendNotification(NotiList.Read_Crate_RecordAndClearWorldBoss);
        }

        SendNotification(NotiList.Read_Crate_world_boss_Login);
        icon.sprite = Resources.Load<Sprite>("Prefabs/monsters/" + SumSave.db_world_boos.name);
        maxHp = SumSave.db_world_boos.BossHpBasic * SumSave.db_world_boos.number;
        progress.maxValue = maxHp;
        progress.value = SumSave.db_world_boos.Get();
        numberText.text = "挑战次数:" + boss_number.ToString() + "/" + boss_number_max;
        nameText.text = SumSave.db_world_boos.name;
        GetList();
        Hp_Text.text = SumSave.db_world_boos.Get() + "/" + maxHp;
    }
    public override void Hide()
    {
        base.Hide();
    }

    public override void Show()
    {
        base.Show();
        if (SumSave.crt_MaxHero_okd.Lv < 20&& SumSave.ios_account_number != "admin001")
        {
            Alert_Dec.Show("世界Boss开启等级为20级");
            gameObject.SetActive(false);
            return;
        }
        Init();

    }
    /// <summary>
    /// 写入对世界boss的伤害
    /// </summary>
    /// <param name="finalDamage"></param>
    private void world_Boss_set(long finalDamage)
    {
        SumSave.db_world_boos.Set(finalDamage);
        SendNotification(NotiList.Read_Crate_world_boss_update);
        SumSave.crt_world_boss_hurt.CauseDamage(finalDamage);
    }

    /// <summary>
    /// 挑战次数++
    /// </summary>
    private void IncreaseFrequency()
    {
        boss_number++;
        bool exist = true;
        List<(string, int)> list = SumSave.crt_needlist.SetMap();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Item1 == SumSave.db_world_boos.name)
            {
                exist = false;
                list[i] = (list[i].Item1, list[i].Item2 + 1);
                SumSave.crt_needlist.SetMap(list[i]);
                break;
            }
        }
        if (exist)
        {
            SumSave.crt_needlist.SetMap((SumSave.db_world_boos.name, 1));
        }
    }




    /// <summary>
    /// 刷新Boss排行榜
    /// </summary>
    private  void Refresh_Rank()
    {

        //if(SumSave.crt_world_boss_rank.lists[SumSave.crt_world_boss_rank.lists.Count-1].Item3<SumSave.crt_world_boss_hurt.damage)
        //{
        //    SumSave.crt_world_boss_rank.lists.Add((SumSave.crt_user.uid, SumSave.crt_hero.hero_name, SumSave.crt_world_boss_hurt.damage));
        //}
        SumSave.crt_world_boss_rank.lists = ArrayHelper.OrderDescding(SumSave.crt_world_boss_rank.lists, x => x.Item3);

        if(SumSave.crt_world_boss_rank.lists.Count>50)//最多显示50条
        {
            int Sum = SumSave.crt_world_boss_rank.lists.Count-1;
            for (int i = Sum; i >49; i--)
            {
                SumSave.crt_world_boss_rank.lists.RemoveAt(i);
            }
        }
        //SumSave.crt_world_boss_rank.SetData();
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.user_world_boss_rank,
          SumSave.crt_world_boss_rank.Set_Uptade_String(), SumSave.crt_world_boss_rank.Get_Update_Character());
    }
    /// <summary>
    /// 创建Boss排行榜
    /// </summary>
    private void crate_rank(long rank)
    {
        ///(uid,名字，伤害)
        List<(string, string, long)> Boss_list = SumSave.crt_world_boss_rank.lists;
        
        for (int i = Boss_list.Count-1; i >=0; i--)
        {
            if (Boss_list[i].Item1 == SumSave.crt_user.uid)
            {
                Boss_list.RemoveAt(i);
            }
        }
        Boss_list.Add((SumSave.crt_user.uid,SumSave.old_crt_hero.hero_name,SumSave.crt_world_boss_hurt.damage));
        SumSave.crt_world_boss_rank.lists = Boss_list;
        //排序
        Refresh_Rank();
        return;

    }

}
