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
using Caliburn.Micro.MEF;
using Wpf.Common.Controls;

namespace Wpf.Common.Demo
{
    class MefBootstrapper  : MefBootstrapperBase<MainViewModel>
    {
        protected override void InjectWindowManager(CompositionBatch batch)
        {
            base.InjectWindowManager(batch);
            batch.AddExportedValue<IMetroWindowManager>(new MetroWindowManager());
        }

    }
}
