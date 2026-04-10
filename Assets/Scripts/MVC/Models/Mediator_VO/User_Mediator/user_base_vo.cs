using System;
namespace MVC
{
    /// <summary>
    /// 主账户数据存储结构
    /// </summary>
    public class user_base_vo : Base_VO
    {

        public int par;//游戏区
        public string uid;//主id
        public DateTime RegisterDate;//注册日期
        public DateTime Nowdate;//登录日期
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public override string[] Set_Instace_String()
        {
            return new string[] {
            GetStr(0),
            GetStr(par),
            GetStr(uid),
            GetStr(Tool_UI.ToStandardFormat( RegisterDate)),
            GetStr(Tool_UI.ToStandardFormat(Nowdate))
            };
              

        }

        public override string[] Set_Uptade_String()
        {
            return new string[] { GetStr(Tool_UI.ToStandardFormat(Nowdate))}; 
        }
        /// <summary>
        /// 写入字符串
        /// </summary>
        /// <returns></returns>
        public override string[] Get_Update_Character()
        {
            return new string[] { "Nowdate" };
        }
    }

}
