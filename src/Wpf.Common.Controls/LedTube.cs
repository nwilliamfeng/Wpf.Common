﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Wpf.Common.Controls.Helper.LedTubes;

namespace Wpf.Common.Controls
{
    /// <summary>
    /// 米字数码管显示
    /// </summary>
    [TemplatePart(Name = PART_Canvas)]
    public class LedTube:Control
    {
        public const string PART_Canvas = "PART_Canvas";
        public const char CHAR_DEFAULT = ' ';
        public const char CHAR_0 = '0';
        public const char CHAR_1 = '1';
        public const char CHAR_2 = '2';
        public const char CHAR_3 = '3';
        public const char CHAR_4 = '4';
        public const char CHAR_5 = '5';
        public const char CHAR_6 = '6';
        public const char CHAR_7 = '7';
        public const char CHAR_8 = '8';
        public const char CHAR_9 = '9';
        public const char CHAR_A = 'A';
        public const char CHAR_B = 'B';
        public const char CHAR_C = 'C';
        public const char CHAR_D = 'D';
        public const char CHAR_E = 'E';
        public const char CHAR_F = 'F';
        public const char CHAR_G = 'G';
        public const char CHAR_H = 'H';
        public const char CHAR_I = 'I';
        public const char CHAR_J = 'J';
        public const char CHAR_K = 'K';
        public const char CHAR_L = 'L';
        public const char CHAR_M = 'M';
        public const char CHAR_N = 'N';
        public const char CHAR_O = 'O';
        public const char CHAR_P = 'P';
        public const char CHAR_Q = 'Q';
        public const char CHAR_R = 'R';
        public const char CHAR_S = 'S';
        public const char CHAR_T = 'T';
        public const char CHAR_U = 'U';
        public const char CHAR_V = 'V';
        public const char CHAR_W= 'W';
        public const char CHAR_X = 'X';
        public const char CHAR_Y = 'Y';
        public const char CHAR_Z = 'Z';

        private static Dictionary<char, DrawLineType[]> drawLineTypeDic;

        public static readonly DependencyProperty CharProperty = DependencyProperty.Register("Char", typeof(char), typeof(LedTube)
            , new PropertyMetadata(' ',OnCharValuePropertyChanged));

        public char Char
        {
            get => this.GetValue<char>(CharProperty);
            set => this.SetValue(CharProperty, value);
        }

        private static void OnCharValuePropertyChanged(DependencyObject obj,DependencyPropertyChangedEventArgs e)
        {
            var led = obj as LedTube;
            var value =(char) e.NewValue ;
            if (led == null || (!char.IsDigit(value) && !char.IsUpper(value)) ) return;

        }

