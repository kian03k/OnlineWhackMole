using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Service
{
    /// <summary>处理游戏大厅中每个房间的玩家</summary>
    public class GameTables
    {
        private const int max = 3; //网格最大的行列数
        public const int None = -1; //无地鼠
        public int x = -1;
        //public const int Black = 0; //
       //public const int White = 1; //

        /// <summary>保存同一房间的两个玩家</summary>
        public User[] players { get; set; }

        private int[,] grid = new int[max, max];


        public GameTables()
        {
            players = new User[2];
            ResetGrid();
        }

        /// <summary>重置棋盘</summary>
        public void ResetGrid()
        {
            for (int i = 0; i < max; i++)
            {
                for (int j = 0; j < max; j++)
                {
                    grid[i, j] = None;
                }
            }
        }

        
        public void IsWin(string username,int clickid)
        {
            int index = 0;
            if (players[1].UserName == username)
            {
                index = 1;
            }
            if (clickid == this.x)
            {
                string message1 = username + "得一分";
                players[index].score += 1;
                string message2 = players[0].UserName + ":" + players[0].score.ToString();//一号选手的得分
                string message3 = players[1].UserName + ":" + players[1].score.ToString();//二号选手的得分
                players[0].callback.Gotscore(message1, message2,message3);
                players[1].callback.Gotscore(message1,message2,message3);
                if (players[index].score == 3)
                {
                    int un_index = (index + 1) % 2;
                    int diff_value = players[index].score - players[un_index].score;
                    string add_username = players[index].UserName;
                    string sub_username =players[un_index].UserName;
                    int index_star=Updata(add_username, diff_value);
                    int unindex_star=Updata(sub_username, -1);
                    string message = username + "获胜,加" + diff_value+"星";
                    players[0].callback.GameWin(message);
                    players[1].callback.GameWin(message);
                    players[index].callback.ShowStar(index_star);
                    players[un_index].callback.ShowStar(unindex_star);
                    players[0].score = 0;
                    players[1].score = 0;
                }
            }
            players[0].callback.ResetGrid();
            players[1].callback.ResetGrid();
            players[0].callback.GameStart();

        }
        public int  Updata(string UserName, int change_number)
        {        
            int number = 0;
            localbd sqlconn = new localbd();
            sqlconn.connect();
            string sql = "select * from table1 ";
            DataTable dt = new DataTable();
            SqlDataAdapter s = new SqlDataAdapter(sql, sqlconn.connect());
            s.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                String name1 = dr["sqlname"].ToString();
                int number1 = Convert.ToInt32(dr["grade"].ToString());
                if (name1 == UserName)
                {
                    number = number1;
                }
            }
            int nownum = number + change_number;
            if (nownum < 0)
            {
                nownum = 0;
            }

            string updataSql = "UPDATE table1  SET grade=@grade WHERE sqlname =@sqlname";
            SqlCommand cmd = new SqlCommand(updataSql, sqlconn.connect());
            SqlParameter parn = new SqlParameter("@sqlname", UserName);
            cmd.Parameters.Add(parn);
            SqlParameter parp = new SqlParameter("@grade", nownum);
            cmd.Parameters.Add(parp);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            string strSql = "INSERT INTO records (sqlname,before,change,present,sqltime) VALUES(@sqlname,@before,@change,@present,@sqltime)";
            SqlCommand cmd1 = new SqlCommand(strSql, sqlconn.connect());
            SqlParameter p1 = new SqlParameter("@sqlname", UserName);
            SqlParameter p2 = new SqlParameter("@before", number);
            SqlParameter p3 = new SqlParameter("@change", change_number);
            SqlParameter p4 = new SqlParameter("@present", nownum);
            SqlParameter p5 = new SqlParameter("@sqltime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd1.Parameters.Add(p1);
            cmd1.Parameters.Add(p2);
            cmd1.Parameters.Add(p3);
            cmd1.Parameters.Add(p4);
            cmd1.Parameters.Add(p5);
            //result接受受影响的行数，也就是说大于0的话表示添加成功
            cmd1.ExecuteNonQuery();
            cmd1.Dispose();
            return nownum;
        }

        /// <summary>放置棋子。参数：行，列</summary>
        public void SetGridDot(int id)
        {
            this.x = id;
            players[0].callback.ShowSetDot(id);
            players[1].callback.ShowSetDot(id);
            //players[0].callback.GameStart();
            //players[1].callback.GameStart();
            //if (IsWin(i, j))
            //{
            //    players[0].IsStarted = false;
            //    players[1].IsStarted = false;
            //    string message = nextColor == 0 ? "黑方胜" : "白方胜";
            //    players[0].callback.GameWin(message);
            //    players[1].callback.GameWin(message);
            //    this.ResetGrid();
            //}
            //else
            //{
            //    nextColor = (nextColor + 1) % 2;
            //}
        }
    }
}
