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

    internal enum LedHorizontalPostion
    {
        Left,
        Right,
        Middle,
    }

    internal enum LedVerticalPostion
    {
        Bottom,
        Top,
        Middle,
    }

    internal enum DrawLineType
    {      
        HorizontalTop,

        HorizontalBottom ,

        HorizontalMiddleLeft,

        HorizontalMiddleRight,

        VerticalTopLeft,

        VerticalBottomLeft,

        VerticalTopRight,

        VerticalBottomRight,

        VerticalTopMiddle,

        VerticalBottomMiddle,

        ObliqueLeftTop,

        ObliqueLeftBottom,

        ObliqueRightTop,

        ObliqueRightBottom,
    }

     
    
}
