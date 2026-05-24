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
    /// Create.xaml 的交互逻辑
    /// </summary>
    public partial class Create : Window
    {
        public Create()
        {
            InitializeComponent();
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            int num = int.Parse(createnum.Text);
            Random rd=new Random();
            for (int i = 0; i < num; i++)
            {
 
                StartLogin(rd.Next(0, 500));
            }
            
        }
        private void StartLogin( int num)
        {
            MainWindow w = new MainWindow();
            w.Left = num;
            w.Top = num;
            //w.Owner = this;
            //w.Closed += (sender, e) => this.Activate();
            w.Show();
            this.Close();
        }


    }
}
