using MDSHO.Helpers;
using MDSHO.ViewModels.Commands;
using System;
using System.Windows;

namespace MDSHO.ViewModels
{
    public class BoxVM : BaseVM
    {
        private AppVM appVM;
        public InfoVM InfoVM { get; set; }
        public RelayCommand ExitApplicationCommand { get; }
        public RelayCommand ShowAboutWindowCommand { get; }


        public BoxVM(InfoVM infoVM)
        {
            try
            {
                appVM = ((App)Application.Current).AppVM;
                InfoVM = infoVM;
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
                appVM.ExitApplication(true);
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
                appVM.ShowAboutWindow();
            }
            catch (Exception ex)
            {
                Error.Show(ex);
            }
        }


    }



}
