
using Common;
using Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MVC
{
    public class panel_login : Panel_Base    
    {
        private const string lastServer = "选区";

        private Button loginBt, userBt, selfBt;//开始按钮, 用户，隐私

        private GameObject AgreementButter, AgreementWindow, userWd, selfWd;//协议点击组件，协议面板

        private Text titleText;//标题

        private  Toggle Toggle;//协议确定开关

        private panel_fight fightPanel;
        /// <summary>
        /// Tap登录按钮
        /// </summary>
        private  Button TaploginBt;
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
        private List<btn_item> select_par_list=new List<btn_item>();
        /// <summary>
        /// 习武日记背景，墨龙背景
        /// </summary>
        private Transform bg_xwrj, bg_molong;
        /// <summary>
        /// 是否打开服务器
        /// </summary>
        private bool open_par = false;
        /// <summary>
        /// 记录开区状态
        /// </summary>
        private Dictionary<int,bool> open_pars= new Dictionary<int, bool>();
   


        private void Start()
        {
            SendNotification(NotiList.Read_Instace);

            StartCoroutine("Read_Instace");
        }

        private IEnumerator Read_Instace()
        {
            while (SumSave.olddb_monsters==null)
            {
                yield return new WaitForSeconds(1f);
                SendNotification(NotiList.Read_Instace);
                Alert_Dec.Show("网络链接中断，请重试");
            }
        }
        public override void Initialize()
        {
            base.Initialize();
            TheServerObg= Find<Transform>("TheServer");
            TheServerObg.gameObject.SetActive(false);
            TheServerList=Find<Transform>("TheServer/TheServerList/Viewport/Content");
            TheServerText=Find<Text>("TheServer/CurrentTheServer/TheServerText/Text");
            TheServerUP=Find<Button>("TheServer/CurrentTheServer/TheServerUP");
            btn_Item= Battle_Tool.Find_Prefabs<btn_item>("btn_item");// Resources.Load<btn_item>("Prefabs/base_tool/btn_item"); 
            TheServerUP.onClick.AddListener(OnLoginClick);

            loginBt = Find<Button>("login");
            loginBt.onClick.AddListener(Open_function);
            loginBt.gameObject.SetActive(false);
            fightPanel = UI_Manager.I.GetPanel<panel_fight>();
            TaploginBt=Find<Button>("Taplogin");
            TaploginBt.onClick.AddListener(TapLogin);
            bg_xwrj = Find<Transform>("bg_xwrj");
            bg_molong = Find<Transform>("bg_molong");

#if UNITY_EDITOR
            TaploginBt.gameObject.SetActive(false);
            loginBt.gameObject.SetActive(true);
            bg_xwrj.gameObject.SetActive(false);
            bg_molong.gameObject.SetActive(true);

#elif UNITY_ANDROID

            TaploginBt.gameObject.SetActive(true);//true
            loginBt.gameObject.SetActive(false);
            bg_xwrj.gameObject.SetActive(false);
            bg_molong.gameObject.SetActive(true);
            //bg_xwrj.gameObject.SetActive(true);
            //bg_molong.gameObject.SetActive(false);
#elif UNITY_IPHONE
            TaploginBt.gameObject.SetActive(false);
            loginBt.gameObject.SetActive(true);

            bg_xwrj.gameObject.SetActive(false);
            bg_molong.gameObject.SetActive(true);
#endif
            #region 用户协议

            AgreementButter = GameObject.Find("AgreementButter");
            if (AgreementButter == null)
            {
                Debug.Log("协议组件为空");
                return;
            }
            userBt = AgreementButter.transform.Find("user").GetComponent<Button>();
            selfBt = AgreementButter.transform.Find("self").GetComponent<Button>();
            userBt = Find<Button>("AgreementButter/user");
            userBt.onClick.AddListener(OpenUser);
            selfBt = Find<Button>("AgreementButter/self");
            selfBt.onClick.AddListener(OpenSelf);

            AgreementWindow = GameObject.Find("AgreementWindow");
            userWd = AgreementWindow.transform.Find("userWd").gameObject;
            selfWd = AgreementWindow.transform.Find("selfWd").gameObject;

            titleText = AgreementWindow.transform.Find("Title").GetComponentInChildren<Text>();

            Toggle = AgreementButter.transform.Find("Toggle").GetComponent<Toggle>();
            Toggle.isOn = PlayerPrefs.GetInt("同意阅读协议", 0) == 1;
            OpenUser();
            #endregion

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
            if (SumSave.db_pars==null)
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
            for (int i=0;i<SumSave.db_pars.Count; i++)
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
        private IEnumerator wait_par(int time,db_base_par par, btn_item item)
        {
            string dec = par.par_name;
            dec += "\n倒计时:" + ConvertSecondsToHHMMSS(time);
            item.Show(par.index, dec);
            while (time>0)
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
            if (par!=null && open_pars.ContainsKey(par.index))
            {
                Alert_Dec.Show("选择了" + (par.par_name));
                select_par = item;
                TheServerText.text = "选中 " + (par.par_name);
                SumSave.par = item.index;
            }
            else Alert.Show("系统错误", "请联系管理员qq386246268");
        }

        //tap登录完成之后打开开关
        public void ShowStartBtn(bool v)
        {
            TaploginBt.gameObject.SetActive(!v);
            loginBt.gameObject.SetActive(v);
        }

        /// <summary>
        /// Tap登录
        /// </summary>
        private void TapLogin()
        {
          
            if (!Toggle.isOn)
            {
                Alert_Dec.Show("请先阅读并勾选同意协议");
                Debug.Log("请先阅读并勾选同意协议");
                PlayerPrefs.SetInt("同意阅读协议", 0);
                return;
            }
#if UNITY_EDITOR
            TaploginBt.gameObject.SetActive(false);
            loginBt.gameObject.SetActive(true);
#elif UNITY_ANDROID
            _ = GameLogin.Instance.Login();
#elif UNITY_IPHONE

#endif
        }

        private void OpenUser()//打开用户协议
        {
            if (IsAgreementWdNull())
            {
                titleText.text = "用户协议";
                selfWd.SetActive(false);
                userWd.SetActive(true);
            }
        }

        private void OpenSelf()//打开隐私协议
        {
            if (IsAgreementWdNull())
            {

                titleText.text = "隐私协议";
                userWd.SetActive(false);
                selfWd.SetActive(true);
            }
        }

        private bool IsAgreementWdNull()//判断协议是否为空
        {
            if (selfWd != null && userWd != null && titleText != null && AgreementWindow != null)
            {
                AgreementWindow.SetActive(true);
                return true;
            }
            else
            {
                Debug.LogError("协议窗口为空");
                return false;
            }
        }

        /// <summary>
        /// 点击登录
        /// </summary>
        private void OnLoginClick()//登录点击
        {
            if (select_par==null)
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
#if UNITY_EDITOR
            //SumSave.ios_account_number = "admin001";
#elif UNITY_ANDROID
            if (!open_pars.ContainsKey(select_par.index)) {
                Alert_Dec.Show("当前服务器暂未开启");
                return;
            }

            if (!open_pars[select_par.index])
            {
                Alert_Dec.Show("当前服务器暂未开启");
                return;
            }
       
           
#elif UNITY_IPHONE
            
             if (!open_pars.ContainsKey(select_par.index)) {
                Alert_Dec.Show("当前服务器暂未开启");
                return;
            }

            if (!open_pars[select_par.index])
            {
                Alert_Dec.Show("当前服务器暂未开启");
                return;
            }

#endif

            if (!Toggle.isOn)
            {
                Alert_Dec.Show("请先阅读并勾选同意协议");
                Debug.Log("请先阅读并勾选同意协议");
                PlayerPrefs.SetInt("同意阅读协议", 0);
                return;
            }
            PlayerPrefs.SetInt("同意阅读协议", 1);
#if UNITY_EDITOR
            #region ios区 
            SumSave.uid = "DSFSDFSDFSDF3"; // 32ac681e5ca845dc9eda67b30a36f6fa
            //SumSave.uid = "e8735c45680048f8a3494d7107a9e5a5";//都做了土（ip） 
            //SumSave.uid = "399be5eb64f84f01b2e4817bba58a4e3";//殇璃（ip） 
            //SumSave.uid = "0f0d1e98f9ad480c852068b2a98eab6f";//魔男（ip） 
            #endregion
            #region 安卓区
            //SumSave.uid = "DSFSDFSDFSDF3";//

            //SumSave.uid = "95b1d23b8a394ee4a0921c775668b81b";//自古圣贤皆寂寞(Tap id)

            //SumSave.uid = "46d188a5694a483aa2ad2f990ccc4449";//帝天 1区

            //SumSave.par = 1;
            #endregion



            Login();
            //UI_Manager.Instance.GetPanel<Panel_cratehero>().Show();
#elif UNITY_ANDROID
            Game_Omphalos.i.Wirte_Tap();
#elif UNITY_IPHONE
            UI_Manager.Instance.GetPanel<Panel_cratehero>().Show();
#endif
        }
        /// <summary>
        /// 确认登录
        /// </summary>
        public void Login()
        {
#if UNITY_EDITOR
            //UI_Manager.Instance.GetPanel<Panel_cratehero>().Hide();

#elif UNITY_ANDROID
                _ = GameLogin.Instance.Login();

#elif UNITY_IPHONE
            UI_Manager.Instance.GetPanel<Panel_cratehero>().Hide();
#endif
            TheServerObg.gameObject.SetActive(false);

           
            if (SumSave.uid != null)
            {
                PlayerPrefs.SetInt(lastServer, select_par.index);
                SendNotification(NotiList.User_Login);
                UI_Manager.I.GetPanel<panel_Mian>().Show();
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
            int number = (int)(SumSave.nowtime - SumSave.crt_resources.now_time).TotalSeconds;
            if (number > 300)
            {
                int maxnumber = 3600 * 2;
                (int,int,string) exp = SumSave.crt_accumulatedrewards.SetRecharge();
                if (exp.Item1 > 0)
                {
                    for (int i = SumSave.db_vip_list.Count - 1; i >= 0; i--)
                    {
                        if (exp.Item1 == SumSave.db_vip_list[i].vip_lv)
                        {
                            maxnumber += SumSave.db_vip_list[i].offlineInterval * 3600;
                            break;
                        }
                    }
                }
                //记录离线收益
                string dec = "离线时长 " + ConvertSecondsToHHMMSS(number)
                    + "\n有效时长 " + (maxnumber > number ? ConvertSecondsToHHMMSS(number) : ConvertSecondsToHHMMSS(maxnumber));
                number = Math.Clamp(number, 0, maxnumber);
                Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_value, SumSave.crt_resources.Set_Uptade_String(), SumSave.crt_resources.Get_Update_Character());
                Battle_Tool.Obtain_Unit(currency_unit.离线积分, number / 30,2);
                dec+="\n获得离线积分 "+ number / 30;
                for (int i = 0; i < SumSave.crt_Trial_Tower_rank.lists.Count; i++)
                {
                    if (SumSave.crt_Trial_Tower_rank.lists[i].Item1 == SumSave.crt_user.uid)
                    {
                        int value = (int)SumSave.crt_Trial_Tower_rank.lists[i].Item3;
                        int value_2 = number / 60 * value;
                        Battle_Tool.Obtain_Unit(currency_unit.试炼积分, value_2, 2);
                        dec += "\n获得试炼积分 " + value_2;
                        break;
                    }
                }
                if (SumSave.crt_world != null)
                {
                    int value = SumSave.db_lvs_old.world_offect_list[SumSave.crt_world.World_Lv];
                    int value_2 = number / 60 * value;
                    Battle_Tool.Obtain_Unit(currency_unit.灵气, value_2, 2);
                    dec += "\n获得灵气 " + value_2;
                }
                if (SumSave.crt_resources.user_map_index != "")
                {
                    user_map_vo map = ArrayHelper.Find(SumSave.read_lose_map, e => e.map_name == SumSave.crt_resources.user_map_index);
                    if (map != null)
                    {
                        float numbers = 7;
                        if (SumSave.crt_MaxHero_okd.totalPower < map.map_index * 10000)
                        {
                            numbers += SumSave.crt_MaxHero_okd.totalPower * 3 / (map.map_index * 10000f);
                        }
                        numbers = Math.Clamp(numbers, 7, 20);
                        int monster_number= (int)(number / numbers);
                        if (Tool_State.IsState(State_List.至尊卡))
                        {
                            //离线至尊积分进度条 
                            if (SumSave.crt_settingold.user_setting[7] == 0) Combat_statistics.offline(monster_number);
                            int unit_2 = Random.Range(monster_number, monster_number * 2);
                            //离线历练值
                            Battle_Tool.Obtain_Unit(currency_unit.元宝, unit_2,2);
                            dec += "\n历练收益 " + unit_2 + Show_Color.Red(" (+" + (long)(unit_2 * Show_Buff(enum_skill_attribute_list.人物历练) / 100) + ")");
                        }
                       
                        List<string> vs = ConfigBattle.Offline(monster_number);
                        crtMaxHeroVO monster = ArrayHelper.Find(SumSave.olddb_monsters, e => e.index == map.map_index);
                        dec += "\n地图 " + map.map_name + " 收益";
                        if (monster != null)
                        {
                            int moeny = (monster.Lv * 5 + 1) * monster_number;
                            Battle_Tool.Obtain_Unit(currency_unit.金币, moeny,2);
                            dec += "\n灵珠收益 " + moeny + Show_Color.Red(" (+" + (int)(moeny * Show_Buff(enum_skill_attribute_list.灵珠收益)/100) + ")");
                            long obexp = (long)(monster.Exp * monster_number * 0.6);
                            Battle_Tool.Obtain_Exp(obexp);
                            dec += "\n经验收益 " + obexp + Show_Color.Red(" (+" + (long)(obexp * Show_Buff(enum_skill_attribute_list.经验加成)/100) + ")");
                        }
                        for (int i = 0; i < vs.Count; i++)
                        {
                            dec += "\n" + vs[i];
                        }
                    }
                }
                Alert.Show("离线收益", dec);

            }
        }
        private float Show_Buff(enum_skill_attribute_list index)
        {
            float value = 0;
            if ((int)index < SumSave.crt_MaxHero_okd.bufflist.Count)
                value += SumSave.crt_MaxHero_okd.bufflist[(int)index];
            return value;
        }
        public override void Show()
        {
            base.Show();
        }
    }


}
