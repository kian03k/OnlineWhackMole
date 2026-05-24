using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Media;

namespace A1
{
    public class InkVideo : DynamicRenderer
    {
        public string inkText { private set; get; }
        public InkVideoStroke InkStroke { private set; get; }
        private Point previousPoint;
        private MediaPlayer p1;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e">为InCanvas.StrokeCollected事件提供的数据</param>
        public void CreateNewStroke(InkCanvasStrokeCollectedEventArgs e)
        {
            InkStroke = new InkVideoStroke(this, e.Stroke.StylusPoints);
        }

        public static MediaPlayer CreateVideoPlayer(string uriString)
        {
            MediaPlayer p = new MediaPlayer();
            //MediaPlayer为媒体提供播放功能
            p.Open(new Uri(uriString, UriKind.Relative));
            p.IsMuted = true;
            //是否静音
            p.MediaEnded += (sender, args) => { p.Position = TimeSpan.Zero; };
            //媒体播放完成后，播放位置回到0.
            p.Play();
            return p;
        }

        public void Draw(Point first, MediaPlayer player, DrawingContext dc, StylusPointCollection points)
        {
            Point pt = (Point)points.Last();
            Vector v = Point.Subtract(pt, first);
            //确定区域
            if (double.IsNegativeInfinity(first.X)) return;
            if (v.Length > 6)
            {
                Rect rect = new Rect(first, v);
                dc.DrawVideo(player, rect);
                //将视频绘制到指定区域内
            }
        }
        /// <summary>
        /// 当笔触接触数字化仪后，在笔线程池中的线程上发生。
        /// </summary>
        /// <param name="rawStylusInput"></param>
        protected override void OnStylusDown(RawStylusInput rawStylusInput)
        {
            base.OnStylusDown(rawStylusInput);
            previousPoint = (Point)rawStylusInput.GetStylusPoints().First();
            inkText = @".\..\..\Videos\" + MainWindow.VideoFileName;
            p1 = CreateVideoPlayer(inkText);
        }
        /// <summary>
        /// 笔触提起时
        /// </summary>
        /// <param name="rawStylusInput"></param>
        protected override void OnStylusUp(RawStylusInput rawStylusInput)
        {
            p1.Stop();
            p1.Close();
            base.OnStylusUp(rawStylusInput);
        }
        /// <summary>
        /// 笔触移到数字化仪，效果是每次移动，都对上一点刷新（即使不点击）。
        /// </summary>
        /// <param name="rawStylusInput"></param>
        protected override void OnStylusMove(RawStylusInput rawStylusInput)
        {
            StylusPointCollection stylusPoints = rawStylusInput.GetStylusPoints();
            this.Reset(Stylus.CurrentStylusDevice, stylusPoints);
            base.OnStylusMove(rawStylusInput);
        }
        /// <summary>
        /// 笔触绘画是实时的
        /// </summary>
        /// <param name="drawingContext"></param>
        /// <param name="stylusPoints"></param>
        /// <param name="geometry"></param>
        /// <param name="fillBrush"></param>
        protected override void OnDraw(DrawingContext drawingContext, StylusPointCollection stylusPoints, Geometry geometry, Brush fillBrush)
        {
            this.Draw(previousPoint, p1, drawingContext, stylusPoints);
        }
    }

    public class InkVideoStroke : Stroke
    {
        private InkVideo ink;
        private string inkText;

        public InkVideoStroke(InkVideo ink, StylusPointCollection stylusPoints) : base(stylusPoints)
        {
            this.ink = ink;
            this.inkText = ink.inkText;
        }
        protected override void DrawCore(DrawingContext drawingContext, DrawingAttributes drawingAttributes)
        {
            MediaPlayer mp = InkVideo.CreateVideoPlayer(inkText);
            Point pt1 = (Point)StylusPoints.First();
            ink.Draw(pt1, mp, drawingContext, StylusPoints);
        }

    }
}
