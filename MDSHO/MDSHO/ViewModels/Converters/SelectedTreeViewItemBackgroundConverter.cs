using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MDSHO.ViewModels.Converters
{
    public class SelectedTreeViewItemBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            SolidColorBrush solidColorBrush = (SolidColorBrush)value;
            Color color = solidColorBrush.Color;
            color.A = 60;
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
