using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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

namespace A2
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Prompt.Visibility = Visibility.Collapsed;
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            string s1 = txt1.Text + txt2.Text;
            string s2 = txt1.Text + txt3.Text;
            IPAddress ip1;
            if (IPAddress.TryParse(s1, out ip1) == false|| IPAddress.TryParse(s2, out ip1) == false)
                //TryParse(String, IPAddress),确定字符串是否为有效的 IP 地址。
            {
                Prompt.Visibility = Visibility.Visible;
                btn1.IsEnabled = false;
                return;
            }
            else
            {
                btn1.IsEnabled = true;
                Prompt.Visibility = Visibility.Collapsed;
            }
        }
        private void btn1_Click(object sender, RoutedEventArgs e)
        {

            int start = int.Parse(txt2.Text);
            int end = int.Parse(txt3.Text);
            if (start > end)
            {
                Prompt.Content = "终止值必须大于等于起始值！";
                Prompt.Visibility = Visibility.Visible;
                // MessageBox.Show("终止值必须大于等于起始值", "地址范围有错");后面的为提示框左上角标题。
                return;
            }
            info.Items.Clear();
            for (int i = start; i <= end; i++)
            {
                IPAddress ip = IPAddress.Parse(txt1.Text + i);
                Task.Run(() => Check(ip));
            }

        }

        private void Check(IPAddress ip)
        {
            Stopwatch task = Stopwatch.StartNew();
            string String1 = "";
            try
            {
                String1 = Dns.GetHostEntry(ip).HostName;
            }
            catch
            {
                String1 = "（不在线）";
            }
            task.Stop();
            info.Dispatcher.Invoke(
                () => info.Items.Add(
                      string.Format("扫描地址：{0}，扫描用时：{1}毫秒，主机DNS名称：{2}",
                      ip, task.ElapsedMilliseconds, String1)
             ));
        }
    }
}
