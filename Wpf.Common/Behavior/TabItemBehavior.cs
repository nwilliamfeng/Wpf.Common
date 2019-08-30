using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.Common.Behavior
{
    public class TabItemDropEventArgs:RoutedEventArgs
    {
        public TabItem DroppedValue { get; private set; }

   
        public TabItemDropEventArgs(RoutedEvent routedEvent, object source,TabItem tabItem )
            :base(routedEvent,source)
        {
            this.DroppedValue = tabItem;  
        }
    }

    

    /// <summary>
    /// TabItem附加行为
    /// </summary>
    public class TabItemBehavior
    {
        /// <summary>
        /// 是否允许拖放
        /// </summary>
        public static readonly DependencyProperty IsDropEnableProperty =
            DependencyProperty.RegisterAttached("IsDropEnable", typeof(bool), typeof(TabItemBehavior)
                , new PropertyMetadata(false,OnIsDropEnablePropertyChanged));

        public static bool GetIsDropEnable(DependencyObject obj)
            => obj.GetValue<bool>(IsDropEnableProperty);

        public static void SetIsDropEnable(DependencyObject obj, object value)
            => obj.SetValue(IsDropEnableProperty, value);


        /// <summary>
        /// 拖放事件触发后的绑定命令 
        /// </summary>
        public static readonly DependencyProperty DropCommandProperty =
            DependencyProperty.RegisterAttached("DropCommand", typeof(ICommand), typeof(TabItemBehavior) , new PropertyMetadata( null));

        public static ICommand GetDropCommand(DependencyObject obj)
            => obj.GetValue<ICommand>(DropCommandProperty);

        public static void SetDropCommand(DependencyObject obj, object value)
            => obj.SetValue(DropCommandProperty, value);


         
        //public static readonly DependencyProperty DropCommandParameterProperty =
        //    DependencyProperty.RegisterAttached("DropCommandParameter", typeof(Tuple<object,object>), typeof(TabItemBehavior), new PropertyMetadata(null));

        //public static Tuple<object, object> GetDropCommandParameter(DependencyObject obj)
        //    => obj.GetValue<Tuple<object, object>>(DropCommandParameterProperty);

        //public static void SetDropCommandParameter(DependencyObject obj, object value)
        //    => obj.SetValue(DropCommandParameterProperty, value);

        //https://docs.microsoft.com/en-us/dotnet/framework/wpf/advanced/attached-events-overview

        public static readonly RoutedEvent DropEvent = EventManager.RegisterRoutedEvent("Drop", RoutingStrategy.Bubble,
            typeof(EventHandler<TabItemDropEventArgs>), typeof(TabItemBehavior));

        public static void AddDropHandler(DependencyObject d, EventHandler<TabItemDropEventArgs> handler)
        {        
            UIElement uie = d as UIElement;
            if (uie != null)
                uie.AddHandler(TabItemBehavior.DropEvent, handler);
        }

        public static void RemoveDropHandler(DependencyObject d, EventHandler<TabItemDropEventArgs> handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
                uie.RemoveHandler(TabItemBehavior.DropEvent, handler);
        }


        private static void OnIsDropEnablePropertyChanged(DependencyObject sender,DependencyPropertyChangedEventArgs e)
        {
            var tabItem = sender as TabItem;
            if (tabItem == null) return;
            if (!(e.NewValue is bool)) return;
            var isEnable = (bool)e.NewValue;
            if (!isEnable) return;
            tabItem.AllowDrop = true;
            tabItem.PreviewMouseMove -= TabItem_PreviewMouseMove;
            tabItem.PreviewMouseMove += TabItem_PreviewMouseMove;
            tabItem.Drop -= TabItem_Drop;
            tabItem.Drop += TabItem_Drop;
        }

        private static void TabItem_Drop(object sender, DragEventArgs e)
        {
            if (e.Data == null) return;
            if (!(sender is TabItem)) return;
            var tabItem = e.Data.GetData<TabItem>();
            var sourceTabItem = sender as TabItem;
            if(sourceTabItem.DataContext!=null && tabItem.DataContext!=null)
            {
                ICommand cmd = GetDropCommand(sourceTabItem);
                var para = new Tuple<object, object>(sourceTabItem.DataContext, tabItem.DataContext);
                if (cmd != null && cmd.CanExecute(para))
                    cmd.Execute(para);
            }
            sourceTabItem.RaiseEvent(new TabItemDropEventArgs(DropEvent, sender, tabItem));
        }

        private static void TabItem_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;
            var tabItem = sender as TabItem;
            if (tabItem == null)
                return;
            DragDrop.DoDragDrop(tabItem, sender, DragDropEffects.Move);
        }
    }
}
