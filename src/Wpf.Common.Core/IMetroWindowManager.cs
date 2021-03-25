using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Common
{
    public interface IMetroWindowManager
    {
        bool? ShowDialog(object dialogViewModel);
    }
}
