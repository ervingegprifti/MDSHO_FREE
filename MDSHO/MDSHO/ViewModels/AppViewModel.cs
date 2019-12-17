using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace MDSHO.ViewModels
{
    public class AppViewModel : BaseViewModel
    {
        public ObservableCollection<BoxViewModel> BoxViewModels { get; set; } = new ObservableCollection<BoxViewModel>();

        public AppViewModel()
        {

        }


        public void ExitApplication(bool confirm)
        {
            ((App)Application.Current).ExitApplication(confirm);
        }

    }
}
