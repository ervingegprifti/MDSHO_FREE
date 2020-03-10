using MDSHO.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace MDSHO.ViewModels
{
    public class AppVM : BaseVM
    {
        public bool MustExitApplication { get; set; }


        public ObservableCollection<BoxVM> BoxVMs { get; set; } = new ObservableCollection<BoxVM>();


        /// <summary>
        /// Exit the application.
        /// </summary>
        /// <param name="confirm">If false then do not disturb the user with confirmation questions.</param>
        public void ExitApplication(bool confirm)
        {
            try
            {
                if (confirm)
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to exit application?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        MustExitApplication = true;
                        Application.Current.MainWindow.Close();
                    }
                }
                else
                {
                    MustExitApplication = true;
                    Application.Current.MainWindow.Close();
                }
            }
            catch (Exception ex)
            {
                Error.Show(ex);
            }
        }
































        public void ShowAboutWindow()
        {
            try
            {
                AboutWindow aboutWindow = new AboutWindow();
                aboutWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                Error.Show(ex);
            }
        }

    }
}
