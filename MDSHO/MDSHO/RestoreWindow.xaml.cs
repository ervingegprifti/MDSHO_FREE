using System;
using System.Windows;
using MDSHO.Helpers;
using MDSHO.ViewModels;


namespace MDSHO
{
    /// <summary>
    /// Interaction logic for RestoreWindow.xaml
    /// </summary>
    public partial class RestoreWindow : Window
    {
        public RestoreWindow()
        {
            InitializeComponent();

            try
            {
                ShortcutsVM shortcutsVM = ((App)Application.Current).DataContext;
                ListViewBackup.ItemsSource = shortcutsVM.BackupVMs;
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
    }
}
