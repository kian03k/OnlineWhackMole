using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Service
{
    public  class localbd
    {
        SqlConnection conn;
        string String_SQL = @"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\player.mdf;Initial Catalog=player;Integrated Security=True";


        public void openSql()
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
        }
        public void closeSql()
        {
            conn.Close();
        }
        public SqlConnection connect()
        {
            conn = new SqlConnection(String_SQL);
            openSql();
            return conn;
        }
        public SqlCommand command(string sql)
        {
            SqlCommand smd = new SqlCommand(sql, connect());
            return smd;
        }
        public int Execute(string sql)//更新操作
        {
            return command(sql).ExecuteNonQuery();
        }
        public SqlDataReader read(string sql)//读取操作
        {
            return command(sql).ExecuteReader();
        }
    }
}