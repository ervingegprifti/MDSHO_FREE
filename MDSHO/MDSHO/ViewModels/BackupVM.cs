using MDSHO.Data;
using MDSHO.Helpers;
using System;
using System.IO;
using System.Windows;
using MDSHO.ViewModels.Commands;
using System.Collections.ObjectModel;

namespace MDSHO.ViewModels
{
    public class BackupVM : BaseVM
    {
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public RelayCommand RestoreSpecificBackupCommand { get; }
        public RelayCommand DeleteSpecificBackupCommand { get; }

        public BackupVM(string fileName, string fileSize)
        {
            FileName = fileName;
            FileSize = fileSize;
            // Set the default values
            RestoreSpecificBackupCommand = new RelayCommand(RestoreSpecificBackup);
            DeleteSpecificBackupCommand = new RelayCommand(DeleteSpecificBackup);
        }

        private void RestoreSpecificBackup(object parameter)
        {
            string resultMessage = "";
            try
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Backup " + FileName + "\n" + "Are you sure you want to restore to this backup?\n\n" + "You will loose current windows & sortcuts.", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    BackupIO.RestoreSpecificBackup(FileName);
                    // Displaying the result to the user
                    resultMessage += "Backup " + FileName + "\n";
                    resultMessage += "restored successfully.";
                    MessageBox.Show(resultMessage, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void DeleteSpecificBackup(object parameter)
        {
            try
            {
                string confirmationMessage = "Backup " + FileName + ".\n";
                confirmationMessage += "Are you sure you want to delete this backup?\n\n";
                confirmationMessage += "This process cannot be undo.";
                MessageBoxResult messageBoxResult = MessageBox.Show(confirmationMessage, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(messageBoxResult == MessageBoxResult.Yes)
                {
                    BackupIO.DeleteSpecificBackup(FileName);
                    // Displaying the result to the user
                    string shortcutsBackupPath = Helper.GetShortcutsBackupPath();
                    string backupFilePath = Path.Combine(shortcutsBackupPath, FileName);
                    if (!File.Exists(backupFilePath))
                    {
                        // Refresh the datasource
                        ShortcutsVM shortcutsVM = ((App)Application.Current).DataContext;
                        shortcutsVM.BackupVMs = new ObservableCollection<BackupVM>();
                        shortcutsVM.BackupVMs = BackupIO.GetBackupVMs();
                    }
                }
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }

    }
}
