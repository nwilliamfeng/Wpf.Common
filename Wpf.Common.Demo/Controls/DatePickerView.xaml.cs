using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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

namespace Wpf.Common.Demo.Controls
{
    /// <summary>
    /// DateTimePickerView.xaml 的交互逻辑
    /// </summary>
    public partial class DatePickerView : UserControl
    {
        public DatePickerView()
        {
            InitializeComponent();
            this.DataContext = new DatePickerViewModel();
        }
    }

    public class DatePickerViewModel : INotifyPropertyChanged, IDataErrorInfo
    {

        private DateTime _date=DateTime.MinValue;

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(Date) && Date <DateTime.Now)
                    return "请选择日期";

                return null;
            }
        }

        public DateTime Date
        {
            get => _date;
            set
            {
                this._date = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Date)));
            }

        }

        public string Error => null;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
