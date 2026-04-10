using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class show_info_item : MonoBehaviour
{
    private Text info;
    private void Awake()
    {
        info = GetComponent<Text>();
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
