using MDSHO.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using MDSHO.Models;
using MDSHO.Data;
using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
using MDSHO.Helpers;

// Creating a background application with WPF
// https://www.thomasclaudiushuber.com/2015/08/22/creating-a-background-application-with-wpf/

namespace MDSHO
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private System.Windows.Forms.NotifyIcon _notifyIcon;
        private bool _isExit;

        public ShortcutsVM DataContext { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {

                // Testing the MDSHO Error form
                // throw new DivideByZeroException();


                if (IsApplicationAlreadyRunning())
                {
                    MessageBox.Show("MDSHO is already running.\nPlease check the taskbar.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    Application.Current.Shutdown();
                }

                StartLife();

                MainWindow = new MainWindow();
                MainWindow.Closing += MainWindow_Closing;
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

                _notifyIcon = new System.Windows.Forms.NotifyIcon();
                _notifyIcon.DoubleClick += (s, args) => ShowAboutWindow();
                _notifyIcon.Icon = MDSHO.Properties.Resources.logo_white_o_16x16;
                _notifyIcon.Visible = true;

                CreateContextMenu();
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                if (!_isExit)
                {
                    e.Cancel = true;
                    // A hidden window can be shown again, a closed one not.
                    MainWindow.Hide();
                }
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }

        #region APPLICATION DATACONTEXT

        public void StartLife()
        {
            try
            {
                SetDataContext();              
                SetWindows();
                SetBackups();
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void SetDataContext()
        {
            try
            {
                Shortcuts shortcuts = null;
                if (IsFirstTimeRun())
                {
                    // This is the first time the application is runing.
                    shortcuts = NewData.NewShortcuts();
                    SaveData.SaveShortcuts(shortcuts, true);
                }
                else
                {
                    // This is not the first time the application is runing.
                    // Get the shortcuts data
                    shortcuts = ShortcutsData.GetShortcuts();

                    // If shortcuts is null then perhaps the sortcut data file is corrupted.
                    if (shortcuts == null)
                    {
                        MessageBoxResult messageBoxResult = MessageBox.Show("For some reason shortcuts are corrupted.\nReseting shortcuts.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        shortcuts = NewData.NewShortcuts();
                        SaveData.SaveShortcuts(shortcuts, true);
                    }
                }
                DataContext = ContextHelper.ShortcutsToVM(shortcuts);
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void SetWindows()
        {
            try
            {
                // Create windows
                if (DataContext.WindowItemVMs.Count == 0)
                {
                    // In case there is no window we just create one
                    NewWindow();
                }
                else
                {
                    // Loop through all window items
                    foreach (WindowItemVM windowItemVM in DataContext.WindowItemVMs)
                    {
                        // Create the shortcuts window
                        ShortcutsWindow shortcutsWindow = new ShortcutsWindow();
                        shortcutsWindow.DataContext = windowItemVM;
                        shortcutsWindow.Show();
                        // Create window clones if any
                        foreach (WindowItemVM windowItemVMClone in windowItemVM.WindowItemVMClones)
                        {
                            ShortcutsWindow shortcutsWindowClone = new ShortcutsWindow();
                            shortcutsWindowClone.DataContext = windowItemVMClone;
                            shortcutsWindowClone.Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void SetBackups()
        {
            try
            {
                // Clear old BackupVMs
                DataContext.BackupVMs = new ObservableCollection<BackupVM>();
                // Get backup file names
                DataContext.BackupVMs = BackupIO.GetBackupVMs();
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        public static bool IsFirstTimeRun()
        {
            // Check to see if this is the first time the application runs
            string shortcutsDataDirectoryPath = Helper.GetShortcutsDataPath();
            // If the shortcutsDataDirectoryPath is empty then this is the first time the application runs.
            return !Directory.EnumerateFileSystemEntries(shortcutsDataDirectoryPath).Any();
        }

        #endregion

        private void CreateContextMenu()
        {
            try
            {
                _notifyIcon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
                _notifyIcon.ContextMenuStrip.Items.Add("New window", Helper.GetImageFromImages("plus-o.png")).Click += (s, e) => NewWindow();
                _notifyIcon.ContextMenuStrip.Items.Add("Restore shortcuts...").Click += (s, e) => ShowRestoreWindow();
                _notifyIcon.ContextMenuStrip.Items.Add("About MDSHO").Click += (s, e) => ShowAboutWindow();
                _notifyIcon.ContextMenuStrip.Items.Add("Exit application").Click += (s, e) => ExitApplication(false);
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void NewWindow()
        {
            try
            {
                if (DataContext != null)
                {
                    System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.PrimaryScreen;
                    DataContext.NewWindow(screen);
                }
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        public void ShowMainWindow(Window parent)
        {
            try
            {
                MainWindow.CenterToParentScreen(parent);
                if (MainWindow.IsVisible)
                {
                    if (MainWindow.WindowState == WindowState.Minimized)
                    {
                        MainWindow.WindowState = WindowState.Normal;
                    }
                    MainWindow.Activate();
                }
                else
                {
                    MainWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        public void ShowRestoreWindow()
        {
            try
            {
                RestoreWindow restoreWindow = new RestoreWindow();
                restoreWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        public void ShowAboutWindow()
        {
            try
            {
                AboutWindow aboutWindow = new AboutWindow();
                aboutWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        /// <summary>
        /// Exit the application.
        /// </summary>
        /// <param name="parent">The window from where the call is comming.</param>
        /// <param name="silent">If true then do not disturb the user with questions.</param>
        public void ExitApplication(bool silent)
        {
            try
            {
                if(silent)
                {
                    ExitApplication();
                }
                else
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to exit MDSHO?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);            
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        ExitApplication();
                    }
                }
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void ExitApplication()
        {
            _notifyIcon.Dispose();
            _notifyIcon = null;
            _isExit = true;
            MainWindow.Close();
        }
        /// <summary>
        /// Check and see if the application ia already running.
        /// </summary>
        /// <returns>Return true if the application is already running otherwise false.</returns>
        private bool IsApplicationAlreadyRunning()
        {
            string processName = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(processName);
            if (processes.Length > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        /*
        // Make sure to put SessionEnding="Application_SessionEnding" in App.xaml Application
        /// <summary>
        /// Fired when the OS is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_SessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            // Due to some corrupt shortcuts data when save was done during the OS shut down
            // We set SAVE_REQUEST_FREQUENCY = 1 so this is not nesesary enymore.
            // SaveData.SaveShortcuts(true);
        }
        */
    }

}

/*
try
{

}
catch (Exception ex)
{
    Error.ShowDialog(ex, null);
}
*/