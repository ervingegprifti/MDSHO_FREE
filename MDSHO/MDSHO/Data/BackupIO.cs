using System;
using System.Linq;
using System.Collections.ObjectModel;
using MDSHO.ViewModels;
using MDSHO.Helpers;
using System.IO;
using System.Windows;
using MDSHO.Models;
using Newtonsoft.Json;
using System.Globalization;

namespace MDSHO.Data
{
    public static class BackupIO
    {
        /// <summary>
        /// Create a backup of the current windows & shortcuts.
        /// </summary>
        /// <returns></returns>
        public static string BackupShortcuts()
        {
            // Get the application context
            ShortcutsVM shortcutsVM = ((App)Application.Current).DataContext;
            Shortcuts shortcuts = ContextHelper.VMToShortcuts(shortcutsVM);
            string shortcutsSerialized = JsonConvert.SerializeObject(shortcuts);
            string shortcutsBackupPath = Helper.GetShortcutsBackupPath();
            DateTime dateTime = DateTime.Now;
            string backupFileName = dateTime.ToString("yyyyMMdd_HHmmss_fff", CultureInfo.InvariantCulture);
            string backupFilePath = Path.Combine(shortcutsBackupPath, backupFileName);
            FileIO.WriteFile(backupFilePath, shortcutsSerialized, false);
            FileInfo fileInfo = new FileInfo(backupFilePath);
            string fileSize = fileInfo.Length.ToString() + " bytes";
            // Update the shortcutsVM
            BackupVM backupVM = new BackupVM(backupFileName, fileSize);
            shortcutsVM.BackupVMs.Add(backupVM);
            shortcutsVM.BackupVMs.Sort((a, b) => { return b.FileName.CompareTo(a.FileName); });
            return backupFileName;
        }
        public static ObservableCollection<BackupVM> GetBackupVMs()
        {
            ObservableCollection<BackupVM> backupVMs = new ObservableCollection<BackupVM>();
            string shortcutsBackupPath = Helper.GetShortcutsBackupPath();
            DirectoryInfo directoryInfo = new DirectoryInfo(shortcutsBackupPath);
            // https://stackoverflow.com/questions/1179970/how-to-find-the-most-recent-file-in-a-directory-using-net-and-without-looping
            //IOrderedEnumerable<FileInfo> fileInfos = directory.GetFiles().OrderByDescending(f => f.LastWriteTime);
            IOrderedEnumerable<FileInfo> fileInfos = directoryInfo.GetFiles().OrderByDescending(f => f.Name);
            foreach (FileInfo fileInfo in fileInfos)
            {
                string fileName = fileInfo.Name;
                string fileSize = fileInfo.Length.ToString() + " bytes";
                BackupVM backupVM = new BackupVM(fileName, fileSize);
                backupVMs.Add(backupVM);
            }
            return backupVMs;
        }
        public static void DeleteSpecificBackup(string fileName)
        {
            // Get the application context
            ShortcutsVM shortcutsVM = ((App)Application.Current).DataContext;
            // Remove the backupVM from BackupVMs
            foreach (BackupVM backupVM in shortcutsVM.BackupVMs)
            {
                if (backupVM.FileName.Equals(fileName, StringComparison.InvariantCultureIgnoreCase))
                {
                    shortcutsVM.BackupVMs.Remove(backupVM);
                    break;
                }
            }
            // Remove the physical backup file
            string shortcutsBackupPath = Helper.GetShortcutsBackupPath();
            string backupFilePath = Path.Combine(shortcutsBackupPath, fileName);
            FileIO.DeleteFile(backupFilePath);
        }
        public static string RestoreLastBackup()
        {
            string returnValue = "";
            string shortcutsBackupDirectoryPath = Helper.GetShortcutsBackupPath();
            DirectoryInfo directoryInfo = new DirectoryInfo(shortcutsBackupDirectoryPath);
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            if (fileInfos.Count() > 0)
            {
                // Get the latest written file
                // https://stackoverflow.com/questions/1179970/how-to-find-the-most-recent-file-in-a-directory-using-net-and-without-looping
                // FileInfo lastWrittenFile = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First();
                FileInfo lastWrittenFile = fileInfos.OrderByDescending(f => f.Name).First();
                if (lastWrittenFile != null)
                {
                    // Read the last written file
                    string serializedShortcuts = FileIO.ReadFile(lastWrittenFile.FullName);
                    // First we set current shortcuts
                    SaveData.SaveShortcuts(serializedShortcuts, true);
                    // Then we do a restore based on the last saved shortcutsData
                    RestoreBackup();
                    // We return the path of the backup restored
                    returnValue = lastWrittenFile.Name;
                }
            }
            return returnValue;
        }
        public static void RestoreSpecificBackup(string fileName)
        {
            string shortcutsBackupPath = Helper.GetShortcutsBackupPath();
            string filePath = Path.Combine(shortcutsBackupPath, fileName);
            if (File.Exists(filePath))
            {
                // Read the specific file
                string serializedShortcuts = FileIO.ReadFile(filePath);
                // Set current shortcuts
                SaveData.SaveShortcuts(serializedShortcuts, true);
                RestoreBackup();
            }
        }
        private static void RestoreBackup()
        {
            // Before doing restore, close all windows
            ShortcutsVM shortcutsVM = ((App)Application.Current).DataContext;
            // Loop through all windows and close them
            foreach (WindowItemVM windowItemVM in shortcutsVM.WindowItemVMs)
            {
                // Loop through all the window clones and close them
                foreach (WindowItemVM windowItemVMClone in windowItemVM.WindowItemVMClones)
                {
                    Window windowClone = Helper.GetWindowFromWindowItemVM(windowItemVMClone);
                    windowClone.Close();
                }
                Window window = Helper.GetWindowFromWindowItemVM(windowItemVM);
                window.Close();
            }
            ((App)Application.Current).StartLife();
        }
    }
}
