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

namespace Wpf.Common.Demo
{
    /// <summary>
    /// MemoryLeakView.xaml 的交互逻辑
    /// </summary>
    public partial class MemoryLeakView : UserControl
    {
        public MemoryLeakView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for(var i=0;i<100000;i++)
            this.textBox.TextChanged += (s,arg)=>
            {
                Console.WriteLine(this.textBox.Text);
            };
             
        }
    }
}
