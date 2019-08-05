using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Wpf.Common.Data
{

    public static class ValueConverters
    {
        public static IValueConverter NotNullToVisibilityConverter
        {
            get
            {

                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {

                    return value != null ? Visibility.Visible : Visibility.Collapsed;
                });
            }
        }

 
        public static IValueConverter NotEmptyToVisibilityConverter
        {
            get
            {

                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    if (value == null)
                        return Visibility.Collapsed;
                    if (!(value is string))
                        return Visibility.Visible;
                    return !string.IsNullOrEmpty(value.ToString()) ? Visibility.Visible : Visibility.Collapsed;
                });
            }
        }

 
        public static IValueConverter NullToVisibilityConverter
        {
            get
            {

                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {

                    return value == null ? Visibility.Visible : Visibility.Collapsed;
                });
            }
        }

        public static IValueConverter NullToBooleanConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {

                    return value == null ? false: true;
                });
            }
        }
        /// <summary>
        /// 字符大写转换
        /// </summary>
        public static IValueConverter CharUpperConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    if (!(value is string))
                        return DependencyProperty.UnsetValue;
                    string str = null;
                    foreach (char c in value as string)
                        str += char.ToUpper(c);
                    return str;
                });
            }
        }

        /// <summary>
        /// 字符小写转换
        /// </summary>
        public static IValueConverter CharLowerConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    if (!(value is string))
                        return DependencyProperty.UnsetValue;
                    string str = null;
                    foreach (char c in value as string)
                        str += char.ToLower(c);
                    return str;
                });
            }
        }

        /// <summary>
        /// 在前方增加空格
        /// </summary>
        public static IValueConverter PrefixWhiteSpaceConverter
        {
            get
            {             
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    var bv = (string)value;
                    if (string.IsNullOrEmpty(bv)) return DependencyProperty.UnsetValue;
                    int count = 0;
                    if (!(int.TryParse(parameter as string, out count))) count = 1;
                    var spaces = "";
                    for (int i = 0; i < count; i++)
                        spaces += " ";
                    return spaces+bv;
                });
            }
        }

        /// <summary>
        /// 在后方增加空格
        /// </summary>
        public static IValueConverter SuffixWhiteSpaceConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    var bv = (string)value;
                    if (string.IsNullOrEmpty(bv)) return DependencyProperty.UnsetValue;
                    int count = 0;
                    if (!(int.TryParse(parameter as string, out count))) count = 1;
                    var spaces = "";
                    for (int i = 0; i < count; i++)
                        spaces += " ";
                    return bv+spaces;
                });
            }
        }

        /// <summary>
        /// 布尔值转中文
        /// </summary>
        public static IValueConverter BooleanToChineseStringConverter
        {
            get
            {
                string TrueString = "是";
                string FalseString = "否";
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    bool bv = (bool)value;
                    return bv ? TrueString : FalseString;
                }, (value, targetType, parameter, cultInfo) =>
                {
                    string str = value as string;
                    return str == TrueString ? true : false;
                });
            }
        }

        /// <summary>
        /// 反向布尔值转换
        /// </summary>
        public static IValueConverter ReverseBooleanConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    bool bv = (bool)value;
                    return !bv;
                });
            }
        }

        /// <summary>
        /// 布尔值转Visibility
        /// </summary>
        public static IValueConverter BooleanToVisibilityConverter
        {
            get
            {

                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    bool bv = (bool)value;
                    return bv ? Visibility.Visible : Visibility.Collapsed;
                });
            }
        }


        public static IValueConverter BrushToColorConverter
        {
            get
            {

                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    SolidColorBrush brush =  value as SolidColorBrush;
                    if (brush == null) return DependencyProperty.UnsetValue;
                    return brush.Color;
                });
            }
        }

        public static IValueConverter BooleanToVisibilityConverter2
        {
            get
            {

                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    bool bv = (bool)value;
                    return bv ? Visibility.Visible : Visibility.Hidden;
                });
            }
        }

        public static IValueConverter ReverseBooleanToVisibilityConverter
        {
            get
            {

                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    bool bv = (bool)value;
                    return bv ? Visibility.Collapsed : Visibility.Visible;
                });
            }
        }

        /// <summary>
        /// 整型转换
        /// </summary>
        public static IValueConverter IntConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {

                    if (!(value is int))
                        return DependencyProperty.UnsetValue;
                    if (parameter is string)
                    {
                        int s = 0;
                        if (int.TryParse(parameter as string, out s))
                            return (int)value + s;
                    }

                    if (parameter != null && !(parameter is int))
                        return value;
                    int v = (int)value;
                    int parm = (int)parameter;
                    return v + parm;
                });
            }
        }

        public static IValueConverter DoubleConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    if (!(value is double))
                        return DependencyProperty.UnsetValue;
                    if (parameter is string)
                    {
                        double s = 0;
                        if (double.TryParse(parameter as string, out s))
                            return (double)value + s;
                    }
                    if (parameter != null && !(parameter is double))
                        return value;
                    double v = (double)value;
                    double parm = (double)parameter;
                    return v + parm;
                });
            }
        }

        /// <summary>
        /// 枚举值转Visibility
        /// </summary>
        public static IValueConverter EnumToVisibilityConverter
        {
            get
            {
                return new DelegateValueConverter((value, targetType, parameter, cultInfo) =>
                {
                    try
                    {
                        string parameterString = parameter as string;
                        if (parameterString == null)
                            return DependencyProperty.UnsetValue;

                        if (Enum.IsDefined(value.GetType(), value) == false)
                            return DependencyProperty.UnsetValue;

                        object parameterValue = Enum.Parse(value.GetType(), parameterString);
                        if (parameterValue.Equals(value))
                            return Visibility.Visible;
                        return Visibility.Collapsed;
                    }
                    catch
                    {

                        return Visibility.Visible;
                    }
                }, (value, targetType, parameter, cultInfo) =>
                {
                    try
                    {
                        string parameterString = parameter as string;
                        if (parameterString == null)
                            return DependencyProperty.UnsetValue;

                        return Enum.Parse(targetType, parameterString);
                    }
                    catch
                    {
                        return value;
                    }

                });
            }
        }
    }
}
