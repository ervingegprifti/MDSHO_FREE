using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MDSHO.Native
{
    // http://pinvoke.net/default.aspx/shell32.SHGetFileInfo
    // https://www.brad-smith.info/blog/archives/164

    public static class ShellIcon
    {
        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_LARGEICON = 0x0; // Large icon 32x32 pixels
        private const uint SHGFI_SMALLICON = 0x1; // Small icon 16x16 pixels

        [StructLayout(LayoutKind.Sequential)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        [DllImport("shell32.dll")]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

        [DllImport("User32.dll")]
        private static extern int DestroyIcon(IntPtr hIcon);

        private static Icon GetIcon(string fileName, uint flags)
        {
            SHFILEINFO shFileInfo = new SHFILEINFO();
            // The handle to the system image list
            IntPtr hImage = SHGetFileInfo(fileName, 0, ref shFileInfo, (uint)Marshal.SizeOf(shFileInfo), SHGFI_ICON | flags);
            // Copy (clone) the returned icon to a new object, thus allowing us to call DestroyIcon immediately
            Icon icon = (Icon)Icon.FromHandle(shFileInfo.hIcon).Clone();
            // Cleanup
            DestroyIcon(shFileInfo.hIcon);
            return icon;
        }

        public static Icon GetSmallIcon(string fileName)
        {
            return GetIcon(fileName, SHGFI_SMALLICON);
        }

        public static Icon GetLargeIcon(string fileName)
        {
            return GetIcon(fileName, SHGFI_LARGEICON);
        }

    }
}
