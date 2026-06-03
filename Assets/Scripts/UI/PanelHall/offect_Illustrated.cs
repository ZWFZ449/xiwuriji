using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum Illustrated_Type
{
    怪物图鉴 = 1,
    BOSS博物馆,
    装备图鉴
}
public class offect_Illustrated : Base_Mono
{
    
    private Transform m_brom;

    private illustrated_Shape_item illustrated_Shape_Item_Prefab;

    private illustrated_type_item illustrated_Type_Item_Prefab;

    private illustrated_item illustrated_Item_Prefab;

    private btn_list_item btn_List_Item_Prefab;

    private illustrated_type_item crt_illustrated_type;

    private illustrated_Shape_item crt_illustrated_Shape;

    private illustrated_item crt_illustrated;
    /// <summary>
    /// 存储所有数据
    /// </summary>
    private Dictionary<illustrated_type_item, Dictionary<illustrated_Shape_item,Dictionary<btn_list_item, List<illustrated_item>>>> illustrated_Dic = new Dictionary<illustrated_type_item, Dictionary<illustrated_Shape_item, Dictionary<btn_list_item, List<illustrated_item>>>>();
    private void Awake()
    {
        m_brom=Find<Transform>("illustrated_list/Viewport/Content");
        illustrated_Shape_Item_Prefab= Tool_UI.Find_Prefabs<illustrated_Shape_item>("illustrated_Shape_item");
        illustrated_Type_Item_Prefab = Tool_UI.Find_Prefabs<illustrated_type_item>("illustrated_type_item");
        illustrated_Item_Prefab = Tool_UI.Find_Prefabs<illustrated_item>("illustrated_item");
        btn_List_Item_Prefab = Tool_UI.Find_Prefabs<btn_list_item>("btn_list_item");
        Init();
    }
    /// <summary>
    /// 
    /// </summary>
    private void Init()
    {
        ClearObject(m_brom);
        Dictionary<string, int> db_illustrateds = SumSave.crt_illustrated.Get_illustrated_list();
        foreach (var type in SumSave.db_illustrateds)
        {
            illustrated_type_item illustrated_type_item = Instantiate(illustrated_Type_Item_Prefab, m_brom);
            illustrated_type_item.Init((Illustrated_Type)(type.Key), type.Key);
            illustrated_type_item.GetComponent<Button>().onClick.AddListener(() => { OnClick_Type(illustrated_type_item); });
            illustrated_Dic.Add(illustrated_type_item, new Dictionary<illustrated_Shape_item, Dictionary<btn_list_item, List<illustrated_item>>>());
            foreach (var item in type.Value)
            { 
                illustrated_Shape_item illustrated_Shape_item = Instantiate(illustrated_Shape_Item_Prefab, m_brom);
                illustrated_Shape_item.Init(item.Illustrated_name, item);
                illustrated_Shape_item.GetComponent<Button>().onClick.AddListener(() => { OnClick_Shape(illustrated_Shape_item); });
                illustrated_Dic[illustrated_type_item].Add(illustrated_Shape_item, new Dictionary<btn_list_item, List<illustrated_item>>());
                btn_list_item btn_list_item = Instantiate(btn_List_Item_Prefab, m_brom);
                illustrated_Dic[illustrated_type_item][illustrated_Shape_item].Add(btn_list_item, new List<illustrated_item>());
                List<string> list = ArrayHelper.Get_Split<string>(item.Illustrated_need, ',');
                switch ((Illustrated_Type)(type.Key))
                {
                    case Illustrated_Type.怪物图鉴:
                    case Illustrated_Type.BOSS博物馆:
                        if (list.Count > 0)
                        {
                            foreach (var need in list)
                            {
                                List<string> value = ArrayHelper.Get_Split<string>(need, ' ');
                                if (value.Count == 2)
                                { 
                                    illustrated_item illustrated_item = Instantiate(illustrated_Item_Prefab, btn_list_item.m_item);
                                    illustrated_item.InitMonster(value[0], db_illustrateds.ContainsKey(value[0])? db_illustrateds[value[0]]:0, int.Parse(value[1]), item);
                                    illustrated_item.GetComponent<Button>().onClick.AddListener(() => { OnClick_EquipItem(illustrated_item); });
                                    illustrated_Dic[illustrated_type_item][illustrated_Shape_item][btn_list_item].Add(illustrated_item);
                                }
                            }
                        }
                        break;
                    case Illustrated_Type.装备图鉴:
                        if (list.Count > 0)
                        {
                            foreach (var need in list)
                            {
                                if (need != "")
                                { 
                                    illustrated_item illustrated_item = Instantiate(illustrated_Item_Prefab, btn_list_item.m_item);
                                    illustrated_item.InitIcon(need, db_illustrateds.ContainsKey(need) ? db_illustrateds[need] : 0,item);
                                    illustrated_item.GetComponent<Button>().onClick.AddListener(() => { OnClick_EquipItem(illustrated_item); });
                                    illustrated_Dic[illustrated_type_item][illustrated_Shape_item][btn_list_item].Add(illustrated_item);
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
                btn_list_item.gameObject.SetActive(false);
                illustrated_Shape_item.gameObject.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        if (SumSave.crtHero.lv < 15)  
        { 
            Alert_Dec.Show("图鉴功能在15级开放");
            Hide();
        }
    }
    private void Hide()
    {
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }


    private void OnClick_EquipItem(illustrated_item item)
    {
        Dictionary<string, int> crt_illustrated = SumSave.crt_illustrated.Get_illustrated_list();
        db_illustrated_vo illustrated = item.GetCrt;
        int crt_number = 0;
        string dec="";
        List<string> list = ArrayHelper.Get_Split<string>(illustrated.Illustrated_need, ',');
        List<string> effect = ArrayHelper.Get_Split<string>(illustrated.Illustrated_effect, ',');
        if (crt_illustrated.ContainsKey(item.GetIndex))
        {
            crt_number = crt_illustrated[item.GetIndex];
        }
        switch ((Illustrated_Type)illustrated.Illustrated_type)
        {
            case Illustrated_Type.怪物图鉴:
            case Illustrated_Type.BOSS博物馆:
                dec += "击杀" + item.GetIndex+"\n";
                for (int i = 0; i < list.Count; i++)
                {
                    List<string> value = ArrayHelper.Get_Split<string>(list[i], ' ');
                    if (value.Count == 2)
                    { if (value[0] == item.GetIndex)
                        {
                            dec += "击杀进度" + crt_number + " / " + value[1] + "次\n";
                            if (effect.Count >= i)
                            { 
                                List<string> crt_effect= ArrayHelper.Get_Split<string>(effect[i], '&');
                                dec += "图鉴加成\n";
                                foreach (var item1 in crt_effect)
                                {
                                    List<string> effect_value = ArrayHelper.Get_Split<string>(item1, ' ');
                                    if (effect_value.Count == 2)
                                    {
                                        dec+= Info_Suit(effect_value, crt_number >= int.Parse(value[1])) + "\n";
                                    }
                                }
                            }
                        }
                    }
                }
                Alert.Show("图鉴收集", dec);

                break;
            case Illustrated_Type.装备图鉴:
                int number = 0;
                dec += "图鉴 " + Show_Color.Yellow(illustrated.Illustrated_name) +" ("+ list.Count+" 件)\n";
                for (int i = 0; i < list.Count; i++)
                {
                    if (crt_illustrated.ContainsKey(list[i]))
                    {
                        number++;
                        dec += Show_Color.Green(list[i] + ":" + crt_illustrated[list[i]] + " / 1\n");
                    }
                    else dec += Show_Color.Grey(list[i] + ":" + 0 + " / 1\n");
                }
                dec += "图鉴加成\n";
                foreach (var item1 in effect)
                {
                    List<string> crt_effect = ArrayHelper.Get_Split<string>(item1, '&');
                    for (int i = 0; i < crt_effect.Count; i++)
                    {
                        List<string> effect_value = ArrayHelper.Get_Split<string>(crt_effect[i], ' ');
                        if (effect_value.Count == 3)
                        {
                            dec += Info_Suit(effect_value, number) + "\n";
                        }
                    }

                } 
                if (crt_number == 0)
                {
                    dec += Show_Color.Red("放入" + enum_equip_quality_list.帝器 + " " + item.GetIndex);
                    Alert.Show("图鉴收集", dec, Add_illustrated, item);


                }
                else
                Alert.Show("图鉴收集", dec);
                break;
        }
    }
    /// <summary>
    /// 加入图鉴
    /// </summary>
    /// <param name="arg0"></param>
    private void Add_illustrated(object arg0)
    {
        illustrated_item item = (illustrated_item)arg0;
        //测试
        //SumSave.crt_illustrated.Add_illustrated_list(item.GetIndex);
        //SendNotification(NotiList.Refresh_Max_Hero_Attribute);
        List<Bag_Base_VO> crtbags=SumSave.crt_bags.Get_Bag_List();
        for (int i = 0; i < crtbags.Count; i++)
        {
            if (crtbags[i].Name == item.GetIndex)
            { 
                List<string> list = ArrayHelper.Get_Split<string>(crtbags[i].user_value, ' ');
                if (list[3] == "0")//不锁定
                {
                    if (int.Parse(list[2]) >= (int)enum_equip_quality_list.帝器)//品质达标
                    { 
                        SumSave.crt_illustrated.Add_illustrated_list(item.GetIndex);
                        Alert.Show("图鉴收集", "图鉴收集成功");
                        crtbags.RemoveAt(i);
                        SumSave.crt_bags.Set_Bag_List(crtbags);
                        SendNotification(NotiList.Refresh_Max_Hero_Attribute);
                        item.SetCount(1);
                        OnClick_EquipItem(item);
                        return;
                    }
                }
            }
        }
        Alert_Dec.Show("图鉴收集失败,缺少未锁定的 " + enum_equip_quality_list.帝器 + " " + item.GetIndex);
    }

    private string Info_Suit(List<string> suit,int number)
    {
        string value = suit[0] + "件加成 ";
        if (suit.Count == 3)
        {
            switch ((Suit_Type)(int.Parse(suit[1])))
            {
                case Suit_Type.物理攻击:
                case Suit_Type.魔法攻击:
                case Suit_Type.道术攻击:
                case Suit_Type.双防:
                    value += (Suit_Type)(int.Parse(suit[1]))+" : " + (suit[2]) + " - " + suit[2];
                    break;
                case Suit_Type.生命:
                case Suit_Type.伤害吸收:
                case Suit_Type.真实伤害:
                    value += (Suit_Type)(int.Parse(suit[1])) + " : " + (suit[2]);
                    break;
                case Suit_Type.攻击速度:
                case Suit_Type.物攻属性:
                case Suit_Type.魔攻属性:
                case Suit_Type.道攻属性:
                case Suit_Type.双防属性:
                    value += (Suit_Type)(int.Parse(suit[1])) + " : " + (suit[2]) + " %";
                    break;
            }
            value = number >= int.Parse(suit[0]) ? Show_Color.Green(value) : Show_Color.Grey(value);
        }
        return value;
    }

    private string Info_Suit(List<string> suit, bool isShow)
    {
        string value = "";
        if (suit.Count == 2)
        {
            switch ((Suit_Type)(int.Parse(suit[0])))
            {
                case Suit_Type.物理攻击:
                case Suit_Type.魔法攻击:
                case Suit_Type.道术攻击:
                case Suit_Type.双防:
                    value += (Suit_Type)(int.Parse(suit[0])) + " : " + (suit[1]) + " - " + suit[1];
                    break;
                case Suit_Type.生命:
                case Suit_Type.伤害吸收:
                case Suit_Type.真实伤害:
                    value += (Suit_Type)(int.Parse(suit[0])) + " : " + (suit[1]);
                    break;
                case Suit_Type.攻击速度:
                case Suit_Type.物攻属性:
                case Suit_Type.魔攻属性:
                case Suit_Type.道攻属性:
                case Suit_Type.双防属性:
                    value += (Suit_Type)(int.Parse(suit[0])) + " : " + (suit[1]) + " %";
                    break;
            }
            value = isShow ? value : Show_Color.Grey(value);
        }
        return value;
    }
    private void OnClick_Shape(illustrated_Shape_item illustrated_Shape_item)
    {
        crt_illustrated_Shape = illustrated_Shape_item;
        Dictionary<string, int> dic = SumSave.crt_illustrated.Get_illustrated_list();
        foreach (var item in illustrated_Dic[crt_illustrated_type][crt_illustrated_Shape])
        {
            bool exist = item.Key.gameObject.activeSelf;
            exist = !exist;
            item.Key.gameObject.SetActive(exist);
            if (exist)
            {
                foreach (var Shape in item.Value)
                {
                    //同步数据
                    if (dic.ContainsKey(Shape.GetIndex))
                    { 
                        Shape.SetCount(dic[Shape.GetIndex]);
                    }
                }
            }
        }
    }

    private void OnClick_Type(illustrated_type_item illustrated_type_item)
    {
        if (crt_illustrated_type != null && illustrated_type_item != crt_illustrated_type) ClearType(crt_illustrated_type,false);
        crt_illustrated_type = illustrated_type_item;
        ClearType(crt_illustrated_type);
    }

    private void ClearType(illustrated_type_item illustrated_type_item,bool isClear = true)
    {
        if (illustrated_Dic.ContainsKey(illustrated_type_item))
        {
            foreach (var item in illustrated_Dic[illustrated_type_item])
            {
                bool exist = item.Key.gameObject.activeSelf;
                exist = !exist;
                item.Key.gameObject.SetActive(isClear ? exist : isClear);
                foreach (var Shape in item.Value)
                {
                    Shape.Key.gameObject.SetActive(false);
                }
            }
        }
    }
}
