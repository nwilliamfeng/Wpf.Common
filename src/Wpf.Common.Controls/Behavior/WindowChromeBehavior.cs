using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shell;
using Wpf.Common.Controls;

namespace Wpf.Common.Controls.Behavior
{
   
    public static class WindowChromeBehavior
    {
        public const string WINDOW_BORDER = "PART_WindowBorder";
        public const string CONTENT_CONTROL = "PART_ContentControl";
        public const string TITLE = "PART_Title";
        public const string TITLE_GRID = "PART_TitleGrid";

        public const string CLOSE_BUTTON = "PART_CloseButton";
        public const string MAXIMIZE_BUTTON = "PART_MaximizeButton";
        public const string MINIMIZE_BUTTON = "PART_MinimizeButton";

        /// <summary>
        /// 设置是否可用，默认是false，即不启用定制样式
        /// </summary>
        public static readonly DependencyProperty IsEnableProperty = DependencyProperty.RegisterAttached("IsEnable", typeof(bool), typeof(WindowChromeBehavior), new PropertyMetadata(BooleanBoxes.False, OnIsEnablePropertyChange));

        /// <summary>
        /// 设置窗体标题栏的高度
        /// </summary>
        public static readonly DependencyProperty TitleHeightProperty = DependencyProperty.RegisterAttached("TitleHeight", typeof(int), typeof(WindowChromeBehavior), new PropertyMetadata(28, OnTitleHeightPropertyChange));

        /// <summary>
        /// 设置标题栏的背景色
        /// </summary>
        public static readonly DependencyProperty TitleBackgroundProperty = DependencyProperty.RegisterAttached("TitleBackground", typeof(Brush), typeof(WindowChromeBehavior), new PropertyMetadata(Brushes.Transparent, OnTitleBackgroundPropertyChange));

        /// <summary>
        /// 设置标题栏的字体大小
        /// </summary>
        public static readonly DependencyProperty TitleFontSizeProperty = DependencyProperty.RegisterAttached("TitleFontSize", typeof(int), typeof(WindowChromeBehavior), new PropertyMetadata(14));

        /// <summary>
        /// 设置标题的内容
        /// </summary>
        public static readonly DependencyProperty TitleContentProperty = DependencyProperty.RegisterAttached("TitleContent", typeof(FrameworkElement), typeof(WindowChromeBehavior), new PropertyMetadata(OnTitleContentPropertyChange));

        public static readonly DependencyProperty TitleContentVerticalAlignmentProperty = DependencyProperty.RegisterAttached("TitleContentVerticalAlignment", typeof(VerticalAlignment), typeof(WindowChromeBehavior), new PropertyMetadata(VerticalAlignment.Center, OnTitleContentVerticalAlignmentPropertyChange));

        public static readonly DependencyProperty TitleForegroundProperty = DependencyProperty.RegisterAttached("TitleForeground", typeof(Brush), typeof(WindowChromeBehavior), new PropertyMetadata(Brushes.Black));

        /// <summary>
        /// 设置窗体命令按钮的Padding
        /// </summary>
        public static readonly DependencyProperty WindowCommandButtonPaddingProperty = DependencyProperty.RegisterAttached("WindowCommandButtonPadding", typeof(Thickness), typeof(WindowChromeBehavior), new PropertyMetadata(new Thickness(16, 9, 16, 9), OnWindowCommandButtonPaddingPropertyChange));

