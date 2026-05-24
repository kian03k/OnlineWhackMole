using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace A1
{

    internal class MyInkCanvas : InkCanvas
    {
        private InkVideo ink = new InkVideo();
        public MyInkCanvas()
        {
            this.DynamicRenderer = ink;
            //获取或设置显现器。
            this.EditingMode = InkCanvasEditingMode.Ink;
            //this.EditingMode获取指点设备使用的模式
            //指示上显示墨迹InkCanvas笔时，将数据发送。
        }

        /// <summary>
        /// 当动态绘制完成后抬起鼠标时，会自动调用此方法收集墨迹
        /// </summary>
        protected override void OnStrokeCollected(InkCanvasStrokeCollectedEventArgs e)
        {
            this.Strokes.Remove(e.Stroke);
            ink.CreateNewStroke(e);
            //创建
            this.Strokes.Add(ink.InkStroke);
            InkCanvasStrokeCollectedEventArgs args = new InkCanvasStrokeCollectedEventArgs(ink.InkStroke);
            base.OnStrokeCollected(args);
        }
    }
}
