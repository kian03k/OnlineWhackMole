using Client.ServiceReference2;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// RecordShow.xaml 的交互逻辑
    /// </summary>
    public partial class RecordShow : Window
    {
        public string UserName
        {
            get { return UserNameLabel.Content.ToString(); }
            set { UserNameLabel.Content = value; }
        }
        public RecordShow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            Service2Client client = new Service2Client();
            CompositeType info = client.querySql(UserName);
            List<SqlInfo> list = info.SqlinfoList.ToList();
            foreach (var sqlInfo in list)
            {
                sb.AppendLine(sqlInfo.datetime + "\t对战前" + sqlInfo.befnum + "星\t战绩为" + sqlInfo.changenum+ "星\t对战后" + sqlInfo.prenum + "星\t");

            }
            textBlock1.Text = sb.ToString();
        }

    }
}
