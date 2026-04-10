using Common;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class panel_Mian : Panel_Base
{
    /// <summary>
    /// 存储面板列表
    /// </summary>
    private Dictionary<string, GameObject> offect_list_dic = new Dictionary<string, GameObject>();

    private Image offect_list;
    /// <summary>
    /// 消息飘窗显示文本
    /// </summary>
    private Text BayWindowText;
    /// <summary>
    /// 消息飘窗编号
    /// </summary>
    private int BayWindow_index = 0;
    /// <summary>
    /// 刷新飘窗按钮
    /// </summary>
    private Button BayWindow_btn;
    /// <summary>
    /// 滚动视图窗口
    /// </summary>
    private ScrollRect scrollRect;

    private Button file;
    public override void Hide()
    {
        if (offect_list.gameObject.activeInHierarchy)
        {
            for (int i = offect_list.transform.childCount - 1; i >= 1; i--)//关闭区域内的UI
            {
                offect_list.transform.GetChild(i).gameObject.SetActive(false);
            }
            offect_list.gameObject.SetActive(false);
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        closeButton=Find<Button>("offect_list/close_button");
        closeButton.onClick.AddListener(() => { Hide(); });
        scrollRect = Find<ScrollRect>("BayWindow/BayWindoText");
        offect_list = Find<Image>("offect_list");
        BayWindowText = Find<Text>("BayWindow/BayWindoText/Viewport/came/Text");
        BayWindow_btn = Find<Button>("BayWindow/Button");
        BayWindow_btn.onClick.AddListener(Read_prizedraw_info);
        SendNotification(NotiList.Read_Message_Window);
        Obtain_PrizeDraw_info();
        StartCoroutine(AutoScroll());
        InvokeRepeating("Read_Message_Window", 600, 600);
        file = Find<Button>("base_info/bg/bg/file");
        file.onClick.AddListener(() => { Open_File(); });
        if (SumSave.crt_settingold.user_setting[4] == 1)//1为静音
        {
            AudioListener.pause = true;
            AudioManager.Instance.audioSource.Stop();
        }
        else
        {
            AudioListener.pause = false;
        }

    }
    /// <summary>
    /// 立刻存档
    /// </summary>
    private void Open_File()
    {
        Game_Omphalos.i.archive();
    }

    /// <summary>
    /// 10分钟读取一次
    /// </summary>
    private void Read_Message_Window()
    {
        SendNotification(NotiList.Read_Message_Window);
    }

    /// <summary>
    /// 文本自动滚动
    /// </summary>
    /// <returns></returns>
    private IEnumerator AutoScroll()
    {
        while (true)
        {
            // 横向垂直滚动
            scrollRect.horizontalNormalizedPosition -= 0.001f;
            yield return new WaitForSeconds(0.005f);

            // 如果滚动到底部，重置位置
            if (scrollRect.horizontalNormalizedPosition <= 0)
            {
                scrollRect.horizontalNormalizedPosition = 1;
                Obtain_PrizeDraw_info();
            }
        }
    }

    /// <summary>
    /// 调用抽取列表
    /// </summary>
    private void Obtain_PrizeDraw_info()
    {
        if (SumSave.crt_message_window.Count > 0)
        {
            BayWindowText.text = SumSave.crt_message_window[Random.Range(0, SumSave.crt_message_window.Count)].Item3;
        }
    }

    /// <summary>
    /// 随机数据
    /// </summary>
    private void Read_prizedraw_info()
    {
        if (SumSave.crt_message_window.Count > 0)
            BayWindowText.text = SumSave.crt_message_window[BayWindow_index].Item3;
        else Obtain_PrizeDraw_info();
    }

    /// <summary>
    /// 自动跳转
    /// </summary>
    protected void Read_prizedraw()
    {
        if (SumSave.crt_message_window.Count == 0)
        {
            Obtain_PrizeDraw_info();
            return;
        }
        BayWindow_index++;
        if (BayWindow_index > SumSave.crt_message_window.Count) BayWindow_index = 0;
        BayWindowText.text = SumSave.crt_message_window[BayWindow_index].Item3;

    }



    /// <summary>
    /// 子物品打开资源提升开关
    /// </summary>
    /// <param name="item"></param>
    protected void OnClickMap(string map)
    {
        Show_GameObject(map, true);
    }

    /// <summary>
    /// 打开开关
    /// </summary>
    /// <param name="index"></param>
    /// <param name="active"></param>
    private void Show_GameObject(string index, bool active)
    {
        offect_list.gameObject.SetActive(true);
        foreach (var item in offect_list_dic.Keys)
        {
            offect_list_dic[item].SetActive(false);
        }
        if (!offect_list_dic.ContainsKey(index))
        {
            GameObject obj = Resources.Load<GameObject>("Prefabs/panel_hall/" + index);
            obj = Instantiate(obj, offect_list.transform);
            offect_list_dic.Add(index, obj);
        }
        offect_list_dic[index].SetActive(active);
        if (active) offect_list_dic[index].GetComponent<Base_Mono>().Show();
    }


    public override void Show()
    {
        base.Show();
        offect_list.gameObject.SetActive(false);
    }

    protected override void Awake()
    {
        Initialize();
    }
}
