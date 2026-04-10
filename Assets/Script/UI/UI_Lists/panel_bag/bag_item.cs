using MVC;
using UnityEngine;
using UnityEngine.UI;

public class bag_item : Base_Mono
{
    private Image item_icon, item_frame,lock_On;
    private Text info;
    private Transform receive;
    private void Awake()
    {
        item_icon = Find<Image>("icon");
        item_frame = Find<Image>("frame");
        info = Find<Text>("info");
        lock_On= Find<Image>("icon/lock");
        receive= Find<Transform>("receive");
        receive.gameObject.SetActive(false);
    }

    private Bag_Base_VO data;

    /// <summary>
    /// 使图标透明化
    /// </summary>
    public void Transparent()
    {
        if (item_icon == null) return;

        // 获取当前颜色
        Color currentColor = item_icon.color;

        // 修改Alpha值（确保在0~1范围内）
        currentColor.a = Mathf.Clamp01(0.5f);

        // 应用新的颜色
        item_icon.color = currentColor;
    }
    /// <summary>
    /// 显示已领取图标
    /// </summary>
    public void showReceive()
    {
        receive.gameObject.SetActive(true);
    }

    /// <summary>
    /// Data
    /// </summary>
    public Bag_Base_VO Data
    {
        set
        {
            data = value;
            if (data == null) return;
            if(item_icon==null)
            {
                Awake();
            }

                item_icon.sprite = UI.UI_Manager.I.GetEquipSprite("icon/", data.Name);
            if (data.StdMode == EquipConfigTypeList.护臂.ToString() || data.StdMode == EquipConfigTypeList.扳指.ToString())
            {
                //方位偏转
            }
            if (data.user_value != null)
            {
                string[] info_str = data.user_value.Split(' ');
                info.text = info_str[1] == "0" ? "" : ("+" + info_str[1]);
                int lv = int.Parse(info_str[2]);
                if (lv <= 5)
                {
                    item_frame.sprite = UI.UI_Manager.I.GetEquipSprite("frame/", lv.ToString());
                    item_frame.color = Color.white;
                }
                else
                {
                    //lv++;
                    item_frame.sprite = UI.UI_Manager.I.GetEquipSprite("frame/", "5");
                    item_frame.color = Color.white;
                    Instantiate(Resources.Load<GameObject>("Prefabs/frame/" + lv), item_frame.transform);
                }
                if (info_str.Length >= 6)
                { 
                    lock_On.gameObject.SetActive(info_str[5] == "1");
                }
            }

        }
        get
        {
            return data;
        }
    }
}
