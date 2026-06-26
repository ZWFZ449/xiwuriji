using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class artifact_item : Base_Mono
{

    public db_artifact_vo crt_artifact;

    private (int, int, long) crt_data;
    public (int, int, long) crt_Data { set { crt_data = value; } get { return crt_data; } }

    private Image icon;

    private Image framge;

    private float basealpha = 100f;

    private void Awake()
    {
        icon = GetComponent<Image>();

        framge = Find<Image>("artifact_frame");
    }


    public void Set((int,int,long) data)
    {
        alpha(100);
        framge.gameObject.SetActive(true);
        crt_data = data;
    }
    public void Initialize(db_artifact_vo db_artifact_vo)
    {
        crt_artifact= db_artifact_vo;
        icon.sprite = UI.UI_Manager.I.GetEquipSprite("artifact/", crt_artifact.artifact_name);
        alpha(30);
        framge.gameObject.SetActive(false);
        
    }

    private void alpha(int crt_alpha)
    {
        float alpha = crt_alpha / basealpha;
        if (alpha <= 0.3f) alpha = 0.3f;
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, alpha);
    }
}
