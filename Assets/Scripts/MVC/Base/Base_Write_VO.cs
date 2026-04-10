using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVC
{
    /// <summary>
    /// 写入模型
    /// </summary>
    public class Base_Wirte_VO
    {
        public Mysql_Type type;//写入类型
        public Mysql_Table_Name tableName;//写入表名
        public string[] columnNames;//更新列名
        public string[] columnValues;//更新列值
        public bool exist = true;//是否写入
    }

}
