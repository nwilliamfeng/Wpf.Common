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
    [TemplatePart(Name=ItemsControlName)]
    public class Rank:Control
    {
        public const string ItemsControlName = "PART_ItemsControl";

        public static readonly DependencyProperty ItemCountProperty = DependencyProperty.Register("ItemCount", typeof(int), typeof(Rank), new PropertyMetadata(5));

        /// <summary>
        /// 设置Rank的值
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(Rank), new PropertyMetadata(0));


        /// <summary>
        /// 设置Rank项的数据模板
        /// </summary>
        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(Rank), new PropertyMetadata(null));
        
        /// <summary>
        /// 设置Rank项的大小
        /// </summary>
        public static readonly DependencyProperty ItemSizeProperty = DependencyProperty.Register("ItemSize", typeof(double), typeof(Rank), new PropertyMetadata(20.0));

        /// <summary>
        /// 设置Rank值
        /// </summary>
        public int Value
        {
            get => this.GetValue<int>(ValueProperty);
            set => this.SetValue(ValueProperty, value);
        }

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
            PropertyChangedEventHandler valueChange = (s, e) =>
            {
                if (e.PropertyName == "IsSelected")
                {
                    var rankItem = s as RankItem;
                    this.Value = rankItem.IsSelected ? rankItem.Value : rankItem.Value - 1;
                }
            };

            items.ForEach(x =>
            {
                x.ParentItems = items;
                x.PropertyChanged += valueChange;
            });

            itemControl.ItemsSource = items;
 
            itemControl.MouseLeave += (s, e) =>
            {
                items.Where(x => !x.IsSelected && x.IsTempSelected)
                .ToList()
                .ForEach(x => x.IsTempSelected = false);
                e.Handled = true;
            };
            
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

        internal event EventHandler Clicked;

        internal void RaiseClick() => this.Clicked?.Invoke(this, EventArgs.Empty);?

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
