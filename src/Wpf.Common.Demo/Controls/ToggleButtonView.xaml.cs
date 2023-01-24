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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf.Common.Demo.Controls
{
    /// <summary>
    /// ToggleButtonView.xaml 的交互逻辑
    /// </summary>
    public partial class ToggleButtonView : UserControl
    {
        public ToggleButtonView()
        {
            InitializeComponent();
        }
    }

    public class ToggleButtonViewModel : NotifyPropertyChangedObject, System.ComponentModel.IDataErrorInfo
    {
        public string this[string columnName]
        {
            get
            {
                if (columnName == "IsChecked")
                    return !this.IsChecked ? "请选择" : null;
                if (columnName == "IsChecked2")
                    return  this.IsChecked2 ? "请取消选择" : null;
                return null;
            }
        }

        private bool _isChecked;

        

        public string Error => null;

        public bool IsChecked
        {
            get => _isChecked;
            set{ _isChecked = value;
                this.NotifyOfPropertyChange(() => this.IsChecked);
            }
        }

        private bool _isChecked2=true;

        public bool IsChecked2
        {
            get => _isChecked2;
            set
            {
                _isChecked2 = value;
                this.NotifyOfPropertyChange(() => this.IsChecked2);
            }
        }
    }
}
