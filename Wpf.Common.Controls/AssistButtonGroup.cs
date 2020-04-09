using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.Common.Controls
{
    public class AssistButtonGroup :HeaderedItemsControl
    {
        static AssistButtonGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AssistButtonGroup), new FrameworkPropertyMetadata(typeof(AssistButtonGroup)));         
        }
        public AssistButtonGroup()
        {
            
        }

        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register(nameof(Columns), typeof(int), typeof(AssistButtonGroup), new PropertyMetadata(3));

        public int Columns
        {
            get => this.GetValue<int>(ColumnsProperty);
            set => this.SetValue(ColumnsProperty, value);
        }
       
    }
}
