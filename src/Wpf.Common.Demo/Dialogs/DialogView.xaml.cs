using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
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
using System.Globalization;
using System.IO;
using Caliburn.Micro;
using Wpf.Common.Input;
using Wpf.Common.Events;

namespace Wpf.Common.Demo
{
    /// <summary>
    /// DialogView.xaml 的交互逻辑
    /// </summary>
    public partial class DialogView : UserControl
    {
        public DialogView()
        {
            InitializeComponent();
            this.DataContext = new DialogViewModel();
        }

     
         
    }

    public class MessageDialogViewModel : PropertyChangedBase
    {
        
        public string Title { get; set; }

        private string _content;

        public string Content {

            get => _content;
            set => this.Set(ref _content, value);
        }

        public ICommand CloseCommand { get; set; }
    }

    public class DialogViewModel : Caliburn.Micro.PropertyChangedBase
    {
        private ICommand _openNormalDialogCommand;

        public ICommand OpenNormalDialogCommand =>
            _openNormalDialogCommand = new RelayCommand(() =>
              {
                  IoC.Get<IWindowManager>().ShowDialog(new MessageDialogViewModel { Title = "dialog", Content = "abcd" });
              });

        private ICommand _openMetroDialogCommand;

        public ICommand OpenMetroDialogCommand =>
            _openMetroDialogCommand = new RelayCommand(async () =>
            {
                var dialog = new MessageDialogViewModel { Title = "dialog", Content = "abcd" };
                dialog.CloseCommand = new RelayCommand(() =>
                 {
                     //  IoC.Get<IEventAggregator>().PublishOnUIThread(new CloseMetroDialogEventArgs(dialog));
                 });
                var dr = await IoC.Get<IMetroWindowManager>().ShowDialog(dialog);
                if (dr == true)
                {
                    MessageBox.Show("ok");
                }
                else if (dr == false)
                {
                    MessageBox.Show("cancel");
                }
                //   IoC.Get<IEventAggregator>().PublishOnUIThread( new OpenMetroDialogEventArgs(dialog) );
            });
    }
 
}
