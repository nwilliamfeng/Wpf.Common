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
    public class ScrollViewerBehavior
    {
        public static readonly DependencyProperty ScrollNextCommandProperty =
            DependencyProperty.RegisterAttached("ScrollNextCommand", typeof(ICommand), typeof(ScrollViewerBehavior)
                , new PropertyMetadata(OnScrollNextCommandPropertyValueChanged) );



        public static void SetScrollNextCommand(DependencyObject obj, ICommand value)
        => obj.SetValue(ScrollNextCommandProperty,value);


        public static ICommand GetScrollNextCommand(DependencyObject obj )
        => obj.GetValue<ICommand>(ScrollNextCommandProperty);


        private static void OnScrollNextCommandPropertyValueChanged(DependencyObject obj,DependencyPropertyChangedEventArgs e)
        {
            var sv = obj as ScrollViewer;
            if (sv == null) return;
            sv.ScrollChanged -= OnScrollViewer_ScrollChanged;
            sv.ScrollChanged += OnScrollViewer_ScrollChanged;
        }

        private static void OnScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var sv = sender as ScrollViewer;
            if (!sv.IsScrollEnd())
                return;
           var  GetScrollNextCommand(sv).Execute
        }
    }
}
