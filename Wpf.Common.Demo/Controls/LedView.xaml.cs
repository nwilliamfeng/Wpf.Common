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
        public const int HEIGHT = 24;
        public const int WIDTH = 24;
        public const int SPAN = 2;
        public const int HORIZONTAL_DIFF = 3;

        public abstract Line Draw(Line line , LedHorizontalPostion p1,LedVerticalPostion p2);

        public static DrawLedLine Horizontal => new DrawLedHorizontalLine();



    }

    public class DrawLedHorizontalLine : DrawLedLine
    {
        public override Line Draw(Line line, LedHorizontalPostion hp,LedVerticalPostion vp)
        {
           
            var pos= GetDefaultX1Y1(vp);
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

        private Tuple<int,int> GetDefaultX1Y1(LedVerticalPostion vp)
        {
            if (vp == LedVerticalPostion.Bottom)
                return new Tuple<int, int>( SPAN,98);
            else if (vp == LedVerticalPostion.Middle)
                return new Tuple<int, int>(SPAN + HORIZONTAL_DIFF,48);
            else if (vp == LedVerticalPostion.Top)
                return new Tuple<int, int>(SPAN + HORIZONTAL_DIFF * 2,0);
            return default(Tuple<int,int>);
        }
            
       
    }

    public class DrawLedVerticalLine : DrawLedLine
    {
        public override Line Draw(Line line, LedHorizontalPostion hp, LedVerticalPostion vp)
        {
           
            var pos = GetDefaultX1Y1(vp);
            line.X1 = pos.Item1;
            line.Y1 = line.Y2 = pos.Item2;
            switch (hp)
            {
                case LedHorizontalPostion.Left:
                    break;

                case LedHorizontalPostion.Right:
                    line.X1 += WIDTH + SPAN;
                    break;


                default: break;
            }
            line.X2 = line.X1 + WIDTH;
            return line;
        }

        private Tuple<int, int> GetDefaultX1Y1(LedHorizontalPostion hp)
        {
          //  < Line X1 = "3" Y1 = "50" X2 = "0" Y2 = "98" />
            if (hp == LedHorizontalPostion.Left)
                return new Tuple<int, int>(SPAN, 98);
            else if (vp == LedVerticalPostion.Middle)
                return new Tuple<int, int>(SPAN + HORIZONTAL_DIFF, 48);
            else if (vp == LedVerticalPostion.Top)
                return new Tuple<int, int>(SPAN + HORIZONTAL_DIFF * 2, 0);
            return default(Tuple<int, int>);
        }


    }
}
