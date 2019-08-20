using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.Windows.Markup;
using System.IO;
using System.Xml;

namespace Wpf.Common.Controls
{
    [TemplatePart(Name =DropdownButtonName)]
    [TemplatePart(Name = SelectedItemButtonName)]
    [TemplatePart(Name = BorderName)]
    public class SplitButton:ContentControl
    {
        public const string DropdownButtonName = "PART_DropdownButton";
        public const string SelectedItemButtonName = "PART_SelectedItemButton";
        public const string BorderName = "PART_Border";

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
            var dropdown = this.GetTemplateChild(DropdownButtonName) as ToggleButton;
            if (dropdown == null) return;
            var border = this.GetTemplateChild(BorderName) as Border;
            if (border == null) return;
            var selectedItemButton = this.GetTemplateChild(SelectedItemButtonName) as Button;
            if (selectedItemButton == null) return;
            this.Menu.PlacementTarget = border;
             
            this.Menu.Placement = PlacementMode.Bottom;
            var menuItems = this.Menu.Items.OfType<MenuItem>().ToList();
            menuItems.ForEach(item =>
            {
                item.Click -= OnMenuItem_Click;
                item.Click += OnMenuItem_Click;
            });
            if (menuItems.Count > 0)
                this.SelectItem(menuItems.First());

            dropdown.Click -= OnDropdownButtonClicked;
            dropdown.Click += OnDropdownButtonClicked;
            Menu.Closed -= OnMenuClosed;
            Menu.Closed += OnMenuClosed;
            Menu.DataContext = this.DataContext; //在初始化时ContextMenu的DataContext为null，需要手动带入，否则menuitem的command永远为空除非打开
          
            selectedItemButton.Click -= OnSelectedItemButtonClicked;
            selectedItemButton.Click += OnSelectedItemButtonClicked;

             

        }

        private void SelectItem(MenuItem item )
        {
            if (object.ReferenceEquals(this._currentMenuItem, item))
                return;
            this._currentMenuItem = item;
            this.Content = null;
            var icon = _currentMenuItem.Icon;
            if (icon != null)
                this.Content = icon.Clone();
            this.ToolTip = item.ToolTip;
        }

        private void OnMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.SelectItem(sender as MenuItem);      
        }

        

        private MenuItem _currentMenuItem;

        private void OnSelectedItemButtonClicked(object sender, RoutedEventArgs args)
        {
            if (_currentMenuItem == null) return;
            var item = this.Menu.Items.OfType<MenuItem>().FirstOrDefault(x => object.ReferenceEquals(this._currentMenuItem, x));
            if (item != null)
                item.ExecuteClick();
            args.Handled = true;
        }


        private void OnDropdownButtonClicked(object sender,RoutedEventArgs args)
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
