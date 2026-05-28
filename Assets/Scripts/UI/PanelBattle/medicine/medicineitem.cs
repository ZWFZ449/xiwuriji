using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 药品使用
/// </summary>
public class medicineitem : Base_Mono 
{
    /// <summary>
    /// 冷却cd
    /// </summary>
    private CircularHealthBar cdBar;
    /// <summary>
    /// 药品类型
    /// </summary>
    private medicineType Type;
    /// <summary>
    /// 药品显示
    /// </summary>
    private Image bg_Image,icon_Image;
    /// <summary>
    /// 药品数量
    /// </summary>
    private Text number_Text;
    /// <summary>
    /// 触发比例
    /// </summary>
    private Text proportion_Text;
    /// <summary>
    /// 药品数量
    /// </summary>
    private int medicine_number = -1;
    /// <summary>
    /// 当前药品
    /// </summary>
    private medicineVO Crt_medicine;
    /// <summary>
    /// 是否使用
    /// </summary>
    private bool isUse = true;
    /// <summary>
    /// 药品冷却cd
    /// </summary>
    private float crt_cdtime;
    /// <summary>
    /// 触发比例
    /// </summary>
    public int proportion = 0;

    private void Awake()
    {
        cdBar = Find<CircularHealthBar>("HealthBar_Background/HealthBar_Fill");
        bg_Image= Find<Image>("bg_Image");
        icon_Image = Find<Image>("icon_Image");
        number_Text = Find<Text>("icon_Image/number");
        proportion_Text= Find<Text>("proportion");
    }


    /// <summary>
    /// 初始化药品
    /// </summary>
    /// <param name="type">类型</param>
    /// <param name="key">名称</param>
    /// <param name="proportion">触发比例</param>
    /// <param name="number">数量</param>
    public void Init(medicineType type,string key,int _proportion,int number)
    {
        Type = type;
        proportion= _proportion;
        bg_Image.sprite = Resources.Load<Sprite>("medicine/" + type);
        icon_Image.sprite = UI.UI_Manager.I.GetEquipSprite("icon/", key);
        medicine_number= number;
        proportion_Text.text = proportion + "%";
        number_Text.text = medicine_number == -1 ? "0" : medicine_number.ToString();
        Init_Medicine(key);
    }
    /// <summary>
    /// 更新药品数量
    /// </summary>
    public void Clear()
    {
        icon_Image.gameObject.SetActive(false);
        medicine_number = -1;
        isUse = true;
        crt_cdtime = 0;
    }
    /// <summary>
    /// 字典初始化 药品字典和库存数量
    /// </summary>
    public void DicInit(string key, int _proportion, int number)
    {
        icon_Image.sprite = UI.UI_Manager.I.GetEquipSprite("icon/",  key);
        proportion = _proportion;
        medicine_number = number;
        number_Text.text = medicine_number == -1 ? "0" : medicine_number.ToString();
        proportion_Text.text = proportion + "%";
        Init_Medicine(key);
    }
    private void Init_Medicine(string key)
    {
        Bag_Base_VO bag = ArrayHelper.Find(SumSave.db_stditems, e => e.Name == key);
        int cd = 3;
        if (bag != null) cd = bag.job;
        Crt_medicine = new medicineVO(key, cd, Type, bag);
        cdBar.Init(Crt_medicine.medicine_cd);
    }
    /// <summary>
    /// 点击事件
    /// </summary>
    public void OnClick()
    {
        if (!isUse || medicine_number <= 0) return;
        On_Use_Effect();
        //使用药品效果
        StartCoroutine(Game_WaitTime(Crt_medicine.medicine_cd));

    }
    private IEnumerator Game_WaitTime(float time)
    {
        isUse = false;
        while (time > 0)
        {
            time -= 0.1f;
            crt_cdtime+= 0.1f;
            cdBar.ChangeHealth(crt_cdtime);
            yield return new WaitForSeconds(0.1f);
        }
        isUse = true;
        crt_cdtime = 0;
        cdBar.ChangeHealth(crt_cdtime);
    }
    /// <summary>
    /// 使用药品效果
    /// </summary>
    private void On_Use_Effect()
    {
        medicine_number--;
        number_Text.text = medicine_number.ToString();
        transform.parent.parent.parent.parent.SendMessage("Use_Medicine", Type);
    }
    /// <summary>
    /// 获取药品效果
    /// </summary>
    public Bag_Base_VO GetBag { get { return Crt_medicine.bag; } }

    /// <summary>
    /// 使用药品
    /// </summary>
    public void On_Use()
    {
        OnClick();
    }
}
