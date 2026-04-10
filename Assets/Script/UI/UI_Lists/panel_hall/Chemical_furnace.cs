using Common;
using Components;
using MVC;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Chemical_furnace : Base_Mono
{
    /// <summary>
    /// 合成物品位置，材料位置
    /// </summary>
    private Transform synthesis_Items,pos_btn,show_pos_synthesis;
    /// <summary>
    /// 合成按钮
    /// </summary>
    private Button synthesisButtom;
    /// <summary>
    /// 合成信息
    /// </summary>
    private Text synthesisinfo,btn_info;
    /// <summary>
    /// 商品输入的数量
    /// </summary>
    private InputField inputField;
    /// <summary>
    /// 合成物品的最大长度
    /// </summary>
    private int maxLength = 2;
    /// <summary>
    /// 玩家需要合成的数量
    /// </summary>
    private int buy_num = 1;
    /// <summary>
    /// 合成物品预制体
    /// </summary>
    private btn_item btn_item_Prefabs;
    /// <summary>
    /// 显示
    /// </summary>
    private material_item material_item_Prefabs;
    /// <summary>
    /// 当前选择的合成物品
    /// </summary>
    private (string,int,int) synthesis_item;
    /// <summary>
    /// 分解物品
    /// </summary>
    private (string, int) crt_break;
    /// <summary>
    /// 合成类型
    /// </summary>
    private string[] synthesis_item_list = new string[] { "资源", "材料" ,"分解","灵宝","灵宠"};
    /// <summary>
    /// 选择类型
    /// </summary>
    private int select_type = -1;

    private void Awake()
    {
        Initialize();
    }
    public void Initialize()
    {
        show_pos_synthesis=Find<Transform>("material_Items/Title");
        synthesisinfo = Find<Text>("material_Items/synthesisinfo");
        pos_btn =Find<Transform>("synthesis_Items/Title/Scroll View/Viewport/Content");
        btn_item_Prefabs = Battle_Tool.Find_Prefabs<btn_item>("btn_item");
        material_item_Prefabs = Battle_Tool.Find_Prefabs<material_item>("material_item");
        inputField = Find<InputField>("material_Items/InputField");
        inputField.onEndEdit.AddListener(OnInputChanged);//监听输入框
        synthesis_Items= Find<Transform>("synthesis_Items/Viewport/Content");
        synthesisButtom = Find<Button>("material_Items/synthesisButtom");
        synthesisButtom.onClick.AddListener(Synthesis);
        btn_info = Find<Text>("material_Items/synthesisButtom/Text");
        for (int i = 0; i < synthesis_item_list.Length; i++) 
        {
            btn_item item = Instantiate(btn_item_Prefabs, pos_btn);
            item.Show(i, synthesis_item_list[i]);
            item.GetComponent<Button>().onClick.AddListener(() => { Show_Material_Type(item); });
            if (select_type == -1) Show_Material_Type(item);
        }
    }
    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        ClearObject(synthesis_Items);
        ClearObject(show_pos_synthesis);
        synthesisinfo.text = "请选择物品";
        buy_num = 1;
        if (inputField.text != "")
            inputField.text = buy_num.ToString();
        if (select_type == 3)
        {
            btn_info.text = "分解";
            //分解
            List<(string, int)> lists = SumSave.crt_bag_resources.Set();
            for (int i = 0; i < lists.Count; i++)
            {
                (string, int) data = lists[i];

                if (data.Item2 > 0)
                {
                    Bag_Base_VO base_VO = ArrayHelper.Find(SumSave.db_stditems, e => e.Name == data.Item1);
                    if (base_VO != null)
                    {
                        switch ((EquipConfigTypeList)Enum.Parse(typeof(EquipConfigTypeList), base_VO.StdMode))
                        {
                            case EquipConfigTypeList.神器:
                                material_item item = Instantiate(material_item_Prefabs, synthesis_Items);
                                item.Init(data);
                                item.GetComponent<Button>().onClick.AddListener(delegate { Show_Material(data); });
                                break;
                        }
                    }

                }
            }

        }
        else
        {
            btn_info.text = "合成";
            for (int i = 0; i < SumSave.db_formula_list.Count; i++)
            {
                if (SumSave.db_formula_list[i].formula_type == select_type)
                {
                    (string, int, int) item1 = SumSave.db_formula_list[i].formula_result_list;
                    material_item item = Instantiate(material_item_Prefabs, synthesis_Items);
                    item.Init((item1.Item1, item1.Item2));
                    item.GetComponent<Button>().onClick.AddListener(() => { ShowMaterial(item1); });
                    if (synthesis_item.Item2 == 0) ShowMaterial(item1);
                }
            }

        }
    }
    /// <summary>
    /// 分解物品
    /// </summary>
    /// <param name="item"></param>
    private void Show_Material((string, int) item)
    {
        crt_break = item;
        ClearObject(show_pos_synthesis);
        Instantiate(material_item_Prefabs, show_pos_synthesis).Init(crt_break);
        string dec = "分解信息：" + crt_break.Item1 + "*" + (crt_break.Item2 * buy_num);
        dec+= "\n分解需求：" + "下品噬心魔种"+" * "+ buy_num;
        dec += "\n分解数量：" + buy_num;
        synthesisinfo.text = dec;
    }

    /// <summary>
    /// 选择类型
    /// </summary>
    /// <param name="item"></param>
    private void Show_Material_Type(btn_item item)
    {
        select_type = item.index + 1;
        Init();
    }

    /// <summary>
    /// 显示需要的材料
    /// </summary>

    private void ShowMaterial((string , int,int) item)
    {
        synthesis_item = item;
        ClearObject(show_pos_synthesis);
        Instantiate(material_item_Prefabs, show_pos_synthesis).Init((item.Item1, item.Item2));
        string dec="合成信息：" + item.Item1 + "*" + (item.Item2 * buy_num);
        db_formula_vo formula = SumSave.db_formula_list.Find((x) => x.formula_result_list.Item1 == item.Item1);
        for (int i = 0; i < formula.formula_need_list.Count; i++)
        {
            dec += "\n" + formula.formula_need_list[i].Item1 + "*" + (formula.formula_need_list[i].Item2 * buy_num);
        }
        dec += "\n合成数量：" + buy_num;
        synthesisinfo.text = dec;
    }

    /// <summary>
    /// 合成物品
    /// </summary>
    private void Synthesis()
    {
        SendNotification(NotiList.Read_Mysql_Base_Time);
        if (SumSave.openMysql)
        {
            Alert_Dec.Show("网络连接失败");
            return;
        }
        if (select_type == 3)
        {
            if (crt_break.Item2 == 0)
            { 
                Alert_Dec.Show("请选择分解物品");
                return;
            }
            NeedConsumablesold("下品噬心魔种", buy_num);
            NeedConsumablesold(crt_break.Item1,buy_num);
            if (RefreshConsumables_old())
            {
                int random = Random.Range(1, 100);
                int number = buy_num;
                int maxnumber = number + Random.Range(1, 100);
                Battle_Tool.Obtain_Resources(Obtain_Int.Add(1, "灵物碎片", new int[] { number + random, random }), maxnumber);
                //Battle_Tool.Obtain_Resources("灵物碎片", buy_num);
                Alert.Show("分解成功", "获得物品" + " 灵物碎片 " + "*" + buy_num);
                Init();

            }else Alert_Dec.Show("分解失败,材料不足");
        }
        else
        {
            if (synthesis_item.Item2 == 0)
            { 
                Alert_Dec.Show("请选择合成物品");
                return;
            }

            db_formula_vo formula = SumSave.db_formula_list.Find((x) => x.formula_result_list.Item1 == synthesis_item.Item1);
            for (int i = 0; i < formula.formula_need_list.Count; i++)
            {
                NeedConsumablesold(formula.formula_need_list[i].Item1, formula.formula_need_list[i].Item2 * buy_num);
            }
            if (RefreshConsumables_old()) 
            {
                if (select_type == 4)//灵宝
                {
                    //SumSave.crt_bag.Add(tool_Categoryt.crate_equip(synthesis_item.Item1, 7));
                    //Game_Omphalos.i.Wirte_ResourcesList(Emun_Resources_List.bag_value, SumSave.crt_bag);
                    Game_Omphalos.i.archive();
                }
                else
                {
                    Battle_Tool.Obtain_result(synthesis_item, buy_num);
                    Alert.Show("合成成功", "获得物品" + synthesis_item.Item1 + "*" + synthesis_item.Item2 * buy_num);
                }
              
                synthesis_Task();
            }
            else Alert_Dec.Show("合成失败,材料不足");
        }
       

    }

    /// <summary>
    /// 造化炉合成任务
    /// </summary>
    private static void breakDown_Task()
    {
        tool_Categoryt.Base_Task(1082);
        tool_Categoryt.Base_Task(1089);
    }


    /// <summary>
    /// 造化炉合成任务
    /// </summary>
    private static void synthesis_Task()
    {
        tool_Categoryt.Base_Task(1085);
    }

    private void OnInputChanged(string newText)
    {
        char[] chars = newText.ToCharArray();
        string filteredText = "";
        // 仅保留数字
        foreach (char c in chars)
        {
            if (char.IsDigit(c))
            {
                filteredText += c;
            }
        }
        // 截断超长部分
        if (filteredText.Length > maxLength)
        {
            filteredText = filteredText.Substring(0, maxLength);
        }
        // 同步输入框内容
        if (filteredText != inputField.text)
        {
            inputField.text = filteredText;
            SetCursorToEnd();
        }
        //获取购买数量
        if (int.TryParse(inputField.text, out int value))
        {
            buy_num = value;
        }
        else
        {
            buy_num = 1;
        }
        if (buy_num <= 0) buy_num = 1;
        if (select_type == 3)
        {
            buy_num = Math.Min(buy_num, crt_break.Item2);
            if (buy_num != 1) Show_Material(crt_break);
        }
        else
        {
            if (buy_num != 1) ShowMaterial(synthesis_item);
        }
    }
    // 保持光标在末尾（避免跳动）
    private void SetCursorToEnd()
    {
        inputField.selectionAnchorPosition = inputField.text.Length;
        inputField.selectionFocusPosition = inputField.text.Length;
    }


    public override void Show()
    {
        base.Show();
        Init();
    }

}
