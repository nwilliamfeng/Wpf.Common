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
using System.Windows.Shapes;

namespace Wpf.Common.Demo
{
    /// <summary>
    /// ChromeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestChromeWindow : Window
    {
        public TestChromeWindow()
        {
            InitializeComponent();
            this.Loaded += TestChromeWindow_Loaded; 
        }

        private void TestChromeWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(this.Content);
        }

        
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("open command execute!");
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.tb.Text.Length>4;
        }
    }
}
