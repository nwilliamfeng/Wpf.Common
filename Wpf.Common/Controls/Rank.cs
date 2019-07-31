using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.ComponentModel;

namespace Wpf.Common.Controls
{
    public class Rank:Control
    {
        private const string ItemsControlName = "PART_ItemsControl";
        public static readonly DependencyProperty ItemCountProperty = DependencyProperty.Register("ItemCount", typeof(int), typeof(Rank), new PropertyMetadata(5));

        /// <summary>
        /// 设置Rank项的数据模板
        /// </summary>
        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(Rank), new PropertyMetadata(null));
        
        /// <summary>
        /// 设置Rank项的大小
        /// </summary>
        public static readonly DependencyProperty ItemSizeProperty = DependencyProperty.Register("ItemSize", typeof(double), typeof(Rank), new PropertyMetadata(20.0));

        /// <summary>
        /// 设置RankItem数
        /// </summary>
        public int ItemCount
        {
            get => this.GetValue<int>(ItemCountProperty);
            set => this.SetValue(ItemCountProperty, value);           
        }

        public DataTemplate ItemTemplate
        {
            get => this.GetValue<DataTemplate>(ItemTemplateProperty);
            set => this.SetValue(ItemTemplateProperty, value);
        }


        public double ItemSize
        {
            get => this.GetValue<double>(ItemTemplateProperty);
            set => this.SetValue(ItemTemplateProperty,value);
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var count = this.GetValue<int>(ItemCountProperty);
            var size = this.GetValue<double>(ItemSizeProperty);
            var itemControl = this.GetTemplateChild(ItemsControlName) as ItemsControl;
            var items = Enumerable.Range(1, count)
                .Select(x => new RankItem(x) { Size = size })
                .ToList();
            items.ForEach(x => x.ParentItems = items);
            itemControl.ItemsSource = items;

        }
    }

    public class RankItem : INotifyPropertyChanged
    {
        private bool _isSelected;

        public RankItem( int value)
        {
            this.Value = value;
        }

        public IEnumerable<RankItem> ParentItems { get; internal set; }

        public bool IsSelected
        {
            get => this._isSelected;
            set
            {
                this._isSelected = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsSelected"));
            }
        }


        private bool _isTempSelected;
        public bool IsTempSelected
        {
            get => this._isTempSelected;
            set
            {
                this._isTempSelected = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsTempSelected"));
            }
        }

        private bool _isEnabled=true;

        public bool IsEnabled
        {
            get => this._isEnabled;
            set
            {
                this._isEnabled = value;
                this.PropertyChanged?.Invoke(this,new PropertyChangedEventArgs("IsEnabled"));
            }
        }

        public double Size { get; set; } =16;

        public int Value { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
