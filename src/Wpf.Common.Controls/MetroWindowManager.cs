using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Common.Behavior;
using Wpf.Common.Controls.Behavior;

namespace Wpf.Common.Controls
{
    public class MetroWindowManager : IMetroWindowManager
    {
        public bool? ShowDialog(object dialogViewModel)
        {
            var window = Application.Current.MainWindow;
            if (window == null) return null;
            if (!WindowChromeBehavior.GetIsEnable(window))
                return null;
            WindowChromeBehavior.SetDialogContent(window,dialogViewModel);
            return true;
        }
    }
}
