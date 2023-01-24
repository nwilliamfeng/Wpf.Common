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
    /// MetroContentControlView.xaml 的交互逻辑
    /// </summary>
    public partial class MetroContentControlView : UserControl
    {
        public MetroContentControlView()
        {
            InitializeComponent();
            this.DataContext = new MetroContentControlViewModel();
        }
    }
 
    public class MetroContentControlViewModel : NotifyPropertyChangedObject
    {

    }
}
