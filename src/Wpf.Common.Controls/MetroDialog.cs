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
    [TemplatePart(Name = OkButtonName)]
    [TemplatePart(Name = CancelButtonName)]
    public class MetroDialog:HeaderedContentControl
    {
        public const string CloseButtonName = "PART_CLOSE_BUTTON";

        public const string OkButtonName = "PART_OK_BUTTON";

        public const string CancelButtonName = "PART_CANCEL_BUTTON";

        public static readonly DependencyProperty DialogResultProperty = DependencyProperty.Register(nameof(DialogResult),typeof(bool?),typeof(MetroDialog));

        public static readonly DependencyProperty ShowOkButtonProperty = DependencyProperty.Register(nameof(ShowOkButton), typeof(bool), typeof(MetroDialog),new PropertyMetadata(BooleanBoxes.True));

        public static readonly DependencyProperty  OkButtonStyleProperty = DependencyProperty.Register(nameof(OkButtonStyle), typeof(bool), typeof(MetroDialog));

        public static readonly DependencyProperty ShowCancelButtonProperty = DependencyProperty.Register(nameof(ShowCancelButton), typeof(bool), typeof(MetroDialog), new PropertyMetadata(BooleanBoxes.True));


        public MetroDialog()
        {
            
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var closeButton = this.GetTemplateChild(CloseButtonName) as Button; 
            closeButton.Click -= CloseButton_Click;
            closeButton.Click += CloseButton_Click;
            var confirmButton = this.GetTemplateChild(OkButtonName) as Button;
            confirmButton.Click -= ConfirmButton_Click;
            confirmButton.Click += ConfirmButton_Click;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            WindowChromeBehavior.SetDialogContent(Application.Current.MainWindow, null);
            this.DialogResult = true;
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

        public bool ShowOkButton
        {
            get => this.GetValue<bool>(ShowOkButtonProperty);
            set => this.SetValue(ShowOkButtonProperty, value);
        }

        public bool ShowCancelButton
        {
            get => this.GetValue<bool>(ShowCancelButtonProperty);
            set => this.SetValue(ShowCancelButtonProperty, value);
        }

        public Style OkButtonStyle
        {
            get => this.GetValue<Style>(OkButtonStyleProperty);
            set => this.SetValue(OkButtonStyleProperty, value);
        }
    }
}
