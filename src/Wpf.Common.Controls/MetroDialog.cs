using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Wpf.Common.Controls.Behavior;

namespace Wpf.Common.Controls
{
    [TemplatePart(Name =CloseButtonName)]
    public class MetroDialog:HeaderedContentControl
    {
        public const string CloseButtonName = "PART_CLOSE_BUTTON";

        public static readonly DependencyProperty DialogResultProperty = DependencyProperty.Register(nameof(DialogResult),typeof(bool?),typeof(MetroDialog));

        ??confirmbutton

        public MetroDialog()
        {

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var button = this.GetTemplateChild(CloseButtonName) as Button; 
            button.Click -= CloseButton_Click;
            button.Click += CloseButton_Click;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            WindowChromeBehavior.SetDialogContent(Application.Current.MainWindow, null);
            this.DialogResult = false;
        }

        public bool? DialogResult
        {
            get => this.GetValue<bool?>(DialogResultProperty);
            set => this.SetValue(DialogResultProperty, value);
        }
    }
}
