using MDSHO.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MDSHO.Data
{
    public static class FileIO
    {
        #region WRITE FILE

        public static void WriteFile(string filePath, string content, bool writeAsync)
        {
            if(writeAsync)
            {
                // Write to a file asynchronously
                WriteFileAsync(filePath, content);
            }
            else

            {
                // Write to a file synchronously
                WriteFileSync(filePath, content);
            }
        }
        private static void WriteFileSync(string filePath, string content)
        {
            // https://dotnetcodr.com/2015/01/21/5-ways-to-write-to-a-file-with-c-net/
            using (StreamWriter streamWriter = File.CreateText(filePath))
            {
                streamWriter.Write(content);
            }
        }
        private static async void WriteFileAsync(string filePath, string content)
        {
            // https://dotnetcodr.com/2015/01/21/5-ways-to-write-to-a-file-with-c-net/
            using (StreamWriter streamWriter = File.CreateText(filePath))
            {
                await streamWriter.WriteAsync(content);
            }
        }

        #endregion
        #region READ FILE

        public static string ReadFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        #endregion
        #region DELETE FILE/s

        public static void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }
        public static async Task DeleteFileAsync(string filePath)
        {
            // C# Async / Await - Make your app more responsive and faster with asynchronous programming
            // https://www.youtube.com/watch?v=2moh18sh5p4&list=PLtcV6eGH8nwerH-iUXe93TNP0mWpdwDGP&index=8&t=1612s
            await Task.Run(() => File.Delete(filePath));
        }
        public static void DeleteOldFiles(string parentDirectory, int nrOfNewFilesToKeep)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(parentDirectory);
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            List<FileInfo> fileInfosOrderedDescending = new List<FileInfo>();
            if (fileInfos.Count() > 0)
            {
                // Sort fileInfos by descending
                // https://stackoverflow.com/questions/1179970/how-to-find-the-most-recent-file-in-a-directory-using-net-and-without-looping
                fileInfosOrderedDescending = fileInfos.OrderByDescending(f => f.Name).ToList();
            }

            // Delete the old files
            for (int i = 0; i < fileInfosOrderedDescending.Count(); i++)
            {
                // Since fileInfos are sorted by descending then we need to keep the first NR_OF_FILES_TO_KEEP and delete the rest
                if (i >= Constants.NR_OF_NEW_FILES_TO_KEEP)
                {
                    File.Delete(fileInfosOrderedDescending[i].FullName);
                }
            }
        }
        public static async Task DeleteOldFilesAsync(string parentDirectory, int nrOfNewFilesToKeep)
        {
            // C# Async / Await - Make your app more responsive and faster with asynchronous programming
            // https://www.youtube.com/watch?v=2moh18sh5p4&list=PLtcV6eGH8nwerH-iUXe93TNP0mWpdwDGP&index=8&t=1612s
            DirectoryInfo directoryInfo = new DirectoryInfo(parentDirectory);
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            List<FileInfo> fileInfosOrderedDescending = new List<FileInfo>();
            if (fileInfos.Count() > 0)
            {
                // Sort fileInfos by descending
                // https://stackoverflow.com/questions/1179970/how-to-find-the-most-recent-file-in-a-directory-using-net-and-without-looping
                fileInfosOrderedDescending = fileInfos.OrderByDescending(f => f.Name).ToList();
            }

            // Delete the old files
            for (int i = 0; i < fileInfosOrderedDescending.Count(); i++)
            {
                // Since fileInfos are sorted by descending then we need to keep the first NR_OF_FILES_TO_KEEP and delete the rest
                if (i >= Constants.NR_OF_NEW_FILES_TO_KEEP)
                {
                    await Task.Run(() => File.Delete(fileInfosOrderedDescending[i].FullName));
                }
            }
        }

        #endregion
    }
}
