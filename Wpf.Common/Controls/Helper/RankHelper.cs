using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

            item.ParentItems
                .Where(x => x.Value > item.Value && !x.IsSelected && x.IsTempSelected)
                .ToList()
                .ForEach(x => x.IsTempSelected = false);
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
            if (item.IsTempSelected)
                item.IsTempSelected = false;
            e.Handled = true;

        }

        public static RoutedEventHandler RankItem_Click => OnRankItem_Click;


        private static void OnRankItem_Click(object sender,  RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext as RankItem;
            if (!item.IsEnabled)
                return;
            bool isSelected = !item.IsSelected ;
            if (isSelected)
                item.ParentItems
                    .Where(x =>  !x.IsSelected && x.Value <= item.Value)
                    .ToList()
                    .ForEach(x => x.IsSelected = true);
            else
                item.ParentItems
                   .Where(x => x.IsSelected  && x.Value >= item.Value)
                   .ToList()
                   .ForEach(x => x.IsSelected = false);
            e.Handled = true;

        }
    }
}
