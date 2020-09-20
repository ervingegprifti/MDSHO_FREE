using MDSHO.Helpers;
using Newtonsoft.Json;
using System;
using System.Windows;
using MDSHO.Models;

namespace MDSHO.Data
{
    public static class ShortcutsData
    {
        public static Shortcuts GetShortcuts()
        {
            string serializedShortcuts = ReadData.ReadShortcuts();

            return GetShortcuts(serializedShortcuts);
        }
        private static Shortcuts GetShortcuts(string serializedShortcuts)
        {
            // The return value
            Shortcuts shortcuts = null;
            if (string.IsNullOrEmpty(serializedShortcuts))
            {
                // This is the first run of the application since the installation.
                shortcuts = NewData.NewShortcuts();
                SaveData.SaveShortcuts(shortcuts, true);
            }
            else
            {
                // This is not the first run of the application since the installation. 
                shortcuts = JsonConvert.DeserializeObject<Shortcuts>(serializedShortcuts);
            }
            return shortcuts;
        }












    }

}
