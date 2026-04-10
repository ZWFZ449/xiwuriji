using TarenaMVC;
using UnityEngine;

namespace MVC
{
    /// <summary>
    ///  MVC框架的入口
    /// </summary>
    public class AppFacade : Facade
    {
        /// <summary>
        ///  复写AppFacade Getter
        /// </summary>
        public new static AppFacade I
        {
            get
            {
                if (instance == null)
                {
                    lock (sync)
                    {
                        if (instance == null)
                        {
                            instance = new AppFacade();
                        }
                    }
                }
                return instance as AppFacade;
            }
        }
        /// <summary>
        ///  启动MVC框架
        /// </summary>
        public void Startup()
        {
            RegisterProxy(new User_Proxy());
            RegisterMediator(new User_Mediator());
            RegisterProxy(new Hero_Proxy());
            RegisterMediator(new Hero_Mediator());
            RegisterProxy(new MySql_Proxy());
            RegisterMediator(new MySql_Mediator());
            RegisterProxy(new Bag_Proxy());
            RegisterMediator(new Bag_Mediator());
            RegisterProxy(new Skill_Proxy());
            RegisterMediator(new Skill_Mediator());
            RegisterProxy(new User_Instace_Proxy());
            RegisterMediator(new User_Instace_Mediator());
            Application.targetFrameRate = 60;
        }
    }
}
