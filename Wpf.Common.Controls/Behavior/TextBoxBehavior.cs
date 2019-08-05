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
        public const string PART_ButtonHostName = "PART_ButtonHost";

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
        /// 设置Button
        /// </summary>
        public static readonly DependencyProperty ButtonProperty =
           DependencyProperty.RegisterAttached("Button", typeof(Button), typeof(TextBoxBehavior), new PropertyMetadata(OnButtonPropertyChanged));


        public static void SetButton(UIElement obj, Button value) => obj.SetValue(ButtonProperty, value);

        public static Button GetButton(UIElement obj) => obj.GetValue<Button>(ButtonProperty);


        /// <summary>
        /// 设置ButtonDock
        /// </summary>
        public static readonly DependencyProperty ButtonDockProperty =
           DependencyProperty.RegisterAttached("ButtonDock", typeof(Dock), typeof(TextBoxBehavior), new PropertyMetadata(Dock.Left));


        public static void SetButtonDock(UIElement obj, Dock value) => obj.SetValue(ButtonDockProperty, value);

        public static Dock GetButtonDock(UIElement obj) => obj.GetValue<Dock>(ButtonDockProperty);


        private static void OnButtonPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs arg)
        {
            var button = arg.NewValue as Button;
            if (button == null) return;
            var textBox = source as TextBox;
            if (textBox == null) return;
            textBox.Initialized += delegate
            {
                var cc = textBox.FindChildrenFromTemplate<ContentControl>(PART_ButtonHostName);
                if (cc != null)
                {
                    cc.Content = button;
                    DockPanel.SetDock(cc, GetButtonDock(textBox));
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
