using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;
public class Dream_Panel_Login : PanelBase
{
    private const string lastServer = "选区";

    private const string user_password = "账号";

    private Button loginBt;
    /// <summary>
    /// 服务器obg
    /// </summary>
    private Transform TheServerObg;
    /// <summary>
    /// 服务器列表
    /// </summary>
    private Transform TheServerList;
    /// <summary>
    /// 当前选择的服务器
    /// </summary>
    private Text TheServerText;
    /// <summary>
    /// 确定选择的服务器
    /// </summary>
    private Button TheServerUP;
    /// <summary>
    /// 按钮预制体
    /// </summary>
    private btn_item btn_Item;
    /// <summary>
    /// 当前选择的服务器
    /// </summary>
    private btn_item select_par;
    /// <summary>
    /// 记住上一次登录的服务器
    /// </summary>
    private int lastServer_Index;
    /// <summary>
    /// 服务器列表
    /// </summary>
    private List<btn_item> select_par_list = new List<btn_item>();
    /// <summary>
    /// 记录开区状态
    /// </summary>
    private Dictionary<int, bool> open_pars = new Dictionary<int, bool>();

    private void Start()
    {
        if (!PlayerPrefs.HasKey(user_password))//创建默认id
        {
            Alert_Dec.Show("创建账号成功");
            SumSave.uid = Guid.NewGuid().ToString("N");
            PlayerPrefs.SetString(user_password, SumSave.uid);
        } 
        else
            SumSave.uid = PlayerPrefs.GetString(user_password);
        SendNotification(NotiList.Read_Instace);
        StartCoroutine("Read_Instace");
    }

    private IEnumerator Read_Instace()

    {
        while (SumSave.db_pars == null)
        {
            yield return new WaitForSeconds(1f);
            SendNotification(NotiList.Read_Instace);
            Alert_Dec.Show("网络链接中断，请重试");
        }
    }
    public override void Initialize()
    {
        base.Initialize();
        btn_Item = Tool_UI.Find_Prefabs<btn_item>("btn_item");
        TheServerObg = Find<Transform>("TheServer");
        TheServerObg.gameObject.SetActive(false);
        TheServerList = Find<Transform>("TheServer/TheServerList/Viewport/Content");
        TheServerText = Find<Text>("TheServer/CurrentTheServer/TheServerText/Text");
        //选区进入游戏
        TheServerUP = Find<Button>("TheServer/CurrentTheServer/TheServerUP");
        TheServerUP.onClick.AddListener(OnLoginClick);
        //登录
        loginBt = Find<Button>("btn_login");
        loginBt.onClick.AddListener(Open_function);
     

#if UNITY_EDITOR

#elif UNITY_ANDROID
        
           
#elif UNITY_IPHONE
        
#endif
      

    }

    private void Open_function()
    {
        UpTheServer();
    }

    /// <summary>
    /// 打开服务器选择界面
    /// </summary>
    public void UpTheServer()
    {
        if (SumSave.db_pars == null)
        {
            Alert_Dec.Show("网络链接中断，请重试");
            return;
        }
        TheServerObg.gameObject.SetActive(true);
        ClearObject(TheServerList);
        int device = 1;
#if UNITY_EDITOR
        device = 1;
#elif UNITY_ANDROID
            device = 1;
#elif UNITY_IPHONE
            device = 2;
#endif
        for (int i = 0; i < SumSave.db_pars.Count; i++)
        {
            if (SumSave.db_pars[i].device == device)
            {
                if (!open_pars.ContainsKey(SumSave.db_pars[i].index))
                {
                    open_pars.Add(SumSave.db_pars[i].index, false);
                }
                btn_item item = Instantiate(btn_Item, TheServerList);
                string into = SumSave.db_pars[i].par_name;
                if (SumSave.nowtime < SumSave.db_pars[i].opentime)
                {
                    int time = Battle_Tool.SettlementTransport(SumSave.db_pars[i].opentime.ToString(), SumSave.nowtime.ToString(), 2);
                    StartCoroutine(wait_par(time, SumSave.db_pars[i], item));
                }
                else
                {
                    into += "\n开区:" + SumSave.db_pars[i].opentime;
                    item.Show(SumSave.db_pars[i].index, into);
                    open_pars[SumSave.db_pars[i].index] = true;
                }
                item.GetComponent<Button>().onClick.AddListener(() => { SelectPar(item); });
                select_par_list.Add(item);
            }
        }
        if (PlayerPrefs.HasKey(lastServer))
        {
            lastServer_Index = PlayerPrefs.GetInt(lastServer);
            foreach (var item in select_par_list)
            {
                if (item.index == lastServer_Index)
                {
                    select_par = item;
                    db_base_par par = ArrayHelper.Find(SumSave.db_pars, e => e.index == item.index);
                    TheServerText.text = "选中" + par.par_name;
                    SumSave.par = select_par.index;
                }
            }
        }
    }
    /// <summary>
    /// 等待开区
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator wait_par(int time, db_base_par par, btn_item item)
    {
        string dec = par.par_name;
        dec += "\n倒计时:" + ConvertSecondsToHHMMSS(time);
        item.Show(par.index, dec);
        while (time > 0)
        {
            yield return new WaitForSeconds(1f);
            time--;
            dec = par.par_name;
            dec += "\n倒计时:" + ConvertSecondsToHHMMSS(time);
            item.Show(par.index, dec);
        }
        open_pars[par.index] = true;
        item.Show(par.index, par.par_name + "\n开区:" + par.opentime);

    }
    /// <summary>
    /// 点击选择服务器
    /// </summary>
    /// <param name="i"></param>
    private void SelectPar(btn_item item)
    {
        db_base_par par = ArrayHelper.Find(SumSave.db_pars, e => e.index == item.index);
        if (par != null && open_pars.ContainsKey(par.index))
        {
            Alert_Dec.Show("选择了" + (par.par_name));
            select_par = item;
            TheServerText.text = "选中 " + (par.par_name);
            SumSave.par = item.index;
        }
        else Alert.Show("系统错误", "请联系管理员qq386246268");
    }
    /// <summary>
    /// 点击登录
    /// </summary>
    private void OnLoginClick()//登录点击
    {
        if (select_par == null)
        {
            Alert_Dec.Show("请先选择服务器");
            return;
        }
        if (!open_pars.ContainsKey(select_par.index))
        {
            Alert_Dec.Show("当前服务器暂未开启");
            return;
        }

        if (!open_pars[select_par.index])
        {
            Alert_Dec.Show("当前服务器暂未开启");
            return;
        }
        Login();
    }
    /// <summary>
    /// 确认登录
    /// </summary>
    public void Login()
    {
        TheServerObg.gameObject.SetActive(false);
        if (SumSave.uid != null)
        {
            PlayerPrefs.SetInt(lastServer, select_par.index);
            SendNotification(NotiList.User_Login);
            UI_Manager.I.GetPanel<PanelMian>().Show();
            Hide();
        }

    }
    public override void Hide()
    {
        ////计算离线收益
        offline();
        base.Hide();
    }
    /// <summary>
    /// 获取离线积分
    /// </summary>
    private void offline()
    {
        //过了多少秒
        //int number = (int)(SumSave.nowtime - SumSave.crt_resources.now_time).TotalSeconds;

    }
    public override void Show()
    {
        base.Show();
    }
}
