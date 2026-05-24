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
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
           
            textBlock1.Text = " 该游戏为打地鼠游戏。游戏内初始0分，谁先打到地鼠到谁+1分，谁先得到3分谁获胜，获胜人的分减失败人的分即增加的星数。" +
                "失败人默认减一星。如果最后统计该用户星数小于0，则重置星数为0.\n"+
                "本游戏有段位划分：\n"+
                "星数在0与20之间为青铜段位\n"+
                "星数在20与40之间为白银段位\n" +
                "星数在40与60之间为黄金段位\n"+
                "星数在60与100之间为钻石段位\n"+
                "星数大于100为钻石段位\n"+
                "注：不同段位有不同图标。";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("祝您游玩愉快！");
            this.Close();
        }
    }
}
