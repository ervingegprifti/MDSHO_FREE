using MDSHO.ViewModels;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using MDSHO.Helpers;
using MDSHO.Data;

// TreeView, data binding and multiple templates
// https://www.wpf-tutorial.com/treeview-control/treeview-data-binding-multiple-templates/
// C# WPF UI Tutorials: 03 - View Model MVVM Basics
// https://www.youtube.com/watch?v=U2ZvZwDZmJU&t=205s

namespace MDSHO
{
    public partial class ShortcutsWindow : Window
    {
        // We use this for those controls that do not have a click event handeler.
        // In the mouse doun click and in the mowse up click clickedObjectHashCode must be the same for a click to be a valid one.
        private int? clickedObjectHashCode = null;
        int lastWindowWidth = 0;
        int lastWindowHeight = 0;
        int lastWindowLeft = 0;
        int lastWindowTop = 0;

        public ShortcutsWindow()
        {
            InitializeComponent();
        }

        #region RENAME WINDOW TITTLE

        private void menuItemRenameWindowTitle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                textBlockTitle.Visibility = Visibility.Hidden;
                textBoxTitle.Visibility = Visibility.Visible;
                // Set the cursor to textBox
                Keyboard.Focus(textBoxTitle);
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void textBoxTitle_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    FocusManager.SetFocusedElement(this, TreeViewShortcuts);
                    // OR
                    // TreeViewShortcuts.Focus();                
                }
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void textBoxTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                textBlockTitle.Visibility = Visibility.Visible;
                textBoxTitle.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }

        #endregion
        
        #region MOVE & SIZE WINDOW

        // https://stackoverflow.com/questions/4474670/how-to-catch-the-ending-resize-window
        // https://msdn.microsoft.com/en-us/library/ms633573(v=VS.85).aspx
        // https://docs.microsoft.com/en-us/windows/desktop/winmsg/about-messages-and-message-queues#system_defined
        // HwndSource. Presents Windows Presentation Foundation (WPF) content in a Win32 window.
        private HwndSource hwndSource;
        // WM_MOVING. Sent to a window that the user is moving.
        // https://docs.microsoft.com/en-us/windows/desktop/winmsg/wm-moving
        private const int WM_MOVING = 0x0216;
        // WM_SIZING. Sent to a window that the user is resizing. 
        // https://docs.microsoft.com/en-us/windows/desktop/winmsg/wm-sizing
        private const int WM_SIZING = 0x214;
        // WM_EXITSIZEMOVE. Sent one time to a window, after it has exited the moving or sizing modal loop.
        // https://docs.microsoft.com/en-us/windows/desktop/winmsg/wm-exitsizemove
        private const int WM_EXITSIZEMOVE = 0x232;
        // 
        private bool windowWasMoved = false;
        private bool windowWasResized = false;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                hwndSource = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
                hwndSource.AddHook(new HwndSourceHook(WndProc));
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        // https://msdn.microsoft.com/en-us/library/ms633573(v=VS.85).aspx
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // Detect moving
            if (msg == WM_MOVING)
            {
                if (windowWasMoved == false)
                {
                    // indicate that the user is moving and not resizing the window
                    windowWasMoved = true;
                }
            }
            // Detect sizing
            if (msg == WM_SIZING)
            {
                if (windowWasResized == false)
                {
                    // indicate that the user is resizing and not moving the window
                    windowWasResized = true;
                }
            }
            // Detect move or size stopped
            if (msg == WM_EXITSIZEMOVE)
            {
                // check that this is the end of move and not resize operation          
                if (windowWasMoved == true)
                {
                    MoveWindowStoped();
                    // set it back to false for the next move
                    windowWasMoved = false;
                }
                // check that this is the end of resize and not move operation          
                if (windowWasResized == true)
                {
                    SizeWindowStoped();
                    // set it back to false for the next resize
                    windowWasResized = false;
                }
            }

            return IntPtr.Zero;
        }
        private void gridHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    if(e.ButtonState == MouseButtonState.Pressed)
                    {
                        this.DragMove();
                    }
                }
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void MoveWindowStoped()
        {
            try
            {
                SaveData.SaveShortcuts(false);
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void SizeWindowStoped()
        {
            try
            {
                SaveData.SaveShortcuts(false);
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }

        #endregion
        
        #region RENAME ITEM

        private void TextBoxName_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                TextBox textBox = (TextBox)sender;
                if (textBox != null)
                {
                    if (textBox.IsVisible)
                    {
                        // Set the cursor to textBox
                        Keyboard.Focus(textBox);
                    }
                }
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void TextBoxName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    FocusManager.SetFocusedElement(this, TreeViewShortcuts);
                    // OR
                    // TreeViewShortcuts.Focus(); 
                }
                if (e.Key == Key.Escape)
                {
                    // User clicked the escape button. Restore the old value.

                    TextBox textBoxName = (TextBox)sender;
                    if(textBoxName != null)
                    {
                        Grid gridName = (Grid)textBoxName.Parent;
                        if (gridName != null)
                        {
                            TextBlock textBlockName = (TextBlock)gridName.FindName("TextBlockName");
                            if (textBlockName != null)
                            {
                                // Restore the old value
                                textBoxName.Text = textBlockName.Text;
                                textBlockName.Visibility = Visibility.Visible;
                                textBoxName.Visibility = Visibility.Hidden;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void TextBoxName_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBox textBoxName = (TextBox)sender;
                if(textBoxName != null)
                {
                    Grid gridName = (Grid)textBoxName.Parent;
                    if (gridName != null)
                    {
                        TextBlock textBlockName = (TextBlock)gridName.FindName("TextBlockName");
                        if (textBlockName != null)
                        {
                            textBlockName.Visibility = Visibility.Visible;
                            textBoxName.Visibility = Visibility.Hidden;
                        }
                    }
                }
                SaveData.SaveShortcuts(false);
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }

        #endregion
        
        #region SET WINDOW BACKGROUND

        private void rectangleColors_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                System.Windows.Shapes.Rectangle rectangleColor = (System.Windows.Shapes.Rectangle)sender;
                if (DataContext != null)
                {
                    WindowItemVM windowItemVM = (WindowItemVM)DataContext;
                    windowItemVM.WindowInfoVM.WindowBackgroundOpacity = rectangleColor.Fill.Opacity;
                }
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }

        #endregion
        
        #region TREEVIEW ITEM CLICKED

        private void StackPanelItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                StackPanel stackPanelItem = (StackPanel)sender;
                if (stackPanelItem != null)
                {
                    ItemVM itemVM = (ItemVM)stackPanelItem.DataContext;
                    if (itemVM != null)
                    {
                        // Detect double clicks
                        if (e.ClickCount == 2 && !itemVM.IsGroup)
                        {
                            clickedObjectHashCode = stackPanelItem.GetHashCode();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void StackPanelItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                StackPanel stackPanelItem = (StackPanel)sender;
                if (stackPanelItem != null)
                {
                    if (clickedObjectHashCode.HasValue)
                    {
                        if (clickedObjectHashCode.Value == stackPanelItem.GetHashCode())
                        {
                            // We have a click
                            ItemVM itemVM = (ItemVM)(stackPanelItem.DataContext);
                            if (itemVM != null)
                            {
                                if (!itemVM.IsGroup)
                                {
                                    try
                                    {
                                        // Working in general
                                        //Process.Start(itemVM.Target);

                                        //// Working in general
                                        //// https://stackoverflow.com/questions/5255086/when-do-we-need-to-set-useshellexecute-to-true
                                        //using (Process process = new Process())
                                        //{
                                        //    process.StartInfo.UseShellExecute = true;
                                        //    process.StartInfo.FileName = target;
                                        //    // process.StartInfo.Arguments = itemVM.Target;
                                        //    bool result = process.Start();
                                        //}

                                        // Working somehow
                                        // Open Control Panel Items using Shell (COM)
                                        // http://tech-giant.blogspot.com/2009/07/open-control-panel-items-using-shell.html
                                        // Shell shell = new Shell();
                                        // shell.ControlPanelItem("appwiz.cpl");
                                        // shell.ControlPanelItem(itemVM.Target);

                                        // Working best for third party control panel items
                                        // Process.Start("explorer.exe", itemVM.Target);

                                        // Working best for third party control panel items
                                        // Process.Start("explorer", itemVM.Target);

                                        /*
                                        // Working best for third party control panel items
                                        // https://stackoverflow.com/questions/5255086/when-do-we-need-to-set-useshellexecute-to-true
                                        using (Process process = new Process())
                                        {
                                            process.StartInfo.UseShellExecute = true;
                                            process.StartInfo.ErrorDialog = true;
                                            process.StartInfo.FileName = "explorer.exe";
                                            process.StartInfo.Arguments = itemVM.Target;
                                            bool result = process.Start();
                                        }
                                        */

                                        /*
                                        // Working best for third party control panel items
                                        // https://stackoverflow.com/questions/5255086/when-do-we-need-to-set-useshellexecute-to-true
                                        string shortcutFilePath = Path.Combine(Helper.GetShortcutsDirectoryPath(), itemVM.Guid + itemVM.Ext);
                                        using (Process process = new Process())
                                        {
                                            process.StartInfo.UseShellExecute = true;
                                            process.StartInfo.ErrorDialog = true;
                                            process.StartInfo.FileName = "explorer.exe";
                                            process.StartInfo.Arguments = shortcutFilePath;
                                            bool result = process.Start();
                                        }
                                        */

                                        string shortcutsPath = Path.Combine(Helper.GetShortcutsPath(), itemVM.Guid + itemVM.Ext);
                                        Process.Start(shortcutsPath);
                                    }
                                    catch (System.ComponentModel.Win32Exception ex)
                                    {
                                        string exceptionMessage = $"Error code: {ex.ErrorCode.ToString()} \n";
                                        exceptionMessage += $"{ex.Message}. \n";
                                        exceptionMessage += "Please make sure the shortcut target is correct!";
                                        MessageBox.Show(this, exceptionMessage, "Exception", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(this, ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                    }
                                }
                            }
                            clickedObjectHashCode = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }

        #endregion
        
        #region DOCK WINDOW

        private void dockWindowLeft_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WindowItemVM windowItemVM = (WindowItemVM)DataContext;
                System.Windows.Forms.Screen screen = Helper.GetScreenFromWindow(this);
                windowItemVM.Left = screen.WorkingArea.Left;
                windowItemVM.Top = screen.WorkingArea.Top;
                windowItemVM.Height = screen.WorkingArea.Height;
                SaveData.SaveShortcuts(false);
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void dockWindowRight_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WindowItemVM windowItemVM = (WindowItemVM)DataContext;
                System.Windows.Forms.Screen screen = Helper.GetScreenFromWindow(this);
                windowItemVM.Left = screen.WorkingArea.Left + screen.WorkingArea.Width - windowItemVM.Width;
                windowItemVM.Top = screen.WorkingArea.Top;
                windowItemVM.Height = screen.WorkingArea.Height;
                SaveData.SaveShortcuts(false);
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }

        #endregion
        
        #region CENTER WINDOW

        private void textBlockTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // Detect double clicks
                if (e.ClickCount == 2)
                {
                    // Position the window to the center of the screen
                    // Perhaps we do not want this behaviour!
                    CenterWindowToItsOwnScreen();
                }
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void CenterWindowToItsOwnScreen()
        {
            WindowItemVM windowItemVM = (WindowItemVM)DataContext;
            if (lastWindowWidth == 0 && lastWindowHeight == 0 && lastWindowLeft == 0 && lastWindowTop == 0)
            {
                // Store the last position
                lastWindowWidth = windowItemVM.Width;
                lastWindowHeight = windowItemVM.Height;
                lastWindowLeft = windowItemVM.Left;
                lastWindowTop = windowItemVM.Top;

                // Center
                System.Windows.Forms.Screen screen = Helper.GetScreenFromWindow(this);
                windowItemVM.Width = 400;
                windowItemVM.Height = 500;
                windowItemVM.Left = (screen.Bounds.Left + ((screen.Bounds.Width - windowItemVM.Width) / 2));
                windowItemVM.Top = (screen.Bounds.Top + ((screen.Bounds.Height - windowItemVM.Height) / 2));
            }
            else
            {
                // Restore to the last position
                windowItemVM.Width = lastWindowWidth;
                windowItemVM.Height = lastWindowHeight;
                windowItemVM.Left = lastWindowLeft;
                windowItemVM.Top = lastWindowTop;

                // Reset last position variables
                lastWindowWidth = 0;
                lastWindowHeight = 0;
                lastWindowLeft = 0;
                lastWindowTop = 0;
            }
            SaveData.SaveShortcuts(false);
        }

        #endregion
        
        #region THEMES

        private void menuItemDarkTheme_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WindowItemVM WindowItemVM = (WindowItemVM)DataContext;
                if (WindowItemVM != null)
                {
                    WindowInfoVM windowInfoVM = WindowItemVM.WindowInfoVM;
                    if (windowInfoVM != null)
                    {
                        Themes.DarkTheme(windowInfoVM);
                        SaveData.SaveShortcuts(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void menuItemLightTheme_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WindowItemVM WindowItemVM = (WindowItemVM)DataContext;
                if (WindowItemVM != null)
                {
                    WindowInfoVM windowInfoVM = WindowItemVM.WindowInfoVM;
                    if (windowInfoVM != null)
                    {
                        Themes.LightTheme(windowInfoVM);
                        SaveData.SaveShortcuts(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void menuItemMyTheme_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WindowItemVM WindowItemVM = (WindowItemVM)DataContext;
                if (WindowItemVM != null)
                {
                    WindowInfoVM windowInfoVM = WindowItemVM.WindowInfoVM;
                    if (windowInfoVM != null)
                    {
                        Themes.MyTheme(windowInfoVM);
                        SaveData.SaveShortcuts(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void menuItemCustomTheme_Click(object sender, RoutedEventArgs e)
        {
            CustomTheme();
        }
        private void ButtonCustomTheme_Click(object sender, RoutedEventArgs e)
        {
            CustomTheme();
        }
        private void CustomTheme()
        {
            try
            {
                WindowItemVM WindowItemVM = (WindowItemVM)DataContext;
                if (WindowItemVM != null)
                {
                    WindowInfoVM windowInfoVM = WindowItemVM.WindowInfoVM;
                    if (windowInfoVM != null)
                    {
                        CustomThemeWindow customThemeWindow = new CustomThemeWindow(windowInfoVM);
                        customThemeWindow.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }

        #endregion

        // TODO need to look
        private void TreeViewShortcuts_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {

            //if(e.ExtentWidthChange != 0)
            //{
            var scrollViewer = sender as ScrollViewer;

            //scrollViewer?.ScrollToLeftEnd();
            //scrollViewer?.ScrollToRightEnd();
            //scrollViewer?.ScrollToHorizontalOffset(0);
            //}
        }


    }

}


/*
try
{

}
catch (Exception ex)
{
    Error.ShowDialog(ex, this);
}
*/
