using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wpf.Common.Behavior
{
    
    public  class TreeViewBehavior
    {
      
        public static readonly DependencyProperty SelectedItemCommandProperty =
            DependencyProperty.RegisterAttached("SelectedItemCommand", typeof(ICommand), typeof(TreeViewBehavior)
                , new FrameworkPropertyMetadata(null,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemCommandPropertyChanged));

       

        public static ICommand GetSelectedItemCommand(DependencyObject dp)
        {
            return (ICommand)dp.GetValue(SelectedItemCommandProperty);
        }

        public static void SetSelectedItemCommand(DependencyObject dp, ICommand value)
        {
            dp.SetValue(SelectedItemCommandProperty, value);
        }


       

        private static void OnSelectedItemCommandPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var treeView =   sender as TreeView;
            if (treeView == null) return;
            var command = e.NewValue as ICommand;
            if (command == null) return;
            treeView.SelectedItemChanged += (s, arg) =>
            {

                if (command.CanExecute(arg.NewValue))
                    command.Execute(arg.NewValue);
            };
          
        }

        
    }
}
