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
    public class SplitButton:ContentControl
    {
        public const string DropdownButtonName = "PART_DropdownButton";

        public static readonly DependencyProperty MenuProperty = DependencyProperty.Register("Menu"
            , typeof(ContextMenu)
            , typeof(SplitButton),new PropertyMetadata(default(ContextMenu)));

      

        public static DependencyProperty IsDropdownProperty = DependencyProperty.Register("IsDropdown"
           , typeof(bool)
           , typeof(SplitButton), new PropertyMetadata(OnIsDropDownPropertyValueChanged));

        public static readonly DependencyProperty ButtonStyleProperty = DependencyProperty.Register("ButtonStyle"
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

        public Style ButtonStyle
        {
            get => this.GetValue<Style>(ButtonStyleProperty);
            set => this.SetValue(ButtonStyleProperty, value);

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
            var contextMenu = this.Menu;
            var button = this.GetTemplateChild(DropdownButtonName) as ToggleButton;
            this.Menu.PlacementTarget = button;
            this.Menu.Placement = PlacementMode.Bottom;
            button.Click -= OnDropdownButtonClicked;
            button.Click += OnDropdownButtonClicked;
            contextMenu.Closed -= OnMenuClosed;
            contextMenu.Closed += OnMenuClosed;
            
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
