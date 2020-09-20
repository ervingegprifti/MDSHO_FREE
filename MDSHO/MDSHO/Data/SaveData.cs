using MDSHO.Helpers;
using Newtonsoft.Json;
using System.Windows;
using MDSHO.Models;
using MDSHO.ViewModels;
using System;
using System.Globalization;
using System.IO;

namespace MDSHO.Data
{
    public static class SaveData
    {
        public static void SaveShortcuts(bool saveNow)
        {
            ShortcutsVM shortcutsVM = ((App)Application.Current).DataContext;
            Shortcuts shortcuts = ContextHelper.VMToShortcuts(shortcutsVM);
            SaveShortcuts(shortcuts, saveNow);
        }
        public static void SaveShortcuts(Shortcuts shortcuts, bool saveNow)
        {
            string serializedShortcuts = JsonConvert.SerializeObject(shortcuts);
            SaveShortcuts(serializedShortcuts, saveNow);
        }
        public static void SaveShortcuts(string serializedShortcuts, bool saveNow)
        {
            if(saveNow)
            {
                // If saveNow = true then just save now. 
                // Save to file
                SaveShortcutsToFile(serializedShortcuts);
                // This is not used anymore. We use files to store the shortcuts data.
                // Save to settings
                // SaveShortcutsToSettings(serializedShortcuts);
            }
            else
            {
                // If saveNow = false then wait for the save request frequency interval to come before saving
                // Save every Constants.SAVE_REQUEST_FREQUENCY times
                ShortcutsVM shortcutsVM = ((App)Application.Current).DataContext;
                shortcutsVM.SaveRequestCounter += 1;
                if ((shortcutsVM.SaveRequestCounter % Constants.SAVE_REQUEST_FREQUENCY) == 0)
                {
                    // Save to file
                    SaveShortcutsToFile(serializedShortcuts);
                    // This is not used anymore. We use files to store the shortcuts data.
                    // Save to settings
                    // SaveShortcutsToSettings(serializedShortcuts);
                }
            }

            // TODO
            //MessageBox.Show("SaveShortcuts");
        }
        private static void SaveShortcutsToFile(string serializedShortcuts)
        {
            string shortcutsDataPath = Helper.GetShortcutsDataPath();
            DateTime dateTime = DateTime.Now;
            string shortcutsDataFileName = dateTime.ToString("yyyyMMdd_HHmmss_fff", CultureInfo.InvariantCulture);
            string shortcutsDataFilePath = Path.Combine(shortcutsDataPath, shortcutsDataFileName);
            FileIO.WriteFile(shortcutsDataFilePath, serializedShortcuts, true);
        }
        /*
        private static void SaveShortcutsToSettings(string serializedShortcuts)
        {
            // To use reading from settings, a string type setting with name ShortcutsSettings must be created in, RMC PROJECT -> Properties -> Settings 
            // Save shortcuts to settings
            Properties.Settings.Default.ShortcutsSettings = serializedShortcuts;
            Properties.Settings.Default.Save();
        }
        */
    }
}
