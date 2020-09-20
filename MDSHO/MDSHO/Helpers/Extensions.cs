using MDSHO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Collections.ObjectModel;

namespace MDSHO.Helpers
{
    public static class Extensions
    {
        
        public static IEnumerable<ItemVM> FlattenItemVMs(this ItemVM root)
        {
            //https://stackoverflow.com/questions/7062882/searching-a-tree-using-linq

            var nodes = new Stack<ItemVM>(new[] { root });
            while (nodes.Any())
            {
                ItemVM node = nodes.Pop();
                yield return node;
                foreach (var n in node.ItemVMs) nodes.Push(n);
            }
        }

        /// <summary>
        /// Sort an ObservableCollection without braking the binding. CollectionChanged is raised per each move.
        /// <para>For sort ascending use it like: myObservableCollection.Sort((a, b) => { return a.Name.CompareTo(b.Name); });</para>
        /// <para>For sort descending use it like: myObservableCollection.Sort((a, b) => { return b.Name.CompareTo(a.Name); });</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="comparison"></param>
        public static void Sort<T>(this ObservableCollection<T> collection, Comparison<T> comparison)
        {
            // https://stackoverflow.com/questions/19112922/sort-observablecollectionstring-through-c-sharp

            var sortableList = new List<T>(collection);
            sortableList.Sort(comparison);

            for (int i = 0; i < sortableList.Count; i++)
            {
                collection.Move(collection.IndexOf(sortableList[i]), i);
            }
        }



        #region IMAGE CONVERTERS

        /*
        // https://joe-bq-wang.iteye.com/blog/1604603
        // you may refer to this page for details on how to convert from System.Drawing.Icon to System.Media.ImageSource  
        // http://stackoverflow.com/questions/1127647/convert-system-drawing-icon-to-system-media-imagesource  
        // https://stackoverflow.com/questions/2969821/display-icon-in-wpf-image
        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);
        public static BitmapSource ToBitmapSource(this Icon icon)
        {
            Bitmap bitmap = icon.ToBitmap();
            IntPtr hBitmap = bitmap.GetHbitmap();

            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            if (!DeleteObject(hBitmap))
            {
                throw new Win32Exception();
            }

            return bitmapSource;
        }
        */

        // https://stackoverflow.com/questions/1127647/convert-system-drawing-icon-to-system-media-imagesource
        public static BitmapSource ToBitmapSource(this Icon icon)
        {
            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(icon.Width, icon.Height));
            return bitmapSource;
        }
        
        
        public static ImageSource ToImageSource(this Icon icon)
        {
            ImageSource imageSource = icon.ToBitmapSource();
            return imageSource;
        }
        
        
        public static BitmapSource ToBitmapSource(this Bitmap bitmap)
        {
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);
            BitmapSource bitmapSource = BitmapSource.Create(bitmapData.Width, bitmapData.Height, bitmap.HorizontalResolution, bitmap.VerticalResolution, PixelFormats.Bgr32, null, bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);
            bitmap.UnlockBits(bitmapData);
            return bitmapSource;
        }
        
        public static BitmapSource ToBitmapSource(this Image image)
        {
            Bitmap bitmap = new Bitmap(image);
            return bitmap.ToBitmapSource();
        }


        #endregion



        public static void CenterToParentScreen(this Window window, Window parent)
        {
            if (parent == null)
            {
                // The call is coming from the taskbar context menu
                System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.PrimaryScreen;
                window.Left = (int)(screen.Bounds.Left + ((screen.Bounds.Width - window.Width) / 2));
                window.Top = (int)(screen.Bounds.Top + ((screen.Bounds.Height - window.Height) / 2));
            }
            else
            {
                // The call is coming from one of the windows
                System.Windows.Forms.Screen screen = Helper.GetScreenFromWindow(parent);
                window.Left = (screen.Bounds.Left + ((screen.Bounds.Width - window.Width) / 2));
                window.Top = (screen.Bounds.Top + ((screen.Bounds.Height - window.Height) / 2));
            }
        }

        #region COLOR CONVERTERS

        /// <summary>
        /// Convert a Color to int.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static int ToInt(this System.Windows.Media.Color colorToInt)
        {
            byte[] bytes = new byte[4];
            bytes[0] = colorToInt.B;
            bytes[1] = colorToInt.G;
            bytes[2] = colorToInt.R;
            bytes[3] = colorToInt.A;
            int intFromColor = BitConverter.ToInt32(bytes, 0);
            return intFromColor;
        }
        /// <summary>
        /// Convert a Color to SolidColorBrush.
        /// </summary>
        /// <param name="colorToSolidColorBrush"></param>
        /// <returns></returns>
        public static SolidColorBrush ToSolidColorBrush(this System.Windows.Media.Color colorToSolidColorBrush)
        {
            return new SolidColorBrush { Color = colorToSolidColorBrush };
        }
        /// <summary>
        /// Convert an int to Color.
        /// </summary>
        /// <param name="intFromColor"></param>
        /// <returns></returns>
        public static System.Windows.Media.Color ToColor(this int intToColor)
        {
            byte[] bytes = BitConverter.GetBytes(intToColor);
            System.Windows.Media.Color colorFromInt = new System.Windows.Media.Color()
            {
                B = bytes[0],
                G = bytes[1],
                R = bytes[2],
                A = bytes[3]
            };
            return colorFromInt;
        }

        #endregion








    }
}
