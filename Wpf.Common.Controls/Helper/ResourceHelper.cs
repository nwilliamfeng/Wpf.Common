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

        public static Border GetWindowBorder() => Application.Current.FindResource("WindowBorder") as Border;

        public static SolidColorBrush GetWindowActiveBorderBrush() => Application.Current.FindResource("window-active-border-brush") as SolidColorBrush;

        public static SolidColorBrush GetWindowInactiveBorderBrush() => Application.Current.FindResource("window-inactive-border-brush") as SolidColorBrush;

    }
}