        public static readonly DependencyProperty WindowCommandButtonMouseOverColorProperty = DependencyProperty.RegisterAttached("WindowCommandButtonMouseOverColor", typeof(Brush), typeof(WindowChromeBehavior), new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E5E5E5"))));

        /// <summary>
        /// 设置最小化按钮是否可见
        /// </summary>
        public static readonly DependencyProperty MinimizeButtonVisibleWhenInToolWindowModeProperty = DependencyProperty.RegisterAttached("MinimizeButtonVisibleWhenInToolWindowMode", typeof(bool), typeof(WindowChromeBehavior), new PropertyMetadata(BooleanBoxes.False));



        /// <summary>
        /// 设置窗体边框画刷
        /// </summary>
        public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.RegisterAttached("BorderBrush", typeof(SolidColorBrush), typeof(WindowChromeBehavior), new PropertyMetadata(ResourceHelper.GetWindowActiveBorderBrush()));

        private static void OnTitleHeightPropertyChange(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is int)) return;
            var height = (int)e.NewValue;
            var window = obj as Window;
            if (window == null) return;
            var windowBorder = window.FindChildrenFromTemplate<Border>(WINDOW_BORDER);
            var titleBorder = windowBorder.FindChildren<Grid>(TITLE_GRID);
            titleBorder.Height = height;
            Action setSize = () =>
            {
                var chrome = WindowChrome.GetWindowChrome(window);
                if (chrome != null) chrome.CaptionHeight = GetChromeCaptionHeight(window);
            };
            if (window.IsInitialized)
                setSize();
            else
                window.Initialized += delegate
                {
                    setSize();
                };
        }

        private static double GetChromeCaptionHeight(UIElement el)
        {
            return GetTitleHeight(el as UIElement) - 5;
        }

