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
    /// Ctrl+Wheel行为
    /// </summary>
    
    public  class MouseWheelWithCtrlKeyBehavior
    {
        /// <summary>
        /// 设置触发拖拽的命令
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(MouseWheelWithCtrlKeyBehavior),
            new PropertyMetadata(OnCommandPropertyChangedCallback));

       

        public static ICommand GetCommand(UIElement element) => element.GetValue<ICommand>(CommandProperty);  
       
        public static void SetCommand(UIElement element,ICommand command)=>element.SetValue(CommandProperty, command);

        
        private static void OnCommandPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var el = d as UIElement;
            if (el == null) return;
            el.PreviewMouseWheel -= OnMouseWheelChanged;
            el.PreviewMouseWheel += OnMouseWheelChanged;
            
        }

       

        private static void OnMouseWheelChanged(object sender,MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                GetCommand(sender as UIElement).Execute(e.Delta);
            
        }

       
    }
}
