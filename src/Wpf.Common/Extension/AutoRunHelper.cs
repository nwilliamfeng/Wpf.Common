using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf.Common
{
    public static class AutoRunHelper
    {
        public const string REGISTRY_KEY = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

        /// <summary>
        /// 返回当前App是否是系统启动后自动运行
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public static bool IsAutoRunWhenWindowsStartup(this Application application)
        {
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey(REGISTRY_KEY, true);
            var appName = application.GetType().Assembly.GetName().Name;
            return rkApp.GetValue(appName) != null;        
        }

        /// <summary>
        /// 设置当前应用为系统启动后自动运行
        /// </summary>
        /// <param name="application"></param>
        public static void EnableAutorunWhenWindowsStartup(this Application application)
        {
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey(REGISTRY_KEY, true);
            var appName = application.GetType().Assembly.GetName().Name;
            var path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            rkApp.SetValue(appName, path);
        }
    }
}
