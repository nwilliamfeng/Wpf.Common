using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Text;
using Wpf.Common.Input;
using System.Threading.Tasks;

namespace Wpf.Common.ViewModel
{
  

    public class PaginationViewModel<T> : ViewModelBase
      where T : class, new()
    {
        private Func<int,int, Tuple<IEnumerable<T>, int>> _getListByPageNumber;

        public PaginationViewModel()
        {
            this.GetListByPageNumber = (page,size) => new Tuple<IEnumerable<T>, int>(default(IEnumerable<T>), 0);
            this.Items = new ObservableCollection<T>();
           
        }


        public Func<int,int, Tuple<IEnumerable<T>, int>> GetListByPageNumber
        {
            get { return _getListByPageNumber; }
            set
            {
                this._getListByPageNumber = value ?? throw new ArgumentNullException();
            }
        }


        private ObservableCollection<T> _items;
        public ObservableCollection<T> Items
        {
            get => this._items;
            protected set
            {
                this._items = value;
                this.NotifyPropertyChanged(nameof(Items));
            }
        }

        

        /// <summary>
        /// 异步加载数据
        /// </summary>
        private async void LoadByPageAsync(int page,int size)
        {
            var result = await Task.Run(() => this.GetListByPageNumber(page,size));
            this.Items.Clear();
            this.Items = new ObservableCollection<T>(result.Item1);
            if (this.TotalCount != result.Item2) //当记录总数和最新的不匹配则更新
            {
                this.TotalCount = result.Item2;

            }
        }

     

        private int _totalRecordCount;

        public int TotalCount
        {
            get => _totalRecordCount;
            set
            {
                this._totalRecordCount = value;
                this.NotifyPropertyChanged(nameof(TotalCount));           
            }
        }


        

        private ICommand _gotoCommand;

        public ICommand GotoCommand
        {
            get
            {
                return this._gotoCommand ?? (this._gotoCommand = new RelayCommand<Tuple<int, int>>(x =>
                {
                    this.LoadByPageAsync(x.Item1,x.Item2);
                } ));
            }
        }

    }
}
