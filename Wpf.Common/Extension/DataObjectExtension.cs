using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf.Common
{
    /// <summary>
    /// IDataObject扩展方法
    /// </summary>
    public static class DataObjectExtension
    {
        /// <summary>
        /// 返回拖拽文件名称集
        /// </summary>
        /// <param name="dataObject"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetDropFileNames(this IDataObject dataObject)
        {
            return dataObject.GetData(DataFormats.FileDrop) as string[];
            
        }
    }
}
