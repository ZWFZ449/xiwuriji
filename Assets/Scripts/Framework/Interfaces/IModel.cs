namespace TarenaMVC
{
    /// <summary>
    ///  管理IProxy
    /// </summary>
    public interface IModel
    {
        /// <summary>
        ///  注册IProxy
        /// </summary>
        /// <param name="proxy"></param>
        void RegisterProxy(IProxy proxy);
        /// <summary>
        ///  获取IProxy
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IProxy RetrieveProxy(string name);
        /// <summary>
        ///  移除IProxy
        /// </summary>
        /// <param name="name"></param>
        void RemoveProxy(string name);
    }
}
