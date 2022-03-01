using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Data.Sqlite;

namespace SQLServer
{
    //所有数据库有关的处理
    public class DbAccess
    {
        /// <summary>
        /// 声明一个连接对象
        /// </summary>
        private SqliteConnection dbConnection;
        /// <summary>
        /// 声明一个操作数据库命令
        /// </summary>
        private SqliteCommand dbCommand;
        /// <summary>
        /// 将读取的结果存在该变量下
        /// </summary>
        private SqliteDataReader reader;

        /// <summary>
        /// 构造函数 数据库的连接字符串，用于建立与特定数据源的连接
        /// </summary>
        /// <param name="connectionString"></param>
        public DbAccess(string connectionString)
        {
            OpenDB(connectionString);
        }
        public DbAccess()
        {

        }

        /// <summary>
        /// 建立与数据库的连接
        /// </summary>
        /// <param name="connectionString"></param>
        public void OpenDB(string connectionString)
        {
            try
            {
                dbConnection = new SqliteConnection(connectionString);
                //打开数据库
                dbConnection.Open();

                Console.WriteLine("Connected to db");
            }
            catch (Exception e)
            {
                string errorInfo = e.ToString();
                Console.WriteLine(errorInfo);
            }

        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void CloseSqlConnection()
        {

            if (dbCommand != null)
            {

                dbCommand.Dispose();

            }

            dbCommand = null;

            if (reader != null)
            {

                reader.Dispose();

            }

            reader = null;

            if (dbConnection != null)
            {

                dbConnection.Close();

            }

            dbConnection = null;

            Console.WriteLine("Disconnected from db.");

        }

        /// <summary>
        /// 执行查询sqlite语句操作
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public SqliteDataReader ExecuteQuery(string sqlQuery)
        {

            dbCommand = dbConnection.CreateCommand();

            dbCommand.CommandText = sqlQuery;

            reader = dbCommand.ExecuteReader();

            return reader;

        }

        /// <summary>
        /// 读取整张表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public SqliteDataReader ReadFullTable(string tableName)
        {
            string query = "SELECT * FROM " + tableName;

            return ExecuteQuery(query);
        }

        /// <summary>
        /// 插入一行数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="values">字段集合</param>
        /// <returns></returns>
        public SqliteDataReader InsertInto(string tableName, string[] values)
        {
            string query = "INSERT INTO " + tableName + " VALUES (" + values[0];

            for (int i = 1; i < values.Length; ++i)
            {
                query += ", " + values[i];
            }

            query += ")";

            return ExecuteQuery(query);

        }
        /// <summary>
        /// 插入一行数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cols">字段</param>
        /// <param name="values">值</param>
        /// <returns></returns>
        public SqliteDataReader InsertIntoSpecific(string tableName, string[] cols, string[] values)
        {
            if (cols.Length != values.Length)
            {
                throw new SqliteException("columns.Length != values.Length");
            }

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
        /// 更新一条数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cols">表头</param>
        /// <param name="colsvalues">更新的集合值</param>
        /// <param name="selectkey">要查询的字段</param>
        /// <param name="selectvalue">要查询的字段值</param>
        /// <returns></returns>
        public SqliteDataReader UpdateInto(string tableName, string[] cols, string[] colsvalues, string selectkey, string selectvalue)
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
        /// 删除一行数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cols">字段</param>
        /// <param name="colsvalues">字段的值</param>
        /// <returns></returns>
        public SqliteDataReader Delete(string tableName, string[] cols, string[] colsvalues)
        {
            string query = "DELETE FROM " + tableName + " WHERE " + cols[0] + " = " + colsvalues[0];

            for (int i = 1; i < colsvalues.Length; ++i)
            {
                query += " or " + cols[i] + " = " + colsvalues[i];
            }
            //Console.WriteLine(query);
            return ExecuteQuery(query);
        }


        /// <summary>
        /// 清空表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public SqliteDataReader DeleteContents(string tableName)
        {
            string query = "DELETE FROM " + tableName;

            return ExecuteQuery(query);
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="name">表名</param>
        /// <param name="col">字段</param>
        /// <param name="colType">类型</param>
        /// <returns></returns>
        public SqliteDataReader CreateTable(string name, string[] col, string[] colType)
        {

            if (col.Length != colType.Length)
            {

                throw new SqliteException("columns.Length != colType.Length");

            }

            string query = "CREATE TABLE IF NOT EXISTS " + name + " (" + col[0] + " " + colType[0];

            for (int i = 1; i < col.Length; ++i)
            {

                query += ", " + col[i] + " " + colType[i];

            }

            query += ")";

            return ExecuteQuery(query);

        }

        /// <summary>
        /// 查询表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="items">查询数据集合</param>
        /// <param name="col">字段</param>
        /// <param name="operation">操作</param>
        /// <param name="values">值</param>
        /// <returns></returns>
        public SqliteDataReader SelectWhere(string tableName, string[] items, string[] col, string[] operation, string[] values)
        {

            if (col.Length != operation.Length || operation.Length != values.Length)
            {

                throw new SqliteException("col.Length != operation.Length != values.Length");

            }

            string query = "SELECT " + items[0];

            for (int i = 1; i < items.Length; ++i)
            {

                query += ", " + items[i];

            }

            query += " FROM " + tableName + " WHERE " + col[0] + operation[0] + "'" + values[0] + "' ";

            for (int i = 1; i < col.Length; ++i)
            {

                query += " AND " + col[i] + operation[i] + "'" + values[i] + "' ";

            }

            return ExecuteQuery(query);

        }

        /// <summary>
        /// 取得表中最大行数
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public int GetTotalRows(string tableName)
        {
            //string query = "SELECT COUNT(*) FROM " + tableName;//取得该表一共有多少行数据    
            string query = "SELECT MAX(ID) FROM " + tableName;//取得ID的最大值

            SqliteDataReader reader = ExecuteQuery(query);
            if (reader.IsDBNull(0))//检查第0行是否有数据，没有数据就直接返回0
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
                return 0;
            }

            int count = 0;
            while (reader.Read())//Read函数能够自动往下偏移（相当于读取行）
            {
                count = reader.GetInt32(0);//读取一列数据
            }
            if (!reader.IsClosed)
            {
                reader.Close();
            }
            return count;
        }

        public SqliteDataReader SelectLike(string tableName, string[] items, string col, string values)
        {
            if (col.Length != values.Length)
            {

                throw new SqliteException("col.Length != values.Length");

            }

            string query = "SELECT " + items[0];

            for (int i = 1; i < items.Length; ++i)
            {

                query += ", " + items[i];

            }

            query += " FROM " + tableName + " WHERE " + col + " LIKE " + "'" + values + "' ";


            return ExecuteQuery(query);
        }
    }
}
