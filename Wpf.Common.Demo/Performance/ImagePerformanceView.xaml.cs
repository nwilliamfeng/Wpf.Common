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
           
            Console.WriteLine(e.VerticalChange);
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
        public ImagePerformanceViewModel()
        {
            this.ImageUrls = new ObservableCollection<ImageViewModel>();
        }

        public ObservableCollection<ImageViewModel> ImageUrls { get; private set; }

        private ICommand _loadCommand;

        public ICommand LoadCommand
        {
            get
            {
                return this._loadCommand ?? (this._loadCommand = new RelayCommand(() =>
                    {
                        for (int i = 0; i < 100000; i++)
                            this.ImageUrls.Add(new ImageViewModel { Url = i.ToString() });
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
                    this.ImageUrls.Clear();
                }));
            }
        }
    }
}
