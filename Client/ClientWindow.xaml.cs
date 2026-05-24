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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Client.ServiceReference1;
using Client.ServiceReference2;

namespace Client
{
    /// <summary>
    /// ClientWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ClientWindow : Window, IServiceCallback
    {
        public string UserName
        {
            get { return textBoxUserName.Text; }
            set { textBoxUserName.Text = value; }
        }
       // private int nextColor = -1;        //该哪一方放置棋子（-1:不允许，0：黑方，1：白方）
        private bool isGameStart = false;  //是否已开始游戏
        private const int max = 4;       //地鼠格子最大值（0～3）
        private int maxTables;            //服务端开设的最大房间号
        private int tableIndex = -1;      //房间号（所坐的游戏桌号）
        private int tableSide = -1;       //座位号

        private Border[,] gameTables;        //开设的房间（每个房间一桌）
        private int flag = -1;//判断是否选择房间
        //private int[,] grid = new int[max, max];  //保存棋盘上每个棋子的颜色
       // private Image[,] images = new Image[max, max];  //保存棋盘上每个棋子
        //private bool isFromServer;          //是否为服务端发送过来的操作
        private  ServiceClient client;  //客户端实例
        public ClientWindow()
        {
            InitializeComponent();

            //确保关闭窗口时关闭客户端
            this.Closing += ClientWindow_Closing;
            this.btnStart.IsEnabled = false;
            ChangeRoomsInfoVisible(false);
            ChangeRoomVisible(false);
            listBoxUser.AddHandler(Button.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.listBoxUser_MouseLeftButtonDown), true);
            //SetNextColor(-1);
            ClientWindow_Login();
            
        }
        private void ClientWindow_Login()//登入
        {
            UserName = textBoxUserName.Text;
            this.Cursor = Cursors.Wait;
            client = new ServiceClient(new InstanceContext(this));//实例化客户端
            try
            {
                client.Login();
                //serviceTextBlock.Text = "服务端地址：" + client.Endpoint.ListenUri.ToString();
                //ChangeState(btnLogin, false, btnLogout, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("与服务端连接失败：" + ex.Message);
                return;
            }
            this.Cursor = Cursors.Arrow;
        }

