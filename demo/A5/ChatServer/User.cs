using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public class User
    {
        public string UserName { get; set; }
        public readonly IService1Callback callback;

        public User(string userName, IService1Callback callback)
        {
            this.UserName = userName;
            this.callback = callback;
        }
    }
}
