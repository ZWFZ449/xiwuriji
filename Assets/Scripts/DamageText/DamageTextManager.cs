using Common;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 战斗飘血
/// </summary>
public class DamageTextManager : MonoBehaviour// MonoSingleton <DamageTextManager>
{
    public static DamageTextManager Instance;

    private Color normalColor = Color.red;
    private Color volleyColor = Color.yellow;
    private Color readlyColor = Color.green;

    /// <summary>
    /// �˺��ı� ��ʹ���е��б�
    /// </summary>
    private Queue<GameObject> DamageTipsList;

    private int maxDamageTextNum = 100;
    private string ch = "-";


    /// <summary>
    /// 显示伤害文本,伤害字
    /// </summary>
    private GameObject damage_text,Image_text;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        DamageTipsList = new Queue<GameObject>();
        damage_text=Resources.Load<GameObject>("Prefabs/panel_text/damage_text");
        Image_text=Resources.Load<GameObject>("Prefabs/panel_text/Image_text");
        Init();
    }

    void Update()
    {

    }
    /// <summary>
    /// 清空对象池
    /// </summary>
    public void ClearAll()
    {
        DamageTipsList.Clear();
        Init();
    }

    public void Init()
    {
        GameObject parent = GameObject.FindWithTag("DamageTextPool");
        for (int i = 0; i < maxDamageTextNum; i++)
        {
            ResManger.LoadPrefabInstance("Prefabs/Show/DamageTips", (damageText) =>
            {
                DamageTipsList.Enqueue(damageText);
                damageText.SetActive(false);
            }, parent.transform);
        }
    }
  

    /// <summary>
    /// 显示伤害文本
    /// </summary>
    /// <param name="damageEnum"></param>
    /// <param name="damage"></param>
    /// <param name="parent"></param>
    public void ShowDamageText(DamageEnum damageEnum, string damage, Transform parent,float offset)
    {
        Color color = normalColor;
        switch (damageEnum)
        {
            case DamageEnum.普通伤害:
                color = normalColor;
                break;
            case DamageEnum.治疗伤害:
                color = volleyColor;
                break;
            case DamageEnum.真实伤害:
                color = readlyColor;
                break;
        }
        GameObject damageText = GetDamageTextFromPool();
        damageText.transform.position = parent.position;
        damageText.transform.SetParent(parent);
        char[] characters = (damageEnum + damage).ToCharArray();
        int max = Mathf.Max(characters.Length, damageText.transform.childCount);
        for (int i = 0; i < max; i++)// damageText.transform.childCount
        {
            if (damageText.transform.childCount <= i)
            { 
               Instantiate(Image_text, damageText.transform);
            }
            damageText.transform.GetChild(i).gameObject.SetActive(true);
            if (characters.Length > i)
            {
                damageText.transform.GetChild(i).GetComponent<Image>().sprite = UI.UI_Manager.I.GetEquipSprite("UI/base_bg/text/", characters[i].ToString()); ;
                damageText.transform.GetChild(i).GetComponent<Image>().color = color;
            }
            else damageText.transform.GetChild(i).gameObject.SetActive(false);


        }
        damageText.transform.GetOrAddComponent<DamageAnimiton>().Init(offset);

    }
   
    /// <summary>
    /// 从对象池中获取伤害文本
    /// </summary>
    private GameObject GetDamageTextFromPool()
    {
        if (DamageTipsList.Count <= 1)
        {
            GameObject parent = GameObject.FindWithTag("DamageTextPool");
            for (int i = 0; i < maxDamageTextNum; i++)
            {
                //ResManger.LoadPrefabInstance("Show/DamageTips", (damageText) =>
                //{
                //    DamageTipsList.Enqueue(damageText);
                //    damageText.SetActive(false);
                //}, parent.transform);
            }
        }

        GameObject damageText = DamageTipsList.Dequeue();

        damageText.SetActive(true);

        return damageText;
    }
    /// <summary>
    /// 将伤害文本放回对象池
    /// </summary>
    /// <param name="damageText"></param>
    public void ReturnDamageTextToPool(GameObject damageText)
    {
        damageText.SetActive(false);
        DamageTipsList.Enqueue(damageText);
        //damageText.GetComponent<DamageAnimiton>().
    }
}
