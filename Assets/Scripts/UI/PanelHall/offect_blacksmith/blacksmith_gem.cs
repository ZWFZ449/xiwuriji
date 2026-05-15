using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class blacksmith_gem : Base_Mono
{
    private TMP_Text info;

    private select_gem_btn_item select_gem_btn_item_prefab;

    private material_item material_item_prefab;

    private dream_BagItem dream_BagItem_prefab;

    private Transform M_bags_brom, M_gem_btn_brom,M_icon_brom;

    private Bag_Base_VO crt_bag;
    /// <summary>
    /// 宝石安装位置
    /// </summary>
    private int index;

    private select_gem_btn_item select_gem_btn_item_crt;

    private Button close;
    private void Awake()
    {
        M_bags_brom = Find<Transform>("bg/bag_list/Viewport/Content");
        M_gem_btn_brom = Find<Transform>("bg/gems"); 
        M_icon_brom = Find<Transform>("bg/icon");
        info = Find<TMP_Text>("bg/title_type_info/info");
        dream_BagItem_prefab = Tool_UI.Find_Prefabs<dream_BagItem>("dream_BagItem");
        select_gem_btn_item_prefab = Tool_UI.Find_Prefabs<select_gem_btn_item>("select_gem_btn_item");
        material_item_prefab = Tool_UI.Find_Prefabs<material_item>("material_item");
        close = Find<Button>("close_button");
        close.onClick.AddListener(() => { Hide(); });
    }

    private void Hide()
    {
        crt_bag = null;
        select_gem_btn_item_crt = null;
        gameObject.SetActive(false);
    }

    public void Show(Bag_Base_VO bag)
    { 
        crt_bag = bag;
        ClearObject(M_icon_brom);
        Instantiate(dream_BagItem_prefab, M_icon_brom).Data = bag;
        Init();
        Show_Gem();
    }
    /// <summary>
    /// 显示宝石
    /// </summary>
    private void Show_Gem()
    {
        select_gem_btn_item_crt = null;
        ClearObject(M_gem_btn_brom);
        string[] info = crt_bag.user_value.Split(' ');
        if (info.Length >= 6)
        {
            //宝石
            List<string> gem = ArrayHelper.Get_Split<string>(info[5], 'X');
            for (int i = 0; i < gem.Count; i++)
            {
                if (gem[i] != "")
                {
                    List<string> gem_value = ArrayHelper.Get_Split<string>(gem[i], '|');
                    if (gem_value.Count == 2)
                    {
                        if (gem_value[1] != "0")
                        {
                            Bag_Base_VO gem_data = ArrayHelper.Find(SumSave.db_stditems, x => x.Name == gem_value[1]);
                            if (gem_data != null)
                            {
                                select_gem_btn_item gem_btn = Instantiate(select_gem_btn_item_prefab, M_gem_btn_brom);
                                gem_btn.SetData(gem_data.Name,i);
                                gem_btn.GetComponent<Button>().onClick.AddListener(() => disassemble_Gem(gem_btn));
                            }
                        }
                        else
                        {
                            Bag_Base_VO gem_data = ArrayHelper.Find(SumSave.db_stditems, x => x.StdMode == "材料" && x.Shape == int.Parse(gem_value[0]));
                            if (gem_data != null)
                            {
                                select_gem_btn_item gem_btn = Instantiate(select_gem_btn_item_prefab, M_gem_btn_brom);
                                gem_btn.SetData(gem_data.Name,i);
                                gem_btn.GetComponent<Button>().onClick.AddListener(() => Select_Gem(gem_btn));
                            }
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// 拆卸宝石
    /// </summary>
    /// <param name="i"></param>
    /// <param name="gem_btn"></param>
    private void disassemble_Gem(select_gem_btn_item gem_btn)
    {
        if (select_gem_btn_item_crt != null)
        { 
            select_gem_btn_item_crt.Select_State = false;
        }
        select_gem_btn_item_crt = gem_btn;
        select_gem_btn_item_crt.Select_State = true;
        index = gem_btn.index;
        Alert.Show("拆卸宝石","将当前宝石取下,\n需要"+ currency_unit.金币+"500w", Disassemble_Gem);
    }

    private void Disassemble_Gem(object arg0)
    {
        Need_Condition(currency_unit.金币, 5000000);
        if (Return_Condition())
        {
            string[] info = crt_bag.user_value.Split(' ');
            if (info.Length >= 6)
            {
                List<string> gems = ArrayHelper.Get_Split<string>(info[5], 'X');
                if (gems.Count > index)
                {
                    if (gems[index] != "")
                    {
                        List<string> gem_value = ArrayHelper.Get_Split<string>(gems[index], '|');
                        if (gem_value.Count == 2)
                        {
                            if (gem_value[1] != "0")
                            {
                                gems[index] = gem_value[0] + "|0";
                                List<string> bag_gems = SumSave.crt_bags.Get_Gem_Value;
                                info[5] = string.Join("X", gems.ToArray());
                                crt_bag.user_value = Battle_Tool.Equip_User_Value(info);
                                bag_gems.Add(gem_value[1]);
                                SumSave.crt_bags.Set_Gem_Value(bag_gems);
                                SumSave.crt_bags.MysqlData();
                                refresh_gem();
                                Alert_Dec.Show("宝石拆卸成功"); 
                                Show(crt_bag);
                            }
                        }
                    }

                }
            }

        }
    }

    /// <summary>
    /// 选中位置
    /// </summary>
    /// <param name="index"></param>
    /// <param name="gem_data"></param>
    private void Select_Gem( select_gem_btn_item gem_data)
    {
        if (select_gem_btn_item_crt != null)
        {
            select_gem_btn_item_crt.Select_State = false;
        }
        select_gem_btn_item_crt = gem_data;
        select_gem_btn_item_crt.Select_State = true;
        this.index = gem_data.index;
    }

    private void Init()
    {
        ClearObject(M_bags_brom);
        List<string> gems = SumSave.crt_bags.Get_Gem_Value;
        for (int i = 0; i < gems.Count; i++)
        {
            material_item bagItem = Instantiate(material_item_prefab, M_bags_brom);
            bagItem.Init((gems[i], 1));
            bagItem.GetComponent<Button>().onClick.AddListener(() => SelectBagResourcesItem(bagItem));
        }
    }
    /// <summary>
    /// 选择宝石
    /// </summary>
    /// <param name="bagItem"></param>
    private void SelectBagResourcesItem(material_item bagItem)
    {
        if (select_gem_btn_item_crt == null) { Alert_Dec.Show("请选择位置");return; }
        string gem = bagItem.GetItemData().Item1;
        string[] info = crt_bag.user_value.Split(' ');
        if (info.Length >= 6)
        { 
            List<string> gems = ArrayHelper.Get_Split<string>(info[5], 'X');
            if (gems.Count > index)
            {
                if (gems[index] != "")
                {
                    List<string> gem_value = ArrayHelper.Get_Split<string>(gems[index], '|');
                    if (gem_value.Count == 2)
                    {
                        if (gem_value[1] == "0")
                        {
                            gems[index] = gem_value[0] + "|" + gem;
                            List<string> bag_gems = SumSave.crt_bags.Get_Gem_Value;
                            info[5]= string.Join("X", gems.ToArray());
                            crt_bag.user_value = Battle_Tool.Equip_User_Value(info);
                            bag_gems.Remove(gem);
                            SumSave.crt_bags.Set_Gem_Value(bag_gems);
                            SumSave.crt_bags.MysqlData();
                            Alert_Dec.Show("宝石镶嵌成功");
                            refresh_gem();
                            Show(crt_bag);
                        }
                    }
                }

            }
        }
    }

    private void refresh_gem()
    {
        transform.parent.SendMessage("refresh_gem",crt_bag);
    }
}
