using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using System.IO;

namespace Wpf.Common.Demo.Controls
{
    /// <summary>
    /// ComboBoxView.xaml 的交互逻辑
    /// </summary>
    public partial class ComboBoxView : UserControl
    {
        public ComboBoxView()
        {
            InitializeComponent();
            this.DataContext = new ComboBoxViewModel(); 
        }
    }

     

    public class ComboBoxViewModel : Caliburn.Micro.PropertyChangedBase,IDataErrorInfo
    {

        private int _value;

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(Value) && Value == 0)
                    return "请选择值";

                return null;
            }
        }

        public int Value { get => _value;
            set
            {
                this._value = value;
                this.NotifyOfPropertyChange(() => this.Value);
            }

        }

        private FileAccess _fileAccess;



        public string Error => null;

        public FileAccess  Access { get => _fileAccess; set
            {
                _fileAccess = value;
                this.NotifyOfPropertyChange(() => this.Access);
            }
        }

       
    }

    public class FileAccessValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is FileAccess))
                return DependencyProperty.UnsetValue;
            switch ((FileAccess) value)
            {
                case FileAccess.Read:
                    return "只读";
                case FileAccess.ReadWrite:
                    return "读/写";
                case FileAccess.Write:
                    return "写";
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;
            if (string.IsNullOrEmpty(str))
                return DependencyProperty.UnsetValue;
            switch (str)
            {
                case "只读":
                    return FileAccess.Read;
                case "读/写":
                    return FileAccess.ReadWrite;
                case "写":
                    return FileAccess.Write;
            }
            return DependencyProperty.UnsetValue;
        }
    }
}
