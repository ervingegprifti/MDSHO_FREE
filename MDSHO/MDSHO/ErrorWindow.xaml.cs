using MDSHO.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
                string errorMessage = "";
                errorMessage += ex.Message;
                errorMessage += Environment.NewLine + Environment.NewLine;
                errorMessage += ex.StackTrace;
                TextBoxErrorMessage.Text = errorMessage;
                // TextBlockErrorMessage.Text = errorMessage;
            }
            catch (Exception inex)
            {
                MessageBox.Show(inex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
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
