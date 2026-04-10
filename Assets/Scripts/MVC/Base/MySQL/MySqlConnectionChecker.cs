using System;
using System.Net.Sockets;
using MySql.Data.MySqlClient;
using UnityEngine;

public class MySqlConnectionChecker
{
    public static bool CheckConnection(string connectionString, int timeoutSeconds = 3)
    {
        // 添加超时设置（单位：秒）
        var builder = new MySqlConnectionStringBuilder(connectionString)
        {
            ConnectionTimeout = (uint)timeoutSeconds
        };

        using (var connection = new MySqlConnection(builder.ConnectionString))
        {
            try
            {
                connection.Open();
                return connection.State == System.Data.ConnectionState.Open;
            }
            catch (MySqlException ex)
            {
                // 处理特定错误码
                switch (ex.Number)
                {
                    case 1042: // 无法连接到服务器
                    case 0:    // 一般连接错误
                        return false;
                    default:
                        Console.WriteLine($"数据库错误: {ex.Message}");
                        return false;
                }
            }
            catch (SocketException) // 网络层错误
            {
                return false;
            }
            catch (TimeoutException) // 连接超时
            {
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"未知错误: {ex.Message}");
                return false;
            }
        }
    }
}
