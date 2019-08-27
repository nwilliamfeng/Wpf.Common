using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf.Common
{
    public static class PointExtension
    {
        public static IEnumerable<Point> ParsePoints(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return new Point[] { };
            var strs =str.Split(' ');
            if (strs.Length == 0)
                return new Point[] { };
            return strs.Select(x =>
            {
                var data = x.Split(',');
                return new Point(int.Parse(data[0]), int.Parse(data[1]));
            });
        }
    }
}
