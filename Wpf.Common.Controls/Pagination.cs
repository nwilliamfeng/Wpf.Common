using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Wpf.Common.Input;
  

namespace Wpf.Common.Controls
{
    
    [TemplatePart(Name = PART_FirstButton)]
    [TemplatePart(Name = PART_NextButton)]
    [TemplatePart(Name = PART_PreviousButton)]
    [TemplatePart(Name = PART_LastButton)]
    public class Pagination:Control
    {
        public const string PART_FirstButton = "PART_FirstButton";
        public const string PART_NextButton = "PART_NextButton";
        public const string PART_PreviousButton = "PART_PreviousButton";
        public const string PART_LastButton = "PART_LastButton";

        public static readonly DependencyProperty CurrentPageNumberProperty = DependencyProperty.Register("CurrentPageNumber", 
            typeof(int), typeof(Pagination),new FrameworkPropertyMetadata(0,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCurrentPageNumberPropertyChanged));

       
        public static readonly DependencyProperty TotalCountProperty = DependencyProperty.Register("TotalCount", typeof(int),typeof(Pagination)
            ,new PropertyMetadata(0,new PropertyChangedCallback(OnTotalCountPropertyChanged)));

        public static readonly DependencyProperty PageSizeProperty = DependencyProperty.Register("PageSize", typeof(int), typeof(Pagination)
          , new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnPageSizePropertyChanged)));

        // public static readonly DependencyProperty NavigatorProperty = DependencyProperty.Register("CurrentPageNumber", typeof(int), typeof(Pagination));


       

        public static readonly DependencyProperty GotoCommandProperty = DependencyProperty.Register("GotoCommand", typeof(ICommand), typeof(Pagination));

        private static readonly DependencyPropertyKey PageNumbersKey = DependencyProperty.RegisterReadOnly("PageNumbers", typeof(IList<int>),
            typeof(Pagination), new FrameworkPropertyMetadata(default(IList<int>), FrameworkPropertyMetadataOptions.None));

        public static readonly DependencyProperty PageNumbersProperty = PageNumbersKey.DependencyProperty;

        public IList<int> PageNumbers
        {
            get { return (IList<int>)GetValue(PageNumbersProperty); }
            protected set
            {
                SetValue(PageNumbersKey, value);
            }
        }


        private static readonly DependencyPropertyKey PageCountKey = DependencyProperty.RegisterReadOnly("PageCount", typeof(int),
           typeof(Pagination), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None));

        public static readonly DependencyProperty PageCountProperty = PageCountKey.DependencyProperty;

        public int PageCount
        {
            get { return (int)GetValue(PageCountProperty); }
            protected set
            {
                SetValue(PageCountKey, value);
            }
        }

        private static void OnCurrentPageNumberPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs arg)
        {
            var paging = obj as Pagination;
            if (paging == null) return;
            if (!(arg.NewValue is int)) return;
            if (paging.CurrentPageNumber < 1) return;
            if (paging._isFirstLoaded)
                paging._isFirstLoaded = false;
            else
                paging.GotoCommand?.Execute(new Tuple<int, int>(paging.CurrentPageNumber, paging.PageSize));
        }

        private static void OnTotalCountPropertyChanged(DependencyObject obj,DependencyPropertyChangedEventArgs arg)
        {
            var paging = obj as Pagination;
            if (paging == null) return;
            if (!(arg.NewValue is int)) return;
            if (arg.NewValue == arg.OldValue) return;
            Update(paging);
        }

        private static void Update(Pagination paging)
        {
            paging.PageCount = (int)Math.Ceiling((decimal)paging.TotalCount / paging.PageSize);
            paging.PageNumbers = Enumerable.Range(1, paging.PageCount).ToList();
            paging.CurrentPageNumber = 0; //强迫更新，否则在页号为1时候不会触发
            paging.CurrentPageNumber = paging.PageCount > 0 ? 1 : 0;
        }

        private static void OnPageSizePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs arg)
        {
            var paging = obj as Pagination;
            if (paging == null) return;
            if (!(arg.NewValue is int)) return;
           
            Update(paging);

        }


        private bool _isFirstLoaded = true; //因为在

        public int CurrentPageNumber
        {
            get => this.GetValue<int>(CurrentPageNumberProperty);
            set => this.SetValue(CurrentPageNumberProperty, value);
        }

        public int PageSize
        {
            get => this.GetValue<int>(PageSizeProperty);
            set => this.SetValue(PageSizeProperty, value);
        }

        public int TotalCount
        {
            get => this.GetValue<int>(TotalCountProperty);
            set => this.SetValue(TotalCountProperty, value);
        }

         

        public ICommand GotoCommand
        {
            get => this.GetValue<ICommand>(GotoCommandProperty);
            set => this.SetValue(GotoCommandProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var nextButton = this.GetTemplateChild(PART_NextButton) as Button;
            this.GotoCommand?.Execute(new Tuple<int,int>(1,this.PageSize));
            nextButton.Command = new RelayCommand(() =>
              {
                  this.CurrentPageNumber += 1;
              }, () => this.CurrentPageNumber > 0 && this.CurrentPageNumber < this.PageCount);

            var firstButton = this.GetTemplateChild(PART_FirstButton) as Button;
            firstButton.Command = new RelayCommand(() =>
            {
                this.CurrentPageNumber = 1;
            }, () => this.CurrentPageNumber > 1);

            var lastButton = this.GetTemplateChild(PART_LastButton) as Button;
            lastButton.Command = new RelayCommand(() =>
            {
                this.CurrentPageNumber = this.PageCount;
            }, () => this.CurrentPageNumber > 0 && this.CurrentPageNumber < this.PageCount);

            
            var previousButton = this.GetTemplateChild(PART_PreviousButton) as Button;
            previousButton.Command = new RelayCommand(() =>
            {
                this.CurrentPageNumber -= 1;
            }, () => this.CurrentPageNumber > 1);
        }

        

    }
}
