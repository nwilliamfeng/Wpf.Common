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
    /// 文本框行为，支持水印和圆角
    /// </summary>
    public static class TextBoxBehavior
    {
        public const string PART_WatermarkName = "PART_Watermark";
        public const string PART_BorderName = "PART_Border";
        public const string PART_IconHostName = "PART_IconHost";

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


        private static void OnIconPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs arg)
        {
            var el = arg.NewValue as FrameworkElement;
            if (el == null) return;
            var textBox = source as TextBox;
            if (textBox == null) return;
            textBox.Initialized += delegate
            {
                var cc = textBox.FindChildrenFromTemplate<ContentControl>(PART_IconHostName);
                if (cc != null)
                {
                    cc.Content = el;
                    DockPanel.SetDock(cc, GetIconDock(textBox));
                }
            };
        }


        private static void OnWatermarkPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs arg)
        {
            var watermark = arg.NewValue as string;
            if (watermark == null) return;
            var textBox = source as TextBox;
            if (textBox == null) return;
            textBox.Initialized += delegate
            {
                var tb= textBox.FindChildrenFromTemplate<TextBlock>(PART_WatermarkName);  
                if (tb != null)
                    tb.Text = watermark;
            };                      
        }

        private static void OnCornerRaduisPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs arg)
        {
            if (!(arg.NewValue is CornerRadius)) return;
            var value =(CornerRadius) arg.NewValue;          
            var textBox = source as TextBox;
            if (textBox == null) return;
            textBox.Initialized += delegate
            {
                var border = textBox.FindChildrenFromTemplate<Border>(PART_BorderName);
                if (border != null)
                    border.CornerRadius=value; 
            };
        }



      
    }
}
