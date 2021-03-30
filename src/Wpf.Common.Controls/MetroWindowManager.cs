using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wpf.Common.Behavior;
using Wpf.Common.Controls.Behavior;

namespace Wpf.Common.Controls
{
    public class MetroWindowManager : IMetroWindowManager
    {
        public async Task<bool?> ShowDialog(object dialogViewModel)
        {
            var window = Application.Current.MainWindow;
            if (window == null) return null;
            if (!WindowChromeBehavior.GetIsEnable(window))
                return null;

            WindowChromeBehavior.SetDialogContent(window, dialogViewModel);
             
            var dialogContentControl = window.FindChildrenFromTemplate<ContentControl>("PART_DialogContainer");
            if (dialogContentControl == null ) return null;
            await Task.Delay(100);
            var dialog=dialogContentControl.FindChildren<MetroDialog>().FirstOrDefault();
            if (dialog == null) return false;
            await Task.Run(() =>
            {
                while ( dialog.IsVisible)
                {

                }
            });

            return dialog.DialogResult ;
       
        }

       
    }
}
