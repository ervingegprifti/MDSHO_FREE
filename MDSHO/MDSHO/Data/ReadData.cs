using MDSHO.Helpers;
using System.IO;
using System.Linq;
using MDSHO.Data;


namespace MDSHO.Data
{
    public static class ReadData
    {
        public static string ReadShortcuts()
        {
            // Read shortcuts from file
            return ReadShortcutsFromFile();
            // This is not used anymore. We use files to store the shortcuts data.
            // Read shortcuts from settings
            // return ReadShortcutsFromSettings();
        }
        private static string ReadShortcutsFromFile()
        {
            string returnValue = "";
            string shortcutsDataDirectoryPath = Helper.GetShortcutsDataPath();
            DirectoryInfo directoryInfo = new DirectoryInfo(shortcutsDataDirectoryPath);
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            if (fileInfos.Count() > 0)
            {
                // Get the latest written file
                // https://stackoverflow.com/questions/1179970/how-to-find-the-most-recent-file-in-a-directory-using-net-and-without-looping
                // FileInfo lastWrittenFile = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First();
                FileInfo lastWrittenFile = fileInfos.OrderByDescending(f => f.Name).First();
                if(lastWrittenFile != null)
                {
                    // Read the last written file
                    returnValue = FileIO.ReadFile(lastWrittenFile.FullName);                    
                }

                // After reading the last written file we need to delete older files and keep only the las 10 for example.
                // There is no need to use a wait here because we are not dependent on the DeleteOldFilesAsync results.
                FileIO.DeleteOldFilesAsync(shortcutsDataDirectoryPath, Constants.NR_OF_NEW_FILES_TO_KEEP);
            }
            return returnValue;
        }


        /*
        private static string ReadShortcutsFromSettings()
        {
            // To use reading from settings, a string type setting with name ShortcutsSettings must be created in, RMC PROJECT -> Properties -> Settings 
            // Read shortcuts from settings
            return Properties.Settings.Default.ShortcutsSettings;
        }
        */
    }
}
