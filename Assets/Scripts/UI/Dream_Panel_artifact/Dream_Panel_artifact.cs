using Common;
using Components;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;


public enum artifact_offect_list
{ 
生命,
魔法,
回血,
回蓝,
生命值,
魔法值,
命中,
闪避,
幸运,
技能等级上限,
鞭尸概率,
灵宠转生加成,
角色转生加成,
物攻,
防御,
免伤,
战系主动技能等级上限,
法系主动技能等级上限,
道士主动技能等级上限,
魔攻,
道术,
}
public class Dream_Panel_artifact : Panel_Base
{

    private enum artifact_btn_list
    { 
        填充,
        升级,
        激活,
    }
    private Transform m_proms, m_icon_proms,m_ptn_proms;

    private Image offect;

    private artifact_item artifact_item_prefab;

    private btn_item btn_item_prefab;

    private artifact_item crt_item;

    private TMP_Text info;

    private input_offect input_offect_prefab, crt_input_offect;
    protected override void Awake()
    {
        base.Awake();
    }

    public override void Initialize()
    {
        base.Initialize();

        offect= Find<Image>("bg/offect_list");

        m_icon_proms =Find<Transform>("bg/offect_list/icon");

        m_proms =Find<Transform>("bg/artifact_list/Scroll View/Viewport/Content");

        artifact_item_prefab = Tool_UI.Find_Prefabs<artifact_item>("artifact_item");

        m_ptn_proms = Find<Transform>("bg/offect_list/btn_list/Scroll View/Viewport/Content");

        btn_item_prefab= Tool_UI.Find_Prefabs<btn_item>("btn_item");

        info= Find<TMP_Text>("bg/offect_list/Scroll View/Viewport/Content/info");

        input_offect_prefab = Tool_UI.Find_Prefabs<input_offect>("input_offect");

        offect.gameObject.SetActive(false);
    }

    public override void Show()
    {
        base.Show();
        baseShow();
        //Hide();
    }

    public override void Hide()
    {
        if(offect.gameObject.activeSelf)offect.gameObject.SetActive(false);
        else
        base.Hide();
    }

