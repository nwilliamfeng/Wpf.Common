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
            //  <Line X1="6" Y1="0" X2="3" Y2="48"    />
            //<Line X1="3" Y1="50" X2="0" Y2="98" />
            //<Line X1="8" Y1="0" X2="32" Y2="0" />
            //<Line X1="34" Y1="0" X2="58" Y2="0" />
            //<Line X1="57" Y1="48" X2="60" Y2="0" />
            //<Line X1="5" Y1="50" X2="29" Y2="50" />
            //<Line X1="31" Y1="50" X2="55" Y2="50" />

            //<Line X1="2" Y1="98" X2="26" Y2="98" />
            //<Line X1="28" Y1="98" X2="52" Y2="98" />

            //<Line X1="54" Y1="98" X2="57" Y2="50" /> 
            
            this.canvas.Children.Add(new Line { X1 = 6, Y1 = 0, X2 = 3, Y2 = 48 });
        }
    }

    public class LedViewModel : Caliburn.Micro.PropertyChangedBase
    {
        
    }

    public enum LedHorizontalPostion
    {
        Left,
        Right,
        Middle,
       
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
        public static Line DrawLine(this Line line, LedHorizontalPostion p1,LedVerticalPostion p2,LedLineType type)
        {
            
        }

        

    }

    public abstract class DrawLedLine
    {
        public const int HEIGHT = 24;
        public const int WIDTH = 24;

        public abstract Line Draw(Line line , LedHorizontalPostion p1,LedVerticalPostion p2);

     
        
    }

    public class DrawLedHorizontalLine : DrawLedLine
    {
        public override Line Draw(Line line, LedHorizontalPostion hp,LedVerticalPostion vp)
        {
            if (hp == LedHorizontalPostion.Middle) return line;
            line.X1 = GetDefaultX1(vp);
            switch (hp)
            {
                case LedHorizontalPostion.Left:
                    break;
               
                case LedHorizontalPostion.Right:
                    line.X1 += WIDTH + 2;
                    break;
                default:break;
            }
            line.X2 = line.X1 + WIDTH;
            return line;           
        }

        private int GetDefaultX1(LedVerticalPostion vp)
        {
            if (vp == LedVerticalPostion.Bottom)
                return 2;
            else if (vp == LedVerticalPostion.Middle)
                return 2 +3;
            else if (vp == LedVerticalPostion.Top)
                return 2+ 3+3;
            return 0;
        }
            
       
    }

}
