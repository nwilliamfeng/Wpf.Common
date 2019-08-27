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
            this.canvas.Children.Add(new Polyline().DrawHorizontal(LedHorizontalPostion.Left, LedVerticalPostion.Bottom));
            this.canvas.Children.Add(new Polyline().DrawHorizontal(LedHorizontalPostion.Left, LedVerticalPostion.Middle));
            this.canvas.Children.Add(new Polyline().DrawHorizontal(LedHorizontalPostion.Right, LedVerticalPostion.Middle));
            this.canvas.Children.Add(new Polyline().DrawHorizontal(LedHorizontalPostion.Left, LedVerticalPostion.Top));

            this.canvas.Children.Add(new Polyline().DrawVertical(LedHorizontalPostion.Left, LedVerticalPostion.Top));
            this.canvas.Children.Add(new Polyline().DrawVertical(LedHorizontalPostion.Left, LedVerticalPostion.Bottom));

            this.canvas.Children.Add(new Polyline().DrawVertical(LedHorizontalPostion.Right, LedVerticalPostion.Top));
            this.canvas.Children.Add(new Polyline().DrawVertical(LedHorizontalPostion.Right, LedVerticalPostion.Bottom));

            this.canvas.Children.Add(new Polyline().DrawVertical(LedHorizontalPostion.Left, LedVerticalPostion.Middle));
            this.canvas.Children.Add(new Polyline().DrawVertical(LedHorizontalPostion.Right, LedVerticalPostion.Middle));

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
         

        public static Polyline DrawHorizontal(this Polyline line, LedHorizontalPostion p1, LedVerticalPostion p2)
        => DrawLedLine.Horizontal.Draw(line, p1, p2);

        public static Polyline DrawVertical(this Polyline line, LedHorizontalPostion p1, LedVerticalPostion p2)
       => DrawLedLine.Vertical.Draw(line, p1, p2);
    }

    public abstract class DrawLedLine
    {
        

        public  Polyline Draw(Polyline line , LedHorizontalPostion hp,LedVerticalPostion vp)
        {
            this.HorizontalPostion = hp;
            this.VerticalPostion = vp;
            var position = this.GetCanvasLocation( );
            var points = GetPoints( );
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


    }

    public class DrawLedHorizontalLine : DrawLedLine
    {
     

        protected override Tuple<int,int> GetCanvasLocation( )
        {
            var offset = 57;
            if (this.VerticalPostion == LedVerticalPostion.Bottom)
                return new Tuple<int, int>( 3, offset * 2);
            else if (VerticalPostion == LedVerticalPostion.Middle)
                return HorizontalPostion== LedHorizontalPostion.Left? new Tuple<int, int>(4, offset) : new Tuple<int, int>(36, offset);
           
            return new Tuple<int, int>(3,0);          
        }

        protected override IEnumerable<Point> GetPoints( )
        {           
            if (VerticalPostion == LedVerticalPostion.Top)
                return "8,12 2,6 4,4 64,4 66,6 60,12".ParsePoints();
            else if (VerticalPostion == LedVerticalPostion.Bottom)
                return "4,12 2,10 8,4 60,4 66,10 64,12".ParsePoints();
            return "4,8 0,4 4,0 26,0 30,4 26,8".ParsePoints();
        }


    }

    public class DrawLedVerticalLine : DrawLedLine
    {


        protected override Tuple<int, int> GetCanvasLocation()
        {
           
            var offset = 58;
            if (HorizontalPostion == LedHorizontalPostion.Left)
                return VerticalPostion == LedVerticalPostion.Top ? new Tuple<int, int>(4, 0) : new Tuple<int, int>(4 + offset, 0);

            else if (HorizontalPostion == LedHorizontalPostion.Right)
                return VerticalPostion == LedVerticalPostion.Top ? new Tuple<int, int>(4, 4+ offset) : new Tuple<int, int>(4 + offset, 4 + offset);
            else
                return VerticalPostion == LedVerticalPostion.Top ? new Tuple<int, int>(31, 9) : new Tuple<int, int>(31, 63);
        }

        protected override IEnumerable<Point> GetPoints()
        {
         
            if (HorizontalPostion == LedHorizontalPostion.Left)
                return "8,60 2,66 0,64 0,12 2,10 8,16".ParsePoints();
            else if (HorizontalPostion == LedHorizontalPostion.Right)
                return "2,10 8,4 10,6 10,58 8,60 2,54".ParsePoints();
            return (VerticalPostion == LedVerticalPostion.Top ? "4,6 8,6 12,6 12,52 8,56 4,52" : "4,8 8,4 12,8 12,54 8,54 4,54").ParsePoints();
        }


    }


}
