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
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf.Common.Demo.Controls
{
    /// <summary>
    /// TabControl.xaml 的交互逻辑
    /// </summary>
    public partial class TabControlView : UserControl
    {
        public TabControlView()
        {
            InitializeComponent();
           
        }

         

        private void TabItem_PreviewDragOver(object sender, DragEventArgs e)
        {
             
        }

        private void TabItem_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed  )
            {
                Console.WriteLine("click");
             //   _isDraging = true;
                DragDrop.DoDragDrop(sender as TabItem, sender, DragDropEffects.Move);
            }
            e.Handled = true;


        }

        private void TabItem_Drop(object sender, DragEventArgs e)
        {
            var tabItem = e.Data.GetData<TabItem>();
            if (tabItem == null)
                return;
            Console.WriteLine($"raise drop event: {tabItem.DataContext}");
           
        }
    }

    public class TabItemViewModel : Caliburn.Micro.PropertyChangedBase
    {
        private string _title;

        public string Title
        {
            get => this._title;
            set
            {
                this._title = value;
                this.NotifyOfPropertyChange(() => this.Title);
            }
        }

        public override string ToString()
        {
            return this.Title;
        }
    }

    public class TabControlViewModel : Caliburn.Micro.PropertyChangedBase
    {

        public TabControlViewModel()
        {
            this.Items = new ObservableCollection<TabItemViewModel>();
            this.Items.Add(new TabItemViewModel { Title="title1" });
            this.Items.Add(new TabItemViewModel { Title = "title2" });
            this.Items.Add(new TabItemViewModel { Title = "title3" });
        }
        public ObservableCollection<TabItemViewModel> Items { get; set; }
    }
}
