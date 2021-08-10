using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Wpf.Common.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void NotifyOfPropertyChange([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        

        public virtual bool Set<T>(ref T oldValue, T newValue, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
                return false;
            oldValue = newValue;
            NotifyOfPropertyChange(propertyName ?? string.Empty);

            return true;
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
