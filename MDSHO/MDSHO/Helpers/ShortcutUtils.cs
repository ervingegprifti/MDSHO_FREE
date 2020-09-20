using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using IWshRuntimeLibrary;
using MDSHO.Native;
using MDSHO.ViewModels;

namespace MDSHO.Helpers
{
    public static class ShortcutUtils
    {
        // LNK (file shortcuts), URL (Internet shortcuts), PIF (DOS shortcuts)
        // How to Show File Extensions of Shortcuts (LNK, URL, PIF) in Windows Explorer?
        // https://www.askvg.com/tip-how-to-show-file-extensions-of-shortcuts-lnk-url-pif-in-windows-explorer/

        public static void GreateShortcutLnk(string shortcutLnkFileName, string targetPath)
        {
            // Use it like this:
            // string shortcutLnkTargetFilePath = @"C:\Users\USERNANE\Desktop\ttt.txt";
            // string shortcutLnkFileName = Test.CreateGuid();
            // Test.GreateShortcutLnk(shortcutLnkFileName, shortcutLnkTargetFilePath);

            // So the issues is that the COM interface requires admin access, which your end users may not have. If you know they have access then it's not an issue. 
            // However, in cases where your users do not have admin access, the WSH with Dyanmics approach below works. 
            // https://stackoverflow.com/questions/2934420/why-do-i-get-e-accessdenied-when-reading-public-shortcuts-through-shell32
            // https://stackoverflow.com/questions/2818179/how-do-i-force-my-net-application-to-run-as-administrator/2818776#2818776

            // Implementation of IWshRuntimeLibrary to create the shortcut
            // https://docs.microsoft.com/en-us/previous-versions/windows/internet-explorer/ie-developer/windows-scripting/xsy6k3ys%28v=vs.84%29
            // https://bytescout.com/blog/create-shortcuts-in-c-and-vbnet.html
            // https://stackoverflow.com/questions/4897655/create-a-shortcut-on-desktop
            // https://books.google.al/books?id=gchk6fz65WYC&pg=PA518&lpg=PA518&dq=iwshshortcut+msdn&source=bl&ots=WQcHHwNbaj&sig=ACfU3U195Tyu-VL1RozOOClatYnfJAmFcQ&hl=en&sa=X&ved=2ahUKEwixw8Llh83gAhVJEVAKHeKHC68Q6AEwBHoECAIQAQ#v=onepage&q=iwshshortcut%20msdn&f=false
            // http://www.wengkien.com/2009/01/08/reading-and-creating-shortcuts-information/

            string shortcutsPath = Helper.GetShortcutsPath();
            string shortcutLnkFilePath = Path.Combine(shortcutsPath, shortcutLnkFileName + Constants.EXT_LNK);

            if (System.IO.File.Exists(shortcutLnkFilePath))
            {
                // The shortcut is already created.
                // No need to create the shortcut.
                return;
            }

            // WshShell Object
            // Provides access to the native Windows shell.
            // You create a WshShell object whenever you want to run a program locally, manipulate the contents of the registry, create a shortcut, or access a system folder. 
            // The WshShell object provides the Environment collection. This collection allows you to handle environmental variables (such as WINDIR, PATH, or PROMPT).
            // https://docs.microsoft.com/en-us/previous-versions/windows/internet-explorer/ie-developer/windows-scripting/aew9yb99%28v%3dvs.84%29
            WshShell wshShell = new WshShell();

            // CreateShortcut Method
            // Creates a new shortcut, or opens an existing shortcut.
            // The CreateShortcut method returns either a WshShortcut object or a WshURLShortcut object. Simply calling the CreateShortcut method does not result in the creation of a shortcut. 
            // The shortcut object and changes you may have made to it are stored in memory until you save it to disk with the Save method.
            // https://docs.microsoft.com/en-us/previous-versions/windows/internet-explorer/ie-developer/windows-scripting/xsy6k3ys%28v=vs.84%29
            // WshShortcut Object
            // Allows you to create a shortcut programmatically.
            // https://docs.microsoft.com/en-us/previous-versions/windows/internet-explorer/ie-developer/windows-scripting/xk6kst2k%28v%3dvs.84%29
            // IWshShortcut iWshShortcut = (IWshShortcut)wshShell.CreateShortcut(shortcutLnkFilePath);
            WshShortcut wshShortcut = (WshShortcut)wshShell.CreateShortcut(shortcutLnkFilePath);

            // Arguments Property (WScript Object)
            // Returns the WshArguments object (a collection of arguments).
            // The arguments used when executing the exe. The Arguments property contains the WshArguments object (a collection of arguments). 
            // Use a zero-based index to retrieve individual arguments from this collection.
            // Example:
            // When Target is "c:\windows\notepad.exe" then Arguments could be "c:\windows\text.txt". So it is telling to open text.txt with notepad.
            // https://docs.microsoft.com/en-us/previous-versions/windows/internet-explorer/ie-developer/windows-scripting/z2b05k8s%28v%3dvs.84%29
            // wshShortcut.Arguments = "";
            // string arguments = wshShortcut.Arguments;

            // Description Property (Windows Script Host)
            // Returns a shortcut's description. 
            // The Description property contains a string value describing a shortcut.
            // https://docs.microsoft.com/en-us/previous-versions/windows/internet-explorer/ie-developer/windows-scripting/ybdhh477%28v%3dvs.84%29
            // wshShortcut.Description = "";
            // string description = wshShortcut.Description;

            // FullName Property (WshShortcut Object)
            // Returns the fully qualified path of the shortcut object's target.
            // The FullName property contains a read-only string value indicating the fully qualified path to the shortcut's target.
            // https://docs.microsoft.com/en-us/previous-versions/windows/internet-explorer/ie-developer/windows-scripting/7c7x465d%28v%3dvs.84%29
            // string fullName = wshShortcut.FullName

            // Hotkey Property
            // Assigns a key-combination to a shortcut, or identifies the key-combination assigned to a shortcut.
            // Example:
            // "Ctrl+Alt+e";
            // https://docs.microsoft.com/en-us/previous-versions/windows/internet-explorer/ie-developer/windows-scripting/3zb1shc6%28v%3dvs.84%29
            // wshShortcut.Hotkey = "";
            // string hotkey = wshShortcut.Hotkey;

            // IconLocation Property
            // Assigns an icon to a shortcut, or identifies the icon assigned to a shortcut.
            // Example:
            // "notepad.exe, 0";  // Zero is the index
            // https://docs.microsoft.com/en-us/previous-versions/windows/internet-explorer/ie-developer/windows-scripting/3s9bx7at%28v%3dvs.84%29
            // wshShortcut.IconLocation = "";
            // string iconLocation = wshShortcut.IconLocation;

            // RelativePath Property
            // Assigns a relative path to a shortcut, or identifies the relative path of a shortcut.
            // Example:
            // If in CreateShortcut only the link name is specified like "linkname.lnk" then we need the RelativePath to be specified like "C:\"
            // https://docs.microsoft.com/en-us/previous-versions/windows/internet-explorer/ie-developer/windows-scripting/85hy4580%28v%3dvs.84%29
            // wshShortcut.RelativePath = "";

            // TargetPath Property
            // The path to the shortcut's executable.
            // This property is for the shortcut's target path only. Any arguments to the shortcut must be placed in the Argument's property.
            wshShortcut.TargetPath = targetPath;
            // string targetPath = wshShortcut.TargetPath;

            // WindowStyle Property
            // Assigns a window style to a shortcut, or identifies the type of window style used by a shortcut.
            // The WindowStyle property returns an integer.
            // The available settings for intWindowStyle:
            // 1 Activates and displays a window.If the window is minimized or maximized, the system restores it to its original size and position.
            // 3 Activates the window and displays it as a maximized window.
            // 7 Minimizes the window and activates the next top-level window. 
            wshShortcut.WindowStyle = 1;
            // int windowStyle = wshShortcut.WindowStyle;

            // WorkingDirectory Property (Windows Script Host)
            // Assign a working directory to a shortcut, or identifies the working directory used by a shortcut.
            // Is the directory where the shortcut is placed. If you miss it then unexpected result might hapen.
            // https://docs.microsoft.com/en-us/previous-versions/windows/internet-explorer/ie-developer/windows-scripting/ae0a4aee%28v%3dvs.84%29
            wshShortcut.WorkingDirectory = shortcutsPath;
            // string WorkingDirectory = wshShortcut.WorkingDirectory;

            // Save Method (Windows Script Host)
            // Saves a shortcut object to disk.
            // After using the CreateShortcut method to create a shortcut object and set the shortcut object's properties, the Save method must be used to save the shortcut object to disk. 
            // The Save method uses the information in the shortcut object's FullName property to determine where to save the shortcut object on a disk. 
            // You can only create shortcuts to system objects. This includes files, directories, and drives (but does not include printer links or scheduled tasks).
            // https://docs.microsoft.com/en-us/previous-versions/windows/internet-explorer/ie-developer/windows-scripting/k5x59zft%28v%3dvs.84%29
            wshShortcut.Save();
        }
        public static void GreateShortcutUrl(string shortcutUrlFileName, string targetUrl)
        {
            // https://www.c-sharpcorner.com/forums/create-url-shortcut-on-client-desktop

            string shortcutsDirectoryPath = Helper.GetShortcutsPath();
            string shortcutUrlFilePath = Path.Combine(shortcutsDirectoryPath, shortcutUrlFileName + Constants.EXT_URL);

            if (System.IO.File.Exists(shortcutUrlFilePath))
            {
                // The shortcut is already created.
                // No need to create the shortcut.
                return;
            }

            using (StreamWriter writer = new StreamWriter(shortcutUrlFilePath))
            {
                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URL=" + targetUrl);
                writer.WriteLine("IDList=");
                writer.WriteLine("HotKey="); // 0
                writer.WriteLine("IconFile="); // .ico file
                writer.WriteLine("IconIndex="); // 0
                writer.Flush();
            }
        }
        public static void DeleteShortcuts(ItemVM itemVMToDelete)
        {
            
            string shortcutsDirectoryPath = Helper.GetShortcutsPath();
            // Delete physical file/s first
            if (itemVMToDelete.IsGroup)
            {
                foreach (ItemVM subItemVMToDelete in itemVMToDelete.ItemVMs)
                {
                    string shortcutFileName = subItemVMToDelete.Guid;
                    string ext = subItemVMToDelete.Ext;
                    string shortcutFilePath = Path.Combine(shortcutsDirectoryPath, shortcutFileName + ext);
                    System.IO.File.Delete(shortcutFilePath);
                    // This should be recoursive
                    DeleteShortcuts(subItemVMToDelete);
                }
            }
            else
            {
                string shortcutFileName = itemVMToDelete.Guid;
                string ext = itemVMToDelete.Ext;
                string shortcutFilePath = Path.Combine(shortcutsDirectoryPath, shortcutFileName + ext);
                System.IO.File.Delete(shortcutFilePath);
            }
        }
        public static BitmapSource GetShortcutIconBitmapSource(string shortcutFilePath)
        {
            // https://www.brad-smith.info/blog/archives/164

            BitmapSource bitmapSource = null;

            /*
            // Just in case we might need to implement this
            string ext = Path.GetExtension(shortcutFilePath);
            if(ext.Equals(Constants.EXT_URL, StringComparison.InvariantCultureIgnoreCase))
            {
                // Uri uriLink = new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "Images/" + "link.png", UriKind.Absolute);
                Uri uriLink = new Uri(@"pack://application:,,,/Images/globe.png");
                bitmapSource = new BitmapImage(uriLink);
                return bitmapSource;
            }
            */

            /*
            // This method works only on files, not on folders.
            // This returns an icon with size of 32x32 pixels
            using (Icon icon = Icon.ExtractAssociatedIcon(shortcutFilePath)) // TODO check if there is a function for 16x16
            {
                bitmapSource = icon?.ToBitmapSource();
            }            
            */

            /*
            // This is a native method and might requaire user previliges
            // This returns an icon with size of 32x32 pixels
            using (Icon icon = ShellIcon.GetLargeIcon(shortcutFilePath))
            {
                bitmapSource = icon?.ToBitmapSource();
            }
            */

            
            // *** Use this for best results *** 
            // This is a native method and might requaire user previliges
            // This returns an icon with size of 16x16 pixels
            using (Icon icon = ShellIcon.GetSmallIcon(shortcutFilePath))
            {
                bitmapSource = icon?.ToBitmapSource();
            }
            

            /*
            // Investigate SmallBitmapSource gives size of 32x32 pixels
            // This is a native method and might requaire user previliges
            // bitmapSource = Microsoft.WindowsAPICodePack.Shell.ShellObject.FromParsingName(shortcutFilePath).Thumbnail.SmallBitmapSource;
            */



            return bitmapSource;
        }

    }
}


