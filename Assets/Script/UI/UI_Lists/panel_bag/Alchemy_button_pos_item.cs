using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alchemy_button_pos_item : MonoBehaviour
{
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        transform.parent.parent.SendMessage("Init_Pos", this);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
