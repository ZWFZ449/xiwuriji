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

public class panel_Buff : Panel_Base
{
    /// <summary>
    /// 输入框
    /// </summary>
    private InputField inputField;
    /// <summary>
    /// 确认按钮
    /// </summary>
    private Button confirm;
    /// <summary>
    /// 信息显示
    /// </summary>
    private Text info;


    /// <summary>
    /// 角色类型
    /// </summary>
    private string skin_state;
    /// <summary>
    /// 角色皮肤预制体
    /// </summary>
    private GameObject skin_prefabs;
    /// <summary>
    /// 角色within位置,五行属性显示位置
    /// </summary>
    private Transform panel_role_health,Five_element_transform;
    /// <summary>
    /// btn_item预制体
    /// </summary>
    private btn_item btn_item_prefabs;
    /// <summary>
    /// 五行类型
    /// </summary>
    private string[] five_element_type = { "土", "火", "水", "木", "金" };  

    /// <summary>
    /// 五行属性显示
    /// </summary>
    private List<btn_item> btn_items=new List<btn_item>();

    public override void Hide()
    {
        base.Hide();
    }

    public override void Initialize()
    {
        base.Initialize();
        inputField = Find<InputField>("bg_main/InputField");
        inputField.onValueChanged.AddListener(FilterNameInput);
        confirm = Find<Button>("bg_main/confirm");
        info = Find<Text>("bg_main/ScrollView/Viewport/Content/info");
        confirm.onClick.AddListener(OnConfirmClick);

        panel_role_health = Find<Transform>("bg_main/bg");

        skin_prefabs = Resources.Load<GameObject>("Prefabs/Skins/within_" + SumSave.old_crt_hero.hero_pos);
        Instantiate(skin_prefabs, panel_role_health);

        Five_element_transform = Find<Transform>("bg_main/Five_element_attribute/Viewport/Content");
        btn_item_prefabs = Battle_Tool.Find_Prefabs<btn_item>("btn_item");

        show_tianming_Platform= Find<Transform>("bg_main/bg/tianming_Platform");


        Display_Five_element_attribute();
    }
    /// <summary>
    /// 初始化皮肤
    /// </summary>
    private void Instance_Skin()
    {
        for (int i = panel_role_health.childCount - 1; i >= 1; i--)//保留天命台组件
        {
            Destroy(panel_role_health.GetChild(i).gameObject);
        }
        skin_prefabs = Resources.Load<GameObject>("Prefabs/Skins/within_" + SumSave.old_crt_hero.hero_pos);
        Instantiate(skin_prefabs, panel_role_health);
        skin_state = SumSave.old_crt_hero.hero_pos;
    }

    #region 显示天命光环
    /// <summary>
    /// 天命台
    /// </summary>
    private int[] tianming_Platform;
    /// <summary>
    /// 天命台位置
    /// </summary>
    private Transform show_tianming_Platform;
    /// <summary>
    /// 天命台父物体大小,当前天命大小
    /// </summary>
    private Vector2 pos_tianming_size,tianming_size;
    /// <summary>
    /// 缩放比例
    /// </summary>
    private float scaling=1;
    /// <summary>
    /// 每个天命的数量
    /// </summary>
    private Dictionary<int, int> tianming_num;

