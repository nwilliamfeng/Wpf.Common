using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows;

namespace Wpf.Common.Controls
{
    [TemplatePart(Name =DropdownButtonName)]
    [TemplatePart(Name = SelectedItemButtonName)]
    public class SplitButton:ContentControl
    {
        public const string DropdownButtonName = "PART_DropdownButton";
        public const string SelectedItemButtonName = "PART_SelectedItemButton";

        public static readonly DependencyProperty MenuProperty = DependencyProperty.Register("Menu"
            , typeof(ContextMenu)
            , typeof(SplitButton),new PropertyMetadata(default(ContextMenu)));

      

        public static DependencyProperty IsDropdownProperty = DependencyProperty.Register("IsDropdown"
           , typeof(bool)
           , typeof(SplitButton), new PropertyMetadata(OnIsDropDownPropertyValueChanged));

        public static readonly DependencyProperty DropdownButtonStyleProperty = DependencyProperty.Register("DropdownButtonStyle"
           , typeof(Style)
           , typeof(SplitButton), new PropertyMetadata(default(Style)));


        public static readonly DependencyProperty SelectedItemButtonStyleProperty = DependencyProperty.Register("SelectedItemButtonStyle"
          , typeof(Style)
          , typeof(SplitButton), new PropertyMetadata(default(Style)));


        public ContextMenu Menu
        {
            get => this.GetValue<ContextMenu>(MenuProperty);
            set => this.SetValue(MenuProperty, value);
           
        }

        public bool IsDropdown
        {
            get => this.GetValue<bool>(IsDropdownProperty);
            set => this.SetValue(IsDropdownProperty, value);

        }

        public Style DropdownButtonStyle
        {
            get => this.GetValue<Style>(DropdownButtonStyleProperty);
            set => this.SetValue(DropdownButtonStyleProperty, value);
        }

        public Style SelectedItemButtonStyle
        {
            get => this.GetValue<Style>(SelectedItemButtonStyleProperty);
            set => this.SetValue(SelectedItemButtonStyleProperty, value);
        }


        private static void OnIsDropDownPropertyValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (!(args.NewValue is bool)) return;
            var isDropdown = (bool)args.NewValue;
            var contextMenu = obj.GetValue(MenuProperty) as ContextMenu;
            if (contextMenu == null) return;
            contextMenu.IsOpen = isDropdown;
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
 
            if (Menu == null) return;
            var button = this.GetTemplateChild(DropdownButtonName) as ToggleButton;
            this.Menu.PlacementTarget = button;
            this.Menu.Placement = PlacementMode.Bottom;
            button.Click -= OnDropdownButtonClicked;
            button.Click += OnDropdownButtonClicked;
            Menu.Closed -= OnMenuClosed;
            Menu.Closed += OnMenuClosed;
            
        }

       

        private void  OnDropdownButtonClicked(object sender,RoutedEventArgs args)
        {
            this.IsDropdown = (sender as ToggleButton).IsChecked == true;
            args.Handled = true;
        }

        private void OnMenuClosed(object sender, RoutedEventArgs args)
        {
            this.IsDropdown = false;
            var button = this.GetTemplateChild(DropdownButtonName) as ToggleButton;
            button.IsChecked = false;
            args.Handled = true;
        }
    }
}
