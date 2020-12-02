using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;

namespace Wpf.Common.Data
{
    public class EnumVisibilityConverter : IValueConverter
    {
        public bool IsReverse { get; set; }

        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string parameterString = parameter as string;
            if (parameterString == null)
                return DependencyProperty.UnsetValue;

            if (Enum.IsDefined(value.GetType(), value) == false)
                return DependencyProperty.UnsetValue;

            object parameterValue = Enum.Parse(value.GetType(), parameterString);
            if (IsReverse)
            {
                if (!parameterValue.Equals(value))
                    return Visibility.Visible;
            }
            else   if (parameterValue.Equals(value))
                    return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string parameterString = parameter as string;
            if (parameterString == null)
                return DependencyProperty.UnsetValue;

            return Enum.Parse(targetType, parameterString);
        }
        #endregion
    }
}
