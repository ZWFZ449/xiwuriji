using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVC;
using UnityEngine.UI;
using System;
using Common;
using UI;
using Components;

public enum Map_Btn_list
{ 
   野外地图,
   个人Boss,
   每日副本,
   中州秘境,
   手动存档,
   返回战斗
}
public class PanelMap : PanelBase
{

    private PanelBattle panelBattle;

    private Transform m_btn_type_borm, m_function_offect_base_map_borm;

    private btn_item btn_item_prefab;

    private Image function_offect, function_offect_base_map;

    private Button close_function_offect;

    private base_map_item base_map_item_prefab;

    private Dictionary<string,base_map_item> base_map_item_dic = new Dictionary<string, base_map_item>();
    /// <summary>
    /// 显示地图信息
    /// </summary>
    private map_show_offect _map_show_offect;

    private bool open_return_battle = false;//是否可以返回战斗
    public override void ClearObject(Transform pos_btn,int cout=0)
    {
        base.ClearObject(pos_btn, cout);
    }

    public override void Hide()
    {
        if(_map_show_offect.gameObject.activeInHierarchy)_map_show_offect.gameObject.SetActive(false);
        else
        base.Hide();
    }

    public override void Initialize()
    {
        base.Initialize();
        panelBattle = UI_Manager.I.GetPanel<PanelBattle>();
        m_btn_type_borm =Find<Transform>("btn_lists");
        btn_item_prefab= Tool_UI.Find_Prefabs<btn_item>("btn_item");
        for (int i = 0; i < Enum.GetNames(typeof(Map_Btn_list)).Length; i++)
        {
            btn_item btn_item = Instantiate(btn_item_prefab, m_btn_type_borm);
            btn_item.Show(i, (Map_Btn_list)i);
            btn_item.GetComponent<Button>().onClick.AddListener(()=> { OnClickBtn(btn_item); });
        }
        close_function_offect=Find<Button>("function_offect/close");
        close_function_offect.onClick.AddListener(()=> { Open_function_offect(); });
        function_offect=Find<Image>("function_offect");
        function_offect_base_map = Find<Image>("function_offect/function_offect_base_map");
        base_map_item_prefab= Tool_UI.Find_Prefabs<base_map_item>("base_map_item");
        m_function_offect_base_map_borm=Find<Transform>("function_offect/function_offect_base_map/Scroll View/Viewport/Content");
        _map_show_offect = Find<map_show_offect>("function_offect/map_show_offect");
        Init_function_offect();
    }
    /// <summary>
    /// 初始化功能开关
    /// </summary>
    private void Init_function_offect()
    {
        function_offect.gameObject.SetActive(true);

        for (int i = 0; i < SumSave.db_maps.Count; i++)
        {
            base_map_item base_map_item = Instantiate(base_map_item_prefab, m_function_offect_base_map_borm);
            base_map_item.Init(SumSave.db_maps[i]);
            base_map_item_dic.Add(SumSave.db_maps[i].map_name, base_map_item);
            base_map_item.GetComponent<Button>().onClick.AddListener(() => { OnClickBaseMap(base_map_item); });
        }
        function_offect.gameObject.SetActive(false);
    }
    /// <summary>
    /// 点击查看地图信息
    /// </summary>
    /// <param name="base_map_item"></param>
    protected void OnClickBaseMap(base_map_item base_map_item)
    {
        _map_show_offect.gameObject.SetActive(true);
        _map_show_offect.Init(base_map_item);
    }
    /// <summary>
    /// 进入地图
    /// </summary>
    /// <param name="base_map_item"></param>
    protected void OnClickEnterMap(base_map_item base_map_item)
    {
        open_return_battle = true;
        panelBattle.Show();
        panelBattle.GoMap(base_map_item.GetMap(),base_map_item.GetMap_Intensity);
    }

    /// <summary>
    /// 关闭功能按钮
    /// </summary>
    private void Open_function_offect(bool exist=false)
    {
        function_offect.gameObject.SetActive(exist);
        function_offect_base_map.gameObject.SetActive(exist);
    }

    /// <summary>
    /// 点击按钮
    /// </summary>
    /// <param name="btn_item"></param>
    private void OnClickBtn(btn_item btn_item)
    {
        switch ((Map_Btn_list)btn_item.index)
        {
            case Map_Btn_list.野外地图:
            case Map_Btn_list.中州秘境:
            case Map_Btn_list.每日副本:
            case Map_Btn_list.个人Boss:
                switch ((Map_Btn_list)btn_item.index)
                {
                    case Map_Btn_list.野外地图:
                        break;
                    case Map_Btn_list.每日副本:
                        if (SumSave.crtHero.lv < 20)
                        { 
                            Alert_Dec.Show("等级不足20级");
                            return;
                        }
                        break;
                    case Map_Btn_list.个人Boss:
                        if (SumSave.crtHero.lv < 20)
                        {
                            Alert_Dec.Show("等级不足20级");
                            return;
                        }
                        break;
                    case Map_Btn_list.中州秘境:
                        if (SumSave.crtHero.lv < 40)
                        {
                            Alert_Dec.Show("等级不足40级");
                            return;
                        }
                        break;
                }
                Open_function_offect(true);

                foreach (var item in base_map_item_dic.Values)
                {
                    item.gameObject.SetActive(satisfy_Need(item, btn_item.index));
                }
                break;
            case Map_Btn_list.返回战斗:
                if (open_return_battle)
                    panelBattle.Show();
                else Alert_Dec.Show("请先开始战斗");
                break;
            case Map_Btn_list.手动存档:
                Game_Omphalos.i.archive();
                break;
            default:
                break;
        }
    }
    private bool satisfy_Need(base_map_item item,int index)
    {
        bool exist = false;
        if (item.GetMap().map_type == index)
        {
            int lv = SumSave.crtHero.lv;
            if (SumSave.crtHero.zs_lvs > 1)
            {
                lv = (int)MathF.Max(60 + ((SumSave.crtHero.zs_lvs - 2) * 5), SumSave.crtHero.lv);
            }
            if (item.GetMap().map_lv <= lv || Tool_Battle.IsBuff(common_Buff.月卡))
            { 
                exist = true;
                return exist;
            }
        }
        return exist;
    }

    public override void Show()
    {
        base.Show();
    }

    protected override void Awake()
    {
        base.Awake();
    }
}
