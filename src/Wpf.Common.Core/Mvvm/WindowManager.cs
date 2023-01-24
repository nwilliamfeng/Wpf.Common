using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows;

namespace Wpf.Common
{
    public class WindowManager : IWindowManager
    {
        public virtual bool? ShowDialog(object rootModel, Action<Window> setting = null)
        {
            return CreateWindow(rootModel, isDialog: true, setting).ShowDialog();
        }


        public virtual void ShowWindow(object rootModel, Action<Window> setting = null)
        {
            Application current = Application.Current;
            CreateWindow(rootModel, isDialog: false, setting).Show();
        }

        public virtual void ShowPopup(object rootModel, Action close = null, Action<Popup> setting = null)
        {
            Popup popup = CreatePopup(setting);
            UIElement d = (popup.Child = ViewLocator.CreateViewForViewModel<UIElement>(rootModel));
            if (d is FrameworkElement fe)
                fe.DataContext = rootModel;
            if (close != null)
                popup.Closed += delegate
                {
                    close();
                };
            popup.IsOpen = true;
            popup.CaptureMouse();
        }

        protected virtual Popup CreatePopup(Action<Popup> setting)
        {
            Popup popup = new Popup();
            if (setting == null)
            {
                popup.Placement = PlacementMode.MousePoint;
                popup.AllowsTransparency = true;
            }
            else
                setting(popup);
            return popup;
        }


        protected virtual Window CreateWindow(object rootModel, bool isDialog, Action<Window> setting = null)
        {
            Window window = EnsureWindow(rootModel, ViewLocator.CreateViewForViewModel<Window>(rootModel), isDialog);
            window.DataContext = rootModel;
            setting?.Invoke(window);
            return window;
        }


        protected virtual Window EnsureWindow(object model, object view, bool isDialog)
        {
            Window window = view as Window;
            if (window == null)
            {
                window = new Window
                {
                    Content = view,
                    SizeToContent = SizeToContent.WidthAndHeight
                };

                Window window2 = GetOwnerWindow(window);
                if (window2 != null)
                {
                    window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    window.Owner = window2;
                }
                else
                {
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }
            }
            else
            {
                Window window3 = GetOwnerWindow(window);
                if (window3 != null && isDialog)
                {
                    window.Owner = window3;
                }
            }

            return window;
        }

        protected virtual Window GetOwnerWindow(Window window)
        {
            Application current = Application.Current;
            if (current == null) return null;
            Window result = current.Windows.OfType<Window>().FirstOrDefault((Window x) => x.IsActive);
            result = result ?? ((PresentationSource.FromVisual(current.MainWindow) == null) ? null : current.MainWindow);
            if (result != window)
                return result;
            return null;
        }
    }
}
