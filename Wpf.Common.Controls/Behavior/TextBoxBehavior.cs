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
    /// <summary>
    /// 文本框行为，支持水印、图标和圆角
    /// </summary>
    public  class TextBoxBehavior
    {
        public const string PART_WatermarkName = "PART_Watermark";
      
      


        /// <summary>
        /// 设置为空时的文本内容
        /// </summary>
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.RegisterAttached("Watermark", typeof(string), typeof(TextBoxBehavior), new PropertyMetadata(OnWatermarkPropertyChanged));


        public static void SetWatermark(UIElement obj, string value)=> obj.SetValue(WatermarkProperty, value);

        public static string GetWatermark(UIElement obj)=> obj.GetValue<string>(WatermarkProperty);


        /// <summary>
        /// 设置圆角
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
           DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(TextBoxBehavior), new PropertyMetadata(default(CornerRadius),OnCornerRaduisPropertyChanged));


        public static void SetCornerRadius(UIElement obj, CornerRadius value) => obj.SetValue(CornerRadiusProperty, value);

        public static CornerRadius GetCornerRadius(UIElement obj) => obj.GetValue<CornerRadius>(CornerRadiusProperty);


        /// <summary>
        /// 设置Icon
        /// </summary>
        public static readonly DependencyProperty IconProperty =
           DependencyProperty.RegisterAttached("Icon", typeof(FrameworkElement), typeof(TextBoxBehavior), new PropertyMetadata(OnIconPropertyChanged));


        public static void SetIcon(UIElement obj, FrameworkElement value) => obj.SetValue(IconProperty, value);

        public static FrameworkElement GetIcon(UIElement obj) => obj.GetValue<FrameworkElement>(IconProperty);


        /// <summary>
        /// 设置IconDock
        /// </summary>
        public static readonly DependencyProperty IconDockProperty =
           DependencyProperty.RegisterAttached("IconDock", typeof(Dock), typeof(TextBoxBehavior), new PropertyMetadata(Dock.Left));


        public static void SetIconDock(UIElement obj, Dock value) => obj.SetValue(IconDockProperty, value);

        public static Dock GetIconDock(UIElement obj) => obj.GetValue<Dock>(IconDockProperty);

 

        private static void OnWatermarkPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs arg)
        {
            var watermark = arg.NewValue as string;
            if (watermark == null) return;
            var textBox = source as TextBox;
            if (textBox == null) return;    
            textBox.Initialized -= OnInitizedToSetWatermark;
            textBox.Initialized += OnInitizedToSetWatermark;
        }

        private static void OnInitizedToSetWatermark(object sender, EventArgs args)
        {
            var textBox = sender as TextBox;
            var tb = textBox.FindChildrenFromTemplate<TextBlock>(PART_WatermarkName);
            if (tb != null)
                tb.Text = GetWatermark(textBox);
        }

        private static void OnCornerRaduisPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs arg)
        {
            var control = source as Control;
            if (control == null) return;
            control.Initialized -= OnInitizedToSetCornerRadius;
            control.Initialized += OnInitizedToSetCornerRadius;
        }


        private static void OnInitizedToSetCornerRadius(object sender, EventArgs args)
        {
            var control = sender as Control;
            var border = control.FindChildrenFromTemplate<Border>(ControlTemplatePartNames. PART_BorderName);
            if (border != null)
                border.CornerRadius = GetCornerRadius(control);
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
            var cc = control.FindChildrenFromTemplate<ContentControl>(ControlTemplatePartNames.PART_IconHostName);
            if (cc != null)
            {
                cc.Content = GetIcon(control);
                DockPanel.SetDock(cc, GetIconDock(sender as UIElement));
            }
        }

    }
}
