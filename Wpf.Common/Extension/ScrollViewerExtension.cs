using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Wpf.Common
{
    public static class ScrollViewerExtension
    {
        /// <summary>
        /// 返回指定的ScrollViewer是否已经滚动到底部
        /// </summary>
        /// <param name="scrollViewer"></param>
        /// <returns></returns>
        public static bool IsScrollEnd(this ScrollViewer scrollViewer)
            => scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight;
        
    }
}
