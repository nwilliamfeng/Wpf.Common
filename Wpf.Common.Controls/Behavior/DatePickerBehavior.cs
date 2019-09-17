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
    /// 日期框行为，支持水印和圆角
    /// </summary>
    public  class DatePickerBehavior 
    {
        
     
       

        /// <summary>
        /// 设置圆角
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
           DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(DatePickerBehavior), new PropertyMetadata(default(CornerRadius),OnCornerRaduisPropertyChanged));


        public static void SetCornerRadius(UIElement obj, CornerRadius value) => obj.SetValue(CornerRadiusProperty, value);

        public static CornerRadius GetCornerRadius(UIElement obj) => obj.GetValue<CornerRadius>(CornerRadiusProperty);

 
      

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
