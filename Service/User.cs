using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class User
    {
        public static string Logintemp { get; set; }
        /// <summary>登录的用户名</summary>
        public string UserName { get; set; }

        /// <summary>与该用户通信的回调接口</summary>
        public readonly IChatServiceCallback callback;

        /// <summary>用户所坐的桌号(-1:大厅)</summary>
        public int Index { get; set; }

        /// <summary>用户所坐的座位号(0:黑方，1:白方)</summary>
        public int Side { get; set; }

        /// <summary>是否已单击【开始】按钮</summary>
        public bool IsStarted { get; set; }

        public User(string userName, IChatServiceCallback callback)
        {
            this.UserName = userName;
            this.callback = callback;
            this.score = 0;

        }
        /// <summary>
        /// 积分
        /// </summary>
        public int score { get; set; }
    }
}
