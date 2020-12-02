using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.Globalization;

namespace Wpf.Common.Data
{
  
    public class DelegateMultiValueConverter :IMultiValueConverter 
    {
        private Func<object[], Type, object, CultureInfo, object> _convertFunc;
        private Func<object, Type[], object, CultureInfo, object[]> _convertBackFunc;

        /// <summary>
        /// DelegateMultiValueConverter的构造函数
        /// </summary>
        /// <param name="convertFunc"></param>
        /// <param name="convertBackFunc"></param>
        public DelegateMultiValueConverter(Func<object[], Type, object, CultureInfo, object> convertFunc, Func<object, Type[], object, CultureInfo, object[]> convertBackFunc)
            : this(convertFunc)
        {
            this._convertBackFunc = convertBackFunc;
        }

        /// <summary>
        /// DelegateMultiValueConverter的构造函数
        /// </summary>
        /// <param name="convertFunc"></param>
        public DelegateMultiValueConverter(Func<object[], Type, object, CultureInfo, object> convertFunc)
        {
            this._convertFunc = convertFunc;
        }

      

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (this._convertFunc != null)
                return this._convertFunc.Invoke(values, targetType, parameter, culture);
            return values;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (this._convertBackFunc == null)
                return new object[] { };
            return this._convertBackFunc.Invoke(value, targetTypes, parameter, culture);
        }
    }

}
