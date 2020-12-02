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
using Wpf.Common.Input;
using Caliburn.Micro;
using System.Collections.ObjectModel;

namespace Wpf.Common.Demo.Scroll
{
    /// <summary>
    /// ImagePerformanceView.xaml 的交互逻辑
    /// </summary>
    public partial class ScrollToLoadView : UserControl
    {
        public ScrollToLoadView()
        {
            InitializeComponent();
             
        }

        private void listBox_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
           
             
        }
    }

    public class ScrollItemViewModel : PropertyChangedBase
    {
        private string _url;
        public string Url
        {
            get => this._url;
            set
            {
                this._url = value;
                this.NotifyOfPropertyChange(() => this.Url);
            }
        }
    }


    public class ScrollToLoadViewModel : PropertyChangedBase
    {
        private List<ScrollItemViewModel> _itemCache  ;
        public ScrollToLoadViewModel()
        {
            this.Items = new ObservableCollection<ScrollItemViewModel>();
            this._itemCache = Enumerable.Range(0, 1000).Select(x => new ScrollItemViewModel { Url = x.ToString() }).ToList();
            this._itemCache.Take(50).ToList().ForEach(x => this.Items.Add(x));
        }

        public ObservableCollection<ScrollItemViewModel> Items { get; private set; }

        private ICommand _loadCommand;

        public ICommand LoadCommand
        {
            get
            {
                return this._loadCommand ?? (this._loadCommand = new RelayCommand(() =>
                    {
                        this.Items.Clear();
                        Enumerable.Range(0, 10000).Select(x => new ScrollItemViewModel { Url = x.ToString() }).ToList()
                        .ForEach(x=>Items.Add(x));
                    }));
            }
        }




        private ICommand _clearCommand;

        public ICommand ClearCommand
        {
            get
            {
                return this._clearCommand ?? (this._clearCommand = new RelayCommand(() =>
                {
                    this.Items.Clear();
                }));
            }
        }

        private ICommand _loadNextCommand;

        public ICommand LoadNextCommand
        {
            get
            {
                return this._loadNextCommand ?? (this._loadNextCommand = new RelayCommand(() =>
                {
                    var count = this.Items.Count;
                    this._itemCache.Skip(count).Take(50).ToList().ForEach(x =>
                    {
                        this.Items.Add(x);
                    }); 
                }));
            }
        }
    }
}
