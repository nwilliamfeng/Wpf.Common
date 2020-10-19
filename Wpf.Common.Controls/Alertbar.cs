using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.Common.Controls
{
    /// <summary>
    /// 提醒框
    /// </summary>
    public class Alertbar:ContentControl
    {
         

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(Alertbar), new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get => this.GetValue<CornerRadius>(CornerRadiusProperty);
            set => this.SetValue(CornerRadiusProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var button = GetTemplateChild("PART_CloseButton") as Button;
            if (button == null) return;
            button.Click -= Button_Click;
            button.Click += Button_Click;
             
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Content = null;
        }
    }
}
