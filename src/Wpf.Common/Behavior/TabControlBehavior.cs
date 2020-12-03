using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Wpf.Common.Behavior
{
    /// <summary>
    /// TabControl附加行为
    /// </summary>
    public class TabControlBehavior
    {
        /// <summary>
        /// 是否允许拖放
        /// </summary>
        public static readonly DependencyProperty IsDropEnableProperty =
            DependencyProperty.RegisterAttached("IsDropEnable", typeof(bool), typeof(TabControlBehavior)
                , new PropertyMetadata(BooleanBoxes.False, OnIsDropEnablePropertyChanged));

        public static bool GetIsDropEnable(DependencyObject obj)
            => obj.GetValue<bool>(IsDropEnableProperty);

        public static void SetIsDropEnable(DependencyObject obj, bool value)
            => obj.SetValue(IsDropEnableProperty, value.Box());

      
        private static void OnIsDropEnablePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var tabControl = sender as TabControl;
            if (tabControl == null) return;
            if (!(e.NewValue is bool)) return;
            var isEnable = (bool)e.NewValue;
            if (!isEnable) return;
            tabControl.Loaded -= TabControl_Loaded;
            tabControl.Loaded += TabControl_Loaded;    
        }

        private static void TabControl_Loaded(object sender, RoutedEventArgs e)
        {
            var tabControl = sender as TabControl;
            var tabItems = tabControl.Items.OfType<TabItem>().ToList();
            if (tabItems.Count != tabControl.Items.Count)
                return;
            tabItems.ForEach(tabItem =>
            {
                tabItem.AllowDrop = true;
                tabItem.PreviewMouseMove -= TabItem_PreviewMouseMove;
                tabItem.PreviewMouseMove += TabItem_PreviewMouseMove;
                tabItem.Drop -= TabItem_Drop;
                tabItem.Drop += TabItem_Drop;
                tabItem.PreviewDragEnter -= TabItem_PreviewDragEnter;
                tabItem.PreviewDragEnter += TabItem_PreviewDragEnter;
                tabItem.PreviewDragLeave -= TabItem_PreviewDragLeave;
                tabItem.PreviewDragLeave += TabItem_PreviewDragLeave;
              
            });
        }

        private static void TabItem_PreviewDragLeave(object sender, DragEventArgs e)
        {
            var tabItem = sender as TabItem;
            tabItem.BorderThickness = default(Thickness);
            e.Handled = true;
        }

        private static void TabItem_PreviewDragEnter(object sender, DragEventArgs e)
        {
            var dropItem = e.Data.GetData<TabItem>();
            if (dropItem == null) return;
            var tabItem = sender as TabItem;

            if (Object.ReferenceEquals(dropItem, tabItem))
                return;

            var tabControl = tabItem.FindParent<TabControl>();
            var items = tabControl.FindChildren<TabItem>().ToList();
            if (items.Count <= 1)
                return;
            var isLast = items.IndexOf(tabItem) == items.Count - 1;
             
            if (IsMouseOnTabItemLeft(tabItem,e))
                tabItem.BorderThickness = new Thickness(1, 0, 0, 0);
            else
            {
                if (isLast)
                    tabItem.BorderThickness = new Thickness(0, 0, 0, 0);
                else
                    items[items.IndexOf(tabItem) + 1].BorderThickness = new Thickness(1, 0, 0, 0);
            }



            e.Handled = true;
        }

        private static bool IsMouseOnTabItemLeft(TabItem tabItem, DragEventArgs e)
        {
            var point = e.GetPosition(tabItem);
            return point.X <= tabItem.ActualWidth / 2;
        }


        private static void TabItem_Drop(object sender, DragEventArgs e)
        {
            if (e.Data == null) return;
            if (!(sender is TabItem)) return;
            var sourceTabItem = e.Data.GetData<TabItem>();
            var targetTabItem = sender as TabItem;
            if (object.ReferenceEquals(sourceTabItem, targetTabItem))
                return;
            var tabControl = sourceTabItem.FindParent<TabControl>();
            tabControl.FindChildren<TabItem>().ToList().ForEach(x => x.BorderThickness = default(Thickness));
            bool isInsert = (IsMouseOnTabItemLeft(targetTabItem, e));

          
            tabControl.Items.Remove(sourceTabItem);
            var i = tabControl.Items.IndexOf(targetTabItem);
            tabControl.Items.Insert(i < 0 ? 0 :isInsert? i:i+1, sourceTabItem);
            sourceTabItem.IsSelected = true;

            e.Handled = true;
        }

        private static void TabItem_PreviewMouseMove(object sender, MouseEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var tabItem = sender as TabItem;
                if (tabItem == null)
                    return;
                var uiElement = tabItem.Content as UIElement;
                if(uiElement==null || uiElement!=null && !uiElement.IsMouseOver)
                    DragDrop.DoDragDrop(tabItem, sender, DragDropEffects.Move);
            }
            e.Handled = true;
        }
    }
}
