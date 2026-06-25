using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class artifact_item : Base_Mono
{

    private db_artifact_vo crt_artifact;

    private Image icon;

    private Image framge;

    private float basealpha = 100f;

    private void Awake()
    {
        icon = GetComponent<Image>();

        framge = Find<Image>("7");
    }


    public void Set()
    {
        alpha(100);
        framge.gameObject.SetActive(true);
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
