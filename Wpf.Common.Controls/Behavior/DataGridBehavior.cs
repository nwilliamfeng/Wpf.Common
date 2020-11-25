/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：SortColumnDemo
*   文件名称 ：DataGridBehavior.cs
*   =================================
*   创 建 者 ：mingrui.wu
*   创建日期 ：11/20/2020 11:21:06 AM 
*   功能描述 ：
*   使用说明 ：
*   =================================
*   修  改 者 ：
*   修改日期 ：
*   修改内容 ：
*   =================================
*  
***************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Wpf.Common.Controls.Behavior
{
    public class DataGridBehavior
    {
        #region ShowRowNumber

        public static DependencyProperty DisplayRowNumberProperty =
            DependencyProperty.RegisterAttached("DisplayRowNumber",
                                                typeof(bool),
                                                typeof(DataGridBehavior),
                                                new FrameworkPropertyMetadata(false, OnDisplayRowNumberChanged));

        public static bool GetDisplayRowNumber(DependencyObject target)
        {
            return (bool)target.GetValue(DisplayRowNumberProperty);
        }

        public static void SetDisplayRowNumber(DependencyObject target, bool value)
        {
            target.SetValue(DisplayRowNumberProperty, value);
        }

        private static void OnDisplayRowNumberChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (!(target is DataGrid dataGrid))
                return;

            if ((bool)e.NewValue == true)
            {
                try
                {
                    dataGrid.HeadersVisibility = DataGridHeadersVisibility.All;
                    void loadedRowHandler(object sender, DataGridRowEventArgs ea)
                    {
                        if (GetDisplayRowNumber(dataGrid) == false)
                        {
                            dataGrid.LoadingRow -= loadedRowHandler;
                            return;
                        }
                        var idx = ea.Row.GetIndex() + 1;
                        ea.Row.Header = idx;
                        //这里需要设定宽度，否则在datagrid从隐藏状态切换到显示状态会导致左上角header被第一列columnheader给遮盖
                        dataGrid.RowHeaderWidth = 48;
                    }
                    dataGrid.LoadingRow += loadedRowHandler;

                    void itemsChangedHandler(object sender, ItemsChangedEventArgs ea)
                    {
                        if (GetDisplayRowNumber(dataGrid) == false)
                        {
                            dataGrid.ItemContainerGenerator.ItemsChanged -= itemsChangedHandler;
                            return;
                        }
                        GetVisualChildCollection<DataGridRow>(dataGrid).ForEach(d => d.Header = d.GetIndex() + 1);
                    }
                    dataGrid.ItemContainerGenerator.ItemsChanged += itemsChangedHandler;
                }
                catch { }
            }
        }

        private static List<T> GetVisualChildCollection<T>(object parent) where T : Visual
        {
            List<T> visualCollection = new List<T>();
            GetVisualChildCollection(parent as DependencyObject, visualCollection);
            return visualCollection;
        }

        private static void GetVisualChildCollection<T>(DependencyObject parent, List<T> visualCollection) where T : Visual
        {
            try
            {
                int count = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < count; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                    if (child is T)
                    {
                        visualCollection.Add(child as T);
                    }
                    if (child != null)
                    {
                        GetVisualChildCollection(child, visualCollection);
                    }
                }
            }
            catch { }
        }
        #endregion

        #region SelectAllButtonTemplate

        //https://coderelief.net/2011/01/04/style-microsofts-datagrid-selectall-button-using-attached-properties/

        public static readonly DependencyProperty SelectAllButtonTemplateProperty =
            DependencyProperty.RegisterAttached("SelectAllButtonTemplate",
                typeof(ControlTemplate), typeof(DataGridBehavior),
                new UIPropertyMetadata(null, OnSelectAllButtonTemplateChanged));

        public static ControlTemplate GetSelectAllButtonTemplate(DataGrid obj)
        {
            return (ControlTemplate)obj.GetValue(SelectAllButtonTemplateProperty);
        }

        public static void SetSelectAllButtonTemplate(DataGrid obj, ControlTemplate value)
        {
            obj.SetValue(SelectAllButtonTemplateProperty, value);
        }

        private static void OnSelectAllButtonTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is DataGrid dataGrid))
            {
                return;
            }

            void handler(object sender, EventArgs args)
            {
                DependencyObject dep = dataGrid;
                try
                {
                    while (dep != null && VisualTreeHelper.GetChildrenCount(dep) != 0
                        && !(dep is Button && ((Button)dep).Command == DataGrid.SelectAllCommand))
                    {
                        dep = VisualTreeHelper.GetChild(dep, 0);
                    }

                    if (dep is Button button)
                    {
                        ControlTemplate template = GetSelectAllButtonTemplate(dataGrid);
                        button.Template = template;
                        dataGrid.LayoutUpdated -= handler;
                    }
                }
                catch { }
            }
            dataGrid.LayoutUpdated -= handler;
            dataGrid.LayoutUpdated += handler;
        }
        #endregion

        #region ScrollToEndAfterRowAdded

        public static readonly DependencyProperty ScrollToEndAfterRowAddedProperty =
            DependencyProperty.RegisterAttached("ScrollToEndAfterRowAdded",
                                                typeof(bool),
                                                typeof(DataGridBehavior),
                                                new PropertyMetadata(false, OnScrollToEndAfterRowAddedPropertyValueChanged));


        public static bool GetScrollToEndAfterRowAdded(DependencyObject dependencyObject)
            => dependencyObject.GetValue<bool>(ScrollToEndAfterRowAddedProperty);

        public static void SetScrollToEndAfterRowAdded(DependencyObject target, bool value)
            => target.SetValue(ScrollToEndAfterRowAddedProperty, value);

        private static void OnScrollToEndAfterRowAddedPropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is DataGrid dataGrid)) return;
            if (!(e.NewValue is bool)) return;
            if (!(bool)e.NewValue) return;

            dataGrid.Loaded -= DataGrid_Loaded;
            dataGrid.Loaded += DataGrid_Loaded;
        }

        private static void DataGrid_Loaded(object sender, EventArgs e)
        {
            if (!(sender is DataGrid dataGrid))
                return;

            if (!(dataGrid.ItemsSource is INotifyCollectionChanged))
                return;
            if (dataGrid.ItemsSource is INotifyCollectionChanged cv)
            {
                void changedEventHandler(object s, NotifyCollectionChangedEventArgs arg)
                {
                    if (arg.Action == NotifyCollectionChangedAction.Add)
                        foreach (var item in arg.NewItems)
                        {
                            try
                            {
                                dataGrid.ScrollIntoView(item);
                            }
                            catch { }
                        }
                }
                cv.CollectionChanged -= changedEventHandler;
                cv.CollectionChanged += changedEventHandler;
                dataGrid.Unloaded += delegate
                {
                    cv.CollectionChanged -= changedEventHandler;
                };
            }
            dataGrid.Loaded -= DataGrid_Loaded; //load事件只允许订阅一次，所以此处需要取消订阅
        }
        #endregion

        #region MultiSelect Binding

        //https://stackoverflow.com/questions/22868445/select-multiple-items-from-a-datagrid-in-an-mvvm-wpf-project
        public static readonly DependencyProperty SelectedItemsSourceProperty =
            DependencyProperty.RegisterAttached("SelectedItemsSource",
                                                typeof(IList),
                                                typeof(DataGridBehavior),
                                                new PropertyMetadata(null, OnSelectedItemsSourcePropertyValueChanged));

        public static IEnumerable GetSelectedItemsSource(DependencyObject dependencyObject)
            => dependencyObject.GetValue<IEnumerable>(SelectedItemsSourceProperty);

        public static void SetSelectedItemsSource(DependencyObject target, IEnumerable value)
            => target.SetValue(SelectedItemsSourceProperty, value);

        private static void OnSelectedItemsSourcePropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is DataGrid dataGrid)) return;
            dataGrid.SelectionChanged -= DataGrid_SelectionChanged;
            dataGrid.SelectionChanged += DataGrid_SelectionChanged;
        }

        private static void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var dataGrid = sender as DataGrid;
                dataGrid.SetValue(SelectedItemsSourceProperty, dataGrid.SelectedItems);
            }
            catch { }
        }
        #endregion

        #region DataColumnConfig
        private static readonly string IgnoreColumn = "CheckBox";

        /// <summary>
        /// 需要配置的列集合
        /// </summary>
        public static readonly DependencyProperty ConfigColumnsProperty =
            DependencyProperty.RegisterAttached("ConfigColumns",
                                                typeof(IEnumerable),
                                                typeof(DataGridBehavior),
                                                new PropertyMetadata(null, OnConfigColumnsPropertyValueChanged));

        public static IEnumerable GetConfigColumns(DependencyObject dependencyObject)
            => dependencyObject.GetValue<IEnumerable>(ConfigColumnsProperty);

        public static void SetConfigColumns(DependencyObject target, IEnumerable value)
            => target.SetValue(ConfigColumnsProperty, value);

        private static void OnConfigColumnsPropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is DataGrid dataGrid)) return;
            if (!(e.NewValue is IEnumerable<DataGridColumnConfig> configs)) return;

            void loadDataGrid(object sender, RoutedEventArgs eventarg)
            {
                if (configs is IList<DataGridColumnConfig> configList && configList != null)
                {
                    try
                    {
                        // 删除不需要的设置显示或隐藏的列
                        var excludeColumns = ExcludeConfigColumns(d, configList);
                        // 设置默认隐藏列
                        var hideColumns = HideColumns(d, configList);
                        // 设置不需要排序的列
                        var unsortColumns = GetIgnoreSortColumns(d, configList);
                        // 设置默认忽略的列
                        var checkList = configList.Where(l => l.Name.Contains(IgnoreColumn)).ToList();
                        checkList.ForEach(item => configList.Remove(item));

                        // 当本地没有存储列隐藏的配置文件时，需要设置“DefaultHideColumnNames”中默认隐藏的列
                        // 故此处 isDefault = true 表示的是不是从本地加载列的显示或隐藏配置
                        bool isDefault = configList.Count == 0;

                        var gridColumns = dataGrid.Columns.Where(c => c.Header != null);
                        foreach (var col in gridColumns)
                        {
                            var header = col.Header.ToString().Trim();
                            if (header.Length > 0
                                && !excludeColumns.Contains(header)
                                && !header.Contains(IgnoreColumn))
                            {
                                var config = configList.FirstOrDefault(y => y.Name == header);
                                if (config == null)
                                {
                                    config = new DataGridColumnConfig
                                    {
                                        Name = header,
                                        SortName = col.SortMemberPath
                                    };
                                    configList.Add(config);
                                }

                                if (isDefault && hideColumns.Contains(config.Name))
                                {
                                    config.Visible = false;
                                    config.IsDefaultHide = true;
                                }

                                if (unsortColumns.Contains(config.Name))
                                {
                                    config.IsIgnoreSorting = true;
                                }

                                col.Visibility = config.IsEnable && config.Visible ? Visibility.Visible : Visibility.Collapsed;

                                void changed(object s, PropertyChangedEventArgs arg)
                                {
                                    if (arg.PropertyName == "Visible" || arg.PropertyName == "IsEnable")
                                    {
                                        col.Visibility = config.IsEnable && config.Visible ? Visibility.Visible : Visibility.Collapsed;
                                    }
                                    if (arg.PropertyName == "Name")
                                    {
                                        if (header != config.Name)
                                            config.Name = header;
                                    }
                                    if (arg.PropertyName == "SortDirection")
                                    {
                                        col.SortDirection = config.SortDirection;
                                        config.SortName = col.SortMemberPath;
                                    }
                                }
                                config.PropertyChanged -= changed;
                                config.PropertyChanged += changed;
                            }
                        }
                    }
                    catch { }
                }
            }
            dataGrid.Loaded -= loadDataGrid;
            dataGrid.Loaded += loadDataGrid;
        }

        #region 不需要配置的列名称
        public static readonly DependencyProperty ExcludeConfigColumnNamesProperty =
            DependencyProperty.RegisterAttached("ExcludeConfigColumnNames",
                                                typeof(string),
                                                typeof(DataGridBehavior));

        public static string GetExcludeConfigColumnNames(DependencyObject dependencyObject)
            => dependencyObject.GetValue<string>(ExcludeConfigColumnNamesProperty);

        public static void SetExcludeConfigColumnNames(DependencyObject target, string value)
            => target.SetValue(ExcludeConfigColumnNamesProperty, value);

        private static string[] ExcludeConfigColumns(DependencyObject d, IList<DataGridColumnConfig> columnConfigs)
        {
            if (columnConfigs == null)
                return new string[] { };

            if (GetExcludeConfigColumnNames(d) is string excludeNames && !string.IsNullOrEmpty(excludeNames))
            {
                var excludeColumnNames = excludeNames.Split(',');
                if (columnConfigs.Count > 0)
                {
                    excludeColumnNames.ToList().ForEach(x =>
                    {
                        var column = columnConfigs.FirstOrDefault(item => item.Name == x);
                        if (column != null)
                            columnConfigs.Remove(column);
                    });
                }
                return excludeColumnNames;
            }
            return new string[] { };
        }
        #endregion

        #region 不需要排序的列名称
        // 说明：配置在“ExcludeConfigColumnNames”中的列 和 “ExcludeSortColumnNames”都是不需要排序的列
        public static readonly DependencyProperty ExcludeSortColumnNamesProperty =
            DependencyProperty.RegisterAttached("ExcludeSortColumnNames",
                                                typeof(string),
                                                typeof(DataGridBehavior));

        public static string GetExcludeSortColumnNames(DependencyObject dependencyObject)
            => dependencyObject.GetValue<string>(ExcludeSortColumnNamesProperty);

        public static void SetExcludeSortColumnNames(DependencyObject target, string value)
            => target.SetValue(ExcludeSortColumnNamesProperty, value);

        private static string[] GetIgnoreSortColumns(DependencyObject d, IList<DataGridColumnConfig> columnConfigs)
        {
            if (columnConfigs == null)
                return new string[] { };

            if (GetExcludeSortColumnNames(d) is string sortNames && !string.IsNullOrEmpty(sortNames))
            {
                var sortColumns = sortNames.Split(',');
                if (columnConfigs.Count > 0)
                {
                    sortColumns.ToList().ForEach(x =>
                    {
                        var column = columnConfigs.FirstOrDefault(item => item.Name == x);
                        if (column != null)
                        {
                            column.IsIgnoreSorting = true;
                        }
                    });
                }
                return sortColumns;
            }
            return new string[] { };
        }
        #endregion

        #region 需要配置，但默认不显示的列名称
        public static readonly DependencyProperty DefaultHideColumnNamesProperty =
            DependencyProperty.RegisterAttached("DefaultHideColumnNames",
                                                typeof(string),
                                                typeof(DataGridBehavior));

        public static string GetDefaultHideColumnNames(DependencyObject dependencyObject)
            => dependencyObject.GetValue<string>(DefaultHideColumnNamesProperty);

        public static void SetDefaultHideColumnNames(DependencyObject target, string value)
            => target.SetValue(DefaultHideColumnNamesProperty, value);

        private static string[] HideColumns(DependencyObject d, IList<DataGridColumnConfig> columnConfigs)
        {
            if (columnConfigs == null)
                return new string[] { };

            if (GetDefaultHideColumnNames(d) is string hideNames && !string.IsNullOrEmpty(hideNames))
            {
                var hideColumnNames = hideNames.Split(',');
                if (columnConfigs.Count > 0)
                {
                    hideColumnNames.ToList().ForEach(x =>
                    {
                        var column = columnConfigs.FirstOrDefault(item => item.Name == x);
                        if (column != null)
                        {
                            column.IsEnable = true;
                            column.Visible = false;
                            column.IsDefaultHide = true;
                        }
                    });
                }
                return hideColumnNames;
            }
            return new string[] { };
        }
        #endregion

        #endregion

        #region FocusWhenDeleteRow

        public static DependencyProperty FocusAfterUnloadRowProperty =
          DependencyProperty.RegisterAttached("FocusAfterUnloadRow",
                                              typeof(bool),
                                              typeof(DataGridBehavior),
                                              new FrameworkPropertyMetadata(false, OnFocusAfterUnloadRowPropertyValueChanged));

        private static void OnFocusAfterUnloadRowPropertyValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs arg)
        {
            if (!(obj is DataGrid datagrid)) return;
            if (!(arg.NewValue is bool value)) return;
            if (!value) return;
            void handle(object s, DataGridRowEventArgs e)
            {
                var win = Window.GetWindow(datagrid);
                if (win != null)
                    datagrid.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ContextIdle, new Action(() =>
                    {
                        try
                        {
                            FocusManager.SetFocusedElement(Window.GetWindow(datagrid), datagrid);
                        }
                        catch { }
                    }));
            }
            datagrid.UnloadingRow -= handle;
            datagrid.UnloadingRow += handle;
        }

        public static bool GetFocusAfterUnloadRow(DependencyObject obj)
            => obj.GetValue<bool>(FocusAfterUnloadRowProperty);

        public static void SetFocusAfterUnloadRow(DependencyObject obj, bool value)
            => obj.SetValue(FocusAfterUnloadRowProperty, value);
        #endregion
    }
}
