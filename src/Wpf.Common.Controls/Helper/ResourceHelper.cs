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

        public static SolidColorBrush GetWindowActiveBorderBrush() => Application.Current.FindResource("ActiveWindowBorderBrush") as SolidColorBrush;

        public static SolidColorBrush GetWindowInactiveBorderBrush() => Application.Current.FindResource("InactiveWindowBorderBrush") as SolidColorBrush;

    }
}
