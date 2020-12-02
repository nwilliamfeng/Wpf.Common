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
   
    public class ComboBoxBehavior
    {
        public const string PART_EMPTY_TextBlock = "PART_EMPTY_TextBlock";

        public static readonly DependencyProperty EmtpyTextProperty = DependencyProperty.RegisterAttached("EmtpyText", typeof(string), typeof(ComboBoxBehavior)
            , new PropertyMetadata(null, OnEmtpyTextPropertyChanged));

        public static string GetEmtpyText(DependencyObject obj) => obj.GetValue<string>(EmtpyTextProperty);

        public static void SetEmtpyText(DependencyObject obj, object value) => obj.SetValue(EmtpyTextProperty, value);

        /// <summary>
        /// 设置圆角
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
           DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(ComboBoxBehavior), new PropertyMetadata(default(CornerRadius), OnCornerRaduisPropertyChanged));


        public static void SetCornerRadius(UIElement obj, CornerRadius value) => obj.SetValue(CornerRadiusProperty, value);

        public static CornerRadius GetCornerRadius(UIElement obj) => obj.GetValue<CornerRadius>(CornerRadiusProperty);



        private static void OnEmtpyTextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null)
                return;
            var txt = e.NewValue as string;
            if (string.IsNullOrEmpty(txt)) return;
            comboBox.Initialized -= ComboBox_Initialized;
            comboBox.Initialized += ComboBox_Initialized;
        }

        private static void ComboBox_Initialized(object sender, System.EventArgs e)
        {
            var comboBox = sender as ComboBox;
            var tb = comboBox.FindChildrenFromTemplate<TextBlock>(PART_EMPTY_TextBlock);
            if (tb != null)
                tb.Text = GetEmtpyText(comboBox);
        }

        public static readonly DependencyProperty InputFilterProperty =
           DependencyProperty.RegisterAttached("InputFilter", typeof(InputFilter), typeof(ComboBoxBehavior), new PropertyMetadata(InputFilter.None));

        public static void SetInputFilter(UIElement obj, object value) => obj.SetValue(InputFilterProperty, value);

        public static InputFilter GetInputFilter(UIElement obj) => obj.GetValue<InputFilter>(InputFilterProperty);

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
            var border = control.FindChildrenFromTemplate<Border>(ControlTemplatePartNames.PART_BorderName);
            if (border != null)
                border.CornerRadius = GetCornerRadius(control);
        }
    }
}
