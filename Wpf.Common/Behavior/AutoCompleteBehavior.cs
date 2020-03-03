using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Specialized;
using System.Windows.Controls.Primitives;
using System.Collections;

namespace Wpf.Common.Behavior
{
    //public class AutoCompleteBehavior
    //{
    //    public static DependencyProperty ItemsSourceProperty = DependencyProperty.RegisterAttached(
    //        "ItemsSource",
    //        typeof(IEnumerable),
    //        typeof(AutoCompleteBehavior),
    //        new PropertyMetadata(null, OnItemSourcePropertyChangedCallback));


    //    public static IEnumerable GetItemsSource(DependencyObject obj)
    //    {
    //        return (IEnumerable)obj.GetValue(ItemsSourceProperty);
    //    }

    //    public static void SetItemsSource(DependencyObject obj, IEnumerable value)
    //    {
    //        obj.SetValue(ItemsSourceProperty, value);
    //    }

    //    public static DependencyProperty SelectedItemProperty = DependencyProperty.RegisterAttached(
    //      "SelectedItem",
    //      typeof(object),
    //      typeof(AutoCompleteBehavior),
    //      new PropertyMetadata());


    //    public static object GetSelectedItem(DependencyObject obj)
    //    {
    //        return  obj.GetValue(SelectedItemProperty);
    //    }

    //    public static void SetSelectedItem(DependencyObject obj, object value)
    //    {
    //        obj.SetValue(SelectedItemProperty, value);
    //    }

    //    private static void OnItemSourcePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    //    {
    //        var textBox = d as TextBoxBase;
    //        if (textBox == null)
    //            return;
    //        SetSelectedItem(d, null);
    //        textBox.TextChanged -= TextBox_TextChanged;
    //        textBox.TextChanged += TextBox_TextChanged;
    //        textBox.KeyUp += TextBox_KeyDown;
            
    //    }

    //    private static void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    //    {
    //        if (e.Key != System.Windows.Input.Key.Down)
    //            return;
    //        var tb = sender as TextBoxBase;
    //        var raw = GetItemsSource(tb);
           
    //        if (raw == null) return;
    //        var lst = raw.OfType<object>().ToList();
    //        if (lst.Count() == 0) return;
    //        var curr = GetSelectedItem(tb);
    //        SetSelectedItem(tb, curr == null ? lst.First() : lst[lst.IndexOf(curr)]);
    //    }

    //    private static void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    //    {
            
    //    }
    //}

}
