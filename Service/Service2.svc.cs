using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Service
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service2”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Service2.svc 或 Service2.svc.cs，然后开始调试。
    public class Service2 : IService2
    {

        public bool Login_Sql(string username, string password)
        {

            /*SqlParameter[] ps = {
                new SqlParameter("@user",loginName),
                new SqlParameter("@pwd",pwd)
            };
            
            DataTable table = Database.ExecSqlReturnDataTable(sql, ps);
            DataTable dt = new DataTable();
            SqlDataAdapter s = new SqlDataAdapter(sql, ps);
            s.Fill(dt);
            return table.Rows.Count > 0;*/
            //1.1获取到用户名和密码

            //1.2发送SQL指令，拼接SQL方式

            localbd sqlconn = new localbd();
            sqlconn.connect();
            string sql = "select count(1) from table1 where sqlname=@name and sqlpw=@password";
            SqlCommand cmd = new SqlCommand(sql, sqlconn.connect());
            SqlParameter p1 = new SqlParameter("@name", username);
            SqlParameter p2 = new SqlParameter("@password", password);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            if(Convert.ToInt32(cmd.ExecuteScalar()) > 0)
            {
                User.Logintemp = username;
                return true;
            }
            else
            {
                return false;
            }
            /*sql
            localbd conn = new localbd();
               string sql = "select count(*) from [table1] where sqlname = '" + userName + "' and sqlpw = '" + passWord + "'";
                SqlCommand cmd = conn.command(sql);
                if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                 {

                     }
                  else
                   {

                 }*/

        }


        public int  Register_Sql(string username, string password)
        {
            
            localbd sqlconn = new localbd  ();
            sqlconn.connect();
            int grade = 0;
            String sql = "INSERT INTO [table1](sqlname,sqlpw,grade) VALUES('" + username + "','" + password + "','"+grade+"')";
            int num=sqlconn.Execute(sql);
            return num;
        }

        public CompositeType querySql(string UserName)
        {
            CompositeType compositeType = new CompositeType();
            localbd sqlconn = new localbd();
            sqlconn.connect();
            string sql = "SELECT * FROM records where sqlname='" + UserName + "'";
            DataTable dt = new DataTable();
            SqlDataAdapter s = new SqlDataAdapter(sql, sqlconn.connect());
            s.Fill(dt);
            List<SqlInfo> users = new List<SqlInfo>();
            foreach (DataRow dr in dt.Rows)
            {
                var userinfo = new SqlInfo();
                userinfo.UserName = dr["sqlname"].ToString();
                userinfo.befnum = dr["before"].ToString();
                userinfo.changenum = dr["change"].ToString();
                userinfo.prenum = dr["present"].ToString();
                userinfo.datetime = Convert.ToDateTime(dr["sqltime"]).ToString("G");
                users.Add(userinfo);
            }
            compositeType.SqlinfoList = users;
            return compositeType;

        }

    }
}
