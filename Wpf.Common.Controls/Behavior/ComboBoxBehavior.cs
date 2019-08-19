using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Wpf.Common.Controls.Behavior
{
    /// <summary>
    /// 密码框行为，支持水印和圆角
    /// </summary>
    public  class ComboBoxBehavior
    {
          
        public const string PART_WatermarkName = "PART_Watermark";

        /// <summary>
        /// 设置为空时的文本内容
        /// </summary>
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.RegisterAttached("Watermark", typeof(string), typeof(ComboBoxBehavior), new PropertyMetadata(OnWatermarkPropertyChanged));


        public static void SetWatermark(UIElement obj, string value)=> obj.SetValue(WatermarkProperty, value);

        public static string GetWatermark(UIElement obj)=> obj.GetValue<string>(WatermarkProperty);


        /// <summary>
        /// 设置圆角
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
           DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(ComboBoxBehavior), new PropertyMetadata(default(CornerRadius),OnCornerRaduisPropertyChanged));


        public static void SetCornerRadius(UIElement obj, CornerRadius value) => obj.SetValue(CornerRadiusProperty, value);

        public static CornerRadius GetCornerRadius(UIElement obj) => obj.GetValue<CornerRadius>(CornerRadiusProperty);



        private static void OnWatermarkPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs arg)
        {
            var watermark = arg.NewValue as string;
            if (watermark == null) return;
            var comboBox = source as ComboBox;
            if (comboBox == null || !comboBox.IsEditable) return;
         
            comboBox.Initialized -= OnInitialized;

            comboBox.Initialized += OnInitialized;

        }

        private static void OnInitialized(object sender ,EventArgs args)
        {
            var comboBox = sender as ComboBox;
            var watermark = GetWatermark(comboBox);
            var tb = comboBox.FindChildrenFromTemplate<TextBlock>(PART_WatermarkName);
            if (tb == null) return;
            tb.Text = watermark;
            comboBox.KeyUp -= OnComboBoxKeyUp;
            comboBox.KeyUp += OnComboBoxKeyUp;
            comboBox.SelectionChanged -= OnComboBoxSelectionChanged;
            comboBox.SelectionChanged += OnComboBoxSelectionChanged;

        }

        private static void UpdateWatermarkVisibility(ComboBox comboBox)
        {
            var tb = comboBox.FindChildrenFromTemplate<TextBlock>(PART_WatermarkName);
            if (tb != null)
                tb.Visibility = (comboBox.SelectedItem == null && string.IsNullOrEmpty(comboBox.Text)) ? Visibility.Visible : Visibility.Collapsed;
        }


        private static void OnComboBoxKeyUp(object sender, KeyEventArgs e)
            => UpdateWatermarkVisibility(sender as ComboBox);

        private static void OnComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
            => UpdateWatermarkVisibility(sender as ComboBox);

        private static void OnCornerRaduisPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs arg)
        {
            source.InitializeControlBorderCornerRadius(arg);
        }




    }
}
