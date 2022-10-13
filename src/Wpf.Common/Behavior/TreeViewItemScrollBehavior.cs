using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wpf.Common.Behavior
{

    public class TreeViewItemScrollBehavior
    {
        public static readonly DependencyProperty RequestBringIntoViewEnableProperty =
           DependencyProperty.RegisterAttached("RequestBringIntoViewEnable", typeof(bool), typeof(TreeViewItemScrollBehavior)
               , new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnRequestBringIntoViewEnablePropertyChanged));


       

        public static bool GetRequestBringIntoViewEnable(DependencyObject dp)
        {
            return (bool)dp.GetValue(RequestBringIntoViewEnableProperty);
        }

        public static void SetRequestBringIntoViewEnable(DependencyObject dp, object value)
        {
            dp.SetValue(RequestBringIntoViewEnableProperty, value);
        }

       
 
        private static void OnRequestBringIntoViewEnablePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var treeViewItem = sender as TreeViewItem;
            if (treeViewItem == null) return;
            var rst =(bool) e.NewValue  ;
            if (rst)
                treeViewItem.RequestBringIntoView -= TreeViewItem_RequestBringIntoView;
            else
                treeViewItem.RequestBringIntoView += TreeViewItem_RequestBringIntoView;

 
        }

        private static void TreeViewItem_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }
    }
}