        void ClientWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //ChangeState(btnLogin, true, btnLogout, false);
            if (client != null)
            {
                if (tableIndex != -1) //如果在房间内，要求先返回大厅
                {
                    MessageBox.Show("请先返回大厅，然后再退出");
                    e.Cancel = true;
                }
                else
                {
                    client.Logout(UserName); //从大厅退出
                    //注意此处不能再调用client.Close()，因为调用Logout后服务端已关闭与该用户的连接
                }
            }
        }

        private void ChangeRoomsInfoVisible(bool visible)
        {
            if (visible == false)
            {
                gridRooms.Visibility = System.Windows.Visibility.Collapsed;
                gridMessage.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                gridRooms.Visibility = System.Windows.Visibility.Visible;
                gridMessage.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void ChangeRoomVisible(bool visible)
        {
            if (visible == false)
            {
                gridRoom.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                gridRoom.Visibility = System.Windows.Visibility.Visible;
            }
        }
       
        private void AddMessage(string str)
        {
            TextBlock t = new TextBlock();
            t.Text = str;
            t.Foreground = Brushes.Blue;
            listBoxMessage.Items.Add(t);
        }

        private void AddColorMessage(string str, SolidColorBrush color)
        {
            TextBlock t = new TextBlock();
            t.Text = str;
            t.Foreground = color;
            listBoxMessage.Items.Add(t);
        }

        private static void ChangeState(Button btnStart, bool isStart, Button btnStop, bool isStop)
        {
            btnStart.IsEnabled = isStart;
            btnStop.IsEnabled = isStop;
        }

        #region 鼠标和键盘事件


        private void Show_record_Click(object sender, RoutedEventArgs e)
        {
            
            RecordShow rs=new RecordShow();
            rs.UserName = UserName;
            rs.Show();

        }
        //在某个座位坐下时引发的事件
        private void RoomSide_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ResetGrid();
            //btnLogout.IsEnabled = false;
            Border border = e.Source as Border;
            if (border != null)
            {
                string s = border.Tag.ToString();
                tableIndex = int.Parse(s[0].ToString());
                tableSide = int.Parse(s[1].ToString());
                client.SitDown(UserName, tableIndex, tableSide);
            }
        }
        private void listBoxUser_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (listBoxUser.SelectedItem != null)
            {
                textBoxTalk.Text = "@" + listBoxUser.SelectedItem.ToString() + ":";
            }

        }
        //单击发送按钮引发的事件
        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxTalk.Text[0] == '@')
            {
                string str = textBoxTalk.Text.Remove(0, 1);
                string[] message = str.Split(':');
                client.TalkToSomebody(UserName, message[0], message[1]);
            }
            else
            {
                client.Talk(tableIndex, UserName, textBoxTalk.Text);
            }
        }
       
        //在对话文本框中按回车键时引发的事件
        private void textBoxTalk_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (textBoxTalk.Text[0] == '@')
                {
                    string str = textBoxTalk.Text.Remove(0, 1);
                    string[] message = str.Split(':');
                    client.TalkToSomebody(UserName, message[0], message[1]);
                }
                else
                {
                    client.Talk(tableIndex, UserName, textBoxTalk.Text);
                }
            }
        }

        //单击开始按钮引发的事件
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (tableIndex == -1)
            {
                MessageBox.Show("选择房间");
            }
            else
            {
                
                client.Start(UserName, tableIndex, tableSide);
                btnStart.IsEnabled = false;
            }
            //SetNextColor(-1);
        }

        //单击返回大厅按钮引发的事件
        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            if (tableIndex != -1)
            {
                client.GetUp(tableIndex, tableSide);
            }
        }
        #region 图片点击事件
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        { }

            //在棋盘上单击鼠标左键时引发的事件
        private void picture1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            client.userset(tableIndex,UserName, 1);
        }
        private void picture2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            client.userset(tableIndex,UserName, 2);
        }
        private void picture3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            client.userset(tableIndex,UserName, 3);
        }
        private void picture4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            client.userset(tableIndex,UserName, 4);
        }
        private void picture5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            client.userset(tableIndex,UserName, 5);
        }
        private void picture6_MouseDown(object sender, MouseButtonEventArgs e)
        {
            client.userset(tableIndex,UserName, 6);
        }
        private void picture7_MouseDown(object sender, MouseButtonEventArgs e)
        {
            client.userset(tableIndex,UserName, 7);
        }
        private void picture8_MouseDown(object sender, MouseButtonEventArgs e)
        {
            client.userset(tableIndex,UserName,8);
        }
        private void picture9_MouseDown(object sender, MouseButtonEventArgs e)
        {
            client.userset(tableIndex,UserName, 9);
        }
        private void picture10_MouseDown(object sender, MouseButtonEventArgs e)
        {
            client.userset(tableIndex, UserName,10);
        }
        #endregion
        private void Room_Click(object sender, RoutedEventArgs e)
        {

            gridRooms.Visibility = System.Windows.Visibility.Visible;
            //this.maxTables = maxTables;
            flag = 1;
           // ChangeRoomsInfoVisible(true);
            this.CreateTables();
        }

        #endregion //鼠标和键盘事件


        #region 实现服务端指定的IServiceCallback接口
        public void InitUsersInfo(string UsersInfo)
        {
            if (UsersInfo.Length == 0) 
                return;
            string[] users = UsersInfo.Split('、');
            for (int i = 0; i < users.Length; i++)
            {
                listBoxUser.Items.Add(users[i]);//listbox里面添加用户
            }
        }
        /// <summary>有用户登录</summary>
        public void ShowLogin(string loginUserName, int maxTables)
        {

            if (loginUserName == UserName)
            {
                ChangeRoomsInfoVisible(true);
                this.maxTables = maxTables;
                this.CreateTables();
            }
            listBoxUser.Items.Add(loginUserName);
            AddMessage(loginUserName + "进入大厅。");
        }
       //public void Showuser(string user)
       // {

       // }
        public void ShowStar(int number)
        {
            star_number.Text = number + "星玩家。";
            string danString = "Bronze";
            var AvailableTerms = new Dictionary<Func<int, bool>, Action>
            {
                {x => (x >= 0  &&  x < 20) , () => danString="Bronze"}, //do nothing
                {x => (x >= 20 &&  x < 40), () => danString="Silver"},
                {x => (x >= 40 &&  x < 60), () => danString="Gold"},
                {x => (x >= 60 &&  x < 100), () => danString="Diamond"},
                {x => (x>100)             , () => danString="Crown"},
            };

           AvailableTerms.First(dan => dan.Key(number)).Value();
            Dan_picture.Source = ((Image)this.Resources[danString]).Source;
        }
 

        /// <summary>其他用户退出</summary>
        public void ShowLogout(string userName)
        {
            AddMessage(userName + "退出大厅。");
        }

        /// <summary>用户入座</summary>
        public void ShowSitDown(string userName, int side)
        {
           // stackPanelGameTip.Visibility = System.Windows.Visibility.Collapsed;
            if (side == tableSide)
            {
                isGameStart = false;
                //btnLogout.IsEnabled = false;
                btnStart.IsEnabled = true;
                listBoxRooms.IsEnabled = false;//返回大厅前不允许再坐到另一个位置
                textBlockRoomNumber.Text = "桌号：" + (tableIndex + 1);
                ChangeRoomVisible(true);
            }
            if (side == 0)
            {
                textBlockBlackUserName.Text =userName+":0";
                AddMessage(string.Format("{0}在房间{1}{2}入座。", userName, tableIndex + 1,userName));
            }
            else
            {
                textBlockWhiteUserName.Text =userName+":0";
                AddMessage(string.Format("{0}在房间{1}{2}入座。", userName, tableIndex + 1,userName));
            }
        }

        /// <summary>用户离座</summary>
        public void ShowGetUp(int side)
        {
            //stackPanelGameTip.Visibility = System.Windows.Visibility.Collapsed;
            if (side == tableSide)
            {
                isGameStart = false;
                //btnLogout.IsEnabled = true;
                listBoxRooms.IsEnabled = true;//返回大厅后允许再坐到另一个位置
                AddMessage(UserName + "返回大厅。");
                this.tableIndex = -1;
                this.tableSide = -1;
                ChangeRoomVisible(false);
            }
            else
            {
                if (isGameStart)
                {
                    AddMessage("对方回大厅了，游戏终止。");
                    isGameStart = false;
                    btnStart.IsEnabled = true;
                }
                else
                {
                    AddMessage("对方返回大厅。");
                }
                if (side == 0) textBlockBlackUserName.Text = "";
                else textBlockWhiteUserName.Text = "";
            }
        }

        public void ShowStart(int side)
        {
            ResetGrid();
            if (side == 0) AddMessage("一号已就位");
            else AddMessage("二号已就位");
        }

        public void ShowTalk(string messageTpye,string userName, string message)
        {
            AddColorMessage(string.Format("<{0}>{1}：{2}",messageTpye,userName, message), Brushes.Black);
        }

        /// <summary>设置地鼠状态。参数：id</summary>
        public void ShowSetDot(int id)//放置地鼠
        {
            // grid[i, j] = color;
            // if (color == 0) 
            switch (id)
            {
                case 1: pictureBox1.Visibility = System.Windows.Visibility.Visible; break;
                case 2: pictureBox2.Visibility = System.Windows.Visibility.Visible; break;
                case 3: pictureBox3.Visibility = System.Windows.Visibility.Visible; break;
                case 4: pictureBox4.Visibility = System.Windows.Visibility.Visible; break;
                case 5: pictureBox5.Visibility = System.Windows.Visibility.Visible; break;
                case 6: pictureBox6.Visibility = System.Windows.Visibility.Visible; break;
                case 7: pictureBox7.Visibility = System.Windows.Visibility.Visible; break;
                case 8: pictureBox8.Visibility = System.Windows.Visibility.Visible; break;
                case 9: pictureBox9.Visibility = System.Windows.Visibility.Visible; break;
                case 10:pictureBox10.Visibility = System.Windows.Visibility.Visible; break;
                    //  SetNextColor((color + 1) % 2);
            }
        }
        /// <summary>
        /// 地鼠刷新
        /// </summary>
        public void ResetGrid()
        {
            invisiable();
        }
        public void invisiable()
        {
            pictureBox1.Visibility = System.Windows.Visibility.Collapsed;
            pictureBox2.Visibility = System.Windows.Visibility.Collapsed;
            pictureBox3.Visibility = System.Windows.Visibility.Collapsed;
            pictureBox4.Visibility = System.Windows.Visibility.Collapsed;
            pictureBox5.Visibility = System.Windows.Visibility.Collapsed;
            pictureBox6.Visibility = System.Windows.Visibility.Collapsed;
            pictureBox7.Visibility = System.Windows.Visibility.Collapsed;
            pictureBox8.Visibility = System.Windows.Visibility.Collapsed;
            pictureBox9.Visibility = System.Windows.Visibility.Collapsed;
            pictureBox10.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void GameStart()//新的游戏得分开始
        {
           // stackPanelGameTip.Visibility = System.Windows.Visibility.Visible;

            this.isGameStart = true;  //为true时才可以点击
           // invisiable();
            client.randomplace(tableIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        public void GameWin(string message)
        {
            AddColorMessage("\n" + message + "\n", Brushes.Red);
            MessageBox.Show(message);

            btnStart.IsEnabled = true;
            //stackPanelGameTip.Visibility = System.Windows.Visibility.Collapsed;
            this.isGameStart = false;
            //SetNextColor(-1);
            //blackImage.Visibility = System.Windows.Visibility.Collapsed;
            invisiable();
        }
        public void Gotscore(string message,string message1,string message2)
        {
            AddMessage(message);
            
            this.textBlockBlackUserName.Text = message1;
            this.textBlockWhiteUserName.Text = message2;
           // this.isGameStart = false;
            invisiable();
        }

        public void UpdateTablesInfo(string tablesInfo, int userCount)
        {
            textBlockMessage.Text = string.Format("在线人数：{0}", userCount);
           
                for (int i = 0; i < maxTables; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (tableIndex == -1)
                        {
                            if (tablesInfo[2 * i + j] == '0')
                            {
                                gameTables[i, j].Child.Visibility = System.Windows.Visibility.Hidden;
                                gameTables[i, j].Child.IsEnabled = true;
                            }
                            else
                            {
                                gameTables[i, j].Child.Visibility = System.Windows.Visibility.Visible;
                                gameTables[i, j].Child.IsEnabled = false;
                            }
                        }
                        else
                        {
                            gameTables[i, j].Child.IsEnabled = false;
                            if (tablesInfo[2 * i + j] == '0')
                            {
                                gameTables[i, j].Child.Visibility = System.Windows.Visibility.Hidden;
                            }
                            else gameTables[i, j].Child.Visibility = System.Windows.Visibility.Visible;
                        }
                    }
                
            }
        }
        #endregion //实现服务端指定的IRndGameServiceCallback接口


        #region 接口调用的方法
        /// <summary>创建游戏桌</summary>
        private void CreateTables()
        {
            this.gameTables = new Border[maxTables, 2];
            //isFromServer = false;
            //创建游戏大厅中的房间（每房间一个游戏桌）
            for (int i = 0; i < maxTables; i++)
            {
                int j = i + 1;
                StackPanel sp = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(5)
                };
                TextBlock text = new TextBlock()
                {
                    Text = "房间" + (i + 1),
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    Width = 40
                };

                gameTables[i, 0] = new Border()
                {
                    Tag = i + "0",
                    Background = Brushes.White,
                    Child = new Image()
                    {
                        Source = ((Image)this.Resources["avatar00"]).Source,
                        Height = 25
                    }
                };
                Image image = new Image()
                {
                    Source = ((Image)this.Resources["battle"]).Source,
                    Height = 25
                };
                gameTables[i, 1] = new Border()
                {
                    Tag = i + "1",
                    Background = Brushes.White,
                    Child = new Image()
                    {
                        Source = ((Image)this.Resources[Change_picture()]).Source,
                        Height = 25
                    }
                };
                gameTables[i, 0].MouseDown += RoomSide_MouseDown;
                gameTables[i, 1].MouseDown += RoomSide_MouseDown;
                sp.Children.Add(text);
                sp.Children.Add(gameTables[i, 0]);
                sp.Children.Add(image);
                sp.Children.Add(gameTables[i, 1]);
                listBoxRooms.Items.Add(sp);
            }
        }
        public String Change_picture()
        {
            String picture = "avatar00";
            Random random = new Random();
            switch (random.Next(0,10))
            {
                case 0: picture = "avatar00";break;
                case 1: picture = "avatar01";break;
                case 2: picture = "avatar02";break;
                case 3: picture = "avatar03";break;
                case 4: picture = "avatar04";break;
                case 5: picture = "avatar05";break;
                case 6: picture = "avatar06";break;
                case 7: picture = "avatar07";break;
                case 8: picture = "avatar08";break;
                case 9: picture = "avatar09";break;
            }
            return picture;
        }




        #endregion //接口调用的方法


    }
}
