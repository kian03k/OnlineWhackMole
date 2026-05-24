using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public class CC
    {
        //连接的用户，每个用户都对应一个Service线程
        public static List<User> Users { get; set; }

        static CC()
        {
            Users = new List<User>();
        }

        public static User GetUser(string userName)
        {
            User user = null;
            foreach (var v in Users)
            {
                if (v.UserName == userName)
                {
                    user = v;
                    break;
                }
            }
            return user;
        }
    }
}
