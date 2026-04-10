using TarenaMVC;

namespace MVC
{
    /// <summary>
    /// 技能系统中介者
    /// </summary>
    public class Skill_Mediator : Mediator
    {

        public new const string NAME = "Skill_Mediator";

        public Skill_Mediator()
        {
            this.MediatorName = NAME;

            skill = AppFacade.I.RetrieveProxy(Skill_Proxy.NAME) as Skill_Proxy;
        }
        /// <summary>
        /// 技能数据
        /// </summary>
        private Skill_Proxy skill;


        /// <summary>
        /// 监听技能信息
        /// </summary>
        /// <returns></returns>
        public override string[] ListNotificationInterests()
        {
            return new string[] {
                 
                };
        }

        /// <summary>
        /// 处理技能系统相关信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public override void HandleNotification(string name, object data)
        {

            switch (name)
            {
               
                default:
                    break;
            }
        }
    }
}