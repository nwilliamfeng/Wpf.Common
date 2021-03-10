using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Wpf.Common.Controls
{
    /// <summary>
    /// Panel Support Zoom 
    /// </summary>
    /// <seealso cref="https://www.codeproject.com/articles/43295/zoomboxpanel-add-custom-commands-to-a-wpf-control"/>
    public class ZoomBoxPanel : Panel
    {
        #region Public enums and constants

        const double MINZOOM = 0.0000001;
        const double DEFAULTLEFTRIGHTMARGIN = 4;
        const double DEFAULTTOPBOTTOMMARGIN = 3;


        #endregion

        public enum ZoomModes
        {
            CustomSize,
            ActualSize,
            FitWidth,
            FitHeight,
            FitPage,
            FitVisible
        }

        #region Dependency Properties
        // Layout
        private static DependencyProperty PaddingProperty;
        private static DependencyProperty CenterContentProperty;

        // Zoom Scale
        private static DependencyProperty MinZoomProperty;
        private static DependencyProperty MaxZoomProperty;
        private static DependencyProperty ZoomProperty;

        private static DependencyPropertyKey CanIncreaseZoomPropertyKey;
        private static DependencyPropertyKey CanDecreaseZoomPropertyKey;
        private static DependencyPropertyKey CanRotateKey;


        // Delta
        private static DependencyProperty ZoomIncrementProperty;
        private static DependencyProperty RotateIncrementProperty;

        // modes
        private static DependencyProperty ZoomModeProperty;

        public Thickness Padding
        {
            set { SetValue(PaddingProperty, value); }
            get { return (Thickness)GetValue(PaddingProperty); }
        }
        public bool CenterContent
        {
            set { SetValue(CenterContentProperty, value); }
            get { return (bool)GetValue(CenterContentProperty); }
        }



        public double MinZoom
        {
            set { SetValue(MinZoomProperty, value); }
            get { return (double)GetValue(MinZoomProperty); }
        }
        public double MaxZoom
        {
            set { SetValue(MaxZoomProperty, value); }
            get { return (double)GetValue(MaxZoomProperty); }
        }
        public double Zoom
        {
            set { SetValue(ZoomProperty, value); }
            get { return (double)GetValue(ZoomProperty); }
        }

        public bool CanIncreaseZoom
        {
            get { return (bool)GetValue(CanIncreaseZoomPropertyKey.DependencyProperty); }
        }
        public bool CanDecreaseZoom
        {
            get { return (bool)GetValue(CanDecreaseZoomPropertyKey.DependencyProperty); }
        }
        public bool CanRotate
        {
            get { return (bool)GetValue(CanRotateKey.DependencyProperty); }
        }


        public double ZoomIncrement
        {
            set { SetValue(ZoomIncrementProperty, value); }
            get { return (double)GetValue(ZoomIncrementProperty); }
        }
        public double RotateIncrement
        {
            set { SetValue(RotateIncrementProperty, value); }
            get { return (double)GetValue(RotateIncrementProperty); }
        }
        public ZoomBoxPanel.ZoomModes ZoomMode
        {
            set { SetValue(ZoomModeProperty, value); }
            get { return (ZoomBoxPanel.ZoomModes)GetValue(ZoomModeProperty); }
        }


        #endregion

        #region Public Properties

        public bool IsZoomMode_CustomSize { set { if (value) ZoomMode = ZoomModes.CustomSize; } get { return ZoomMode == ZoomModes.CustomSize; } }
        public bool IsZoomMode_ActualSize { set { if (value) ZoomMode = ZoomModes.ActualSize; } get { return ZoomMode == ZoomModes.ActualSize; } }
        public bool IsZoomMode_FitWidth { set { if (value) ZoomMode = ZoomModes.FitWidth; } get { return ZoomMode == ZoomModes.FitWidth; } }
        public bool IsZoomMode_FitHeight { set { if (value) ZoomMode = ZoomModes.FitHeight; } get { return ZoomMode == ZoomModes.FitHeight; } }
        public bool IsZoomMode_FitPage { set { if (value) ZoomMode = ZoomModes.FitPage; } get { return ZoomMode == ZoomModes.FitPage; } }
        public bool IsZoomMode_FitVisible { set { if (value) ZoomMode = ZoomModes.FitVisible; } get { return ZoomMode == ZoomModes.FitVisible; } }


        #endregion

        #region Member variables
        private Size childSize;

        private TranslateTransform translateTransform;
        private RotateTransform rotateTransform;
        private ScaleTransform zoomTransform;
        private TransformGroup transformGroup;

        private double panX, panY;

        private double rotateAngle, rotateCenterX, rotateCenterY;

        #endregion


        #region Properties
        protected double ZoomFactor
        {
            get { return Zoom / 100; }
            set { Zoom = value * 100; }
        }

        #endregion


        #region Command Handlers

        public static RoutedUICommand RotateClockwise { get; private set; }

        public static RoutedUICommand RotateCounterclockwise { get; private set; }

        public static RoutedUICommand RotateHome { get; private set; }

        public static RoutedUICommand RotateReverse { get; private set; }

        private static void CanExecuteEventHandler_IfHasContent(Object sender, CanExecuteRoutedEventArgs e)
        {
            ZoomBoxPanel z = sender as ZoomBoxPanel;
            e.CanExecute = ((z != null) && (z.Children.Count > 0));
            e.Handled = true;
        }

        private static void CanExecuteEventHandler_IfCanIncreaseZoom(Object sender, CanExecuteRoutedEventArgs e)
        {
            ZoomBoxPanel z = sender as ZoomBoxPanel;
            e.CanExecute = ((z != null) && (z.CanIncreaseZoom));
            e.Handled = true;
        }
        private static void CanExecuteEventHandler_IfCanDecreaseZoom(Object sender, CanExecuteRoutedEventArgs e)
        {
            ZoomBoxPanel z = sender as ZoomBoxPanel;
            e.CanExecute = ((z != null) && (z.CanDecreaseZoom));
            e.Handled = true;
        }

        public static void ExecutedEventHandler_IncreaseZoom(Object sender, ExecutedRoutedEventArgs e)
        {
            ZoomBoxPanel z = sender as ZoomBoxPanel;
            if (z != null) z.process_ZoomCommand(true);
        }
        public static void ExecutedEventHandler_DecreaseZoom(Object sender, ExecutedRoutedEventArgs e)
        {
            ZoomBoxPanel z = sender as ZoomBoxPanel;
            if (z != null) z.process_ZoomCommand(false);
        }
        public static void ExecutedEventHandler_RotateClockwise(Object sender, ExecutedRoutedEventArgs e)
        {
            ZoomBoxPanel z = sender as ZoomBoxPanel;
            double? param = null;
            string p = e.Parameter as string;
            if ((p != null) && (p.Length > 0))
                param = Double.Parse(p);
            z?.process_RotateCommand(true, param);
        }
        public static void ExecutedEventHandler_RotateCounterclockwise(Object sender, ExecutedRoutedEventArgs e)
        {
            ZoomBoxPanel z = sender as ZoomBoxPanel;
            double? param = null;
            string p = e.Parameter as string;
            if ((p != null) && (p.Length > 0))
                param = Double.Parse(p);
            if (z != null) z.process_RotateCommand(false, param);
        }

        public static void ExecutedEventHandler_RotateHome(Object sender, ExecutedRoutedEventArgs e)
        {
            ZoomBoxPanel z = sender as ZoomBoxPanel;
            if (z != null) z.process_RotateHomeReverse(true);
        }
        public static void ExecutedEventHandler_RotateReverse(Object sender, ExecutedRoutedEventArgs e)
        {
            ZoomBoxPanel z = sender as ZoomBoxPanel;
            if (z != null) z.process_RotateHomeReverse(false);
        }


        private static void PropertyChanged_Zoom(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZoomBoxPanel z = d as ZoomBoxPanel;
            if (z != null)
                z.process_PropertyChanged_Zoom(e);
        }
        #endregion

        #region Command as methods

        public void IncreaseZoom() => process_ZoomCommand(true);

        public void DecreaseZoom() => process_ZoomCommand(false);

        #endregion


        #region Constructors
        static ZoomBoxPanel()
        {

            // WPF properties
            PaddingProperty = DependencyProperty.Register(
                "Padding", typeof(Thickness), typeof(ZoomBoxPanel),
                new FrameworkPropertyMetadata(
                    new Thickness(DEFAULTLEFTRIGHTMARGIN, DEFAULTTOPBOTTOMMARGIN, DEFAULTLEFTRIGHTMARGIN, DEFAULTTOPBOTTOMMARGIN),
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Journal, null, null),
                null);

            CenterContentProperty = DependencyProperty.Register(
                "CenterContent", typeof(bool), typeof(ZoomBoxPanel),
                new FrameworkPropertyMetadata(true,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Journal, null, null),
                null);

            MinZoomProperty = DependencyProperty.Register(
                "MinZoom", typeof(double), typeof(ZoomBoxPanel),
                new FrameworkPropertyMetadata(1.0,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Journal, PropertyChanged_Zoom, CoerceMinZoom),
                new ValidateValueCallback(ZoomBoxPanel.ValidateIsPositiveNonZero));
            MaxZoomProperty = DependencyProperty.Register(
                "MaxZoom", typeof(double), typeof(ZoomBoxPanel),
                new FrameworkPropertyMetadata(1000.0,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Journal, PropertyChanged_Zoom, CoerceMaxZoom),
                new ValidateValueCallback(ZoomBoxPanel.ValidateIsPositiveNonZero));
            ZoomProperty = DependencyProperty.Register(
                "Zoom", typeof(double), typeof(ZoomBoxPanel),
                new FrameworkPropertyMetadata(100.0,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Journal, PropertyChanged_Zoom, CoerceZoom),
                new ValidateValueCallback(ZoomBoxPanel.ValidateIsPositiveNonZero));

            CanIncreaseZoomPropertyKey = DependencyProperty.RegisterReadOnly(
                "CanIncreaseZoom", typeof(bool), typeof(ZoomBoxPanel),
                 new FrameworkPropertyMetadata(false, null, null));

            CanDecreaseZoomPropertyKey = DependencyProperty.RegisterReadOnly(
                "CanDecreaseZoom", typeof(bool), typeof(ZoomBoxPanel),
                 new FrameworkPropertyMetadata(false, null, null));

            CanRotateKey = DependencyProperty.RegisterReadOnly(
                "CanRotate", typeof(bool), typeof(ZoomBoxPanel),
                 new FrameworkPropertyMetadata(true, null, null));

            ZoomIncrementProperty = DependencyProperty.Register(
                "ZoomIncrement", typeof(double), typeof(ZoomBoxPanel),
                new FrameworkPropertyMetadata(20.0),
                new ValidateValueCallback(ZoomBoxPanel.ValidateIsPositiveNonZero));

            RotateIncrementProperty = DependencyProperty.Register(
                "RotateIncrement", typeof(double), typeof(ZoomBoxPanel),
                new FrameworkPropertyMetadata(15.0, null, CoerceRotateIncrement),
                new ValidateValueCallback(ZoomBoxPanel.ValidateIsPositiveNonZero));

            ZoomModeProperty = DependencyProperty.Register(
                "ZoomMode", typeof(ZoomBoxPanel.ZoomModes), typeof(ZoomBoxPanel),
                new FrameworkPropertyMetadata(ZoomBoxPanel.ZoomModes.FitPage,
                 FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Journal,
                 null, null),
                null);

            //-----------------------------------------------------------------
            // Commands
            CommandManager.RegisterClassCommandBinding(typeof(ZoomBoxPanel),
                new CommandBinding(NavigationCommands.IncreaseZoom,
                    ExecutedEventHandler_IncreaseZoom, CanExecuteEventHandler_IfCanIncreaseZoom));

            CommandManager.RegisterClassCommandBinding(typeof(ZoomBoxPanel),
                new CommandBinding(NavigationCommands.DecreaseZoom,
                    ExecutedEventHandler_DecreaseZoom, CanExecuteEventHandler_IfCanDecreaseZoom));

            InputGestureCollection rotateInputGestures = new InputGestureCollection();
            rotateInputGestures.Add(new KeyGesture(Key.R, ModifierKeys.Control));
            RotateClockwise = new RoutedUICommand("Rotate Clockwise", "RotateClockwise", typeof(ZoomBoxPanel), rotateInputGestures);
            CommandManager.RegisterClassCommandBinding(typeof(ZoomBoxPanel),
                new CommandBinding(RotateClockwise,
                    ExecutedEventHandler_RotateClockwise, CanExecuteEventHandler_IfHasContent));

            InputGestureCollection rotateCounterInputGestures = new InputGestureCollection();
            rotateCounterInputGestures.Add(new KeyGesture(Key.R, ModifierKeys.Alt));
            RotateCounterclockwise = new RoutedUICommand("Rotate Counterclockwise", "RotateCounterclockwise", typeof(ZoomBoxPanel), rotateCounterInputGestures);
            CommandManager.RegisterClassCommandBinding(typeof(ZoomBoxPanel),
                new CommandBinding(RotateCounterclockwise,
                    ExecutedEventHandler_RotateCounterclockwise, CanExecuteEventHandler_IfHasContent));

            InputGestureCollection rotateHomeInputGestures = new InputGestureCollection();
            rotateHomeInputGestures.Add(new KeyGesture(Key.Home, ModifierKeys.None));
            RotateHome = new RoutedUICommand("Rotate Home", "RotateHome", typeof(ZoomBoxPanel), rotateHomeInputGestures);
            CommandManager.RegisterClassCommandBinding(typeof(ZoomBoxPanel),
                new CommandBinding(RotateHome,
                    ExecutedEventHandler_RotateHome, CanExecuteEventHandler_IfHasContent));

            InputGestureCollection rotateReverseInputGestures = new InputGestureCollection();
            rotateReverseInputGestures.Add(new KeyGesture(Key.End, ModifierKeys.None));
            RotateReverse = new RoutedUICommand("Rotate Reverse", "RotateReverse", typeof(ZoomBoxPanel), rotateReverseInputGestures);
            CommandManager.RegisterClassCommandBinding(typeof(ZoomBoxPanel),
                new CommandBinding(RotateReverse,
                    ExecutedEventHandler_RotateReverse, CanExecuteEventHandler_IfHasContent));

        }

        private static bool ValidateIsPositiveNonZero(object value)
        {
            double v = (double)value;
            return (v > 0.0) ? true : false;
        }
        private static object CoerceRotateIncrement(DependencyObject d, object value)
        {
            double dv = (double)value;
            if (dv <= 0) dv = 1;
            else if (dv > 359) dv = 359;
            return dv;
        }
        private static object CoerceZoom(DependencyObject d, object value)
        {
            double dv = (double)value;
            ZoomBoxPanel z = d as ZoomBoxPanel;
            if (z != null)
            {
                if (dv > z.MaxZoom)
                    dv = z.MaxZoom;
                else if (dv < z.MinZoom)
                    dv = z.MinZoom;
            }
            return dv;
        }
        private static object CoerceMinZoom(DependencyObject d, object value)
        {
            double dv = (double)value;
            ZoomBoxPanel z = d as ZoomBoxPanel;
            if (z != null)
            {
                if (dv <= MINZOOM)
                    dv = MINZOOM;
                if (dv > z.MaxZoom)
                    z.MaxZoom = dv;
                if (z.Zoom < dv)
                    z.Zoom = dv; ;
            }
            return dv;
        }
        private static object CoerceMaxZoom(DependencyObject d, object value)
        {
            double dv = (double)value;
            ZoomBoxPanel z = d as ZoomBoxPanel;
            if (z != null)
            {
                if (dv < z.MinZoom)
                    z.MinZoom = dv;
                if (z.Zoom > dv)
                    z.Zoom = dv; ;
            }
            return dv;
        }

        public ZoomBoxPanel()
        {
            translateTransform = new TranslateTransform();
            rotateTransform = new RotateTransform();
            zoomTransform = new ScaleTransform();
            transformGroup = new TransformGroup();
            transformGroup.Children.Add(this.rotateTransform);
            transformGroup.Children.Add(this.zoomTransform);
            transformGroup.Children.Add(this.translateTransform);
            panX = 0;
            panY = 0;
            rotateAngle = 0;
            rotateCenterX = 0;
            rotateCenterY = 0;
            childSize = new Size(1, 1);
            ClipToBounds = true;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            ApplyZoom(false);

            foreach (UIElement element in base.InternalChildren)
            {
                element.RenderTransform = this.transformGroup;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Focusable = true;
        }
        #endregion


        #region Layout methods
        protected override Size MeasureOverride(Size constraint)
        {
            Size panelSize = new Size(150, 150);
            if ((!Double.IsNaN(constraint.Width)) && (!Double.IsInfinity(constraint.Width)))
                panelSize.Width = constraint.Width;
            if ((!Double.IsNaN(constraint.Height)) && (!Double.IsInfinity(constraint.Height)))
                panelSize.Height = constraint.Height;

            childSize = new Size(1, 1);
            Size infSize = new Size(double.PositiveInfinity, double.PositiveInfinity);

            foreach (UIElement element in base.InternalChildren)
            {
                element.Measure(infSize);

                if (element.DesiredSize.Width > childSize.Width)
                    childSize.Width = element.DesiredSize.Width;

                if (element.DesiredSize.Height > childSize.Height)
                    childSize.Height = element.DesiredSize.Height;
            }

            return panelSize;
        }

        protected override Size ArrangeOverride(Size panelRect)
        {
            foreach (UIElement element in base.InternalChildren)
            {
                element.Arrange(new Rect(0, 0, element.DesiredSize.Width, element.DesiredSize.Height));
            }

            RecalcPage(panelRect);

            return panelRect;
        }

        // called on panel resize, zoom mode change
        protected void RecalcPage(Size panelRect)
        {
            double desiredW = 0, desiredH = 0;
            double zoomX = 0, zoomY = 0;

            double minDimension = 5;

            switch (ZoomMode)
            {
                case ZoomModes.CustomSize:
                    break;
                case ZoomModes.ActualSize:
                    ZoomFactor = 1.0;
                    panX = CalcCenterOffset(panelRect.Width, childSize.Width, Padding.Left);
                    panY = CalcCenterOffset(panelRect.Height, childSize.Height, Padding.Top);
                    ApplyZoom(false);
                    break;

                case ZoomModes.FitWidth:
                    desiredW = panelRect.Width - Padding.Left - Padding.Right;
                    if (desiredW < minDimension) desiredW = minDimension;
                    zoomX = desiredW / childSize.Width;

                    ZoomFactor = zoomX;
                    panX = Padding.Left;
                    panY = CalcCenterOffset(panelRect.Height, childSize.Height, Padding.Top);

                    ApplyZoom(false);
                    break;
                case ZoomModes.FitHeight:
                    desiredH = panelRect.Height - Padding.Top - Padding.Bottom;
                    if (desiredH < minDimension) desiredH = minDimension;
                    zoomY = desiredH / childSize.Height;

                    ZoomFactor = zoomY;
                    panX = CalcCenterOffset(panelRect.Width, childSize.Width, Padding.Left);
                    panY = Padding.Top;

                    ApplyZoom(false);
                    break;
                case ZoomModes.FitPage:
                    desiredW = panelRect.Width - Padding.Left - Padding.Right;
                    if (desiredW < minDimension) desiredW = minDimension;
                    zoomX = desiredW / childSize.Width;
                    desiredH = panelRect.Height - Padding.Top - Padding.Bottom;
                    if (desiredH < minDimension) desiredH = minDimension;
                    zoomY = desiredH / childSize.Height;

                    if (zoomX <= zoomY)
                    {
                        ZoomFactor = zoomX;
                        panX = Padding.Left;
                        panY = CalcCenterOffset(panelRect.Height, childSize.Height, Padding.Top);
                    }
                    else
                    {
                        ZoomFactor = zoomY;
                        panX = CalcCenterOffset(panelRect.Width, childSize.Width, Padding.Left);
                        panY = Padding.Top;
                    }
                    ApplyZoom(false);
                    break;
                case ZoomModes.FitVisible:
                    desiredW = panelRect.Width - Padding.Left - Padding.Right;
                    if (desiredW < minDimension) desiredW = minDimension;
                    zoomX = desiredW / childSize.Width;
                    desiredH = panelRect.Height - Padding.Top - Padding.Bottom;
                    if (desiredH < minDimension) desiredH = minDimension;
                    zoomY = desiredH / childSize.Height;

                    if (zoomX >= zoomY)
                    {
                        ZoomFactor = zoomX;
                        panX = Padding.Left;
                        panY = CalcCenterOffset(panelRect.Height, childSize.Height, Padding.Top);
                    }
                    else
                    {
                        ZoomFactor = zoomY;
                        panX = CalcCenterOffset(panelRect.Width, childSize.Width, Padding.Left);
                        panY = Padding.Top;
                    }
                    ApplyZoom(false);
                    break;
            }
        }

        protected double CalcCenterOffset(double parent, double child, double margin)
        {
            if (CenterContent)
            {
                double offset = 0;
                offset = (parent - (child * ZoomFactor)) / 2;
                if (offset > margin)
                    return offset;
            }
            return margin;
        }

        protected void ApplyZoomCommand(double delta, int factor, Point panelPoint)
        {
            if (factor > 0)
            {
                double zoom = ZoomFactor;
                for (int i = 1; i <= factor; i++)
                    zoom = zoom * delta;
                ZoomFactor = zoom;
                ApplyZoomCommand(panelPoint);
            }
        }

        protected void ApplyZoomCommand(Point panelPoint)
        {
            Point logicalPoint = transformGroup.Inverse.Transform(panelPoint);
            ZoomMode = ZoomModes.CustomSize;
            panX = -1 * (logicalPoint.X * ZoomFactor - panelPoint.X);
            panY = -1 * (logicalPoint.Y * ZoomFactor - panelPoint.Y);
            ApplyZoom(true);
        }

        protected void ApplyRotateCommand(double delta, int factor, Point panelPoint)
        {
            if (factor > 0)
            {
                rotateAngle = (rotateAngle + (delta * factor)) % 360;
                if (rotateAngle < 0)
                    rotateAngle += 360;

                ZoomMode = ZoomModes.CustomSize;

                rotateCenterX = childSize.Width / 2;
                rotateCenterY = childSize.Height / 2;

                ApplyZoom(true);
            }
        }

        protected void ApplyZoom(bool animate)
        {
            rotateTransform.Angle = rotateAngle;
            rotateTransform.CenterX = rotateCenterX;
            rotateTransform.CenterY = rotateCenterY;
            translateTransform.X = panX;
            translateTransform.Y = panY;
            zoomTransform.ScaleX = ZoomFactor;
            zoomTransform.ScaleY = ZoomFactor;
        }


        #endregion

        #region Event handlers
        private void process_PropertyChanged_Zoom(DependencyPropertyChangedEventArgs e)
        {
            bool canIncrease = (Zoom < MaxZoom);
            bool canDecrease = (Zoom > MinZoom);

            if (canIncrease != CanIncreaseZoom)
                this.SetValue(ZoomBoxPanel.CanIncreaseZoomPropertyKey, canIncrease);
            if (canDecrease != CanDecreaseZoom)
                this.SetValue(ZoomBoxPanel.CanDecreaseZoomPropertyKey, canDecrease);
        }

        private void process_RotateCommand(bool clockWise, double? angle)
        {
            Point panelPoint = new Point(ActualWidth / 2, ActualHeight / 2);
            double delta = RotateIncrement;
            if (angle != null)
                delta = (double)angle;
            if (!clockWise)
                delta *= -1;
            ApplyRotateCommand(delta, 1, panelPoint);
        }

        private void process_RotateHomeReverse(bool isHome)
        {
            Point panelPoint = new Point(ActualWidth / 2, ActualHeight / 2);
            rotateAngle = isHome ? 0 : 180;
            rotateCenterX = childSize.Width / 2;
            rotateCenterY = childSize.Height / 2;
            ApplyZoom(true);
        }

        private void process_ZoomCommand(bool increase)
        {
            Point panelPoint = new Point(ActualWidth / 2, ActualHeight / 2);

            double delta = (1 + (ZoomIncrement / 100));
            if (!increase)
                delta = 1 / delta;

            ApplyZoomCommand(delta, 1, panelPoint);
        }

        #endregion
    }


}
