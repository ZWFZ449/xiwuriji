using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Common;
using MVC;
using MySql.Data.MySqlClient; // 或使用 MySqlConnector

public static class Program
{
    private static DatabaseService _dbService;
    /// <summary>
    /// 读取路径
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static async Task QueryUsersAsync(Mysql_Table_Name tableName)
    {
        try
        {
            var users = await _dbService.ExecuteQueryAsync(async conn =>
            {
                string str = path_Mysql(tableName);
                using var cmd = new MySqlCommand(str, conn);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    switch (tableName)
                    { 
                       case Mysql_Table_Name.mysql_time:
                            //Mysql_Read.Read(reader);
                            break;
                        case Mysql_Table_Name.user_trial_towers:
                            Mysql_Read.Read_towers(reader);
                            break;
                        case Mysql_Table_Name.user_rank:
                            Mysql_Read.Read_user_rank(reader);
                            break;
                        case Mysql_Table_Name.user_endless_battle:
                            Mysql_Read.Read_user_endless_battle(reader);
                            break;
                    }
                }
                return await reader.ReadAsync();

            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"查询用户失败: {ex.Message}");
        }
    }
    /// <summary>
    /// 读取路径
    /// </summary>
    /// <param name="tableName"></param>
    public static void Read_path_Mysql1(Mysql_Table_Name tableName) 
    {
        Task.Run(() => Program.QueryUsersAsync(tableName));
    }

    /// <summary>
    /// 获取路径
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    private static string path_Mysql(Mysql_Table_Name tableName)
    {
        string str = "";
        switch (tableName)
        {
            case Mysql_Table_Name.db_monster:
                break;
            case Mysql_Table_Name.db_stditems:
                break;
            case Mysql_Table_Name.db_magic:
                break;
            case Mysql_Table_Name.db_map:
                break;
            case Mysql_Table_Name.db_heros:
                break;
            case Mysql_Table_Name.db_setting:
                break;
            case Mysql_Table_Name.db_artifact:
                break;
            case Mysql_Table_Name.mo_user_base:
                break;
            case Mysql_Table_Name.mo_user_value:
                break;
            case Mysql_Table_Name.mo_user_hero:
                break;
            case Mysql_Table_Name.mo_user_setting:
                break;
            case Mysql_Table_Name.mo_user_artifact:
                break;
            case Mysql_Table_Name.mo_user:
                break;
            case Mysql_Table_Name.loglist:
                break;
            case Mysql_Table_Name.user_login:
                break;
            case Mysql_Table_Name.db_pass:
                break;
            case Mysql_Table_Name.mo_user_pass:
                break;
            case Mysql_Table_Name.mo_user_plant:
                break;
            case Mysql_Table_Name.db_plant:
                break;
            case Mysql_Table_Name.db_pet:
                break;
            case Mysql_Table_Name.mo_user_pet_hatching:
                break;
            case Mysql_Table_Name.mo_user_pet_explore:
                break;
            case Mysql_Table_Name.mo_user_pet:
                break;
            case Mysql_Table_Name.db_pet_explore:
                break;
            case Mysql_Table_Name.mo_user_world:
                break;
            case Mysql_Table_Name.db_lv:
                break;
            case Mysql_Table_Name.user_rank:
                break;
            case Mysql_Table_Name.db_hall:
                break;
            case Mysql_Table_Name.mo_user_achieve:
                break;
            case Mysql_Table_Name.db_achieve:
                break;
            case Mysql_Table_Name.db_store:
                break;
            case Mysql_Table_Name.db_seed:
                break;
            case Mysql_Table_Name.mo_user_seed:
                break;
            case Mysql_Table_Name.mo_user_needlist:
                break;
            case Mysql_Table_Name.db_collect:
                break;
            case Mysql_Table_Name.mo_user_collect:
                break;
            case Mysql_Table_Name.db_signin:
                break;
            case Mysql_Table_Name.dream_user_signin:
                break;
            case Mysql_Table_Name.mo_user_tap:
                break;
            case Mysql_Table_Name.mo_user_iphone:
                break;
            case Mysql_Table_Name.db_pars:
                break;
            case Mysql_Table_Name.user_message_window:
                break;
            case Mysql_Table_Name.db_basetask:
                break;
            case Mysql_Table_Name.mo_user_greenhandguide:
                break;
            case Mysql_Table_Name.user_player_buff:
                break;
            case Mysql_Table_Name.user_emial:
                break;
            case Mysql_Table_Name.server_mail:
                break;
            case Mysql_Table_Name.history_server_mail:
                break;
            case Mysql_Table_Name.db_accumulatedrewards:
                break;
            case Mysql_Table_Name.mo_user_rewards_state:
                break;
            case Mysql_Table_Name.db_fate:
                break;
            case Mysql_Table_Name.db_vips:
                break;
            case Mysql_Table_Name.db_world_boss:
                break;
            case Mysql_Table_Name.user_world_boss_rank:
                break;
            case Mysql_Table_Name.user_world_boss:
                break;
            case Mysql_Table_Name.history_world_boss:
                break;
            case Mysql_Table_Name.db_formula:
                break;
            case Mysql_Table_Name.db_suits:
                break;
            case Mysql_Table_Name.db_dec:
                break;
            case Mysql_Table_Name.versions_task:
                break;
            case Mysql_Table_Name.db_weather:
                break;
            case Mysql_Table_Name.user_trial_towers:
                str = Mysql_Read.Select(Mysql_Table_Name.user_trial_towers, "par", GetStr(SumSave.par));
                break;
            case Mysql_Table_Name.user_world_boss_copy1:
                break;
            case Mysql_Table_Name.db_endlessbattle:
                break;
            case Mysql_Table_Name.user_endless_battle:
                str = Mysql_Read.Select(Mysql_Table_Name.user_endless_battle, "par", GetStr(SumSave.par));
                break;
            case Mysql_Table_Name.db_strengthen_needlist:
                break;
            case Mysql_Table_Name.db_equip_suit:
                break;
            case Mysql_Table_Name.mysql_time:
                str = "SELECT NOW()";
                break;
            default:
                break;
        }
        return str;
    }

