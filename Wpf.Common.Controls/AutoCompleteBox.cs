
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Collections;
using Wpf.Common.Controls.Helper;

namespace Wpf.Common.Controls
{

    [TemplatePart(Name = "PART_ListBox", Type = typeof(ListBox))]
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    [TemplatePart(Name = "PART_Dropdown_Button", Type = typeof(Button))]
    public class AutoCompleteBox : Selector
    {
        static AutoCompleteBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoCompleteBox), new FrameworkPropertyMetadata(typeof(AutoCompleteBox)));
        }

        private ToggleButton _dropdownButton;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.Loaded -= AutoCompleteBox_Loaded;
            this.Loaded += AutoCompleteBox_Loaded;

            _dropdownButton = GetTemplateChild("PART_Dropdown_Button") as ToggleButton;
            if (_dropdownButton == null) return;
            _dropdownButton.Checked -= _dropdownButton_Checked;
            _dropdownButton.Checked += _dropdownButton_Checked;

            _textBox = GetTemplateChild("PART_TextBox") as TextBox;
            if (_textBox == null) return;
            _textBox.TextChanged -= OnTextChanged;
            _textBox.TextChanged += OnTextChanged;

            _textBox.KeyUp -= TextBox_KeyUp;
            _textBox.KeyUp += TextBox_KeyUp;

            _textBox.GotKeyboardFocus -= _textBox_GotKeyboardFocus;
            _textBox.GotKeyboardFocus += _textBox_GotKeyboardFocus;

            _textBox.LostKeyboardFocus -= _textBox_LostKeyboardFocus;
            _textBox.LostKeyboardFocus += _textBox_LostKeyboardFocus;

            _textBox.PreviewMouseDoubleClick -= _textBox_PreviewMouseDoubleClick;
            _textBox.PreviewMouseDoubleClick += _textBox_PreviewMouseDoubleClick;

            _popup = GetTemplateChild("PART_Popup") as Popup;
            if (_popup == null) return;
            this._popup.Closed -= _popup_Closed;
            this._popup.Closed += _popup_Closed;

            this._popup.Opened -= _popup_Opened;
            this._popup.Opened += _popup_Opened;
            _listBox = GetTemplateChild("PART_ListBox") as ListBox;
            if (_listBox == null) return;
            //处理点击或按回车选中item并关闭popup
            _listBox.PreviewMouseDown -= ListBox_PreviewMouseDown;
            _listBox.PreviewMouseDown += ListBox_PreviewMouseDown;

            _textBox.PreviewKeyDown -= _textBox_PreviewKeyDown;
            _textBox.PreviewKeyDown += _textBox_PreviewKeyDown;
           
        }


        private void _dropdownButton_Checked(object sender, RoutedEventArgs e)
        {         
            if (this._dropdownButton.IsChecked == true)
            {
                _needLoadAll = true;
                
                (ItemsSource as ICollectionView)?.Refresh();
                this._popup.IsOpen = true;
            }
              
            else if (this._dropdownButton.IsChecked == false)
                this._popup.IsOpen = false;
            this._textBox.Focus();
        }

      
        

        private void _textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !this._popup.IsOpen)
            {
                e.Handled = true;
                _textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
            if (!this._popup.IsOpen)
            {
                if (e.Key == Key.Down) //如果未打开则触发打开
                    this._dropdownButton.IsChecked = true;
                return;
            }
            if (this._listBox.Items.Count == 0) return;
            if (e.Key == Key.Down)
                this._listBox.SelectedIndex = this._listBox.SelectedIndex < this._listBox.Items.Count - 1 ? this._listBox.SelectedIndex + 1 : 0;

            if (e.Key == Key.Up)
                this._listBox.SelectedIndex = this._listBox.SelectedIndex > 0 ? this._listBox.SelectedIndex - 1 : this._listBox.Items.Count - 1;

            if ((e.Key == Key.Up || e.Key == Key.Down) && this._listBox.SelectedItem != null)
                this._listBox.ScrollIntoView(this._listBox.SelectedItem);
            if (e.Key == Key.Enter)
            {
                this.SelectedItem = _listBox.SelectedItem;
                ClosePopup();
            }

        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            if (this._textBox != null)
                this._textBox.Focus();
        }

        private void _textBox_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this._textBox.SelectAll();
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            this.SetSelectItemText();
        }

        private void SetSelectItemText()
        {
            if (_textBox == null || this._listBox==null) return;
            var item = this.SelectedItem;
            this._disableSearch = true;
            if (item == null)
            {
                this._textBox.Text = null;
                this._listBox.SelectedItem = null;
                return;
            }
            if (string.IsNullOrEmpty(this.SearchPath) && string.IsNullOrEmpty(this.DisplayMemberPath))
            {
                this._textBox.Text = item.ToString();
            }
            else
            {
                var pName = string.IsNullOrEmpty(this.SearchPath) ? this.DisplayMemberPath : this.SearchPath;
                this._textBox.Text = item.GetType().GetProperty(pName)?.GetValue(item,null)?.ToString();
            }
        }


        private void _textBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            this._popup.StaysOpen = false;
            if (!this._popup.IsOpen && !string.IsNullOrEmpty(SearchPath) && this.SelectedItem != null)
            {
                var pi = this.SelectedItem.GetType().GetProperty(SearchPath);

                if (pi != null)
                {
                    var txt = pi.GetValue(SelectedItem,null) as string;
                    if (this._textBox.Text != txt)
                        this._textBox.Text = txt;
                }
            }
        }

        private void _textBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(this._textBox.Text) && !this._popup.IsOpen)
            {
                if (this.SelectedItem != null) return;
                if (this._listBox.ItemsSource?.OfType<object>().Count() == 0) return;
                this._popup.IsOpen = true;
                if (this._listBox.Items.Count > 0)
                    this._listBox.SelectedIndex = 0;
                this._popup.StaysOpen = true;
            }
        }

        private void _popup_Opened(object sender, EventArgs e)
        {
             this._dropdownButton.IsChecked = true;
            this._listBox.SelectedItem = null;
        }

        private void _popup_Closed(object sender, EventArgs e)
        {
            this._dropdownButton.IsChecked = false;
            if (this._listBox.SelectedItem != null)
            {
                var str = GetListBoxItemText(this.SelectedItem);
                if (this._textBox.Text != str)
                {
                    _disableSearch = true;
                    this._textBox.Text = str;
                }
            }
        }


        private string GetListBoxItemText(object obj)
        {
            if (obj == null) return null;
            if (string.IsNullOrEmpty(this.SearchPath)) return obj.ToString();
            var pi = obj.GetType().GetProperty(this.SearchPath);
            if (pi == null) return null;
            return pi.GetValue(obj,null) as string;
        }

        private bool _disableSearch = false;

        private TextBox _textBox;

        private void AutoCompleteBox_Loaded(object sender, RoutedEventArgs e)
        {
            this.SetSelectItemText();
            if (!(this.ItemsSource is IList)) return;
            var cvs = CollectionViewSource.GetDefaultView(this.ItemsSource);

            cvs.Filter = x =>
            {
                if (_needLoadAll) return true;
                if (string.IsNullOrEmpty(this._textBox.Text)) return true;
                string value = null;
                if (string.IsNullOrEmpty(this.SearchPath)) //如果SearchPath为空，则判断tostring
                {
                    value = x.ToString();
                }
                else
                {
                    var pi = x.GetType().GetProperty(this.SearchPath);
                    if (pi == null) return false;
                    value = pi.GetValue(x,null) as string;
                    if (string.IsNullOrEmpty(value)) return false;
                 
                }

                var result = value.Contains(_textBox.Text);
                if (result == false)
                {
                    var spell = ChineseSpellHelper.GetSpellCode(value);
                    return spell.Contains(_textBox.Text.ToUpper());
                }
                return result;

            };
            this.ItemsSource = cvs;
        }

        /// <summary>
        /// 针对处理下拉框，需要全部加载
        /// </summary>
        private bool _needLoadAll = false;

        private void ListBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(_listBox, e.OriginalSource as DependencyObject) as ListBoxItem;
            this.AfterSelectListBoxItem(item);
            e.Handled = this._listBox.ActualWidth - e.GetPosition(this._listBox).X >= 15; //处理滚动条点击
        }

        private void AfterSelectListBoxItem(ListBoxItem listBoxItem)
        {
            if (listBoxItem == null) return;
            this.SelectedItem = listBoxItem.DataContext;
            ClosePopup();
        }


        private void ClosePopup()
        {
            this._needLoadAll = false;
            _popup.StaysOpen = false;
            this._popup.IsOpen = false;
            this._textBox.Focus();
            this._textBox.CaretIndex = this._textBox.Text.Length > 0 ? this._textBox.Text.Length : 0;
        }


        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            //if (!this._popup.IsOpen) 
            //{
            //    if (e.Key == Key.Down) //如果未打开则触发打开
            //        this._dropdownButton.IsChecked = true;
            //    return;
            //}
            //if (this._listBox.Items.Count == 0) return;
            //if (e.Key == Key.Down)
            //    this._listBox.SelectedIndex = this._listBox.SelectedIndex < this._listBox.Items.Count - 1 ? this._listBox.SelectedIndex + 1 : 0;

            //if (e.Key == Key.Up)
            //    this._listBox.SelectedIndex = this._listBox.SelectedIndex > 0 ? this._listBox.SelectedIndex - 1 : this._listBox.Items.Count - 1;

            //if((e.Key==Key.Up || e.Key==Key.Down) && this._listBox.SelectedItem!=null)
            //    this._listBox.ScrollIntoView(this._listBox.SelectedItem);
            //if (e.Key == Key.Enter)
            //{
            //    this.SelectedItem = _listBox.SelectedItem;
            //    ClosePopup();
            //}
        }

      
        private Popup _popup;

        private ListBox _listBox;

        public static readonly DependencyProperty SearchPathProperty = DependencyProperty.Register("SearchPath", typeof(string), typeof(AutoCompleteBox));

        public string SearchPath
        {
            get => this.GetValue<string>(SearchPathProperty);
            set => this.SetValue(SearchPathProperty, value);
        }

        private void OnTextChanged(object sender, TextChangedEventArgs arg)
        {
            var txtBox = sender as TextBox;
            this._needLoadAll = false;

            if (this.SelectedItem == null && string.IsNullOrEmpty(this._textBox.Text)) //如果未选中并且文本为空需要刷新collectionview
            {
                (ItemsSource as ICollectionView)?.Refresh();
                return;
            }

            if (this.SelectedItem != null && !string.IsNullOrEmpty(this.SearchPath))
            {
                var pi = this.SelectedItem.GetType().GetProperty(SearchPath);
                if (pi != null && pi.GetValue(SelectedItem,null) as string == txtBox.Text)
                    return;
            }
            if (!_disableSearch)
            {
                (ItemsSource as ICollectionView)?.Refresh();
                if (this.ItemsSource != null && this.ItemsSource.OfType<object>().Count() > 0 && !_popup.IsOpen)
                {
                    _popup.IsOpen = true;
                    if (this._listBox.Items.Count > 0)
                        this._listBox.SelectedIndex = 0;
                }
                else if (this._listBox.Items.Count > 0)
                    this._listBox.SelectedIndex = 0;
                this._textBox.CaretIndex = this._textBox.Text.Length; //在获得焦点后将光标移到最后
            }
            else
            {
                _disableSearch = false;

            }


        }

    }

}
