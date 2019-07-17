using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Wpf.Common.Behavior
{
    /// <summary>
    /// Drop行为
    /// </summary>
    /// <seealso cref="https://www.wpfsharp.com/2012/03/22/mvvm-and-drag-and-drop-command-binding-with-an-attached-behavior/"/>
    public static class DropBehavior
    {
        public static readonly DependencyProperty DropCommandProperty = DependencyProperty.RegisterAttached("DropCommand",
            typeof(ICommand),
            typeof(DropBehavior),
            new PropertyMetadata(OnPropertyChangedCallback));


        public static ICommand GetDropCommand(UIElement element)
        {
            return element.GetValue(DropCommandProperty) as ICommand;
        }

        public static void SetDropCommand(UIElement element,ICommand command)
        {
            element.SetValue(DropCommandProperty, command);
        }

        private static void OnPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var el = d as UIElement;
            if (el == null) return;
            el.PreviewDragOver += (s, arg) => arg.Handled = true;
            el.Drop += (s, arg) =>
            {
                GetDropCommand(el).Execute(arg.Data);
                arg.Handled = true;
            };
           
        }


    }
}
