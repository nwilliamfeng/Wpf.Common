using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.IO;

namespace Wpf.Common.Behavior
{
    public static class FileDropAndOpenBehavior
    {
        /// <summary>
        /// 是否拖拽后自动打开文件
        /// </summary>
        public static readonly DependencyProperty OpenFileEnableProperty = DependencyProperty.RegisterAttached("OpenFileEnable"
            , typeof(bool)
            , typeof(FileDropAndOpenBehavior)
            , new PropertyMetadata(OnPropertyChange));

        /// <summary>
        /// 支持的文件扩展名
        /// </summary>
        public static readonly DependencyProperty FileExtensionsProperty = DependencyProperty.RegisterAttached("FileExtensions"
            , typeof(string)
            , typeof(FileDropAndOpenBehavior)
            , null);

        public static void SetFileExtensions(UIElement element, string value) => element.SetValue(FileExtensionsProperty, value);

        public static string GetFileExtensions(UIElement element) => element.GetValue<string>(FileExtensionsProperty);

        public static void SetOpenFileEnable(UIElement element, bool value) => element.SetValue(OpenFileEnableProperty, value);

        public static bool GetOpenFileEnable(UIElement element) => element.GetValue<bool>(OpenFileEnableProperty);


        private static void OnPropertyChange(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(obj is TextBoxBase)) return;
            var tb = obj as TextBoxBase;

            tb.PreviewDragOver += (s, arg) =>
            {
                if (!tb.GetValue<bool>(OpenFileEnableProperty))
                    return;
                if (arg.Data.IsMultiFileDroped())
                    return;
                var file = arg.Data.GetDropFileNames().FirstOrDefault();
                var ext = new FileInfo(file).Extension;
                var filters = new string[] { ".txt",   ".html", ".js" };
                var otherFilters = tb.GetValue<string>(FileExtensionsProperty);
                if (!string.IsNullOrEmpty(otherFilters))
                    filters = filters.Union(otherFilters.Split(',')).Distinct().ToArray();
                arg.Effects = filters.Any(x => x.Equals(ext, StringComparison.InvariantCultureIgnoreCase)) ? DragDropEffects.Copy : DragDropEffects.None;
                arg.Handled = true;
            };

            tb.Drop += (s, arg) =>
            {
                var file = arg.Data.GetDropFileNames().FirstOrDefault();     
                var sb = new StringBuilder();
                File.ReadAllLines(file).ToList().ForEach(x=>sb.AppendLine(x));
                tb.AppendText(sb.ToString());
            };
 
                
        }

     
    }
}
