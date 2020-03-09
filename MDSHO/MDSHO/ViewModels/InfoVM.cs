using System.Windows.Media;

namespace MDSHO.ViewModels
{
    public class InfoVM : BaseVM
    {
        private SolidColorBrush boxBg;
        private double boxBgOpacity;


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


        public InfoVM(SolidColorBrush boxBg, double boxBgOpacity)
        {
            BoxBg = boxBg;
            BoxBgOpacity = boxBgOpacity;
        }
    }

}
