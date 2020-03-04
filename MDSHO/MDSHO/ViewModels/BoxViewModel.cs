using MDSHO.Helpers;
using MDSHO.ViewModels.Commands;
using System;
using System.Windows;

namespace MDSHO.ViewModels
{
    public class BoxViewModel : BaseViewModel
    {
        private AppViewModel appViewModel;
        public InfoViewModel InfoViewModel { get; set; }
        public RelayCommand ExitApplicationCommand { get; }
        public RelayCommand ShowAboutWindowCommand { get; }


        public BoxViewModel(InfoViewModel infoViewModel)
        {
            try
            {
                appViewModel = ((App)Application.Current).AppViewModel;
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
                appViewModel.ExitApplication(true);
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
                appViewModel.ShowAboutWindow();
            }
            catch (Exception ex)
            {
                Error.Show(ex);
            }
        }


    }



}
