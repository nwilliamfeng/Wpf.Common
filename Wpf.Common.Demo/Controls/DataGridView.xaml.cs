using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Caliburn.Micro;

namespace Wpf.Common.Demo.Controls
{
    /// <summary>
    /// DataGridView.xaml 的交互逻辑
    /// </summary>
    public partial class DataGridView : UserControl
    {
        public DataGridView()
        {
            InitializeComponent();
            this.DataContext = new DataGridViewModel();
        }
    }

    public class DataGridViewModel : Caliburn.Micro.PropertyChangedBase
    {
        public sealed class NormalOrder : PropertyChangedBase
        {
            public string InstrumentCode { get => _instrumentCode; set => Set(ref _instrumentCode, value); }
            private string _instrumentCode = null;

            public int Lots { get => _lots; set => Set(ref _lots, value); }
            public int _lots = 0;

            public NormalOrder(string code, int lots)
            {
                InstrumentCode = code;
                Lots = lots;
            }




        }

        private bool? _instrumentCodeSortDirection;

        public bool? InstrumentCodeSortDirection
        {
            get => _instrumentCodeSortDirection;
            set
            {
                this.Set(ref _instrumentCodeSortDirection, value);

                this.Datas.SortDescriptions.Clear();
                if (value == true)
                    this.Datas.SortDescriptions.Add(new SortDescription(nameof(NormalOrder.InstrumentCode), ListSortDirection.Ascending));
                if (value == false)
                    this.Datas.SortDescriptions.Add(new SortDescription(nameof(NormalOrder.InstrumentCode), ListSortDirection.Descending));
                this.Datas.Refresh();
            }
        }

        private ObservableCollection<NormalOrder> _datas { get; set; }

        public ICollectionView Datas { get; }

        public DataGridViewModel()
        {
            _datas = new ObservableCollection<NormalOrder>
            {
                new NormalOrder("cu2019", 10),
                new NormalOrder("cu2019", 9),
                new NormalOrder("cu2011", 6),
                new NormalOrder("cu2011", 12),
                new NormalOrder("cu2012", 17)
            };
            this.Datas = CollectionViewSource.GetDefaultView(_datas);

        }

    }
}
