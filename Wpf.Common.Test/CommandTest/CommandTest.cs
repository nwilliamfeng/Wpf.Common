using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf.Common.Input;
using System.ComponentModel;
using System.Windows;

namespace Wpf.Common.Test
{
    public class CommandTest : INotifyPropertyChanged
    {
        private ICommand _loadCommand;

        public ICommand LoadFileCommand
        {
            get {
                return _loadCommand ?? (this._loadCommand = new RelayCommand<IDataObject>(obj =>
              {
                  StringBuilder sb = new StringBuilder();
                  obj.GetDropFileNames().ToList().ForEach(x => sb.AppendLine(x));
                  this.FileNames =sb.ToString();
              }));
            }
        }

        private string _fileNames;

        public string FileNames
        {
            get { return this._fileNames; }
            set
            {
                this._fileNames = value;
                this.PropertyChanged?.Invoke(this,new PropertyChangedEventArgs("FileNames"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
