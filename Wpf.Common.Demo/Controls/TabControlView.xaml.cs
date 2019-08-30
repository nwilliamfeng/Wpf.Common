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
using Wpf.Common.Behavior;
using Wpf.Common.Input;

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

        private void OnTabItemDrop(object sender, TabItemDropEventArgs args)
        {
            this.fireTB2.Text = "Event Sender:" + (sender as TabItem).Header.ToString();
            this.dragTB2.Text = "TabItemDropEventArgs :" + args.DroppedValue.Header.ToString();
        }

 
        
        private void TabItem_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed  )
            {
                DragDrop.DoDragDrop(sender as TabItem, sender, DragDropEffects.Move);
            }
            e.Handled = true;
        }

        private void TabItem_Drop(object sender, DragEventArgs e)
        {
            var tabItem = e.Data.GetData<TabItem>();
            if (tabItem == null)
                return;
            this.fireTB1.Text ="Event Sender:"+(sender as TabItem).Header.ToString();
            this.dragTB1.Text = "Drop object:" + tabItem.Header.ToString();
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

        public override bool Equals(object obj)
        {
            var other = obj as TabItemViewModel;
            if (other == null) return false;
            return this.Title == other.Title;
            
        }

        public override int GetHashCode()
        {           
            return string.IsNullOrEmpty(this.Title)?base.GetHashCode(): this.Title.GetHashCode();
        }
    }

    public class TabControlViewModel : Caliburn.Micro.PropertyChangedBase
    {
        private ICommand _setPositionCommand;

        /// <summary>
        /// 当tabitem执行drop后触发交换命令
        /// </summary>
        public ICommand SetPositionCommand =>
            this._setPositionCommand ?? (this._setPositionCommand = new RelayCommand<Tuple<object,object>>(x=>
            {
                var nwItem = x.Item2 as TabItemViewModel;
                var currItem = x.Item1 as TabItemViewModel;
                if (nwItem == null || currItem == null) return;
                this.Items.Remove(nwItem);
                var idx = this.Items.IndexOf(currItem);
                if (idx < 0) return;
                this.Items.Insert( idx, nwItem);
            }));

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