    /// <summary>
    /// 显示五行光环
    /// </summary>
    private void Show_Info_life()
    {

        tianming_Platform = (int[])SumSave.old_crt_hero.tianming_Platform.Clone();

        for (int i = show_tianming_Platform.childCount - 1; i >= 0; i--)//清空区域内按钮
        {
            Destroy(show_tianming_Platform.GetChild(i).gameObject);
        }
        pos_tianming_size = show_tianming_Platform.GetComponent<RectTransform>().rect.size;

        tianming_num = new Dictionary<int, int>();



        for (int i = 0; i < SumSave.old_crt_hero.tianming_Platform.Length; i++)
        {
            if(tianming_num.ContainsKey(SumSave.old_crt_hero.tianming_Platform[i]))
            {
                tianming_num[SumSave.old_crt_hero.tianming_Platform[i]]++;
            }
            else
            {
                tianming_num.Add(SumSave.old_crt_hero.tianming_Platform[i], 1);
            }
        }
        

        for (int i = 0; i < SumSave.old_crt_hero.tianming_Platform.Length; i++)
        {
            GameObject game = Resources.Load<GameObject>("Prefabs/halo/halo_" + (SumSave.old_crt_hero.tianming_Platform[i] + 1));
            GameObject tianming = Instantiate(game, show_tianming_Platform);

            tianming.transform.Rotate(new Vector3(0, 0, 15 * i));

           
            tianming_size = new Vector2(pos_tianming_size.x * scaling, pos_tianming_size.y * scaling);
            tianming.GetComponent<RectTransform>().sizeDelta = tianming_size;

            Color currentColor = tianming.GetComponentInChildren<Image>().color;
            currentColor.a = tianming_num[SumSave.old_crt_hero.tianming_Platform[i]] * 0.2f;
            tianming.GetComponentInChildren<Image>().color = currentColor;
        }
    }

#endregion

    /// <summary>
    /// 点击事件
    /// </summary>
    private void OnConfirmClick()
    {
        Alert.Show("提示", "确定将角色名称修改为\n" + Show_Color.Red(inputField.text) + " ？", Confirm);
    }

    /// <summary>
    ///  过滤掉不符合规则的字符
    /// </summary>
    /// <param name="input"></param>
    private void FilterNameInput(string input)
    {
        
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        foreach (char c in input)
        {
            if (char.IsLetterOrDigit(c) || c == '_' || IsChineseCharacter(c))
            {
                sb.Append(c);
            }
        }

        if (sb.ToString() != input)
        {
            inputField.text = sb.ToString();
        }
    }
    private bool IsChineseCharacter(char c)
    {
        return c >= 0x4E00 && c <= 0x9FFF;
    }

    /// <summary>
    /// 确认
    /// </summary>
    /// <param name="arg"></param>
    private void Confirm(object arg)
    { 
        SumSave.old_crt_hero.hero_name= inputField.text;
        SumSave.old_crt_hero.hero_material_list[0] = 1;
        SumSave.old_crt_hero.MysqlData();
        SendNotification(NotiList.Refresh_Max_Hero_Attribute);
        Show();
    }
    public override void Show()
    {
        base.Show();

        if (SumSave.old_crt_hero.hero_pos != skin_state)
        {
            Instance_Skin();
        }

        confirm.gameObject.SetActive(SumSave.old_crt_hero.hero_material_list[0] == 0 || SumSave.old_crt_hero.hero_name == "墨龙新星");
        inputField.text = SumSave.old_crt_hero.hero_name;
        InitInformation();
        Refresh_Five_element_attribute();

        if (tianming_Platform == null || !tianming_Platform.SequenceEqual(SumSave.old_crt_hero.tianming_Platform))
        {
            Show_Info_life();
        }
    }
    private void Update()
    {
        InitInformation();
    }

    /// <summary>
    /// 刷新五行属性
    /// </summary>
    private void Refresh_Five_element_attribute()
    {
        for(int i= 0; i < btn_items.Count; i++)
        {
            btn_items[i].Show(i, five_element_type[i] + "\n" + SumSave.crt_MaxHero_okd.life[i].ToString());
        }
    }

    /// <summary>
    /// 初始化五行属性
    /// </summary>
    private void Display_Five_element_attribute()
    {
        ClearObject(Five_element_transform);

        int[] five_element = SumSave.crt_MaxHero_okd.life;

        for(int i= 0; i < five_element.Length; i++)
        {
            btn_item item= Instantiate(btn_item_prefabs, Five_element_transform);
            item.Show(i, five_element_type[i]+"\n"+ five_element[i].ToString());  
            btn_items.Add(item);
        }
    }



