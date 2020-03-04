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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Wpf.Common.Controls
{
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_ButtonUp", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_ButtonDown", Type = typeof(ButtonBase))]
    public class NumericUpDown : Control
    {
        private TextBox PART_TextBox = new TextBox();

        public static readonly DependencyProperty CaretBrushProperty = DependencyProperty.Register("CaretBrush",typeof(Brush),typeof(NumericUpDown));

        public Brush CaretBrush
        {
            get => this.GetValue<Brush>(CaretBrushProperty);
            set => this.SetValue(CaretBrushProperty, value);
        }

        public static readonly DependencyProperty ButtonVisibilityProperty = DependencyProperty.Register("ButtonVisibility", typeof(Visibility), typeof(NumericUpDown),new PropertyMetadata(Visibility.Visible));

        public Visibility ButtonVisibility
        {
            get => this.GetValue<Visibility>(ButtonVisibilityProperty);
            set => this.SetValue(ButtonVisibilityProperty, value);
        }


        public NumericUpDown() { }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_TextBox") is TextBox textBox)
            {
                PART_TextBox = textBox;
                PART_TextBox.PreviewKeyDown -= TextBox_PreviewKeyDown;
                PART_TextBox.PreviewKeyDown += TextBox_PreviewKeyDown;

                PART_TextBox.TextChanged -= TextBox_TextChanged;
                PART_TextBox.TextChanged += TextBox_TextChanged;

                PART_TextBox.LostKeyboardFocus -= PART_TextBox_LostKeyboardFocus;
                PART_TextBox.LostKeyboardFocus += PART_TextBox_LostKeyboardFocus;
              
                PART_TextBox.Text = Value.ToString();
            }
            if (GetTemplateChild("PART_ButtonUp") is ButtonBase PART_ButtonUp)
            {
                PART_ButtonUp.Click -= ButtonUp_Click;
                PART_ButtonUp.Click += ButtonUp_Click;
            }
            if (GetTemplateChild("PART_ButtonDown") is ButtonBase PART_ButtonDown)
            {
                PART_ButtonDown.Click -= ButtonDown_Click;
                PART_ButtonDown.Click += ButtonDown_Click;
            }
            this.GotKeyboardFocus += NumericUpDown_GotKeyboardFocus;
        }

        private void NumericUpDown_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            this.PART_TextBox.Focus();
        }

        private void PART_TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (this.PART_TextBox.Text == "-" || string.IsNullOrEmpty(this.PART_TextBox.Text))
            {
                this.Value = this.MinValue;
                this.PART_TextBox.Text =this.GetDecimalString( this.MinValue.ToString());
                return;
            }

            if (decimal.TryParse(PART_TextBox.Text, out decimal d))
            {
                var currValue = Math.Round(d, this.DecimalDigit);
                if (currValue == this.Value  ) //处理小数点后无数据时进行补零
                {
                    if (this.DecimalDigit > 0)
                    {
                        if (this.PART_TextBox.Text != null && (this.PART_TextBox.Text.Last() == '.' || currValue==0))
                            this.PART_TextBox.Text = this.GetDecimalStringWithDigit();
                    }
                    else if (currValue == 0)
                        this.PART_TextBox.Text = "0";
                }
                else
                    this.Value = Math.Round(d, this.DecimalDigit);
            }
            else
                this.Value = 0;

            if (this.Value > MaxValue)
                this.Value = MaxValue;
            else if (this.Value < MinValue)
                this.Value = MinValue;
        }

        #region Other

        private static bool ValidateValueCallback(object value)
        {
            if (value == null)
                return false;
            if (value is decimal val
                && val >= decimal.MinValue
                && val <= decimal.MaxValue)
                return true;
            return false;
        }

        private decimal _tmpValue;

        private static object CoerceValueCallback(DependencyObject d, object value)
        {
            if (value == null)
                return value;
            var c = (NumericUpDown)d;
            if (c == null)
                return value;
            if (value is decimal v)
            {
                if (v < c.MinValue)
                    return c.MinValue;
                if (v > c.MaxValue)
                    return c.MaxValue;
            }
            return value;
        }

        static NumericUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(NumericUpDown)));
        }

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Direct,
            typeof(RoutedEventHandler), typeof(NumericUpDown));

        public event RoutedEventHandler ValueChanged
        {
            add
            {
                this.AddHandler(ValueChangedEvent, value);
            }
            remove
            {
                this.RemoveHandler(ValueChangedEvent, value);
            }
        }

        public event Action<string> TextChanged;

        public static readonly RoutedEvent IncreasingEvent = EventManager.RegisterRoutedEvent("Increasing", RoutingStrategy.Direct,
           typeof(RoutedEventHandler), typeof(NumericUpDown));

        public event RoutedEventHandler Increasing
        {
            add
            {
                this.AddHandler(IncreasingEvent, value);
            }
            remove
            {
                this.RemoveHandler(IncreasingEvent, value);
            }
        }

        public static readonly RoutedEvent DecreasingEvent = EventManager.RegisterRoutedEvent("Decreasing", RoutingStrategy.Direct,
           typeof(RoutedEventHandler), typeof(NumericUpDown));

        public event RoutedEventHandler Decreasing
        {
            add
            {
                this.AddHandler(DecreasingEvent, value);
            }
            remove
            {
                this.RemoveHandler(DecreasingEvent, value);
            }
        }

        public decimal MaxValue
        {
            get { return (decimal)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(decimal), typeof(NumericUpDown), new FrameworkPropertyMetadata(decimal.MaxValue, MaxValueChangedCallback, CoerceMaxValueCallback));

        private static object CoerceMaxValueCallback(DependencyObject d, object value)
        {
            if (value == null)
                return 0;

            var c = (NumericUpDown)d;
            if (c == null)
                return value;

            if (value is decimal v)
            {
                if (v < c.MinValue)
                    return c.MinValue;
            }

            return value;
        }

        private static void MaxValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown numericUpDown = ((NumericUpDown)d);
            numericUpDown.CoerceValue(MinValueProperty);
            numericUpDown.CoerceValue(ValueProperty);
        }

        public decimal MinValue
        {
            get { return (decimal)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(decimal), typeof(NumericUpDown), new FrameworkPropertyMetadata(0m, MinValueChangedCallback, CoerceMinValueCallback));

        private static object CoerceMinValueCallback(DependencyObject d, object value)
        {
            if (value == null)
                return value;
            var c = (NumericUpDown)d;
            if (c == null)
                return value;
            if (value is decimal v)
            {
                if (v > c.MaxValue)
                    return c.MaxValue;
            }

            return value;
        }

        private static void MinValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown numericUpDown = ((NumericUpDown)d);
            numericUpDown.CoerceValue(NumericUpDown.MaxValueProperty);
            numericUpDown.CoerceValue(NumericUpDown.ValueProperty);
        }

        public decimal Increment
        {
            get { return (decimal)GetValue(IncrementProperty); }
            set { SetValue(IncrementProperty, value); }
        }

        public static readonly DependencyProperty IncrementProperty =
            DependencyProperty.Register("Increment", typeof(decimal), typeof(NumericUpDown), new FrameworkPropertyMetadata(1m, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, CoerceIncrementCallback));

        private static object CoerceIncrementCallback(DependencyObject d, object value)
        {
            if (value == null)
                return value;

            var c = (NumericUpDown)d;
            if (c == null)
                return value;

            if (value is decimal v)
            {
                if (v < c.MinValue)
                    return 1;
                if (v > c.MaxValue)
                    return 1;
            }

            return value;
        }

        public decimal Value
        {
            get { return (decimal)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(decimal), typeof(NumericUpDown), new FrameworkPropertyMetadata(0m, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChangedCallback, CoerceValueCallback), ValidateValueCallback);

        #endregion

        private static void OnValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown numericUpDown = (NumericUpDown)d;
            if (numericUpDown == null)
                return;

            if (e.OldValue != e.NewValue)
            {
                if (!string.IsNullOrEmpty(numericUpDown.PART_TextBox.Text))
                {
                    numericUpDown.PART_TextBox.Text = numericUpDown.DecimalDigit == 0
                          ? numericUpDown.GetDecimalString(e.NewValue)
                          : numericUpDown.GetDecimalStringWithDigit();
                    numericUpDown.PART_TextBox.CaretIndex = numericUpDown.PART_TextBox.Text.Length;
                }
                RoutedEventArgs newEventArgs = new RoutedEventArgs(NumericUpDown.ValueChangedEvent, numericUpDown);
                numericUpDown.RaiseEvent(newEventArgs);
            }
        }

        private void DoIncrease()
        {
            if (this.Value >= this.MaxValue) return;
            _tmpValue += Increment;
            Value = _tmpValue;
            RoutedEventArgs newEventArgs = new RoutedEventArgs(NumericUpDown.IncreasingEvent, this);
            this.RaiseEvent(newEventArgs);
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e) => this.DoIncrease();

        private void DoDecrease()
        {
            if (this.Value <= this.MinValue) return;
            _tmpValue -= Increment;
            Value = _tmpValue;
            RoutedEventArgs newEventArgs = new RoutedEventArgs(NumericUpDown.DecreasingEvent, this);
            this.RaiseEvent(newEventArgs);
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e) => DoDecrease();

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Action check = () =>
            {
                decimal x;
                if (decimal.TryParse(this.PART_TextBox.Text, out x))
                {
                    if (x == 0)
                    {
                        this.Value = 0;
                        this.PART_TextBox.Text = "0";
                    }
                }
                else
                {
                    this.Value = 0;
                    this.PART_TextBox.Text = "0";
                }
            };
            var textBox = sender as TextBox;
            if (e.Key == Key.Up)
            {
                check();
                this.DoIncrease();
                return;
            }

            if (e.Key == Key.Down)
            {
                check();
                this.DoDecrease();
                return;
            }

            var valid = (this.MinValue < 0 &&
                (e.Key == Key.OemMinus ||
                e.Key == Key.Subtract)) ||
                e.Key == Key.Left ||
                e.Key == Key.Right ||
                e.Key == Key.Back ||
                (e.Key >= Key.D0 && e.Key <= Key.D9) ||
                (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) ||
                e.Key == Key.Tab;

            if (this.DecimalDigit > 0 && this.PART_TextBox.Text.Contains(".") && e.Key != Key.Back && e.Key != Key.Delete && e.Key != Key.Tab) //处理小数位已满禁止再输入
            {

                var strs = this.PART_TextBox.Text.Split('.');
                if (strs.Length == 2 && strs[1].Length == this.DecimalDigit)
                {
                    var carretIdx = this.PART_TextBox.CaretIndex;
                    var pidx = this.PART_TextBox.Text.ToString().IndexOf('.');
                    if (pidx < carretIdx)
                    {
                        var hasSelectDecimal = this.PART_TextBox.SelectedText != null && this.PART_TextBox.SelectedText.Length > 0;
                        if (!hasSelectDecimal)
                        {
                            e.Handled = true;
                            return;
                        }
                    }
                }
            }
            e.Handled = this.DecimalDigit == 0 ? !valid : !(valid || e.Key == Key.OemPeriod || e.Key == Key.Decimal);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int index = PART_TextBox.CaretIndex;

            if (string.IsNullOrEmpty(PART_TextBox.Text)) //处理空内容
            {
                if (e.Changes.Any(x => x.RemovedLength > 0))
                    this.Value = 0;
                return;
            }
            if (this.MinValue < 0 && PART_TextBox.Text == "-")//处理单个负号
            {
                return;
            }
            if (!decimal.TryParse(PART_TextBox.Text, out decimal result))
            {
                var changes = e.Changes.FirstOrDefault();
                PART_TextBox.Text = PART_TextBox.Text.Remove(changes.Offset, changes.AddedLength);
                if (string.IsNullOrEmpty(PART_TextBox.Text))
                    PART_TextBox.Text = "0";
                PART_TextBox.CaretIndex = index > 0 ? index - changes.AddedLength : 0;
            }
            else
            {
                if (result > this.MaxValue || result < this.MinValue)
                {
                    var changes = e.Changes.FirstOrDefault();
                    PART_TextBox.Text = PART_TextBox.Text.Remove(changes.Offset, changes.AddedLength);
                    PART_TextBox.CaretIndex = PART_TextBox.Text.Length;
                }
                else
                    _tmpValue = result;
            }
            this.TextChanged?.Invoke(PART_TextBox.Text);
        }

        private string GetDecimalString(object value)
        {
            if (value == null
                || string.IsNullOrEmpty(value.ToString()))
                return string.Empty;

            var numStr = value.ToString().Trim();
            var indexOf = numStr.IndexOf(".");
            if (indexOf < 0)
                return numStr;

            var cutEndZero = numStr.TrimEnd('0');
            if (cutEndZero.EndsWith("."))
            {
                return cutEndZero.TrimEnd('.');
            }

            return cutEndZero;
        }

        private string GetDecimalStringWithDigit()
        {
            var str = Math.Round(Value, this.DecimalDigit).ToString();
            var parts = str.Split('.');
            if (parts.Length != 2)
            {
                if (this.DecimalDigit == 0)
                    return str;
                str += ".";
                for (int i = 0; i < this.DecimalDigit; i++)
                    str += '0';
                return str;
            }
            if (parts[1].Length == this.DecimalDigit)
                return str;
            for (int i = 0; i < this.DecimalDigit - parts[1].Length; i++)
                str += '0';
            return str;
        }

        public static readonly DependencyProperty DecimalDigitProperty = DependencyProperty.Register("DecimalDigit", typeof(int), typeof(NumericUpDown), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int DecimalDigit
        {
            get => this.GetValue<int>(DecimalDigitProperty);
            set => this.SetValue(DecimalDigitProperty, value);
        }
    }
}
