using UnityEngine;
using System;
using MySql.Data.MySqlClient;
using Components;
using Common;

/// <summary>
/// mysql数据库操作类
/// </summary>
public class MysqlDbAccess
{
    private MySqlConnection conn; // mysql连接

    private MySqlCommand cmd; // mysql命令

    private MySqlDataReader reader; // mysql读取器
    /// <summary>
    /// 数据库路径
    /// </summary>
    private string path = "server=rm-bp1ilq26us071qrl8eo.mysql.rds.aliyuncs.com;port=3306;database=mysql_cqhyl;user=mysql_db_shadow;password=tcm520WLF;"; 

    /// <summary>
    /// 判断网络开关
    /// </summary>
    public bool MysqlClose = false;
    public MysqlDbAccess() {
        OpenDB();
    }
    /// <summary>
    /// 关闭数据库连接
    /// </summary>
    public void CloseSqlConnection()
    {
        if (cmd != null) { cmd.Dispose(); cmd = null; }
        if (reader != null) { reader.Dispose(); reader = null; }
        if (conn != null) { conn.Close(); conn = null; }
    }

    /// <summary>
    /// 获取时间
    /// </summary>
    /// <returns></returns>
    public MySqlDataReader QueryTime()
    {
        string query = "SELECT NOW()";

        return ExecuteQuery(query);
    }

    /// <summary>
    /// 打开数据库
    /// </summary>
    /// <param name="connectionString"></param>
    private void OpenDB()
    {
        MysqlClose = true;
        SumSave.openMysql = true;
        try
        {
            conn = new MySqlConnection(path);
            conn.Open();
            MysqlClose = false;
            SumSave.openMysql = false;
        }
        catch (Exception e)
        {

        }
    }
    /// <summary>
    /// 执行SQL语句
    /// </summary>
    /// <param name="sqlQuery"></param>
    /// <returns></returns>
    public MySqlDataReader ExecuteQuery(string sqlQuery)
    {
        if (MysqlClose) return null;
        if (reader != null) { reader.Dispose(); reader = null; }
        cmd = conn.CreateCommand();
        cmd.CommandText = sqlQuery;
        try
        {
            reader = cmd.ExecuteReader();
            MysqlClose = false;
            SumSave.openMysql = false;
        }
        catch (Exception)
        {
            return null;
        }
        return reader;
    }

    /// <summary>
    /// 执行SQL语句
    /// </summary>
    /// <param name="sqlQuery"></param>
    /// <returns></returns>
    public async void ExecuteQueryAsync(string sqlQuery, Action<MySqlDataReader> callBack)
    {
        if (reader != null) { reader.Dispose(); reader = null; }
        cmd = conn.CreateCommand();
        cmd.CommandText = sqlQuery;
        reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();
        if (callBack != null)
            callBack(reader);
    }

    /// <summary>
    /// 查询表中全部数据 param tableName=表名 
    /// </summary>
    public MySqlDataReader ReadFullTable(Mysql_Table_Name tableName)
    {
        string query = "SELECT * FROM " + tableName;
        return ExecuteQuery(query);
    }
    /// <summary>
    /// 指定条数查询
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="limit">最新多少条</param>
    /// <returns></returns>
    public MySqlDataReader ReadGetLatest(Mysql_Table_Name tableName,int limit)
    {
        string query = "SELECT * FROM " + tableName;
        query += " ORDER BY id DESC LIMIT " + limit;
        return ExecuteQuery(query);
    }
    public MySqlDataReader ReadGetLatest(Mysql_Table_Name tableName, string col, string values , int limit)
    {
        string query = "SELECT * FROM " + tableName;
        query += " WHERE " + col + " = " + values;
        query += " ORDER BY id DESC LIMIT " + limit;
        return ExecuteQuery(query);
    }
    /// <summary>
    /// 指定范围查询
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <param name="startId">开始主键</param>
    /// <param name="endId">结束主键</param>
    /// <param name="idField">指定主键</param>
    /// <returns></returns>
    public MySqlDataReader ReadGetLatest(Mysql_Table_Name tableName, int startId, int endId, string idField = "id")
    {
        string query = "SELECT * FROM " + tableName;
        query += " WHERE " + idField + " BETWEEN " + startId + " AND " + endId;
        return ExecuteQuery(query);
    }
    public void ReadFullTableAsync(string tableName, Action<MySqlDataReader> callBack)
    {
        string query = "SELECT * FROM " + tableName;
        ExecuteQueryAsync(query, callBack);
    }

    /// <summary>
    /// 插入数据 param tableName=表名 values=插入数据内容
    /// </summary>
    public void InsertIntoAsync(Mysql_Table_Name tableName, string[] values, Action<MySqlDataReader> callBack = null)
    {
        string query = "INSERT INTO " + tableName + " VALUES (" + values[0];
        for (int i = 1; i < values.Length; ++i)
        {
            query += ", " + values[i];
        }
        query += ")";
        ExecuteQueryAsync(query, callBack);
    }
    public void InsertInto(Mysql_Table_Name tableName, string[] values)
    {
        string query = "INSERT INTO " + tableName + " VALUES (" + values[0];
        for (int i = 1; i < values.Length; ++i)
        {
            query += ", " + values[i];
        }
        query += ")";
        ExecuteQuery(query);
    }
    /// <summary>
    /// 更新数据 param tableName=表名 cols=更新字段 colsvalues=更新内容 selectkey=查找字段（主键) selectvalue=查找内容
    /// </summary>
    public MySqlDataReader UpdateInto(Mysql_Table_Name tableName, string[] cols, string[] colsvalues, string selectkey, string selectvalue)
    {
        string query = "UPDATE " + tableName + " SET " + cols[0] + " = " + colsvalues[0];
        for (int i = 1; i < colsvalues.Length; ++i)
        {
            query += ", " + cols[i] + " =" + colsvalues[i];
        }
        query += " WHERE " + selectkey + " = " + selectvalue + " ";

        return ExecuteQuery(query);
    }

