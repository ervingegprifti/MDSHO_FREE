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
        public AppViewModel AppViewModel { get; set; } = new AppViewModel();
        private Forms.NotifyIcon notifyIcon = new Forms.NotifyIcon();
        private bool mustExit;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                // Make sure the same application is not running.
                string processName = Process.GetCurrentProcess().ProcessName;
                Process[] processes = Process.GetProcessesByName(processName);
                // If the application is already running do not start another one.
                if (processes.Length > 1)
                {
                    MessageBox.Show($"{processName} is already running.\nPlease check the taskbar.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    Current.Shutdown();
                }


                //StartLife();

                MainWindow = new MainWindow();
                MainWindow.Closing += MainWindow_Closing;
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

                // notifyIcon.DoubleClick += (s, args) => ShowAboutWindow();
                notifyIcon.Icon = MDSHO.Properties.Resources.logo_white_16x16;
                notifyIcon.Visible = true;

                // Create the context menu strip
                notifyIcon.ContextMenuStrip = new Forms.ContextMenuStrip();
                //_notifyIcon.ContextMenuStrip.Items.Add("New window", Helper.GetImageFromImages("plus-o.png")).Click += (s, e) => NewWindow();
                //_notifyIcon.ContextMenuStrip.Items.Add("Restore shortcuts...", Helper.GetImageFromImages("database-o.png")).Click += (s, e) => ShowRestoreWindow();
                //_notifyIcon.ContextMenuStrip.Items.Add("About QuickShortcuts", Helper.GetImageFromImages("info-o.png")).Click += (s, e) => ShowAboutWindow();
                notifyIcon.ContextMenuStrip.Items.Add("Exit application").Click += (s, e) => ExitApplication(confirm: false);





                // TODO temp delete later
                SolidColorBrush windowBackground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString("Green") };
                double windowBackgroundOpacity = 1D;
                InfoViewModel infoViewModel = new InfoViewModel(windowBackground, windowBackgroundOpacity);
                BoxViewModel boxViewModel = new BoxViewModel(infoViewModel);
                AppViewModel.BoxViewModels.Add(boxViewModel);
                BoxWindow boxWindow = new BoxWindow(boxViewModel);
                boxWindow.Show();

            }
            catch (Exception ex)
            {
                Error.Show(ex);
            }
        }




        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!mustExit)
            {
                e.Cancel = true;
                // A hidden window can be shown again, a closed one not.
                MainWindow.Hide();
            }
        }







        /// <summary>
        /// Exit the application.
        /// </summary>
        /// <param name="confirm">If false then do not disturb the user with confirmation questions.</param>
        public void ExitApplication(bool confirm)
        {
            if (confirm)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to exit application?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    notifyIcon.Dispose();
                    notifyIcon = null;
                    mustExit = true;
                    MainWindow.Close();
                }
            }
            else
            {
                    notifyIcon.Dispose();
                    notifyIcon = null;
                    mustExit = true;
                    MainWindow.Close();
            }
        }






    }
}
