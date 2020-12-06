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
using Wpf.Common.Input;

namespace Wpf.Common.Demo.Controls
{
    /// <summary>
    /// DialogWindowView.xaml 的交互逻辑
    /// </summary>
    public partial class WindowView : UserControl
    {
        public WindowView()
        {
            InitializeComponent();
            this.DataContext = new MetroContentControlViewModel();
        }

        private void openNormalWindowBtn_Click(object sender, RoutedEventArgs e) => new NormalWindow().Show();


        private void openToolWindowBtn_Click(object sender, RoutedEventArgs e) => new ToolWindow().Show();
         
    }

    public class WindowViewModel : Caliburn.Micro.PropertyChangedBase
    {
       
    }
 
}