    private void baseShow()
    {
        ClearObject(m_proms);
        offect.gameObject.SetActive(false);
        if (crt_input_offect != null) crt_input_offect.gameObject.SetActive(false);
        List<(int, int, long)> list = SumSave.crt_user_artifact.Get;
        for (int i = 0; i < SumSave.db_artifacts.Count; i++)
        {
            artifact_item item = Instantiate(artifact_item_prefab, m_proms);
            item.Initialize(SumSave.db_artifacts[i]);
            item.GetComponent<Button>().onClick.AddListener(() => { OnClickItem(item); });
            for (int j= 0; j < list.Count; j++)
            {
                if (list[j].Item1 == SumSave.db_artifacts[i].artifact_type)
                {
                    item.Set(list[j]);
                }
            }
        }

    }
    /// <summary>
    /// 点击物品
    /// </summary>
    /// <param name="item"></param>
    private void OnClickItem(artifact_item item)
    {
        if (item.crt_artifact.artifact_type > 4)
        {
            Alert_Dec.Show("该神器尚未激活");
            return;
        }
        offect.gameObject.SetActive(true);

        crt_item = item;

        ClearObject(m_icon_proms);
        //Instantiate(artifact_item_prefab, m_proms).Initialize(item.crt_artifact);
        string dec = item.crt_artifact.artifact_name + "Lv." + item.crt_Data.Item2 + "\n";
        //info.text = item.crt_artifact.artifact_name + "Lv." + item.crt_Data.Item2;
        List<string> list = ArrayHelper.Get_Split<string>(item.crt_artifact.artifact_offect, ',');

        for (int i = 0; i < list.Count; i++)
        {
            List<string> list2 = ArrayHelper.Get_Split<string>(list[i], ' ');
            if (list2.Count == 3)
            {
                int value = (((item.crt_Data.Item2 / int.Parse(list2[1])) + 1) * (int.Parse(list2[2])));
                if (item.crt_artifact.artifact_type == 3)
                {
                    if ((artifact_offect_list)(int.Parse(list2[0])) == artifact_offect_list.幸运)
                    {
                        value -= 1;
                    }
                }
                dec += (artifact_offect_list)(int.Parse(list2[0])) + ": " + value;

                switch ((artifact_offect_list)(int.Parse(list2[0])))
                {
                    case artifact_offect_list.生命:
                    case artifact_offect_list.魔法:
                        dec += " % \n";
                        break;
                    case artifact_offect_list.回血:
                    case artifact_offect_list.回蓝:
                        dec += " /s \n";

                        break;
                    case artifact_offect_list.命中:
                    case artifact_offect_list.闪避:
                        dec += "  \n";
                        break;
                    case artifact_offect_list.技能等级上限:
                        dec += "  \n";
                        break;
                    case artifact_offect_list.鞭尸概率:
                        dec += " % \n";
                        break;
                    case artifact_offect_list.灵宠转生加成:
                        dec += " % \n";
                        break;
                    case artifact_offect_list.角色转生加成:
                        dec += " % \n";
                        break;
                    case artifact_offect_list.生命值:
                        dec += " \n";
                        break;
                    case artifact_offect_list.魔法值:
                        dec += " \n";
                        break;
                    case artifact_offect_list.幸运:
                        dec += " \n";
                        break;
                    case artifact_offect_list.物攻:
                    case artifact_offect_list.防御:
                    case artifact_offect_list.免伤:
                    case artifact_offect_list.魔攻:
                    case artifact_offect_list.道术:
                        dec += " \n";
                        break;
                    case artifact_offect_list.战系主动技能等级上限:
                    case artifact_offect_list.法系主动技能等级上限:
                    case artifact_offect_list.道士主动技能等级上限:
                        dec += "级\n";
                        break;

                    default:
                        break;
                }
            }
        }

        if (item.crt_Data.Item1 == 1 || item.crt_Data.Item1 == 2)
        {
            dec += "存量" + Battle_Tool.FormatNumberToChineseUnit(item.crt_Data.Item3);
            switch (item.crt_Data.Item1)
            {
                case 1: dec += "\n特效:每30s可以用存量抵抗一次致命伤害"; break;
                case 2: dec += "\n特效:当蓝量不足时自动使用存量填充"; break;
                default:
                    break;
            }
        }
        dec+= "\n";
        if (item.crt_Data.Item2 >= item.crt_artifact.artifact_max)
        {
            dec += "已满级"; 
        }
        else
        {
            List<string> needs = ArrayHelper.Get_Split<string>(item.crt_artifact.artifact_need, ',');
            for (int i = 0; i < needs.Count; i++)
            { 
                List<string> need = ArrayHelper.Get_Split<string>(needs[i], ' ');
                if (need.Count == 3)
                {
                    switch (need[0])
                    {
                        case "0":
                            dec += "升级需求" + Battle_Tool.FormatNumberToChineseUnit(int.Parse(need[2]))+" "+ ((currency_unit)(int.Parse(need[1])))+"\n";
                            break;
                        case "1":
                        case "3":
                            dec += "升级需求" + need[1] + " * " + need[2]+"\n";
                            break;
                        default:
                            break;
                    }
                }
            }

        }
        ClearObject(m_ptn_proms);
        info.text = dec;

        for (int i = 0; i < item.crt_artifact.artifact_btn.Count; i++)
        {
            btn_item btn = Instantiate(btn_item_prefab, m_ptn_proms);
            btn.Show(item.crt_artifact.artifact_btn[i], (artifact_btn_list)item.crt_artifact.artifact_btn[i]);
            btn.GetComponent<Button>().onClick.AddListener(() => { OnClickBtn(btn); });
        }
    }
    /// <summary>
    /// 升级按钮
    /// </summary>
    /// <param name="btn"></param>
    private void OnClickBtn(btn_item btn)
    {
        switch ((artifact_btn_list)btn.index)
        {
            case artifact_btn_list.填充:
                string dec= "";
                switch (crt_item.crt_Data.Item1)
                {
                    case 1:dec += artifact_btn_list.填充 + " " + crt_item.crt_artifact.artifact_name + "\n每1个金条 + 10点血量"; break;
                    case 2: dec += artifact_btn_list.填充 + " " + crt_item.crt_artifact.artifact_name + "\n每1个金条 + 10w魔法"; break;
                    default:
                        break;
                }
                Alert.Show(artifact_btn_list.填充 + "", dec, confirm);
                break;
            case artifact_btn_list.升级:
                UpLv();
                break;
            case artifact_btn_list.激活:
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void UpLv()
    {
        Clear_Condition();
        if (crt_item.crt_Data.Item2 >= crt_item.crt_artifact.artifact_max)
        {
            Alert_Dec.Show("已满级");
            return;
        }
        List<string> needs = ArrayHelper.Get_Split<string>(crt_item.crt_artifact.artifact_need, ',');
        for (int i = 0; i < needs.Count; i++)
        {
            List<string> need = ArrayHelper.Get_Split<string>(needs[i], ' ');
            if (need.Count == 3)
            {
                switch (need[0])
                {
                    case "0":
                        Need_Condition(((currency_unit)(int.Parse(need[1]))), int.Parse(need[2]));
                        break;
                    case "1":
                    case "3":
                        Need_Condition(need[1],int.Parse(need[2]));
                        break;
                    default:
                        break;
                }
            }
        }
        if (Return_Condition())
        {
            bool exist = true;
            List<(int, int, long)> data = SumSave.crt_user_artifact.Get;
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].Item1 == crt_item.crt_Data.Item1)
                {
                    exist = false;
                    data[i] = (data[i].Item1, data[i].Item2 + 1, data[i].Item3);
                }
            }
            if (exist)
            {
                data.Add((crt_item.crt_artifact.artifact_type, 1, 0));
                Alert_Dec.Show("激活成功");
            }
            else
                Alert_Dec.Show("升级成功");
            SumSave.crt_user_artifact.MysqlData();//更新数据库
            SendNotification(NotiList.Refresh_Max_Hero_Attribute);
            baseShow();
        }
        else Alert_Dec.Show("升级失败,材料不足");
    }

    private void confirm(object arg0)
    {
        if (crt_input_offect == null)
        {
            crt_input_offect = Instantiate(input_offect_prefab, transform);
            crt_input_offect.GetConfirm.onClick.AddListener(() => { confirm(); });
        }
        crt_input_offect.gameObject.SetActive(true);
        crt_input_offect.Init("填充" + crt_item.crt_artifact.artifact_name, "请输入数量"); 
    }

    private void confirm()
    {
        if (crt_input_offect.GetInput != "")
        {
            if (int.TryParse(crt_input_offect.GetInput, out int result))
            {
                // 是纯数字（可转为 int）
                int number = int.Parse(crt_input_offect.GetInput);
                if (number <= 0)
                {
                    Alert_Dec.Show("请输入正确的数字");
                    return;
                }
                Clear_Condition();
                Need_Condition(common_items_list.金条, number);
                if (Return_Condition())
                {
                    List<(int, int, long)> data = SumSave.crt_user_artifact.Get;
                    long value = 0;
                    switch (crt_item.crt_Data.Item1)
                    {
                        case 1: value += number * 10; break;
                        case 2: value += number * 100000; break;
                        default:
                            break;
                    }
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i].Item1 == crt_item.crt_Data.Item1)
                        {
                            data[i] = (data[i].Item1, data[i].Item2, data[i].Item3 + value);
                        }
                    }
                    Alert_Dec.Show("填充成功");
                    SumSave.crt_user_artifact.MysqlData();//更新数据库
                    baseShow();
                }
                else Alert_Dec.Show("填充失败,材料不足"); 
            }
            else Alert_Dec.Show("请输入正确的数字");
        }
        else Alert_Dec.Show("请输入正确的数字");
    }
}
