/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：QiCheng.QCTrader.Controls
*   文件名称 ：NumericUpDown.cs
*   =================================
*   创 建 者 ：wei.feng
*   创建日期 ：2019/10/20 17:16:14 
*   功能描述 ：
*   使用说明 ：
*   =================================
*   修 改 者 ：
*   修改日期 ：
*   修改内容 ：
*   =================================
*  
***************************************************************************/
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Wpf.Common.Controls
{
    [TemplatePart(Name = "PART_DatePicker", Type = typeof(DatePicker))]
    [TemplatePart(Name = "PART_Hours_TB", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_Minutes_TB", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_Seconds_TB", Type = typeof(TextBox))]
    public class DateTimePicker : Control
    {
        private NumericUpDown HourNumericDown { get; set; }
        private NumericUpDown SecondsNumericDown { get; set; }
        private NumericUpDown MinuteNumericDown { get; set; }

        private TextBox HourTextBox { get; set; }
        private TextBox SecondsTextBox { get; set; }
        private TextBox MinuteTextBox { get; set; }

        private DatePicker DatePicker { get; set; }

        public DateTimePicker()
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            DatePicker = GetTemplateChild("PART_DatePicker") as DatePicker;
            if (DatePicker == null) return;
            DatePicker.SelectedDateChanged -= DatePicker_SelectedDateChanged;
            DatePicker.SelectedDateChanged += DatePicker_SelectedDateChanged;
             
            this.HourTextBox = DatePicker.FindChildrenFromTemplate<TextBox>("PART_Hours_TB");
            if (HourTextBox == null) return;
            this.HourTextBox.GotMouseCapture -= OnTextBox_GotMouseCapture;
            this.HourTextBox.GotMouseCapture += OnTextBox_GotMouseCapture;
          
            this.HourTextBox.LostKeyboardFocus -= HourTextBox_LostKeyboardFocus;
            this.HourTextBox.LostKeyboardFocus += HourTextBox_LostKeyboardFocus;
            this.HourTextBox.PreviewKeyDown -= OnHourTextBox_PreviewKeyDown;
            this.HourTextBox.PreviewKeyDown += OnHourTextBox_PreviewKeyDown;
            this.HourTextBox.KeyUp -= HourTextBox_KeyUp;
            this.HourTextBox.KeyUp += HourTextBox_KeyUp;

            this.MinuteTextBox = DatePicker.FindChildrenFromTemplate<TextBox>("PART_Minutes_TB");
            if (MinuteTextBox == null) return;
            this.MinuteTextBox.GotMouseCapture -= OnTextBox_GotMouseCapture;
            this.MinuteTextBox.GotMouseCapture += OnTextBox_GotMouseCapture;
           
            this.MinuteTextBox.PreviewKeyDown -= OnSecondOrMinuteTextBox_PreviewKeyDown;
            this.MinuteTextBox.PreviewKeyDown += OnSecondOrMinuteTextBox_PreviewKeyDown; 
            this.MinuteTextBox.KeyUp -= MinuteTextBox_KeyUp;
            this.MinuteTextBox.KeyUp += MinuteTextBox_KeyUp;
            this.MinuteTextBox.LostKeyboardFocus -= MinuteTextBox_LostKeyboardFocus;
            this.MinuteTextBox.LostKeyboardFocus += MinuteTextBox_LostKeyboardFocus;


            this.SecondsTextBox = DatePicker.FindChildrenFromTemplate<TextBox>("PART_Seconds_TB");
            if (this.SecondsTextBox == null) return;
            this.SecondsTextBox.GotMouseCapture -= OnTextBox_GotMouseCapture;
            this.SecondsTextBox.GotMouseCapture += OnTextBox_GotMouseCapture;
            
            this.SecondsTextBox.PreviewKeyDown -= OnSecondOrMinuteTextBox_PreviewKeyDown;
            this.SecondsTextBox.PreviewKeyDown += OnSecondOrMinuteTextBox_PreviewKeyDown; 
            this.SecondsTextBox.KeyUp -= SecondsTextBox_KeyUp;
            this.SecondsTextBox.KeyUp += SecondsTextBox_KeyUp;
            this.SecondsTextBox.LostKeyboardFocus -= SecondsTextBox_LostKeyboardFocus;
            this.SecondsTextBox.LostKeyboardFocus += SecondsTextBox_LostKeyboardFocus;

            var popup = DatePicker.FindChildrenFromTemplate<Popup>("PART_Popup");
            if (popup == null) return;
            popup.Opened -= OnDatePickerPopupOpened;
            popup.Opened += OnDatePickerPopupOpened;
            popup.Closed -= OnDatePickerPopupClosed;
            popup.Closed += OnDatePickerPopupClosed;
            var calendar = popup.Child as Calendar;
            var calendarItem = calendar.FindChildrenFromTemplate<CalendarItem>("PART_CalendarItem");
            if (calendarItem == null) return;
           
            HourNumericDown = calendarItem.FindChildrenFromTemplate<NumericUpDown>("PART_Hours_NumericUpDown");
            if (HourNumericDown == null) return;
            HourNumericDown.ValueChanged -= NumericDown_ValueChanged;
            HourNumericDown.ValueChanged += NumericDown_ValueChanged;
            MinuteNumericDown = calendarItem.FindChildrenFromTemplate<NumericUpDown>("PART_Minutes_NumericUpDown");
            if (MinuteNumericDown == null) return;
            MinuteNumericDown.ValueChanged -= NumericDown_ValueChanged;
            MinuteNumericDown.ValueChanged += NumericDown_ValueChanged;
            SecondsNumericDown = calendarItem.FindChildrenFromTemplate<NumericUpDown>("PART_Seconds_NumericUpDown");
            if (SecondsNumericDown == null) return;
            SecondsNumericDown.ValueChanged -= NumericDown_ValueChanged;
            SecondsNumericDown.ValueChanged += NumericDown_ValueChanged;

        }

        private void SecondsTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            int value = 0;
            var tb = sender as TextBox;
            if (!int.TryParse(tb.Text, out value) || value > 59)
                tb.Text = this.Value.Second.ToString("00");
            else
                this.Value = this.Value.AddSeconds(value - this.Value.Second);
        }

        private void MinuteTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            int value = 0;
            var tb = sender as TextBox;
            if (!int.TryParse(tb.Text, out value) || value > 59)
                tb.Text = this.Value.Minute.ToString("00");
            else
                this.Value = this.Value.AddMinutes(value - this.Value.Minute);
        }

        private void HourTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            int value = 0;
            var tb = sender as TextBox;
            if (!int.TryParse(tb.Text, out value) || value > 23)
                tb.Text = this.Value.Hour.ToString("00");
            else
                this.Value = this.Value.AddHours(value - this.Value.Hour);
        }

        private void OnTextBox_GotMouseCapture(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

      

        private bool _isPopupTimeChanged;

        private void NumericDown_ValueChanged(object sender, RoutedEventArgs e) => this._isPopupTimeChanged = true;
       

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var dp = (sender as DatePicker).SelectedDate.Value;
            this.Value = new DateTime(dp.Year, dp.Month, dp.Day, this.Value.Hour, this.Value.Minute, this.Value.Second);
        }

        private void MinuteTextBox_KeyUp(object sender, KeyEventArgs e)
        {                 
            SelectTodayIfNull();          
        }

        private void SelectTodayIfNull()
        {
            if (this.DatePicker.SelectedDate == null || this.DatePicker.SelectedDate == DateTime.MinValue)
                this.DatePicker.SelectedDate = DateTime.Now;
        }

        private void SecondsTextBox_KeyUp(object sender, KeyEventArgs e)
        {    
            SelectTodayIfNull();           
        }

        private void HourTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            SelectTodayIfNull();     
        }

        private void OnSecondOrMinuteTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = !ValidPreviewKeyDown(e);
            var textBox = sender as TextBox;
            int value = 0;
            if (e.Key == Key.Down)
            {
                if (int.TryParse(textBox.Text, out value))
                    if (value > 0)
                    {
                        textBox.Text = (value - 1).ToString("00");
                        textBox.CaretIndex = textBox.Text.Length;
                    }
            }
            else if (e.Key == Key.Up)
            {
                if (int.TryParse(textBox.Text, out value))
                    if (value < 59)
                    {
                        textBox.Text = (value + 1).ToString("00");
                        textBox.CaretIndex = textBox.Text.Length;
                    }
            }
        }

        private bool ValidPreviewKeyDown(KeyEventArgs e)
        {
            var valid = e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Back || (e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9);
            return valid;
        }

        private void OnHourTextBox_PreviewKeyDown(object sender,KeyEventArgs e)
        {
            e.Handled = !ValidPreviewKeyDown(e);
            int hour = 0;
            if (e.Key == Key.Down)
            {
                if (int.TryParse(HourTextBox.Text, out hour))
                    if (hour > 0)
                    {
                        HourTextBox.Text = (hour - 1).ToString("00");
                        HourTextBox.CaretIndex = HourTextBox.Text.Length;
                    }
            }
            else if (e.Key == Key.Up)
            {
                if (int.TryParse(HourTextBox.Text, out hour))
                    if (hour < 23)
                    {
                        HourTextBox.Text = (hour + 1).ToString("00");
                        HourTextBox.CaretIndex = HourTextBox.Text.Length;
                    }
            }
        }

        private void OnDatePickerPopupClosed(object sender, EventArgs e)
        {
            if (this.DatePicker == null) return;
            if (this.DatePicker.SelectedDate == null)
            {
                if (this._isPopupTimeChanged)
                    this.SelectTodayIfNull();
                else
                    return;

            }
            this.Value = new DateTime
               (this.Value.Year,
                this.Value.Month,
                this.Value.Day,
                (int)this.HourNumericDown.Value,
                (int)this.MinuteNumericDown.Value,
                (int)this.SecondsNumericDown.Value);
        }

        private void OnDatePickerPopupOpened(object sender, EventArgs e)
        {            
            this.HourNumericDown.Value = this.Value.Hour;
            this.MinuteNumericDown.Value = this.Value.Minute;
            this.SecondsNumericDown.Value = this.Value.Second;
            this._isPopupTimeChanged = false;
        }

        public DateTime Value
        {
            get { return (DateTime)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(DateTime), typeof(DateTimePicker), new FrameworkPropertyMetadata(default(DateTime), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChangedCallback));

        private static void OnValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is DateTime))
                return;
            var value = (DateTime)e.NewValue;
            var dtp = d as DateTimePicker;
            if (dtp.DatePicker != null)
                dtp.DatePicker.SelectedDate = value;
            if (dtp.HourTextBox != null)
                dtp.HourTextBox.Text = value.Hour.ToString("00");
            if (dtp.MinuteTextBox != null)
                dtp.MinuteTextBox.Text = value.Minute.ToString("00");
            if (dtp.SecondsTextBox != null)
                dtp.SecondsTextBox.Text = value.Second.ToString("00");
        }
    }
}
