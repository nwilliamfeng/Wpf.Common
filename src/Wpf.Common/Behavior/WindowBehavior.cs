using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wpf.Common.Behavior
{
    /// <summary>
    ///  WindowBehavior
    /// </summary>
    public class WindowBehavior
    {
        #region Blink
        public static readonly DependencyProperty IsBlinkEnableProperty =
        DependencyProperty.RegisterAttached("IsBlinkEnable", typeof(bool), typeof(WindowBehavior), new PropertyMetadata(BooleanBoxes.False, OnIsBlinkEnablePropertyChanged));

        public static bool GetIsBlinkEnable(DependencyObject obj) => obj.GetValue<bool>(IsBlinkEnableProperty);

        public static void SetIsBlinkEnable(DependencyObject obj, bool value) => obj.SetValue(IsBlinkEnableProperty, value.Box());


        private static void OnIsBlinkEnablePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var window = sender as Window;
            if (window == null || !(e.NewValue is bool))
                return;
            var isEnable = (bool)e.NewValue;
            if (isEnable)
                window.FlashWindow();
            else
                window.StopFlashingWindow();
        }

        public static readonly DependencyProperty CloseBlinkWhenDeactiveProperty =
       DependencyProperty.RegisterAttached("CloseBlinkWhenDeactive", typeof(bool), typeof(WindowBehavior), new PropertyMetadata(BooleanBoxes.False, OnCloseBlinkWhenDeactiveChange));

        public static bool GetCloseBlinkWhenDeactive(DependencyObject obj) => obj.GetValue<bool>(CloseBlinkWhenDeactiveProperty);

        public static void SetCloseBlinkWhenDeactive(DependencyObject obj, bool value) => obj.SetValue(CloseBlinkWhenDeactiveProperty, value.Box());

        private static void OnCloseBlinkWhenDeactiveChange(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var window = sender as Window;
            if (window == null || !(e.NewValue is bool))
                return;
            var isEnable = (bool)e.NewValue;
            if (!isEnable) return;
            window.Activated += OnWindowActivated;
            window.StopFlashingWindow();

        }

        private static void OnWindowActivated(object sender, System.EventArgs e)
        {
            (sender as Window).StopFlashingWindow();
        }
        #endregion

        #region Esc To Close

        public static readonly DependencyProperty EscToCloseProperty
            = DependencyProperty.RegisterAttached("EscToClose", typeof(bool), typeof(WindowBehavior), new PropertyMetadata(BooleanBoxes.False, new PropertyChangedCallback(OnEscToClosePropertyChanged)));

        private static void OnEscToClosePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var window = sender as Window;
            if (window == null) return;
            if (!(e.NewValue is bool)) return;
            bool enable = (bool)e.NewValue;
            if (!enable) return;
            window.KeyDown -= OnWatchWindow_Esc_KeyDown;
            window.KeyDown += OnWatchWindow_Esc_KeyDown;
            window.Unloaded += delegate
            {
                window.KeyDown -= OnWatchWindow_Esc_KeyDown;
            };
        }

        public static void SetEscToClose(DependencyObject obj, bool enable) => obj.SetValue(EscToCloseProperty, enable.Box());

        public static void GetEscToClose(DependencyObject obj) => obj.GetValue<bool>(EscToCloseProperty);

        private static void OnWatchWindow_Esc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                (sender as Window)?.Close();
        }

        #endregion
    }
}