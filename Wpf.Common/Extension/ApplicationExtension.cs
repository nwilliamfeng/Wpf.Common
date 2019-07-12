using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace Wpf.Common
{
    public static class SingleInstanceHelper
    {
        public static void SetSingleInstance(this Application application, Mutex mutex, Action action)
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                action();
                var window = application.Windows.Cast<Window>().FirstOrDefault();
                if (window == null)
                {
                    Console.WriteLine("there is no window");
                    return;
                }
                HwndSource source = PresentationSource.FromVisual(window) as HwndSource;
                source.AddHook(WndProc);

                Application.Current.Exit += (s, arg) => mutex.ReleaseMutex();
            }
            else
            {
                NativeMethods.PostMessage(
                (IntPtr)NativeMethods.HWND_BROADCAST,
                NativeMethods.WM_SHOWME,
                IntPtr.Zero,
                IntPtr.Zero);
                Application.Current.Shutdown();
            }

        }

        public static void SetSingleInstance(this Application application, Action action=null)
        {
            if (action == null)
                action = new System.Action(() => { });
            SetSingleInstance(application, new Mutex(true, System.Reflection.Assembly.GetEntryAssembly().FullName), action);

        }


        //  TODO: BUG: HWND_BROADCAST WM_SHOWME后本方法并没有被触发, 不知何故???
        private static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == NativeMethods.WM_SHOWME)
            {
                ShowMe();
            }

            return IntPtr.Zero;
        }

        private static void ShowMe()
        {
            var mainWindow = Application.Current.MainWindow;
            //  TODO. Not tested.
            if (!mainWindow.IsVisible)
                mainWindow.Show();
            mainWindow.Activate();

            /*
            if (mainWindow.WindowState == WindowState.Minimized)
            {
                mainWindow.WindowState = WindowState.Normal;
            }
            
            // get our current "TopMost" value (ours will always be false though)
            var top = mainWindow.Topmost;
            // make our form jump to the top of everything
            mainWindow.Topmost = true;
            // set it back to whatever it was
            mainWindow.Topmost = top;
            */
        }

        internal class NativeMethods
        {
            public const int HWND_BROADCAST = 0xffff;

            public static readonly int WM_SHOWME = RegisterWindowMessage("WM_SHOWME");

            [DllImport("user32")]
            public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

            [DllImport("user32")]
            public static extern int RegisterWindowMessage(string message);
        }
    }
}
