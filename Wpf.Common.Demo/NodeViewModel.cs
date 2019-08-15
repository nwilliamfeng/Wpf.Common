using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Wpf.Common.Input;

namespace Wpf.Common.Demo
{
    public class NodeViewModel:PropertyChangedBase
    {
        private string _name;
        private static IEventAggregator eventAggregator;

        public NodeViewModel()
        {
            if (eventAggregator == null)
                eventAggregator = IoC.Get<IEventAggregator>();
        }

        public string Name
        {
            get => this._name;
            set
            {
                this._name = value;
                this.NotifyOfPropertyChange(() => this.Name);
            }
        }

        private int _code;
        public int Code
        {
            get => this._code;
            set
            {
                this._code = value;
                this.NotifyOfPropertyChange(() => this.Code);
            }
        }

        private ICommand _openCommand;

        public ICommand OpenCommand
        {
            get
            {
                return this._openCommand ?? (this._openCommand = new RelayCommand(() =>
                    {
                        eventAggregator.PublishOnUIThread(new NodeSelectEventArgs { NodeCode=this.Code});
                    }));
            }
        }
    }

    public class GroupNode
    {
        public string Name { get; set; }
        public ObservableCollection<NodeViewModel> Items { get; private set; } = new ObservableCollection<NodeViewModel>();
    }

    public class NodeSelectEventArgs : EventArgs
    {
        public int NodeCode { get; set; }
    }

    public class NodeCodes
    {
        public const int ERROR_TEMPLATE = 1;
    }
}
