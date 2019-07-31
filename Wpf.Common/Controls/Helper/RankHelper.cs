using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wpf.Common.Controls
{
    internal static class RankHelper
    {

        public static MouseEventHandler RankItem_MouseEnter => OnRankItem_MouseEnter;
         

        private static void OnRankItem_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var item = (sender as Button).DataContext as RankItem;
            if (!item.IsEnabled)
                return;
            item.ParentItems
                .Where(x => x.Value <= item.Value && !x.IsSelected)
                .ToList()
                .ForEach(x => x.IsTempSelected = true);
            e.Handled = true;

        }

        public static MouseEventHandler RankItem_MouseLeave => OnRankItem_MouseLeave;


        private static void OnRankItem_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var item = (sender as Button).DataContext as RankItem;
            if (!item.IsEnabled)
                return;
            if (item.ParentItems.Any(x => !x.IsSelected && x.IsTempSelected && x.Value > item.Value))
                return;

            item.ParentItems
                .Where(x => x.IsTempSelected && !x.IsSelected)
                .ToList()
                .ForEach(x => x.IsTempSelected = false);
            e.Handled = true;

        }
    }
}
