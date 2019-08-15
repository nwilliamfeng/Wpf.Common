using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Wpf.Common.Demo
{
    class MefBootstrapper : BootstrapperBase
    {
        private CompositionContainer _container;

        public MefBootstrapper()
        {

            Initialize();

        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainViewModel>();
        }

        protected override void Configure()
        {
            var catalog = new AggregateCatalog();

            var composablePartCatalog = AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>();

            foreach (var part in composablePartCatalog)
                catalog.Catalogs.Add(part);

            _container = new CompositionContainer(catalog);

            var batch = new CompositionBatch();

            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(_container);
            _container.Compose(batch);
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            var contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            var exports = _container.GetExportedValues<object>(contract);

            if (exports.Count() > 0)
                return exports.First();

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            List<Assembly> allAssemblies = new List<Assembly>();
            string path = Assembly.GetExecutingAssembly().Location;
            foreach (string dll in Directory.GetFiles(Directory.GetParent(path).FullName, "*.dll"))
                try
                {
                    allAssemblies.Add(Assembly.LoadFile(dll));
                }
                catch { }
            allAssemblies.Add(Assembly.GetExecutingAssembly());
            return allAssemblies;
        }

        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            base.OnUnhandledException(sender, e);
        }


    }
}
