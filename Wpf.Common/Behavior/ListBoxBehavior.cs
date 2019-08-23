using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wpf.Common.Behavior
{
    public class ListBoxBehavior
    {
        /// <summary>
        /// 当滚动条触底的时候触发命令
        /// </summary>
        public static readonly DependencyProperty ScrollNextCommandProperty =
            DependencyProperty.RegisterAttached("ScrollNextCommand", typeof(ICommand), typeof(ListBoxBehavior)
                , new PropertyMetadata(OnScrollNextCommandPropertyValueChanged));



        public static void SetScrollNextCommand(DependencyObject obj, ICommand value)
        => obj.SetValue(ScrollNextCommandProperty,value);


        public static ICommand GetScrollNextCommand(DependencyObject obj )
        => obj.GetValue<ICommand>(ScrollNextCommandProperty);


        private static void OnScrollNextCommandPropertyValueChanged(DependencyObject obj,DependencyPropertyChangedEventArgs e)
        {
            var listBox = obj as ListBox;
            if (listBox == null) return;
            var sv = listBox.GetScrollViewer();
            sv.Tag = listBox;
            sv.ScrollChanged -= OnScrollViewer_ScrollChanged;
            sv.ScrollChanged += OnScrollViewer_ScrollChanged;
        }

        private static void OnScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var sv = sender as ScrollViewer;
            if (!sv.IsScrollEnd())
                return;
            ListBox listBox = sv.Tag as ListBox;           
            if (listBox == null) return;
            var cmd = GetScrollNextCommand(listBox);            
            if (cmd.CanExecute(null))
            {
                var oldsh = sv.ScrollableHeight;
                cmd.Execute(null);
                sv.ScrollToVerticalOffset(oldsh);
            }
        }
    }
}
