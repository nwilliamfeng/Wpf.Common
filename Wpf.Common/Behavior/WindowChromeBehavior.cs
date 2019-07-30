using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shell;
using Wpf.Common.Resources;

namespace Wpf.Common.Behavior
{
    /// <summary>
    /// 自定义Window样式
    /// </summary>
    public static class WindowChromeBehavior
    {
        /// <summary>
        /// 设置是否可用，默认是false，即不启用定制样式
        /// </summary>
        public static readonly DependencyProperty IsEnableProperty = DependencyProperty.RegisterAttached("IsEnable",typeof(bool),typeof(WindowChromeBehavior),new PropertyMetadata(OnIsEnablePropertyChange));

        /// <summary>
        /// 设置窗体标题栏的高度
        /// </summary>
        public static readonly DependencyProperty TitleHeightProperty = DependencyProperty.RegisterAttached("TitleHeight", typeof(int), typeof(WindowChromeBehavior), new PropertyMetadata(28,OnTitleHeightPropertyChange));


        /// <summary>
        /// 设置标题栏的背景色
        /// </summary>
        public static readonly DependencyProperty TitleBackgroundProperty = DependencyProperty.RegisterAttached("TitleBackground", typeof(Brush), typeof(WindowChromeBehavior), new PropertyMetadata(Brushes.Transparent));


        /// <summary>
        /// 设置标题栏的字体大小
        /// </summary>
        public static readonly DependencyProperty TitleFontSizeProperty = DependencyProperty.RegisterAttached("TitleFontSize", typeof(int), typeof(WindowChromeBehavior), new PropertyMetadata(14));

        /// <summary>
        /// 设置标题的内容
        /// </summary>
        public static readonly DependencyProperty TitleContentProperty = DependencyProperty.RegisterAttached("TitleContent", typeof(FrameworkElement), typeof(WindowChromeBehavior), new PropertyMetadata(OnTitleContentPropertyChange));


        public static readonly DependencyProperty TitleForegroundProperty = DependencyProperty.RegisterAttached("TitleForeground", typeof(Brush), typeof(WindowChromeBehavior), new PropertyMetadata(Brushes.Black));


        /// <summary>
        /// 设置窗体边框画刷
        /// </summary>
        public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.RegisterAttached("BorderBrush",typeof(SolidColorBrush),typeof(WindowChromeBehavior),new PropertyMetadata(ResourceHelper.GetWindowActiveBorderBrush()));

        private static void OnTitleHeightPropertyChange(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is int) ) return;
            var height = (int)e.NewValue;
            var window = obj as Window;
            if (window==null) return;
            var windowBorder = ResourceHelper.GetWindowBorder();
            var titleBorder = windowBorder.FindChildren<Border>("titleBorder");
            titleBorder.Height = height;
            var chrome = WindowChrome.GetWindowChrome(window);
            if (chrome != null) chrome.CaptionHeight = height - 5;
        }

         

        private static void OnTitleContentPropertyChange(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is FrameworkElement)) return;
            var el = (FrameworkElement)e.NewValue;
            var window = obj as Window;
            if (window == null) return;
            var contentControl = ResourceHelper.GetWindowBorder().FindChildren<ContentControl>("titleContent");
            contentControl.Content = el;
          
        }

       

        private static void OnIsEnablePropertyChange(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {          
            if (!(e.NewValue is bool) || !(bool)e.NewValue) return;
            if (!(obj is Window)) return;
            var window = obj as Window;
           
           
            window.Initialized += delegate
            {
                var content = window.Content as UIElement;
                var windowBorder = ResourceHelper.GetWindowBorder();

               // windowBorder.BorderBrush = GetBorderBrush(window);
              //  windowBorder.Effect = new DropShadowEffect { ShadowDepth = 0, Color =(windowBorder.BorderBrush as SolidColorBrush).Color,  Opacity = 1, BlurRadius = 5 };

                window.Activated += delegate
                {
                    windowBorder.BorderBrush = GetBorderBrush(window);
                    windowBorder.Effect = new DropShadowEffect { ShadowDepth = 0, Color = (windowBorder.BorderBrush as SolidColorBrush).Color, Opacity = 1, BlurRadius = 5 };
                };
                window.Deactivated += delegate
                {
                    windowBorder.BorderBrush =ResourceHelper.GetWindowInactiveBorderBrush();
                    windowBorder.Effect = new DropShadowEffect { ShadowDepth = 0, Color = (windowBorder.BorderBrush as SolidColorBrush).Color, Opacity = 1, BlurRadius = 5 };
                };

                var titleBorder = windowBorder.FindChildren<Border>("titleBorder");
                TextElement.SetFontSize(titleBorder,GetTitleFontSize(window));
                TextElement.SetForeground(titleBorder, GetTitleForeground(window));
                titleBorder.Background = GetTitleBackground(obj as UIElement);
                titleBorder.Height = GetTitleHeight(obj as UIElement);
                var windowContentControl = windowBorder.FindChildren<ContentControl>("contentControl");
                windowContentControl.Content = content;
                window.Content = windowBorder;
                //注册命令按钮事件
                var buttons = windowBorder.FindChildren<Button>().ToList();
                buttons.First(x => x.Name == "maximizeButton").Click += (s, arg) => window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                buttons.First(x => x.Name == "minimizeButton").Click += (s, arg) => window.WindowState = WindowState.Minimized;
                buttons.First(x => x.Name == "closeButton").Click += (s, arg) => window.Close();

            };

       
            window.WindowStyle = WindowStyle.None;
            window.ResizeMode = ResizeMode.CanResizeWithGrip;
            window.AllowsTransparency = true;
            WindowChrome chrome = new WindowChrome {
                ResizeBorderThickness = new Thickness(5),
                CornerRadius = new CornerRadius(0),
                UseAeroCaptionButtons = false,
                GlassFrameThickness = new Thickness(0),
                NonClientFrameEdges = NonClientFrameEdges.None,
            };

           

            

            window.SizeChanged += delegate
            {
                var windowBorder = ResourceHelper.GetWindowBorder();
                var titleBorder = windowBorder.FindChildren<Border>("titleBorder");
                var newThickness = window.WindowState == WindowState.Maximized ? new Thickness(window.Padding.Left + 5, window.Padding.Top +5, window.Padding.Right+5 , window.Padding.Bottom )
                : window.Padding;
                titleBorder.Padding = newThickness;
                titleBorder.Height = window.WindowState == WindowState.Maximized ? titleBorder.Height + 5 : GetTitleHeight(window);
                windowBorder.FindChildren<Border>("contentBorder").Padding = newThickness;
            };
             
            WindowChrome.SetWindowChrome(window,chrome);
            
        }

       

        public static void SetIsEnable(UIElement el, bool value) => el.SetValue(IsEnableProperty, value);

        public static bool GetIsEnable(UIElement el) => el.GetValue<bool>(IsEnableProperty);


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
