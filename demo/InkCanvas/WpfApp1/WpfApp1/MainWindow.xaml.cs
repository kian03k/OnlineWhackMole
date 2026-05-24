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

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string p = AppDomain.CurrentDomain.BaseDirectory;
            if (p.IndexOf("\\bin\\") > 0)
            {
                if (p.EndsWith("\\bin\\Debug\\"))
                    p = p.Replace("\\bin\\Debug", "");
                if (p.EndsWith("\\bin\\Release\\"))
                    p = p.Replace("\\bin\\Release", "");
            }
            if (!p.EndsWith("App_Data\\"))
                p = p + "App_Data\\";
            AppDomain.CurrentDomain.SetData("DataDirectory", p);
            fd.Text = p;
        }
    }
}
