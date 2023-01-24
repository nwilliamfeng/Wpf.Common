
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Wpf.Common.Composition
{
    public class MefBootstrapper : BootstrapperBase
    {
        private CompositionContainer _container;

        protected override void Configure()
        {
            var catalog = new AggregateCatalog();
            var partCatalogs = this.LoadAssemblies().Select(x => new AssemblyCatalog(x)).ToList();
            foreach (var item in partCatalogs)
                catalog.Catalogs.Add(item);
            _container = new CompositionContainer(catalog);
            var batch = new CompositionBatch();
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue<IWindowManager>(new WindowManager());
            InjectParts(batch);
            batch.AddExportedValue(_container);
            
            _container.Compose(batch);
        }

        
        protected virtual void InjectParts(CompositionBatch batch)
        {
           
        }

        protected override object GetInstance(Type serviceType)
        {
            var contract = AttributedModelServices.GetContractName(serviceType);
            var exports = _container.GetExportedValues<object>(contract);
            if (exports.Count() > 0)
                return exports.First();
            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
            => _container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));

        //protected IEnumerable<Assembly> LoadAssemblies()
        //{
        //    List<Assembly> result = new List<Assembly>();
        //    string path = Assembly.GetEntryAssembly().Location;

        //    foreach (string dll in Directory.GetFiles(Directory.GetParent(path).FullName, "*.dll"))
        //        try
        //        {
        //            result.Add(Assembly.LoadFile(dll));
        //        }
        //        catch
        //        {

        //        }

        //    return result;
        //}

        protected IEnumerable<Assembly> LoadAssemblies()
        {
            var mainAmbly = Assembly.GetEntryAssembly();
            yield return mainAmbly;

            foreach (var name in mainAmbly.GetReferencedAssemblies())
                yield return Assembly.Load(name);

        }
    }
}
