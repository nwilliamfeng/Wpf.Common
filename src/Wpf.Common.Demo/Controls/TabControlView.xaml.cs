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

        private void TabItem_Drop_Normal(object sender, DragEventArgs e)
        {
            var source = e.Data.GetData<TabItem>();
            if (source == null)
                return;
            var target = sender as TabItem;
            this.fireTB1.Text = "Event Sender:" + target.Header.ToString();
            this.dragTB1.Text = "Drop object:" + source.Header.ToString();
            if (object.ReferenceEquals(source, target))
                return;
            tabControl.Items.Remove(source);
            var i = tabControl.Items.IndexOf(sender as TabItem);
            tabControl.Items.Insert(i<0?0:i, source);
            source.IsSelected = true;
        }

        private void TabItem_PreviewMouseMove_Normal(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(sender as TabItem, sender, DragDropEffects.Move);
            }
            e.Handled = true;
        }
    }

    public class TabItemViewModel : NotifyPropertyChangedObject
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

    public class TabControlViewModel : NotifyPropertyChangedObject
    {
        private ICommand _setPositionCommand;

        /// <summary>
        /// 当tabitem执行drop后触发交换命令
        /// </summary>
        public ICommand SetPositionCommand =>
            this._setPositionCommand ?? (this._setPositionCommand = new RelayCommand<Tuple<object,object,bool>>(x=>
            {
                var nwItem = x.Item2 as TabItemViewModel;
                var currItem = x.Item1 as TabItemViewModel;
                if (nwItem == null || currItem == null) return;
                if (object.ReferenceEquals(nwItem, currItem)) return;
                
                this.Items.Remove(nwItem);
                var idx = this.Items.IndexOf(currItem);
                if (idx < 0) return;
                this.Items.Insert(x.Item3? idx:idx+1, nwItem);
                this.SelectedItem = nwItem;
            }));

        private TabItemViewModel _selectedItem;

        public TabItemViewModel SelectedItem
        {
            get => this._selectedItem;

            set
            {
                this._selectedItem = value;
                this.NotifyOfPropertyChange(() => this.SelectedItem);
            }
        }

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
