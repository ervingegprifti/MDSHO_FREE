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
        public AppVM AppVM { get; set; } = new AppVM();
        private Forms.NotifyIcon notifyIcon = new Forms.NotifyIcon();
        private bool mustExit;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SetupGlobalExceptionHandling();

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
                SolidColorBrush boxBg = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString("Green") };
                double boxBgOpacity = 1D;
                InfoVM infoVM = new InfoVM(boxBg, boxBgOpacity);
                BoxVM boxVM = new BoxVM(infoVM);
                AppVM.BoxVMs.Add(boxVM);
                BoxWindow boxWindow = new BoxWindow(boxVM);
                boxWindow.Show();

        }



        /// <summary>
        /// Used to globally catch unhalted exceptions. <br />
        /// https://stackoverflow.com/questions/793100/globally-catch-exceptions-in-a-wpf-application <br />
        /// https://stackoverflow.com/questions/1472498/wpf-global-exception-handler/1472562#1472562
        /// </summary>
        private void SetupGlobalExceptionHandling()
        {
            // AppDomain.CurrentDomain.UnhandledException. From all threads in the AppDomain.
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                Error.Show((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");
            };

            // Application.Current.Dispatcher.UnhandledException. From a single specific UI dispatcher thread.
            Dispatcher.UnhandledException += (s, e) =>
            {
                Error.Show(e.Exception, "Application.Current.Dispatcher.UnhandledException");
                e.Handled = true;
            };

            // Application.Current.DispatcherUnhandledException. From the main UI dispatcher thread in your WPF application.
            DispatcherUnhandledException += (s, e) =>
            {
                Error.Show(e.Exception, "Application.Current.DispatcherUnhandledException");
                e.Handled = true;
            };

            // TaskScheduler.UnobservedTaskException. From within each AppDomain that uses a task scheduler for asynchronous operations.
            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                Error.Show(e.Exception, "TaskScheduler.UnobservedTaskException");
                e.SetObserved();
            };
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
