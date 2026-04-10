using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class offect_pharmacy : Base_Mono
{
    private offect_store offectStore;
    private void Awake()
    {
        offectStore = Instantiate(Tool_UI.Find_Prefabs<offect_store>("offect_store"), this.transform);
    }

    public override void Show()
    {
        base.Show();
        offectStore.Show();
        offectStore.Init(Store_Type.药铺);
    }
}
