namespace TarenaMVC
{
    /// <summary>
    ///  数据处理接口
    /// </summary>
    public interface IProxy : INotifier
    {
        /// <summary>
        ///  Proxy的名称
        /// </summary>
        string ProxyName { get; set; }
    }
}