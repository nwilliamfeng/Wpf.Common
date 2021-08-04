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
    /// NumericUpDownView.xaml 的交互逻辑
    /// </summary>
    public partial class NumericUpDownView : UserControl
    {
        public NumericUpDownView()
        {
            InitializeComponent();
            this.DataContext = new NumericUpDownViewModel();
        }
    }

    public class NumericUpDownViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;

       
        private double? _value1;

        public double? Value1
        {
            get => _value1;
            set
            {
                this._value1 = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value1)));
            }
        }

        private int _value2;

        public int Value2
        {
            get => _value2;
            set
            {
                this._value2 = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value2)));
            }
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(Value1):
                        return Value1==null ? "请输入值" : null;
                   
                    default:
                        return null;
                }

            }
        }
    }
}