    private void InitInformation()
    {
 
        string dec = " ";
        List<float> buff_list = new List<float>(3) { 0,0,0};//0经验加成 1灵珠加成 2历练加成
        if (SumSave.crt_player_buff.player_Buffs.Count > 0)
        {
            foreach (var item in SumSave.crt_player_buff.player_Buffs)
            {
                (DateTime, int, float, int) time = item.Value;
                int remainingTime = Battle_Tool.SettlementTransport((time.Item1).ToString("yyyy-MM-dd HH:mm:ss"), 2);
                if (time.Item4 == 3)//月卡
                {
                    if (remainingTime < time.Item2*60)
                    {
                        buff_list[0] += time.Item3 * 100 - 100;
                        buff_list[2]+= time.Item3 * 100 - 100;
                        dec += Show_Color.Red(item.Key + ": " + ConvertSecondsToHHMMSS(time.Item2 * 60 - remainingTime,2) + "\n ");

                    }
                }
                if (time.Item4 == 5)//至尊卡
                {
                    buff_list[0] += time.Item3 * 100 - 100;
                    buff_list[1] += (float)Math.Ceiling(time.Item3 * 100 - 100);
                    dec += Show_Color.Red(item.Key + ": " + ConvertSecondsToHHMMSS(time.Item2 * 60 - remainingTime, 2) + "\n ");
                }


                if (time.Item4 == 1 || time.Item4 == 2)
                {
                    if (remainingTime < time.Item2 * 60)
                    {
                        if (time.Item4 == 1) buff_list[0] += time.Item3 * 100 - 100;
                        if (time.Item4 == 2) buff_list[2] += time.Item3 * 100 - 100;
                        dec += Show_Color.Red(item.Key + ": " + ConvertSecondsToHHMMSS(time.Item2 * 60 - remainingTime,2) + "\n ");

                    }
                }
            }
        }

        for (int i = 0; i < buff_list.Count; i++) 
        {
            if (buff_list[i] > 0)
            {
                switch (i)
                {
                    case 0:dec += Show_Color.Green("[Buff] 经验加成 " + buff_list[i] + "%\n "); break;
                    case 1: dec += Show_Color.Green("[Buff] 灵珠加成 " + buff_list[i] + "%\n "); break;
                    case 2: dec += Show_Color.Green("[Buff] 历练加成 " + buff_list[i] + "%\n "); break;
                    default:
                        break;
                }
            }
        }


        dec += enum_skill_attribute_list.经验加成 + ": " + Show_Buff(enum_skill_attribute_list.经验加成) + "%\n ";
        dec += enum_skill_attribute_list.人物历练 + ": " + Show_Buff(enum_skill_attribute_list.人物历练) + "%\n ";
        dec += enum_skill_attribute_list.灵珠收益 + ": " + Show_Buff(enum_skill_attribute_list.灵珠收益) + "%\n ";
        dec += enum_skill_attribute_list.装备爆率 + ": " + Show_Buff(enum_skill_attribute_list.装备爆率) + "%\n ";
        dec += enum_skill_attribute_list.装备掉落 + ": " + Show_Buff(enum_skill_attribute_list.装备掉落) + "%\n ";
        dec += enum_skill_attribute_list.宠物获取 + ": " + Show_Buff(enum_skill_attribute_list.宠物获取) + "%\n ";
        dec += enum_skill_attribute_list.寻怪间隔 + ": -" + (Show_Buff(enum_skill_attribute_list.寻怪间隔) /10f) + "s\n ";
        dec += enum_skill_attribute_list.复活次数 + ": " + Show_Buff(enum_skill_attribute_list.复活次数) + "次\n ";


        info.text = dec;
    }

    /// <summary>
    /// 显示buff
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private float Show_Buff(enum_skill_attribute_list index)
    {
        float value = 0;
        if ((int)index < SumSave.crt_MaxHero_okd.bufflist.Count)
            value = SumSave.crt_MaxHero_okd.bufflist[(int)index];
        return value;
    }

    protected override void Awake()
    {
        base.Awake();
    }
}

