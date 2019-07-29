using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.Common.Resources
{
    public static class WindowResourceHelper
    {

        public static Border GetWindowBorder() => Application.Current.FindResource("WindowBorder") as Border;

    }
}
