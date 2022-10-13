using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wpf.Common.Behavior
{

    public class TreeViewSelectBehavior
    {
        public static readonly DependencyProperty SelectedItemEnableProperty =
           DependencyProperty.RegisterAttached("SelectedItemEnable", typeof(bool), typeof(TreeViewSelectBehavior)
               , new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemEnablePropertyChanged));


        public static readonly DependencyProperty SelectedItemProperty =DependencyProperty.RegisterAttached("SelectedItem", typeof(object), typeof(TreeViewSelectBehavior));


        public static bool GetSelectedItemEnable(DependencyObject dp)
        {
            return (bool)dp.GetValue(SelectedItemEnableProperty);
        }

        public static void SetSelectedItemEnable(DependencyObject dp, object value)
        {
            dp.SetValue(SelectedItemEnableProperty, value);
        }

        public static object GetSelectedItem(DependencyObject dp)
        {
            return (object)dp.GetValue(SelectedItemProperty);
        }

        public static void SetSelectedItem(DependencyObject dp, object value)
        {
            dp.SetValue(SelectedItemProperty, value);
        }
 
        private static void OnSelectedItemEnablePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var treeView = sender as TreeView;
            if (treeView == null) return;
            var rst =(bool) e.NewValue  ;

            RoutedPropertyChangedEventHandler<object> selectItemChangeHandle =(s,arg)=> SetSelectedItem(treeView, treeView.SelectedItem);
            if (rst)
                treeView.SelectedItemChanged += selectItemChangeHandle;
            else
                treeView.SelectedItemChanged -= selectItemChangeHandle;

        }


    }
}
