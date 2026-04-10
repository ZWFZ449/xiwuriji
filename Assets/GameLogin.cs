using TapSDK.Core;
using TapSDK.Login;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using System;
using Common;
using MVC;
using UI;
using TapSDK.Compliance;
using Components;

public class GameLogin : Singleton<GameLogin>
{
    protected override void Initialize()
    {
        // 核心配置
        TapTapSdkOptions coreOptions = new TapTapSdkOptions
        {
            //// 客户端 ID，开发者后台获取//习武日记
            //clientId = "diqpfrjikflwxbkm0z",
            //// 客户端令牌，开发者后台获取
            //clientToken = "6lbvS0IuoPFVm8t0SWh2BpOj0lwnLXf0f3ucc0hJ",


            // 客户端 ID，开发者后台获取//墨龙
            clientId = "99w80cd8fszpefza13",
            // 客户端令牌，开发者后台获取
            clientToken = "iLYHRXKbRCbHfyK79xtOeegwoNAeXd80ifIh4fzf",

            // 地区，CN 为国内，Overseas 为海外
            region = TapTapRegionType.CN,
            // 语言，默认为 Auto，默认情况下，国内为 zh_Hans，海外为 en
            preferredLanguage = TapTapLanguageType.zh_Hans,
            // 是否开启日志，Release 版本请设置为 false
            enableLog = true
        };
        // TapSDK 初始化
        TapTapSDK.Init(coreOptions);


        // 当需要添加其他模块的初始化配置项，例如合规认证、成就等， 请使用如下 API
        TapTapSdkBaseOptions[] otherOptions = new TapTapSdkBaseOptions[]
        {
            // 其他模块配置项
        };
        TapTapSDK.Init(coreOptions, otherOptions);
        AntiAddictionInit();//防沉迷初始化

    }
    public async Task Login()
    {
       
        //TapTapAccount account = await TapTapLogin.Instance.GetCurrentTapAccount();
        try
        {
            // 定义授权范围
            List<string> scopes = new List<string>
                {
                    TapTapLogin.TAP_LOGIN_SCOPE_PUBLIC_PROFILE,
                };
            var userInfo = await TapTapLogin.Instance.LoginWithScopes(scopes.ToArray());
            AntiAddiction();
            SumSave.Tapid = userInfo.openId;
            Crate_Tap(userInfo);
        }
        catch (TaskCanceledException)
        {
            Debug.Log("用户取消登录");
        }
        catch (Exception exception)
        {
            Debug.Log("错误提示" + exception.ToString());
            Debug.Log($"登录失败:{exception.Message}");
            if (exception is TapException tapError)  // using TapTap.Common
            {
                Debug.Log($"encounter exception:{tapError.code} message:{tapError.message}");
                if (tapError.code == (int)TapErrorCode.ERROR_CODE_BIND_CANCEL) // 取消登录
                {
                    Debug.Log("登录取消");
                }
            }
        }
        /*
        if (account == null)
        {
        }
        else
        {
            // 用户已登录
            Debug.Log("已登录");
            Crate_Tap(account);
            UI_Manager.Instance.GetPanel<panel_login>().ShowStartBtn(true);
            // 进入游戏
            SumSave.Tapid = account.openId;
        }
        */
    }    
    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="currentUser"></param>

    private void Crate_Tap(TapTapAccount userInfo)
    {
        string[] vs = new string[] { userInfo.openId, userInfo.name };
        Game_Omphalos.i.Crate_Accout(vs);
    }

    /// <summary>
    /// 防沉迷回调
    /// </summary>
    private Action<int, string> callback => (code, s) => {
        // do something
        if (code == 500)
        {
            Debug.Log("玩家未受到限制，正常进入游戏");
            UI_Manager.Instance.GetPanel<panel_login>().ShowStartBtn(true);
            //Crate_Par();
        }
        //else if (code == 1001)
        //{
        //    Debug.Log("用户点击切换账号，游戏应返回到登录页");
        //    Login();
        //}
        //else if (code == 1030)
        //{
        //    Debug.Log("用户当前时间无法进行游戏，此时用户只能退出游戏或切换账号");
        //}
        //else if(code== 1050)
        //{
        //    Debug.Log("用户无可玩时长，此时用户只能退出游戏或切换账号");
        //}
        //else if (code == 1100)
        //{
        //    Debug.Log("当前用户因触发应用设置的年龄限制无法进入游戏，该回调的优先级高于 1030，触发该回调无弹窗提示");
        //}
        //else if(code==1200)
        //{
        //    Debug.Log("数据请求失败，游戏需检查当前设置的应用信息是否正确及判断当前网络连接是否正常");
        //}
        else if (code == 9002)
        {
            Debug.Log("实名过程中点击了关闭实名窗，游戏可重新开始防沉迷认证");
            AntiAddiction();
        }
        else
        {
            Login();
        }
        
    };


    /// <summary>
    /// 防沉迷设置
    /// </summary>
    private void AntiAddictionInit()
    {
        // 核心配置
        TapTapSdkOptions coreOptions = new TapTapSdkOptions
        {
            //// 客户端 ID，开发者后台获取//习武日记
            //clientId = "diqpfrjikflwxbkm0z",
            //// 客户端令牌，开发者后台获取
            //clientToken = "6lbvS0IuoPFVm8t0SWh2BpOj0lwnLXf0f3ucc0hJ",


            // 客户端 ID，开发者后台获取//墨龙
            clientId = "99w80cd8fszpefza13",
            // 客户端令牌，开发者后台获取
            clientToken = "iLYHRXKbRCbHfyK79xtOeegwoNAeXd80ifIh4fzf",

            // 地区，CN 为国内，Overseas 为海外
            region = TapTapRegionType.CN,
            // 语言，默认为 Auto，默认情况下，国内为 zh_Hans，海外为 en
            preferredLanguage = TapTapLanguageType.zh_Hans,
            // 是否开启日志，Release 版本请设置为 false
            enableLog = true
        };
        // TapSDK 初始化
        TapTapSDK.Init(coreOptions);
        // TODO: coreOptions 应用参数配置

        // 合规认证配置
        TapTapComplianceOption complianceOption = new TapTapComplianceOption
        {
            showSwitchAccount = true,// 是否显示切换账号按钮
            useAgeRange = false//游戏是否需要获取真实年龄段信息
        };
        // 其他模块配置项
        TapTapSdkBaseOptions[] otherOptions = new TapTapSdkBaseOptions[]
        {
            complianceOption
           
        };
        TapTapCompliance.RegisterComplianceCallback(callback);
        // TapSDK 初始化
        TapTapSDK.Init(coreOptions, otherOptions);

    }
    /// <summary>
    /// 发启防沉迷检查
    /// </summary>
    private async void AntiAddiction()
    {
        TapTapAccount account = await TapTapLogin.Instance.GetCurrentTapAccount();
        if (account != null)
        {
            string userIdentifier = account.unionId;
            TapTapCompliance.Startup(userIdentifier);
        }
        else
        {
            Debug.Log("当前用户未登录");
        }
        
    }

}


