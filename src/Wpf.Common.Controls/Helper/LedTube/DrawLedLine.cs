using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Wpf.Common.Controls.Helper.LedTubes
{
    
    internal abstract class DrawLedLine
    {

        public virtual Polyline Draw(Polyline line, LedHorizontalPostion hp, LedVerticalPostion vp)
        {
            this.HorizontalPostion = hp;
            this.VerticalPostion = vp;
            var position = this.GetCanvasLocation();
            var points = GetPoints();
            points.ToList().ForEach(p => line.Points.Add(p));
            Canvas.SetLeft(line, position.Item1);
            Canvas.SetTop(line, position.Item2);
            return line;
        }


        protected LedHorizontalPostion HorizontalPostion { get; private set; }

        protected LedVerticalPostion VerticalPostion { get; private set; }

        protected abstract Tuple<int, int> GetCanvasLocation();

        protected abstract IEnumerable<Point> GetPoints();

        public static DrawLedLine Horizontal => new DrawLedHorizontalLine();

        public static DrawLedLine Vertical => new DrawLedVerticalLine();

        public static DrawLedLine Oblique => new DrawLedObliqueLine();

         

    }

    internal class DrawLedHorizontalLine : DrawLedLine
    {

        protected override Tuple<int, int> GetCanvasLocation()
        {
            if (HorizontalPostion == LedHorizontalPostion.Middle)
                return default(Tuple<int, int>);
            var offset = 57;
            if (this.VerticalPostion == LedVerticalPostion.Bottom)
                return new Tuple<int, int>(3, offset * 2);
            else if (VerticalPostion == LedVerticalPostion.Middle)
                return HorizontalPostion == LedHorizontalPostion.Left ? new Tuple<int, int>(4, offset) : new Tuple<int, int>(36, offset);

            return new Tuple<int, int>(3, 0);
        }

        protected override IEnumerable<Point> GetPoints()
        {
            if (HorizontalPostion == LedHorizontalPostion.Middle)
                return default(IEnumerable<Point>);
            if (VerticalPostion == LedVerticalPostion.Top)
                return "8,12 2,6 4,4 64,4 66,6 60,12".ParsePoints();
            else if (VerticalPostion == LedVerticalPostion.Bottom)
                return "4,12 2,10 8,4 60,4 66,10 64,12".ParsePoints();
            return "4,8 0,4 4,0 26,0 30,4 26,8".ParsePoints();
        }


    }

    internal class DrawLedVerticalLine : DrawLedLine
    {
        protected override Tuple<int, int> GetCanvasLocation()
        {
            if (VerticalPostion == LedVerticalPostion.Middle)
                return default(Tuple<int, int>);
            var left = HorizontalPostion == LedHorizontalPostion.Left ? 0 : HorizontalPostion == LedHorizontalPostion.Middle ? 31 : 62;
            var top = VerticalPostion == LedVerticalPostion.Top ? HorizontalPostion == LedHorizontalPostion.Middle ? 9 : 4
                : HorizontalPostion == LedHorizontalPostion.Middle ? 63 : 62;
            return new Tuple<int, int>(left, top);
        }

        protected override IEnumerable<Point> GetPoints()
        {
            if (VerticalPostion == LedVerticalPostion.Middle)
                return default(IEnumerable<Point>);

            if (VerticalPostion != LedVerticalPostion.Middle)
            {
                if (HorizontalPostion == LedHorizontalPostion.Left)
                    return "8,60 2,66 0,64 0,12 2,10 8,16".ParsePoints();
                else if (HorizontalPostion == LedHorizontalPostion.Right)
                    return "2,10 8,4 10,6 10,58 8,60 2,54".ParsePoints();
            }
            return (VerticalPostion == LedVerticalPostion.Top ? "4,6 8,6 12,6 12,52 8,56 4,52" : "4,8 8,4 12,8 12,54 8,54 4,54").ParsePoints();
        }
    }

    internal class DrawLedObliqueLine : DrawLedLine
    {
        protected override Tuple<int, int> GetCanvasLocation()
        {
            if (VerticalPostion == LedVerticalPostion.Middle || HorizontalPostion == LedHorizontalPostion.Middle)
                return default(Tuple<int, int>);
            var left = HorizontalPostion == LedHorizontalPostion.Left ? 8 : 39;
            var top = VerticalPostion == LedVerticalPostion.Top ? 15 : 67;
            return new Tuple<int, int>(left, top);
        }

        protected override IEnumerable<Point> GetPoints()
        {
            if (VerticalPostion == LedVerticalPostion.Middle || HorizontalPostion == LedHorizontalPostion.Middle)
                return default(IEnumerable<Point>);

            if (VerticalPostion == LedVerticalPostion.Top && HorizontalPostion == LedHorizontalPostion.Left
                || VerticalPostion == LedVerticalPostion.Bottom && HorizontalPostion == LedHorizontalPostion.Right)
                return "0,0 16,32".ParsePoints();

            return "10,60 26,28".ParsePoints();
        }

        public override Polyline Draw(Polyline line, LedHorizontalPostion hp, LedVerticalPostion vp)
        {
            var result = base.Draw(line, hp, vp);
            result.StrokeThickness = 8;
            if (result.Fill != null)
            {
                result.Stroke = result.Fill;
                result.Fill = Brushes.Transparent;
            }

            return result;
        }
    }
}
