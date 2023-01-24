using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;


namespace Wpf.Common
{
    public class EventAggregator : IEventAggregator
    {
        private class ObservableProxy<T> : IObservable<T>, IObserver<T>
        {
            private List<IObserver<T>> _observers=new List<IObserver<T>> ();
            private Dictionary<object, IDisposable> _cancellationDict = new Dictionary<object, IDisposable>();

            public void OnCompleted()
            {
                this._observers.ForEach(x => x.OnCompleted());
            }

            public void OnError(Exception error)
            {
                this._observers.ForEach(x => x.OnError(error));
            }

            public void OnNext(T value)
            {
                this._observers.ForEach(x => x.OnNext(value));
            }

            public void Subscribe(IObservable<T> provider)
            {
                if (_cancellationDict.ContainsKey(provider)) return;
                _cancellationDict[provider] = provider.Subscribe(this);
            }

            public void Unsubscribe(IObservable<T> provider)
            {
                if (!_cancellationDict.ContainsKey(provider)) return;
                _cancellationDict[provider].Dispose();
                _cancellationDict.Remove(provider);
            }

            IDisposable IObservable<T>.Subscribe(IObserver<T> observer)
            {
                if (!_observers.Contains(observer))
                {
                    _observers.Add(observer);
                }
                return new Unsubscriber<T>(_observers, observer);
            }


        }

        private class Unsubscriber<T> : IDisposable
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


        private static ConcurrentDictionary<string, object> eventSourceDictionary = new ConcurrentDictionary<string, object>();

        public IObservable<T> GetEvent<T>(string eventSourceKey = null)
        {
            var key = GetEventSourceKey<T>(eventSourceKey);
            if (!eventSourceDictionary.ContainsKey(key))
                eventSourceDictionary.TryAdd(key, new ObservableProxy<T>());
            return eventSourceDictionary[key] as IObservable<T>;
        }


        public bool RegistEvent<T>(IObservable<T> eventSource, string eventSourceKey = null)
        {
            if (eventSource == null)
                throw new ArgumentNullException("eventSource is null");
            var key = GetEventSourceKey<T>(eventSourceKey);
            if (!eventSourceDictionary.ContainsKey(key))
                eventSourceDictionary.TryAdd(key, new ObservableProxy<T>());
            var proxy = eventSourceDictionary[key] as ObservableProxy<T>;
            if (proxy != null)
            {
                proxy.Subscribe(eventSource);
                return true;
            }
            return false;
        }

        private string GetEventSourceKey<T>(string eventSourceKey) => string.IsNullOrEmpty(eventSourceKey) ? typeof(T).Name : eventSourceKey;


        public bool UnregistEvent<T>(IObservable<T> eventSource, string eventSourceKey = null)
        {
            if (eventSource == null)
                throw new ArgumentNullException("eventSource is null");
            var key = string.IsNullOrEmpty(eventSourceKey) ? eventSource.GetType().Name : eventSourceKey;
            if (!eventSourceDictionary.ContainsKey(key)) return false;
            var proxy = eventSourceDictionary[key] as ObservableProxy<T>;
            if (proxy == null) return false;
            proxy.Unsubscribe(eventSource);
            return true;
        }
    }
}
