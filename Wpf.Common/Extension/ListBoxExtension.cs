using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Wpf.Common
{
    public static class ListBoxExtension
    {
        public static ScrollViewer GetScrollViewer(this ListBox listBox) => listBox.GetDescendantByType<ScrollViewer>();
    }
}
