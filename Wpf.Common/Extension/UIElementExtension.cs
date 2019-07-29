using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

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



        public static T FindChildren<T>(this DependencyObject element,string name)
            where T:FrameworkElement
        {
            var items = FindChildren<T>(element);
            return items.OfType<FrameworkElement>().FirstOrDefault(x => x.Name == name) as T;
       
        }

        public static IEnumerable<T> FindChildren<T>(this DependencyObject element)
           where T : DependencyObject
        {
            var count = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                var t = child as T;
                if (t != null )
                    yield return t;
               var children= FindChildren<T>(child);
                foreach (var item in children)
                    yield return item;
                         
            }

        }

    }
}
