﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
using System.Windows.Input;
using System.Collections.ObjectModel;
using Wpf.Common.Input;
using System.Windows.Media.Media3D;

namespace Wpf.Common.Demo
{
    public abstract class NodeViewModelBase : NotifyPropertyChangedObject
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

    internal class Unsubscriber<T> : IDisposable
    {
        private List<IObserver<T>> _observers;
        private IObserver<T> _observer;

        internal Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }

    public class NodeViewModel: NodeViewModelBase,IObservable<NodeSelectEventArgs>
    {
        private List<IObserver<NodeSelectEventArgs>> _observers=new List<IObserver<NodeSelectEventArgs>> ();
     
        private static IEventAggregator eventAggregator;

        public NodeViewModel()
        {
            if (eventAggregator == null)
                eventAggregator = IoC.Get<IEventAggregator>();
            eventAggregator.RegistEvent<NodeSelectEventArgs>(this);
        }
 

        private ICommand _openCommand;

        public override ICommand OpenCommand
        {
            get
            {
                return this._openCommand ?? (this._openCommand = new RelayCommand(() =>
                    {
                        _observers.ForEach(obs => obs.OnNext(new NodeSelectEventArgs(Name)));
                    }));
            }
        }

        public IDisposable Subscribe(IObserver<NodeSelectEventArgs> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
            return new Unsubscriber<NodeSelectEventArgs>(_observers, observer);
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
        public const string DROP = "Drop";

        public const string ERROR_TEMPLATE = "ErrorTemplate";

        public const string DATE_PICKER = "DatePicker";

        public const string PASSWORD_BOX = "PasswordBox";

        public const string RANK = "RankControl";

        public const string TEXTBOX = "TextBox";

        public const string NUMERIC_UPDOWN = "NumericUpDown";

        public const string COMBOBOX = "ComboBox";

        public const string MENU = "Menu";

        public const string SLIDER = "Slider";

        public const string TOGGLE_BUTTON = "ToggleButton";

        public const string DATAGRID = "DataGrid";

        public const string RADIO_BUTTON = "RadioButton";

        public const string CHECK_BOX = "CheckBox";

        public const string TAB_CONTROL = "TabControl";

        public const string DROPDOWN_BUTTON = "Dropdown & Split Button";

        public const string SCROLL_TO_LOAD = "Scroll To Load Data";

        public const string SCROLL_TO_SELECT = "Scroll To Select";

        public const string LED = "LED Control";

        public const string IMAGE_RENDER = "Image Render";

        public const string BRUSH = "Brush";

        public const string PROGRESS = "ProgressBar";

        public const string PAGINSTION = "Pagination";

        public const string ASSISTBUTTONGROUP = "AssistButtonGroup";

        public const string WINDOW = "WINDOW";

        public const string DIALOG = "DIALOG";

        public const string MetroContentControl = "MetroContentControl";
    }
}
