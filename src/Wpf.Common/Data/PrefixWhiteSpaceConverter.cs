using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Wpf.Common.Data
{
    public class PrefixWhiteSpaceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bv = (string)value;
            if (string.IsNullOrEmpty(bv)) return DependencyProperty.UnsetValue;
            int count = 0;
            if (!(int.TryParse(parameter as string, out count))) count = 1;
            var spaces = "";
            for (int i = 0; i < count; i++)
                spaces += " ";
            return spaces + bv;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
