﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace Wpf.Common.Controls
{
    //https://stackoverflow.com/questions/4672867/can-i-use-a-different-template-for-the-selected-item-in-a-wpf-combobox-than-for
    public class ComboBoxTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SelectedItemTemplate { get; set; }
        public DataTemplateSelector SelectedItemTemplateSelector { get; set; }
        public DataTemplate DropdownItemsTemplate { get; set; }
        public DataTemplateSelector DropdownItemsTemplateSelector { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var itemToCheck = container;

            // Search up the visual tree, stopping at either a ComboBox or
            // a ComboBoxItem (or null). This will determine which template to use
            while (itemToCheck != null && !(itemToCheck is ComboBoxItem) && !(itemToCheck is ComboBox))
                itemToCheck = VisualTreeHelper.GetParent(itemToCheck);

            // If you stopped at a ComboBoxItem, you're in the dropdown
            var inDropDown = (itemToCheck is ComboBoxItem);

            return inDropDown
                ? DropdownItemsTemplate ?? DropdownItemsTemplateSelector?.SelectTemplate(item, container)
                : SelectedItemTemplate ?? SelectedItemTemplateSelector?.SelectTemplate(item, container);
        }
    }

    public class ComboBoxTemplateSelectorExtension : MarkupExtension
    {
        public DataTemplate SelectedItemTemplate { get; set; }
        public DataTemplateSelector SelectedItemTemplateSelector { get; set; }
        public DataTemplate DropdownItemsTemplate { get; set; }
        public DataTemplateSelector DropdownItemsTemplateSelector { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new ComboBoxTemplateSelector()
            {
                SelectedItemTemplate = SelectedItemTemplate,
                SelectedItemTemplateSelector = SelectedItemTemplateSelector,
                DropdownItemsTemplate = DropdownItemsTemplate,
                DropdownItemsTemplateSelector = DropdownItemsTemplateSelector
            };
        }
    }
}
