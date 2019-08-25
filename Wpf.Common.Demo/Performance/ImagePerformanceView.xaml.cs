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

namespace Wpf.Common.Demo.Performance
{
    /// <summary>
    /// ImagePerformanceView.xaml 的交互逻辑
    /// </summary>
    public partial class ImagePerformanceView : UserControl
    {
        public ImagePerformanceView()
        {
            InitializeComponent();
             
        }

        private void listBox_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
           
             
        }
    }

    public class ImageViewModel : PropertyChangedBase
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


    public class ImagePerformanceViewModel : PropertyChangedBase
    {
        private List<ImageViewModel> _imgCache  ;
        public ImagePerformanceViewModel()
        {
            this.Images = new ObservableCollection<ImageViewModel>();
            this._imgCache = Enumerable.Range(0, 10000).Select(x => new ImageViewModel { Url = x.ToString() }).ToList();
            this._imgCache.Take(50).ToList().ForEach(x => this.Images.Add(x));
        }

        public ObservableCollection<ImageViewModel> Images { get; private set; }

        private ICommand _loadCommand;

        public ICommand LoadCommand
        {
            get
            {
                return this._loadCommand ?? (this._loadCommand = new RelayCommand(() =>
                    {
                        this.Images.Clear();
                        Enumerable.Range(0, 100000).Select(x => new ImageViewModel { Url = x.ToString() }).ToList()
                        .ForEach(x=>Images.Add(x));
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
                    this.Images.Clear();
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
                    var count = this.Images.Count;
                    this._imgCache.Skip(count).Take(50).ToList().ForEach(x =>
                    {
                        this.Images.Add(x);
                    }); 
                }));
            }
        }
    }
}
