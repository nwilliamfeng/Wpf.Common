using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf.Common
{
    public static class UIElementExtension
    {
        /// <summary>
        /// 返回依赖属性的当前有效值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static T GetValue<T>(this UIElement element, DependencyProperty dp) => (T)element.GetValue(dp);



    }
}
