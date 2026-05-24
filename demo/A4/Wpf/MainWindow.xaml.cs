using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace Wpf
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
            filesInfo = client.GetFilesInfo();
            for (int i = 0; i < filesInfo.Length; i++)
            {
                string[] s = filesInfo[i].Split(',');
                Label label = new Label();
                label.Content = string.Format("文件名：{0}，文件长度：{1}字节", s[0], s[1]);
                listBox1.Items.Add(label);
            }
            client.Close();
            if (listBox1.Items.Count > 0)
            {
                listBox1.SelectedIndex = 0;
                btnStart.IsEnabled = true;
            }
            else
            {
                btnStart.IsEnabled = false;
                AddInfo("找不到可供下载的文件！");
            }
        }

        string[] filesInfo;
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            textBlock1.Text = "";
            string[] s = filesInfo[listBox1.SelectedIndex].Split(',');
            AddInfo("正在下载{0}", s[0]);
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
            string filePath = System.IO.Path.Combine( System.Environment.CurrentDirectory, s[0]);
            Stream stream1 = client.DownloadStream(s[0]);
            SaveStreamToFile(stream1, filePath);
        }

        private void SaveStreamToFile(Stream stream1, string filePath)
        {
            FileStream outstream = File.Open(filePath, FileMode.Create, FileAccess.Write);
            const int bufferLen = 4096;
            byte[] buffer = new byte[bufferLen];
            int count = 0;
            int bytecount = 0;
            while ((count = stream1.Read(buffer, 0, bufferLen)) > 0)
            {
                outstream.Write(buffer, 0, count);
                bytecount += count;
            }
            outstream.Close();
            stream1.Close();
            AddInfo("下载完成，共下载{0}字节，文件保存到{1}", bytecount, filePath);
        }

        void AddInfo(string format, params object[] args)
        {
            textBlock1.Text += string.Format(format, args);
            textBlock1.Text += Environment.NewLine;
        }
    }

}
