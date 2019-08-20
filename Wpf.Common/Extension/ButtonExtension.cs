using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace Wpf.Common
{
    public static class ButtonExtension
    {
        public static void ExecuteClick(this MenuItem menuItem)
        {

            MenuItemAutomationPeer peer = new MenuItemAutomationPeer(menuItem);
            IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
            invokeProv.Invoke();
        }
    }
}
