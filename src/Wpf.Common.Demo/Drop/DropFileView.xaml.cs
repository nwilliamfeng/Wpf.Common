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
using System.Collections.ObjectModel;
using Caliburn.Micro;
using Wpf.Common.Input;

namespace Wpf.Common.Demo
{
    /// <summary>
    /// DropFileView.xaml 的交互逻辑
    /// </summary>
    public partial class DropFileView : UserControl
    {
        public DropFileView()
        {
            InitializeComponent();
            this.DataContext = new CommandTest();           
        }
    }


    public class DropFileViewModel : PropertyChangedBase
    {
        private ICommand _loadFileCommand;

        public ICommand LoadFileCommand
       => this._loadFileCommand ?? (_loadFileCommand = new RelayCommand<IDataObject>(x =>
            {
                var sb = new StringBuilder(this.FileNames);

                x.GetDropFileNames().ToList().ForEach(n => sb.AppendLine(n));
                this.FileNames = sb.ToString();
            }));

        public string FileNames
        { get => _fileNames;
            set
            {
                _fileNames = value;
                this.NotifyOfPropertyChange(() => this.FileNames);
            }
        }

        private string _fileNames;
             
    }
}
