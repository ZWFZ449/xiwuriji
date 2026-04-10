using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;
using UnityEngine.UI;


public class panel_close : Panel_Base
{
    private Button Mask;

    private void Awake()
    {
        Mask = transform.Find("Mask").GetComponent<Button>();
        if (Mask != null)
            Mask.onClick.AddListener(Hide);
    }

}
