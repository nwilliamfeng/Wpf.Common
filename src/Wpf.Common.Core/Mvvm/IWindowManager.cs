using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Wpf.Common
{
    public interface IWindowManager
    {
       
        bool? ShowDialog(object rootModel,   Action<Window> setting = null);

      
        void ShowWindow(object rootModel,   Action<Window> setting = null);

      
        void ShowPopup(object rootModel, Action close=null , Action<Popup> setting = null);
    }

}
