using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Wpf.Common.Controls
{
    internal static class BrushResourceKey
    {
        public const string ForegroundBrush = "ForegroundBrush"; 
        public const string AccentBrush = "AccentBrush";
        public const string TextboxBackground = "TextboxBackground";
      
        public const string ControlBorderBrush = "ControlBorderBrush";
        public const string ContextMenuBrush = "ContextMenuBrush";
        public const string MenuItemMouseOverBrush = "MenuItemMouseOverBrush";
        public const string ControlBorderActiveBrush = "ControlBorderActiveBrush";
    }

    internal static class ColorResourceKey
    {
        public const string Gray1 = "Gray1";
        public const string Gray2 = "Gray2";
        public const string Gray3 = "Gray3";
        public const string Gray4 = "Gray4";
        public const string Gray5 = "Gray5";
        public const string Gray6 = "Gray6";
        public const string Gray7 = "Gray7";
        public const string Gray8 = "Gray8";
        public const string Gray9 = "Gray9";
        public const string Gray10 = "Gray10";
        public const string Gray11 = "Gray11";
        public const string Gray12 = "Gray12";
    }

    public static class ThemeExtension
    {
        public static void SetDarkTheme(this Application application)
        {
            var dic = application.Resources;
            dic[BrushResourceKey.ForegroundBrush] = new SolidColorBrush(Colors.White);
            dic[BrushResourceKey.AccentBrush] = GetBrushFromResource(application, ColorResourceKey.Gray1);
            dic[BrushResourceKey.TextboxBackground] = GetBrushFromResource(application, ColorResourceKey.Gray10);
        
            dic[BrushResourceKey.ControlBorderBrush] = GetBrushFromResource(application, ColorResourceKey.Gray3);
            dic[BrushResourceKey.ContextMenuBrush] = GetBrushFromResource(application, ColorResourceKey.Gray7);
            dic[BrushResourceKey.MenuItemMouseOverBrush] = GetBrushFromResource(application, ColorResourceKey.Gray8);
        }

        public static Color GetColorFromResource(this Application application, string colorKey) =>(Color) ColorConverter.ConvertFromString(application.Resources[colorKey].ToString());

        public static Brush GetBrushFromResource(this Application application, string colorKey) => new SolidColorBrush(application.GetColorFromResource(colorKey));
             
    }
}
