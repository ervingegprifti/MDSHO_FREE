using MDSHO.Helpers;
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
        public RelayCommand ShowAboutWindowCommand { get; }


        public BoxViewModel(InfoViewModel infoViewModel)
        {
            try
            {
                InfoViewModel = infoViewModel;
                ExitApplicationCommand = new RelayCommand(ExitApplication);
                ShowAboutWindowCommand = new RelayCommand(ShowAboutWindow);
            }
            catch (Exception ex)
            {
                Error.Show(ex);
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
                Error.Show(ex);
            }
        }
        private void ShowAboutWindow(object parameter)
        {
            try
            {
                AppContext.ShowAboutWindow();
            }
            catch (Exception ex)
            {
                Error.Show(ex);
            }
        }


    }



}
