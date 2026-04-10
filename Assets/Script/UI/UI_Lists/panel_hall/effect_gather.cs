using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class effect_gather : Base_Mono
{
    public Transform crt_interval;

    public RefiningDemonsinterval_item color_item_Prefabs;

    private Slider slider;

    private float slider_speed = 1f;

    private List<int> receive_list = new List<int>();

    private List<int> interval_list = new List<int>();

    private Dictionary<string, int> keys = new Dictionary<string, int>();
    /// <summary>
    /// 极品次数
    /// </summary>
    private int success_Number = 0;

    private Button gather;

    /// <summary>
    /// 标准时间
    /// </summary>
    private static float base_time = 30f;
    /// <summary>
    /// 标准次数
    /// </summary>
    private static int base_number = 5;
    /// <summary>
    /// 限制次数
    /// </summary>
    private int LimitNumber = base_number;
    /// <summary>
    /// 显示时间
    /// </summary>
    private Text info_time, info_number, info_base_time;



    protected void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        gather = Find<Button>("effect/gather");
        gather.onClick.AddListener(Refining_Gather);
        info_time = Find<Text>("info_time");
        info_number = Find<Text>("effect/gather/text");
        slider = Find<Slider>("effect/show_interval/Slider");

    }
    /// <summary>
    /// 打开开关
    /// </summary>
    public void OpenEffect_Gather(int number)
    {
        StopAllCoroutines();
        success_Number = 0;
        LimitNumber = number;
        getlist();
        StartCoroutine(Read_time(base_time+ number*5f));
    }

    private void getlist()
    {
        interval_list = new List<int>() { 3, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 };

        receive_list.Clear();

        info_number.text = "采集(" + LimitNumber + "次)";

        for (int i = crt_interval.childCount - 1; i >= 0; i--)
        {
            Destroy(crt_interval.GetChild(i).gameObject);
        }

        for (int i = 0; i < 20; i++)
        {
            RefiningDemonsinterval_item item = Instantiate(color_item_Prefabs, crt_interval);

            int random = UnityEngine.Random.Range(0, interval_list.Count);

            item.show_color(i, interval_list[random]);

            receive_list.Add(interval_list[random]);

            interval_list.RemoveAt(random);
        }

        slider.maxValue = 720;
        slider.value = 0;
        slider_speed = 2f;
    }

    private IEnumerator Read_time(float time)
    {
        while (time > 0)
        {
            info_time.text = "炼丹剩余时间" + time + "s";

            yield return new WaitForSeconds(1f);
            time -= 1;
        }
        Obtain_Other_Receive();
    }

    private void Obtain_Other_Receive()
    {
        Alert_Dec.Show("本次炼丹结束");
        transform.parent.SendMessage("Confirm_Number", success_Number);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            if (slider.value >= slider.maxValue) slider.value = 0;

            else
                slider.value += slider_speed;
        }
    }

    /// <summary>
    /// 采集结果
    /// </summary>
    private void Refining_Gather()
    {
        slider_speed += Random.Range(1, 5);
        if (LimitNumber <= 0)
        {
            Obtain_Other_Receive();
            return;
        }
        LimitNumber--;
        int index = -1;
        index = (int)(slider.value / 36);
        if (index != -1)
        {
            index = (int)MathF.Min(index, receive_list.Count - 1);

            int interval_receive = receive_list[index];
            if (interval_receive == 3)
            {
                success_Number+=2;
                Alert_Dec.Show("注灵成功,获得极品值+2");
            }
            if (interval_receive == 2)
            {
                success_Number += 1;
                Alert_Dec.Show("注灵成功,获得极品值+1");
            }
        }

        if(success_Number>10)///添加炼丹极品值限制
        {
            success_Number= 10;
        }

        info_number.text = "注灵(" + LimitNumber + "次)";

        if (LimitNumber <= 0)
        {
           
        }
        else getlist();

    }

    private string Obtain_Receive(string obtain_name, int number)
    {
        number = Mathf.Max(1, number);

        if (!keys.ContainsKey(obtain_name)) keys.Add(obtain_name, number);

        else keys[obtain_name] += number;

        return "获得" + Show_Color.Yellow(obtain_name) + " * " + number;

    }

    public override void Show()
    {
        base.Show();
    }

    public void Hide()
    {

    }
}

