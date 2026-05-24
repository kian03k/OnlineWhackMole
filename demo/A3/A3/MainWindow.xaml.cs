using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace A3
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

        Stopwatch stopwatch = new Stopwatch();


        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            long t1 = await Task.Run(() => Multiply(200, 18, 27));
            textBlock1.Text = string.Format("测试1（矩阵1：200×18，矩阵2：18×27），用时：{0}毫秒",t1);

            long t2 = await Task.Run(() => Multiply(2000, 180, 270));
            textBlock1.Text += string.Format("\n测试2（矩阵1：2000×180，矩阵2：180×270），用时：{0}毫秒", t2);

            long t3 = await Task.Run(() => Multiply(3000, 200, 300));
            textBlock1.Text += string.Format("\n测试3（矩阵1：2000×200，矩阵2：200×300），用时：{0}毫秒", t3);
        }


        private long Multiply(int rowCount, int colCount, int colCount2)
        {
            double[,] m1 = InitializeMatrix(rowCount, colCount);
            double[,] m2 = InitializeMatrix(colCount, colCount2);
            double[,] result = new double[rowCount, colCount2];

            // 并行
            stopwatch.Restart();
            result = new double[rowCount, colCount2];
            MultiplyMatricesParallel(m1, m2, result);
            stopwatch.Stop();
            long timeElapsed = stopwatch.ElapsedMilliseconds;
            return timeElapsed;
        }

        private void MultiplyMatricesParallel(double[,] m1, double[,] m2, double[,] result)
        {
            int Rows = m1.GetLength(0);
            int Cols = m1.GetLength(1);
            int m2Cols = m2.GetLength(1);
            //GetLength(0)取行数，GetLength(1)取列数。
            // 内循环不需要并行
            Action<int> action = i =>
            {
                for (int j = 0; j < m2Cols; j++)
                {
                    double temp = 0; 
                    for (int k = 0; k < Cols; k++)
                    {
                        temp += m1[i, k] * m2[k, j];
                    }
                    result[i, j] = temp;
                }
            };
            // 外循环并行执行
            Parallel.For(0, Rows, action);
        }

        private double[,] InitializeMatrix(int rowCount, int colCount)
        {
            double[,] matrix = new double[rowCount, colCount];

            Random r = new Random();
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    matrix[i, j] = r.Next(100);
                }
            }
            return matrix;
        }

    }
}
