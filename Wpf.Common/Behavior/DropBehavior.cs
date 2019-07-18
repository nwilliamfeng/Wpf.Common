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
        /// <summary>
        /// 设置触发拖拽的命令
        /// </summary>
        public static readonly DependencyProperty DropCommandProperty = DependencyProperty.RegisterAttached("DropCommand",
            typeof(ICommand),
            typeof(DropBehavior),
            new PropertyMetadata(OnDropCommandPropertyChangedCallback));

        /// <summary>
        /// 设置是否支持多文件拖拽
        /// </summary>
        public static readonly DependencyProperty MultiFileEnableProperty = DependencyProperty.RegisterAttached("MultiFileEnable",
           typeof(bool),
           typeof(DropBehavior),
           new PropertyMetadata(false));


        public static ICommand GetDropCommand(UIElement element) => element.GetValue<ICommand>(DropCommandProperty);  
       
        public static void SetDropCommand(UIElement element,ICommand command)=>element.SetValue(DropCommandProperty, command);

        public static bool GetMultiFileEnable(UIElement element) => element.GetValue<bool>(MultiFileEnableProperty);

        public static void SetMultiFileEnable(UIElement element, bool enable) => element.SetValue(MultiFileEnableProperty, enable);
        
        private static void OnDropCommandPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var el = d as UIElement;
            if (el == null) return;
            el.PreviewDragOver += (s, arg) =>
            {
                arg.Effects = !el.GetValue<bool>(MultiFileEnableProperty) && arg.Data.IsMultiFileDroped() ? DragDropEffects.None : DragDropEffects.Copy;
                arg.Handled = true;
            };

            el.Drop += (s, arg) =>
            {
                GetDropCommand(el).Execute(arg.Data);
                arg.Handled = true;
            };
           
        }


    }
}
