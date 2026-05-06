using MVC;
using TMPro;
using UnityEngine.UI;

public class store_item : Base_Mono
{
    /// <summary>
    /// 购买
    /// </summary>
    private Button buy;
    /// <summary>
    /// 显示信息
    /// </summary>
    private TMP_Text baseinfo;
    /// <summary>
    /// 显示图标
    /// </summary>
    private Image icon;

    private Image state;
    /// <summary>
    /// 数据
    /// </summary>
    private material_item material_item_Prefabs;
    private db_store_vo data;
    private void Awake() 
    {
        baseinfo=Find<TMP_Text>("info/info");
        icon=Find<Image>("icon/icon");
        material_item_Prefabs = Tool_UI.Find_Prefabs<material_item>("material_item"); //Battle_Tool.Find_Prefabs<material_item>("material_item");
        state=Find<Image>("icon/state");
    }
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="bag_Resources"></param>
    public void Init((string, int) bag_Resources,string unit)
    {
        baseinfo.text = Show_Color.White(bag_Resources.Item1) + "\n单价"
            + Battle_Tool.FormatNumberToChineseUnit(bag_Resources.Item2)
            + " " + unit
            + "\n" + Show_Color.Green("购买");
        material_item item = Instantiate(material_item_Prefabs, icon.transform);
        item.Init((bag_Resources.Item1,1));
    }
    /// <summary>
    /// 显示物品信息
    /// </summary>
    /// <param name="store"></param>
    public void Init(db_store_vo store)
    {
        data = store;
        state.gameObject.SetActive(store.ItemMaxQuantity != -1);
        baseinfo.text = Show_Color.White(store.ItemName) + "\n单价"
            + Battle_Tool.FormatNumberToChineseUnit(store.ItemPrice)
            + " " + store.unit
            + "\n" + Show_Color.Green("购买");
        string name = store.ItemName;
        if (store.ItemName == "灵宠")
        {
            db_vip crt_vip = Tool_Battle.Obtain_Vip();
            if (crt_vip == null)
            {
                name = pet_list.麋鹿.ToString();
            }
            else name = ((pet_list)(crt_vip.vip_lv - 1)).ToString();
        }
        Instantiate(material_item_Prefabs, icon.transform).Init((name, 1));
    }
    /// <summary>
    /// 获取值
    /// </summary>
    /// <returns></returns>
    public db_store_vo Set()
    {
        return data;
    }
    public void PetInit((string, int) bag_Resources, string unit)
    {
        //baseinfo.alignment =  
        //baseinfo.alignment = TextAnchor.MiddleCenter;
        baseinfo.text = Show_Color.White(bag_Resources.Item1);
        material_item item = Instantiate(material_item_Prefabs, icon.transform);
        item.Init((bag_Resources.Item1, 1));
    }
}
