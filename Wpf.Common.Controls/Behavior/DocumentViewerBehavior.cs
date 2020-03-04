using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using Wpf.Common.Input;

namespace Wpf.Common.Controls.Behaviors
{
    /// <summary>
    /// DocumentViewerBehavior
    /// </summary>
    public class DocumentViewerBehavior
    {
        public const string PART_PreviousPageButton_Name = "PART_PreviousPageButton";
        public const string PART_NextPageButton_Name = "PART_NextPageButton";
        public const string PART_PageNumberNumericUpDown_Name = "PART_PageNumberNumericUpDown";
        public const string PART_ZoominButton_Name = "PART_ZoominButton";
        public const string PART_ZoomoutButton_Name = "PART_ZoomoutButton";
        public const string PART_ZoomComboBox_Name = "PART_ZoomComboBox";
     
        public static readonly DependencyProperty IsEnableProperty = DependencyProperty.RegisterAttached("IsEnable", typeof(bool), typeof(DocumentViewerBehavior)
           , new PropertyMetadata(false, OnIsEnablePropertyChanged));

        public static bool GetIsEnable(DependencyObject obj) => obj.GetValue<bool>(IsEnableProperty);

        public static void SetIsEnable(DependencyObject obj, object value) => obj.SetValue(IsEnableProperty, value);
       
        private static void OnIsEnablePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var dv = sender as DocumentViewer;
            if (dv == null)
                return;
            if (!(e.NewValue is bool)) return;
            if (!(bool)e.NewValue) return;
            dv.Loaded -= OnDocumentViewerLoaded;
            dv.Loaded += OnDocumentViewerLoaded;
        }

        private static void OnDocumentViewerLoaded(object sender, RoutedEventArgs e)
        {
            var dv = sender as DocumentViewer;
            var pageNumUpDown = dv.Template.FindName(PART_PageNumberNumericUpDown_Name,dv) as NumericUpDown;
            
            //前移
            var previousPagebutton = dv.Template.FindName(PART_PreviousPageButton_Name, dv) as Button;
            if (previousPagebutton == null) return;
            previousPagebutton.Command = new RelayCommand(() =>
              {
                  dv.PreviousPage();  
              },()=>dv.CanGoToPreviousPage);

            //后移
            var nextPagebutton = dv.Template.FindName(PART_NextPageButton_Name, dv) as Button;
            if (nextPagebutton != null)
                nextPagebutton.Command = new RelayCommand(() =>
                {
                    dv.NextPage();
                },()=>dv.CanGoToNextPage);



            var pnNumUpDown = dv.Template.FindName(PART_PageNumberNumericUpDown_Name, dv) as NumericUpDown;
            RoutedEventHandler pageChange = (s, arg) =>
            {
                if (pnNumUpDown.Value == dv.MasterPageNumber) return;
                pnNumUpDown.Value = dv.MasterPageNumber;
            };
            pnNumUpDown.ValueChanged += pageChange;
            pnNumUpDown.ValueChanged += pageChange;
            KeyEventHandler pageNumInput = (s, arg) =>
            {
                if (arg.Key == Key.Return && pnNumUpDown.Tag!=null)
                    dv.GoToPage((int)pnNumUpDown.Tag);
            };
            Action<string> pageNumTxtChange = s => pnNumUpDown.Tag = string.IsNullOrEmpty(s) ? 1 : int.Parse(s);
            pnNumUpDown.TextChanged -= pageNumTxtChange;
            pnNumUpDown.TextChanged += pageNumTxtChange;
            pnNumUpDown.PreviewKeyDown -= pageNumInput;
            pnNumUpDown.PreviewKeyDown += pageNumInput;

            var zoomComboBox =dv.Template.FindName(PART_ZoomComboBox_Name, dv) as ComboBox;
  
            var zoomTB= zoomComboBox.Template.FindName("PART_EditableTextBox", zoomComboBox) as TextBox;
            Binding zoomBinding = new Binding("Zoom");
       
            zoomBinding.StringFormat = "{0}%";
            zoomBinding.Source = dv;
            zoomBinding.Mode = BindingMode.OneWay;
          
            zoomBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            zoomTB.SetBinding(TextBox.TextProperty, zoomBinding);
            KeyEventHandler zoomChange = (s, arg) =>
            {
                if (arg.Key == Key.Return)
                {
                    double x;
                    if (double.TryParse((s as TextBox).Text?.TrimEnd('%'), out x))
                        dv.Zoom =Math.Round( x,0);
                }                         
            };

         
            zoomTB.PreviewKeyDown -= zoomChange;
            zoomTB.PreviewKeyDown += zoomChange;
          
           
            SelectionChangedEventHandler selectionChangedEventHandler = (s, arg) =>
            {
                double x;
                if (double.TryParse(zoomComboBox.SelectedItem?.ToString().TrimEnd('%'), out x))
                    dv.Zoom = x;
            };
            zoomComboBox.SelectionChanged -= selectionChangedEventHandler;
            zoomComboBox.SelectionChanged += selectionChangedEventHandler;

            var zoomInButton = dv.Template.FindName(PART_ZoominButton_Name,dv) as Button;
            zoomInButton.Command = new RelayCommand(() =>
             {
                 dv.IncreaseZoom();
             },()=>dv.CanIncreaseZoom);

            var zoomOutButton = dv.Template.FindName(PART_ZoomoutButton_Name, dv) as Button;
            zoomOutButton.Command = new RelayCommand(() =>
            {
                dv.DecreaseZoom();
            },()=>dv.CanDecreaseZoom);
        }
     
    }
}
