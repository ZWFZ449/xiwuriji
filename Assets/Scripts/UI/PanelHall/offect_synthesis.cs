using Common;
using Components;
using MVC;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class offect_synthesis : Base_Mono
{

    private Transform m_btn_brom, m_synthesis_brom;

    private btn_item btn_item_prefab;

    private synthesis_item synthesis_item_prefab;

    private Text info;

    private btn_item crt_btn_item;

    private Dictionary<string,List<db_synthesis_vo>> dic_synthesis = new Dictionary<string, List<db_synthesis_vo>>();
    /// <summary>
    /// 合成材料
    /// </summary>
    private List<string> needs = new List<string>();
    /// <summary>
    /// 宝石合成材料
    /// </summary>
    private List<string> gem_needs = new List<string>();
    private void Awake()
    {
        m_btn_brom=Find<Transform>("btn_list/Viewport/Content");
        m_synthesis_brom = Find<Transform>("synthesis_list/Viewport/Content");
        btn_item_prefab = Tool_UI.Find_Prefabs<btn_item>("btn_item");
        synthesis_item_prefab = Tool_UI.Find_Prefabs<synthesis_item>("synthesis_item");
        init();
    }

    private void OnEnable()
    {
        if (SumSave.crtHero.lv <= 10)  
        { 
            Alert_Dec.Show("合成功能在10级开放");
            Hide();
        }
    }
    private void init()
    {
        for (int i = 0; i < SumSave.db_synthesis.Count; i++)
        {
            if (!dic_synthesis.ContainsKey(SumSave.db_synthesis[i].synthesis_type))
            {
                dic_synthesis.Add(SumSave.db_synthesis[i].synthesis_type, new List<db_synthesis_vo>());
            }
            dic_synthesis[SumSave.db_synthesis[i].synthesis_type].Add(SumSave.db_synthesis[i]);
        }
        bool isShow = true;
        foreach (var item in dic_synthesis)
        { 
            btn_item btn_item = Instantiate(btn_item_prefab, m_btn_brom);
            btn_item.Show(1,item.Key);
            btn_item.GetComponent<Button>().onClick.AddListener(() => SelectBtn(item.Key));
            if (isShow)
            { 
                SelectBtn(item.Key);
                isShow = false;
            }
        }
    }
    /// <summary>
    /// 合成功能
    /// </summary>
    /// <param name="key"></param>
    private void SelectBtn(string key)
    {
        ClearObject(m_synthesis_brom);
        if (dic_synthesis.ContainsKey(key))
        {
            for (int i = 0; i < dic_synthesis[key].Count; i++)
            {
                synthesis_item synthesis_item = Instantiate(synthesis_item_prefab, m_synthesis_brom);
                synthesis_item.Data = dic_synthesis[key][i];
                synthesis_item.GetComponent<Button>().onClick.AddListener(() => Synthesis(synthesis_item));
            }
        }
    }
    /// <summary>
    /// 合成
    /// </summary>
    /// <param name="db_synthesis_vo"></param>
    private void Synthesis(synthesis_item item)
    {
        db_synthesis_vo needlists = item.Data;
        needs.Clear();
        gem_needs.Clear();
        Clear_Condition();
        string dec = "合成" + needlists.synthesis_name;
        List<string> needlist = ArrayHelper.Get_Split<string>(needlists.synthesis_need,',');
        for (int i = 0; i < needlist.Count; i++)
        { 
            List<string> need = ArrayHelper.Get_Split<string>(needlist[i], ' ');
            if (need.Count == 3)
            {
                switch (need[0])
                {
                    case "1":
                        dec += "\n" + (currency_unit)(int.Parse(need[1])) + " * " + Battle_Tool.FormatNumberToChineseUnit(int.Parse(need[2]));
                        Need_Condition((currency_unit)(int.Parse(need[1])), int.Parse(need[2]));
                        break;
                    case "2":
                        dec += "\n" + need[1] + " * " + need[2];
                        Need_Condition(need[1], int.Parse(need[2]));
                        break;
                    case "3":
                        dec += "\n" + need[2] + " * " + 1;
                        needs.Add(need[2]);
                        break;
                    case "4":
                        dec += "\n" + need[2] + " * " + 1;
                        gem_needs.Add(need[2]);
                            break;
                }
            }
        }

        Alert.Show("合成", dec, confirm,needlists);
    }
    /// <summary>
    /// 合成
    /// </summary>
    /// <param name="arg0"></param>
    private void confirm(object arg0)
    {
        db_synthesis_vo data = (db_synthesis_vo)arg0;
        if (data.synthesis_type.Contains("宝石"))
        {
            List<string> gems = SumSave.crt_bags.Get_Gem_Value;
            int number = 0;
            for (int i = 0; i < gem_needs.Count; i++)
            {
                for (int j = 0; j < gems.Count; j++)
                {
                    if (gem_needs[i] == gems[j])
                    { 
                        number++;
                        break;
                    }
                }
            }
            if (number >= gem_needs.Count)
            {
                if (Return_Condition())
                {
                    for (int i = 0; i < gem_needs.Count; i++)
                    { 
                        gems.Remove(gem_needs[i]);
                    }
                    gems.Add(data.synthesis_name);
                    SumSave.crt_bags.Set_Gem_Value(gems);
                    Alert_Dec.Show("合成成功 " + " 获得 " + data.synthesis_name);
                } else Alert_Dec.Show("合成失败,材料不足");
            }
        }
        else
        {
            List<Bag_Base_VO> equips = SumSave.crt_equips.Get(Dream_User_Equip_Type.装备);
            List<Bag_Base_VO> bags = new List<Bag_Base_VO>();
            int number = 0;
            for (int i = 0; i < needs.Count; i++)
            {
                for (int j = 0; j < equips.Count; j++)
                {
                    if (needs[i] == equips[j].Name)
                    {
                        number++;
                        bags.Add(equips[j]);
                    }
                }
            }
            if (number >= needs.Count)
            {
                if (Return_Condition())
                {
                    for (int i = 0; i < bags.Count; i++) equips.Remove(bags[i]);
                    SumSave.crt_equips.Set(Dream_User_Equip_Type.装备, equips);
                    SendNotification(NotiList.Refresh_Max_Hero_Attribute);
                    Alert_Dec.Show("合成成功 " + " 获得 " + data.synthesis_name);
                    Bag_Base_VO synthesis_value = ArrayHelper.Find(SumSave.db_stditems, e => e.Name == data.synthesis_name);
                    if (synthesis_value != null)
                    {
                        string user_value = Tool_Battle.Obtain_Equip(synthesis_value, 1, 1);
                        Bag_Base_VO synthesis = tool_Categoryt.Read_BaseBag(user_value);
                        SumSave.crt_bags.Set_Bag_List(synthesis);
                    }
                    UI_Manager.Instance.GetPanel<PanelMian>().Show();
                }
                else Alert_Dec.Show("合成失败,材料不足");
            }
            else Alert_Dec.Show("合成失败,材料不足");
        }
       
        

    }
    private void Hide()
    {
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }

}
