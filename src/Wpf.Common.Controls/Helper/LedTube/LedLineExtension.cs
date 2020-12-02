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



    internal static class LedLineExtension
    {

        public static DrawLineType[] ExcludeObliques(this DrawLineType[] dts)
        {
           return dts.Except(new DrawLineType[] {
               DrawLineType.ObliqueLeftBottom,
               DrawLineType.ObliqueLeftTop,
               DrawLineType.ObliqueRightBottom,
               DrawLineType.ObliqueRightTop,
           }).ToArray();
        }

        public static DrawLineType[] ExcludeMiddleVerticals(this DrawLineType[] dts)
        {
            return dts.Except(new DrawLineType[] {
               DrawLineType.VerticalBottomMiddle,
               DrawLineType.VerticalTopMiddle,
               
           }).ToArray();
        }

        public static DrawLineType[] Exclude(this DrawLineType[] dts,params DrawLineType[] types )
        {
            return dts.Except(types).ToArray();        
        }

        public static DrawLineType[] ExcludeMiddleHorizontals(this DrawLineType[] dts)
        {
            return dts.Except(new DrawLineType[] {
               DrawLineType.HorizontalMiddleLeft,
               DrawLineType.HorizontalMiddleRight,

           }).ToArray();
        }

        public static DrawLineType[] ExcludeMiddles(this DrawLineType[] dts)
        {
            return dts.Except(new DrawLineType[] {
               DrawLineType.VerticalBottomMiddle,
               DrawLineType.VerticalTopMiddle,
               DrawLineType.HorizontalMiddleLeft,
               DrawLineType.HorizontalMiddleRight,

           }).ToArray();
        }

        public static Polyline DrawOblique(this Polyline line, LedHorizontalPostion p1, LedVerticalPostion p2)
        => DrawLedLine.Oblique.Draw(line, p1, p2);

        public static Polyline DrawHorizontal(this Polyline line, LedHorizontalPostion p1, LedVerticalPostion p2)
        => DrawLedLine.Horizontal.Draw(line, p1, p2);

        public static Polyline DrawVertical(this Polyline line, LedHorizontalPostion p1, LedVerticalPostion p2)
        => DrawLedLine.Vertical.Draw(line, p1, p2);


        public static Polyline Draw(this Polyline line,Canvas canvas, DrawLineType type)
        {
            switch (type)
            {
                case DrawLineType.HorizontalBottom:
                    line.DrawBottomHorizontal(canvas);
                    break;
                case DrawLineType.HorizontalMiddleLeft:
                    line.DrawMiddleLeftHorizontal(canvas);
                    break;
                case DrawLineType.HorizontalMiddleRight:
                    line.DrawMiddleRightHorizontal(canvas);
                    break;
                case DrawLineType.HorizontalTop:
                    line.DrawTopHorizontal(canvas);
                    break;
                case DrawLineType.ObliqueLeftBottom:
                    line.DrawObliqueLeftBottom(canvas);
                    break;
                case DrawLineType.ObliqueLeftTop:
                    line.DrawObliqueLeftTop(canvas);
                    break;
                case DrawLineType.ObliqueRightBottom:
                    line.DrawObliqueRightBottom(canvas);
                    break;
                case DrawLineType.ObliqueRightTop:
                    line.DrawObliqueRightTop(canvas);
                    break;
                case DrawLineType.VerticalBottomLeft:
                    line.DrawVerticalLeftBottom(canvas);
                    break;
                case DrawLineType.VerticalBottomMiddle:
                    line.DrawVerticalMiddleBottom(canvas);
                    break;
                case DrawLineType.VerticalBottomRight:
                    line.DrawVerticalRightBottom(canvas);
                    break;
                case DrawLineType.VerticalTopLeft:
                    line.DrawVerticalLeftTop(canvas);
                    break;
                case DrawLineType.VerticalTopMiddle:
                    line.DrawVerticalMiddleTop(canvas);
                    break;
                case DrawLineType.VerticalTopRight:
                    line.DrawVerticalRightTop(canvas);
                    break;
                default:
                    break;
                
            }

            return line;
        }

        private static void DrawTopHorizontal(this Polyline line, Canvas canvas)
        {

            canvas.Children.Add(line.DrawHorizontal(LedHorizontalPostion.Left, LedVerticalPostion.Top));
        }

        private static void DrawBottomHorizontal(this Polyline line, Canvas canvas)
        {

            canvas.Children.Add(line.DrawHorizontal(LedHorizontalPostion.Left, LedVerticalPostion.Bottom));
        }

        private static void DrawMiddleLeftHorizontal(this Polyline line, Canvas canvas)
        {

            canvas.Children.Add(line.DrawHorizontal(LedHorizontalPostion.Left, LedVerticalPostion.Middle));
        }

        private static void DrawMiddleRightHorizontal(this Polyline line, Canvas canvas)
        {

            canvas.Children.Add(line.DrawHorizontal(LedHorizontalPostion.Right, LedVerticalPostion.Middle));
        }

        private static void DrawVerticalLeftTop(this Polyline line, Canvas canvas)
        {
            canvas.Children.Add(line.DrawVertical(LedHorizontalPostion.Left, LedVerticalPostion.Top));
        }

        private static void DrawVerticalLeftBottom(this Polyline line, Canvas canvas)
        {

            canvas.Children.Add(line.DrawVertical(LedHorizontalPostion.Left, LedVerticalPostion.Bottom));
        }

        private static void DrawVerticalRightTop(this Polyline line, Canvas canvas)
        {

            canvas.Children.Add(line.DrawVertical(LedHorizontalPostion.Right, LedVerticalPostion.Top));
        }

        private static void DrawVerticalRightBottom(this Polyline line, Canvas canvas)
        {

            canvas.Children.Add(line.DrawVertical(LedHorizontalPostion.Right, LedVerticalPostion.Bottom));
        }

        private static void DrawVerticalMiddleTop(this Polyline line, Canvas canvas)
        {

            canvas.Children.Add(line.DrawVertical(LedHorizontalPostion.Middle, LedVerticalPostion.Top));
        }

        private static void DrawVerticalMiddleBottom(this Polyline line, Canvas canvas)
        {

            canvas.Children.Add(line.DrawVertical(LedHorizontalPostion.Middle, LedVerticalPostion.Bottom));
        }

        private static void DrawObliqueRightTop(this Polyline line, Canvas canvas)
        {

            canvas.Children.Add(line.DrawOblique(LedHorizontalPostion.Right, LedVerticalPostion.Top));
        }

        private static void DrawObliqueRightBottom(this Polyline line, Canvas canvas)
        {

            canvas.Children.Add(line.DrawOblique(LedHorizontalPostion.Right, LedVerticalPostion.Bottom));
        }

        private static void DrawObliqueLeftTop(this Polyline line, Canvas canvas)
        {

            canvas.Children.Add(line.DrawOblique(LedHorizontalPostion.Left, LedVerticalPostion.Top));
        }

        private static void DrawObliqueLeftBottom(this Polyline line, Canvas canvas)
        {
            canvas.Children.Add(line.DrawOblique(LedHorizontalPostion.Left, LedVerticalPostion.Bottom));
        }

    }


}
