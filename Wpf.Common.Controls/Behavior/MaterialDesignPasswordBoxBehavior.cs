using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Wpf.Common.Controls.Behavior
{

    public class MaterialDesignPasswordBoxBehavior:Wpf.Common.Behavior.PasswordBoxBehavior
    {

        public const string PART_WatermarkName = "PART_Watermark";
        private const string PART_Title = "PART_Title";

        /// <summary>
        /// 设置为空时的文本内容
        /// </summary>
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.RegisterAttached("Watermark", typeof(string), typeof(MaterialDesignPasswordBoxBehavior), new PropertyMetadata(OnWatermarkPropertyChanged));


        public static void SetWatermark(UIElement obj, string value) => obj.SetValue(WatermarkProperty, value);

        public static string GetWatermark(UIElement obj) => obj.GetValue<string>(WatermarkProperty);


        


        /// <summary>
        /// 设置Icon
        /// </summary>
        public static readonly DependencyProperty IconProperty =
           DependencyProperty.RegisterAttached("Icon", typeof(FrameworkElement), typeof(MaterialDesignPasswordBoxBehavior), new PropertyMetadata(OnIconPropertyChanged));


        public static void SetIcon(UIElement obj, FrameworkElement value) => obj.SetValue(IconProperty, value);

        public static FrameworkElement GetIcon(UIElement obj) => obj.GetValue<FrameworkElement>(IconProperty);


        private static void OnWatermarkPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs arg)
        {
            var watermark = arg.NewValue as string;
            if (watermark == null) return;
            var passwordBox = source as PasswordBox;
            if (passwordBox == null) return;

            passwordBox.Initialized -= OnInitizedToSetWatermarkVisibility;
            passwordBox.Initialized += OnInitizedToSetWatermarkVisibility;

            passwordBox.PasswordChanged -= OnPasswordChanged;
            passwordBox.PasswordChanged += OnPasswordChanged;
            passwordBox.IsKeyboardFocusedChanged -= PasswordBox_IsKeyboardFocusedChanged;
            passwordBox.IsKeyboardFocusedChanged += PasswordBox_IsKeyboardFocusedChanged;
        }

        private static void PasswordBox_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            var waterMark = passwordBox.FindChildrenFromTemplate<TextBlock>(PART_WatermarkName);
            var title = passwordBox.FindChildrenFromTemplate<TextBlock>(PART_Title);
            if (passwordBox.IsKeyboardFocused)
            {
                title.Visibility = Visibility.Visible;
                waterMark.Visibility = Visibility.Collapsed;
            }
            else
            {
                title.Visibility = string.IsNullOrEmpty(passwordBox.Password) ? Visibility.Collapsed : Visibility.Visible;
                waterMark.Visibility = string.IsNullOrEmpty(passwordBox.Password) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private static void OnInitizedToSetWatermarkVisibility(object sender, EventArgs args)
        {
            var passwordBox = sender as PasswordBox;
            var tb = passwordBox.FindChildrenFromTemplate<TextBlock>(PART_WatermarkName);
            if (tb != null)
            {
                var title = passwordBox.FindChildrenFromTemplate<TextBlock>(PART_Title);
                tb.Text = GetWatermark(passwordBox);
                tb.Visibility = string.IsNullOrEmpty(passwordBox.Password) ? Visibility.Visible : Visibility.Collapsed;
                title.Visibility = Visibility.Collapsed;
            }
        }

        private static void OnPasswordChanged(object sender, RoutedEventArgs args)
        {
            var passwordBox = sender as PasswordBox;
            var tb = passwordBox.FindChildrenFromTemplate<TextBlock>(PART_WatermarkName);
            if (tb != null)
                tb.Visibility = string.IsNullOrEmpty(passwordBox.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

 

        private static void OnIconPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs arg)
        {
            var control = source as Control;
            if (control == null) return;
            control.Initialized -= OnInitizedToSetIcon;
            control.Initialized += OnInitizedToSetIcon;
        }

        private static void OnInitizedToSetIcon(object sender, EventArgs args)
        {
            var control = sender as Control;
            var cc = control.FindChildrenFromTemplate<ContentControl>(ControlTemplateHelper.PART_IconHostName);
            if (cc != null)
            {
                cc.Content = GetIcon(control);
                DockPanel.SetDock(cc, Dock.Left);
            }
        }
    }
}
