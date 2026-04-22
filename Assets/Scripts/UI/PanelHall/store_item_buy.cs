using Common;
using Components;
using MVC;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class store_item_buy : Base_Mono
{
    private Transform m_icon_brom;
    private InputField inputField;
    private TMP_Text buy_item_Title;
    private Button buy_btn;
    private TMP_Text buy_text;
    private material_item material_item_Prefabs;
    private db_store_vo buy_title;

    private void Awake()
    {
        inputField = Find<InputField>("InputField");
        buy_item_Title = Find<TMP_Text>("buy_item_Title/Title/info");
        buy_btn = Find<Button>("buy_btn");
        buy_text = Find<TMP_Text>("buy_text/info");
        m_icon_brom = Find<Transform>("icon");
        material_item_Prefabs = Tool_UI.Find_Prefabs<material_item>("material_item");
        inputField.onEndEdit.AddListener(OnInputChanged);//监听输入框
        buy_btn.onClick.AddListener(BuyItem);//监听购买按钮
    }

    private void BuyItem()
    {
        int number= int.Parse(inputField.text);
        if (number <= 0) return;
        if (buy_title.ItemMaxQuantity != -1)
        { 
            int maxnumber = SumSave.crt_signin.GetIsValue(buy_title.ItemName);
            if (maxnumber >= buy_title.ItemMaxQuantity) { Alert_Dec.Show("今日购买数量已满") ; return; }
            number = Mathf.Min(number, buy_title.ItemMaxQuantity - maxnumber);
        }
        List<long> Units = SumSave.crt_user_unit.Set();
        currency_unit unit = (currency_unit)System.Enum.Parse(typeof(currency_unit), buy_title.unit);
        long price = number * buy_title.ItemPrice;
        if (price <= 0) return;
        if (Units[(int)unit] >= price)
        {
            if (buy_title.ItemMaxQuantity != -1)
            {
                SumSave.crt_signin.SetIsValue(buy_title.ItemName, number);
            }
            Battle_Tool.Dream_Obtain_Unit(unit, -price, Obtain_Int.Add_unit(-price));
            if (buy_title.ItemName == "灵宠")
            {
                db_vip crt_vip = Tool_Battle.Obtain_Vip();
                pet_list pet= (pet_list)Random.Range(0, (int)pet_list.麋鹿);
                if (crt_vip == null)
                {
                    pet = pet_list.麋鹿;
                }
                else pet = ((pet_list)(crt_vip.vip_lv - 1));
                for (int i = 0; i < number; i++)
                {
                    SumSave.crt_pet.AddPet(pet);
                }
                Alert_Dec.Show("购买成功获得 " + buy_title.ItemName + " * " + number);
            }
            else
            {
                int random = Random.Range(1, 1000);
                int maxnumber = number + Random.Range(1, 1000);
                Battle_Tool.Dream_Obtain_Resources(Obtain_Int.Add(1, buy_title.ItemName, new int[] { number + random, random }), maxnumber);
                Alert_Dec.Show("购买成功获得 " + buy_title.ItemName + " * " + number);
            }
            
            Hide();
        }
        else Alert_Dec.Show("购买失败," + buy_title.unit + "不足");
        //if (buy_title.unit == currency_unit.金币.ToString())
        //{ 
        //    long price =  number * buy_title.ItemPrice;
        //    if (price <= 0) return;
        //    if (Units[(int)currency_unit.金币] >= price)
        //    {
        //        Battle_Tool.Dream_Obtain_Unit(currency_unit.金币, -price, Obtain_Int.Add_unit(-price));
        //        int random = Random.Range(1, 1000);
        //        int maxnumber = number + Random.Range(1, 1000);
        //        Battle_Tool.Dream_Obtain_Resources(Obtain_Int.Add(1, buy_title.ItemName, new int[] { number + random, random }), maxnumber);
        //        Alert_Dec.Show("购买成功");
        //        Hide();
        //    }
        //    else Alert_Dec.Show("购买失败," + buy_title.unit + "不足");

        //}
    }
    /// <summary>
    /// 关闭
    /// </summary>
    private void Hide()
    { 
        gameObject.SetActive(false);
    }

    private void OnInputChanged(string arg0)
    {
        int number = int.Parse(arg0);
        if (number <= 0) inputField.text = "1";

    }
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="title"></param>
    public void Init(db_store_vo title)
    {
        buy_title = title;
        ClearObject(m_icon_brom);
        string name= title.ItemName;
        if (title.ItemName == "灵宠")
        {
            db_vip crt_vip = Tool_Battle.Obtain_Vip();
            if (crt_vip == null)
            {
                name = pet_list.麋鹿.ToString();
            }
            else name = ((pet_list)(crt_vip.vip_lv - 1)).ToString();
        }
        Instantiate(material_item_Prefabs, m_icon_brom).Init((name, 1));
        buy_item_Title.text = title.ItemName;
        buy_text.text = Tool_State.Show_Stditems(title.ItemName) + "\n购买单价: " + title.ItemPrice + " " + title.unit;
        if (title.ItemMaxQuantity != -1)
        { 
            buy_text.text += "\n购买数量: " + title.ItemMaxQuantity;
        }


    }
}
