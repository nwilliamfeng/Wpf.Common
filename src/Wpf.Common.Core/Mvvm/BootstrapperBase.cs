using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;

namespace Wpf.Common
{
    public abstract class BootstrapperBase
    { 
        protected Application Application { get;   }

        protected BootstrapperBase() 
        {
            Application = Application.Current;
            Configure();
            IoC.GetInstance = GetInstance;
            IoC.GetAllInstances = GetAllInstances;
          
            PrepareApplication();
        }

        protected virtual void PrepareApplication()
        {
            Application.Startup += OnStartup;
            Application.DispatcherUnhandledException += OnUnhandledException;
            Application.Exit += OnExit;
        }

        protected abstract void Configure();

        protected virtual void OnStartup(object sender, StartupEventArgs e)
        {
        }

     
        protected virtual void OnExit(object sender, EventArgs e)
        {
        }

       
        protected virtual void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
        }


        protected virtual object GetInstance(Type service)
        {
            return Activator.CreateInstance(service);
        }

       
        protected virtual IEnumerable<object> GetAllInstances(Type service)
        {
            return new object[1] { Activator.CreateInstance(service) };
        }

        protected void DisplayRootViewFor(Type viewModelType, Action<Window> setting = null)
        {
            IoC.Get<IWindowManager>().ShowWindow(IoC.GetInstance(viewModelType), setting);
        }

        protected void DisplayRootViewFor<T>( Action<Window> setting = null)
            where T:class
        {
            DisplayRootViewFor(typeof(T),setting);
        }
    }
}
