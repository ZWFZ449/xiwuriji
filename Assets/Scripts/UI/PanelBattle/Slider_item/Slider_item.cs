using MVC;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slider_item : Base_Mono
{
    private Slider slider;

    private TMP_Text info;
    private void Awake()
    {
        slider=Find<Slider>("Slider");
        info = Find<TMP_Text>("info");
    }

    public void Init(Color up_color, Color down_color,int value, int max_value)
    { 
        slider.fillRect.GetComponent<Image>().color = up_color;
        //slider.handleRect.GetComponent<Image>().color = down_color;
        slider.maxValue = max_value;
        slider.value = value;
    }

    public void Refresh(int value,int max_value,object info)
    { 
        slider.value = value;
        slider.maxValue = max_value;
        this.info.text = info+" "+ slider.value+"/"+slider.maxValue;
    }
}
