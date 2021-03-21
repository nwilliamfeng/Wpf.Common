using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace Wpf.Common.Controls
{
    public class MetroDialog:HeaderedContentControl
    {
        public static readonly DependencyProperty DialogResultProperty = DependencyProperty.Register(nameof(DialogResult),typeof(bool?),typeof(MetroDialog));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        public bool? DialogResult
        {
            get => this.GetValue<bool?>(DialogResultProperty);
            set => this.SetValue(DialogResultProperty, value);
        }
    }
}