    public static void MysqlMain(List<Base_Wirte_VO> wirtes)
    {
        Task.Run(() => Program.Main(wirtes));
    }
    public static async Task Main(List<Base_Wirte_VO> wirtes)
    {
        //server=rm-bp1ilq26us071qrl8eo.mysql.rds.aliyuncs.com;port=3306;database=onlinestore;user=mysql_db_shadow;password=tcm520WLF;
        // 初始化数据库服务
        if (_dbService == null) _dbService = new DatabaseService("rm-bp1ilq26us071qrl8eo.mysql.rds.aliyuncs.com", "onlinestore", "mysql_db_shadow", "tcm520WLF");
        //if(_dbService==null) _dbService=new DatabaseService();
        // 示例1：执行简单查询
        // 读取时间
        await QueryUsersAsync(Mysql_Table_Name.mysql_time);
        string query = "";
        for (int i = 0; i < wirtes.Count; i++)
        {
            query = "";
            if (wirtes[i].exist)
            {
                switch (wirtes[i].type)
                {
                    case Mysql_Type.InsertInto:
                        query = InsertInto(wirtes[i].tableName, wirtes[i].columnValues);
                        break;
                    case Mysql_Type.UpdateInto:
                        if (wirtes[i].tableName == Mysql_Table_Name.user_rank 
                            || wirtes[i].tableName == Mysql_Table_Name.user_world_boss_rank
                            || wirtes[i].tableName == Mysql_Table_Name.user_trial_towers
                            || wirtes[i].tableName == Mysql_Table_Name.db_world_boss
                            || wirtes[i].tableName == Mysql_Table_Name.user_endless_battle)
                        {
                            query = UpdateInto(wirtes[i].tableName, wirtes[i].columnNames, wirtes[i].columnValues, "par", GetStr(SumSave.par));
                        }
                        else
                            query = UpdateInto(wirtes[i].tableName, wirtes[i].columnNames, wirtes[i].columnValues, "uid", GetStr(SumSave.crt_user.uid));
                        break;
                    case Mysql_Type.Delete:
                        query = Delete(wirtes[i].tableName, new string[] { "uid" }, new string[] { GetStr(SumSave.crt_user.uid) });
                        break;

                    default: break;
                }
                if (query != "") await QueryUserAsync(query);
                wirtes[i].exist = false;
            }
        }
    }
    /// <summary>
    /// 获取字符串
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    private static string GetStr(object o)
    {
        return "'" + o + "'";

    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="cols"></param>
    /// <param name="colsvalues"></param>
    /// <returns></returns>
    public static string Delete(Mysql_Table_Name tableName, string[] cols, string[] colsvalues)
    {
        string query = "DELETE FROM " + tableName + " WHERE " + cols[0] + " = " + colsvalues[0];
        for (int i = 1; i < colsvalues.Length; ++i)
        {
            query += " and " + cols[i] + " = " + colsvalues[i];
        }
        return (query);
    }
    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="cols"></param>
    /// <param name="colsvalues"></param>
    /// <param name="selectkey"></param>
    /// <param name="selectvalue"></param>
    /// <returns></returns>
    public static string UpdateInto(Mysql_Table_Name tableName, string[] cols, string[] colsvalues, string selectkey, string selectvalue)
    {
        string query = "UPDATE " + tableName + " SET " + cols[0] + " = " + colsvalues[0];
        for (int i = 1; i < colsvalues.Length; ++i)
        {
            query += ", " + cols[i] + " =" + colsvalues[i];
        }
        query += " WHERE " + selectkey + " = " + selectvalue + " ";

        return query;
    }
    /// <summary>
    /// 插入
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static string InsertInto(Mysql_Table_Name tableName, string[] values)
    {
        string query = "INSERT INTO " + tableName + " VALUES (" + values[0];
        for (int i = 1; i < values.Length; ++i)
        {
            query += ", " + values[i];
        }
        query += ")";
        return query;

    }
    private static async Task QueryUserAsync(string query)
    {
        try
        {
             await _dbService.ExecuteQueryAsync(async conn =>
            {
                using var cmd = new MySqlCommand(query, conn);
                return await cmd.ExecuteScalarAsync();
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"查询失败: {ex.Message}");
        }
    }
   
}