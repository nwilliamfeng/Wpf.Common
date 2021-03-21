using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Common.Events
{
    public class OpenMetroDialogEventArgs:EventArgs
    {
        public OpenMetroDialogEventArgs(object dialog)
        {
            Dialog = dialog;
        }

        public object Dialog { get; }
    }

    public class CloseMetroDialogEventArgs : EventArgs
    {
        public CloseMetroDialogEventArgs(object dialog)
        {
            Dialog = dialog;
        }

        public object Dialog { get; }
    }
}
