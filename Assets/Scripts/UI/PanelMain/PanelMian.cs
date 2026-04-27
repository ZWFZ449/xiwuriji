using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PanelMian : PanelBase
{
    private Transform pos_equip, posLevelequip,pos_btn_main, m_global_info_brom;
    /// <summary>
    /// 装备类型
    /// </summary>
    private equipuiItem equipuiItemprefab;
    /// <summary>
    /// 装备类型数据存储
    /// </summary>
    private Dictionary<equip_type_list, equipuiItem> keyValuePairs = new Dictionary<equip_type_list, equipuiItem>();
    /// <summary>
    /// 英雄图片
    /// </summary>
    private Image hero_img, hero_icon;
    /// <summary>
    ///  大厅
    /// </summary>
    private PanelHall panelHall;

    private panel_hero_equip panel_hero_equip;
    /// <summary>
    /// 经验条
    /// </summary>
    private CircularHealthBar expBar;

    private btn_item btnitem_prefab;

    private List<string> btn_names = new List<string>() { "大厅", "战斗" };

    /// <summary>
    /// 记录日志
    /// </summary>
    private global_info_item global_info_item_prefab;
    /// <summary>
    /// 循环日志
    /// </summary>
    private List<global_info_item> dec_items = new List<global_info_item>();
    /// <summary>
    /// 金币 元宝 积分
    /// </summary>
    private TMP_Text moeny, sycee, points, hero_name;
    /// <summary>
    /// 改名
    /// </summary>
    private Button btn_name;
    /// <summary>
    /// 改名预制件
    /// </summary>
    private Transform m_input_brom;

    private input_offect input_offect_prefab, crt_input_offect;


    protected override void Awake()
    {
        base.Awake();
    }
    public override void Initialize()
    {
        base.Initialize();
        string path = "Scroll View/Viewport/Content/";
        pos_equip =Find<Transform>(path+"hero_equips/equips");
        posLevelequip = Find<Transform>(path + "hero_equips/equips/level_equips");
        equipuiItemprefab = Tool_UI.Find_Prefabs<equipuiItem>("equipuiItem");
        btnitem_prefab = Tool_UI.Find_Prefabs<btn_item>("btn_item");
        for (int i = 0; i < Enum.GetNames(typeof(equip_type_list)).Length; i++)
        {
            equipuiItem equipuiItem = Instantiate(equipuiItemprefab, i >= 12 ? posLevelequip : pos_equip);
            equipuiItem.Insance_Crate((equip_type_list)(i));
            keyValuePairs.Add((equip_type_list)(i), equipuiItem);
        }
        expBar = Find<CircularHealthBar>(path + "btn_list/show_hero/exp");
        hero_img= Find<Image>(path + "btn_list/show_hero/hero_img/icon");
        hero_icon = Find<Image>(path + "hero_equips/hero_icon/icon");
        panelHall = UI_Manager.I.GetPanel<PanelHall>();
        panel_hero_equip= UI_Manager.I.GetPanel<panel_hero_equip>();
        pos_btn_main = Find<Transform>(path + "main_list");
        for (int i = 0; i < btn_names.Count; i++)
        {
            btn_item btn_item = Instantiate(btnitem_prefab, pos_btn_main);
            btn_item.Show(i, btn_names[i]);
            btn_item.GetComponent<Button>().onClick.AddListener(() => SelectBtn(btn_item));
        }
        m_global_info_brom = Find<Transform>(path + "special_list/Scroll View/Viewport/Content");
        global_info_item_prefab = Tool_UI.Find_Prefabs<global_info_item>("global_info_item");
        moeny = Find<TMP_Text>(path + "monitor_info/show_unit/moeny/info/info");
        sycee = Find<TMP_Text>(path + "monitor_info/show_unit/sycee/info/info");
        points = Find<TMP_Text>(path + "monitor_info/show_unit/points/info/info");
        hero_name = Find<TMP_Text>(path + "hero_equips/hero_name/info/info");
        btn_name = Find<Button>(path + "hero_equips/hero_name");
        btn_name.onClick.AddListener(() => { show_name(); });
        m_input_brom = GetComponent<Transform>();
        input_offect_prefab = Tool_UI.Find_Prefabs<input_offect>("input_offect");
        for (int i = 0; i < 110; i++)
        {
            global_info_item global_info_item = Instantiate(global_info_item_prefab, m_global_info_brom);
            global_info_item.gameObject.SetActive(false);
            dec_items.Add(global_info_item);
        }
    }

    private void show_name()
    {
        if (SumSave.crtHero.hero_name.Contains("[可改名]"))
        {
            if (crt_input_offect == null)
            {
                crt_input_offect = Instantiate(input_offect_prefab, m_input_brom);
                crt_input_offect.GetConfirm.onClick.AddListener(() => { confirm(); });
            }
            crt_input_offect.gameObject.SetActive(true);
            crt_input_offect.Init("重命名", "对" + SumSave.crtHero.hero_name + "进行调整");
        }
        else Alert_Dec.Show("每个角色只可以改名一次");

    }

    private void confirm()
    {
        if (crt_input_offect.GetInput != "")
        {
            Alert.Show("改名", "确定修改名称" + crt_input_offect.GetInput + Show_Color.Red("\n只可以改名一次"), rename_confirm);
        }
        else Alert_Dec.Show("请输入不含特殊符号的内容");
    }
    /// <summary>
    /// 改名确认
    /// </summary>
    /// <param name="arg0"></param>
    private void rename_confirm(object arg0)
    {
        SumSave.crtHero.hero_name = crt_input_offect.GetInput;
        SumSave.crtHero.MysqlData();
        SendNotification(NotiList.Refresh_Max_Hero_Attribute);
        Show_hero();
        crt_input_offect.gameObject.SetActive(false);
        Alert_Dec.Show("修改成功");
    }

    /// <summary>
    /// 选择按钮
    /// </summary>
    /// 
    /// <param name="btn_item"></param>
    private void SelectBtn(btn_item btn_item)
    {
        switch (btn_names[btn_item.index])
        { 
            case "大厅":
                panelHall.Show();
                //panel_hero_equip.Show();
                break;
            case "战斗":
                UI_Manager.I.GetPanel<PanelMap>().Show();
                break;
        }
    }

    public override void Show()
    {
        base.Show();
        Base_Show();
        Obtain_Info_list();
    }
    /// <summary>
    /// 获取日志
    /// </summary>
    private void Obtain_Info_list()
    {
        SendNotification(NotiList.read_Obtain_Info);
    }

    public void Show_Golbal_Info_list()
    {
        foreach (var item in dec_items)
        {
            item.gameObject.SetActive(false);
        }
        for (int i = SumSave.global_battle_info.Count; i > 0; i--)
        {
            global_info_item global_info_item = dec_items[0];
            global_info_item.SetInfo(SumSave.global_battle_info[i-1]);
            global_info_item.GetComponent<Button>().onClick.AddListener(() => { Show_Info(global_info_item); });
            global_info_item.gameObject.SetActive(true);
            global_info_item.transform.SetAsLastSibling();
            dec_items.RemoveAt(0);
            dec_items.Add(global_info_item);
        }
    
    }
    /// <summary>
    /// 显示内容
    /// </summary>
    /// <param name="global_info_item"></param>
    private void Show_Info(global_info_item global_info_item)
    {
        List<string> info = ArrayHelper.Get_Split<string>(global_info_item.data.value, ';');
        if (info.Count == 2)
        {
            Bag_Base_VO bag_base_VO = tool_Categoryt.Read_BaseBag(info[1]);
            if (bag_base_VO != null)
            {
                panel_hero_equip.Show();
                dream_BagItem bagItem= new dream_BagItem();
                bagItem.Data = bag_base_VO;
                panel_hero_equip.Select_Bag(bagItem, Panel_BagType.展示);
            }
        }
    }
    private void Base_Show()
    {
        show_equip();
        Show_hero();
        Show_unit();
    }

    public void Show_unit()
    {
        List<long> Units= SumSave.crt_user_unit.Set();
        moeny.text = currency_unit.金币 + " " + Battle_Tool.FormatNumberToChineseUnit(Units[(int)currency_unit.金币]);
        sycee.text = currency_unit.元宝 + " " + Battle_Tool.FormatNumberToChineseUnit(Units[(int)currency_unit.元宝]);
        points.text = "荣耀积分"+ " " + SumSave.crt_global_gift.GetGiftPoints; 
    }

    private void Show_hero()
    {
        hero_name.text = SumSave.crtMaxBattle.crt_name +" "+ Battle_Tool.Obtain_Talent_Name()+ " Lv." + SumSave.crtMaxBattle.lv;
        hero_img.sprite = UI.UI_Manager.I.GetEquipSprite("UI/player/", SumSave.crtMaxBattle.hero_type+"头像");
        hero_icon.sprite = UI.UI_Manager.I.GetEquipSprite("UI/player/", SumSave.crtMaxBattle.hero_type);
    }

    /// <summary>
    /// 显示装备
    /// </summary>
    private void show_equip()
    {
        List<Bag_Base_VO> crt_equips = SumSave.crt_equips.Get(Dream_User_Equip_Type.装备);
        int pantsindex = -1, rindindex = -1;
        foreach (equip_type_list item in keyValuePairs.Keys)
        {
            keyValuePairs[item].Initialize();
            if (item == equip_type_list.灵宠)
            {
                keyValuePairs[item].Pet_Data = SumSave.crt_pet.GetPet;
                continue;
            }
            for (int i= 0; i < crt_equips.Count; i++) 
            {
                if (crt_equips[i].StdMode == Stditem_StdMode_List.戒指.ToString())
                {
                    if (item == equip_type_list.左戒|| item == equip_type_list.右戒)
                    {
                        if (rindindex == -1)
                        {
                            keyValuePairs[item].Data = crt_equips[i];
                            rindindex = i;
                            break ;
                        }
                        else
                        {
                            if (rindindex != i)
                            { 
                                keyValuePairs[item].Data = crt_equips[i];
                                break;
                            }
                        }
                    }
                }
                else
                if (crt_equips[i].StdMode == Stditem_StdMode_List.手镯.ToString())
                {
                    if (item == equip_type_list.左手|| item == equip_type_list.右手)
                    {
                        if (pantsindex == -1)
                        {
                            keyValuePairs[item].Data = crt_equips[i];
                            pantsindex = i;
                            break;
                        }
                        else
                        {
                            if (pantsindex != i)
                            {
                                keyValuePairs[item].Data = crt_equips[i];
                                break;
                            }
                        }
                    }
                }
                else
                if (item.ToString() == crt_equips[i].StdMode)
                {
                    keyValuePairs[item].Data = crt_equips[i];
                    continue;
                }
            }
        }
    }
    private void Update()
    {
    }
}
