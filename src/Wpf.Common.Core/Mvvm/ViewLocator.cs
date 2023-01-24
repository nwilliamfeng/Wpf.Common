using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf.Common
{
    public static class ViewLocator
    {
        private static Dictionary<Type, Type> viewDict = new Dictionary<Type, Type>();

        public static T CreateViewForViewModel<T>(object viewModel)
            where T : UIElement, new() => CreateViewForViewModel<T>(viewModel.GetType());

        public static T CreateViewForViewModel<T>(Type viewModelType)
            where T : UIElement,new()
        {
            if(viewDict.ContainsKey(viewModelType))
                return Activator.CreateInstance(viewDict[viewModelType]) as T;
            var viewType = LocatorViewType(viewModelType);
            if (viewType == null)
                return default(T);            
            return Activator.CreateInstance(viewType) as T;
        }

        public static void RegistViewTypeFor<T>(Type viewType)
            where T :class, INotifyPropertyChanged
        {
            if(!viewDict.ContainsKey(typeof(T)))
                viewDict[typeof(T)]=viewType;
        }

        public static void RegistViewTypeFor<T, Y>()
            where T : class, INotifyPropertyChanged
            where Y : UIElement, new() => RegistViewTypeFor<T>(typeof(Y));
      

        public static Func<Type, Type> LocatorViewType { get;  set; } = type =>
        {
            if (!type.Name.EndsWith("ViewModel")) return null;
            var vmName = type.Name;
            Func<Assembly, Type> func = ambly => ambly.GetExportedTypes().FirstOrDefault(t => type.Name.Replace("ViewModel", "View") == t.Name);
            var result = func(type.Assembly);
            if (result != null) return result;
            return func(Assembly.GetEntryAssembly());
        };

        
        
    }
}