        private void InitizeDrawTypes()
        {
            drawLineTypeDic = new Dictionary<char, DrawLineType[]>();
            drawLineTypeDic[CHAR_DEFAULT] = AllDrawLineTypes;
            drawLineTypeDic[CHAR_0] = AllDrawLineTypes.ExcludeObliques().ExcludeMiddles();
            drawLineTypeDic[CHAR_1] = new DrawLineType[] {DrawLineType.VerticalBottomRight,DrawLineType.VerticalTopRight };
            drawLineTypeDic[CHAR_2] = AllDrawLineTypes.ExcludeObliques().ExcludeMiddleVerticals().Exclude(  DrawLineType.VerticalTopLeft, DrawLineType.VerticalBottomRight ) ;
            drawLineTypeDic[CHAR_3] = AllDrawLineTypes.ExcludeObliques().ExcludeMiddleVerticals().Exclude(  DrawLineType.VerticalTopLeft, DrawLineType.VerticalBottomLeft ) ;
            drawLineTypeDic[CHAR_4] = new DrawLineType[] { DrawLineType.VerticalBottomRight , DrawLineType.VerticalTopLeft, DrawLineType.VerticalTopRight, DrawLineType.HorizontalMiddleLeft,DrawLineType.HorizontalMiddleRight };
            drawLineTypeDic[CHAR_5] = new DrawLineType[] { DrawLineType.HorizontalTop, DrawLineType.HorizontalBottom, DrawLineType.VerticalBottomRight, DrawLineType.VerticalTopLeft, DrawLineType.HorizontalMiddleLeft, DrawLineType.HorizontalMiddleRight };
            drawLineTypeDic[CHAR_6] = new DrawLineType[] { DrawLineType.HorizontalTop, DrawLineType.HorizontalBottom, DrawLineType.VerticalBottomRight, DrawLineType.VerticalBottomLeft, DrawLineType.VerticalTopLeft, DrawLineType.HorizontalMiddleLeft, DrawLineType.HorizontalMiddleRight };
            drawLineTypeDic[CHAR_7] = new DrawLineType[] { DrawLineType.HorizontalTop, DrawLineType.VerticalTopRight,  DrawLineType.VerticalBottomRight };
            drawLineTypeDic[CHAR_8] = AllDrawLineTypes.ExcludeObliques().ExcludeMiddleVerticals();
            drawLineTypeDic[CHAR_9] = AllDrawLineTypes.ExcludeObliques().ExcludeMiddleVerticals().Exclude(  DrawLineType.VerticalBottomLeft ) ;

            //处理字母
            drawLineTypeDic[CHAR_A] = AllDrawLineTypes.ExcludeObliques().ExcludeMiddleVerticals().Exclude(DrawLineType.HorizontalBottom);
            drawLineTypeDic[CHAR_B] = AllDrawLineTypes.ExcludeObliques().Exclude(DrawLineType.VerticalTopLeft, DrawLineType.VerticalBottomLeft,DrawLineType.HorizontalMiddleLeft);
            drawLineTypeDic[CHAR_C] = new DrawLineType[] { DrawLineType.HorizontalBottom, DrawLineType.HorizontalTop,   DrawLineType.VerticalBottomLeft, DrawLineType.VerticalTopLeft };
            drawLineTypeDic[CHAR_D] = new DrawLineType[] { DrawLineType.HorizontalBottom, DrawLineType.HorizontalTop, DrawLineType.VerticalBottomRight, DrawLineType.VerticalTopRight,DrawLineType.VerticalBottomMiddle,DrawLineType.VerticalTopMiddle };
            drawLineTypeDic[CHAR_E] = AllDrawLineTypes.ExcludeObliques().ExcludeMiddleVerticals().Exclude(DrawLineType.VerticalTopRight,DrawLineType.VerticalBottomRight);

            drawLineTypeDic[CHAR_F] = AllDrawLineTypes.ExcludeObliques().ExcludeMiddleVerticals().Exclude(DrawLineType.HorizontalBottom, DrawLineType.VerticalTopRight, DrawLineType.VerticalBottomRight);

            drawLineTypeDic[CHAR_G] = new DrawLineType[] {DrawLineType.VerticalBottomLeft,DrawLineType.VerticalTopLeft ,DrawLineType.HorizontalMiddleRight,  DrawLineType.HorizontalBottom, DrawLineType.HorizontalTop, DrawLineType.VerticalBottomRight,  };
            drawLineTypeDic[CHAR_H] = AllDrawLineTypes.ExcludeObliques().ExcludeMiddleVerticals().Exclude(DrawLineType.HorizontalBottom, DrawLineType.HorizontalTop);
            drawLineTypeDic[CHAR_I] = new DrawLineType[] { DrawLineType.HorizontalTop, DrawLineType.HorizontalBottom, DrawLineType.VerticalTopMiddle, DrawLineType.VerticalBottomMiddle };
            drawLineTypeDic[CHAR_J] = new DrawLineType[] {DrawLineType.VerticalBottomLeft,   DrawLineType.HorizontalBottom, DrawLineType.VerticalBottomRight, DrawLineType.VerticalTopRight };

            drawLineTypeDic[CHAR_K] = new DrawLineType[] {  DrawLineType.VerticalBottomLeft,DrawLineType.VerticalTopLeft, DrawLineType.HorizontalMiddleLeft,DrawLineType.ObliqueRightBottom,DrawLineType.ObliqueRightTop };
            drawLineTypeDic[CHAR_L] = new DrawLineType[] { DrawLineType.VerticalBottomLeft, DrawLineType.VerticalTopLeft,  DrawLineType.HorizontalBottom };
            drawLineTypeDic[CHAR_M] = AllDrawLineTypes.ExcludeMiddles().Exclude(DrawLineType.HorizontalTop, DrawLineType.HorizontalBottom, DrawLineType.ObliqueLeftBottom,DrawLineType.ObliqueRightBottom);
            drawLineTypeDic[CHAR_N] = AllDrawLineTypes.ExcludeMiddles().Exclude(DrawLineType.HorizontalTop, DrawLineType.HorizontalBottom, DrawLineType.ObliqueLeftBottom, DrawLineType.ObliqueRightTop);
            drawLineTypeDic[CHAR_O] = AllDrawLineTypes.ExcludeMiddles().Exclude(DrawLineType.ObliqueLeftTop,  DrawLineType.ObliqueRightBottom);

            drawLineTypeDic[CHAR_P] = AllDrawLineTypes.ExcludeMiddleVerticals().ExcludeObliques().Exclude(DrawLineType.HorizontalBottom, DrawLineType.VerticalBottomRight);
            drawLineTypeDic[CHAR_Q] = AllDrawLineTypes.ExcludeObliques().ExcludeMiddles().Union(new DrawLineType[] { DrawLineType.ObliqueRightBottom }).ToArray();

            drawLineTypeDic[CHAR_R] = new DrawLineType[] {DrawLineType.HorizontalMiddleLeft,DrawLineType.HorizontalMiddleRight,  DrawLineType.VerticalTopLeft,DrawLineType.VerticalBottomLeft,DrawLineType.HorizontalTop,DrawLineType.VerticalTopRight,DrawLineType.ObliqueRightBottom };
            drawLineTypeDic[CHAR_S] = new DrawLineType[] {DrawLineType.HorizontalTop,   DrawLineType.ObliqueLeftTop,DrawLineType.HorizontalMiddleRight,DrawLineType.VerticalBottomRight ,DrawLineType.HorizontalBottom };
            drawLineTypeDic[CHAR_T] = new DrawLineType[] {DrawLineType.HorizontalTop, DrawLineType.VerticalBottomMiddle,DrawLineType.VerticalTopMiddle };
            drawLineTypeDic[CHAR_U] = AllDrawLineTypes.ExcludeMiddles().ExcludeObliques().Exclude(DrawLineType.HorizontalTop);
            drawLineTypeDic[CHAR_V] = new DrawLineType[] {  DrawLineType.VerticalBottomLeft,DrawLineType.VerticalTopLeft,DrawLineType.ObliqueLeftBottom,DrawLineType.ObliqueRightTop };
            drawLineTypeDic[CHAR_W] = AllDrawLineTypes.ExcludeMiddles().Exclude(DrawLineType.HorizontalTop,DrawLineType.HorizontalBottom,DrawLineType.ObliqueLeftTop,DrawLineType.ObliqueRightTop);
            drawLineTypeDic[CHAR_X] = new DrawLineType[] {DrawLineType.ObliqueLeftTop,DrawLineType.ObliqueLeftBottom,DrawLineType.ObliqueRightBottom,DrawLineType.ObliqueRightTop};
            drawLineTypeDic[CHAR_Y] = new DrawLineType[] { DrawLineType.ObliqueLeftTop, DrawLineType.VerticalBottomMiddle,   DrawLineType.ObliqueRightTop };
            drawLineTypeDic[CHAR_Z] = new DrawLineType[] { DrawLineType.HorizontalBottom,DrawLineType.HorizontalTop,DrawLineType.ObliqueLeftBottom,DrawLineType.ObliqueRightTop };

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (drawLineTypeDic == null)
                this.InitizeDrawTypes();
            var canvas = this.GetTemplateChild(PART_Canvas) as Canvas;
            if (drawLineTypeDic.ContainsKey(this.Char))
                drawLineTypeDic[this.Char]
                    .ToList()
                    .ForEach(x => this.CreateLine().Draw(canvas, x));          
        }

        private DrawLineType[] AllDrawLineTypes => Enum.GetValues(typeof(DrawLineType)).OfType<DrawLineType>().ToArray();

      
        private Polyline CreateLine()
        {
            return new Polyline { Fill = this.Foreground, Stretch = Stretch.Uniform };
        }
    }

    
}
