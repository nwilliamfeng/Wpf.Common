using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Common
{
    public static class KeyboardHelper
    {
        /// <summary>
        /// 返回物理键盘名称
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetKeyboardNames()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select Name from Win32_Keyboard");
            foreach (ManagementObject keyboard in searcher.Get())
            {
                if (!keyboard.GetPropertyValue("Name").Equals(string.Empty))
                    yield return keyboard.GetPropertyValue("Name").ToString();
            }
        }

        /// <summary>
        /// 是否存在键盘
        /// </summary>
        /// <returns></returns>
        public static bool ExistKeyboard() => GetKeyboardNames().Any();
    }
}
