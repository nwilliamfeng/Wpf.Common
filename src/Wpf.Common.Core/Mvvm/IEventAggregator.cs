using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Common
{
    public interface IEventAggregator
    {
        IObservable<T> GetEvent<T>(string? eventSourceKey=null);

        bool RegistEvent<T>(IObservable<T> eventSource, string eventSourceKey = null) ;

        bool UnregistEvent<T>(IObservable<T> eventSource, string eventSourceKey = null) ;
    }

    
}
