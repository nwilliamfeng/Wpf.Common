using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Wpf.Common.Controls
{
    internal static class ResourceHelper
    {

        public static Border GetWindowBorder()
        {
            var grid = GetWindowGrid();
            return grid == null ? null : grid.FindChildren<Border>("WindowBorder");
        }

        public static Border GetWindowGlowBorder()
        {
            var grid = GetWindowGrid();
            return grid == null ? null : grid.FindChildren<Border>("GlowBorder");
        }

        public static Grid GetWindowGrid() => Application.Current.FindResource("WindowGrid") as Grid;
       

        public static SolidColorBrush GetWindowActiveBorderBrush() => Application.Current.FindResource("window-border-active-brush") as SolidColorBrush;

        public static SolidColorBrush GetWindowInactiveBorderBrush() => Application.Current.FindResource("window-border-inactive-brush") as SolidColorBrush;

    }
}
