using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Specialized;
using System.Windows.Controls.Primitives;

namespace Wpf.Common.Behavior
{
    public class SelectorBehavior
    {
        public static DependencyProperty FocusWhenSelectedProperty = DependencyProperty.RegisterAttached(
            "FocusWhenSelected",
            typeof(bool),
            typeof(SelectorBehavior),
            new PropertyMetadata(false, new PropertyChangedCallback(OnFocusWhenSelectedChanged)));


        public static bool GetFocusWhenSelected(DependencyObject obj)
        {
            return (bool)obj.GetValue(FocusWhenSelectedProperty);
        }

        public static void SetFocusWhenSelected(DependencyObject obj, bool value)
        {
            obj.SetValue(FocusWhenSelectedProperty, value);
        }


        private static void OnFocusWhenSelectedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var selector = (Selector)obj;
        
            selector.SelectionChanged += (s, e) =>
            {
                try
                {

                    selector.UpdateLayout();
                    var ue = (UIElement)selector
                        .ItemContainerGenerator
                        .ContainerFromItem(selector.SelectedItem);
                    if (ue != null)
                        ue.Focus();
                }
                catch
                {

                }
            };
        }
    }

}
