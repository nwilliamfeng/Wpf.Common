
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Wpf.Common.Controls.Behavior;

namespace Wpf.Common.Controls
{  
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_ButtonUp", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_ButtonDown", Type = typeof(ButtonBase))]
    public class NumericUpDown : Control
    {
        private TextBox _textBox = new TextBox();

        public static readonly DependencyProperty CaretBrushProperty = DependencyProperty.Register("CaretBrush", typeof(Brush), typeof(NumericUpDown));

        public Brush CaretBrush
        {
            get => this.GetValue<Brush>(CaretBrushProperty);
            set => this.SetValue(CaretBrushProperty, value);
        }

        public static readonly DependencyProperty ButtonVisibilityProperty = DependencyProperty.Register("ButtonVisibility", typeof(Visibility), typeof(NumericUpDown), new PropertyMetadata(Visibility.Visible));

        public Visibility ButtonVisibility
        {
            get => this.GetValue<Visibility>(ButtonVisibilityProperty);
            set => this.SetValue(ButtonVisibilityProperty, value);
        }

        public NumericUpDown() 
        {
            Loaded += (s, e) => OnApplyTemplate();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if(_textBox!=null)
            {
                _textBox.TextChanged -= TextBox_TextChanged;
                _textBox.LostFocus -= TextBox_LostFocus;
                _textBox.MouseWheel -= TextBox_MouseWheel;
            }
                
            if (GetTemplateChild("PART_TextBox") is TextBox textBox)
            {
                _textBox = textBox;            
                TextCompositionManager.RemovePreviewTextInputHandler(_textBox, PreviewTextInputHandler);               
                _textBox.TextChanged += TextBox_TextChanged;               
                _textBox.LostFocus += TextBox_LostFocus;
                _textBox.MouseWheel += TextBox_MouseWheel;
                this._textBox.Text = this.CurrentText;     
                this.UpdateData();
            }
            if (GetTemplateChild("PART_ButtonUp") is ButtonBase buttonUp)
            {
                buttonUp.Click -= ButtonUp_Click;
                buttonUp.Click += ButtonUp_Click;
            }
            if (GetTemplateChild("PART_ButtonDown") is ButtonBase buttonDown)
            {
                buttonDown.Click -= ButtonDown_Click;
                buttonDown.Click += ButtonDown_Click;
            }
            TextCompositionManager.AddPreviewTextInputHandler(_textBox, PreviewTextInputHandler);

            DataObjectPastingEventHandler dataPastingHandle = (s, e) =>
            {
                if (e.DataObject.GetDataPresent(typeof(String)))
                {
                    String text = (String)e.DataObject.GetData(typeof(String));
                    if (DecimalDigit==0)
                    {
                        if (TextBoxBehavior.Int_regex.IsMatch(text))
                            e.CancelCommand();
                    }
                    else  
                    {
                        if (TextBoxBehavior.Double_regex.IsMatch(text))
                            e.CancelCommand();
                    }
                }
                else
                    e.CancelCommand();
            };

            InputMethod.SetIsInputMethodEnabled(_textBox, false);
            DataObject.RemovePastingHandler(_textBox, dataPastingHandle);
            DataObject.AddPastingHandler(_textBox, dataPastingHandle);
        }

        private void TextBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0 && ( this.Value + Increment <= this.MaxValue))
                this.Value += Increment;
            else if (e.Delta < 0 && (this.Value - Increment >= this.MinValue))
                this.Value -= Increment;
        }

        private void PreviewTextInputHandler(object sender, TextCompositionEventArgs e)
        {
            var tb = sender as TextBox;
            var idx = tb.CaretIndex;
            if (this.DecimalDigit == 0)
            {
                e.Handled = TextBoxBehavior.Int_regex.IsMatch(e.Text);
                
            }
            else if (this.DecimalDigit > 0)
            {
                e.Handled = TextBoxBehavior.Double_regex.IsMatch(e.Text);

                if (tb.Text.Contains(".") && e.Text == ".") 
                    e.Handled = true;                           
            }
           
            if (!e.Handled)
                UpdateData();
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            if (_textBox != null)
            {
                _textBox?.Focus();
                _textBox.Select(_textBox.Text.Length, 0);
            }
        }

   

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
          
            this._textBox.Text = CurrentText;
        }

        #region Other

        private static bool ValidateValueCallback(object value)
        {
            if (value == null)
                return false;
            if (value is double val
                && val >= double.MinValue
                && val <= double.MaxValue)
                return true;
            return false;
        }

       
        private static object CoerceValueCallback(DependencyObject d, object value)
        {
           
            var c = (NumericUpDown)d;
            if (c == null)
                return value;
            bool overrange = false;
            if (value is double v)
            {
               
                var min = c.MinValue;
                if (v < min)
                {
                    c.Value = min;
                    overrange = true;
                }
                var max = c.MaxValue;
                if (v > max)
                {
                    c.Value= max;
                    overrange = true;
                }
            }

            if (overrange)
                c._textBox.Text = c.Value.ToString();
            else
                c.SetText();

            return overrange?c.Value: value;
        }

        static NumericUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(NumericUpDown)));
        }

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(NumericUpDown));

        public event RoutedEventHandler ValueChanged
        {
            add=> this.AddHandler(ValueChangedEvent, value);
            remove=> this.RemoveHandler(ValueChangedEvent, value);
        }

       

        public static readonly RoutedEvent IncreasingEvent = EventManager.RegisterRoutedEvent("Increasing", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(NumericUpDown));

        public event RoutedEventHandler Increasing
        {
            add=> this.AddHandler(IncreasingEvent, value);
            remove => this.RemoveHandler(IncreasingEvent, value);
        }

        public static readonly RoutedEvent DecreasingEvent = EventManager.RegisterRoutedEvent("Decreasing", RoutingStrategy.Direct,typeof(RoutedEventHandler), typeof(NumericUpDown));

        public event RoutedEventHandler Decreasing
        {
            add=> this.AddHandler(DecreasingEvent, value);       
            remove=> this.RemoveHandler(DecreasingEvent, value);         
        }

        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(NumericUpDown), new FrameworkPropertyMetadata(double.MaxValue, MaxValueChangedCallback, CoerceMaxValueCallback));

        private static object CoerceMaxValueCallback(DependencyObject d, object value)
        {
            if (value == null)
                return 0;

            var c = (NumericUpDown)d;
            if (c == null)
                return value;

            if (value is double v)
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

        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(NumericUpDown), new FrameworkPropertyMetadata(0d, MinValueChangedCallback, CoerceMinValueCallback));

        private static object CoerceMinValueCallback(DependencyObject d, object value)
        {
            if (value == null)
                return value;
            var c = (NumericUpDown)d;
            if (c == null)
                return value;
            if (value is double v)
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

        public double Increment
        {
            get { return (double)GetValue(IncrementProperty); }
            set { SetValue(IncrementProperty, value); }
        }

        public static readonly DependencyProperty IncrementProperty =
            DependencyProperty.Register("Increment", typeof(double), typeof(NumericUpDown), new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, CoerceIncrementCallback));

        private static object CoerceIncrementCallback(DependencyObject d, object value)
        {
            if (value == null)
                return value;

            var c = (NumericUpDown)d;
            if (c == null)
                return value;

            if (value is double v)
            {
                if (v < c.MinValue)
                    return 1;
                if (v > c.MaxValue)
                    return 1;
            }

            return value;
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(NumericUpDown), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged, CoerceValueCallback), ValidateValueCallback);

        #endregion

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown numericUpDown = (NumericUpDown)d;
            if (numericUpDown == null)
                return;

            if (e.OldValue != e.NewValue)
                numericUpDown.SetText();
        }

        private void DoIncrease()
        {
            if (this.Value + Increment> this.MaxValue) return;
        
            Value+=Increment;
            RoutedEventArgs newEventArgs = new RoutedEventArgs(NumericUpDown.IncreasingEvent, this);
            this.RaiseEvent(newEventArgs);
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e) => this.DoIncrease();

        private void DoDecrease()
        {
            if (this.Value -Increment < this.MinValue) return;
             
            Value -= Increment;
            RoutedEventArgs newEventArgs = new RoutedEventArgs(NumericUpDown.DecreasingEvent, this);
            this.RaiseEvent(newEventArgs);
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e) => DoDecrease();

        private void UpdateData()
        {           
            if (double.TryParse(this._textBox.Text, out var value))
            {
                 _setTextEnable = false;
                Value = value;
                _setTextEnable = true;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.UpdateData();
        }

        public string ValueFormat
        {
            get => (string)GetValue(ValueFormatProperty);
            set => SetValue(ValueFormatProperty, value);
        }

        public static readonly DependencyProperty ValueFormatProperty = DependencyProperty.Register(  "ValueFormat", typeof(string), typeof(NumericUpDown), new PropertyMetadata(default(string)));

        private bool _setTextEnable  ;

        private string CurrentText => string.IsNullOrWhiteSpace(ValueFormat)
           ? DecimalDigit > 0 ? Value.ToString($"#0.{new string('0', DecimalDigit)}") : Value.ToString() : Value.ToString(ValueFormat);

        private void SetText()
        {
            if (_setTextEnable && _textBox != null)
            {
                _textBox.Text = CurrentText;
                _textBox.Select(_textBox.Text.Length, 0);
            }
        }


        public static readonly DependencyProperty DecimalDigitProperty = DependencyProperty.Register("DecimalDigit", typeof(int), typeof(NumericUpDown), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnDecimalDigitChanged));


        private static void OnDecimalDigitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown numericUpDown = (NumericUpDown)d;
            if (numericUpDown == null)
                return;
            numericUpDown.UpdateData();
        }


        public int DecimalDigit
        {
            get => this.GetValue<int>(DecimalDigitProperty);
            set => this.SetValue(DecimalDigitProperty, value);
        }
    }
}
