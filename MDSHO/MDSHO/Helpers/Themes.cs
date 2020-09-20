using MDSHO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;


namespace MDSHO.Helpers
{
    public static class Themes
    {
        public static void DarkTheme(WindowInfoVM windowInfoVM)
        {
            windowInfoVM.WindowBorderSolidColorBrush = Colors.Black.ToSolidColorBrush();
            windowInfoVM.TitleBackgroundSolidColorBrush = Colors.Black.ToSolidColorBrush();
            windowInfoVM.TitleBackgroundOpacity = 1.0D;
            windowInfoVM.TitleTextSolidColorBrush = Colors.White.ToSolidColorBrush();
            windowInfoVM.WindowBackgroundSolidColorBrush = Colors.Black.ToSolidColorBrush();
            windowInfoVM.WindowBackgroundOpacity = 0.7D;
            windowInfoVM.WindowTextSolidColorBrush = Colors.White.ToSolidColorBrush();
        }
        public static void LightTheme(WindowInfoVM windowInfoVM)
        {
            windowInfoVM.WindowBorderSolidColorBrush = Colors.WhiteSmoke.ToSolidColorBrush();
            windowInfoVM.TitleBackgroundSolidColorBrush = Colors.WhiteSmoke.ToSolidColorBrush();
            windowInfoVM.TitleBackgroundOpacity = 1.0D;
            windowInfoVM.TitleTextSolidColorBrush = Colors.Black.ToSolidColorBrush();
            windowInfoVM.WindowBackgroundSolidColorBrush = Colors.WhiteSmoke.ToSolidColorBrush();
            windowInfoVM.WindowBackgroundOpacity = 0.7D;
            windowInfoVM.WindowTextSolidColorBrush = Colors.Black.ToSolidColorBrush();
        }
        public static void MyTheme(WindowInfoVM windowInfoVM)
        {
            windowInfoVM.WindowBorderSolidColorBrush = ((Color)ColorConverter.ConvertFromString("#2E2E2B")).ToSolidColorBrush();
            windowInfoVM.TitleBackgroundSolidColorBrush = ((Color)ColorConverter.ConvertFromString("#DCD9CD")).ToSolidColorBrush();
            windowInfoVM.TitleBackgroundOpacity = 1.0D;
            windowInfoVM.TitleTextSolidColorBrush = ((Color)ColorConverter.ConvertFromString("#2E2E2B")).ToSolidColorBrush();
            windowInfoVM.WindowBackgroundSolidColorBrush = ((Color)ColorConverter.ConvertFromString("#DCD9CD")).ToSolidColorBrush();
            windowInfoVM.WindowBackgroundOpacity = 0.9D;
            windowInfoVM.WindowTextSolidColorBrush = ((Color)ColorConverter.ConvertFromString("#2E2E2B")).ToSolidColorBrush();
        }
    }

}
