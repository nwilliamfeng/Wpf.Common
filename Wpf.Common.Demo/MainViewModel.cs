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
using Wpf.Common.Demo.Scroll;
using Wpf.Common.Demo.Image;
using System.Windows;
using Wpf.Common.Controls;

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
           
            var gpNode2 = new GroupNode { Name = "Controls" };         
            gpNode2.Items.Add(new NodeViewModel { Name = NodeNames.CHECK_BOX });
            gpNode2.Items.Add(new NodeViewModel { Name = NodeNames.COMBOBOX });
            gpNode2.Items.Add(new NodeViewModel { Name = NodeNames.DATE_PICKER });
            gpNode2.Items.Add(new NodeViewModel { Name = NodeNames.TOGGLE_BUTTON });
            gpNode2.Items.Add(new NodeViewModel { Name = NodeNames.MENU });
            gpNode2.Items.Add(new NodeViewModel { Name = NodeNames.PASSWORD_BOX });
            gpNode2.Items.Add(new NodeViewModel { Name = NodeNames.TEXTBOX });
            gpNode2.Items.Add(new NodeViewModel { Name = NodeNames.RADIO_BUTTON });
            gpNode2.Items.Add(new NodeViewModel { Name = NodeNames.TAB_CONTROL });
            gpNode2.Items.Add(new NodeViewModel { Name = NodeNames.SLIDER });
            gpNode2.Items.Add(new NodeViewModel { Name = NodeNames.PROGRESS });
            


            var gpNode3 = new GroupNode { Name = "Behavior" };
            gpNode3.Items.Add(new NodeViewModel { Name = NodeNames.DROP });

            var gpNode4 = new GroupNode { Name = "Performance" };
            gpNode4.Items.Add(new NodeViewModel { Name = NodeNames.SCROLL_TO_LOAD });

            var gpNode5 = new GroupNode { Name = "Brush&Drawing" };
            gpNode5.Items.Add(new NodeViewModel { Name = NodeNames.BRUSH });
         

            var gpNode6 = new GroupNode { Name = "Custom Controls" };
            gpNode6.Items.Add(new NodeViewModel { Name = NodeNames.DROPDOWN_BUTTON });
            gpNode6.Items.Add(new NodeViewModel { Name = NodeNames.LED });
            gpNode6.Items.Add(new NodeViewModel { Name = NodeNames.RANK });
            gpNode6.Items.Add(new NodeViewModel { Name = NodeNames.PAGINSTION });
            gpNode6.Items.Add(new NodeViewModel { Name = NodeNames.ASSISTBUTTONGROUP });

            var gpNode7 = new GroupNode { Name = "Image" };
            gpNode7.Items.Add(new NodeViewModel { Name = NodeNames.IMAGE_RENDER });



            Nodes.Add(gpNode1);
            Nodes.Add(gpNode2);
            Nodes.Add(gpNode3);
            Nodes.Add(gpNode4);
            Nodes.Add(gpNode5);
            Nodes.Add(gpNode6);
            Nodes.Add(gpNode7);
            this.DisplayName = "Demo";
        }

        private bool _isDarkTheme;

        public bool IsDarkTheme
        {
            get => this._isDarkTheme;
            set
            {
                this._isDarkTheme = value;
                if (value)
                    Application.Current.SetDarkTheme();
                else
                    Application.Current.SetDefaultTheme();
                this.NotifyOfPropertyChange(() => this.IsDarkTheme);
            }
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
                case NodeNames.DROP:
                    this.ActivateItem(new DropFileViewModel());
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

                case NodeNames.SCROLL_TO_LOAD:
                    this.ActivateItem(new ScrollToLoadViewModel());
                    break;

                case NodeNames.PROGRESS:
                    this.ActivateItem(new ProgressViewModel());
                    break;

                case NodeNames.LED:
                    this.ActivateItem(new LedViewModel());
                    break;

                case NodeNames.IMAGE_RENDER:
                    this.ActivateItem(new ImageRenderViewModel());
                    break;

                case NodeNames.BRUSH:
                    this.ActivateItem(new BrushViewModel());
                    break;
                case NodeNames.PAGINSTION:
                    this.ActivateItem(new PaginationViewModel());
                    break;
                case NodeNames.ASSISTBUTTONGROUP:
                    this.ActivateItem(new AssistButtonGroupViewModel());
                    break;
                default:
                    break;
            }
        }
    }
}
