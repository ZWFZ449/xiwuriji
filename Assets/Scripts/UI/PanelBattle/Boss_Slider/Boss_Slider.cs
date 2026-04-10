using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_Slider : Base_Mono
{
    /// <summary>
    /// Boss血条
    /// </summary>
    private Slider slider;
    /// <summary>
    /// Boss血量
    /// </summary>
    private Text info;

    private string base_Name = "Boss";

    private long maxHp;
    private void Awake()
    {
        slider= GetComponent<Slider>();
        info = Find<Text>("info");
    }
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="name"></param>
    /// <param name="hp"></param>
    public void Init(string name, long hp)
    {
        maxHp = hp;
        base_Name = name;
        slider.maxValue = hp;
        slider.value = hp;
        info.text = base_Name + hp + "/" + maxHp;
    }
    /// <summary>
    /// 实时刷新
    /// </summary>
    /// <param name="hp"></param>
    public void Set(int hp)
    { 
        slider.value = hp;
        info.text = base_Name + hp + "/" + maxHp;
        if(hp<=0)this.gameObject.SetActive(false);
    }
}
