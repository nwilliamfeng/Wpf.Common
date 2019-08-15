using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;

namespace Wpf.Common.Demo
{

    [Export(typeof(MainViewModel))]
    public class MainViewModel:Screen
    {
        public ObservableCollection<GroupNode> Nodes { get; private set; }

        public MainViewModel()
        {
            this.Nodes = new ObservableCollection<GroupNode>();
            var gpNode1 = new GroupNode { Name="Template"};
            gpNode1.Items.Add(new NodeViewModel { Name = "ErrorTemplate", Code = NodeCodes.ERROR_TEMPLATE });
            gpNode1.Items.Add(new NodeViewModel { Name = "abc", Code = NodeCodes.ERROR_TEMPLATE });
            Nodes.Add(gpNode1);
           
        }
    }
}
