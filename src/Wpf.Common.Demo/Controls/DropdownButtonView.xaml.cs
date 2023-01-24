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
using Wpf.Common.Input;


namespace Wpf.Common.Demo.Controls
{
    /// <summary>
    /// DropdownButton.xaml 的交互逻辑
    /// </summary>
    public partial class DropdownButtonView : UserControl
    {
        public DropdownButtonView()
        {
            InitializeComponent();
            this.DataContext = new DropdownButtonViewModel();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show((sender as MenuItem).Header.ToString());
      
        }

        private void MenuItem_Initialized(object sender, EventArgs e)
        {

        }

        private void MenuItem_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
    }

    public class DropdownButtonViewModel : NotifyPropertyChangedObject
    {
        private ICommand _openCommand;

        public ICommand OpenCommand
        => this._openCommand ?? (this._openCommand = new RelayCommand<OpenCommandType>(x =>
             {
                 MessageBox.Show(x.ToString());
             }));


    }

    public enum OpenCommandType
    {
        OpenA=0,

        OpenB=1,

        OpenC=2,
    }
}
