using System.Windows.Media;

namespace MDSHO.ViewModels
{
    public class InfoViewModel : BaseViewModel
    {
        private SolidColorBrush windowBackground;
        private double windowBackgroundOpacity;


        public SolidColorBrush WindowBackground
        {
            get
            {
                return windowBackground;
            }
            set
            {
                windowBackground = value;
                OnPropertyChanged(nameof(WindowBackground));
            }
        }
        public double WindowBackgroundOpacity
        {
            get
            {
                return windowBackgroundOpacity;
            }
            set
            {
                windowBackgroundOpacity = value;
                OnPropertyChanged(nameof(WindowBackgroundOpacity));
            }
        }


        public InfoViewModel(
            SolidColorBrush windowBackground,
            double windowBackgroundOpacity)
        {
            WindowBackground = windowBackground;
            WindowBackgroundOpacity = windowBackgroundOpacity;
        }
    }

}
