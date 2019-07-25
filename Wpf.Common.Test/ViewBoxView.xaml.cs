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

namespace Wpf.Common.Test
{
    /// <summary>
    /// ViewBoxView.xaml 的交互逻辑
    /// </summary>
    public partial class ViewBoxView : UserControl
    {
        public ViewBoxView()
        {
            InitializeComponent();
        }

        private void ViewBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
               
                var vb = this.viewBox ;
                Console.WriteLine(vb.Width);
                vb.Width *=e.Delta>0? vb.Width>=vb.MaxWidth?1: 1.1 :0.95;
                vb.Height *= e.Delta >0 ? vb.Height >= vb.MaxHeight  ?1: 1.1 : 0.95;
            }
        }
    }
}
