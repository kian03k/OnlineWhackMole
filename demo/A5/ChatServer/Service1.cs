using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatServer
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“Service1”。
    public class Service1 : IService1
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        public void Login(string userName)
        {
            OperationContext context = OperationContext.Current;
            IService1Callback callback = context.GetCallbackChannel<IService1Callback>();
            User newUser = new User(userName, callback);
            string str = "";
            for (int i = 0; i < CC.Users.Count; i++)
            {
                str += CC.Users[i].UserName + "、";
            }
            newUser.callback.InitUsersInfo(str.TrimEnd('、'));
            CC.Users.Add(newUser);
            foreach (var user in CC.Users)
            {
                user.callback.ShowLogin(userName);
            }
        }

        /// <summary>
        /// 用户退出。参数：退出者用户名
        /// </summary>
        public void Logout(string userName)
        {
            User logoutUser = CC.GetUser(userName);
            CC.Users.Remove(logoutUser);
            logoutUser = null; //将其设置为null后，WCF会自动关闭该客户端
            foreach (var user in CC.Users)
            {
                user.callback.ShowLogout(userName);
            }
        }

        /// <summary>
        /// 客户端发来的对话信息。参数：发送者用户名，对话内容
        /// </summary>
        public void Talk(string userName, string message)
        {
            User user = CC.GetUser(userName);
            foreach (var v in CC.Users)
            {
                v.callback.ShowTalk(userName, message);
            }
        }
    }
}
