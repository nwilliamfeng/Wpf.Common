using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Wpf.Common.Controls.Behavior
{
    public class DataGridColumnHeaderBehavior
    {
        
        public static DependencyProperty SortDirectionProperty =
            DependencyProperty.RegisterAttached("SortDirection",
                                                typeof(bool?),
                                                typeof(DataGridColumnHeaderBehavior),
                                                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));

        public static bool? GetSortDirection(DependencyObject target)
        {
            var value= target.GetValue(SortDirectionProperty);
            if (value == null) return null;
            return (bool)value;
        }

        public static void SetSortDirection(DependencyObject target, bool? value)
        {
            target.SetValue(SortDirectionProperty, value);
        }

    }
}
