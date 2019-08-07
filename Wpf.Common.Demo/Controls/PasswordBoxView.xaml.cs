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
    /// PasswordBoxView.xaml 的交互逻辑
    /// </summary>
    public partial class PasswordBoxView : UserControl
    {
        public PasswordBoxView()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel();
        }
    }

    public class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _password;

        public string Password
        {
            get => this._password;
            set
            {
                this._password = value;
                this.PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(Password)));
            }
        }

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
    }
}
