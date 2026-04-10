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

public enum Panel_BagType
{ 
    装备,
    灵宠,
    宝石,
    材料,
    存入仓库,
    取出仓库,
    一键出售,
    已装备,//不显示内容
    展示,//不显示内容
}
/// <summary>
/// 背包面板
/// </summary>
public class Dream_Panel_Bag : Panel_Base
{
    private Transform m_BagItem_brom, m_btn_brom;

    private btn_item p_btn_item_prefab;

    private dream_BagItem p_bagItem_prefab;

    private material_item p_material_item_prefab;
    /// <summary>
    /// 当前背包类型
    /// </summary>
    private Panel_BagType m_curPanel_BagType = Panel_BagType.装备;

    private panel_hero_equip m_panel_hero_equip;

    private Dream_Panel_Pet m_panel_pet;

    private Text base_info;
    public override void Hide()
    {
        base.Hide();
    }

    public override void Initialize()
    {
        base.Initialize();
        m_BagItem_brom =Find<Transform>("bg/baglist/Viewport/Content");
        m_btn_brom = Find<Transform>("bg/btnlist/Viewport/Content");
        p_btn_item_prefab = Tool_UI.Find_Prefabs<btn_item>("btn_item");
        p_bagItem_prefab = Tool_UI.Find_Prefabs<dream_BagItem>("dream_BagItem");
        p_material_item_prefab = Tool_UI.Find_Prefabs<material_item>("material_item");
        base_info = Find<Text>("bg/base_info");
        ClearObject(m_BagItem_brom);
        ClearObject(m_btn_brom);
        for (int i = 0; i < Enum.GetNames(typeof(Panel_BagType)).Length-2; i++)
        {
            btn_item btn_item = Instantiate(p_btn_item_prefab, m_btn_brom);
            btn_item.Show(i, (Panel_BagType)i);
            btn_item.GetComponent<Button>().onClick.AddListener(() => SelectBtn(btn_item));
        }
        m_panel_hero_equip = UI_Manager.I.GetPanel<panel_hero_equip>();
        m_panel_pet = UI_Manager.I.GetPanel<Dream_Panel_Pet>();

    }
    /// <summary>
    /// 选择功能按钮
    /// </summary>
    /// <param name="btn_item"></param>
    private void SelectBtn(btn_item btn_item)
    {
        switch ((Panel_BagType)btn_item.index)
        {
            case Panel_BagType.装备:
            case Panel_BagType.材料:
            case Panel_BagType.存入仓库:
            case Panel_BagType.灵宠:
            case Panel_BagType.宝石:
                m_curPanel_BagType = (Panel_BagType)btn_item.index;
                baseShow();
                break;
            case Panel_BagType.取出仓库:
                m_curPanel_BagType = Panel_BagType.取出仓库;
                //打开仓库界面
                baseShow();
                break;
            case Panel_BagType.一键出售:
                m_curPanel_BagType = Panel_BagType.装备;
                OneClickSell();
                break;

            case Panel_BagType.已装备:
                break;
            case Panel_BagType.展示:
                break;
        }
    }
    /// <summary>
    /// 一键出售
    /// </summary>
    private void OneClickSell()
    {
        List<Bag_Base_VO> baglist = SumSave.crt_bags.Get_Bag_List();
        int moeny = 0;//回收金币
        int sycee = 0;//回收元宝
        for (int i = 0; i < baglist.Count; i++)
        {
            string[] info_str = baglist[i].user_value.Split(' ');
            int lv = int.Parse(info_str[2]);
            int islock = int.Parse(info_str[3]);
            if (islock == 0)
            {
                moeny += baglist[i].price * lv / Enum.GetValues(typeof(enum_equip_quality_list)).Cast<int>().Max();
                if (lv >= 5) sycee += baglist[i].need_lv / 7 * (lv - 5) + 1;
                baglist.RemoveAt(i);
                i--;
            }
        }
        if (moeny > 0)
        {
            Battle_Tool.Dream_Obtain_Unit(currency_unit.金币, moeny, Obtain_Int.Add_unit(moeny));
            if (sycee > 0)
                Battle_Tool.Dream_Obtain_Unit(currency_unit.元宝, sycee, Obtain_Int.Add_unit(sycee));
            SumSave.crt_bags.Set_Bag_List(baglist);
            Alert.Show("一键出售", "出售成功,\n获得 " + currency_unit.金币 + " * " + moeny + "\n" + currency_unit.元宝 + " * " + sycee);

            baseShow();
        }
    }

