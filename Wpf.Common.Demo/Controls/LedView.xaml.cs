using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
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
    /// LedView.xaml 的交互逻辑
    /// </summary>
    public partial class LedView : UserControl
    {

        public LedView()
        {
            InitializeComponent();



          
        }
 
    }

    public class LedViewModel : Caliburn.Micro.PropertyChangedBase
    {
        public ObservableCollection<char> Items { get; set; }

        public LedViewModel()
        {
          
            Items = new ObservableCollection<char>(Enumerable.Range((int)'0', 10)
                .Union(Enumerable.Range((int)'A', 26))               
                .Select(x => (char)x)
                .Union(new char[] { ' '}));
            
        }
    }

   


}
