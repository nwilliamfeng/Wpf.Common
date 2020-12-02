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

namespace Wpf.Common.Demo.Controls
{
    /// <summary>
    /// PaginationView.xaml 的交互逻辑
    /// </summary>
    public partial class PaginationView : UserControl
    {
        public PaginationView()
        {
            InitializeComponent();
        }
    }

    public class Record
    {
        public string Content { get; set; }

        public int Id { get; set; }
    }

    public class PaginationViewModel : Wpf.Common.ViewModel.PaginationViewModel<Record>
    {
        private List<Record> records = new List<Record>();

        public PaginationViewModel()
        {
            
            records.AddRange(Enumerable.Range(0, 63).Select(x => new Record { Id = x, Content = "item" + x.ToString() }));

            GetListByPageNumber = (page,size) =>
             {
                // System.Threading.Thread.Sleep(3000); //延迟测试
                 return new Tuple<IEnumerable<Record>, int>(records.Skip((page - 1) * size).Take(size), records.Count);
             };

        }

        private ICommand _changeMoreRecordCommand;

        public ICommand ChangeMoreCommand
        {
            get
            {
                return this._changeMoreRecordCommand ?? (this._changeMoreRecordCommand = new RelayCommand(() =>
                    {
                        records.AddRange(Enumerable.Range(63, 10).Select(x => new Record { Id = x, Content = "item" + x.ToString() }));
                    }));
            }
        }

        private ICommand _changeLessRecordCommand;

        public ICommand ChangeLessCommand
        {
            get
            {
                return this._changeLessRecordCommand ?? (this._changeLessRecordCommand = new RelayCommand(() =>
                {
                   records=  Enumerable.Range(0, 27).Select(x => new Record { Id = x, Content = "item" + x.ToString() }).ToList();
                }));
            }
        }
    }


}
