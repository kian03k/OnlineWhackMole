using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;

namespace Service
{
    public class GobangService : IService
    {
        public GobangService()
        {
            if (CC.Users == null)
            {
                CC.Users = new List<User>();
                CC.Rooms = new GameTables[CC.maxRooms];
                for (int i = 0; i < CC.maxRooms; i++)
                {
                    CC.Rooms[i] = new GameTables();
                }
            }
        }
        

        /// <summary>每座位用一位表示，0表示无人，1表示有人</summary>
        private string GetTablesInfo()
        {
            string str = "";
            for (int i = 0; i < CC.Rooms.Length; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    str += CC.Rooms[i].players[j] == null ? "0" : "1";
                }
            }
            return str;
        }

        /// <summary>将当前游戏室情况发送给所有用户</summary>
        private void SendRoomsInfoToAllUsers()
        {
            int userCount = CC.Users.Count;
            string roomInfo = this.GetTablesInfo();
            foreach (var user in CC.Users)
            {
                user.callback.UpdateTablesInfo(roomInfo, userCount);
            }
        }

        #region 实现服务端接口

        public void Login()
        {
            int numstar = 0;
            string userName = User.Logintemp;
            OperationContext context = OperationContext.Current;
            IChatServiceCallback callback = context.GetCallbackChannel<IChatServiceCallback>();//实例化客户端请求
            User newUser = new User(userName, callback);
            //newUser.callback.StartGame(userName);
            string str = "";
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
                if (name1 == userName)
                {
                    numstar = number1;
                }
            }
            for (int i = 0; i < CC.Users.Count; i++)
            {
                str += CC.Users[i].UserName + "、";
            }
            newUser.callback.InitUsersInfo(str.TrimEnd('、'));
            newUser.callback.ShowStar(numstar);
            CC.Users.Add(newUser);
            foreach (var user in CC.Users)
            {
                user.callback.ShowLogin(userName, CC.maxRooms);
            }
            SendRoomsInfoToAllUsers();

        }


        /// <summary>用户退出</summary>
        public void Logout(string userName)
        {
            User logoutUser = CC.GetUser(userName);
            foreach (var user in CC.Users)
            {
                //不需要发给退出用户
                if (user.UserName != logoutUser.UserName)
                {
                    user.callback.ShowLogout(userName);
                }
            }
            CC.Users.Remove(logoutUser);
            logoutUser = null; //将其设置为null后，WCF会自动关闭该客户端
            SendRoomsInfoToAllUsers();
        }

        /// <summary>用户入座,参数：用户名,桌号,座位号</summary>
        public void SitDown(string userName, int index, int side)
        {
            User p = CC.GetUser(userName);
            p.Index = index;
            p.Side = side;
            CC.Rooms[index].ResetGrid();
            CC.Rooms[index].players[side] = p;
            //告诉入座玩家入座信息
            p.callback.ShowSitDown(userName, side);
            int anotherSide = (side + 1) % 2;  //同一桌的另一个玩家
            User p1 = CC.Rooms[index].players[anotherSide];
            if (p1 != null)
            {
                //告诉入座玩家另一个玩家是谁
                p.callback.ShowSitDown(p1.UserName, anotherSide);
                //告诉另一个玩家入座玩家是谁
                p1.callback.ShowSitDown(p.UserName, side);
            }
            //重新将游戏室各桌情况发送给所有用户
            SendRoomsInfoToAllUsers();
        }

        /// <summary>用户离开座位退出,参数：桌号,座位号,游戏是否已经开始</summary>
        public void GetUp(int index, int side)
        {
            User p0 = CC.Rooms[index].players[side];
            User p1 = CC.Rooms[index].players[(side + 1) % 2];
            p0.score = 0;
            p0.callback.ShowGetUp(side);
            CC.Rooms[index].players[side] = null; //注意该语句执行后p0!=null
            if (p1 != null)
            {
                p1.callback.ShowGetUp(side);
                p1.IsStarted = false;
            }
            else
            {
                CC.Rooms[index].ResetGrid();
            }
            //重新将游戏室各桌情况发送给所有用户
            SendRoomsInfoToAllUsers();
        }
        /// <summary>该用户单击了开始按钮,参数：用户名,桌号,座位号</summary>
        public void Start(string userName, int index, int side)
        {
            User p0 = CC.Rooms[index].players[side];
            p0.IsStarted = true;
            p0.callback.ShowStart(side);
            int anotherSide = (side + 1) % 2;   //对方座位号
            User p1 = CC.Rooms[index].players[anotherSide];
            if (p1 != null)
            {
                p1.callback.ShowStart(side);
                if (p1.IsStarted && p0.IsStarted)
                {
                    p0.callback.GameStart();
                }
            }
        }
        /// <summary>
        /// 用户鼠标点击位置
        /// </summary>
        public void userset(int index,string username,int clickid)
        {
            CC.Rooms[index].IsWin(username,clickid);
        }
        /// <summary>放置地鼠,参数：桌号,洞的编号</summary>
        public void SetDot(int index, int id)
        {
            CC.Rooms[index].SetGridDot(id);
        }
        /// <summary>
        /// 随机生成地鼠出现位置
        /// </summary>
        /// <param name="index"></param>
        public void randomplace(int index)
        {
            Random ran = new Random();
            int delay = ran.Next(900, 1100);
            Thread.Sleep(delay);

            int x = ran.Next(1, 11);
            SetDot(index, x);
        }
        /// <summary>客户端发的对话信息,参数：桌号,用户名,对话内容</summary>
        public void Talk(int index, string userName, string message)//桌号为-1时，群发
        {
            if (index == -1)
            {
                User user = CC.GetUser(userName);//获取用户
                foreach (var v in CC.Users)
                {
                    v.callback.ShowTalk("群发", userName, message);
                }
            }
            else//房间私聊
            {
                User p0 = CC.Rooms[index].players[0];
                User p1 = CC.Rooms[index].players[1];
                if (p0 != null) p0.callback.ShowTalk("私聊", userName, message);
                if (p1 != null) p1.callback.ShowTalk("私聊", userName, message);
            }
        }
        public void TalkToSomebody(string sender, string receiver, string message)//选择用户私聊
        {
            User mess_sender = CC.GetUser(sender);//发送者
            User mess_receiver = CC.GetUser(receiver);//接收者
            mess_receiver.callback.ShowTalk("私聊", sender, message);
            mess_sender.callback.ShowTalk("私聊", sender, message);

        }


        #endregion //实现服务端接口
    }
}
