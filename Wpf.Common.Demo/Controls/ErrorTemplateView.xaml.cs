using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf.Common.Demo.Controls
{
    /// <summary>
    /// ErrorTemplateView.xaml 的交互逻辑
    /// </summary>
    public partial class ErrorTemplateView : UserControl
    {
        public ErrorTemplateView()
        {
            InitializeComponent();
            this.DataContext = new Customer();
        }
    }


    public class Customer : INotifyPropertyChanged,IDataErrorInfo
    {
        private string _name;

        public string this[string columnName]
        {
            get
            {
                if(columnName=="Name")
                    return string.IsNullOrEmpty(Name) ? "请输入名字" : null;
                return null;
            }
        }

        public string Name
        {
            get => this._name;
            set
            {
                this._name = value;
                this.PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        public string Error
        {
            get
            {
                return string.IsNullOrEmpty(Name) ? "请输入名字" : null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
