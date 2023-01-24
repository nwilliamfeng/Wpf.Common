using System;
using System.Collections.Generic;
using System.Linq;


namespace Wpf.Common
{
    public static class IoC
    {
        public static Func<Type,  object> GetInstance { get; internal set; } = delegate
        {
            throw new InvalidOperationException("IoC is not initialized.");
        };

       
        public static Func<Type, IEnumerable<object>> GetAllInstances { get; internal set; } = delegate
        {
            throw new InvalidOperationException("IoC is not initialized.");
        };

       
        public static T Get<T>()
        {
            return (T)GetInstance(typeof(T));
        }

        
        public static IEnumerable<T> GetAll<T>()
        {
            return GetAllInstances(typeof(T)).OfType<T>();
        }
    }
}
