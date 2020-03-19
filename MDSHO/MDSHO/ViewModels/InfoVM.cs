using System.Windows.Media;

namespace MDSHO.ViewModels
{
    public class InfoVM : BaseVM
    {
        private SolidColorBrush boxBg;
        private double boxBgOpacity;

        private string title;
        private SolidColorBrush windowBorderSolidColorBrush;
        private SolidColorBrush titleBackgroundSolidColorBrush;
        private double titleBackgroundOpacity;
        private SolidColorBrush titleTextSolidColorBrush;
        private SolidColorBrush windowTextSolidColorBrush;


        public SolidColorBrush BoxBg
        {
            get
            {
                return boxBg;
            }
            set
            {
                boxBg = value;
                OnPropertyChanged(nameof(BoxBg));
            }
        }
        public double BoxBgOpacity
        {
            get
            {
                return boxBgOpacity;
            }
            set
            {
                boxBgOpacity = value;
                OnPropertyChanged(nameof(BoxBgOpacity));
            }
        }


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

        public InfoVM(SolidColorBrush boxBg, double boxBgOpacity
            //string title,
            //SolidColorBrush windowBorderSolidColorBrush,
            //SolidColorBrush titleBackgroundSolidColorBrush,
            //double titleBackgroundOpacity,
            //SolidColorBrush titleTextSolidColorBrush,
            //SolidColorBrush windowTextSolidColorBrush

            )
        {
            BoxBg = boxBg;
            BoxBgOpacity = boxBgOpacity;

            //Title = title;
            //WindowBorderSolidColorBrush = windowBorderSolidColorBrush;
            //TitleBackgroundSolidColorBrush = titleBackgroundSolidColorBrush;
            //TitleBackgroundOpacity = titleBackgroundOpacity;
            //TitleTextSolidColorBrush = titleTextSolidColorBrush;
            //WindowTextSolidColorBrush = windowTextSolidColorBrush;
        }
    }

}
