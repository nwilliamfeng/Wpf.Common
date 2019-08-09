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

     

    public class ComboBoxViewModel : INotifyPropertyChanged,IDataErrorInfo
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
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }

        }

        public string Error => null;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
