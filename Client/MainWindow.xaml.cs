using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Client.ServiceReference1;
using Client.ServiceReference2;
using Microsoft.Win32;

namespace Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Boolean boolean;
        private ServiceClient client;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void textAdmin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtAdmin.Focus();
        }

        private void txtAdmin_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAdmin.Text) && txtAdmin.Text.Length > 0)
            {
                textAdmin.Visibility = Visibility.Collapsed;
            }
            else
            {
                textAdmin.Visibility = Visibility.Visible;
            }
        }

        private void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Focus();
        }

        private void txtPassword_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Password) && txtPassword.Password.Length > 0)
            {
                textPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                textPassword.Visibility = Visibility.Visible;
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            string username = this.txtAdmin.Text;
            string password = this.txtPassword.Password;
            if (string.IsNullOrEmpty(txtAdmin.Text))
            {
                MessageBox.Show("用户名不能为空！");
            }
            else
            {
                this.Cursor = Cursors.Wait;
               
                Service2Client client = new Service2Client();
                boolean = client.Login_Sql(username, password);
                if (boolean)
                {
                    ClientWindow clientWindow = new ClientWindow();
                    clientWindow.UserName = username;
                    this.Close();
                    clientWindow.Show();           
                }
                else
                {
                    MessageBox.Show("用户名或密码不正确！");
                }

                this.Cursor = Cursors.Arrow;
               
            }

        }
        private void close_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Register register = new Register();
            register.Show();
            this.Close();
        }

        private void Button_Click22(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            window1.Show();
        }
    }
}