    /// <summary>
    /// 更新数据 param tableName=表名 cols=更新字段 colsvalues=更新内容 selectkey=查找字段（主键) selectvalue=查找内容
    /// </summary>
    public void UpdateIntoAsync(string tableName, string[] cols, string[] colsvalues, string selectkey, string selectvalue, Action<MySqlDataReader> callBack = null)
    {
        string query = "UPDATE " + tableName + " SET " + cols[0] + " = " + colsvalues[0];
        for (int i = 1; i < colsvalues.Length; ++i)
        {
            query += ", " + cols[i] + " =" + colsvalues[i];
        }
        query += " WHERE " + selectkey + " = " + selectvalue + " ";

        ExecuteQueryAsync(query, callBack);
    }

    /// <summary>
    /// 删除数据 param tableName=表名 cols=字段 colsvalues=内容
    /// </summary>
    public MySqlDataReader Delete(Mysql_Table_Name tableName, string[] cols, string[] colsvalues)
    {
        string query = "DELETE FROM " + tableName + " WHERE " + cols[0] + " = " + colsvalues[0];
        for (int i = 1; i < colsvalues.Length; ++i)
        {
            query += " and " + cols[i] + " = " + colsvalues[i];
        }
        return ExecuteQuery(query);
    }
    /// <summary>
    /// 插入数据 param tableName=表名 cols=插入字段 value=插入内容
    /// </summary>
    public MySqlDataReader InsertIntoSpecific(string tableName, string[] cols, string[] values)
    {
        //if (cols.Length != values.Length) {	
        //	throw new MySqlException("columns.Length != values.Length");
        //}
        string query = "INSERT INTO " + tableName + "(" + cols[0];
        for (int i = 1; i < cols.Length; ++i)
        {
            query += ", " + cols[i];
        }
        query += ") VALUES (" + values[0];
        for (int i = 1; i < values.Length; ++i)
        {
            query += ", " + values[i];
        }
        query += ")";
        return ExecuteQuery(query);
    }
    /// <summary>
    /// 删除表中全部数据
    /// </summary>
    public MySqlDataReader DeleteContents(string tableName)
    {
        string query = "DELETE FROM " + tableName;
        return ExecuteQuery(query);
    }
    /// <summary>
    /// 创建表 param name=表名 col=字段名 colType=字段类型
    /// </summary>
    public MySqlDataReader CreateTable(string name, string[] col, string[] colType)
    {
        //if (col.Length != colType.Length) {
        //	throw new mysqlException ("columns.Length != colType.Length");
        //}
        string query = "CREATE TABLE " + name + " (" + col[0] + " " + colType[0];
        for (int i = 1; i < col.Length; ++i)
        {
            query += ", " + col[i] + " " + colType[i];
        }
        query += ")";
        return ExecuteQuery(query);
    }
    /// <summary>
    /// 按条件查询数据 param tableName=表名 col=查找字段 operation=运算符 values=内容
    /// </summary>
    public MySqlDataReader SelectWhere(Mysql_Table_Name tableName, string[] col, string[] operation, string[] values)
    {
        string query = " SELECT * FROM " + tableName + " WHERE " + col[0] + operation[0] + "'" + values[0] + "' ";
        for (int i = 1; i < col.Length; ++i)
        {
            query += " AND " + col[i] + operation[i] + "'" + values[i] + "' ";
        }
        return ExecuteQuery(query);
    }
    /// <summary>
    /// 查询表
    /// </summary>
    public MySqlDataReader Select(Mysql_Table_Name tableName, string col, string values)
    {
        string query = "SELECT * FROM " + tableName + " WHERE " + col + " = " + values;

        return ExecuteQuery(query);
    }

    /// <summary>
    /// 根据时间查询
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="col">时间</param>
    /// <returns></returns>
    public MySqlDataReader SelectTime(string tableName, string col)
    {
        string query = "SELECT * FROM " + tableName + " WHERE TO_DAYS " + col + " = " + "TO_DAYS (NOW())";

        return ExecuteQuery(query);
    }

    public MySqlDataReader Select(string tableName, string col, string operation, string values)
    {
        string query = "SELECT * FROM " + tableName + " WHERE " + col + operation + values;
        return ExecuteQuery(query);
    }
    /// <summary>
    /// 升序查询
    /// </summary>
    public MySqlDataReader SelectOrderASC(string tableName, string col)
    {
        string query = "SELECT * FROM " + tableName + " ORDER BY " + col + " ASC";
        return ExecuteQuery(query);
    }
    /// <summary>
    /// 降序查询
    /// </summary>
    public MySqlDataReader SelectOrderDESC(string tableName, string col)
    {
        string query = "SELECT * FROM " + tableName + " ORDER BY " + col + " DESC";
        return ExecuteQuery(query);
    }
    /// <summary>
    /// 查询表行数
    /// </summary>
    public MySqlDataReader SelectCount(string tableName)
    {
        string query = "SELECT COUNT(*) FROM " + tableName;
        return ExecuteQuery(query);

    }
}