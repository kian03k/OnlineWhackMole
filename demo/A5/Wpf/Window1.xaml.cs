using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.ServiceReference1;

namespace Wpf
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window, IService1Callback
    {
        public Window1()
        {
            InitializeComponent();
            this.Closing += ClientWindow_Closing;
            dockPanel1.Visibility = System.Windows.Visibility.Hidden;
        }

        public string UserName
        {
            get { return textBoxUserName.Text; }
            set { textBoxUserName.Text = value; }
        }
        private Service1Client client;

        void ClientWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (client != null)
            {
                client.Logout(UserName); //退出
                client.Close();
            }
        }

        private void AddMessage(string str)
        {
            TextBlock t = new TextBlock();
            t.Text = str;
            t.Foreground = Brushes.Blue;
            listBoxMessage.Items.Add(t);
        }

        //单击登录按钮引发的事件
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            UserName = textBoxUserName.Text;
            InstanceContext context = new InstanceContext(this);
            client = new ServiceReference1.Service1Client(context);
            try
            {
                client.Login(textBoxUserName.Text);
                btnLogin.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("与服务端连接失败：" + ex.Message);
                return;
            }
            this.Cursor = Cursors.Arrow;
        }

        //单击发送按钮引发的事件
        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            client.Talk(UserName, textBoxTalk.Text);
            textBoxTalk.Text = "";
        }

        //在文本框中输入对话后按回车键时引发的事件
        private void textBoxTalk_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                client.Talk(UserName, textBoxTalk.Text);
                textBoxTalk.Text = "";
            }
        }

        #region 实现IRndGameServiceCallback接口
        /// <summary>有用户登录</summary>
        public void ShowLogin(string loginUserName)
        {
            if (loginUserName == UserName)
            {
                dockPanel1.Visibility = System.Windows.Visibility.Visible;
            }
            listBoxOnLine.Items.Add(loginUserName);
        }

        /// <summary>其他用户退出</summary>
        public void ShowLogout(string userName)
        {
            listBoxOnLine.Items.Remove(userName);
        }

        public void ShowTalk(string userName, string message)
        {
            AddMessage(string.Format("[{0}]说：{1}", userName, message));
        }

        public void InitUsersInfo(string UsersInfo)
        {
            if (UsersInfo.Length == 0) return;
            string[] users = UsersInfo.Split('、');
            for (int i = 0; i < users.Length; i++)
            {
                listBoxOnLine.Items.Add(users[i]);
            }
        }
        #endregion
    }
}
