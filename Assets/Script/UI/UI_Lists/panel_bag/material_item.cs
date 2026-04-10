using MVC;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class material_item : Base_Mono
{
    private Image item_icon, item_frame;
    /// <summary>
    /// 基础信息
    /// </summary>
    private Text base_info;
    /// <summary>
    /// 资源
    /// </summary>
    private (string, int) data;
    /// <summary>
    /// 炼丹材料
    /// </summary>
    private (string, List<string>) data_seed;
    /// <summary>
    /// 存储数据
    /// </summary>
    private object base_data;
    private void Awake()
    {
        item_icon = Find<Image>("icon");
        item_frame = GetComponent<Image>();
        base_info= Find<Text>("base_info");
    }
     
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="bag_Resources"></param>
    public void Init((string, int) bag_Resources)
    { 
        data = bag_Resources;
        item_icon.sprite = UI.UI_Manager.I.GetEquipSprite("icon/", data.Item1);
        base_info.text = Battle_Tool.FormatNumberToChineseUnit(data.Item2);
    }
    public void Init((string,List<string>) bag_Resources)
    {
        data_seed = bag_Resources;
        item_icon.sprite = UI.UI_Manager.I.GetEquipSprite("icon/", bag_Resources.Item1);
        base_info.text = "1";
    }
    /// <summary>
    /// 是否隐藏效果
    /// </summary>
    /// <param name="isSelect"></param> 
    public void SetFrame(bool isSelect)
    {
        item_icon.color = isSelect ? Color.white : Color.gray;
    }
    /// <summary>
    /// 返回数据
    /// </summary>
    /// <returns></returns>
    public (string, int) GetItemData()
    {
        return data;
    }

    public (string, List<string>) GetSeedData()
    {
        return data_seed;
    }
}
