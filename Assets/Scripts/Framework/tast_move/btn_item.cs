using MVC;
using UnityEngine;
using UnityEngine.UI;

public class btn_item : Base_Mono
{
    private Text info;
    public int index;
    /// <summary>
    /// 是否为激活状态
    /// </summary>
    private bool active = false;

    private void Awake()
    {
        info = Find<Text>("info"); 
    }

    /// <summary>
    ///  选中状态
    /// </summary>
    public bool Selected
    {
        set
        {
            active = value;
            info.color = value ? Color.red : Color.white;
        }
    }

    /// <summary>
    /// 引用
    /// </summary>
    /// <param name="_index"></param>
    /// <param name="dec"></param>
    public void Show(int _index, object dec)
    {
        index = _index;
        info.text = dec.ToString();
    }


    // <summary>
    // 是否激活
    // </summary>
    // <returns></returns>
    public bool Active()
    {
        return active;
    }
}
