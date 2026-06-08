using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class show_refined_item : MonoBehaviour
{
    private Text info;
    private void Awake()
    {
        info = GetComponent<Text>();
    }

    public void Show(string value)
    { 
        info.text = value;
    }
}
