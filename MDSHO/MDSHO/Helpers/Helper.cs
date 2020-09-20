using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Media;
using Newtonsoft.Json;
using MDSHO.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Reflection;

namespace MDSHO.Helpers
{
    public class Helper
    {
        #region GET PATHS

        private static string GetLocalApplicationDataPath()
        {
            // https://www.codeproject.com/Tips/370232/Where-should-I-store-my-data
            // C:\Users\UserName\AppData\Local
            // LocalApplicationData (applies to current user - local only). No need for Administrator access to write on that folder.
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }
        public static string GetShortcutsPath()
        {
#if (DEBUG)
            string path = Path.Combine(GetLocalApplicationDataPath(), "MDSHO", "DebugMDSHO");
#endif
#if (!DEBUG)
            string path = Path.Combine(GetLocalApplicationDataPath(), "MDSHO", "MDSHO");
#endif
      if (Directory.Exists(path))
            {
                return path;
            }
            else
            {
                // If directory does not exists then it is created. If it exists then nothing is done.
                DirectoryInfo directoryInfo = Directory.CreateDirectory(path);
                return directoryInfo.FullName;
            }
        }
        public static string GetShortcutsDataPath()
        {
#if (DEBUG)
            string path = Path.Combine(GetLocalApplicationDataPath(), "MDSHO", "DebugMDSHOData");
#endif
#if (!DEBUG)
            string path = Path.Combine(GetLocalApplicationDataPath(), "MDSHO", "MDSHOData");
#endif

      if (Directory.Exists(path))
            {
                return path;
            }
            else
            {
                // If directory does not exists then it is created. If it exists then nothing is done.
                DirectoryInfo directoryInfo = Directory.CreateDirectory(path);
                return directoryInfo.FullName;
            }
        }
        public static string GetShortcutsBackupPath()
        {
#if (DEBUG)
            string path = Path.Combine(GetLocalApplicationDataPath(), "MDSHO", "DebugMDSHOBackups");
#endif
#if (!DEBUG)
            string path = Path.Combine(GetLocalApplicationDataPath(), "MDSHO", "MDSHOBackups");
#endif

      if (Directory.Exists(path))
            {
                return path;
            }
            else
            {
                // If directory does not exists then it is created. If it exists then nothing is done.
                DirectoryInfo directoryInfo = Directory.CreateDirectory(path);
                return directoryInfo.FullName;
            }
        }




#endregion



        public static ObservableCollection<WindowItemVM> GetWindowItemVMClones(WindowItemVM windowItemVMToClose)
        {
            // Get the application context
            ShortcutsVM shortcutsVM = ((App)Application.Current).DataContext;
            foreach (WindowItemVM windowItemVM in shortcutsVM.WindowItemVMs)
            {
                if (windowItemVM.WindowItemVMClones != null)
                {
                    foreach (WindowItemVM windowItemVMClone in windowItemVM.WindowItemVMClones)
                    {
                        if (windowItemVMClone == windowItemVMToClose)
                        {
                            return windowItemVM.WindowItemVMClones;
                        }
                    }
                }
            }
            return null;
        }
        public static Window GetWindowFromWindowItemVM(WindowItemVM windowItemVM)
        {
            foreach (Window window in ((App)Application.Current).Windows)
            {
                if (window.DataContext is WindowItemVM)
                {
                    if ((WindowItemVM)window.DataContext == windowItemVM)
                    {
                        return window;
                    }
                }
            }
            return null;
        }
        public static System.Windows.Forms.Screen GetScreenFromWindow(Window window)
        {
            WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);
            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.FromHandle(windowInteropHelper.Handle);
            return screen;
        }



        // WPF - Get an Image Resource and Convert to System.Drawing.Image
        // https://stackoverflow.com/questions/32486679/how-to-get-a-system-drawing-image-object-from-image-which-build-action-is-marked
        // https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-4.0/aa970069(v=vs.100)
        public static System.Drawing.Image GetImageFromImages(string imageName)
        {
            Uri uri = new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "Images/" + imageName, UriKind.Absolute);
            BitmapImage bitmapImage = new BitmapImage(uri);

            PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
            pngBitmapEncoder.Frames.Add(BitmapFrame.Create((BitmapImage)bitmapImage));
            MemoryStream memoryStream = new MemoryStream();
            pngBitmapEncoder.Save(memoryStream);
            memoryStream.Flush();
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(memoryStream);

            return image;
        }






    }

}
