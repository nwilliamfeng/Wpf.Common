using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.Common.Controls
{
    public static class ControlTemplateHelper
    {
        public const string PART_BorderName = "PART_Border";
        
        public const string PART_IconHostName = "PART_IconHost";

        internal static void InitializeControlBorderCornerRadius(this DependencyObject source, DependencyPropertyChangedEventArgs arg)
        {
            if (!(arg.NewValue is CornerRadius)) return;
            var value = (CornerRadius)arg.NewValue;
            var control = source as Control;
            if (control == null) return;
            control.Initialized += delegate
            {
                var border = control.FindChildrenFromTemplate<Border>(PART_BorderName);
                if (border != null)
                    border.CornerRadius = value;
            };
        }

        internal static void InitializeControlIcon(this DependencyObject source, DependencyPropertyChangedEventArgs arg,Dock dock)
        {
            var el = arg.NewValue as FrameworkElement;
            if (el == null) return;
            var control = source as Control;
            if (control == null) return;
            control.Initialized += delegate
            {
                var cc = control.FindChildrenFromTemplate<ContentControl>(PART_IconHostName);
                if (cc != null)
                {
                    cc.Content = el;
                    DockPanel.SetDock(cc, dock);
                }
            };
        }
    }
}
