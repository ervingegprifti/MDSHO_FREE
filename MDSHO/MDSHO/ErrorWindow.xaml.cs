using MDSHO.Helpers;
using System;
using System.Text;
using System.Windows;

namespace MDSHO
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        public ErrorWindow(Exception ex, string title = null)
        {
            InitializeComponent();

            try
            {
                Title = title;
                StringBuilder errorMessage = new StringBuilder();
                errorMessage.AppendLine($"Unhandled exception in {Helper.GetApplicationName()} version {Helper.GetApplicationVersion()}");
                errorMessage.AppendLine(ex.Message);
                errorMessage.AppendLine(ex.StackTrace);
                TextBoxErrorMessage.Text = errorMessage.ToString();
            }
            catch (Exception inex)
            {
                MessageBox.Show(inex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonDismiss_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
