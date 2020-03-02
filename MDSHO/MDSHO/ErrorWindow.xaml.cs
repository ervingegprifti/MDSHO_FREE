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
        public ErrorWindow(Exception ex)
        {
            InitializeComponent();

            try
            {
                StringBuilder errorMessage = new StringBuilder();
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
            this.Close();
        }
    }
}
