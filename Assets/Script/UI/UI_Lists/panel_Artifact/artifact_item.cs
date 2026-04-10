using MVC;
using UnityEngine.UI;

public class artifact_item : Base_Mono
{
    /// <summary>
    /// 物品图标
    /// </summary>
    private Image icon;
    /// <summary>
    /// 物品信息
    /// </summary>
    private Text info;

    public int base_lv = 0;
    private void Awake()
    {
        icon = Find<Image>("icon");
        info = Find<Text>("info");
    }

    private db_artifact_vo data;

    /// <summary>
    /// Data
    /// </summary>
    public db_artifact_vo Data
    {
        set
        {
            data = value;
            if (data == null) return;
            icon.sprite = UI.UI_Manager.I.GetEquipSprite("icon/", data.arrifact_name);
            info.text = data.arrifact_name + "(未激活)";
        }
        get
        {
            return data;
        }
    }
    /// <summary>
    /// 读取等级
    /// </summary>
    /// <param name="lv"></param>
    public void Set(int lv)
    {
        base_lv = lv;
        info.text = data.arrifact_name+"Lv."+lv;

    }
}
