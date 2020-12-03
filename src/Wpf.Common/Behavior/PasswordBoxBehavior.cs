using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.Common.Behavior
{
    /// <summary>
    /// 密码框行为
    /// </summary>
    public  class PasswordBoxBehavior
    {
        /// <summary>
        /// 设置密码
        /// </summary>
        /// <remarks>此处需要默认双向绑定</remarks>
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password",typeof(string), typeof(PasswordBoxBehavior)
                , new FrameworkPropertyMetadata(string.Empty,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPasswordPropertyChanged));

        /// <summary>
        /// 设置是否有效，默认为false，如果为true将启用密码绑定
        /// </summary>
        public static readonly DependencyProperty IsEnableProperty =
            DependencyProperty.RegisterAttached("IsEnable",typeof(bool), typeof(PasswordBoxBehavior), new PropertyMetadata(BooleanBoxes.False, Attach));

        private static readonly DependencyProperty IsUpdatingProperty =
           DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(PasswordBoxBehavior),new PropertyMetadata(BooleanBoxes.False));


        public static void SetIsEnable(DependencyObject dp, bool value)
        {
            dp.SetValue(IsEnableProperty, value.Box());
        }

        public static bool GetIsEnable(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsEnableProperty);
        }

        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }

        private static bool GetIsUpdating(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsUpdatingProperty);
        }

        private static void SetIsUpdating(DependencyObject dp, bool value)
        {
            dp.SetValue(IsUpdatingProperty, value.Box());
        }

        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            passwordBox.PasswordChanged -= PasswordChanged;

            if (!(bool)GetIsUpdating(passwordBox))
            {
                passwordBox.Password = (string)e.NewValue;

            }
            passwordBox.PasswordChanged += PasswordChanged;
        }

        private static void Attach(DependencyObject sender,  DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;

            if (passwordBox == null)
                return;
    
            if ((bool)e.OldValue)
            {
                passwordBox.PasswordChanged -= PasswordChanged;
            }

            if ((bool)e.NewValue)
            {
                passwordBox.PasswordChanged += PasswordChanged;
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);

        }
    }
}
