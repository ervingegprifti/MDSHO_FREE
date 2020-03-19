using MDSHO.ViewModels;
using System.Windows.Media;

namespace MDSHO.Helpers
{
    public static class Themes
    {
        public static void DarkTheme(InfoVM infoVM)
        {
            infoVM.WindowBorderSolidColorBrush = Colors.Black.ToSolidColorBrush();
            infoVM.TitleBackgroundSolidColorBrush = Colors.Black.ToSolidColorBrush();
            infoVM.TitleBackgroundOpacity = 1.0D;
            infoVM.TitleTextSolidColorBrush = Colors.White.ToSolidColorBrush();
            infoVM.BoxBg = Colors.Black.ToSolidColorBrush();
            infoVM.BoxBgOpacity = 0.7D;
            infoVM.WindowTextSolidColorBrush = Colors.White.ToSolidColorBrush();
        }
        public static void LightTheme(InfoVM infoVM)
        {
            infoVM.WindowBorderSolidColorBrush = ((Color)ColorConverter.ConvertFromString("#2E2E2B")).ToSolidColorBrush();
            infoVM.TitleBackgroundSolidColorBrush = ((Color)ColorConverter.ConvertFromString("#DCD9CD")).ToSolidColorBrush();
            infoVM.TitleBackgroundOpacity = 1.0D;
            infoVM.TitleTextSolidColorBrush = ((Color)ColorConverter.ConvertFromString("#2E2E2B")).ToSolidColorBrush();
            infoVM.BoxBg = ((Color)ColorConverter.ConvertFromString("#DCD9CD")).ToSolidColorBrush();
            infoVM.BoxBgOpacity = 0.9D;
            infoVM.WindowTextSolidColorBrush = ((Color)ColorConverter.ConvertFromString("#2E2E2B")).ToSolidColorBrush();
        }
    }
}
