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
    public class TreeViewDragDropBehavior
    {
        /// <summary>
        /// DragDropEnable
        /// </summary>
        public static readonly DependencyProperty DragDropEnableProperty
            = DependencyProperty.RegisterAttached("DragDropEnable", typeof(bool), typeof(TreeViewDragDropBehavior), new PropertyMetadata(false, OnDragDropEnableChanged));

        /// <summary>
        /// DropItem
        /// </summary>
        public static readonly DependencyProperty DropItemProperty
            = DependencyProperty.RegisterAttached("DropItem", typeof(Node), typeof(TreeViewDragDropBehavior),new FrameworkPropertyMetadata(null,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault ));

        public static Node GetDropItem (DependencyObject dependencyObject) => (Node)dependencyObject.GetValue(DropItemProperty);

        public static void SetDropItem(DependencyObject dependencyObject, Node value) => dependencyObject.SetValue(DropItemProperty, value);

        public static bool GetDragDropEnable(DependencyObject dependencyObject) => (bool)dependencyObject.GetValue(DragDropEnableProperty);

        public static void SetDragDropEnable(DependencyObject dependencyObject, bool value) => dependencyObject.SetValue(DragDropEnableProperty, value);

        public static readonly DependencyProperty DropFileCommandProperty
          = DependencyProperty.RegisterAttached("DropFileCommand", typeof(ICommand), typeof(TreeViewDragDropBehavior)   );

        public static ICommand GetDropFileCommand(DependencyObject dependencyObject) => dependencyObject.GetValue(DropFileCommandProperty) as ICommand;

        public static void SetDropFileCommand(DependencyObject dependencyObject, ICommand value) => dependencyObject.SetValue(DropFileCommandProperty,value);

        private static void OnDragDropEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TreeView)) return;
            var treeView = d as TreeView;
            var style = treeView.ItemContainerStyle;
            var enable = (bool)e.NewValue;


            if (enable && style == null)
                treeView.ItemContainerStyle = new Style(typeof(TreeViewItem));
            treeView.AllowDrop = enable;
            DragEventHandler dropEvent = (s, arg) =>
            {
                try
                {
                    arg.Effects = DragDropEffects.None;
                    arg.Handled = true;
                    if (arg.OriginalSource is FrameworkElement fe)
                    {
                        if (fe.DataContext is Node node)
                        {
                            var format = arg.Data.GetFormats().FirstOrDefault();
                            if (arg.Data.GetDataPresent(DataFormats.FileDrop))
                            {
                                string[] files = (string[])arg.Data.GetData(DataFormats.FileDrop);
                                var command =GetDropFileCommand(d);
                                command?.Execute(new Tuple<Node,string[]>(node, files));
                                return; 
                            }
                            if (!arg.Data.GetData(format).Equals(node)) //有可能是同一个treeviewitem
                                treeView.SetValue(TreeViewDragDropBehavior.DropItemProperty, node);
                        }
                    }
                }
                catch (Exception)
                {
                }
            };

            if (enable)
            {
                treeView.ItemContainerStyle.Setters.Add(new EventSetter { Event = TreeViewItem.DragOverEvent, Handler = new DragEventHandler(treeViewItem_DragOver) });
                treeView.ItemContainerStyle.Setters.Add(new EventSetter { Event = TreeViewItem.MouseMoveEvent, Handler = new MouseEventHandler(treeViewItem_MouseMove) });
                treeView.ItemContainerStyle.Setters.Add(new EventSetter { Event = TreeViewItem.DropEvent, Handler = new DragEventHandler(dropEvent) });
            }
            else
            {
                treeView.ItemContainerStyle.Setters.Remove(new EventSetter { Event = TreeViewItem.DragOverEvent, Handler = new DragEventHandler(treeViewItem_DragOver) });
                treeView.ItemContainerStyle.Setters.Remove(new EventSetter { Event = TreeViewItem.MouseMoveEvent, Handler = new MouseEventHandler(treeViewItem_MouseMove) });
                treeView.ItemContainerStyle.Setters.Remove(new EventSetter { Event = TreeViewItem.DropEvent, Handler = new DragEventHandler(dropEvent) });
            }
        }

        private static void treeViewItem_MouseMove(object sender, MouseEventArgs s)
        {
            try
            {
                if (s.LeftButton != MouseButtonState.Pressed) return;
                if (s.OriginalSource is FrameworkElement fe)
                    if (fe.DataContext is Node node)
                        DragDrop.DoDragDrop(sender as TreeViewItem, node, DragDropEffects.Move);
            }
            catch (Exception ex)
            {
            }
        }

        private static void treeViewItem_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    if (e.OriginalSource is FrameworkElement fe)
                        if (fe.DataContext is Node node)
                        {
                            e.Effects = node.NodeType== NodeType.Folder ? DragDropEffects.Copy : DragDropEffects.None;
                        }
                }
                else if (e.OriginalSource is FrameworkElement fe)
                    if (fe.DataContext is Node node)
                    {
                        e.Effects = node.NodeType == NodeType.Folder ? DragDropEffects.Move : DragDropEffects.None;
                    }
                e.Handled = true;
            }
            catch (Exception ex)
            {
            }
        }    
    }
}
