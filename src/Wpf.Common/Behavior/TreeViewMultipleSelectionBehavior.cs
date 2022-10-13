using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Wpf.Common.Behavior
{
    /// <summary>
    /// https://chrigas.blogspot.com/2014/08/wpf-treeview-with-multiple-selection.html
    /// 备注：如果treeview绑定源清空后需要手动清空此附加属性对应的SelectedItems源
    /// </summary>
    public class TreeViewMultipleSelectionBehavior
    {
        public static readonly DependencyProperty IsMultipleSelectionProperty =
        DependencyProperty.RegisterAttached("IsMultipleSelection", typeof(Boolean), typeof(TreeViewMultipleSelectionBehavior), new PropertyMetadata(false, OnMultipleSelectionPropertyChanged));

        public static bool GetIsMultipleSelection(TreeView element) => (bool)element.GetValue(IsMultipleSelectionProperty);

        public static void SetIsMultipleSelection(TreeView element, Boolean value) => element.SetValue(IsMultipleSelectionProperty, value);

        /// <summary>
        /// 是否全部不选中，todo，在vm中即使操作所有的node，将isselected置false，但仍有高亮显示，所以只能从treeview这边下手
        /// </summary>
        public static readonly DependencyProperty  UnselectAllProperty =
       DependencyProperty.RegisterAttached("UnselectAll", typeof(bool), typeof(TreeViewMultipleSelectionBehavior), new PropertyMetadata(false, OnUnselectAllPropertyChanged));


        public static bool GetUnselectAll(TreeView element) => (bool)element.GetValue(UnselectAllProperty);

        public static void SetUnselectAll(TreeView element, bool value) => element.SetValue(UnselectAllProperty, value);


        private static void OnUnselectAllPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool value)
                if (value)
                    DeSelectAllItems(d as TreeView, null);
        }

        private static void OnTreeViewItemClickUp(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = FindTreeViewItem(e.OriginalSource as DependencyObject);
            TreeView treeView = sender as TreeView;

            if (treeViewItem != null && treeView != null)
            {
                if (Keyboard.Modifiers == ModifierKeys.Control)
                    SelectMultipleItemsRandomly(treeView, treeViewItem);
                else if (Keyboard.Modifiers == ModifierKeys.Shift)
                {
                    var selectedTreeViewItems = SelectMultipleItemsContinuously(treeView, treeViewItem);
                    var childs = GetDisableChilds(selectedTreeViewItems);
                    childs.ToList().ForEach(x =>
                    {
                        SetIsItemSelected(x, false);
                    });
                }
                else
                {
                    SelectSingleItem(treeView, treeViewItem);
                }
            }
        }

        private static TreeViewItem FindTreeViewItem(DependencyObject dependencyObject)
        {
            if (dependencyObject == null) return null;
            TreeViewItem treeViewItem = dependencyObject as TreeViewItem;
            if (treeViewItem != null)
                return treeViewItem;
            return FindTreeViewItem(VisualTreeHelper.GetParent(dependencyObject));
        }

        private static void SelectSingleItem(TreeView treeView, TreeViewItem treeViewItem)
        {
            DeSelectAllItems(treeView, null);
            SetIsItemSelected(treeViewItem, true);
            SetStartItem(treeView, treeViewItem);
        }

        private static void DeSelectAllItems(TreeView treeView, TreeViewItem treeViewItem)
        {
            if (treeView != null)
            {
                for (int i = 0; i < treeView.Items.Count; i++)
                {
                    TreeViewItem item = treeView.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem;
                    if (item != null)
                    {
                        SetIsItemSelected(item, false);
                        DeSelectAllItems(null, item);
                    }
                }
            }
            else
            {
                for (int i = 0; i < treeViewItem.Items.Count; i++)
                {
                    TreeViewItem item = treeViewItem.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem;
                    if (item != null)
                    {
                        SetIsItemSelected(item, false);
                        DeSelectAllItems(null, item);
                    }
                }
            }
        }

        private static void OnIsItemSelectedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TreeViewItem treeViewItem = d as TreeViewItem;
            TreeView treeView = FindTreeView(treeViewItem);
            if (treeViewItem != null && treeView != null)
            {
                var selectedItems = GetSelectedItems(treeView);
                
                if (selectedItems != null)
                {
                    if (GetIsItemSelected(treeViewItem))
                    {
                        if (treeViewItem.Header != null)
                            selectedItems.Add(treeViewItem.Header);
                        else
                            Console.WriteLine("find the null header ,can't add!");
                    }
                    else
                        selectedItems.Remove(treeViewItem.Header);
                }
            }
        }

        private static TreeView FindTreeView(DependencyObject dependencyObject)
        {
            if (dependencyObject == null) return null;
            TreeView treeView = dependencyObject as TreeView;
            if (treeView != null)
                return treeView;
            return FindTreeView(VisualTreeHelper.GetParent(dependencyObject));
        }

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached("SelectedItems", typeof(IList), typeof(TreeViewMultipleSelectionBehavior), new PropertyMetadata());

        public static IList GetSelectedItems(TreeView element) => (IList)element.GetValue(SelectedItemsProperty);

        public static void SetSelectedItems(TreeView element, IList value) => element.SetValue(SelectedItemsProperty, value);

        private static bool IsRelationShip(INode node1, INode node2)
        {
            if (node1.Level == node2.Level) return false;
            if (node1.Level > node2.Level)
            {
                var parent = node1.Parent;
                while (true)
                {
                    if (parent == null)
                        break;
                    if (parent.Equals(node2)) return true;
                    parent = parent.Parent;
                }
                return false;
            }
            else
            {
                var parent = node2.Parent;
                while (true)
                {
                    if (parent == null)
                        break;
                    if (parent.Equals(node1)) return true;
                    parent = parent.Parent;
                }
                return false;
            }
        }

        private static void SelectMultipleItemsRandomly(TreeView treeView, TreeViewItem treeViewItem)
        {
            var isSelected = !GetIsItemSelected(treeViewItem);
            var currNode = treeViewItem.DataContext as INode;
            foreach (var node in GetSelectedItems(treeView).OfType<INode>())
            {
                if (IsRelationShip(node, currNode))
                {
                    isSelected = false;
                    break;
                }
            };

            SetIsItemSelected(treeViewItem, isSelected);
            if (GetStartItem(treeView) == null)
            {
                if (GetIsItemSelected(treeViewItem))
                    SetStartItem(treeView, treeViewItem);
            }
            else
            {
                if (GetSelectedItems(treeView).Count == 0)
                    SetStartItem(treeView, null);
            }

        }

        private static void GetAllItems(TreeView treeView, TreeViewItem treeViewItem, ICollection<TreeViewItem> allItems)
        {
            if (treeView != null)
            {
                for (int i = 0; i < treeView.Items.Count; i++)
                {
                    TreeViewItem item = treeView.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem;
                    if (item != null)
                    {
                        allItems.Add(item);
                        GetAllItems(null, item, allItems);
                    }
                }
            }
            else
            {
                for (int i = 0; i < treeViewItem.Items.Count; i++)
                {
                    TreeViewItem item = treeViewItem.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem;
                    if (item != null)
                    {
                        allItems.Add(item);
                        GetAllItems(null, item, allItems);
                    }
                }
            }
        }


        private static List<TreeViewItem> SelectMultipleItemsContinuously(TreeView treeView, TreeViewItem treeViewItem)
        {
            TreeViewItem startItem = GetStartItem(treeView);
            List<TreeViewItem> rst = new List<TreeViewItem>();
            if (startItem != null)
            {
                if (startItem == treeViewItem)
                {
                    SelectSingleItem(treeView, treeViewItem);
                    return new List<TreeViewItem>();
                }
                ICollection<TreeViewItem> allItems = new List<TreeViewItem>();
                GetAllItems(treeView, null, allItems);
                DeSelectAllItems(treeView, null);
                bool isBetween = false;

                foreach (var item in allItems)
                {
                    if (item == treeViewItem || item == startItem)
                    {
                        // toggle to true if first element is found and back to false if last element is found
                        isBetween = !isBetween;
                        // set boundary element
                        SetIsItemSelected(item, true);
                        rst.Add(item);
                        continue;
                    }
                    if (isBetween)
                    {
                        SetIsItemSelected(item, true);
                        rst.Add(item);
                    }
                }
            }
            return rst;
        }

        /// <summary>
        /// 比对集合里的item，如果有存在父子关系的，则将子项返回
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private static List<TreeViewItem> GetDisableChilds(List<TreeViewItem> items)
        {
            var dic = items.ToDictionary(x => x.DataContext as INode);
            var lst = dic.Keys.ToList();
            var deleteItems = new List<INode>();
            for (int i = 0; i < lst.Count; i++)
            {
                var node = lst[i];
                var parent = node.Parent;
                while (true)
                {
                    if (parent == null)
                        break;
                    for (int j = 0; j < lst.Count; j++)
                    {
                        if (parent.Equals(lst[j]))
                        {
                            if (!deleteItems.Contains(node))
                                deleteItems.Add(node);
                            break;
                        }
                    }
                    parent = parent.Parent;
                }
            }
            return deleteItems.Select(x => dic[x]).ToList();
        }

        private static readonly DependencyProperty StartItemProperty = DependencyProperty.RegisterAttached("StartItem", typeof(TreeViewItem), typeof(TreeViewMultipleSelectionBehavior), new PropertyMetadata());

        private static TreeViewItem GetStartItem(TreeView element) => (TreeViewItem)element.GetValue(StartItemProperty);

        private static void SetStartItem(TreeView element, TreeViewItem value) => element.SetValue(StartItemProperty, value);


        public static readonly DependencyProperty IsItemSelectedProperty = DependencyProperty.RegisterAttached("IsItemSelected", typeof(Boolean), typeof(TreeViewMultipleSelectionBehavior), new PropertyMetadata(false, OnIsItemSelectedPropertyChanged));

        public static bool GetIsItemSelected(TreeViewItem element) => (bool)element.GetValue(IsItemSelectedProperty);

        public static void SetIsItemSelected(TreeViewItem element, Boolean value) => element.SetValue(IsItemSelectedProperty, value);

        private static void OnMultipleSelectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TreeView treeView)
            {
                if (e.NewValue is bool)
                {

                    if ((bool)e.NewValue)
                    {
                        treeView.AddHandler(TreeViewItem.MouseLeftButtonUpEvent, new MouseButtonEventHandler(OnTreeViewItemClickUp), true);
                        InitIreeViewItemStyle(treeView);
                    }
                    else
                    {
                        treeView.RemoveHandler(TreeViewItem.MouseLeftButtonUpEvent, new MouseButtonEventHandler(OnTreeViewItemClickUp));
                    }
                }
            }
        }

        private static void InitIreeViewItemStyle(TreeView treeView)
        {
            if (treeView.ItemContainerStyle == null)
                treeView.ItemContainerStyle = new Style { TargetType = typeof(TreeViewItem) };
            else
                treeView.ItemContainerStyle = new Style { TargetType = typeof(TreeViewItem), BasedOn = treeView.ItemContainerStyle };

            Trigger trigger = new Trigger();
            trigger.Property = TreeViewMultipleSelectionBehavior.IsItemSelectedProperty;
            trigger.Value = true;
            trigger.Setters.Add(new Setter { Property = TreeViewItem.BackgroundProperty, Value = SystemColors.HighlightBrush });
            trigger.Setters.Add(new Setter { Property = TreeViewItem.ForegroundProperty, Value = SystemColors.HighlightTextBrush });
            treeView.ItemContainerStyle.Triggers.Add(trigger);

            Setter setter1 = new Setter { Property = TreeViewMultipleSelectionBehavior.IsItemSelectedProperty, Value = new Binding { Path = new PropertyPath(nameof(INode.IsSelected)), Mode = BindingMode.TwoWay } };
            treeView.ItemContainerStyle.Setters.Add(setter1);

            MultiTrigger multiTrigger2 = new MultiTrigger();
            multiTrigger2.Conditions.Add(new Condition { Property = TreeViewMultipleSelectionBehavior.IsItemSelectedProperty, Value = false });
            multiTrigger2.Conditions.Add(new Condition { Property = TreeViewItem.IsSelectedProperty, Value = true });
            multiTrigger2.Setters.Add(new Setter { Property = TreeViewItem.BackgroundProperty, Value = Brushes.Transparent });
            multiTrigger2.Setters.Add(new Setter { Property = TreeViewItem.ForegroundProperty, Value = SystemColors.ControlTextBrush });
            treeView.ItemContainerStyle.Triggers.Add(multiTrigger2);

        }
    }
}
