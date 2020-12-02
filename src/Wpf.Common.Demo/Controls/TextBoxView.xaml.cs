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
    /// TextBoxView.xaml 的交互逻辑
    /// </summary>
    public partial class TextBoxView : UserControl
    {
        public TextBoxView()
        {
            InitializeComponent();
            this.DataContext = new TextBoxViewModel();
        }
    }

    public class TextBoxViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;

        public string Name
        {
            get => this._name;
            set
            {
                this._name = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        private string _name2="abc";

        public string Name2
        {
            get => this._name2;
            set
            {
                this._name2 = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name2)));
            }
        }



        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(Name):
                        return string.IsNullOrEmpty(this.Name) ? "请输入名称" : null;
                    case nameof(Name2):
                        return string.IsNullOrEmpty(this.Name2) ? "请输入名称" : null;
                    default:
                        return null;
                }

            }
        }
    }
}
