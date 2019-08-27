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

namespace Wpf.Common.Demo.Controls
{
    

    /// <summary>
    /// LedView.xaml 的交互逻辑
    /// </summary>
    public partial class LedView : UserControl
    {

        public LedView()
        {
            InitializeComponent();
        
            this.canvas.Children.Add(new Line().DrawHorizontal(LedHorizontalPostion.Left, LedVerticalPostion.Bottom ));
            this.canvas.Children.Add(new Line().DrawHorizontal(LedHorizontalPostion.Right, LedVerticalPostion.Bottom));
            this.canvas.Children.Add(new Line().DrawHorizontal(LedHorizontalPostion.Left, LedVerticalPostion.Middle));
            this.canvas.Children.Add(new Line().DrawHorizontal(LedHorizontalPostion.Right, LedVerticalPostion.Middle));
            this.canvas.Children.Add(new Line().DrawHorizontal(LedHorizontalPostion.Left, LedVerticalPostion.Top));
            this.canvas.Children.Add(new Line().DrawHorizontal(LedHorizontalPostion.Right, LedVerticalPostion.Top));
        }
    }

    public class LedViewModel : Caliburn.Micro.PropertyChangedBase
    {
        
    }

    public enum LedHorizontalPostion
    {
        Left,
        Right,
    
       
    }

    public enum LedVerticalPostion
    {
        Bottom,
        Top,
        Middle,
    }

    public enum LedLineType
    {
        Horizontal,
        Vertical,
        Oblique,
    }

     

    public static class LedLineExtension
    {
        private static Line Draw(this Line line, LedHorizontalPostion p1,LedVerticalPostion p2,LedLineType type)
        {
            if (type == LedLineType.Horizontal)
                return DrawLedLine.Horizontal.Draw(line,p1,p2);
            return line;
        }


        public static Line DrawHorizontal(this Line line, LedHorizontalPostion p1, LedVerticalPostion p2)
        => Draw(line ,p1,p2, LedLineType.Horizontal);
    }

    public abstract class DrawLedLine
    {
        

        public abstract Line Draw(Polyline line , LedHorizontalPostion p1,LedVerticalPostion p2);

        public static DrawLedLine Horizontal => new DrawLedHorizontalLine();



    }

    public class DrawLedHorizontalLine : DrawLedLine
    {
        public override Line Draw(Polyline line, LedHorizontalPostion hp,LedVerticalPostion vp)
        {
           //< Polyline Points = "4,12 2,10 8,4 60,4 66,10 64,12" Canvas.Left = "3"  Canvas.Top = "114" />

          //   < Polyline Points = "4,8 0,4 4,0 26,0 30,4 26,8"  Canvas.Left = "4"   Canvas.Top = "57" />
            var pos= GetCanvasLocation(vp);
            line.X1 = pos.Item1;
            line.Y1 = line.Y2 = pos.Item2;
            switch (hp)
            {
                case LedHorizontalPostion.Left:
                    break;
               
                case LedHorizontalPostion.Right:
                    line.X1 += WIDTH + SPAN;
                    break;

               
                default:break;
            }
            line.X2 = line.X1 + WIDTH;
            return line;           
        }

        private Tuple<int,int> GetCanvasLocation(LedVerticalPostion vp,LedHorizontalPostion hp= LedHorizontalPostion.Left)
        {
            if (vp == LedVerticalPostion.Bottom)
                return new Tuple<int, int>( 3,114);
            else if (vp == LedVerticalPostion.Middle)
                return hp== LedHorizontalPostion.Left? new Tuple<int, int>(4,57) : new Tuple<int, int>(36, 57);
            else if (vp == LedVerticalPostion.Top)
                return new Tuple<int, int>(3,0);
            return default(Tuple<int,int>);
        }
            
       
    }

    //public class DrawLedVerticalLine : DrawLedLine
    //{
    //    public override Line Draw(Polyline line, LedHorizontalPostion hp, LedVerticalPostion vp)
    //    {
           
    //        var pos = GetDefaultX1Y1(vp);
    //        line.X1 = pos.Item1;
    //        line.Y1 = line.Y2 = pos.Item2;
    //        switch (hp)
    //        {
    //            case LedHorizontalPostion.Left:
    //                break;

    //            case LedHorizontalPostion.Right:
    //                line.X1 += WIDTH + SPAN;
    //                break;


    //            default: break;
    //        }
    //        line.X2 = line.X1 + WIDTH;
    //        return line;
    //    }

    //    private Tuple<int, int> GetDefaultX1Y1(LedHorizontalPostion hp)
    //    {
    //      //  < Line X1 = "3" Y1 = "50" X2 = "0" Y2 = "98" />
    //        if (hp == LedHorizontalPostion.Left)
    //            return new Tuple<int, int>(SPAN, 98);
    //        else if (vp == LedVerticalPostion.Middle)
    //            return new Tuple<int, int>(SPAN + HORIZONTAL_DIFF, 48);
    //        else if (vp == LedVerticalPostion.Top)
    //            return new Tuple<int, int>(SPAN + HORIZONTAL_DIFF * 2, 0);
    //        return default(Tuple<int, int>);
    //    }


    //}
}
