using MDSHO.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MDSHO.Native;



namespace MDSHO
{
    /// <summary>
    /// Interaction logic for BoxWindow.xaml
    /// </summary>
    public partial class BoxWindow : Window
    {
        public BoxWindow(BoxVM boxVM)
        {
            InitializeComponent();

            DataContext = boxVM;
        }





        #region Move & size the box

        // https://stackoverflow.com/questions/4474670/how-to-catch-the-ending-resize-window
        // https://msdn.microsoft.com/en-us/library/ms633573(v=VS.85).aspx
        // https://docs.microsoft.com/en-us/windows/desktop/winmsg/about-messages-and-message-queues#system_defined
        // HwndSource. Presents Windows Presentation Foundation (WPF) content in a Win32 window.
        private HwndSource hwndSource;
        private bool boxWasMoved;
        private bool boxWasResized;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            hwndSource = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            hwndSource.AddHook(new HwndSourceHook(WndProc));
        }
        // https://msdn.microsoft.com/en-us/library/ms633573(v=VS.85).aspx
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // Detect moving
            if (msg == WinMsg.WM_MOVING && !boxWasMoved)
            {
                // indicate that the user is moving and not resizing the window
                boxWasMoved = true;
            }
            // Detect sizing
            if (msg == WinMsg.WM_SIZING && !boxWasResized)
            {
                // indicate that the user is resizing and not moving the window
                boxWasResized = true;
            }
            // Detect move or size stopped
            if (msg == WinMsg.WM_EXITSIZEMOVE)
            {
                // check that this is the end of move and not resize operation
                if (boxWasMoved)
                {
                    // Set it back to false for the next move.
                    boxWasMoved = false;
                    // Moving stopped. Save the new position.
                    // TODO
                    MessageBox.Show("TODO \n ");
                    // SaveData.SaveShortcuts(false);
                }
                // check that this is the end of resize and not move operation
                if (boxWasResized)
                {
                    // Set it back to false for the next resize
                    boxWasResized = false;
                    // Sizing stopped. Save the new position.
                    // TODO
                    MessageBox.Show("TODO \n ");
                    // SaveData.SaveShortcuts(false);
                }
            }
            return IntPtr.Zero;
        }
        private void BoxTitle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        #endregion








        #region Functions used to change the background opacity from clicking on the bottom buttons of a box.

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                ((BoxVM)DataContext).InfoVM.BoxBgOpacity = ((Rectangle)sender).Fill.Opacity;
            }
        }
        private void Rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                ((BoxVM)DataContext).InfoVM.BoxBgOpacity = ((Rectangle)sender).Fill.Opacity;
            }
        }


        #endregion


        #region THEMES

        private void MenuItemDarkTheme_Click(object sender, RoutedEventArgs e)
        {
            //WindowItemVM WindowItemVM = (WindowItemVM)DataContext;
            //if (WindowItemVM != null)
            //{
            //    WindowInfoVM windowInfoVM = WindowItemVM.WindowInfoVM;
            //    if (windowInfoVM != null)
            //    {
            //        Themes.DarkTheme(windowInfoVM);
            //        SaveData.SaveShortcuts(false);
            //    }
            //}
        }
        private void MenuItemLightTheme_Click(object sender, RoutedEventArgs e)
        {
            //WindowItemVM WindowItemVM = (WindowItemVM)DataContext;
            //if (WindowItemVM != null)
            //{
            //    WindowInfoVM windowInfoVM = WindowItemVM.WindowInfoVM;
            //    if (windowInfoVM != null)
            //    {
            //        Themes.LightTheme(windowInfoVM);
            //        SaveData.SaveShortcuts(false);
            //    }
            //}
        }
        private void MenuItemCustomTheme_Click(object sender, RoutedEventArgs e)
        {
            CustomTheme();
        }

        private void CustomTheme()
        {
            InfoVM infoVM = ((BoxVM)DataContext).InfoVM;
            CustomThemeWindow customThemeWindow = new CustomThemeWindow(infoVM);
            customThemeWindow.ShowDialog();
        }

        #endregion

        private void MenuItemDarkTheme_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemLightTheme_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
