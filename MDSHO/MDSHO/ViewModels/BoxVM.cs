using MDSHO.Helpers;
using MDSHO.ViewModels.Commands;
using System;
using System.Windows;

namespace MDSHO.ViewModels
{
    public class BoxVM : BaseVM
    {
        public InfoVM InfoVM { get; set; }
        public RelayCommand ExitApplicationCommand { get; }
        public RelayCommand ShowAboutWindowCommand { get; }


        public BoxVM(InfoVM infoVM)
        {
            try
            {
                InfoVM = infoVM;
                ExitApplicationCommand = new RelayCommand(ExitApplicationCmd);
                ShowAboutWindowCommand = new RelayCommand(ShowAboutWindowCmd);
            }
            catch (Exception ex)
            {
                Error.Show(ex);
            }
        }


        private void ExitApplicationCmd(object parameter)
        {
            try
            {
                ((App)Application.Current).AppVM.ExitApplication(true);
            }
            catch (Exception ex)
            {
                Error.Show(ex);
            }
        }


        private void ShowAboutWindowCmd(object parameter)
        {
            try
            {
                ((App)Application.Current).AppVM.ShowAboutWindow();
            }
            catch (Exception ex)
            {
                Error.Show(ex);
            }
        }








    }



}
