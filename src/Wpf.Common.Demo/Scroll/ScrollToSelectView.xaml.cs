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
 
using System.Collections.ObjectModel;

namespace Wpf.Common.Demo.Scroll
{
    /// <summary>
    /// ScrollToSelectView.xaml 的交互逻辑
    /// </summary>
    public partial class ScrollToSelectView : UserControl
    {
        public ScrollToSelectView()
        {
            InitializeComponent();
             
        }

        private void listBox_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var lb = sender as ListBox;
           var sv= lb.GetScrollViewer();
            if (sv.IsScrollEnd())
            {
               
            }
             
        }
    }


    public class ScrollItemGroupViewModel : NotifyPropertyChangedObject
    {
        public ScrollItemGroupViewModel(string groupName)
        {
            this.Items = new ObservableCollection<ScrollItemViewModel>();
            GroupName = groupName;
        }

        private Visibility _visibility= Visibility.Collapsed;
        public Visibility Visibility
        {
            get => _visibility;
            set
            {
                this.Set(ref _visibility, value);
            }
        }

        public string GroupName { get; }

        public ObservableCollection<ScrollItemViewModel> Items { get;}
    }

    public class ScrollToSelectViewModel : NotifyPropertyChangedObject
    {
        private ScrollItemGroupViewModel _selectedItem;
        public ScrollItemGroupViewModel SelectedItem
        {
            get => _selectedItem;
            set=>this.Set(ref  _selectedItem, value);
        }

        public ScrollToSelectViewModel()
        {
            this.Items = new ObservableCollection<ScrollItemGroupViewModel>();
            for (int i = 0; i < 5; i++)
            {
                var gp = new ScrollItemGroupViewModel($"gp{i + 1}");
                this.Items.Add(gp);
                for (int j = 0; j < 8; j++)
                {
                    gp.Items.Add(new ScrollItemViewModel() { Url = "url:" + (DateTime.Now.ToOADate() + j).ToString() });
                }
            }
            this.Items.First().Visibility = Visibility.Visible;
        }

        public ObservableCollection<ScrollItemGroupViewModel> Items { get; private set; }

        private ICommand _loadCommand;

        public ICommand LoadCommand
        {
            get
            {
                return this._loadCommand ?? (this._loadCommand = new RelayCommand(() =>
                    {
                      
                    }));
            }
        }




       

        
    }
}
