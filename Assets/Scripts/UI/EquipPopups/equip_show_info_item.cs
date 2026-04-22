using MVC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 装备属性类型
/// </summary>
public enum enum_equip_basetype_list
{ 
    基础属性 = -1,
    附加属性,
    元素属性,
    铭文属性
}
/// <summary>
/// 装备属性条目
/// </summary>
public enum enum_equip_entry_list
{
    生命值 = 14,
    魔法值 = 66,
    物理防御 = 4,
    魔法防御 = 19,
    物理攻击 = 17,
    魔法攻击 = 2,
    道术攻击 = 3,
    每秒回血=20,
    每秒回蓝=21,
    真实伤害 = 36,
    吸收伤害 = 37,
    幸运=0,
    生命属性=22,
    魔法属性=23,
    防御属性=24,
    魔防属性=25,
    物攻属性=26,
    魔攻属性=27,
    道攻属性=28,
    攻击速度=40,
    攻击范围=41,
    暴击属性=42,
    暴击伤害=43,
    命中=87,
    闪避=77,
    烈阳文=44,
    盾护文,
    守月文,
    幽狼文,
    神行文,
    怒目文,
    震火文,
    金刚文,
    大愈文,
    回春文,
    回心文,
    峰芒文,
    破枪文,
    深寒文,
    瑶光文,
    物理下防 = 101,
    魔法下防,
    物理下攻,
    魔法下攻,
    道术下攻,
    物伤减免=18,
    魔伤减免=39,
    怪物爆率=7,
    极品爆率=8,
    经验加成=9,
    金币掉落=6,

}

public class equip_show_info_item : Base_Mono
{
    private Image icon;
    private TMP_Text info;
    private void Awake()
    {
        icon = Find<Image>("show_info");
        info = Find<TMP_Text>("base_info/info");
    }

    public void Init(enum_equip_basetype_list basetype, enum_equip_entry_list type,object _info,Color color)
    {
        string path = "", subpath = "";
        switch (basetype)
        {
            case enum_equip_basetype_list.基础属性:
                path = "UI/bg_lists/bgbuff/BUFF_ICON04";
                subpath = $"BUFF_ICON04_{(int)type}";
                break;
            case enum_equip_basetype_list.附加属性:
                path = "UI/bg_lists/bgbuff/BUFF_ICON02";
                subpath = $"BUFF_ICON02_{(int)type}";
                break;
            case enum_equip_basetype_list.元素属性:
                path = "UI/bg_lists/bgbuff/BUFF_ICON03";
                subpath = $"BUFF_ICON03_{(int)type}";
                break;
            case enum_equip_basetype_list.铭文属性:
                path = "UI/bg_lists/bgbuff/BUFF_ICON01";
                subpath = $"BUFF_ICON01_{(int)type}";
                break;
            default:
                break;
        }
        icon.sprite = Tool_UI.Obtain_Sprite(path, subpath);
        info.text = "   " + type + ": " + _info;
        info.color = color;

    }

    public void Init(string value,int lv)
    {
        icon.sprite = UI.UI_Manager.I.GetEquipSprite("skill/base_icon/",value);
        info.text =  "弹道 " + value + " + " + lv;
        info.color = UnityColorPresets.HexToColor("#ffff00");
    }
}
