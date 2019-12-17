using MDSHO.ViewModels.Commands;
using System;
using System.Windows;

namespace MDSHO.ViewModels
{
    public class BoxViewModel : BaseViewModel
    {
        public AppViewModel AppContext 
        { 
            get
            {
                return ((App)Application.Current).AppContext;
            }
        }
        public InfoViewModel InfoViewModel { get; set; }
        public RelayCommand ExitApplicationCommand { get; }

        public BoxViewModel(InfoViewModel infoViewModel)
        {
            try
            {
                InfoViewModel = infoViewModel;
                ExitApplicationCommand = new RelayCommand(ExitApplication);
            }
            catch (Exception)
            {
                // TODO
                // Error.ShowDialog(ex);
            }
        }


        private void ExitApplication(object parameter)
        {
            try
            {
                AppContext.ExitApplication(true);
            }
            catch (Exception ex)
            {
                // TODO
                // Error.ShowDialog(ex);
            }
        }


    }



}
