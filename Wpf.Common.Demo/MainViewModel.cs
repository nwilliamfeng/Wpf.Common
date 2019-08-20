using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Wpf.Common.Input;
using System.ComponentModel.Composition;
using Wpf.Common.Demo.Controls;

namespace Wpf.Common.Demo
{

    [Export(typeof(MainViewModel))]
    public class MainViewModel:Conductor<object>,IHandle<NodeSelectEventArgs>
    {
        public ObservableCollection<GroupNode> Nodes { get; private set; }
        
        [ImportingConstructor]
        public MainViewModel(IEventAggregator eventAggregator)
        {
             
            eventAggregator.Subscribe(this);
            this.Nodes = new ObservableCollection<GroupNode>();
            var gpNode1 = new GroupNode { Name="Template"};
            gpNode1.Items.Add(new NodeViewModel { Name = NodeNames.ERROR_TEMPLATE  });
          
            Nodes.Add(gpNode1);

            var gpNode2 = new GroupNode { Name = "Controls" };
            foreach(var field in typeof(NodeNames).GetFields())
            {
                var name = field.GetValue(new NodeNames()) as string;
                if (name != NodeNames.ERROR_TEMPLATE)
                    gpNode2.Items.Add(new NodeViewModel { Name = name });
            }

            Nodes.Add(gpNode2);
            this.DisplayName = "Demo";
        }

        private ICommand _openCommand;

        public   ICommand OpenCommand
        {
            get
            {
                return this._openCommand ?? (this._openCommand = new RelayCommand<NodeViewModelBase>(obj =>
                {
                    obj.OpenCommand.Execute(null);
                }));
            }
        }

     

        private string _selectedNodeName;

        public string SelectedNodeName
        {
            get => _selectedNodeName;
            set
            {
                this._selectedNodeName = value;
                this.NotifyOfPropertyChange(() => this.SelectedNodeName);
            }
        }

        void IHandle<NodeSelectEventArgs>.Handle(NodeSelectEventArgs arg)
        {
            this.SelectedNodeName = arg.Name;
            switch (arg.Name)
            {
                case NodeNames.ERROR_TEMPLATE:
                    this.ActivateItem(new ErrorTemplateViewModel());
                    break;

                case NodeNames.PASSWORD_BOX:
                    this.ActivateItem(new PasswordBoxViewModel());
                    break;

                case NodeNames.DATE_PICKER:
                    this.ActivateItem(new DatePickerViewModel());
                    break;
                case NodeNames.CHECK_BOX:
                    this.ActivateItem(new CheckBoxViewModel());
                    break;
                case NodeNames.COMBOBOX:
                    this.ActivateItem(new ComboBoxViewModel());
                    break;
                case NodeNames.MENU:
                    this.ActivateItem(new MenuViewModel());
                    break;
                case NodeNames.RADIO_BUTTON:
                    this.ActivateItem(new RadioViewModel());
                    break;
                case NodeNames.RANK:
                    this.ActivateItem(new RankViewModel());
                    break;
                case NodeNames.SLIDER:
                    this.ActivateItem(new SliderViewModel());
                    break;
                case NodeNames.TEXTBOX:
                    this.ActivateItem(new TextBoxViewModel());
                    break;
                case NodeNames.TOGGLE_BUTTON:
                    this.ActivateItem(new ToggleButtonViewModel());
                    break;
                case NodeNames.TAB_CONTROL:
                    this.ActivateItem(new TabControlViewModel());
                    break;
                case NodeNames.DROPDOWN_BUTTON:
                    this.ActivateItem(new DropdownButtonViewModel());
                    break;
                default:
                    break;
            }
        }
    }
}
