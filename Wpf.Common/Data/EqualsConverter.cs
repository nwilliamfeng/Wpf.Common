using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Data;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows;

namespace Wpf.Common.Data
{
    public class EqualsConverter : IValueConverter
    {
        private object defaultValue = Binding.DoNothing;
        public object DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return object.Equals(value, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool && (bool)value)
                return parameter;

            return DefaultValue;
        }
    }
}
