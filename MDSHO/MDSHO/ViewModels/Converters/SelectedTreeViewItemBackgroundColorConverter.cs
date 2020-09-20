using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MDSHO.ViewModels.Converters
{
    public class SelectedTreeViewItemBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            Color color = (Color)value;
            color.A = 60;
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
