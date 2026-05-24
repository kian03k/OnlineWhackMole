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
        }



        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            StartWindow("西西皮", 50, 300);
            StartWindow("瓜瓜糖", 600, 300);
        }

        private void StartWindow(string userName, int left, int top)
        {
            Window1 w = new Window1();
            w.Left = left;
            w.Top = top;
            w.UserName = userName;
            w.Owner = this;
            w.Closed += (sender, e) => this.Activate();
            w.Show();
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            Window1 w = new Window1();
            w.Owner = this;
            w.Closed += (sendObj, args) => this.Activate();
            w.Show();
        }

    }
}
