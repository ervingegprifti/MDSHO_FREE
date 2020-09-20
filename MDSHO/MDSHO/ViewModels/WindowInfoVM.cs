using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MDSHO.ViewModels
{
    public class WindowInfoVM : BaseVM
    {
        // Private properties
        private string title;
        private SolidColorBrush windowBorderSolidColorBrush;
        private SolidColorBrush titleBackgroundSolidColorBrush;
        private double titleBackgroundOpacity;
        private SolidColorBrush titleTextSolidColorBrush;
        private SolidColorBrush windowBackgroundSolidColorBrush;
        private double windowBackgroundOpacity;
        private SolidColorBrush windowTextSolidColorBrush;
        
        // Public properties
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        public SolidColorBrush WindowBorderSolidColorBrush
        {
            get
            {
                return windowBorderSolidColorBrush;
            }
            set
            {
                windowBorderSolidColorBrush = value;
                OnPropertyChanged(nameof(WindowBorderSolidColorBrush));
            }
        }
        public SolidColorBrush TitleBackgroundSolidColorBrush
        {
            get
            {
                return titleBackgroundSolidColorBrush;
            }
            set
            {
                titleBackgroundSolidColorBrush = value;
                OnPropertyChanged(nameof(TitleBackgroundSolidColorBrush));
            }
        }
        public double TitleBackgroundOpacity
        {
            get
            {
                return titleBackgroundOpacity;
            }
            set
            {
                titleBackgroundOpacity = value;
                OnPropertyChanged(nameof(TitleBackgroundOpacity));
            }
        }
        public SolidColorBrush TitleTextSolidColorBrush
        {
            get
            {
                return titleTextSolidColorBrush;
            }
            set
            {
                titleTextSolidColorBrush = value;
                OnPropertyChanged(nameof(TitleTextSolidColorBrush));
            }
        }
        public SolidColorBrush WindowBackgroundSolidColorBrush
        {
            get
            {
                return windowBackgroundSolidColorBrush;
            }
            set
            {
                windowBackgroundSolidColorBrush = value;
                OnPropertyChanged(nameof(WindowBackgroundSolidColorBrush));
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
        public SolidColorBrush WindowTextSolidColorBrush
        {
            get
            {
                return windowTextSolidColorBrush;
            }
            set
            {
                windowTextSolidColorBrush = value;
                OnPropertyChanged(nameof(WindowTextSolidColorBrush));
            }
        }
        
        // The constructor
        public WindowInfoVM(string title, 
                                   SolidColorBrush windowBorderSolidColorBrush, 
                                   SolidColorBrush titleBackgroundSolidColorBrush,
                                   double titleBackgroundOpacity,
                                   SolidColorBrush titleTextSolidColorBrush,
                                   SolidColorBrush windowBackgroundSolidColorBrush, 
                                   double windowBackgroundOpacity,
                                   SolidColorBrush windowTextSolidColorBrush)
        {
            Title = title;
            WindowBorderSolidColorBrush = windowBorderSolidColorBrush;
            TitleBackgroundSolidColorBrush = titleBackgroundSolidColorBrush;
            TitleBackgroundOpacity = titleBackgroundOpacity;
            TitleTextSolidColorBrush = titleTextSolidColorBrush;
            WindowBackgroundSolidColorBrush = windowBackgroundSolidColorBrush;
            WindowBackgroundOpacity = windowBackgroundOpacity;
            WindowTextSolidColorBrush = windowTextSolidColorBrush;
        }
    }
}
