using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Wpf.Common
{
    public static class VisualExtension
    {
        public static T GetDescendantByType<T>(this Visual element)
            where T:Visual
        {
            if (element == null)
            {
                return null;
            }
            if (element.GetType() == typeof(T))
            {
                return element as T;
            }
            Visual foundElement = null;
            if (element is FrameworkElement)
            {
                (element as FrameworkElement).ApplyTemplate();
            }
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                Visual visual = VisualTreeHelper.GetChild(element, i) as Visual;
                foundElement = GetDescendantByType<T>(visual);
                if (foundElement != null)
                {
                    break;
                }
            }
            return foundElement as T;
        }
    }
}
