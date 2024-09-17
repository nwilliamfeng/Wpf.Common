using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Wpf.Common.Behavior
{
	/// <summary>
	/// https://stackoverflow.com/questions/1104601/xaml-how-to-have-global-inputbindings
	/// </summary>
	public static class InputBindingBehavior
	{
		public static readonly DependencyProperty InputBindingsProperty = DependencyProperty.RegisterAttached("InputBindings", typeof(InputBindingCollection), typeof(InputBindingBehavior)
			,new FrameworkPropertyMetadata(new InputBindingCollection(),OnInputBindingsPropertyValueChanged));

		private static void OnInputBindingsPropertyValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs arg)
		{
			if(dependencyObject is UIElement element)
			{
				element.InputBindings.Clear();
				element.InputBindings.AddRange(arg.NewValue as InputBindingCollection);
			}
		}

		public static InputBindingCollection GetInputBindings(UIElement uIElement)
		{
			return (InputBindingCollection)uIElement.GetValue(InputBindingsProperty);
		}

		public static void SetInputBindings(UIElement uIElement,InputBindingCollection newValue)
		{
			uIElement.SetValue(InputBindingsProperty, newValue);
		}
	}
}
