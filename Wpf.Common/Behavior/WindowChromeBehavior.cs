using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shell;

namespace Wpf.Common.Behavior
{
    public static class WindowChromeBehavior
    {
        public static readonly DependencyProperty IsEnableProperty = DependencyProperty.RegisterAttached("IsEnable",
            typeof(bool),
            typeof(WindowChromeBehavior),
            new PropertyMetadata(OnEnablePropertyChange));

        private static void OnEnablePropertyChange(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is bool) || !(bool)e.NewValue) return;
            if (!(obj is Window)) return;
            var window = obj as Window;

            window.WindowStyle = WindowStyle.None;
            window.ResizeMode = ResizeMode.CanResizeWithGrip;
            window.AllowsTransparency = true;

            ??

            WindowChrome.SetWindowChrome(window)

        }
    }
}