    public override void Show()
    {
        base.Show();
        baseShow();
    }
    /// <summary>
    /// 显示背包
    /// </summary>
    private void baseShow()
    {
        ClearObject(m_BagItem_brom);
        switch (m_curPanel_BagType)
        { case Panel_BagType.灵宠:
                List<db_pet_vo> pet_list = SumSave.crt_pet.GetPets;
                base_info.text = m_curPanel_BagType + " " + pet_list.Count + "/10";
                for (int i = 0; i < pet_list.Count; i++)
                { 
                    dream_BagItem bagItem = Instantiate(p_bagItem_prefab, m_BagItem_brom);
                    bagItem.Pet_Data= pet_list[i];
                    bagItem.GetComponent<Button>().onClick.AddListener(() => SelectPetItem(bagItem));
                }
                break;
           case Panel_BagType.装备:
           case Panel_BagType.存入仓库:
                List<Bag_Base_VO> baglist = SumSave.crt_bags.Get_Bag_List();
                base_info.text = m_curPanel_BagType + " " + baglist.Count + "/120";
                for (int i = 0; i < baglist.Count; i++)
                { 
                    dream_BagItem bagItem = Instantiate(p_bagItem_prefab, m_BagItem_brom);
                    bagItem.Data= baglist[i];
                    bagItem.GetComponent<Button>().onClick.AddListener(() => SelectBagItem(bagItem));
                }
                break;
            case Panel_BagType.材料:
                List<(string, int)> resources_list = SumSave.crt_bags.Set();
                base_info.text = m_curPanel_BagType + " " + resources_list.Count + "/120";
                for (int i = 0; i < resources_list.Count; i++)
                { 
                    material_item bagItem = Instantiate(p_material_item_prefab, m_BagItem_brom);
                    bagItem.Init(resources_list[i]);
                    bagItem.GetComponent<Button>().onClick.AddListener(() => SelectBagResourcesItem(bagItem));
                }
                break;
        }
    }

    private void SelectPetItem(dream_BagItem bagItem)
    {
        m_panel_pet.Show();
        m_panel_pet.InitPet(bagItem, Panel_BagType.装备);
    }

    private void TemoInitBag()
    {
        for (int i = 0; i < 10; i++)
        {
            SumSave.crt_bags.Set_Bag_List(Init_Bag());
        }
    }

    /// <summary>
    /// 选择背包物品
    /// </summary>
    /// <param name="bagItem"></param>
    private void SelectBagResourcesItem(material_item bagItem)
    {

        m_panel_hero_equip.Show();
        m_panel_hero_equip.Select_Resources(bagItem, m_curPanel_BagType);
    }
    /// <summary>
    /// 选择背包药品
    /// </summary>
    /// <param name="bagItem"></param>
    private void SelectBagdrugItem(material_item bagItem)
    {

        m_panel_hero_equip.Show();
        m_panel_hero_equip.Select_Resources(bagItem, m_curPanel_BagType);
    }


    private Bag_Base_VO Init_Bag()
    {
        Bag_Base_VO data = SumSave.db_stditems[Random.Range(0, SumSave.db_stditems.Count)];
        
        while (data.StdMode != equip_type_list.武器.ToString())
        {
            data = SumSave.db_stditems[Random.Range(0, SumSave.db_stditems.Count)];
        }
        data.user_value = Tool_Battle.Obtain_Equip(data, 1, Random.Range(1,7));
        data = tool_Categoryt.Read_Bag(data.user_value);
        return data;

    }


    private void SelectBagItem(dream_BagItem bagItem)
    {
        m_panel_hero_equip.Show();
        m_panel_hero_equip.Select_Bag(bagItem, m_curPanel_BagType);
    }

    protected override void Awake()
    {
        base.Awake();
    }
}
