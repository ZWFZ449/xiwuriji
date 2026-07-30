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

public class TowerBabel_map : offect_TowerBabel
{
    enum map_type
    { 
    挑战,
    重置,
    扫荡,
    }
    private Transform btn_brom,map_btn_brom;

    private Text info;

    private Text btn_title_name;

    private btn_item btn_item_prefab;

    private TMP_Text map_info;

    private Image bg_offects, bg_show_map; 

    List<map_type> btn_list = new List<map_type>();
    private Dictionary<btn_item, db_map_vo> base_map_item_dic = new Dictionary<btn_item, db_map_vo>();

    private Button close_bg;

    private PanelBattle panelBattle;

    private btn_item select_btn;
    private void Awake()
    {
        info = Find<Text>("title_name/info");
        panelBattle = UI_Manager.I.GetPanel<PanelBattle>();
        btn_title_name = Find<Text>("offect/show_map/title_name/info");
        bg_offects = Find<Image>("offect");
        bg_show_map = Find<Image>("offect/show_map");
        btn_item_prefab = Tool_UI.Find_Prefabs<btn_item>("btn_item");
        btn_brom = Find<Transform>("Scroll View/Viewport/Content");
        map_btn_brom = Find<Transform>("offect/show_map/btn_list");
        map_info = Find<TMP_Text>("offect/show_map/Text (TMP)");
        close_bg= Find<Button>("offect/close_button");
        close_bg.onClick.AddListener(() => { bg_offects.gameObject.SetActive(false); });
        Init();
    }

    private void Init()
    {
        ClearObject(btn_brom);
        for (int i = 0; i < SumSave.db_maps.Count; i++)
        {
            if (SumSave.db_maps[i].map_type == 10)
            {
                btn_item btn_item = Instantiate(btn_item_prefab, btn_brom);
                btn_item.Show(i, SumSave.db_maps[i].map_name);
                btn_item.GetComponent<Button>().onClick.AddListener(() => SelectBtn(btn_item));
                base_map_item_dic.Add(btn_item, SumSave.db_maps[i]);
            }
        }
    }

    private void SelectBtn(btn_item btn_item)
    {
        string dec = "";
        db_map_vo map = base_map_item_dic[btn_item];
        if (map != null)
        {
            bg_show_map.gameObject.SetActive(true);
            select_btn = btn_item;
            btn_title_name.text = map.map_name;
            btn_list = new List<map_type>();
            bg_offects.gameObject.SetActive(true);
            if (map.map_lv != -1)
            {
                dec = "当前击杀数量" + Random.Range(0, 10000);
                btn_list.Add(map_type.挑战);
                btn_list.Add(map_type.重置);
                //btn_list.Add(map_type.扫荡);

            }
            else
            { 
                dec = "未开启";
            }
            Init_Btn();
            map_info.text = dec;
        }
      
    }

    private void Init_Btn()
    {
        ClearObject(map_btn_brom);
        for (int i = 0; i < btn_list.Count; i++)
        {
            btn_item btn_item = Instantiate(btn_item_prefab, map_btn_brom);
            btn_item.Show(i, btn_list[i]);
            btn_item.GetComponent<Button>().onClick.AddListener(() => SelectMapBtn(btn_item));
        }
    }

    private void SelectMapBtn(btn_item btn_item)
    {
        switch ((map_type)btn_item.index)
        {
            case map_type.挑战:
                Confirm(1);
                break;
            case map_type.重置:
                Alert.Show("是否重置", "重置后将直接开始从0挑战,请确认是否从0开始进行挑战", Confirm,0);
                break;
            case map_type.扫荡:
                break;
        }
    }

    private void Confirm(object arg0)
    {
        panelBattle.Show();
        panelBattle.GoMaxMap(base_map_item_dic[select_btn], 0);
        bg_offects.gameObject.SetActive(false);
    }

    public  void InitShow()
    {
        info.text = TowerType.通天之路 + "";
    }
}
