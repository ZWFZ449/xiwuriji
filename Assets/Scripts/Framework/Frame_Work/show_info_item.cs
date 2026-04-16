using MVC;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class show_info_item : Base_Mono
{
    private TMP_Text info;
    private void Awake()
    {
        info = Find<TMP_Text>("info");
    }

    public void Show(string str)
    { 
        info.text= str;
        StartCoroutine(Game_WaitTime(3));
    }

    private IEnumerator Game_WaitTime(float time)
    {
        while (time > 0)
        {
            time -= 0.1f;
           
            yield return new WaitForSeconds(0.1f);
        }
        this.gameObject.SetActive(false);
    }
   
}
