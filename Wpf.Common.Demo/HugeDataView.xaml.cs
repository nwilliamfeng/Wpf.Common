using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
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
    /// ScrollViewerView.xaml 的交互逻辑
    /// </summary>
    public partial class HugeDataView : UserControl
    {
        public HugeDataView()
        {
            InitializeComponent();
            this.DataContext = new HugeDataViewModel();
          
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as HugeDataViewModel;
            vm.Clear();
            for (int i = 0; i < 200000; i++)
            {
                vm.Add(new HugeDataItemViewModel { Index = i, Name = "name" + i.ToString(),Time =DateTime.Now });
            }

           
        }
    }



    public class HugeDataItemViewModel : INotifyPropertyChanged
    {
        public int Index { get; set; }

        public string Name { get; set; }

        public DateTime Time { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class HugeDataViewModel : INotifyPropertyChanged
    {

        public HugeDataViewModel()
        {

            this.Items =  CollectionViewSource.GetDefaultView(this._items);
          // this.Items.SortDescriptions.Add(new SortDescription { Direction = ListSortDirection.Ascending, PropertyName = "Time" });//不要设置!
           
           
        }

        private string _filter;

        public string Filter
        {
            get { return this._filter; }
            set
            {
                this._filter = value;
                this.PropertyChanged?.Invoke(this,new PropertyChangedEventArgs("Filter"));
                //触发filter
                this.Items.Filter = x =>
                {
                    return string.IsNullOrEmpty(Filter) ? true : (x as HugeDataItemViewModel).Name.Contains(Filter);
                };

            }
        }

        public void Clear()
        {
            this._items.Clear();
        }

        public void Add(HugeDataItemViewModel dataItem)
        {
            this._items.Add(dataItem);
        }

        public ICollectionView Items { get; private set; }

        private  ObservableCollection<HugeDataItemViewModel> _items  = new ObservableCollection<HugeDataItemViewModel>();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
