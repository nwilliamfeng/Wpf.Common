using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Xml;

namespace Wpf.Common
{
    public static class  ObjectExtension
    {
        public static T Clone<T>(this T obj)
            where T:class
        {
            string xaml = XamlWriter.Save(obj);
            StringReader stringReader = new StringReader(xaml);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            return XamlReader.Load(xmlReader) as T;
        }
    }
}
