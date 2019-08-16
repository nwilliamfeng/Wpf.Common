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
    public abstract class NodeViewModelBase : PropertyChangedBase
    {
        private string _name;
        public string Name
        {
            get => this._name;
            set
            {
                this._name = value;
                this.NotifyOfPropertyChange(() => this.Name);
            }
        }

        

        public abstract ICommand OpenCommand { get; }
        
    }


    public class NodeViewModel: NodeViewModelBase
    {
     
        private static IEventAggregator eventAggregator;

        public NodeViewModel()
        {
            if (eventAggregator == null)
                eventAggregator = IoC.Get<IEventAggregator>();
        }

        

       

        private ICommand _openCommand;

        public override ICommand OpenCommand
        {
            get
            {
                return this._openCommand ?? (this._openCommand = new RelayCommand(() =>
                    {
                        eventAggregator.PublishOnUIThread(new NodeSelectEventArgs( Name));
                    }));
            }
        }
    }

    public class GroupNode : NodeViewModelBase
    {
        private ICommand _openCommand;

        public override ICommand OpenCommand
        {
            get
            {
                return this._openCommand ?? (this._openCommand = new RelayCommand(() =>
                {
                     
                }));
            }
        }

        public ObservableCollection<NodeViewModel> Items { get; private set; } = new ObservableCollection<NodeViewModel>();

      
    }

    public class NodeSelectEventArgs : EventArgs
    {
        public NodeSelectEventArgs( string name)
        {
           
            this.Name=name;
        }
     

        public string Name { get; private set; }
    }



    public class NodeNames
    {
        public const string ERROR_TEMPLATE = "ErrorTemplate";

        public const string DATE_PICKER = "DatePicker";

        public const string PASSWORD_BOX = "PasswordBox";

        public const string RANK = "RankControl";

        public const string TEXTBOX = "TextBox";

        public const string COMBOBOX = "ComboBox";

        public const string MENU = "Menu";

        public const string SLIDER = "Slider";

        public const string TOGGLE_BUTTON = "ToggleButton";

        public const string RADIO_BUTTON = "RadioButton";

        public const string CHECK_BOX = "CheckBox";
    }
}
