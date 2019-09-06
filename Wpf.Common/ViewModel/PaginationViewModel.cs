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
    public class PaginationViewModel<T>:ViewModelBase
        where T:class,new()
    {
        private Func<int, Tuple<IEnumerable<T>, int>> GetListByPageNumber { get; set; }

        public PaginationViewModel(Func<int, Tuple< IEnumerable<T> ,int>> getListByPageNumber, int totalRecordCount,int pageSize=20)
        {
            this.GetListByPageNumber = getListByPageNumber ?? throw new ArgumentNullException("获取");
            this.PageSize = pageSize;
            this.TotalCount = totalRecordCount;
            this.Items=new ObservableCollection<T> ();
        }

        private int _pageSize;

        public int PageSize
        {
            get { return this._pageSize; }
            set
            {
                this._pageSize = value;
                this.NotifyPropertyChanged(nameof(PageSize));
            }
        }


        public ObservableCollection<T> Items { get; private set; }  

        private int _currentPageNumber;

        /// <summary>
        /// 获取或设置当前的页号
        /// </summary>
        public int CurrentPageNumber
        {
            get => this._currentPageNumber;
            set
            {
                if (value > this.PageCount) value = this.PageCount;
                if (value < 1) value = 1;
                this._currentPageNumber = value;
                this.NotifyPropertyChanged(nameof(CurrentPageNumber));
                this.LoadByPageAsync();
            }
        }

        /// <summary>
        /// 异步加载数据
        /// </summary>
        private async void LoadByPageAsync() 
        {
            var result = await Task.Run(() => this.GetListByPageNumber(this.CurrentPageNumber));
            this.Items.Clear();
            this.Items = new ObservableCollection<T>(result.Item1);
            if (this.TotalCount != result.Item2) //当记录总数和最新的不匹配则更新
            {
                this.TotalCount = result.Item2;
                if (this.CurrentPageNumber > this.PageCount) //如果当前的页号大于总页数（记录被删除了）则更新
                    this.CurrentPageNumber = this.PageCount;
            }
        }

        public int PageCount => (int)Math.Ceiling((decimal)TotalCount / PageSize);


        private int _totalRecordCount;

        public int TotalCount
        {
            get => _totalRecordCount;
            protected set
            {
                this._totalRecordCount = value;
                this.NotifyPropertyChanged(nameof(TotalCount));
                this.NotifyPropertyChanged(nameof(PageCount));
            }
        }


        private ICommand _previousCommand;

        public ICommand PreviousCommand
        {
            get { return this._previousCommand??(this._previousCommand=new RelayCommand(()=>
            {
                this.CurrentPageNumber -=1;
            },()=>this.CurrentPageNumber>1)); }            
        }


        private ICommand _nextCommand;

        public ICommand NextCommand
        {
            get
            {
                return this._nextCommand ?? (this._nextCommand = new RelayCommand(() =>
                {
                    this.CurrentPageNumber += 1;
                }, () => this.CurrentPageNumber <this.PageCount));
            }
        }

        private ICommand _firstCommand;

        public ICommand FirstCommand
        {
            get
            {
                return this._firstCommand ?? (this._firstCommand = new RelayCommand(() =>
                {
                    this.CurrentPageNumber = 1;
                }, () => this.CurrentPageNumber >1));
            }
        }

        private ICommand _lastCommand;

        public ICommand LastCommand
        {
            get
            {
                return this._lastCommand ?? (this._lastCommand = new RelayCommand(() =>
                {
                    this.CurrentPageNumber =this.PageCount;
                }, () => this.CurrentPageNumber < this.PageCount));
            }
        }

    }
}
