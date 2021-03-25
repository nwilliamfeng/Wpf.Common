using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf.Common.Controls
{
    public class MetroWindowManager : IMetroWindowManager
    {
        public bool? ShowDialog(object dialogViewModel)
        {
            var window = Application.Current.MainWindow;
            if (window == null) return null;
        }
    }
}
