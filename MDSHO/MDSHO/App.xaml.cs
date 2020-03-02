using MDSHO.Helpers;
using MDSHO.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Forms = System.Windows.Forms;

// Creating a background application with WPF
// https://www.thomasclaudiushuber.com/2015/08/22/creating-a-background-application-with-wpf/

namespace MDSHO
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public AppViewModel AppContext { get; set; } = new AppViewModel();
        private Forms.NotifyIcon _notifyIcon;
        private bool _isExit;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                // Make sure the same application is not running.
                string processName = Process.GetCurrentProcess().ProcessName;
                Process[] processes = Process.GetProcessesByName(processName);
                if (processes.Length > 1)
                {
                    MessageBox.Show($"{processName} is already running.\nPlease check the taskbar.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    Current.Shutdown();
                }








                //StartLife();

                MainWindow = new MainWindow();
                MainWindow.Closing += MainWindow_Closing;
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

                _notifyIcon = new Forms.NotifyIcon();
                //_notifyIcon.DoubleClick += (s, args) => ShowAboutWindow();
                //_notifyIcon.Icon = QuickShortcuts.Properties.Resources.quickshortcuts_neg_nobg_o;
                _notifyIcon.Visible = true;

                CreateContextMenu();


                ShowAboutWindow();


                // TODO temp delete later
                SolidColorBrush windowBackground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString("Green") };
                double windowBackgroundOpacity = 1D;
                InfoViewModel infoViewModel = new InfoViewModel(windowBackground, windowBackgroundOpacity);
                BoxViewModel boxViewModel = new BoxViewModel(infoViewModel);
                AppContext.BoxViewModels.Add(boxViewModel);
                BoxWindow boxWindow = new BoxWindow(boxViewModel);
                boxWindow.Show();

            }
            catch (Exception ex)
            {
                Error.Show(ex);
            }
        }


        private void CreateContextMenu()
        {
            try
            {
                _notifyIcon.ContextMenuStrip = new Forms.ContextMenuStrip();
                //_notifyIcon.ContextMenuStrip.Items.Add("New window", Helper.GetImageFromImages("plus-o.png")).Click += (s, e) => NewWindow();
                //_notifyIcon.ContextMenuStrip.Items.Add("Restore shortcuts...", Helper.GetImageFromImages("database-o.png")).Click += (s, e) => ShowRestoreWindow();
                //_notifyIcon.ContextMenuStrip.Items.Add("About QuickShortcuts", Helper.GetImageFromImages("info-o.png")).Click += (s, e) => ShowAboutWindow();
                _notifyIcon.ContextMenuStrip.Items.Add("Exit application").Click += (s, e) => ExitApplication(false);
            }
            catch (Exception ex)
            {
                Error.Show(ex);
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
                Error.Show(ex);
            }
        }







        /// <summary>
        /// Exit the application.
        /// </summary>
        /// <param name="confirm">If false then do not disturb the user with confirmation questions.</param>
        public void ExitApplication(bool confirm)
        {
            try
            {
                if (confirm)
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to exit application?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        _notifyIcon.Dispose();
                        _notifyIcon = null;
                        _isExit = true;
                        MainWindow.Close();
                    }
                }
                else
                {
                        _notifyIcon.Dispose();
                        _notifyIcon = null;
                        _isExit = true;
                        MainWindow.Close();
                }
            }
            catch (Exception ex)
            {
                Error.Show(ex);
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
                Error.Show(ex);
            }
        }



    }
}
