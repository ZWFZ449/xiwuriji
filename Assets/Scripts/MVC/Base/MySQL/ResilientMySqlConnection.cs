using System;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient; // 或使用 MySqlConnector

public class ResilientMySqlConnection
{
    private readonly string _connectionString;
    private MySqlConnection _connection;
    private bool _isNetworkAvailable = true;
    private readonly Timer _networkMonitorTimer;
    private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);
    public ResilientMySqlConnection(string connectionString)
    {
        _connectionString = connectionString;

        // 添加连接池配置
        var builder = new MySqlConnectionStringBuilder(connectionString)
        {
            Pooling = true,
            MinimumPoolSize = 1,
            MaximumPoolSize = 20,
            ConnectionLifeTime = 300,
            ConnectionReset = true
        };
        _connectionString = builder.ConnectionString;

        // 网络状态监测
        _networkMonitorTimer = new Timer(CheckNetworkStatus, null, 0, 5000); // 每5秒检查一次
    }

    // 网络状态检测
    private void CheckNetworkStatus(object state)
    {
        bool previousStatus = _isNetworkAvailable;
        _isNetworkAvailable = System.Net.NetworkInformation.NetworkInterface
            .GetIsNetworkAvailable();

        // 网络恢复时尝试重连
        if (!previousStatus && _isNetworkAvailable)
        {
            Task.Run(() => ReconnectAsync());
        }
    }

    // 智能重连方法
    public async Task<MySqlConnection> GetConnectionAsync()
    {
        await _connectionLock.WaitAsync();
        try
        {
            if (_connection == null || _connection.State != System.Data.ConnectionState.Open)
            {
                await ReconnectAsync();
            }
            return _connection;
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    // 带重试机制的连接方法
    private async Task ReconnectAsync()
    {
        int retryCount = 0;
        const int maxRetries = 5;
        const int initialDelay = 1000; // 1秒

        // 关闭旧连接（如果存在）
        if (_connection != null)
        {
            try
            {
                await _connection.CloseAsync();
            }
            catch { /* 忽略关闭错误 */ }
            _connection.Dispose();
            _connection = null;
        }

        while (retryCount < maxRetries)
        {
            try
            {
                // 指数退避策略
                int delay = (int)(initialDelay * Math.Pow(2, retryCount));
                await Task.Delay(delay);

                _connection = new MySqlConnection(_connectionString);
                await _connection.OpenAsync();

                Console.WriteLine($"数据库连接成功! 重试次数: {retryCount}");
                return; // 连接成功
            }
            catch (MySqlException ex) when (IsNetworkRelatedError(ex))
            {
                retryCount++;
                Console.WriteLine($"网络错误重试中... 尝试 #{retryCount}，错误: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"连接失败: {ex.Message}");
                throw; // 非网络错误直接抛出
            }
        }

        throw new Exception($"无法连接数据库，已尝试 {maxRetries} 次");
    }

    // 判断是否为网络相关错误
    private bool IsNetworkRelatedError(MySqlException ex)
    {
        // 常见网络错误码
        int[] networkErrorCodes = {
            1042, // 无法连接到服务器
            2013, // 查询期间连接丢失
            2003, // 无法连接到服务器
            2006, // 服务器已断开连接
            0     // 一般网络错误
        };

        return Array.Exists(networkErrorCodes, code => code == ex.Number);
    }

    // 执行带重连机制的查询
    public async Task<T> ExecuteWithRetryAsync<T>(Func<MySqlConnection, Task<T>> operation)
    {
        int retryCount = 0;
        const int maxRetries = 3;

        while (retryCount <= maxRetries)
        {
            try
            {
                var conn = await GetConnectionAsync();
                return await operation(conn);
            }
            catch (MySqlException ex) when (IsNetworkRelatedError(ex) && retryCount < maxRetries)
            {
                retryCount++;
                Console.WriteLine($"查询失败，重试中... 尝试 #{retryCount}");
                await ReconnectAsync(); // 尝试重新连接
            }
        }

        throw new Exception($"操作失败，已重试 {maxRetries} 次");
    }

    // 资源清理
    public void Dispose()
    {
        _networkMonitorTimer?.Dispose();
        _connection?.Close();
        _connection?.Dispose();
    }
}