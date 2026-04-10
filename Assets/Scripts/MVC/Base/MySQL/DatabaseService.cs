using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Net.Sockets;

public class DatabaseService : IDisposable
{
    private readonly string _connectionString;
    private MySqlConnection _connection;
    private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);
    private Timer _keepAliveTimer;
    /// <summary>
    /// 数据库路径
    /// </summary>
    private static string path = "server=rm-bp1ilq26us071qrl8eo.mysql.rds.aliyuncs.com;port=3306;database=onlinestore;user=mysql_db_shadow;password=tcm520WLF;";

    // 连接池配置参数
    public DatabaseService(string server, string database, string user, string password)
    {
        var builder = new MySqlConnectionStringBuilder
        {
            Server = server,
            Database = database,
            UserID = user,
            Password = password,
            Pooling = true,                  // 启用连接池
            MinimumPoolSize = 1,               // 保持最小连接
            MaximumPoolSize = 50,              // 最大连接数
            //ConnectionTimeout = 30,            // 连接超时(秒)
            //ConnectionIdleTimeout = 180,       // 空闲连接超时(秒)
            ConnectionLifeTime = 600,          // 连接最大生存时间(秒)
            ConnectionReset = true,            // 每次使用前重置连接
            DefaultCommandTimeout = 30,        // 命令超时时间(秒)
            //CancellationTimeout = 5,           // 取消命令超时(秒)
            UseCompression = false,            // 根据网络情况决定
            //SslMode = MySqlSslMode.Preferred   // 安全连接
        };

        _connectionString = builder.ConnectionString;
        StartKeepAliveTimer();
    }
    // 启动保活定时器防止空闲断开
    private void StartKeepAliveTimer()
    {
        _keepAliveTimer = new Timer(async _ =>
        {
            if (_connection?.State == System.Data.ConnectionState.Open)
            {
                try
                {
                    using var cmd = new MySqlCommand("SELECT 1", _connection);
                    await cmd.ExecuteScalarAsync().ConfigureAwait(false);
                }
                catch
                {
                    // 忽略错误，下次操作时会自动重连
                }
            }
        }, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
    }

    // 获取数据库连接（带智能重连）
    public async Task<MySqlConnection> GetConnectionAsync()
    {
        await _connectionLock.WaitAsync().ConfigureAwait(false);
        try
        {
            if (_connection == null || _connection.State != System.Data.ConnectionState.Open)
            {
                _connection?.Dispose();
                _connection = await CreateConnectionWithRetryAsync().ConfigureAwait(false);
            }
            return _connection;
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    // 带重试的连接创建
    private async Task<MySqlConnection> CreateConnectionWithRetryAsync(int maxRetries = 3, int delayMs = 1000)
    {
        int retryCount = 0;
        while (retryCount < maxRetries)
        {
            try
            {
                var conn = new MySqlConnection(_connectionString);
                await conn.OpenAsync().ConfigureAwait(false);
                return conn;
            }
            catch (MySqlException) when (retryCount < maxRetries - 1)
            {
                await Task.Delay(delayMs * (int)Math.Pow(2, retryCount)).ConfigureAwait(false);
                retryCount++;
            }
        }
        throw new Exception("无法连接到数据库");
    }

    // 执行查询（防卡顿核心）
    public async Task<T> ExecuteQueryAsync<T>(Func<MySqlConnection, Task<T>> operation, CancellationToken ct = default)
    {
        using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, timeoutCts.Token);

        try
        {
            using var conn = await GetConnectionAsync().ConfigureAwait(false);
            return await operation(conn).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (timeoutCts.IsCancellationRequested)
        {
            throw new TimeoutException("查询操作超时");
        }
    }

    // 分页查询辅助方法
    public async Task<PaginatedResult<T>> QueryPaginatedAsync<T>(
        string baseQuery,
        object parameters,
        int page,
        int pageSize,
        Func<MySqlDataReader, T> mapper)
    {
        int offset = (page - 1) * pageSize;
        string sql = $"{baseQuery} LIMIT {pageSize} OFFSET {offset}";

        var results = new List<T>();
        int totalCount = 0;

        await ExecuteQueryAsync(async conn =>
        {
            // 获取总数
            var countQuery = $"SELECT COUNT(*) FROM ({baseQuery}) AS total";
            using var countCmd = new MySqlCommand(countQuery, conn);
            AddParameters(countCmd, parameters);
            totalCount = Convert.ToInt32(await countCmd.ExecuteScalarAsync());

            // 获取分页数据
            using var cmd = new MySqlCommand(sql, conn);
            AddParameters(cmd, parameters);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(mapper((MySqlDataReader)reader));
            }
            return new PaginatedResult<T>(results, totalCount, page, pageSize);
        });

        return new PaginatedResult<T>(results, totalCount, page, pageSize);
    }

    private void AddParameters(MySqlCommand cmd, object parameters)
    {
        if (parameters == null) return;

        var properties = parameters.GetType().GetProperties();
        foreach (var prop in properties)
        {
            cmd.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(parameters));
        }
    }

    public void Dispose()
    {
        _keepAliveTimer?.Dispose();
        _connection?.Close();
        _connection?.Dispose();
    }
}

// 分页结果类
public class PaginatedResult<T>
{
    public List<T> Items { get; }
    public int TotalCount { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public PaginatedResult(List<T> items, int totalCount, int page, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
    }
}