        private static void OnTitleBackgroundPropertyChange(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is Brush)) return;
            var brush = (Brush)e.NewValue;
            var window = obj as Window;
            if (window == null) return;
            var windowBorder = window.FindChildrenFromTemplate<Border>(WINDOW_BORDER);
            var titleBorder = windowBorder.FindChildren<Grid>(TITLE_GRID);
            titleBorder.Background = brush;
        }


        private static void OnWindowCommandButtonPaddingPropertyChange(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is Thickness)) return;
            var padding = (Thickness)e.NewValue;
            var window = obj as Window;
            if (window == null) return;
            window.Initialized += delegate
            {
                var windowBorder = window.FindChildrenFromTemplate<Border>(WINDOW_BORDER);
                windowBorder.FindChildren<Button>().ToList().ForEach(x => x.Padding = padding);
            };

        }

        private static void OnTitleContentVerticalAlignmentPropertyChange(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is VerticalAlignment)) return;
            var va = (VerticalAlignment)e.NewValue;
            var window = obj as Window;
            if (window == null) return;
            window.Initialized += delegate
            {
                var contentControl = window.FindChildrenFromTemplate<Border>(WINDOW_BORDER)?.FindChildren<ContentControl>(TITLE);
                contentControl.VerticalAlignment = va;
            };
        }

        private static void OnTitleContentPropertyChange(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is FrameworkElement)) return;
            var el = (FrameworkElement)e.NewValue;
            var window = obj as Window;
            if (window == null) return;
            var contentControl = window.FindChildrenFromTemplate<Border>(WINDOW_BORDER)?.FindChildren<ContentControl>(TITLE);
            contentControl.Content = el;
        }

        private static void OnIsEnablePropertyChange(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is bool) || !(bool)e.NewValue) return;
            var window = obj as Window;
            if (window == null) return;
            if (window.WindowStyle == WindowStyle.None) //如果是none无需加载
                return;
            var windowStyle = window.WindowStyle;

            Action init = () =>
            {
                var content = window.Content as UIElement;
                var windowBorder = window.FindChildrenFromTemplate<Border>(WINDOW_BORDER);

                var titleBorder = windowBorder.FindChildren<Grid>(TITLE_GRID);
                TextElement.SetFontSize(titleBorder, GetTitleFontSize(window));
                TextElement.SetForeground(titleBorder, GetTitleForeground(window));
                titleBorder.Background = GetTitleBackground(obj as UIElement);
                titleBorder.Height = GetTitleHeight(obj as UIElement);

                var windowContentControl = windowBorder.FindChildren<ContentControl>(CONTENT_CONTROL);

                windowContentControl.Content = content;
                window.Content = windowBorder.Parent;
                //注册命令按钮事件

                var buttons = windowBorder.FindChildren<Button>().ToList();
                buttons.ForEach(x => x.Foreground = GetTitleForeground(window));
                var maxBtn = buttons.First(x => x.Name == MAXIMIZE_BUTTON);
                var minBtn = buttons.First(x => x.Name == MINIMIZE_BUTTON);
                if (windowStyle == WindowStyle.ToolWindow)
                {
                    maxBtn.Visibility = Visibility.Collapsed;

                    minBtn.Visibility = GetMinimizeButtonVisibleWhenInToolWindowMode(window) ? Visibility.Visible : Visibility.Collapsed;
                    window.ResizeMode = ResizeMode.CanResize;
                }
                else
                {
                    maxBtn.Click += (s, arg) => window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                }

                minBtn.Click += (s, arg) => window.WindowState = WindowState.Minimized;
                buttons.First(x => x.Name == CLOSE_BUTTON).Click += (s, arg) => window.Close();
            };
            if (window.IsInitialized)
                init();
            else
                window.Initialized += delegate
                {
                    init();

                };

            WindowChrome chrome = new WindowChrome
            {
                // ResizeBorderThickness = new Thickness(5),
                //  CornerRadius = new CornerRadius(0),
                //  UseAeroCaptionButtons = false,
                GlassFrameThickness = new Thickness(1),
                CaptionHeight = GetChromeCaptionHeight(window),
                //   NonClientFrameEdges = NonClientFrameEdges.None,
            };

            WindowChrome.SetWindowChrome(window, chrome);
        }



        public static void SetMinimizeButtonVisibleWhenInToolWindowMode(UIElement el, bool value) => el.SetValue(MinimizeButtonVisibleWhenInToolWindowModeProperty, value.Box());

        public static bool GetMinimizeButtonVisibleWhenInToolWindowMode(UIElement el) => el.GetValue<bool>(MinimizeButtonVisibleWhenInToolWindowModeProperty);

        public static void SetIsEnable(UIElement el, bool value) => el.SetValue(IsEnableProperty, value.Box());

        public static bool GetIsEnable(UIElement el) => el.GetValue<bool>(IsEnableProperty);

        public static void SetTitleContentVerticalAlignment(UIElement el, VerticalAlignment value) => el.SetValue(TitleContentVerticalAlignmentProperty, value);

        public static VerticalAlignment GetTitleContentVerticalAlignment(UIElement el) => el.GetValue<VerticalAlignment>(TitleContentVerticalAlignmentProperty);

        public static void SetWindowCommandButtonPadding(UIElement el, Thickness value) => el.SetValue(WindowCommandButtonPaddingProperty, value);

        public static Thickness GetWindowCommandButtonPadding(UIElement el) => el.GetValue<Thickness>(WindowCommandButtonPaddingProperty);

        public static void SetWindowCommandButtonMouseOverColor(UIElement el, Brush value) => el.SetValue(WindowCommandButtonMouseOverColorProperty, value);

        public static Brush GetWindowCommandButtonMouseOverColor(UIElement el) => el.GetValue<Brush>(WindowCommandButtonMouseOverColorProperty);

        public static void SetTitleFontSize(UIElement el, int value) => el.SetValue(TitleFontSizeProperty, value);

        public static int GetTitleFontSize(UIElement el) => el.GetValue<int>(TitleFontSizeProperty);

        public static void SetTitleHeight(UIElement el, int value) => el.SetValue(TitleHeightProperty, value);

        public static int GetTitleHeight(UIElement el) => el.GetValue<int>(TitleHeightProperty);

        public static void SetTitleBackground(UIElement el, Brush value) => el.SetValue(TitleBackgroundProperty, value);

        public static Brush GetTitleBackground(UIElement el) => el.GetValue<Brush>(TitleBackgroundProperty);

        public static void SetTitleContent(UIElement el, FrameworkElement value) => el.SetValue(TitleContentProperty, value);

        public static FrameworkElement GetTitleContent(UIElement el) => el.GetValue<FrameworkElement>(TitleContentProperty);

        public static void SetTitleForeground(UIElement el, Brush value) => el.SetValue(TitleForegroundProperty, value);

        public static Brush GetTitleForeground(UIElement el) => el.GetValue<Brush>(TitleForegroundProperty);

        public static void SetBorderBrush(UIElement el, SolidColorBrush value) => el.SetValue(BorderBrushProperty, value);

        public static SolidColorBrush GetBorderBrush(UIElement el) => el.GetValue<SolidColorBrush>(BorderBrushProperty);
    }
}
